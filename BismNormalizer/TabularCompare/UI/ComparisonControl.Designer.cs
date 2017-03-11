namespace BismNormalizer.TabularCompare.UI
{
    partial class ComparisonControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComparisonControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.spltSourceTarget = new System.Windows.Forms.SplitContainer();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnCompareTabularModels = new System.Windows.Forms.ToolStripButton();
            this.ddSelectActions = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuHideSkipObjects = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowSkipObjects = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSkipAllObjectsMissingInSource = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteAllObjectsMissingInSource = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSkipAllObjectsMissingInTarget = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCreateAllObjectsMissingInTarget = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSkipAllObjectsWithDifferentDefinitions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdateAllObjectsWithDifferentDefinitions = new System.Windows.Forms.ToolStripMenuItem();
            this.btnValidateSelection = new System.Windows.Forms.ToolStripButton();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.btnGenerateScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOptions = new System.Windows.Forms.ToolStripButton();
            this.btnReportDifferences = new System.Windows.Forms.ToolStripButton();
            this.scDifferenceResults = new System.Windows.Forms.SplitContainer();
            this.pnlProgressBar = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgressBar = new System.Windows.Forms.Label();
            this.treeGridComparisonResults = new BismNormalizer.TabularCompare.UI.TreeGridViewComparison();
            this.TreeGridImageList = new System.Windows.Forms.ImageList(this.components);
            this.scObjectDefinitions = new System.Windows.Forms.SplitContainer();
            this.txtSourceObjectDefinition = new BismNormalizer.TabularCompare.UI.SynchronizedScrollRichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTargetObjectDefinition = new BismNormalizer.TabularCompare.UI.SynchronizedScrollRichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.spltSourceTarget)).BeginInit();
            this.spltSourceTarget.Panel1.SuspendLayout();
            this.spltSourceTarget.Panel2.SuspendLayout();
            this.spltSourceTarget.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scDifferenceResults)).BeginInit();
            this.scDifferenceResults.Panel1.SuspendLayout();
            this.scDifferenceResults.Panel2.SuspendLayout();
            this.scDifferenceResults.SuspendLayout();
            this.pnlProgressBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeGridComparisonResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scObjectDefinitions)).BeginInit();
            this.scObjectDefinitions.Panel1.SuspendLayout();
            this.scObjectDefinitions.Panel2.SuspendLayout();
            this.scObjectDefinitions.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 25);
            this.label1.TabIndex = 39;
            this.label1.Text = "Source";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 25);
            this.label2.TabIndex = 40;
            this.label2.Text = "Target";
            // 
            // txtSource
            // 
            this.txtSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSource.BackColor = System.Drawing.SystemColors.Control;
            this.txtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSource.Location = new System.Drawing.Point(98, 21);
            this.txtSource.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(536, 31);
            this.txtSource.TabIndex = 41;
            this.txtSource.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // txtTarget
            // 
            this.txtTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTarget.BackColor = System.Drawing.SystemColors.Control;
            this.txtTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTarget.Location = new System.Drawing.Point(90, 21);
            this.txtTarget.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(558, 31);
            this.txtTarget.TabIndex = 42;
            this.txtTarget.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // spltSourceTarget
            // 
            this.spltSourceTarget.Dock = System.Windows.Forms.DockStyle.Top;
            this.spltSourceTarget.IsSplitterFixed = true;
            this.spltSourceTarget.Location = new System.Drawing.Point(0, 39);
            this.spltSourceTarget.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.spltSourceTarget.Name = "spltSourceTarget";
            // 
            // spltSourceTarget.Panel1
            // 
            this.spltSourceTarget.Panel1.Controls.Add(this.label1);
            this.spltSourceTarget.Panel1.Controls.Add(this.txtSource);
            // 
            // spltSourceTarget.Panel2
            // 
            this.spltSourceTarget.Panel2.Controls.Add(this.txtTarget);
            this.spltSourceTarget.Panel2.Controls.Add(this.label2);
            this.spltSourceTarget.Size = new System.Drawing.Size(1306, 57);
            this.spltSourceTarget.SplitterDistance = 642;
            this.spltSourceTarget.SplitterWidth = 8;
            this.spltSourceTarget.TabIndex = 45;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHeader.Controls.Add(this.spltSourceTarget);
            this.pnlHeader.Controls.Add(this.toolStrip1);
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1306, 108);
            this.pnlHeader.TabIndex = 46;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCompareTabularModels,
            this.ddSelectActions,
            this.btnValidateSelection,
            this.btnUpdate,
            this.btnGenerateScript,
            this.toolStripButton1,
            this.btnOptions,
            this.btnReportDifferences});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1306, 39);
            this.toolStrip1.TabIndex = 46;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnCompareTabularModels
            // 
            this.btnCompareTabularModels.Image = ((System.Drawing.Image)(resources.GetObject("btnCompareTabularModels.Image")));
            this.btnCompareTabularModels.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCompareTabularModels.Name = "btnCompareTabularModels";
            this.btnCompareTabularModels.Size = new System.Drawing.Size(163, 36);
            this.btnCompareTabularModels.Text = "Compare...";
            this.btnCompareTabularModels.ToolTipText = "Compare (Shift+Alt+C)";
            this.btnCompareTabularModels.Click += new System.EventHandler(this.btnCompareTabularModels_Click);
            // 
            // ddSelectActions
            // 
            this.ddSelectActions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHideSkipObjects,
            this.mnuShowSkipObjects,
            this.toolStripSeparator1,
            this.mnuSkipAllObjectsMissingInSource,
            this.mnuDeleteAllObjectsMissingInSource,
            this.mnuSkipAllObjectsMissingInTarget,
            this.mnuCreateAllObjectsMissingInTarget,
            this.mnuSkipAllObjectsWithDifferentDefinitions,
            this.mnuUpdateAllObjectsWithDifferentDefinitions});
            this.ddSelectActions.Enabled = false;
            this.ddSelectActions.Image = ((System.Drawing.Image)(resources.GetObject("ddSelectActions.Image")));
            this.ddSelectActions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddSelectActions.Name = "ddSelectActions";
            this.ddSelectActions.Size = new System.Drawing.Size(218, 36);
            this.ddSelectActions.Text = "Select Actions";
            // 
            // mnuHideSkipObjects
            // 
            this.mnuHideSkipObjects.Name = "mnuHideSkipObjects";
            this.mnuHideSkipObjects.Size = new System.Drawing.Size(583, 38);
            this.mnuHideSkipObjects.Text = "Hide Skip Objects";
            this.mnuHideSkipObjects.Click += new System.EventHandler(this.mnuHideSkipObjects_Click);
            // 
            // mnuShowSkipObjects
            // 
            this.mnuShowSkipObjects.Name = "mnuShowSkipObjects";
            this.mnuShowSkipObjects.Size = new System.Drawing.Size(583, 38);
            this.mnuShowSkipObjects.Text = "Show Skip Objects";
            this.mnuShowSkipObjects.Click += new System.EventHandler(this.mnuShowSkipObjects_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(580, 6);
            // 
            // mnuSkipAllObjectsMissingInSource
            // 
            this.mnuSkipAllObjectsMissingInSource.Name = "mnuSkipAllObjectsMissingInSource";
            this.mnuSkipAllObjectsMissingInSource.Size = new System.Drawing.Size(583, 38);
            this.mnuSkipAllObjectsMissingInSource.Text = "Skip all objects Missing in Source";
            this.mnuSkipAllObjectsMissingInSource.Click += new System.EventHandler(this.mnuSkipAllObjectsMissingInSource_Click);
            // 
            // mnuDeleteAllObjectsMissingInSource
            // 
            this.mnuDeleteAllObjectsMissingInSource.Name = "mnuDeleteAllObjectsMissingInSource";
            this.mnuDeleteAllObjectsMissingInSource.Size = new System.Drawing.Size(583, 38);
            this.mnuDeleteAllObjectsMissingInSource.Text = "Delete all objects Missing in Source";
            this.mnuDeleteAllObjectsMissingInSource.Click += new System.EventHandler(this.mnuDeleteAllObjectsMissingInSource_Click);
            // 
            // mnuSkipAllObjectsMissingInTarget
            // 
            this.mnuSkipAllObjectsMissingInTarget.Name = "mnuSkipAllObjectsMissingInTarget";
            this.mnuSkipAllObjectsMissingInTarget.Size = new System.Drawing.Size(583, 38);
            this.mnuSkipAllObjectsMissingInTarget.Text = "Skip all objects Missing in Target";
            this.mnuSkipAllObjectsMissingInTarget.Click += new System.EventHandler(this.mnuSkipAllObjectsMissingInTarget_Click);
            // 
            // mnuCreateAllObjectsMissingInTarget
            // 
            this.mnuCreateAllObjectsMissingInTarget.Name = "mnuCreateAllObjectsMissingInTarget";
            this.mnuCreateAllObjectsMissingInTarget.Size = new System.Drawing.Size(583, 38);
            this.mnuCreateAllObjectsMissingInTarget.Text = "Create all objects Missing in Target";
            this.mnuCreateAllObjectsMissingInTarget.Click += new System.EventHandler(this.mnuCreateAllObjectsMissingInTarget_Click);
            // 
            // mnuSkipAllObjectsWithDifferentDefinitions
            // 
            this.mnuSkipAllObjectsWithDifferentDefinitions.Name = "mnuSkipAllObjectsWithDifferentDefinitions";
            this.mnuSkipAllObjectsWithDifferentDefinitions.Size = new System.Drawing.Size(583, 38);
            this.mnuSkipAllObjectsWithDifferentDefinitions.Text = "Skip all objects with Different Definitions";
            this.mnuSkipAllObjectsWithDifferentDefinitions.Click += new System.EventHandler(this.mnuSkipAllObjectsWithDifferentDefinitions_Click);
            // 
            // mnuUpdateAllObjectsWithDifferentDefinitions
            // 
            this.mnuUpdateAllObjectsWithDifferentDefinitions.Name = "mnuUpdateAllObjectsWithDifferentDefinitions";
            this.mnuUpdateAllObjectsWithDifferentDefinitions.Size = new System.Drawing.Size(583, 38);
            this.mnuUpdateAllObjectsWithDifferentDefinitions.Text = "Update all objects with Different Definitions";
            this.mnuUpdateAllObjectsWithDifferentDefinitions.Click += new System.EventHandler(this.mnuUpdateAllObjectsWithDifferentDefinitions_Click);
            // 
            // btnValidateSelection
            // 
            this.btnValidateSelection.Enabled = false;
            this.btnValidateSelection.Image = ((System.Drawing.Image)(resources.GetObject("btnValidateSelection.Image")));
            this.btnValidateSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnValidateSelection.Name = "btnValidateSelection";
            this.btnValidateSelection.Size = new System.Drawing.Size(240, 36);
            this.btnValidateSelection.Text = "Validate Selection";
            this.btnValidateSelection.Click += new System.EventHandler(this.btnValidateSelection_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(128, 36);
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnGenerateScript
            // 
            this.btnGenerateScript.Enabled = false;
            this.btnGenerateScript.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerateScript.Image")));
            this.btnGenerateScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGenerateScript.Name = "btnGenerateScript";
            this.btnGenerateScript.Size = new System.Drawing.Size(215, 36);
            this.btnGenerateScript.Text = "Generate Script";
            this.btnGenerateScript.Click += new System.EventHandler(this.btnGenerateScript_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(6, 39);
            // 
            // btnOptions
            // 
            this.btnOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnOptions.Image")));
            this.btnOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(135, 36);
            this.btnOptions.Text = "Options";
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // btnReportDifferences
            // 
            this.btnReportDifferences.Enabled = false;
            this.btnReportDifferences.Image = ((System.Drawing.Image)(resources.GetObject("btnReportDifferences.Image")));
            this.btnReportDifferences.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReportDifferences.Name = "btnReportDifferences";
            this.btnReportDifferences.Size = new System.Drawing.Size(249, 36);
            this.btnReportDifferences.Text = "Report Differences";
            this.btnReportDifferences.Click += new System.EventHandler(this.btnReportDifferences_Click);
            // 
            // scDifferenceResults
            // 
            this.scDifferenceResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scDifferenceResults.Location = new System.Drawing.Point(0, 108);
            this.scDifferenceResults.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.scDifferenceResults.Name = "scDifferenceResults";
            this.scDifferenceResults.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scDifferenceResults.Panel1
            // 
            this.scDifferenceResults.Panel1.Controls.Add(this.pnlProgressBar);
            this.scDifferenceResults.Panel1.Controls.Add(this.treeGridComparisonResults);
            // 
            // scDifferenceResults.Panel2
            // 
            this.scDifferenceResults.Panel2.Controls.Add(this.scObjectDefinitions);
            this.scDifferenceResults.Size = new System.Drawing.Size(1306, 975);
            this.scDifferenceResults.SplitterDistance = 590;
            this.scDifferenceResults.SplitterWidth = 8;
            this.scDifferenceResults.TabIndex = 2;
            // 
            // pnlProgressBar
            // 
            this.pnlProgressBar.BackColor = System.Drawing.SystemColors.Control;
            this.pnlProgressBar.Controls.Add(this.progressBar);
            this.pnlProgressBar.Controls.Add(this.lblProgressBar);
            this.pnlProgressBar.Location = new System.Drawing.Point(120, 40);
            this.pnlProgressBar.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlProgressBar.Name = "pnlProgressBar";
            this.pnlProgressBar.Size = new System.Drawing.Size(560, 104);
            this.pnlProgressBar.TabIndex = 1;
            this.pnlProgressBar.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(240, 31);
            this.progressBar.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(274, 35);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 1;
            // 
            // lblProgressBar
            // 
            this.lblProgressBar.AutoSize = true;
            this.lblProgressBar.Location = new System.Drawing.Point(26, 35);
            this.lblProgressBar.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblProgressBar.Name = "lblProgressBar";
            this.lblProgressBar.Size = new System.Drawing.Size(204, 25);
            this.lblProgressBar.TabIndex = 0;
            this.lblProgressBar.Text = "Generating report ...";
            // 
            // treeGridComparisonResults
            // 
            this.treeGridComparisonResults.AllowUserToAddRows = false;
            this.treeGridComparisonResults.AllowUserToDeleteRows = false;
            this.treeGridComparisonResults.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.treeGridComparisonResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.treeGridComparisonResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.treeGridComparisonResults.Comparison = null;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.treeGridComparisonResults.DefaultCellStyle = dataGridViewCellStyle4;
            this.treeGridComparisonResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeGridComparisonResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.treeGridComparisonResults.ImageList = this.TreeGridImageList;
            this.treeGridComparisonResults.Location = new System.Drawing.Point(0, 0);
            this.treeGridComparisonResults.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.treeGridComparisonResults.Name = "treeGridComparisonResults";
            this.treeGridComparisonResults.RowHeadersVisible = false;
            this.treeGridComparisonResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.treeGridComparisonResults.Size = new System.Drawing.Size(1306, 590);
            this.treeGridComparisonResults.TabIndex = 0;
            this.treeGridComparisonResults.Unloading = false;
            this.treeGridComparisonResults.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeGridComparisonResults_MouseUp);
            // 
            // TreeGridImageList
            // 
            this.TreeGridImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TreeGridImageList.ImageStream")));
            this.TreeGridImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.TreeGridImageList.Images.SetKeyName(0, "Connection.png");
            this.TreeGridImageList.Images.SetKeyName(1, "Table.png");
            this.TreeGridImageList.Images.SetKeyName(2, "Relationship.png");
            this.TreeGridImageList.Images.SetKeyName(3, "BismMeasure.png");
            this.TreeGridImageList.Images.SetKeyName(4, "KPI.png");
            this.TreeGridImageList.Images.SetKeyName(5, "DeleteAction.png");
            this.TreeGridImageList.Images.SetKeyName(6, "UpdateAction.png");
            this.TreeGridImageList.Images.SetKeyName(7, "CreateAction.png");
            this.TreeGridImageList.Images.SetKeyName(8, "SkipAction.png");
            this.TreeGridImageList.Images.SetKeyName(9, "Plus.png");
            this.TreeGridImageList.Images.SetKeyName(10, "Minus.png");
            this.TreeGridImageList.Images.SetKeyName(11, "Informational.png");
            this.TreeGridImageList.Images.SetKeyName(12, "Warning.png");
            this.TreeGridImageList.Images.SetKeyName(13, "WarningToolWindow.png");
            this.TreeGridImageList.Images.SetKeyName(14, "Role.png");
            this.TreeGridImageList.Images.SetKeyName(15, "Perspective.png");
            this.TreeGridImageList.Images.SetKeyName(16, "Action.png");
            this.TreeGridImageList.Images.SetKeyName(17, "CompareBismModels_Small.png");
            this.TreeGridImageList.Images.SetKeyName(18, "DeleteActionGrey.png");
            this.TreeGridImageList.Images.SetKeyName(19, "SkipActionGrey.png");
            this.TreeGridImageList.Images.SetKeyName(20, "CreateActionGrey.png");
            this.TreeGridImageList.Images.SetKeyName(21, "Culture.png");
            // 
            // scObjectDefinitions
            // 
            this.scObjectDefinitions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scObjectDefinitions.Location = new System.Drawing.Point(0, 0);
            this.scObjectDefinitions.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.scObjectDefinitions.Name = "scObjectDefinitions";
            // 
            // scObjectDefinitions.Panel1
            // 
            this.scObjectDefinitions.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.scObjectDefinitions.Panel1.Controls.Add(this.txtSourceObjectDefinition);
            this.scObjectDefinitions.Panel1.Controls.Add(this.label4);
            // 
            // scObjectDefinitions.Panel2
            // 
            this.scObjectDefinitions.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.scObjectDefinitions.Panel2.Controls.Add(this.txtTargetObjectDefinition);
            this.scObjectDefinitions.Panel2.Controls.Add(this.label5);
            this.scObjectDefinitions.Size = new System.Drawing.Size(1306, 377);
            this.scObjectDefinitions.SplitterDistance = 656;
            this.scObjectDefinitions.SplitterWidth = 8;
            this.scObjectDefinitions.TabIndex = 0;
            // 
            // txtSourceObjectDefinition
            // 
            this.txtSourceObjectDefinition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceObjectDefinition.BackColor = System.Drawing.Color.White;
            this.txtSourceObjectDefinition.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSourceObjectDefinition.Location = new System.Drawing.Point(0, 38);
            this.txtSourceObjectDefinition.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtSourceObjectDefinition.Name = "txtSourceObjectDefinition";
            this.txtSourceObjectDefinition.ReadOnly = true;
            this.txtSourceObjectDefinition.Size = new System.Drawing.Size(652, 335);
            this.txtSourceObjectDefinition.TabIndex = 1;
            this.txtSourceObjectDefinition.Text = "";
            this.txtSourceObjectDefinition.WordWrap = false;
            this.txtSourceObjectDefinition.vScroll += new BismNormalizer.TabularCompare.UI.SynchronizedScrollRichTextBox.vScrollEventHandler(this.txtSourceObjectDefinition_vScroll);
            this.txtSourceObjectDefinition.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSourceObjectDefinition_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 8);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(244, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Source Object Definition";
            // 
            // txtTargetObjectDefinition
            // 
            this.txtTargetObjectDefinition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTargetObjectDefinition.BackColor = System.Drawing.Color.White;
            this.txtTargetObjectDefinition.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTargetObjectDefinition.Location = new System.Drawing.Point(0, 38);
            this.txtTargetObjectDefinition.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtTargetObjectDefinition.Name = "txtTargetObjectDefinition";
            this.txtTargetObjectDefinition.ReadOnly = true;
            this.txtTargetObjectDefinition.Size = new System.Drawing.Size(638, 335);
            this.txtTargetObjectDefinition.TabIndex = 2;
            this.txtTargetObjectDefinition.Text = "";
            this.txtTargetObjectDefinition.WordWrap = false;
            this.txtTargetObjectDefinition.vScroll += new BismNormalizer.TabularCompare.UI.SynchronizedScrollRichTextBox.vScrollEventHandler(this.txtTargetObjectDefinition_vScroll);
            this.txtTargetObjectDefinition.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtTargetObjectDefinition_KeyUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 8);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(238, 25);
            this.label5.TabIndex = 1;
            this.label5.Text = "Target Object Definition";
            // 
            // ComparisonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scDifferenceResults);
            this.Controls.Add(this.pnlHeader);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "ComparisonControl";
            this.Size = new System.Drawing.Size(1306, 1087);
            this.Load += new System.EventHandler(this.BismNormalizer_Load);
            this.spltSourceTarget.Panel1.ResumeLayout(false);
            this.spltSourceTarget.Panel1.PerformLayout();
            this.spltSourceTarget.Panel2.ResumeLayout(false);
            this.spltSourceTarget.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spltSourceTarget)).EndInit();
            this.spltSourceTarget.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.scDifferenceResults.Panel1.ResumeLayout(false);
            this.scDifferenceResults.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scDifferenceResults)).EndInit();
            this.scDifferenceResults.ResumeLayout(false);
            this.pnlProgressBar.ResumeLayout(false);
            this.pnlProgressBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeGridComparisonResults)).EndInit();
            this.scObjectDefinitions.Panel1.ResumeLayout(false);
            this.scObjectDefinitions.Panel1.PerformLayout();
            this.scObjectDefinitions.Panel2.ResumeLayout(false);
            this.scObjectDefinitions.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scObjectDefinitions)).EndInit();
            this.scObjectDefinitions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TreeGridViewComparison treeGridComparisonResults;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.SplitContainer scDifferenceResults;
        private System.Windows.Forms.SplitContainer scObjectDefinitions;
        private SynchronizedScrollRichTextBox txtSourceObjectDefinition;
        private System.Windows.Forms.Label label4;
        private SynchronizedScrollRichTextBox txtTargetObjectDefinition;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.SplitContainer spltSourceTarget;
        private System.Windows.Forms.Panel pnlProgressBar;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblProgressBar;
        private System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.ImageList TreeGridImageList;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnCompareTabularModels;
        private System.Windows.Forms.ToolStripDropDownButton ddSelectActions;
        private System.Windows.Forms.ToolStripMenuItem mnuHideSkipObjects;
        private System.Windows.Forms.ToolStripMenuItem mnuShowSkipObjects;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuSkipAllObjectsMissingInSource;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteAllObjectsMissingInSource;
        private System.Windows.Forms.ToolStripMenuItem mnuSkipAllObjectsMissingInTarget;
        private System.Windows.Forms.ToolStripMenuItem mnuCreateAllObjectsMissingInTarget;
        private System.Windows.Forms.ToolStripMenuItem mnuSkipAllObjectsWithDifferentDefinitions;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateAllObjectsWithDifferentDefinitions;
        private System.Windows.Forms.ToolStripButton btnValidateSelection;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.ToolStripButton btnGenerateScript;
        private System.Windows.Forms.ToolStripSeparator toolStripButton1;
        private System.Windows.Forms.ToolStripButton btnOptions;
        private System.Windows.Forms.ToolStripButton btnReportDifferences;
    }
}
