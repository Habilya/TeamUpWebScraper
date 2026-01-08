using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace TeamUpWebScraperLibrary.Helpers;

public static class StringHelper
{
	public static string RemoveDiacritics(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
			return text;

		var normalized = text.Normalize(NormalizationForm.FormD);
		var sb = new StringBuilder(normalized.Length);

		foreach (var c in normalized)
		{
			if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
				sb.Append(c);
		}

		return sb.ToString()
				 .Normalize(NormalizationForm.FormC);
	}

	public static string NormalizeString(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
			return string.Empty;

		// Step 1: remove accents / diacritics
		var cleaned = RemoveDiacritics(input);

		// Step 2: lowercase & remove punctuation
		cleaned = cleaned.ToLowerInvariant();

		// Keep letters, numbers, parentheses, and spaces
		cleaned = Regex.Replace(cleaned, @"[^a-z0-9\s\(\)]", " ");
		cleaned = Regex.Replace(cleaned, @"\s+", " ").Trim();

		// Step 4: sort name parts to reduce order issues
		var parts = cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		Array.Sort(parts);

		return string.Join(" ", parts);
	}

	public static string GetLastName(string name)
	{
		var normalized = NormalizeString(name);
		var parts = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		return parts.Length > 1 ? parts[^1] : ""; // last word if exists
	}

	public static bool HasLastName(string name) => !string.IsNullOrWhiteSpace(GetLastName(name));

	public static double JaroWinklerSimilarity(string s1, string s2)
	{
		if (s1 == s2) return 1.0;
		if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2)) return 0.0;

		int matchDistance = Math.Max(s1.Length, s2.Length) / 2 - 1;
		bool[] s1Matches = new bool[s1.Length];
		bool[] s2Matches = new bool[s2.Length];

		int matches = 0;
		for (int i = 0; i < s1.Length; i++)
		{
			int start = Math.Max(0, i - matchDistance);
			int end = Math.Min(i + matchDistance + 1, s2.Length);

			for (int j = start; j < end; j++)
			{
				if (s2Matches[j]) continue;
				if (s1[i] != s2[j]) continue;

				s1Matches[i] = true;
				s2Matches[j] = true;
				matches++;
				break;
			}
		}

		if (matches == 0) return 0.0;

		double t = 0;
		int k = 0;
		for (int i = 0; i < s1.Length; i++)
		{
			if (!s1Matches[i]) continue;
			while (!s2Matches[k]) k++;
			if (s1[i] != s2[k]) t++;
			k++;
		}
		t /= 2.0;

		double jaro = ((double)matches / s1.Length +
					   (double)matches / s2.Length +
					   (matches - t) / matches) / 3.0;

		// Jaro-Winkler prefix adjustment
		int prefix = 0;
		for (int i = 0; i < Math.Min(4, Math.Min(s1.Length, s2.Length)); i++)
		{
			if (s1[i] == s2[i]) prefix++;
			else break;
		}

		return jaro + (prefix * 0.1 * (1 - jaro));
	}
}
