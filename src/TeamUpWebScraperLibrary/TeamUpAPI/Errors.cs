using ErrorOr;

namespace TeamUpWebScraperLibrary.TeamUpAPI;

public static partial class Errors
{
	public static class Events
	{
		public static Error ApiTokenMissingOrInvalid() => Error.Validation(code: "Events.ApiToken.MissingOrInvalid", description: $"API Token missing or invalid");

		public static Error NotFound() => Error.Validation(code: "Events.MotFound", description: $"404 Not found");

		public static Error BadRequest() => Error.Validation(code: "Events.BadRequest", description: $"400 Bad Request");

		public static Error ApiRequestError(ErrorDetail error) => Error.Validation(code: error.id, description: $"{error.title}\n{error.message}");
	}
}

public class ErrorResponse
{
	public ErrorDetail error { get; set; } = default!;
}

public class ErrorDetail
{
	public string id { get; set; } = default!;
	public string title { get; set; } = default!;
	public string message { get; set; } = default!;
}
