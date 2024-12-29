using ClosedXML.Excel;

namespace TeamUpWebScraperLibrary.ExcelSpreadsheetReport
{
	public interface IXLWorkBookFactory
	{
		IXLWorkbook CreateXLWorkBook();
	}
}