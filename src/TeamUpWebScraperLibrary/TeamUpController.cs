using ErrorOr;
using Microsoft.Extensions.Caching.Hybrid;
using System.Diagnostics;
using TeamUpWebScraperLibrary.DisplayGridView;
using TeamUpWebScraperLibrary.DTO;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.Helpers;
using TeamUpWebScraperLibrary.Logging;
using TeamUpWebScraperLibrary.Providers;
using TeamUpWebScraperLibrary.TeamUpAPI;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;
using TeamUpWebScraperLibrary.Transformers;
using TeamUpWebScraperLibrary.Validators;

namespace TeamUpWebScraperLibrary;

public class TeamUpController
{
	private readonly ILoggerAdapter<TeamUpController> _logger;
	private readonly InputValidation _inputValidation;
	private readonly ITeamUpAPIService _teamUpAPIService;
	private readonly HybridCache _hybridCache;
	private readonly IEventApiResponseTransformer _eventApiResponseTransformer;
	private readonly IExcelSpreadsheetReportProvider _excelSpreadsheetReportProvider;
	private readonly IDisplayGridViewProvider _displayGridViewProvider;

	private List<EventSpreadSheetLine> ReportSpreadsheetLines { get; set; } = default!;

	public TeamUpController(ILoggerAdapter<TeamUpController> logger,
		InputValidation inputValidation,
		HybridCache hybridCache,
		ITeamUpAPIService teamUpAPIService,
		IEventApiResponseTransformer eventApiResponseTransformer,
		IExcelSpreadsheetReportProvider excelSpreadsheetReportProvider,
		IDisplayGridViewProvider displayGridViewProvider)
	{
		_logger = logger;
		_inputValidation = inputValidation;
		_hybridCache = hybridCache;
		_teamUpAPIService = teamUpAPIService;
		_eventApiResponseTransformer = eventApiResponseTransformer;
		_excelSpreadsheetReportProvider = excelSpreadsheetReportProvider;
		_displayGridViewProvider = displayGridViewProvider;
	}

	public TeamUpViewModel IsValidInputValues(InputViewModel inputValues)
	{
		var inputValidationResults = _inputValidation.Validate(inputValues);

		if (inputValidationResults.IsValid)
		{
			return new(true, "", "");
		}
		else
		{
			var message = string.Join("\n", inputValidationResults.Errors.Select(q => q.ErrorMessage));
			return new(false, "Validation Warning", message);
		}
	}

	public async Task<TeamUpViewModel> CallTeamUpAPI(InputViewModel inputValues)
	{
		ReportSpreadsheetLines = default!;
		try
		{
			var subCalendarsRouteResponse = await GetSubcalendarsFromTeamUpAPI();
			if (subCalendarsRouteResponse.IsError)
			{
				return new(false, subCalendarsRouteResponse.FirstError.Code, CombineErrorMessages(subCalendarsRouteResponse));
			}

			var subCalendarsList = subCalendarsRouteResponse.Value.Subcalendars;
			if (subCalendarsList is null || !subCalendarsList.Any())
			{
				return new(false, "SubCalendars List Empty", "For some reason the event SubCalendars list is empty...");
			}

			var eventsRouteResponse = await GetEventsFromTeamUpAPI(inputValues);
			if (eventsRouteResponse.IsError)
			{
				return new(false, eventsRouteResponse.FirstError.Code, CombineErrorMessages(eventsRouteResponse));
			}

			var eventsList = eventsRouteResponse.Value.Events;
			if (eventsList is null || !eventsList.Any())
			{
				return new(false, "Events List Empty", "For some reason the events list is empty...");
			}

			ReportSpreadsheetLines = _eventApiResponseTransformer.EventApiResponseToSpreadSheetLines(eventsList, subCalendarsList);


			if (ReportSpreadsheetLines is null || !ReportSpreadsheetLines.Any())
			{
				return new(false, "Events List Empty", "For some reason the Transformed list that goes in Excel is empty...");
			}
			else
			{
				return new(true, "", "");
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Demystify(), "CallTeamUpAPI threw an unhandled exception.");
			throw;
		}
	}

	private async Task<ErrorOr<EventResponse>> GetEventsFromTeamUpAPI(InputViewModel inputValues)
	{
		// At this point, Assuming the input Values have been validated
		var dateFrom = (DateTime)inputValues.DateFrom!;
		var dateTo = (DateTime)inputValues.DateTo!;

		var cacheKey = $"Events{dateFrom.ToString()}-{dateTo.ToString()}";

		return await _hybridCache.GetOrCreateAsync(
			cacheKey,
			async _ => await _teamUpAPIService.GetEventsAsync(dateFrom, dateTo));
	}

	private async Task<ErrorOr<SubcalendarResponse>> GetSubcalendarsFromTeamUpAPI()
	{
		var cacheKey = "subcalendars";

		return await _hybridCache.GetOrCreateAsync(
				cacheKey,
				async _ => await _teamUpAPIService.GetSubcalendarsAsync());
	}

	public List<DisplayGridViewModel> GetDisplayableGridResults()
	{
		return _displayGridViewProvider.TransformReportSpreadsheetLinesInotDisplayLines(ReportSpreadsheetLines);
	}

	private string CombineErrorMessages<T>(ErrorOr<T> errorableResponse)
	{
		var errorMessages = string.Join('\n', errorableResponse.Errors.Select(q => q.Description));
		return errorMessages;
	}

	public (string defaultSavePath, string fileName, string filter) GetSaveDialogOptions()
	{
		return (
			_excelSpreadsheetReportProvider.ExcelReportDefaultSavePath,
			_excelSpreadsheetReportProvider.ExcelReportFileName,
			_excelSpreadsheetReportProvider.ExcelReportSaveDialogFilter
		);
	}

	public void SaveXLSX(string saveFullPath, List<int> selectedIds, bool isMemberTimeAnalysisIncluded)
	{
		try
		{
			var filteredReportLines = ReportSpreadsheetLines
				.Where(q => selectedIds.Contains(q.UniqueId))
				.ToList();

			var memberTimeAnalysisData = new List<MemberTimeAnalysisModel>();
			var memberTimeReportData = new List<MemberTimeReportModel>();
			if (isMemberTimeAnalysisIncluded)
			{
				var memberTimeAnalysisRawData = new List<MemberTimeAnalysisModel>();
				foreach (var line in filteredReportLines)
				{
					foreach (var signup in line.Signups)
					{
						memberTimeAnalysisRawData.Add(new MemberTimeAnalysisModel
						{
							SignupName = signup,
							EventName = line.Title,
							StartDate = line.StartDate,
							Hours = line.Hours,
							EndDate = line.EndDate,
							HoursPlus2 = line.Hours + 2
						});
					}
				}

				var similarMemberGroups = ClusterSimilarMembers(memberTimeAnalysisRawData);
				// Flatten groups
				memberTimeAnalysisData = similarMemberGroups
					.SelectMany(g => g.Members.Select(m => new MemberTimeAnalysisModel
					{
						SignupName = m.SignupName,
						Hours = m.Hours,
						HoursPlus2 = m.HoursPlus2,
						EventName = m.EventName,
						StartDate = m.StartDate,
						EndDate = m.EndDate,
					}))
					.ToList();

				memberTimeReportData = similarMemberGroups
					.Select(g =>
					{
						var firstMember = g.Members.First(); // pick a representative
						return new MemberTimeReportModel
						{
							SignupName = firstMember.SignupName, // or you could combine names
							Hours = g.Members.Sum(x => x.Hours),
							HoursPlus2 = g.Members.Sum(x => x.HoursPlus2),
							NBEvents = g.Members.Count,
							OtherNameOccurances = string.Join(", ",
								g.Members
								 .Skip(1) // exclude the representative
								 .Select(x => x.SignupName))
						};
					})
					.ToList();
			}

			_excelSpreadsheetReportProvider.SaveExcelReport(
					saveFullPath,
					filteredReportLines,
					isMemberTimeAnalysisIncluded,
					memberTimeAnalysisData,
					memberTimeReportData);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Demystify(), "SaveXLSX threw an unhandled exception.");
			throw;
		}
	}

	public static List<SimilarMemberNameGroup> ClusterSimilarMembers(
		List<MemberTimeAnalysisModel> members,
		double threshold = 0.92) // raise threshold for safety
	{
		var groups = new List<SimilarMemberNameGroup>();
		var used = new HashSet<MemberTimeAnalysisModel>();

		var nameSimilarityCalculator = new NameSimilarityCalculator();

		foreach (var member in members)
		{
			if (used.Contains(member)) continue;

			var group = new SimilarMemberNameGroup();
			group.Members.Add(member);
			used.Add(member);

			foreach (var other in members)
			{
				if (used.Contains(other) || other == member) continue;

				// Optionally: check last name if member entered more than one word
				var memberLastName = StringHelper.GetLastName(member.SignupName);
				var otherLastName = StringHelper.GetLastName(other.SignupName);

				// Only group if last names match or one of them is missing a last name
				if (!string.IsNullOrEmpty(memberLastName) && !string.IsNullOrEmpty(otherLastName))
				{
					if (memberLastName != otherLastName) continue; // different people, skip
				}

				var score = nameSimilarityCalculator.Compute(member.SignupName, other.SignupName);

				if (score >= threshold)
				{
					group.Members.Add(other);
					group.Similarities.Add(new NameSimilarity
					{
						Name1 = member.SignupName,
						Name2 = other.SignupName,
						Score = score
					});
					used.Add(other);
				}
			}

			groups.Add(group); // add the group even if it has only one member
		}

		return groups;
	}
}
