namespace TeamUpWebScraperUI
{
	partial class Dashboard : Form
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			var resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
			apiLabel = new Label();
			callAPI = new Button();
			statusStrip = new StatusStrip();
			systemStatusLabel = new ToolStripStatusLabel();
			resultsLabel = new Label();
			label1 = new Label();
			dtpDateFrom = new DateTimePicker();
			dtpDateTo = new DateTimePicker();
			label2 = new Label();
			saveXLSX = new Button();
			dataGridViewResults = new DataGridView();
			cbSelectUnselectAllDisplayed = new CheckBox();
			cbMemberTimeAnalysis = new CheckBox();
			tbFilterByName = new TextBox();
			label3 = new Label();
			statusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewResults).BeginInit();
			SuspendLayout();
			// 
			// apiLabel
			// 
			apiLabel.AutoSize = true;
			apiLabel.Location = new Point(25, 81);
			apiLabel.Name = "apiLabel";
			apiLabel.Size = new Size(74, 32);
			apiLabel.TabIndex = 1;
			apiLabel.Text = "From:";
			// 
			// callAPI
			// 
			callAPI.Location = new Point(744, 78);
			callAPI.Name = "callAPI";
			callAPI.Size = new Size(110, 39);
			callAPI.TabIndex = 3;
			callAPI.Text = "Go";
			callAPI.UseVisualStyleBackColor = true;
			callAPI.Click += CallAPI_Click;
			// 
			// statusStrip
			// 
			statusStrip.Items.AddRange(new ToolStripItem[] { systemStatusLabel });
			statusStrip.Location = new Point(0, 647);
			statusStrip.Name = "statusStrip";
			statusStrip.Size = new Size(884, 30);
			statusStrip.TabIndex = 5;
			statusStrip.Text = "statusStrip1";
			// 
			// systemStatusLabel
			// 
			systemStatusLabel.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
			systemStatusLabel.Name = "systemStatusLabel";
			systemStatusLabel.Size = new Size(62, 25);
			systemStatusLabel.Text = "Ready";
			// 
			// resultsLabel
			// 
			resultsLabel.AutoSize = true;
			resultsLabel.Location = new Point(19, 155);
			resultsLabel.Name = "resultsLabel";
			resultsLabel.Size = new Size(88, 32);
			resultsLabel.TabIndex = 6;
			resultsLabel.Text = "Results";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(316, 81);
			label1.Name = "label1";
			label1.Size = new Size(44, 32);
			label1.TabIndex = 7;
			label1.Text = "To:";
			// 
			// dtpDateFrom
			// 
			dtpDateFrom.Format = DateTimePickerFormat.Custom;
			dtpDateFrom.Location = new Point(105, 81);
			dtpDateFrom.MaxDate = new DateTime(2300, 12, 31, 0, 0, 0, 0);
			dtpDateFrom.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
			dtpDateFrom.Name = "dtpDateFrom";
			dtpDateFrom.Size = new Size(172, 39);
			dtpDateFrom.TabIndex = 8;
			// 
			// dtpDateTo
			// 
			dtpDateTo.Format = DateTimePickerFormat.Custom;
			dtpDateTo.Location = new Point(379, 81);
			dtpDateTo.MaxDate = new DateTime(2300, 12, 31, 0, 0, 0, 0);
			dtpDateTo.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
			dtpDateTo.Name = "dtpDateTo";
			dtpDateTo.Size = new Size(172, 39);
			dtpDateTo.TabIndex = 9;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(25, 21);
			label2.Name = "label2";
			label2.Size = new Size(219, 32);
			label2.TabIndex = 10;
			label2.Text = "Select Dates Range";
			// 
			// saveXLSX
			// 
			saveXLSX.Enabled = false;
			saveXLSX.Location = new Point(744, 141);
			saveXLSX.Name = "saveXLSX";
			saveXLSX.Size = new Size(110, 76);
			saveXLSX.TabIndex = 11;
			saveXLSX.Text = "Save .xlsx";
			saveXLSX.UseVisualStyleBackColor = true;
			saveXLSX.Click += SaveXLSX_Click;
			// 
			// dataGridViewResults
			// 
			dataGridViewResults.AllowUserToAddRows = false;
			dataGridViewResults.AllowUserToDeleteRows = false;
			dataGridViewResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewResults.Location = new Point(25, 232);
			dataGridViewResults.Name = "dataGridViewResults";
			dataGridViewResults.ReadOnly = true;
			dataGridViewResults.Size = new Size(829, 403);
			dataGridViewResults.TabIndex = 12;
			// 
			// cbSelectUnselectAllDisplayed
			// 
			cbSelectUnselectAllDisplayed.AutoSize = true;
			cbSelectUnselectAllDisplayed.Location = new Point(25, 190);
			cbSelectUnselectAllDisplayed.Name = "cbSelectUnselectAllDisplayed";
			cbSelectUnselectAllDisplayed.Size = new Size(231, 36);
			cbSelectUnselectAllDisplayed.TabIndex = 13;
			cbSelectUnselectAllDisplayed.Text = "Select/Deselect All";
			cbSelectUnselectAllDisplayed.UseVisualStyleBackColor = true;
			cbSelectUnselectAllDisplayed.CheckedChanged += cbSelectUnselectAllDisplayed_CheckedChanged;
			// 
			// cbMemberTimeAnalysis
			// 
			cbMemberTimeAnalysis.AutoSize = true;
			cbMemberTimeAnalysis.Location = new Point(448, 141);
			cbMemberTimeAnalysis.Name = "cbMemberTimeAnalysis";
			cbMemberTimeAnalysis.Size = new Size(292, 36);
			cbMemberTimeAnalysis.TabIndex = 14;
			cbMemberTimeAnalysis.Text = "Member's Time Analysis";
			cbMemberTimeAnalysis.UseVisualStyleBackColor = true;
			// 
			// tbFilterByName
			// 
			tbFilterByName.Location = new Point(454, 187);
			tbFilterByName.Name = "tbFilterByName";
			tbFilterByName.Size = new Size(284, 39);
			tbFilterByName.TabIndex = 15;
			tbFilterByName.TextChanged += tbFilterByName_TextChanged;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(381, 190);
			label3.Name = "label3";
			label3.Size = new Size(67, 32);
			label3.TabIndex = 16;
			label3.Text = "Filter";
			// 
			// Dashboard
			// 
			AutoScaleDimensions = new SizeF(13F, 32F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(884, 677);
			Controls.Add(label3);
			Controls.Add(tbFilterByName);
			Controls.Add(cbMemberTimeAnalysis);
			Controls.Add(cbSelectUnselectAllDisplayed);
			Controls.Add(dataGridViewResults);
			Controls.Add(saveXLSX);
			Controls.Add(label2);
			Controls.Add(dtpDateTo);
			Controls.Add(dtpDateFrom);
			Controls.Add(label1);
			Controls.Add(resultsLabel);
			Controls.Add(statusStrip);
			Controls.Add(callAPI);
			Controls.Add(apiLabel);
			Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(6);
			Name = "Dashboard";
			Text = "TeamUp Time Parser";
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewResults).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private Label apiLabel;
		private Button callAPI;
		private StatusStrip statusStrip;
		private Label resultsLabel;
		private ToolStripStatusLabel systemStatusLabel;
		private Label label1;
		private DateTimePicker dtpDateFrom;
		private DateTimePicker dtpDateTo;
		private Label label2;
		private Button saveXLSX;
		private DataGridView dataGridViewResults;
		private CheckBox cbSelectUnselectAllDisplayed;
		private CheckBox cbMemberTimeAnalysis;
		private TextBox tbFilterByName;
		private Label label3;
	}
}
