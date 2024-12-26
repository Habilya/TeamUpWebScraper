namespace TeamUpWebScraperLibrary;

public class ApiAccess : IApiAccess
{
	// Not to instantiate the HttpClient every time library is called
	private readonly HttpClient client = new();

	public async Task<string> CallApiAsync(string url,
		bool formatOutput = true,
		HttpAction action = HttpAction.GET
	)
	{
		var response = await client.GetAsync(url);

		if (response.IsSuccessStatusCode)
		{
			string responseString = await response.Content.ReadAsStringAsync();
			return "leave me alone for now";

		}
		else
		{
			return $"Error: {response.StatusCode}";
		}
	}

	public bool IsValidUrl(string url)
	{
		if (string.IsNullOrWhiteSpace(url))
		{
			return false;
		}

		bool output = Uri.TryCreate(url, UriKind.Absolute, out Uri uriOutput) &&
			(uriOutput.Scheme == Uri.UriSchemeHttps);

		return output;
	}
}
