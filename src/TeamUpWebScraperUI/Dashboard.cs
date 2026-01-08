using TeamUpWebScraperLibrary;
using TeamUpWebScraperLibrary.DTO;
using TeamUpWebScraperLibrary.Helpers;
using TeamUpWebScraperUI.Constants;
using TeamUpWebScraperUI.DisplayDataGridGeneration;

namespace TeamUpWebScraperUI;

public partial class Dashboard : Form
{
	private readonly TeamUpController _teamUpController;

	private List<DisplayGridViewModel> _displayGridItems = new();
	private BindingSource _displayGridBindingSource = new();

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
		cbMemberTimeAnalysis.Enabled = false;
		cbSelectUnselectAllDisplayed.Enabled = false;
		tbFilterByName.Enabled = false;
		cbMemberTimeAnalysis.Checked = false;
		cbSelectUnselectAllDisplayed.Checked = false;
		tbFilterByName.Text = string.Empty;

		_displayGridItems = new();
		_displayGridBindingSource = new();

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
				resultsLabel.Text = string.Format(DashBoardConstants.RESULTS_LABEL_WITH_SELECTED_RESULTS, displayResults.Count);
				systemStatusLabel.Text = string.Format(DashBoardConstants.SYSTEM_STATUS_LABEL_WITH_RESULTS, displayResults.Count);
				_displayGridBindingSource = DataGridViewHelper.GenerateDataGridView(dataGridViewResults, displayResults);
				_displayGridItems = displayResults;
				saveXLSX.Enabled = true;
				cbMemberTimeAnalysis.Enabled = true;
				cbSelectUnselectAllDisplayed.Enabled = true;
				tbFilterByName.Enabled = true;

				// Defaults
				cbMemberTimeAnalysis.Checked = false;
				cbSelectUnselectAllDisplayed.Checked = true;
				tbFilterByName.Text = string.Empty;
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

	private InputViewModel GetInputIntoModel()
	{
		return new InputViewModel(dtpDateFrom.Value.Date, dtpDateTo.Value.Date);
	}

	private bool IsValidInputValues(InputViewModel inputValues)
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
			var isMemberTimeAnalysisIncluded = cbMemberTimeAnalysis.Checked;
			var selectedIds = GetListOfSelectedIds();
			if (!selectedIds.Any())
			{
				MessageBox.Show("No Items slected in the view.\n" +
					"Report is not generated.\n" +
					"Make sure list is not empty and use checkboxes.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				return;
			}

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
				_teamUpController.SaveXLSX(saveFileDialog.FileName, selectedIds, isMemberTimeAnalysisIncluded);
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

	private List<int> GetListOfSelectedIds()
	{
		if (_displayGridBindingSource.Count == 0)
		{
			return new List<int>();
		}

		var selectedIds = _displayGridBindingSource.List
			.Cast<DisplayGridViewModel>()
			.Where(item => item.Selected)
			.Select(item => item.UniqueId)
			.ToList();

		return selectedIds;
	}

	private static void ShowUnhandledExceptionPopup()
	{
		MessageBox.Show("An unhandled exception was thrown, more information in log file.", "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
	}

	private void cbSelectUnselectAllDisplayed_CheckedChanged(object sender, EventArgs e)
	{
		bool check = cbSelectUnselectAllDisplayed.Checked;

		// Only visible rows are in the BindingSource
		foreach (DisplayGridViewModel item in _displayGridBindingSource.List)
		{
			item.Selected = check;
		}

		_displayGridBindingSource.ResetBindings(false);
	}

	private void tbFilterByName_TextChanged(object sender, EventArgs e)
	{
		ApplyFilterToDataGridViewResults(tbFilterByName.Text);
	}

	private void ApplyFilterToDataGridViewResults(string filter)
	{
		// filter = filter?.Trim().ToLower();
		// Converting null literal or possible null value to non-nullable type.
		filter = (filter ?? string.Empty).Trim().ToLower();

		bool noFilter = string.IsNullOrEmpty(filter);

		// Update selection based on visibility
		foreach (var item in _displayGridItems)
		{
			bool visible = noFilter || IsTitleMatching(filter!, item);
			item.Selected = visible;
		}

		// Apply filter
		_displayGridBindingSource.DataSource = noFilter
			? _displayGridItems
			: _displayGridItems
				.Where(x => IsTitleMatching(filter!, x))
				.ToList();

		static bool IsTitleMatching(string filter, DisplayGridViewModel x)
		{
			return x.Title?.ToLower().Contains(filter) == true
				|| StringHelper.RemoveDiacritics(x.Title!)?.ToLower().Contains(filter) == true;
		}
	}
}
