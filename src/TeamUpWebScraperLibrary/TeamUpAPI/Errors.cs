using ErrorOr;

namespace TeamUpWebScraperLibrary.TeamUpAPI;

public static partial class Errors
{
	public static class TeamUpAPIServiceErrors
	{
		public static Error ApiResponseWithTextError(string errorMessage) => Error.Validation(code: "BadRequest with error as text", description: errorMessage);

		public static Error ApiResponseWithJsonError(ErrorDetail error) => Error.Validation(code: error.Id, description: $"{error.Title}\n{error.Message}");
	}
}

public class ErrorResponse
{
	public ErrorDetail Error { get; set; } = default!;
}

public class ErrorDetail
{
	public string Id { get; set; } = default!;
	public string Title { get; set; } = default!;
	public string Message { get; set; } = default!;
}
