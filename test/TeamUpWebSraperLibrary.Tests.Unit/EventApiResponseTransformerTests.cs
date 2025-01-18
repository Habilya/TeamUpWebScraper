using NSubstitute;
using Shouldly;
using System.Reflection;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;
using TeamUpWebScraperLibrary.Transformers;

namespace TeamUpWebSraperLibrary.Tests.Unit;

public class EventApiResponseTransformerTests
{
	private readonly VerifySettings _verifySettings;

	public EventApiResponseTransformerTests()
	{
		_verifySettings = new VerifySettings();
		_verifySettings.UseDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EventApiResponseTransformerTestFiles"));
	}

	[Fact]
	public async Task EventApiResponseToSpreadSheetLines_ShouldReturnMatchingList_WhenInputValid()
	{
		// Arrange
		var excelReportSpreadSheetConfig = Substitute.For<ExcelReportSpreadSheetConfig>();
		var _sut = new EventApiResponseTransformer(excelReportSpreadSheetConfig);
		var subCalendars = TestsHelper.ReadSubCalendarsFromJSON();

		var input = new List<Event>
		{
			new Event
			{
				Id = "1759667815",
				Title = "Exercice sauvetage forestier",
				Location = "Boisé Papineau, dans le stationnement derrière le CC 3235, boulevard Saint-Martin Est\nLaval (Québec)  H7E 5G8",
				StartDate = new DateTimeOffset(2024, 12, 01, 09, 00, 00, new TimeSpan(-5, 0, 0)),
				EndDate = new DateTimeOffset(2024, 12, 01, 12, 00, 00, new TimeSpan(-5, 0, 0)),
				CreationDate = new DateTimeOffset(2024, 10, 13, 21, 18, 47, new TimeSpan(-4, 0, 0)),
				UpdateDate = new DateTimeOffset(2024, 12, 01, 06, 28, 41, new TimeSpan(-5, 0, 0)),
				SubcalendarId = 9634218L,
				SubcalendarIds = new List<long>{ 9634218L },
				Notes = "<p>Exercice de sauvetage en milieu forestier</p>",
				Custom = new Custom
				{
					ContratProvincialContract = new List<string> { "non_no" }
				},
				SignupCount = 3,
				Signups = new List<Signup>
				{
					new Signup
					{
						Id = 3830570L,
						Name = "Benoit Vachon PR971"
					}, // 0
					new Signup
					{
						Id = 3830397L,
						Name = "Giuliana Rotella"
					}, // 1
					new Signup
					{
						Id = 3830089L,
						Name = "Michael Benigno"
					}, // 2
				}
			}, // 0
			new Event
			{
				Id = "1776293830",
				Title = "Fan Club et Club 1909",
				Location = "1225 st antoine",
				StartDate = new DateTimeOffset(2024, 12, 01, 14, 00, 00, new TimeSpan(-5, 0, 0)),
				EndDate = new DateTimeOffset(2024, 12, 01, 19, 30, 00, new TimeSpan(-5, 0, 0)),
				CreationDate = new DateTimeOffset(2024, 11, 12, 15, 53, 57, new TimeSpan(-5, 0, 0)),
				UpdateDate = new DateTimeOffset(2024, 11, 30, 20, 04, 57, new TimeSpan(-5, 0, 0)),
				SubcalendarId = 9616459L,
				SubcalendarIds = new List<long>{ 9616459L },
				Notes = "<p>Date : 1 décembre 2024</p>",
				Custom = new Custom
				{
					Division = "452",
					Client2 = "centre bell",
					ContratProvincialContract = new List<string> { "non_no" },
					NombreDeMembresNecessaires = "4"
				},
				SignupCount = 2,
				Signups = new List<Signup>
				{
					new Signup
					{
						Id = 3830388L,
						Name = "Pascal Pedneault (PR) 1002"
					},
					new Signup
					{
						Id = 3830390L,
						Name = "Charles-Etienne Pedneault (PR) 1002"
					}
				}
			}, // 1
			new Event
			{
				Id = "1776293831",
				Title = "Some Other Event",
				Location = "1225 st antoine",
				StartDate = new DateTimeOffset(2024, 12, 01, 14, 00, 00, new TimeSpan(-5, 0, 0)),
				EndDate = new DateTimeOffset(2024, 12, 01, 19, 30, 00, new TimeSpan(-5, 0, 0)),
				CreationDate = new DateTimeOffset(2024, 11, 12, 15, 53, 57, new TimeSpan(-5, 0, 0)),
				UpdateDate = new DateTimeOffset(2024, 11, 30, 20, 04, 57, new TimeSpan(-5, 0, 0)),
				SubcalendarId = 9616459L,
				SubcalendarIds = new List<long>{ 9616459L },
				Notes = "<p>Date : 1 décembre 2024</p>",
				Custom = new Custom
				{
					Division = "452",
					Client2 = "centre bell",
					ContratProvincialContract = new List<string> { "non_no" },
					NombreDeMembresNecessaires = "4"
				},
				SignupCount = 0,
				Signups = new List<Signup>
				{

				}
			}  // 2 (should not be considered as SignupCount = 0)
		};

		// Act
		var actual = _sut.EventApiResponseToSpreadSheetLines(input, subCalendars);

		// Assert
		await Verify(actual, _verifySettings).DontScrubDateTimes();
	}

	[Theory]
	[InlineData(1, "DIV-241201-0900-0971")]
	[InlineData(2, "CB-241201-1400-0452")]
	[InlineData(3, "PB-241023-1830-0971")]
	[InlineData(4, "DIV-241025-1215-0452")]
	[InlineData(5, "DIV-241111-0900-0452")]
	[InlineData(6, "DIV-240924-0700-Prov")]
	[InlineData(7, "DIV-240525-1100-Prov")]
	[InlineData(8, "DIV-241002-1330-Gard")]
	public void GetEventId_ShouldReturnMatching_WhenInputValid(int dataId, string expected)
	{
		// Arrange
		var excelReportSpreadSheetConfig = Substitute.For<ExcelReportSpreadSheetConfig>();
		var _sut = new EventApiResponseTransformer(excelReportSpreadSheetConfig);
		var dataMap = new Dictionary<int, Event>
		{
			{1, new Event{
				Title = "Exercice sauvetage forestier",
				StartDate = new DateTimeOffset(2024, 12, 01, 09, 00, 00, new TimeSpan(-5, 0, 0)),
				SubcalendarId = 9634218L,
				SubcalendarIds = new List<long>{ 9634218L },
				Custom = new Custom
				{
					ContratProvincialContract = new List<string> { "non_no" }
				}
			}}, // DIV-241201-0900-0971
			{2, new Event{
				Title = "Fan Club et Club 1909",
				StartDate = new DateTimeOffset(2024, 12, 01, 14, 00, 00, new TimeSpan(-5, 0, 0)),
				SubcalendarId = 9616459L,
				SubcalendarIds = new List<long>{ 9616459L },
				Custom = new Custom
				{
					Division = "452",
					Client2 = "centre bell",
					ContratProvincialContract = new List<string> { "non_no" },
				}
			}}, // CB-241201-1400-0452
			{3, new Event{
				Title = "PB Christian Nodal",
				StartDate = new DateTimeOffset(2024, 10, 23, 18, 30, 00, new TimeSpan(-5, 0, 0)),
				SubcalendarId = 9634218L,
				SubcalendarIds = new List<long>{ 9634218L },
				Custom = new Custom
				{
					ContratProvincialContract = new List<string> { "non_no" },
				}
			}}, // PB-241023-1830-0971
			{4, new Event{
				Title = "OP SANG LIMITES (Montréal)",
				StartDate = new DateTimeOffset(2024, 10, 25, 12, 15, 00, new TimeSpan(-5, 0, 0)),
				SubcalendarId = 10065801L,
				SubcalendarIds = new List<long>{ 10065801L, 11159835L },
				Custom = new Custom
				{
					Division = "D452",
					ContratProvincialContract = new List<string> { "non_no" },
				}
			}}, // DIV-241025-1215-0452
			{5, new Event{
				Title = "Parade du Jour du Souvenir à Montréal",
				StartDate = new DateTimeOffset(2024, 11, 11, 09, 00, 00, new TimeSpan(-5, 0, 0)),
				SubcalendarId = 13329555L,
				SubcalendarIds = new List<long>{ 13329555L, 11159835L },
				Custom = new Custom
				{
					Division = "Garde d'Honneur",
					ContratProvincialContract = new List<string> { "non_no" },
				}
			}}, // DIV-241111-0900-0452
			{6, new Event{
				Title = "Tournoi PGA",
				StartDate = new DateTimeOffset(2024, 09, 24, 07, 00, 00, new TimeSpan(-5, 0, 0)),
				SubcalendarId = 10065801L,
				SubcalendarIds = new List<long>{ 10065801L },
				Custom = new Custom
				{
					ContratProvincialContract = new List<string> { "non_no" },
				}
			}}, // DIV-240924-0700-Prov
			{7, new Event{
				Title = "Fuego Fuego",
				StartDate = new DateTimeOffset(2024, 05, 25, 11, 00, 00, new TimeSpan(-5, 0, 0)),
				SubcalendarId = 10065801L,
				SubcalendarIds = new List<long>{ 10065801L },
				Custom = new Custom
				{
					Client2 = "Evenko",
					Division = "Provincial",
					ContratProvincialContract = new List<string> { "non_no" },
				}
			}}, // DIV-240525-1100-Prov
			{8, new Event{
				Title = "Garde d'Honneur - Funérailles",
				StartDate = new DateTimeOffset(2024, 10, 02, 13, 30, 00, new TimeSpan(-5, 0, 0)),
				SubcalendarId = 13329555L,
				SubcalendarIds = new List<long>{ 13329555L },
				Custom = new Custom
				{
					Division = "Garde d'Honneur",
					ContratProvincialContract = new List<string> { "non_no" },
				}
			}}, // DIV-241002-1330-Gard
		};

		var eventData = dataMap[dataId];

		var subCalendars = TestsHelper.ReadSubCalendarsFromJSON();

		// Private Method info obtained using REFLEXION
		MethodInfo privateMethodGetEventId = typeof(EventApiResponseTransformer)
			.GetMethod("GetEventId", BindingFlags.NonPublic | BindingFlags.Instance)!;

		object[] methodParameters = new object[2] { eventData, subCalendars };


		// Act
		var actual = (string)privateMethodGetEventId.Invoke(_sut, methodParameters)!;


		// Assert
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(1, "Fierté Québec (ancienne fête arc-en-ciel)", false)]
	[InlineData(2, "Annulé PB Arcangel & De La Ghetto", true)]
	[InlineData(2, "Rencontre divisionnaire mensuelle", false)]
	[InlineData(3, "ANNULÉ PB Grupo Frontera", true)]
	[InlineData(4, "PB Annulé Davido", true)]
	[InlineData(5, "Concert d’orgue", false)]
	[InlineData(6, "Annulé Préparation sécurité civile", true)]
	[InlineData(7, "REPORTÉ PB Keshi avec Mac Ayres, Starfall", true)]
	[InlineData(8, "PB Reporté juin 2025 Tournée du Trio- Ferrari, Tsamère, Lecaplain.", true)]
	[InlineData(9, "Reporté au 7 septembre 2024 PB Davido", true)]
	[InlineData(10, "Annulé Course St-Maxime", true)]
	[InlineData(11, "ANNULÉ nouvelle date à déterminer SimulationsAnnulé PB PWHL Séries", true)]
	[InlineData(12, "Annulé PB PWHL Séries", true)]
	[InlineData(13, "annulé PESO PLUMA 10PR/8SG", true)]
	[InlineData(14, "ANNULE - Festival Fierté Montréal", true)]
	[InlineData(15, "Festival international du rire ComediHa! (Québec) (annulé)", true)]
	[InlineData(16, "Cancelled event1", true)]
	[InlineData(17, "Anulé event2", true)]
	[InlineData(18, "Reporte event3", true)]
	[InlineData(19, "cancel event4", true)]
	[InlineData(20, "annui", false)]
	[InlineData(21, "", false)]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "UnitTests with testId")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "UnitTests with testId")]
	public void GetLineHighLightColor_ShouldMatchExpected(int id, string eventTitle, bool expected)
	{
		// Arrange
		var thePattern = "cancel|annul[é|e]|report[é|e]|report|anul[é|e]";
		#region arrangement
		var excelReportSpreadSheetConfig = Substitute.For<ExcelReportSpreadSheetConfig>();
		excelReportSpreadSheetConfig.EventTitlesToHighLightPattern = thePattern;
		excelReportSpreadSheetConfig.ReportAttentionRequiredHighlightingColorHtml = "[NotImportantJustNeedsAValue]";

		var _sut = new EventApiResponseTransformer(excelReportSpreadSheetConfig);

		var subCalendars = Substitute.For<List<Subcalendar>>();
		var eventData = Substitute.For<Event>();
		eventData.Title = eventTitle;

		// Private Method info obtained using REFLEXION
		MethodInfo privateMethodGetEventId = typeof(EventApiResponseTransformer)
			.GetMethod("GetLineHighLightColor", BindingFlags.NonPublic | BindingFlags.Instance)!;

		object[] methodParameters = new object[2] { eventData, subCalendars };
		#endregion


		// Act
		var highlitColor = (string)privateMethodGetEventId.Invoke(_sut, methodParameters)!;
		// At this point I don't care about the value of highlight, I just want to see if it is highlighted or not
		var actual = highlitColor is null
			? false
			: highlitColor.Equals(excelReportSpreadSheetConfig.ReportAttentionRequiredHighlightingColorHtml);


		// Assert
		actual.ShouldBe(expected);
	}
}
