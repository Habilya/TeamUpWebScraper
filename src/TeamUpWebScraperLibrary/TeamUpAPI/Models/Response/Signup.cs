namespace TeamUpWebScraperLibrary.TeamUpAPI.Models.Response;

public class Signup
{
	public long Id { get; set; } = default!;
	public string Name { get; set; } = default!;


	#region !! Those fields are not used, just left here for a reference !!
	//public string EventId { get; set; } = default!;
	//public string Email { get; set; } = default!;
	//public string EmailHash { get; set; }
	//public string RemoteId { get; set; }
	//public bool Readonly { get; set; }
	//public DateTime CreationDt { get; set; }
	//public DateTime? UpdateDt { get; set; }
	#endregion
}
