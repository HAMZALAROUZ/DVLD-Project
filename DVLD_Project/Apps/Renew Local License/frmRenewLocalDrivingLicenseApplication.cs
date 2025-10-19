using DVLD_Buisness;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses;
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

namespace DVLD_Project.Apps.Renew_Local_License
{
    public partial class frmRenewLocalDrivingLicenseApplication : Form
    {
        private clsLicense _License;
        public frmRenewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }
        private void DefaultValues()
        {
            lblApplicationID.Text = "[???]";
            lblApplicationDate.Text = "[??/??/????]";
            lblIssueDate.Text = "[??/??/????]";
            lblApplicationFees.Text = "[$$$]";
            lblLicenseFees.Text = "[$$$]";
            txtNotes.Text = "";
            lblRenewedLicenseID.Text = "[???]";
            lblOldLicenseID.Text = "[???]";
            lblExpirationDate.Text = "[??/??/????]";
            lblCreatedByUser.Text = "[????]";
            lblTotalFees.Text = "[$$$]";
        }
        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _License = clsLicense.Find(obj);
            DefaultValues();

            if (_License != null)
            {
                if (!_License.IsLicenseExpired())
                {
                    MessageBox.Show($"Error : This License Is Not Expaired Yet, Expaired In " +
                        $"{_License.ExpirationDate.ToShortDateString()}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    btnRenewLicense.Enabled = false;
                    llShowLicenseHistory.Enabled = true;
                    llShowLicenseInfo.Enabled = false;
                    return;
                }

                if (!_License.IsActive)
                {
                    MessageBox.Show("Error : You Can't Renew This License, It's Not Active!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    btnRenewLicense.Enabled = false;
                    llShowLicenseHistory.Enabled = false;
                    llShowLicenseInfo.Enabled = false;
                    return;
                }
            }

            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblApplicationFees.Text = clsApplicationType.FindApplicationTypeInfoByType(clsApplication.enApplicationType.RenewDrivingLicense).ApplicationFees.ToString();
            lblLicenseFees.Text = _License.LicenseClassInfo.ClassFees.ToString();
            lblOldLicenseID.Text = ctrlDriverLicenseInfoWithFilter1.LicenseID.ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(clsLicenseClass.Find(_License.LicenseClassInfo.LicenseClassID).DefaultValidityLength).ToShortDateString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            lblTotalFees.Text = (Convert.ToInt32(lblApplicationFees.Text) + Convert.ToInt32(lblLicenseFees.Text)).ToString();

            btnRenewLicense.Enabled = true;
            llShowLicenseHistory.Enabled = true;


        }

        private void btnRenewLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Renew The License?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;


            clsApplication _application = new clsApplication();


            _application.ApplicantPersonID = _License.DriverInfo.PersonID;
            _application.ApplicationDate = DateTime.Now;
            _application.ApplicationTypeID = (int)clsApplication.enApplicationType.RenewDrivingLicense;
            _application.LastStatusDate = DateTime.Now;
            _application.PaidFees = clsApplicationType.FindApplicationTypeInfoByType(clsApplication.enApplicationType.RenewDrivingLicense).ApplicationFees;
            _application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (!_application.Save())
            {
                MessageBox.Show("Error : Failed To Add Renew Application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsLicense _NewLicense = new clsLicense();

            _NewLicense = _License.RenewLicense(txtNotes.Text.Trim(), clsGlobal.CurrentUser.UserID);

            _NewLicense.ApplicationID = _application.ApplicationID;


            if (_NewLicense.Save())
            {
                MessageBox.Show("License Renewed", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                _License.IsActive = false;
                _License = _NewLicense;
            }
            else
            {
                MessageBox.Show("Error : License Failed To Renew", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _License.IsActive = true;

            btnRenewLicense.Enabled = false;
            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true;

            lblApplicationID.Text = _License.ApplicationID.ToString();
            lblRenewedLicenseID.Text = _License.LicenseID.ToString();

        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_License.LicenseID);
            frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(_License.DriverInfo.PersonID);
            frm.ShowDialog();
        }
    }
}
