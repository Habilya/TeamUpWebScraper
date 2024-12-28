﻿using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.TeamUpAPI;
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
				Location = "Boisé Papineau, dans le stationnement derrière le CC 3235, boulevard Saint-Martin Est\nLaval (Québec)  H7E 5G8",
				Notes = "<p>Exercice de sauvetage en milieu forestier</p>",
				StartDate = "2024-12-01",
				EndDate = "2024-12-01",
				CreationDate = "2024-10-13",
				UpdateDate = "2024-12-01",
				DeleteDate = default!,
				SignupVisibility = default!,
				SignupCount = "0",
				Signups = new List<string>
				{

				},
				Division = ""

			}, // 0
			new EventSpreadSheetLine
			{
				Id = "CB-241201-1400-0452",
				Title = "Fan Club et Club 1909",
				Location = "1225 st antoine",
				Notes = "<p>Date : 1 décembre 2024</p>",
				StartDate = "2024-12-01",
				EndDate = "2024-12-01",
				CreationDate = "2024-11-12",
				UpdateDate = "2024-11-30",
				DeleteDate = default!,
				SignupVisibility = default!,
				SignupCount = "0",
				Signups = new List<string>
				{

				},
				Division = ""
			} // 1
		};


		var input = new List<Event>
		{
			new Event
			{
				Id = "1759667815",
				Title = "Exercice sauvetage forestier",
				Location = "Boisé Papineau, dans le stationnement derrière le CC 3235, boulevard Saint-Martin Est\nLaval (Québec)  H7E 5G8",
				StartDate = new DateTime(2024, 12, 01, 09, 00, 00),
				EndDate = new DateTime(2024, 12, 01, 12, 00, 00),
				CreationDate = new DateTime(2024, 10, 13, 21, 18, 47),
				UpdateDate = new DateTime(2024, 12, 01, 06, 28, 41),
				SubcalendarId = 9634218L,
				SubcalendarIds = new List<long>{ 9634218L },
				Notes = "<p>Exercice de sauvetage en milieu forestier</p>",
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
			}, // 0
			new Event
			{
				Id = "1776293830",
				Title = "Fan Club et Club 1909",
				Location = "1225 st antoine",
				StartDate = new DateTime(2024, 12, 01, 14, 00, 00),
				EndDate = new DateTime(2024, 12, 01, 19, 30, 00),
				CreationDate = new DateTime(2024, 11, 12, 15, 53, 57),
				UpdateDate = new DateTime(2024, 11, 30, 20, 04, 57),
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
			}  // 1
		};

		// Act
		var actual = EventApiResponseTransformer.EventApiResponseToSpreadSheetLines(input, config.Calendars);

		// Assert
		actual.Should()
			.NotBeNull()
			.And.BeEquivalentTo(expected);
	}

	[Theory]
	[InlineData(1, "DIV-241201-0900-0971")]
	[InlineData(2, "CB-241201-1400-0452")]
	public void GetEventId_ShouldReturnMatching_WhenInputValid(int dataId, string expected)
	{
		//private static string GetEventId(Event eventData, List<CalendarConfiguration> calendarsMapping)
		// Arrange
		var config = ReadConfigIntoModel();
		var dataMap = new Dictionary<int, Event>
		{
			{1, new Event{
				StartDate = new DateTime(2024, 12, 01, 09, 00, 00),
				SubcalendarId = 9634218L,
				SubcalendarIds = new List<long>{ 9634218L },
				Custom = new Custom
				{
					ContratProvincialContract = new List<string> { "non_no" }
				}
			}}, // DIV-241201-0900-0971
			{2, new Event{
				StartDate = new DateTime(2024, 12, 01, 14, 00, 00),
				SubcalendarId = 9616459L,
				SubcalendarIds = new List<long>{ 9616459L },
				Custom = new Custom
				{
					Division = "452",
					Client2 = "centre bell",
					ContratProvincialContract = new List<string> { "non_no" },
				}
			}}, // CB-241201-1400-0452
		};

		var eventData = dataMap[dataId];

		// Private Method info obtained using REFLEXION
		MethodInfo privateMethodGetEventId = typeof(EventApiResponseTransformer)
			.GetMethod("GetEventId", BindingFlags.NonPublic | BindingFlags.Static)!;

		object[] methodParameters = new object[2] { eventData, config.Calendars };

		// Act
		var actual = (string)privateMethodGetEventId.Invoke(null, methodParameters)!;


		// Assert
		actual.Should().Be(expected);
	}


	// TODO: Move this potentially to a separate class if another test for reading config is needed
	private static TeamUpApiConfiguration ReadConfigIntoModel()
	{
		var builder = new ConfigurationBuilder();
		builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			   .AddJsonFile(@"EventApiResponseTransformerTestFiles\TestsConfig.json", optional: false, reloadOnChange: true);

		var configuration = builder.Build();  // Build the configuration

		// Create the TeamUpApiConfiguration object to bind the section to
		var teamUpApiConfiguration = new TeamUpApiConfiguration();

		// Bind the configuration section to the model
		configuration.GetSection(TeamUpApiConstants.CONFIG_SECTION_NAME).Bind(teamUpApiConfiguration);

		return teamUpApiConfiguration;
	}
}
