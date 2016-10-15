using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BismNormalizer.TabularCompare.UI
{
    public partial class ProcessingErrorMessage : Form
    {
        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        public ProcessingErrorMessage()
        {
            InitializeComponent();
        }

        private void ErrorMessage_Load(object sender, EventArgs e)
        {
            txtErrorMessage.Text = _errorMessage;

            //do not want the OK button selected (closes inadvertently)
            txtErrorMessage.Focus();
            txtErrorMessage.SelectionStart = 0;
            txtErrorMessage.SelectionLength = 0;
        }
    }
}
