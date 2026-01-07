namespace TeamUpWebScraperLibrary.DTO;

public record DisplayGridViewModel(
	string Title,
	string EventId,
	string StartDate,
	string EndDate,
	double Hours,
	string SignupCount,
	string PresencesConcat
);
