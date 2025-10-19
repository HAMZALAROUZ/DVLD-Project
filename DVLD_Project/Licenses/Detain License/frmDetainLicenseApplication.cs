using DVLD_Buisness;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses.Locale_License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.Detain_License
{
    public partial class frmDetainLicenseApplication : Form
    {        
        public frmDetainLicenseApplication()
        {
            InitializeComponent();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int _PersonID = obj;

            if (_PersonID == -1)
                return;

            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true;

            //when license already detained
            if(ctrlDriverLicenseInfoWithFilter1.LicenseInfo.IsDetained)
            {
                MessageBox.Show("Error : This License Is Already Detained","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                btnDetain.Enabled = false;
                return;
            }

            btnDetain.Enabled = true;

            lblDetainDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("You Miss Some Data To Set!","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are You Sure You Want To Detaine This License?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;

            float Fees = int.Parse(txtFineFees.Text.Trim());

            int DetaineID = ctrlDriverLicenseInfoWithFilter1.LicenseInfo.Detain(Fees, clsGlobal.CurrentUser.UserID);

            if(DetaineID == -1)
            {
                MessageBox.Show("Error : Detaine License Failed","Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
                return;
            }

            ctrlDriverLicenseInfoWithFilter1.FilterEnable = false;
            btnDetain.Enabled = false;
            txtFineFees.Enabled = false;

            lblDetainID.Text = DetaineID.ToString();
            lblLicenseID.Text = ctrlDriverLicenseInfoWithFilter1.LicenseInfo.LicenseID.ToString();
        }

        private void frmDetainLicenseApplication_Load(object sender, EventArgs e)
        {
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
            btnDetain.Enabled = false;
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(ctrlDriverLicenseInfoWithFilter1.LicenseInfo.LicenseID);
            frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.LicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtFineFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Required");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFineFees, "");
            }

            if(!clsValidation.IsNumber(txtFineFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Invalid Number");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFineFees, "");
            }
        }
    }
}
