namespace TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;

public class EventSpreadSheetLine
{
	/*

	events.id	events.title	events.location	events.notes	events.start_dt	events.end_dt	events.creation_dt	events.update_dt	events.delete_dt	events.signup_visibility	events.signup_count	
	1	2	3	4	5	6	7	8	9	10	11	12	13	14	15	16	17	18	19	20	21	22	23	24	25	26	27	28	29	30	31	32	33	34	35	36	37	38	39	40	41	42	43	44	45	46	47	48	49	50	51	52	53	54	55	56	57	58	59	60	
	Column2	


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

	public string Id { get; set; } = default!;

	public string Title { get; set; } = default!;
	public string Location { get; set; } = default!;
	public string Notes { get; set; } = default!;
	public string StartDate { get; set; } = default!;
	public string EndDate { get; set; } = default!;
	public string CreationDate { get; set; } = default!;
	public string UpdateDate { get; set; } = default!;
	public string DeleteDate { get; set; } = default!;
	public string SignupVisibility { get; set; } = default!;
	public string SignupCount { get; set; } = default!;

	public List<string> Signups { get; set; } = default!;

	public string Division { get; set; } = default!;
}
