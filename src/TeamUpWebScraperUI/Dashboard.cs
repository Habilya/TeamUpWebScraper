using TeamUpWebScraperLibrary.DisplayGridView;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport;
using TeamUpWebScraperLibrary.ExcelSpreadsheetReport.Models;
using TeamUpWebScraperLibrary.Logging;
using TeamUpWebScraperLibrary.TeamUpAPI;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Config;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Input;
using TeamUpWebScraperLibrary.Transformers;
using TeamUpWebScraperLibrary.Validators;
using TeamUpWebScraperUI.Constants;
using TeamUpWebScraperUI.DisplayDataGridGeneration;

namespace TeamUpWebScraperUI;

public partial class Dashboard : Form
{
	private readonly ILoggerAdapter<Dashboard> _logger;
	private readonly InputValidation _inputValidation;
	private readonly ITeamUpAPIService _teamUpAPIService;
	private readonly IEventApiResponseTransformer _eventApiResponseTransformer;
	private readonly IExcelSpreadsheetReportProvider _excelSpreadsheetReportProvider;
	private readonly TeamUpApiConfiguration _teamUpApiConfiguration;
	private readonly IDisplayGridViewProvider _displayGridViewProvider;


	private List<EventSpreadSheetLine> ReportSpreadsheetLines { get; set; } = default!;

	public Dashboard(
		ILoggerAdapter<Dashboard> logger,
		InputValidation inputValidation,
		ITeamUpAPIService teamUpAPIService,
		IEventApiResponseTransformer eventApiResponseTransformer,
		IExcelSpreadsheetReportProvider excelSpreadsheetReportProvider,
		TeamUpApiConfiguration teamUpApiConfiguration,
		IDisplayGridViewProvider displayGridViewProvider)
	{
		_logger = logger;
		_inputValidation = inputValidation;
		_teamUpAPIService = teamUpAPIService;
		_eventApiResponseTransformer = eventApiResponseTransformer;
		_excelSpreadsheetReportProvider = excelSpreadsheetReportProvider;
		_teamUpApiConfiguration = teamUpApiConfiguration;
		_displayGridViewProvider = displayGridViewProvider;

		InitializeComponent();
		ReinitUIElements();
		DisplayVersion();
	}

	private void ReinitUIElements()
	{
		saveXLSX.Enabled = false;
		resultsLabel.Text = DashBoardConstants.RESULTS_LABEL_DEFAULT;
		dataGridViewResults.Columns.Clear();
		dataGridViewResults.Rows.Clear();
	}

	private void DisplayVersion()
	{
		// Add Version to this form title bar
		Text += $" {Versionning.GetVersionOneLiner()}";
	}

	private async void CallAPI_Click(object sender, EventArgs e)
	{
		try
		{
			ReinitUIElements();
			ReportSpreadsheetLines = default!;
			#region Input Validation
			// Get Input values into a model
			var inputValues = GetInputIntoModel();
			if (!IsValidInputValues(inputValues))
			{
				return;
			}
			#endregion

			#region Calling API
			// At this point, Assuming the input Values have been validated
			var eventsRouteResponse = await _teamUpAPIService.GetEventsAsync((DateTime)inputValues.DateFrom!, (DateTime)inputValues.DateTo!);
			if (eventsRouteResponse.IsError)
			{
				var firstError = eventsRouteResponse.FirstError;
				MessageBox.Show(firstError.Description, firstError.Code, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var eventsList = eventsRouteResponse.Value.Events;
			if (eventsList is null || !eventsList.Any())
			{
				MessageBox.Show("For some reason the events list is empty...", "Events List Empty", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			#endregion

			#region Transforming API response into reportable model object
			ReportSpreadsheetLines = _eventApiResponseTransformer.EventApiResponseToSpreadSheetLines(eventsList, _teamUpApiConfiguration.Calendars);
			#endregion

			#region UI acttions depending on the result
			if (ReportSpreadsheetLines is null || !ReportSpreadsheetLines.Any())
			{
				MessageBox.Show("For some reason the Transformed list that goes in Excel is empty...", "Events List Empty", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			else
			{
				resultsLabel.Text = string.Format(DashBoardConstants.RESULTS_LABEL_WITH_RESULTS, ReportSpreadsheetLines.Count);
				var displayResults = _displayGridViewProvider.TransformReportSpreadsheetLinesInotDisplayLines(ReportSpreadsheetLines);
				DataGridViewHelper.GenerateDataGridView(dataGridViewResults, displayResults);
				saveXLSX.Enabled = true;
			}
			#endregion
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "CallAPI_Click Button threw an unhandled exception.");
			ShowUnhandledExceptionPopup();
		}
	}

	private InputModel GetInputIntoModel()
	{
		return new InputModel
		{
			DateFrom = dtpDateFrom.Value.Date,
			DateTo = dtpDateTo.Value.Date
		};
	}

	private bool IsValidInputValues(InputModel inputValues)
	{
		var inputValidationResults = _inputValidation.Validate(inputValues);

		if (inputValidationResults.IsValid)
		{
			return true;
		}
		else
		{
			var message = string.Join("\n", inputValidationResults.Errors.Select(q => q.ErrorMessage));
			MessageBox.Show(message, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return false;
		}
	}

	private void SaveXLSX_Click(object sender, EventArgs e)
	{
		try
		{
			// Preconfigured file path and file name and filter
			SaveFileDialog saveFileDialog = new SaveFileDialog
			{
				InitialDirectory = _excelSpreadsheetReportProvider.ExcelReportDefaultSavePath,
				FileName = _excelSpreadsheetReportProvider.ExcelReportFileName,
				Filter = _excelSpreadsheetReportProvider.ExcelReportSaveDialogFilter,
			};

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				_excelSpreadsheetReportProvider.SaveExcelReport(saveFileDialog.FileName, ReportSpreadsheetLines);
				MessageBox.Show("Excel report saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				return;
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "SaveXLSX_Click Button threw an unhandled exception.");
			ShowUnhandledExceptionPopup();
		}
	}

	private static void ShowUnhandledExceptionPopup()
	{
		MessageBox.Show("An unhandled exception was thrown, more information in log file.", "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
	}
}
