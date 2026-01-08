using System.Globalization;
using System.Text;

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
}
