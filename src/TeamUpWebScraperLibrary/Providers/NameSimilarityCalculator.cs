using TeamUpWebScraperLibrary.Helpers;

namespace TeamUpWebScraperLibrary.Providers;

public class NameSimilarityCalculator
{
	public double Compute(string raw1, string raw2)
	{
		var n1 = StringHelper.NormalizeString(raw1);
		var n2 = StringHelper.NormalizeString(raw2);

		return StringHelper.JaroWinklerSimilarity(n1, n2);
	}
}
