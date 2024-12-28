using FluentAssertions;
using System.Text.Json;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;
using TeamUpWebScraperLibrary.Transformers;

namespace TeamUpWebSraperLibrary.Tests.Unit;

public class EventApiResponseTransformerTests
{
	[Fact]
	public void EventApiResponseToSpreadSheetLines_ShouldReturnMatchingList_WhenInputValid()
	{
		// Arrange
		var config = ReadConfigIntoModel();

		var expected = new List<EventSpreadSheetLine>
		{
			new EventSpreadSheetLine
			{
				Id = "DIV-241201-0900-0971",
				Title = "Exercice sauvetage forestier",
			}, // 0
			new EventSpreadSheetLine
			{
				Id = "CB-241201-1400-0452",
				Title = "Fan Club et Club 1909",
			} // 1
		};


		var input = new List<Event>
		{
			new Event
				{
					Id = "1759667815",
					Title = "Exercice sauvetage forestier",
					StartDate =  new DateTime(2024, 12, 01, 09, 00, 00),
					EndDate = new DateTime(2024, 12, 01, 12, 00, 00),
					SubcalendarId = 9634218L,
					SubcalendarIds = new List<long>{ 9634218L },
					Custom = new Custom
					{
						ContratProvincialContract = new List<string> { "non_no" }
					},
					SignupCount = 0,
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
				},
			new Event
			{
				Id = "1776293830",
				Title = "Fan Club et Club 1909",
				StartDate =  new DateTime(2024, 12, 01, 14, 00, 00),
				EndDate = new DateTime(2024, 12, 01, 19, 30, 00),
				SubcalendarId = 9616459L,
				SubcalendarIds = new List<long>{ 9616459L },
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
			}
		};

		// Act
		var actual = EventApiResponseTransformer.EventApiResponseToSpreadSheetLines(input, config.Calendars);

		// Assert
		actual.Should()
			.NotBeNull()
			.And.BeEquivalentTo(expected);
	}

	private static TeamUpApiConfiguration ReadConfigIntoModel()
	{
		// Read appsettings.json into a model
		var configWithMappingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EventApiResponseTransformerTestFiles\TestsConfig.json");
		var configWithMappingAsString = File.ReadAllText(configWithMappingPath);
		var config = JsonSerializer.Deserialize<TeamUpApiConfiguration>(configWithMappingAsString);
		return config!;
	}
}
