namespace TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;

public class Custom
{
	public string? Division { get; set; } = default!;
	public List<string>? ContratProvincialContract { get; set; } = default!;
	public string? NombreDeMembresNecessaires { get; set; } = default!;

	#region !! Those fields are not used, just left here for a reference !!
	//public string MeDicalMedical { get; set; }
	//public List<string> PrioritePriority2 { get; set; }
	//public List<string> CateGorieCategory { get; set; }
	//public string ResponsableInCharge { get; set; }
	//public string Client2 { get; set; }
	#endregion
}
