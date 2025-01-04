using FluentAssertions;
using NSubstitute;
using System.Net;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.Logging;
using TeamUpWebScraperLibrary.TeamUpAPI;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;
using TeamUpWebScraperLibrary.Transformers;

namespace TeamUpWebSraperLibrary.Tests.Unit;

public class EventApiResponseToReportModelAcceptanceTests
{
	private readonly VerifySettings _verifySettings;

	public EventApiResponseToReportModelAcceptanceTests()
	{
		_verifySettings = new VerifySettings();
		_verifySettings.UseDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EventApiResponseToReportModelAcceptanceTests"));
	}

	[Fact]
	public async Task EventApiResponseToSpreadSheetLines_ShouldMatchExpected()
	{
		// Arrange
		#region Expected (Huge object)
		var expected = new List<EventSpreadSheetLine>
		{
				new EventSpreadSheetLine
				{
					Id = "DIV-240901-1545-0094",
					Division = "0094",
					PresencesConcat = "mohamedabidi82 //Gaetan Lamarre //Emilie Dionne //Stéphanie Déragon //Franck //Pamela"
				}, // 0 DIV-240901-1545-0094
				new EventSpreadSheetLine
				{
					Id = "DIV-240901-1630-0062",
					Division = "0062",
					PresencesConcat = "Nicolas Arnau"
				}, // 1 DIV-240901-1630-0062
				new EventSpreadSheetLine
				{
					Id = "PB-240901-1830-0971",
					Division = "0971",
					PresencesConcat = "Steve Sirois //Ethan Sirois - PR 971 //Théo Mandamiento (PR) 452"
				}, // 2 PB-240901-1830-0971 // (OK) not in original expected
				new EventSpreadSheetLine
				{
					Id = "DIV-240902-1900-1002",
					Division = "1002",
					PresencesConcat = "Eric Theriault (Pr 1002) //Benjamin Rose (PR) 1002"
				}, // 3 DIV-240902-1900-1002
				new EventSpreadSheetLine
				{
					Id = "PB-240904-1830-0971",
					Division = "0971",
					PresencesConcat = "Yannick gagnon //Olivier Robert 971 PR //Martin Lee SG 971 //Serge Pellerin PR 971 //Kristie Lam(SG-M) 452 //Jaiden Tamakuwala //Dany Levesque //Benoit Vachon PR971 //Steve Sirois //Ethan Sirois - PR 971"
				}, // 4 PB-240904-1830-0971 // (OK) not in original expected
				new EventSpreadSheetLine
				{
					Id = "DIV-240905-1900-0971",
					Division = "0971",
					PresencesConcat = "Yannick gagnon //Benoit Vachon PR971 //Jean-Luc Renaud //Hélène Caron - 971 PR //Daniel Calvé Pr-971 //Martin Lee SG 971 //Steve Sirois //Marie-Ève Bélanger PR 971 //Olivier Robert 971 PR //martin chicoine //Serge Pellerin PR 971"
				}, // 5 DIV-240905-1900-0971
				new EventSpreadSheetLine
				{
					Id = "DIV-240907-1000-1002",
					Division = "1002",
					PresencesConcat = "Abidi //Charles-Etienne Pedneault (PR) 1002 //Eric Theriault"
				}, // 6 DIV-240907-1000-1002
				new EventSpreadSheetLine
				{
					Id = "DIV-240907-1100-0062",
					Division = "0062",
					PresencesConcat = "Ken Harding PRM 0062"
				}, // 7 DIV-240907-1100-0062
				new EventSpreadSheetLine
				{
					Id = "DIV-240907-1330-0280",
					Division = "0280",
					PresencesConcat = "Claudine Morisseau PR 280"
				}, // 8 DIV-240907-1330-0280
				new EventSpreadSheetLine
				{
					Id = "DIV-240907-1500-0158",
					Division = "0158",
					PresencesConcat = "Caroline Girardot -SG 158 //Stephane Larocque PRM 158"
				}, // 9 DIV-240907-1500-0158
				new EventSpreadSheetLine
				{
					Id = "PB-240907-1830-0971",
					Division = "0971",
					PresencesConcat = "Ethan Sirois - PR 971 //Martin Lee //Thibault T. Becquaert (PR) PROV //Francis Dessureault-Vinette PR 971 //Nathan Lemelin SG 971 //Guillaume Benoit-Gagné PR 971 //Yannick gagnon //Steve Sirois //mylene bessette //Evens-Edson MAURICE //Sarah Tessier PR (883) //Suzie Gingras (SG) 452 //Erick Phiri //Hélène Caron Pr 971 //Stéphanie Bernier Benoit (PR) 452 //Mylene Binette (Pr) 280 //Dany Levesque //Precious Tsikwa //Serge Pellerin 971 PR //Marie-Ève Bélanger SG 971"
				}, // 10 PB-240907-1830-0971 // (OK) not in original expected
				new EventSpreadSheetLine
				{
					Id = "DIV-240907-1830-0280",
					Division = "",
					PresencesConcat = ""
				}, // 11 DIV-240907-1830-0280
				// DIV-240908-0900-0062  (OK) Not here because 0 signups
				new EventSpreadSheetLine
				{
					Id = "DIV-240908-1300-0280",
					Division = "",
					PresencesConcat = ""
				}, // 12 DIV-240908-1300-0280
				new EventSpreadSheetLine
				{
					Id = "DIV-240908-1345-0094",
					Division = "",
					PresencesConcat = ""
				}, // 13 DIV-240908-1345-0094
				new EventSpreadSheetLine
				{
					Id = "DIV-240909-0745-0094",
					Division = "",
					PresencesConcat = ""
				}, // 14 DIV-240909-0745-0094
				new EventSpreadSheetLine
				{
					Id = "DIV-240910-0745-0094",
					Division = "",
					PresencesConcat = ""
				}, // 15 DIV-240910-0745-0094
				new EventSpreadSheetLine
				{
					Id = "DIV-240910-1850-0452",
					Division = "",
					PresencesConcat = ""
				}, // 16 DIV-240910-1850-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240911-0745-0094",
					Division = "",
					PresencesConcat = ""
				}, // 17 DIV-240911-0745-0094
				new EventSpreadSheetLine
				{
					Id = "DIV-240912-1600-0280",
					Division = "",
					PresencesConcat = ""
				}, // 18 DIV-240912-1600-0280
				new EventSpreadSheetLine
				{
					Id = "DIV-240913-1300-0062",
					Division = "",
					PresencesConcat = ""
				}, // 19 DIV-240913-1300-0062
				new EventSpreadSheetLine
				{
					Id = "DIV-240913-1630-0971",
					Division = "",
					PresencesConcat = ""
				}, // 20 DIV-240913-1630-0971
				new EventSpreadSheetLine
				{
					Id = "CB-240913-1730-0452",
					Division = "",
					PresencesConcat = ""
				}, // 21 CB-240913-1730-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240913-1815-0094",
					Division = "",
					PresencesConcat = ""
				}, // 22 DIV-240913-1815-0094
				new EventSpreadSheetLine
				{
					Id = "DIV-240914-0830-0452",
					Division = "",
					PresencesConcat = ""
				}, // 23 DIV-240914-0830-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240914-0945-0094",
					Division = "",
					PresencesConcat = ""
				}, // 24 DIV-240914-0945-0094
				new EventSpreadSheetLine
				{
					Id = "CB-240914-1030-0452",
					Division = "",
					PresencesConcat = ""
				}, // 25 CB-240914-1030-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240914-1130-0452",
					Division = "",
					PresencesConcat = ""
				}, // 26 DIV-240914-1130-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240914-1300-0062",
					Division = "",
					PresencesConcat = ""
				}, // 27 DIV-240914-1300-0062
				new EventSpreadSheetLine
				{
					Id = "PB-240914-1830-0971",
					Division = "",
					PresencesConcat = ""
				}, // 28 PB-240914-1830-0971
				new EventSpreadSheetLine
				{
					Id = "DIV-240915-0915-1002",
					Division = "",
					PresencesConcat = ""
				}, // 29 DIV-240915-0915-1002
				new EventSpreadSheetLine
				{
					Id = "CB-240915-1100-0452",
					Division = "",
					PresencesConcat = ""
				}, // 30 CB-240915-1100-0452
				new EventSpreadSheetLine
				{
					Id = "PB-240915-1800-0971",
					Division = "",
					PresencesConcat = ""
				}, // 31 PB-240915-1800-0971
				new EventSpreadSheetLine
				{
					Id = "PB-240916-1800-0971",
					Division = "",
					PresencesConcat = ""
				}, // 32 PB-240916-1800-0971
				new EventSpreadSheetLine
				{
					Id = "CB-240917-1800-0452",
					Division = "",
					PresencesConcat = ""
				}, // 33 CB-240917-1800-0452
				new EventSpreadSheetLine
				{
					Id = "CB-240918-1630-0452",
					Division = "",
					PresencesConcat = ""
				}, // 34 CB-240918-1630-0452
				new EventSpreadSheetLine
				{
					Id = "PB-240919-1700-0971",
					Division = "",
					PresencesConcat = ""
				}, // 35 PB-240919-1700-0971
				new EventSpreadSheetLine
				{
					Id = "DIV-240919-1900-0452",
					Division = "",
					PresencesConcat = ""
				}, // 36 DIV-240919-1900-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240920-0000-0783",
					Division = "",
					PresencesConcat = ""
				}, // 37 DIV-240920-0000-0783
				new EventSpreadSheetLine
				{
					Id = "DIV-240920-0800-0062",
					Division = "",
					PresencesConcat = ""
				}, // 38 DIV-240920-0800-0062
				new EventSpreadSheetLine
				{
					Id = "CB-240920-1700-0452",
					Division = "",
					PresencesConcat = ""
				}, // 39 CB-240920-1700-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240920-1800-0158",
					Division = "",
					PresencesConcat = ""
				}, // 40 DIV-240920-1800-0158
				new EventSpreadSheetLine
				{
					Id = "PB-240920-1830-0971",
					Division = "",
					PresencesConcat = ""
				}, // 41 PB-240920-1830-0971
				new EventSpreadSheetLine
				{
					Id = "DIV-240921-0945-0158",
					Division = "",
					PresencesConcat = ""
				}, // 42 DIV-240921-0945-0158
				new EventSpreadSheetLine
				{
					Id = "DIV-240921-1100-0094",
					Division = "",
					PresencesConcat = ""
				}, // 43 DIV-240921-1100-0094
				new EventSpreadSheetLine
				{
					Id = "DIV-240921-1515-0094",
					Division = "",
					PresencesConcat = ""
				}, // 44 DIV-240921-1515-0094
				new EventSpreadSheetLine
				{
					Id = "PB-240921-1800-0971",
					Division = "",
					PresencesConcat = ""
				}, // 45 PB-240921-1800-0971
				new EventSpreadSheetLine
				{
					Id = "CB-240921-1800-0452",
					Division = "",
					PresencesConcat = ""
				}, // 46 CB-240921-1800-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240922-0930-0452",
					Division = "",
					PresencesConcat = ""
				}, // 47 DIV-240922-0930-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240922-1500-0158",
					Division = "",
					PresencesConcat = ""
				}, // 48 DIV-240922-1500-0158
				new EventSpreadSheetLine
				{
					Id = "CB-240923-1700-0452",
					Division = "",
					PresencesConcat = ""
				}, // 49 CB-240923-1700-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240924-0700-Prov",
					Division = "",
					PresencesConcat = ""
				}, // 50 DIV-240924-0700-Prov
				new EventSpreadSheetLine
				{
					Id = "PB-240924-1630-0971",
					Division = "",
					PresencesConcat = ""
				}, // 51 PB-240924-1630-0971
				new EventSpreadSheetLine
				{
					Id = "CB-240924-1700-0452",
					Division = "",
					PresencesConcat = ""
				}, // 52 CB-240924-1700-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240924-1850-0452",
					Division = "",
					PresencesConcat = ""
				}, // 53 DIV-240924-1850-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240925-0700-Prov",
					Division = "",
					PresencesConcat = ""
				}, // 54 DIV-240925-0700-Prov
				new EventSpreadSheetLine
				{
					Id = "DIV-240925-1100-0094",
					Division = "",
					PresencesConcat = ""
				}, // 55 DIV-240925-1100-0094
				new EventSpreadSheetLine
				{
					Id = "PB-240925-1730-0971",
					Division = "",
					PresencesConcat = ""
				}, //56 PB-240925-1730-0971
				new EventSpreadSheetLine
				{
					Id = "CB-240925-1800-0452",
					Division = "",
					PresencesConcat = ""
				}, // 57 CB-240925-1800-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240926-0700-Prov",
					Division = "",
					PresencesConcat = ""
				}, // 58 DIV-240926-0700-Prov
				new EventSpreadSheetLine
				{
					Id = "DIV-240926-1900-0971",
					Division = "",
					PresencesConcat = ""
				}, // 59 DIV-240926-1900-0971 // (OK) not in original expected
				new EventSpreadSheetLine
				{
					Id = "DIV-240927-0700-Prov",
					Division = "",
					PresencesConcat = ""
				}, // 60 DIV-240927-0700-Prov
				new EventSpreadSheetLine
				{
					Id = "DIV-240927-1530-0452",
					Division = "",
					PresencesConcat = ""
				}, // 61 DIV-240927-1530-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240927-1800-0158",
					Division = "",
					PresencesConcat = ""
				}, // 62 DIV-240927-1800-0158
				new EventSpreadSheetLine
				{
					Id = "DIV-240927-1800-0783",
					Division = "",
					PresencesConcat = ""
				}, // 63 DIV-240927-1800-0783
				new EventSpreadSheetLine
				{
					Id = "DIV-240927-1815-0094",
					Division = "",
					PresencesConcat = ""
				}, // 64 DIV-240927-1815-0094
				new EventSpreadSheetLine
				{
					Id = "DIV-240928-0700-Prov",
					Division = "",
					PresencesConcat = ""
				}, // 65 DIV-240928-0700-Prov
				new EventSpreadSheetLine
				{
					Id = "DIV-240928-0800-0062",
					Division = "",
					PresencesConcat = ""
				}, // 66 DIV-240928-0800-0062
				new EventSpreadSheetLine
				{
					Id = "DIV-240928-0800-0971",
					Division = "",
					PresencesConcat = ""
				}, // 67 DIV-240928-0800-0971
				new EventSpreadSheetLine
				{
					Id = "DIV-240928-0945-0452",
					Division = "",
					PresencesConcat = ""
				}, // 68 DIV-240928-0945-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240928-1000-0280",
					Division = "",
					PresencesConcat = ""
				}, // 69 DIV-240928-1000-0280
				new EventSpreadSheetLine
				{
					Id = "DIV-240928-1015-0094",
					Division = "",
					PresencesConcat = ""
				}, // 70 DIV-240928-1015-0094
				new EventSpreadSheetLine
				{
					Id = "DIV-240928-1645-0094",
					Division = "",
					PresencesConcat = ""
				}, // 71 DIV-240928-1645-0094
				new EventSpreadSheetLine
				{
					Id = "CB-240928-1700-0452",
					Division = "",
					PresencesConcat = ""
				}, // 72 CB-240928-1700-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240928-1800-0158",
					Division = "",
					PresencesConcat = ""
				}, // 73 DIV-240928-1800-0158
				new EventSpreadSheetLine
				{
					Id = "DIV-240929-0700-Prov",
					Division = "",
					PresencesConcat = ""
				}, // 74 DIV-240929-0700-Prov
				new EventSpreadSheetLine
				{
					Id = "DIV-240929-0800-0280",
					Division = "",
					PresencesConcat = ""
				}, // 75 DIV-240929-0800-0280
				new EventSpreadSheetLine
				{
					Id = "DIV-240929-1130-0452",
					Division = "",
					PresencesConcat = ""
				}, // 76 DIV-240929-1130-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240929-1230-0452",
					Division = "",
					PresencesConcat = ""
				}, // 77 DIV-240929-1230-0452
				new EventSpreadSheetLine
				{
					Id = "DIV-240929-1500-0158",
					Division = "",
					PresencesConcat = ""
				}, // 78 DIV-240929-1500-0158
				new EventSpreadSheetLine
				{
					Id = "DIV-240929-1500-0783",
					Division = "",
					PresencesConcat = ""
				}, // 79 DIV-240929-1500-0783
				new EventSpreadSheetLine
				{
					Id = "PB-240930-1730-0971",
					Division = "",
					PresencesConcat = ""
				}, // 80 PB-240930-1730-0971
				new EventSpreadSheetLine
				{
					Id = "CB-240930-1900-0452",
					Division = "",
					PresencesConcat = ""
				}, // 81 CB-240930-1900-0452
		};
		#endregion

		#region Arrangement of APIService response
		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var logger = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		var responseAsStringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EventApiResponseToReportModelAcceptanceTests\SeptemberEventsAPIResponse.json");
		var responseAsString = File.ReadAllText(responseAsStringPath);

		var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(responseAsString)
		};
		TestsHelper.ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var teamUpAPIService = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, logger);
		// Doesn't really matter as the response is mocked
		var dateFromParam = new DateTime(2024, 11, 1);
		var dateToParam = new DateTime(2024, 11, 30);
		#endregion

		var _sut = new EventApiResponseTransformer();
		var subCalendars = TestsHelper.ReadSubCalendarsFromJSON(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TeamUpApiServiceTestFiles\SubCalendars.json"));

		// Act
		var apiResponse = await teamUpAPIService.GetEventsAsync(dateFromParam, dateToParam);
		var actual = _sut.EventApiResponseToSpreadSheetLines(apiResponse.Value.Events, subCalendars);


		// Assert
		actual.Should()
			.NotBeNull()
			.And.HaveCount(82)
			.And.BeEquivalentTo(expected, options =>
			{
				options
					.Including(s => s.Id)
					//.Including(s => s.Division)
					//.Including(s => s.PresencesConcat)
					.WithStrictOrdering();
				return options;
			});
	}

	[Fact]
	public async Task EventApiResponseToSpreadSheetLines_ShouldVerrify()
	{
		// Arrange
		#region Arrangement of APIService response
		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var logger = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		var responseAsStringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EventApiResponseToReportModelAcceptanceTests\SeptemberEventsAPIResponse.json");
		var responseAsString = File.ReadAllText(responseAsStringPath);

		var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(responseAsString)
		};
		TestsHelper.ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var teamUpAPIService = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, logger);
		// Doesn't really matter as the response is mocked
		var dateFromParam = new DateTime(2024, 11, 1);
		var dateToParam = new DateTime(2024, 11, 30);
		#endregion

		var _sut = new EventApiResponseTransformer();
		var subCalendars = TestsHelper.ReadSubCalendarsFromJSON(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TeamUpApiServiceTestFiles\SubCalendars.json"));

		// Act
		var apiResponse = await teamUpAPIService.GetEventsAsync(dateFromParam, dateToParam);
		var actual = _sut.EventApiResponseToSpreadSheetLines(apiResponse.Value.Events, subCalendars);


		// Assert
		await Verify(actual, _verifySettings)
				.DontScrubDateTimes();
	}
}
