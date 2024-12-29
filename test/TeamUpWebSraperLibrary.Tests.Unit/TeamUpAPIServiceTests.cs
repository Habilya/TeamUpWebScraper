using FluentAssertions;
using NSubstitute;
using System.Net;
using System.Text.Json;
using TeamUpWebScraperLibrary.Logging;
using TeamUpWebScraperLibrary.TeamUpAPI;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;

namespace TeamUpWebSraperLibrary.Tests.Unit;

public class CustomHttpMessageHandler : HttpMessageHandler
{
	private readonly HttpResponseMessage _response;

	public CustomHttpMessageHandler(HttpResponseMessage response)
	{
		_response = response;
	}

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return Task.FromResult(_response);
	}
}

public class TeamUpAPIServiceTests
{
	[Fact]
	public async Task GetEventsAsync_OkButTextResponse_ShouldMatch()
	{
		// Arrange
		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var logger = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		var responseAsString = "Hello, world!";
		var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(responseAsString)
		};
		ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var _sut = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, logger);


		// Act
		var actual = await _sut.GetEventsAsync(new DateTime(2012, 12, 1), new DateTime(2012, 12, 25));


		// Assert
		logger.Received(1).LogError(
			Arg.Is<JsonException>(ex => ex.Message.Equals("'H' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.")),
			"Unhandled Exception while GetEventsProcessOkResponse"
		);

		logger.Received(1).LogWarning("Response as text:\nHello, world!");
	}

	// If an exception assertion is needed
	/*
		// Act
		Func<Task> act = () => _sut.GetEventsAsync(new DateTime(2012, 12, 1), new DateTime(2012, 12, 25));


		// Assert
		await act.Should().ThrowAsync<JsonException>()
			.WithMessage("'H' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.");

		// Note: these assertions are called after the await of async function
	 
	 */


	[Fact]
	public async Task GetEventsAsync_OkJsonResponse_ShouldMatch()
	{
		// Arrange
		#region Expected (Huge object)
		var expected = new EventResponse
		{
			Events = new List<Event>
			{
				new Event
				{
					Id = "1781416861",
					Title = "Compétition régionale Yoseikan Budo (arts martiaux)",
					Location = "École Plein Soleil 9 Rue Genest Lévis G6J 1P5 QC",
					StartDate = new DateTime(2024, 12, 01, 09, 00,00),
					EndDate = new DateTime(2024, 12, 01, 16, 00, 00),
					CreationDate = new DateTime(2024, 11, 21, 17, 37, 31),
					UpdateDate = new DateTime(2024, 11, 24, 07, 32, 20),
					SubcalendarId = 11798519L,
					SubcalendarIds = new List<long>{ 11798519L },
					Notes = "<p>Repas et stationnement fournis</p>",
					Custom = new Custom
					{
						ContratProvincialContract = new List<string> { "non_no" },
						NombreDeMembresNecessaires = "2",
						CateGorieCategory = new List<string> { "service" },
					},
					SignupCount = 1,
					Signups = new List<Signup>
					{
						new Signup
						{
							Id = 3821288L,
							Name = "Provost, Francois"
						}
					}
				}, // 0
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
						ContratProvincialContract = new List<string> { "non_no" },
						CateGorieCategory = new List<string> { "formation_training" },
						PrioritePriority2 = new List<string> { "normal" },
						ResponsableInCharge = "Steve Sirois 1-877-889-0631 poste 6102",
					},
					SignupCount = 17,
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
						new Signup
						{
							Id = 3830086L,
							Name = "Antonio Benigno"
						}, // 3
						new Signup
						{
							Id = 3829769L,
							Name = "Rizk Sujaa SC 1002"
						}, // 4
						new Signup
						{
							Id = 3828460L,
							Name = "Alexandra Tardif-Morency"
						}, // 5
						new Signup
						{
							Id = 3827248L,
							Name = "Dany Levesque 971 Laval"
						}, // 6
						new Signup
						{
							Id = 3803990L,
							Name = "Serge Pellerin PR 971"
						}, // 7
						new Signup
						{
							Id = 3791762L,
							Name = "Yannick Gagnon (PR) 883"
						}, // 8
						new Signup
						{
							Id = 3781556L,
							Name = "Renée Legault PR 971"
						}, // 9
						new Signup
						{
							Id = 3771504L,
							Name = "Julie R SG971"
						}, // 10
						new Signup
						{
							Id = 3763438L,
							Name = "Mylene Bessette pr 971"
						}, // 11
						new Signup
						{
							Id = 3757063L,
							Name = "Martin Lee SG 971"
						}, // 12
						new Signup
						{
							Id = 3751965L,
							Name = "Jacques Frédérick PR (SC)"
						}, // 13
						new Signup
						{
							Id = 3751963L,
							Name = "Dany Houde PR (Prov)"
						}, // 14
						new Signup
						{
							Id = 3747445L,
							Name = "Martin Chicoine"
						}, // 15
						new Signup
						{
							Id = 3747442L,
							Name = "Marie-Ève Bélanger (PR) 971"
						}  // 16
					}
				}, // 1
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
						NombreDeMembresNecessaires = "4",
						CateGorieCategory = new List<string> { "service" },
						PrioritePriority2 = new List<string> { "normal" },
					},
					SignupCount = 4,
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
				}, // 2
			}
		};
		#endregion

		var iHttpClientFactory = Substitute.For<IHttpClientFactory>();
		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		var logger = Substitute.For<ILoggerAdapter<TeamUpAPIService>>();

		teamUpApiConfiguration.TimeZone = "America/Toronto";

		var responseAsStringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TeamUpApiServiceTestFiles\ShortEventsResponse.json");
		var responseAsString = File.ReadAllText(responseAsStringPath);

		var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(responseAsString)
		};
		ArrangeHttpClientMock(iHttpClientFactory, responseMessage);

		var _sut = new TeamUpAPIService(iHttpClientFactory, teamUpApiConfiguration, logger);


		// Act
		var actual = await _sut.GetEventsAsync(new DateTime(2012, 12, 1), new DateTime(2012, 12, 25));


		// Assert
		logger.Received(0).LogError(Arg.Any<Exception>(), Arg.Any<string>());
		logger.Received(0).LogWarning(Arg.Any<string>());
		actual.Value.Should().BeEquivalentTo(expected);
	}

	private static void ArrangeHttpClientMock(IHttpClientFactory iHttpClientFactory, HttpResponseMessage httpResponseMessage)
	{
		var handler = new CustomHttpMessageHandler(httpResponseMessage);

		// Create an instance of HttpClient using the mock handler
		var httpClient = new HttpClient(handler)
		{
			// Base address can be any valid URI
			BaseAddress = new Uri("http://localhost/")
		};

		iHttpClientFactory.CreateClient(Arg.Any<string>()).Returns(httpClient);
	}
}
