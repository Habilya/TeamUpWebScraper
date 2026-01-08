namespace TeamUpWebScraperUI.DisplayDataGridGeneration;

public class DataGridViewHelper
{
	public static BindingSource GenerateDataGridView<T>(
		DataGridView dataGridView,
		IList<T> data)
	{
		var bindingSource = new BindingSource();
		bindingSource.DataSource = data;

		dataGridView.Columns.Clear();
		dataGridView.AutoGenerateColumns = false;
		dataGridView.DataSource = bindingSource;

		var props = typeof(T).GetProperties();

		// Checkbox column FIRST
		dataGridView.Columns.Add(new DataGridViewCheckBoxColumn
		{
			Name = "",
			DataPropertyName = "Selected",
			Width = 30,
			ReadOnly = false
		});

		// Other properties
		foreach (var prop in props)
		{
			if (prop.Name == "Selected") continue;

			dataGridView.Columns.Add(new DataGridViewTextBoxColumn
			{
				Name = prop.Name,
				HeaderText = prop.Name,
				DataPropertyName = prop.Name,
				ReadOnly = true,
				AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
			});
		}

		DataGridViewStyling(dataGridView);
		return bindingSource;
	}

	private static void DataGridViewStyling(DataGridView dataGridView)
	{
		// Optionally, make the DataGridView AutoSize the columns for better presentation
		dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

		dataGridView.Columns["UniqueId"].Visible = false;

		dataGridView.AllowUserToResizeColumns = false;
		dataGridView.AllowUserToResizeRows = false;
		dataGridView.RowHeadersVisible = false;

		dataGridView.ReadOnly = false;
		dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;

	}

	/// <summary>
	/// Generates a DataGridView dynamically based on the list of objects.
	/// </summary>
	/// <param name="dataGridView">The DataGridView to populate.</param>
	/// <param name="data">The data list to bind to the DataGridView.</param>
	public static void GenerateDataGridView(DataGridView dataGridView, IEnumerable<object> data)
	{
		if (data == null)
			throw new ArgumentNullException(nameof(data));

		// Create columns based on the properties of the first item in the data list
		var dataType = data.GetType().GetGenericArguments()[0];  // Get the type of objects in the list
		var properties = dataType.GetProperties();

		// Clear any existing columns or rows
		dataGridView.Columns.Clear();
		dataGridView.Rows.Clear();

		// Create columns dynamically based on properties
		foreach (var prop in properties)
		{
			var column = new DataGridViewTextBoxColumn
			{
				Name = prop.Name,
				HeaderText = prop.Name,
				DataPropertyName = prop.Name
			};
			dataGridView.Columns.Add(column);
		}

		// Bind data to the DataGridView
		foreach (var item in data)
		{
			var row = new List<object>();

			foreach (var prop in properties)
			{
				row.Add(prop.GetValue(item) ?? "");
			}

			dataGridView.Rows.Add(row.ToArray());
		}

		DataGridViewStyling(dataGridView);
	}
}
