﻿namespace TeamUpWebScraperLibrary.ExcelSpreadsheetReport;

public static class ExcelSpreadSheetConstants
{
	public const string ExcelReportFileNameTemplate = "WatcherFileSystemFromKofax_{0}.xlsx";
	public const string ExcelReportFileNameDateFormat = "yyyy-MM-dd_hh-mm-ss";

	public const string ExcelReportSheetName = "Export";

	public const int ExcelReportHeaderLineNumber = 1;

	public const int ExcelReportSignupsLimit = 60;




	public const int SIGNUP_NAME_COLUMN_WIDTH = 15;

	public static readonly List<KeyValuePair<string, int>> ExcelReportHeaders = new List<KeyValuePair<string, int>>
	{
		new KeyValuePair<string, int>("events.id", 20),
		new KeyValuePair<string, int>("events.title", 65),
		new KeyValuePair<string, int>("events.location", 25),
		new KeyValuePair<string, int>("events.notes", 15),
		new KeyValuePair<string, int>("events.start_dt", 15),
		new KeyValuePair<string, int>("events.end_dt", 15),
		new KeyValuePair<string, int>("events.creation_dt", 15),
		new KeyValuePair<string, int>("events.update_dt", 15),
		new KeyValuePair<string, int>("events.delete_dt", 15),
		new KeyValuePair<string, int>("events.signup_visibility", 10),
		new KeyValuePair<string, int>("events.signup_count", 15),

		new KeyValuePair<string, int>("1", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("2", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("3", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("4", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("5", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("6", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("7", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("8", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("9", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("10", SIGNUP_NAME_COLUMN_WIDTH),

		new KeyValuePair<string, int>("11", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("12", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("13", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("14", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("15", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("16", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("17", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("18", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("19", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("20", SIGNUP_NAME_COLUMN_WIDTH),

		new KeyValuePair<string, int>("21", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("22", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("23", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("24", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("25", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("26", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("27", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("28", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("29", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("30", SIGNUP_NAME_COLUMN_WIDTH),

		new KeyValuePair<string, int>("31", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("32", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("33", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("34", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("35", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("36", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("37", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("38", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("39", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("40", SIGNUP_NAME_COLUMN_WIDTH),

		new KeyValuePair<string, int>("41", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("42", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("43", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("44", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("45", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("46", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("47", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("48", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("49", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("50", SIGNUP_NAME_COLUMN_WIDTH),

		new KeyValuePair<string, int>("51", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("52", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("53", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("54", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("55", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("56", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("57", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("58", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("59", SIGNUP_NAME_COLUMN_WIDTH),
		new KeyValuePair<string, int>("60", SIGNUP_NAME_COLUMN_WIDTH),

		new KeyValuePair<string, int>("Column2", 25),
	};

	public enum ExcelReportHeadersColumns
	{
		Id = 1,
		title = 2,
		location = 3,
		notes = 4,
		Start_Dt = 5,
		End_Dt = 6,
		Creation_Dt = 7,
		Update_Dt = 8,
		Delete_Dt = 9,
		SignupVisibility = 10,
		SignupCount = 11,

		Column1 = 12,
		Column2 = 13,
		Column3 = 14,
		Column4 = 15,
		Column5 = 16,
		Column6 = 17,
		Column7 = 18,
		Column8 = 19,
		Column9 = 20,
		Column10 = 21,

		Column11 = 22,
		Column12 = 23,
		Column13 = 24,
		Column14 = 25,
		Column15 = 26,
		Column16 = 27,
		Column17 = 28,
		Column18 = 29,
		Column19 = 30,
		Column20 = 31,

		Column21 = 32,
		Column22 = 33,
		Column23 = 34,
		Column24 = 35,
		Column25 = 36,
		Column26 = 37,
		Column27 = 38,
		Column28 = 39,
		Column29 = 40,
		Column30 = 41,

		Column31 = 42,
		Column32 = 43,
		Column33 = 44,
		Column34 = 45,
		Column35 = 46,
		Column36 = 47,
		Column37 = 48,
		Column38 = 49,
		Column39 = 50,
		Column40 = 51,

		Column41 = 52,
		Column42 = 53,
		Column43 = 54,
		Column44 = 55,
		Column45 = 56,
		Column46 = 57,
		Column47 = 58,
		Column48 = 59,
		Column49 = 60,
		Column50 = 61,

		Column51 = 62,
		Column52 = 63,
		Column53 = 64,
		Column54 = 65,
		Column55 = 66,
		Column56 = 67,
		Column57 = 68,
		Column58 = 69,
		Column59 = 70,
		Column60 = 71,

		Division_Column2 = 72
	}
}