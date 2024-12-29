using System.Text.Json.Serialization;

namespace TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;

public class Custom
{
	public string? Division { get; set; } = default;
	public string? Client2 { get; set; } = default;
	public string? MeDicalMedical { get; set; } = default;

	[JsonPropertyName("priorite_priority2")]
	public List<string>? PrioritePriority2 { get; set; } = default;

	[JsonPropertyName("cate_gorie_category")]
	public List<string>? CateGorieCategory { get; set; } = default;

	[JsonPropertyName("responsable_in_charge")]
	public string? ResponsableInCharge { get; set; } = default;

	[JsonPropertyName("contrat_provincial_contract")]
	public List<string>? ContratProvincialContract { get; set; } = default;

	[JsonPropertyName("nombre_de_membres_ne_cessaires")]
	public string? NombreDeMembresNecessaires { get; set; } = default;
}
