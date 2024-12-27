using TeamUpWebScraperLibrary.Logging;
using TeamUpWebScraperLibrary.TeamUpAPI;
using TeamUpWebScraperLibrary.Validators;
using TeamUpWebScraperUI.Constants;

namespace TeamUpWebScraperUI;

public partial class Dashboard : Form
{
	private readonly ILoggerAdapter<Dashboard> _logger;
	private readonly InputValidation _inputValidation;
	private readonly ITeamUpAPIService _teamUpAPIService;

	public Dashboard(
		ILoggerAdapter<Dashboard> logger,
		InputValidation inputValidation,
		ITeamUpAPIService teamUpAPIService)
	{
		_logger = logger;
		_inputValidation = inputValidation;
		_teamUpAPIService = teamUpAPIService;

		InitializeComponent();
		DisplayVersion();
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
			var dateFromValue = dtpDateFrom.Value.Date;
			var dateToValue = dtpDateTo.Value.Date;
			if (!IsValidDatesSpan(dateFromValue, dateToValue))
			{
				return;
			}

			var eventsRouteResponse = await _teamUpAPIService.GetEventsAsync(dateFromValue, dateToValue);
			if (eventsRouteResponse.IsError)
			{
				var firstError = eventsRouteResponse.FirstError;
				MessageBox.Show(firstError.Description, firstError.Code, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var eventsList = eventsRouteResponse.Value.Events;

			// TODO: Logic here
			// [OK] 1. write tests with _teamUpAPIService mocked with n substitute using provided json file
			// 2. write logic that transforms the data
			// [OK] 3. Model for Excel Table
			// 4. Excel spreadsheet report provider

		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "CallAPI_Click Button threw an unhandled exception.");
			MessageBox.Show("An unhandled exception was thrown, more information in log file.", "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}

	private bool IsValidDatesSpan(DateTime dateFromValue, DateTime dateToValue)
	{
		var datesSpanValidationResults = _inputValidation.ValidateDatesRange(dateFromValue, dateToValue);
		if (datesSpanValidationResults.Any())
		{
			var validationsMessage = string.Join('\n', datesSpanValidationResults.ToArray());
			MessageBox.Show(validationsMessage, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return false;
		}

		return true;
	}
}
