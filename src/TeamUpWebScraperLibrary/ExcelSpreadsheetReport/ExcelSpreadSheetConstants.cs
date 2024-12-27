namespace TeamUpWebScraperLibrary.ExcelSpreadsheetReport;

public static class ExcelSpreadSheetConstants
{
	public const string ExcelReportFileNameTemplate = "WatcherFileSystemFromKofax_{0}.xlsx";
	public const string ExcelReportFileNameDateFormat = "yyyy-MM-dd_hh-mm-ss";

	public const string ExcelReportSheetName = "Export";

	public const int ExcelReportHeaderLineNumber = 1;


	/*

	events.id	events.title	events.location	events.notes	events.start_dt	events.end_dt	events.creation_dt	events.update_dt	events.delete_dt	events.signup_visibility	events.signup_count	 1	2	3	4	5	6	7	8	9	10	11	12	13	14	15	16	17	18	19	20	21	22	23	24	25	26	27	28	29	30	31	32	33	34	35	36	37	38	39	40	41	42	43	44	45	46	47	48	49	50	51	52	53	54	55	56	57	58	59	60	Column2	


	DIV-241213-0915-0452

	CB-241221-1700-0452	
	Canadiens vs Detroit	
	1225 rue Saint Antoine O	
	
	2024-12-21T17:00:00-05:00	
	2024-12-21T22:45:00-05:00					
	

	
	16	
	JR Guilbault (PR) 0452 	
	Anik Boudreau (PR) 0452 	
	George Loutochin (PR) 0452 	
	Kefan Wu (SG-M) 452 	
	Keandra Charpentier (SG) Jeunesse 843 	
	Louis-Philippe Lemoyne (PRM) 843 	
	John Mercier (PR) 452 	
	Charles-Etienne Pedneault (PR) 1002 	
	Pascal Pedneault (PR) 1002 	
	Zach Inzlicht (SG-M) 452 	
	Alexandre Chevalier-Poirier (PR) 452 	
	Michaël Lambert (843) PRM 	
	Marylène Bouchard-Verret (062) PRM 	
	Mathieu Keromnes (PR) 452 	
	Rosalie Lajeunesse (PR) 1002 	
	Jérémie Martin (PR) 452 																																														0452 





	 */



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

		new KeyValuePair<string, int>("1", 15),
		new KeyValuePair<string, int>("2", 15),
		new KeyValuePair<string, int>("3", 15),
		new KeyValuePair<string, int>("4", 15),
		new KeyValuePair<string, int>("5", 15),
		new KeyValuePair<string, int>("6", 15),
		new KeyValuePair<string, int>("7", 15),
		new KeyValuePair<string, int>("8", 15),
		new KeyValuePair<string, int>("9", 15),
		new KeyValuePair<string, int>("10", 15),

		new KeyValuePair<string, int>("11", 15),
		new KeyValuePair<string, int>("12", 15),
		new KeyValuePair<string, int>("13", 15),
		new KeyValuePair<string, int>("14", 15),
		new KeyValuePair<string, int>("15", 15),
		new KeyValuePair<string, int>("16", 15),
		new KeyValuePair<string, int>("17", 15),
		new KeyValuePair<string, int>("18", 15),
		new KeyValuePair<string, int>("19", 15),
		new KeyValuePair<string, int>("20", 15),

		new KeyValuePair<string, int>("21", 15),
		new KeyValuePair<string, int>("22", 15),
		new KeyValuePair<string, int>("23", 15),
		new KeyValuePair<string, int>("24", 15),
		new KeyValuePair<string, int>("25", 15),
		new KeyValuePair<string, int>("26", 15),
		new KeyValuePair<string, int>("27", 15),
		new KeyValuePair<string, int>("28", 15),
		new KeyValuePair<string, int>("29", 15),
		new KeyValuePair<string, int>("30", 15),

		new KeyValuePair<string, int>("31", 15),
		new KeyValuePair<string, int>("32", 15),
		new KeyValuePair<string, int>("33", 15),
		new KeyValuePair<string, int>("34", 15),
		new KeyValuePair<string, int>("35", 15),
		new KeyValuePair<string, int>("36", 15),
		new KeyValuePair<string, int>("37", 15),
		new KeyValuePair<string, int>("38", 15),
		new KeyValuePair<string, int>("39", 15),
		new KeyValuePair<string, int>("40", 15),

		new KeyValuePair<string, int>("41", 15),
		new KeyValuePair<string, int>("42", 15),
		new KeyValuePair<string, int>("43", 15),
		new KeyValuePair<string, int>("44", 15),
		new KeyValuePair<string, int>("45", 15),
		new KeyValuePair<string, int>("46", 15),
		new KeyValuePair<string, int>("47", 15),
		new KeyValuePair<string, int>("48", 15),
		new KeyValuePair<string, int>("49", 15),
		new KeyValuePair<string, int>("50", 15),

		new KeyValuePair<string, int>("51", 15),
		new KeyValuePair<string, int>("52", 15),
		new KeyValuePair<string, int>("53", 15),
		new KeyValuePair<string, int>("54", 15),
		new KeyValuePair<string, int>("55", 15),
		new KeyValuePair<string, int>("56", 15),
		new KeyValuePair<string, int>("57", 15),
		new KeyValuePair<string, int>("58", 15),
		new KeyValuePair<string, int>("59", 15),
		new KeyValuePair<string, int>("60", 15),

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
