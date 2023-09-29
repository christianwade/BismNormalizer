﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BismNormalizer.TabularCompare.Core;

namespace BismNormalizer.TabularCompare.UI
{
    public partial class Options : Form
    {
        private ComparisonInfo _comparisonInfo;
        private float _dpiScaleFactor;

        public Options()
        {
            InitializeComponent();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            this.Height = Convert.ToInt32(groupBox1.Height + groupBox2.Height + panel2.Height*2.1);
            //MessageBox.Show($"Scale factor: {_dpiScaleFactor}\nSecondary scale factor: {Utils.SecondaryFudgeFactor}\n\n" +
            //    $"Groupbox1 height: {groupBox1.Height}\nGroupbox2 height: {groupBox2.Height}\nPanel2 height:{panel2.Height}\n\nForm height: {this.Height}");

            if (_dpiScaleFactor > 1)
            {
                float dpiScaleFactorFudged = _dpiScaleFactor * Utils.PrimaryFudgeFactor;
                //DPI
                this.Scale(new SizeF(dpiScaleFactorFudged * (_dpiScaleFactor > 1.7 ? 1 : Utils.SecondaryFudgeFactor), dpiScaleFactorFudged * Utils.SecondaryFudgeFactor));
                this.Width = Convert.ToInt32(this.Width * dpiScaleFactorFudged);
                foreach (Control control in Utils.GetChildInControl(this)) //.OfType<Button>())
                {
                    if (control is GroupBox || control is Button)
                    {
                        control.Font = new Font(control.Font.FontFamily,
                                                control.Font.Size * dpiScaleFactorFudged * Utils.SecondaryFudgeFactor,
                                                control.Font.Style);
                    }
                }
                this.cboProcessingOption.Left = label1.Right + Convert.ToInt32(12 * dpiScaleFactorFudged);
                
                //this.Height = Convert.ToInt32(this.Height * _dpiScaleFactor * Utils.SecondaryFudgeFactor);
            }

            this.KeyPreview = true;
            chkMeasureDependencies.Text = "Display warnings for measure dependencies (DAX\nreference to missing measure/column)";

            chkPerspectives.Checked = _comparisonInfo.OptionsInfo.OptionPerspectives;
            chkMergePerspectives.Checked = _comparisonInfo.OptionsInfo.OptionMergePerspectives;
            chkCultures.Checked = _comparisonInfo.OptionsInfo.OptionCultures;
            chkMergeCultures.Checked = _comparisonInfo.OptionsInfo.OptionMergeCultures;
            chkRoles.Checked = _comparisonInfo.OptionsInfo.OptionRoles;
            chkRetainRoleMembers.Checked = _comparisonInfo.OptionsInfo.OptionRetainRoleMembers;
            chkPartitions.Checked = _comparisonInfo.OptionsInfo.OptionPartitions;
            chkLineageTag.Checked = _comparisonInfo.OptionsInfo.OptionLineageTag;
            chkRetainPartitions.Checked = _comparisonInfo.OptionsInfo.OptionRetainPartitions;
            chkRetainPolicyPartitions.Checked = _comparisonInfo.OptionsInfo.OptionRetainPolicyPartitions;
            chkRetainRefreshPolicy.Checked = _comparisonInfo.OptionsInfo.OptionRetainRefreshPolicy;
            chkRetainStorageMode.Checked = _comparisonInfo.OptionsInfo.OptionRetainStorageMode;
            chkMeasureDependencies.Checked = _comparisonInfo.OptionsInfo.OptionMeasureDependencies;
            string processingOption = _comparisonInfo.OptionsInfo.OptionProcessingOption.ToString();
            cboProcessingOption.Text = processingOption == "DoNotProcess" ? "Do Not Process" : processingOption;
            chkAffectedTables.Checked = _comparisonInfo.OptionsInfo.OptionAffectedTables;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _comparisonInfo.OptionsInfo.OptionPerspectives = chkPerspectives.Checked;
            _comparisonInfo.OptionsInfo.OptionMergePerspectives = chkMergePerspectives.Checked;
            _comparisonInfo.OptionsInfo.OptionCultures = chkCultures.Checked;
            _comparisonInfo.OptionsInfo.OptionMergeCultures = chkMergeCultures.Checked;
            _comparisonInfo.OptionsInfo.OptionRoles = chkRoles.Checked;
            _comparisonInfo.OptionsInfo.OptionRetainRoleMembers = chkRetainRoleMembers.Checked;
            _comparisonInfo.OptionsInfo.OptionActions = false;
            _comparisonInfo.OptionsInfo.OptionPartitions = chkPartitions.Checked;
            _comparisonInfo.OptionsInfo.OptionLineageTag = chkLineageTag.Checked;
            _comparisonInfo.OptionsInfo.OptionRetainPartitions = chkRetainPartitions.Checked;
            _comparisonInfo.OptionsInfo.OptionRetainPolicyPartitions = chkRetainPolicyPartitions.Checked;
            _comparisonInfo.OptionsInfo.OptionRetainRefreshPolicy = chkRetainRefreshPolicy.Checked;
            _comparisonInfo.OptionsInfo.OptionRetainStorageMode = chkRetainStorageMode.Checked;
            _comparisonInfo.OptionsInfo.OptionMeasureDependencies = chkMeasureDependencies.Checked;
            _comparisonInfo.OptionsInfo.OptionProcessingOption = (ProcessingOption)Enum.Parse(typeof(ProcessingOption), cboProcessingOption.Text.Replace(" ", ""));
            _comparisonInfo.OptionsInfo.OptionTransaction = false;
            _comparisonInfo.OptionsInfo.OptionAffectedTables = chkAffectedTables.Checked;

            _comparisonInfo.OptionsInfo.Save();
        }

        private void chkPerspectives_CheckedChanged(object sender, EventArgs e)
        {
            chkMergePerspectives.Enabled = chkPerspectives.Checked;
        }

        private void chkCultures_CheckedChanged(object sender, EventArgs e)
        {
            chkMergeCultures.Enabled = chkCultures.Checked;
        }

        private void chkRoles_CheckedChanged(object sender, EventArgs e)
        {
            chkRetainRoleMembers.Enabled = chkRoles.Checked;
        }

        private void ChkRetainPartitions_CheckedChanged(object sender, EventArgs e)
        {
            chkRetainPolicyPartitions.Enabled = chkRetainPartitions.Checked;
        }

        private void Options_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.D)
            {
                if (MessageBox.Show($"Are you sure you want to toggle 192 Device DPI from optimized for {(Settings.Default.OptionHighDpiLocal ? "local" : "Remote Desktop")} to {(Settings.Default.OptionHighDpiLocal ? "Remote Desktop" : "local")}?", Utils.AssemblyProduct, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Settings.Default.OptionHighDpiLocal = !Settings.Default.OptionHighDpiLocal;
                }
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.C)
            {
                if (MessageBox.Show($"Are you sure you want to {(Settings.Default.OptionCompositeModelsOverride ? "*disallow*" : "*allow*")} composite model comparisons on Analysis Services?", Utils.AssemblyProduct, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Settings.Default.OptionCompositeModelsOverride = !Settings.Default.OptionCompositeModelsOverride;
                }
            }
        }

        public ComparisonInfo ComparisonInfo
        {
            get { return _comparisonInfo; }
            set { _comparisonInfo = value; }
        }

        public float DpiScaleFactor
        {
            get { return _dpiScaleFactor; }
            set { _dpiScaleFactor = value; }
        }

        private void cboProcessingOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProcessingWarning.Visible = (cboProcessingOption.Text != "Recalc" && cboProcessingOption.Text != "Do Not Process");
        }
    }
}
