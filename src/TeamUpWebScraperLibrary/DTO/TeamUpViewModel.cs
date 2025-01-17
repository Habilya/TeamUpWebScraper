namespace TeamUpWebScraperLibrary.DTO;

public record TeamUpViewModel(
	bool IsValid,
	string ErrorTitle,
	string ErrorMessage
);
