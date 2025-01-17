using TeamUpWebScraperLibrary;
using TeamUpWebScraperLibrary.TeamUpAPI.Models.Input;
using TeamUpWebScraperUI.Constants;
using TeamUpWebScraperUI.DisplayDataGridGeneration;

namespace TeamUpWebScraperUI;

public partial class Dashboard : Form
{
	private readonly TeamUpController _teamUpController;

	public Dashboard(
		TeamUpController teamUpController)
	{
		_teamUpController = teamUpController;

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

			// Get Input values into a model
			var inputValues = GetInputIntoModel();
			if (!IsValidInputValues(inputValues))
			{
				return;
			}

			var result = await _teamUpController.CallTeamUpAPI(inputValues);

			if (result.IsValid)
			{
				var displayResults = _teamUpController.GetDisplayableGridResults();
				resultsLabel.Text = string.Format(DashBoardConstants.RESULTS_LABEL_WITH_RESULTS, displayResults.Count);
				DataGridViewHelper.GenerateDataGridView(dataGridViewResults, displayResults);
				saveXLSX.Enabled = true;
			}
			else
			{
				MessageBox.Show(result.ErrorMessage, result.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
		}
		catch (Exception)
		{
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
		var result = _teamUpController.IsValidInputValues(inputValues);

		if (result.IsValid)
		{
			return true;
		}
		else
		{
			MessageBox.Show(result.ErrorMessage, result.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return false;
		}
	}

	private void SaveXLSX_Click(object sender, EventArgs e)
	{
		try
		{
			// Preconfigured file path and file name and filter
			var (defaultSavePath, fileName, filter) = _teamUpController.GetSaveDialogOptions();
			SaveFileDialog saveFileDialog = new SaveFileDialog
			{
				InitialDirectory = defaultSavePath,
				FileName = fileName,
				Filter = filter,
			};

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				_teamUpController.SaveXLSX(saveFileDialog.FileName);
				MessageBox.Show("Excel report saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				return;
			}
		}
		catch (Exception)
		{
			ShowUnhandledExceptionPopup();
		}
	}

	private static void ShowUnhandledExceptionPopup()
	{
		MessageBox.Show("An unhandled exception was thrown, more information in log file.", "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
	}
}
