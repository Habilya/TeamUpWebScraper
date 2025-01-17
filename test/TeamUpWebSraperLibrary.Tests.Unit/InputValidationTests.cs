﻿using NSubstitute;
using Shouldly;
using TeamUpWebScraperLibrary.DTO;
using TeamUpWebScraperLibrary.Providers;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;
using TeamUpWebScraperLibrary.Validators;

namespace TeamUpWebSraperLibrary.Tests.Unit;

public class InputValidationTests
{
	private readonly string DateInputFormat = "yyyy-MM-dd";

	private readonly InputValidation _sut;

	public InputValidationTests()
	{
		// !! Attention !! : this code in the constructor will be executed for each single test.
		// This assures the self containability of tests
		var dateTimeProvider = Substitute.For<IDateTimeProvider>();
		dateTimeProvider.DateTimeNow.Returns(new DateTime(2022, 2, 2, 20, 0, 0));

		var teamUpApiConfiguration = Substitute.For<TeamUpApiConfiguration>();
		teamUpApiConfiguration.MaxDaysDataSpanLimit = 60;

		_sut = new InputValidation(dateTimeProvider, teamUpApiConfiguration);
	}


	[Theory]
	// Validation should fail when either of the input dates is null
	[InlineData(1, null, null, false)]
	[InlineData(2, "2022-02-02", null, false)]
	// Validation should fail when date From is greater than DateTo
	[InlineData(3, "2022-01-05", "2022-01-02", false)]
	// Span of DateFrom to DateTo cannot be greater than 2 months
	[InlineData(4, "2022-02-02", "2022-05-02", false)]
	// DateFrom may not be in the future
	[InlineData(5, "2022-03-02", "2022-03-20", false)]
	// All validations are met, this should pass (Exactly 60 days) DateTo >= "2022-04-03" wouldn't pass
	[InlineData(6, "2022-02-01", "2022-04-02", true)]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "UnitTests with testId")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "UnitTests with testId")]
	public void InputValidation_Validate_ShouldValidate_WhenInputProvided(int id, string? dateFromAsString, string? dateToAsString, bool expected)
	{
		// Arrange
		var inputValues = new InputViewModel(
			TestsHelper.ParseDateForTest(dateFromAsString, DateInputFormat),
			TestsHelper.ParseDateForTest(dateToAsString, DateInputFormat));

		// Act
		var actual = _sut.Validate(inputValues);

		// Assert
		actual.IsValid.ShouldBe(expected);
	}

	[Theory]
	// Validation should fail when either of the input dates is null
	[InlineData(1, null, null)]
	[InlineData(2, "2022-02-02", null)]
	// Validation should fail when date From is greater than DateTo
	[InlineData(3, "2022-01-05", "2022-01-02")]
	// Span of DateFrom to DateTo cannot be greater than 2 months
	[InlineData(4, "2022-02-02", "2022-05-02")]
	// DateFrom may not be in the future
	[InlineData(5, "2022-03-02", "2022-03-20")]
	// All validations are met, this should pass (Exactly 60 days) DateTo >= "2022-04-03" wouldn't pass
	[InlineData(6, "2022-02-01", "2022-04-02")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "UnitTests with testId")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "UnitTests with testId")]
	public void InputValidation_Validate_ShouldReturnValidationMessages_WhenInputProvided(int id, string? dateFromAsString, string? dateToAsString)
	{
		// Arrange
		var inputValues = new InputViewModel(
			TestsHelper.ParseDateForTest(dateFromAsString, DateInputFormat),
			TestsHelper.ParseDateForTest(dateToAsString, DateInputFormat));

		var dataMap = new Dictionary<int, List<string>>
		{
			//  [InlineData(1, null, null)]
			{1, new List<string>
				{
					"DateFrom cannot be empty",
					"DateTo cannot be empty"
				}
			},
			// [InlineData(2, "2022-02-02", null)]
			{2, new List<string>
				{
					"DateFrom cannot be greater than DateTo",
					"DateTo cannot be empty"
				}
			},
			// [InlineData(3, "2022-01-05", "2022-01-02")]
			{3, new List<string>
				{
					"DateFrom cannot be greater than DateTo"
				}
			},
			// [InlineData(4, "2022-02-02", "2022-05-02")]
			{4, new List<string>
				{
					"Span of DateFrom to DateTo cannot be greater than 60 days (Too much Data)"
				}
			},
			// [InlineData(5, "2022-03-02", "2022-03-20")]
			{5, new List<string>
				{
					"DateFrom may not be in the future (greater than 2022-02-02)"
				}
			},
			// [InlineData(6, "2022-02-01", "2022-04-02")]
			{6, new List<string>{} },
		};

		var expected = dataMap[id];


		// Act
		var actual = _sut.Validate(inputValues);

		// Assert
		actual.Errors
			.Select(s => s.ErrorMessage)
			.ToList()
			.ShouldBe(expected);
	}
}
