using DVLD_Buisness;
using DVLD_Project.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Apps.Application_Types
{
    public partial class frmEditApplicationType : Form
    {
        int _AppTypeID = -1;
        clsApplicationType _ApplicationType;
        public frmEditApplicationType(int AppTypeID)
        {
            InitializeComponent();
            _AppTypeID = AppTypeID;
        }
        void SetData()
        {
            lblApplicationTypeID.Text = _ApplicationType.ApplicationTypeID.ToString();
            txtTitle.Text = _ApplicationType.ApplicationTypeTitle.ToString();
            txtFees.Text = _ApplicationType.ApplicationFees.ToString();
        }
        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            _ApplicationType = clsApplicationType.GetApplicationTypeInfoByID(_AppTypeID);
            if (_ApplicationType != null)
            {
                SetData();
            }
            else
            {
                MessageBox.Show("No Application Type With This ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if(txtTitle.Text.Trim() == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtTitle, "");
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (txtFees.Text.Trim() == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Required");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFees, "");
            }

            if(!clsValidation.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Invalid Number");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFees, "");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _ApplicationType.ApplicationTypeTitle = txtTitle.Text;
            _ApplicationType.ApplicationFees = float.Parse(txtFees.Text);

            if (_ApplicationType.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
