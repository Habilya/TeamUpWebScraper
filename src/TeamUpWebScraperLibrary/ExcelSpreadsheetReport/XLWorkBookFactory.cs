using ClosedXML.Excel;

namespace TeamUpWebScraperLibrary.ExcelSpreadsheetReport;

public class XLWorkBookFactory : IXLWorkBookFactory
{
	public IXLWorkbook CreateXLWorkBook()
	{
		return new XLWorkbook();
	}
}
