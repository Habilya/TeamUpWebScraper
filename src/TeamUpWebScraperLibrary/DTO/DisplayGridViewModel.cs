namespace TeamUpWebScraperLibrary.DTO;

public class DisplayGridViewModel
{
	public bool Selected { get; set; } = default!;
	public int UniqueId { get; set; } = default!;
	public string Title { get; init; } = default!;
	public string EventId { get; init; } = default!;
	public string StartDate { get; init; } = default!;
	public string EndDate { get; init; } = default!;
	public double Hours { get; init; } = default!;
	public string SignupCount { get; init; } = default!;
	public string PresencesConcat { get; init; } = default!;
}
