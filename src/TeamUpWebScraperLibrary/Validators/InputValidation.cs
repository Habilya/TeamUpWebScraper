using FluentValidation;
using TeamUpWebScraperLibrary.Providers;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Input;

namespace TeamUpWebScraperLibrary.Validators;

public class InputValidation : AbstractValidator<InputModel>
{
	// TODO: Maybe read it from config??
	private const int MAX_DAYS_SPAN_LIMIT = 60;

	private readonly IDateTimeProvider _dateTimeProvider;

	public InputValidation(IDateTimeProvider dateTimeProvider)
	{
		_dateTimeProvider = dateTimeProvider;

		RuleFor(x => x.DateFrom)
			.Cascade(CascadeMode.Stop)
			.NotNull().WithMessage("DateFrom cannot be empty")
			.LessThanOrEqualTo(x => x.DateTo).WithMessage("DateFrom cannot be greater than DateTo")
			.Must((x, dateFrom) => IsValidDaysSpan(x.DateFrom, x.DateTo)).WithMessage($"Span of DateFrom to DateTo cannot be greater than {MAX_DAYS_SPAN_LIMIT} days (Too much Data)");

		RuleFor(x => x.DateTo)
			.Cascade(CascadeMode.Stop)
			.NotNull().WithMessage("DateTo cannot be empty");

		// DateFrom must NOT be in the future
		RuleFor(x => x.DateFrom)
			.Must((x, dateFrom) => NotInFutureDate(x.DateFrom)).WithMessage($"DateFrom may not be in the future (greater than {_dateTimeProvider.DateTimeNow.ToString("yyyy-MM-dd")})");
	}

	private bool NotInFutureDate(DateTime? dateFrom)
	{
		if (dateFrom is null)
		{
			return true;
		}

		return dateFrom <= _dateTimeProvider.DateTimeNow;
	}

	private bool IsValidDaysSpan(DateTime? dateFrom, DateTime? dateTo)
	{
		if (dateFrom is null || dateTo is null)
		{
			// If either is null, skip the validation
			return true;
		}

		// They've been checked for null at this point...
		TimeSpan difference = (DateTime)dateTo - (DateTime)dateFrom;
		return difference.Days <= MAX_DAYS_SPAN_LIMIT;
	}
}
