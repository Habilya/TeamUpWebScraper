using ErrorOr;
using System.Diagnostics;
using TeamUpWebScraperLibrary.DisplayGridView;
using TeamUpWebScraperLibrary.DTO;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.Logging;
using TeamUpWebScraperLibrary.TeamUpAPI;
using TeamUpWebScraperLibrary.Transformers;
using TeamUpWebScraperLibrary.Validators;

namespace TeamUpWebScraperLibrary;

public class TeamUpController
{
	private readonly ILoggerAdapter<TeamUpController> _logger;
	private readonly InputValidation _inputValidation;
	private readonly ITeamUpAPIService _teamUpAPIService;
	private readonly IEventApiResponseTransformer _eventApiResponseTransformer;
	private readonly IExcelSpreadsheetReportProvider _excelSpreadsheetReportProvider;
	private readonly IDisplayGridViewProvider _displayGridViewProvider;

	private List<EventSpreadSheetLine> ReportSpreadsheetLines { get; set; } = default!;

	public TeamUpController(ILoggerAdapter<TeamUpController> logger,
		InputValidation inputValidation,
		ITeamUpAPIService teamUpAPIService,
		IEventApiResponseTransformer eventApiResponseTransformer,
		IExcelSpreadsheetReportProvider excelSpreadsheetReportProvider,
		IDisplayGridViewProvider displayGridViewProvider)
	{
		_logger = logger;
		_inputValidation = inputValidation;
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
			var subCalendarsRouteResponse = await _teamUpAPIService.GetSubcalendarsAsync();
			if (subCalendarsRouteResponse.IsError)
			{
				return new(false, subCalendarsRouteResponse.FirstError.Code, CombineErrorMessages(subCalendarsRouteResponse));
			}

			var subCalendarsList = subCalendarsRouteResponse.Value.Subcalendars;
			if (subCalendarsList is null || !subCalendarsList.Any())
			{
				return new(false, "SubCalendars List Empty", "For some reason the event SubCalendars list is empty...");
			}

			// At this point, Assuming the input Values have been validated
			var eventsRouteResponse = await _teamUpAPIService.GetEventsAsync((DateTime)inputValues.DateFrom!, (DateTime)inputValues.DateTo!);
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

	public void SaveXLSX(string saveFullPath)
	{
		try
		{
			_excelSpreadsheetReportProvider.SaveExcelReport(saveFullPath, ReportSpreadsheetLines);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Demystify(), "SaveXLSX threw an unhandled exception.");
			throw;
		}
	}
}
