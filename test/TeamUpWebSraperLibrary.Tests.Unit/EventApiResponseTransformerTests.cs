using FluentAssertions;
using System.Reflection;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;
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
		var _sut = new EventApiResponseTransformer();
		var config = TestsHelper.ReadConfigIntoModel<TeamUpApiConfiguration>(@"EventApiResponseTransformerTestFiles\TestsConfig.json", AppsettingsConstants.CONFIG_SECTION_NAME_TEAMUP_API)!;

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
		var actual = _sut.EventApiResponseToSpreadSheetLines(input, config.Calendars);

		// Assert
		await Verify(actual, _verifySettings)
				.DontScrubDateTimes();
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
		var _sut = new EventApiResponseTransformer();
		var config = TestsHelper.ReadConfigIntoModel<TeamUpApiConfiguration>(@"EventApiResponseTransformerTestFiles\TestsConfig.json", AppsettingsConstants.CONFIG_SECTION_NAME_TEAMUP_API)!;
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

		// Private Method info obtained using REFLEXION
		MethodInfo privateMethodGetEventId = typeof(EventApiResponseTransformer)
			.GetMethod("GetEventId", BindingFlags.NonPublic | BindingFlags.Instance)!;

		object[] methodParameters = new object[2] { eventData, config.Calendars };


		// Act
		var actual = (string)privateMethodGetEventId.Invoke(_sut, methodParameters)!;


		// Assert
		actual.Should().Be(expected);
	}
}
