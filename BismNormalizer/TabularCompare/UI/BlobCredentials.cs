﻿using System;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Windows.Forms;

namespace BismNormalizer.TabularCompare.UI
{
    public partial class BlobCredentials : Form
    {
        private string _authenticationKind;
        private string _connectionName;
        private string _accountKey;
        private string _privacyLevel;
        private float _dpiScaleFactor;

        public string AuthenticationKind
        {
            get { return _authenticationKind; }
            set { _authenticationKind = value; }
        }
        public string ConnectionName
        {
            get { return _connectionName; }
            set { _connectionName = value; }
        }
        public string AccountKey
        {
            get { return _accountKey; }
            set { _accountKey = value; }
        }
        public string PrivacyLevel
        {
            get { return _privacyLevel; }
            set { _privacyLevel = value; }
        }
        public float DpiScaleFactor
        {
            get { return _dpiScaleFactor; }
            set { _dpiScaleFactor = value; }
        }

        public BlobCredentials()
        {
            InitializeComponent();
        }

        private void BlobCredentials_Load(object sender, EventArgs e)
        {
            if (_dpiScaleFactor > 1)
            {
                //DPI
                float dpiScaleFactorFudged = _dpiScaleFactor * HighDPIUtils.PrimaryFudgeFactor;

                this.Scale(new SizeF(dpiScaleFactorFudged * 0.44f, dpiScaleFactorFudged * 0.38f));
                this.Width = Convert.ToInt32(this.Width * dpiScaleFactorFudged * 0.6f);
                foreach (Control control in HighDPIUtils.GetChildInControl(this))
                {
                    control.Font = new Font(control.Font.FontFamily,
                                      control.Font.Size * dpiScaleFactorFudged * HighDPIUtils.PrimaryFudgeFactor,
                                      control.Font.Style);
                }
            }

            this.KeyPreview = true;

            txtConnectionName.Text = _connectionName;
            txtAccountKey.Text = _accountKey;
            cboPrivacyLevel.Text = "None";

            this.ActiveControl = txtAccountKey;
        }

        private void ImpersonationCredentials_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                //User Cancelling, so do nothing
                return;
            }
            _accountKey = txtAccountKey.Text;
            _privacyLevel = cboPrivacyLevel.Text;
        }
    }
}
