using FluentAssertions;
using NSubstitute;
using TeamUpWebScraperLibrary.Providers;
using TeamUpWebScraperLibrary.Validators;

namespace TeamUpWebSraperLibrary.Tests.Unit;

public class InputValidationTests
{
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
	// All validations are met, this should pass
	[InlineData(6, "2022-02-01", "2022-02-25")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "UnitTests with testId")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "UnitTests with testId")]
	public void ValidateDatesRange_ShouldReturnValidationMessages_WhenInputIncorrect(int id, string? dateFromAsString, string? dateToAsString)
	{
		// Arrange
		IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();
		dateTimeProvider.DateTimeNow.Returns(new DateTime(2022, 2, 2, 20, 0, 0));

		var _sut = new InputValidation(dateTimeProvider);

		const string DateInputFormat = "yyyy-MM-dd";
		DateTime? dateFrom;
		DateTime? dateTo;
		dateFrom = ParseDateForTest(dateFromAsString, DateInputFormat);
		dateTo = ParseDateForTest(dateToAsString, DateInputFormat);

		var dataMap = new Dictionary<int, List<string>>
		{
			{1, new List<string> {"DateFrom cannot be empty", "DateTo cannot be empty"}},
			{2, new List<string> {"DateTo cannot be empty"}},
			{3, new List<string> {"DateFrom cannot be greater than DateTo"}},
			{4, new List<string> {"Span of DateFrom to DateTo cannot be greater than 2 months (Too much Data)"}},
			{5, new List<string> {"DateFrom may not be in the future (greater than 2022-02-02)"}},
			{6, new List<string> { }},
		};

		var expected = dataMap[id];


		// Act
		var actual = _sut.ValidateDatesRange(dateFrom, dateTo);

		// Assert
		actual.Should().BeEquivalentTo(expected);
	}

	private static DateTime? ParseDateForTest(string? dateAsString, string DateInputFormat)
	{
		if (string.IsNullOrWhiteSpace(dateAsString))
		{
			return null;
		}

		DateTime.TryParseExact(dateAsString, DateInputFormat, null, System.Globalization.DateTimeStyles.None, out DateTime dateFrom);
		return dateFrom;
	}
}
