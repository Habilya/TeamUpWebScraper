using TeamUpWebScraperLibrary.Providers;

namespace TeamUpWebScraperLibrary.Validators;

public class InputValidation
{
	private readonly IDateTimeProvider _dateTimeProvider;

	public InputValidation(IDateTimeProvider dateTimeProvider)
	{
		_dateTimeProvider = dateTimeProvider;
	}

	public List<string> ValidateDatesRange(DateTime? dateFrom, DateTime? dateTo)
	{
		var validationMessages = new List<string>();

		if (dateFrom is null)
		{
			validationMessages.Add("DateFrom cannot be empty");
		}

		if (dateTo is null)
		{
			validationMessages.Add("DateTo cannot be empty");
		}

		if (dateFrom > dateTo)
		{
			validationMessages.Add("DateFrom cannot be greater than DateTo");
		}

		if (dateTo is not null && dateFrom is not null)
		{
			var monthsSpan = (dateTo.Value.Month - dateFrom.Value.Month) + 12 * (dateTo.Value.Year - dateFrom.Value.Year);
			if (monthsSpan > 2)
			{
				validationMessages.Add("Span of DateFrom to DateTo cannot be greater than 2 months (Too much Data)");
			}
		}

		var currentDateTime = _dateTimeProvider.DateTimeNow;
		if (dateFrom > currentDateTime)
		{
			validationMessages.Add($"DateFrom may not be in the future (greater than {currentDateTime.ToString("yyyy-MM-dd")})");
		}

		return validationMessages;
	}
}
