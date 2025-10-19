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

namespace DVLD_Project.Licenses.Locale_License
{
    public partial class frmIssueDriverLicenseFirstTime : Form
    {
        private int _LocalDrivingLicenseApplicationID;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        public frmIssueDriverLicenseFirstTime(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            //ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(LocalDrivingLicenseApplicationID);
        }

        clsLicense _License = new clsLicense();
        clsDriver _Driver = new clsDriver();
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void FillLicenseInfo()
        {
            clsLicenseClass licenseClass = clsLicenseClass.Find(ctrlDrivingLicenseApplicationInfo1.SelectedLocalDrivingLicenseAppInfo.LicenseClassID);

            _License.ApplicationID = ctrlDrivingLicenseApplicationInfo1.SelectedLocalDrivingLicenseAppInfo.ApplicationID;
            _License.DriverID = _Driver.DriverID;
            _License.LicenseClass = ctrlDrivingLicenseApplicationInfo1.SelectedLocalDrivingLicenseAppInfo.LicenseClassID;
            _License.IssueDate = DateTime.Now;
            _License.ExpirationDate = _License.IssueDate.AddYears(licenseClass.DefaultValidityLength);
            _License.Notes = txtNotes.Text.Trim();
            _License.PaidFees = licenseClass.ClassFees;
            _License.IsActive = true;
            _License.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            int applicationTypeID = ctrlDrivingLicenseApplicationInfo1.SelectedLocalDrivingLicenseAppInfo.ApplicationTypeID;
            switch (applicationTypeID)
            {
                case 1:
                    _License.IssueReason = clsLicense.enIssueReason.FirstTime;
                    break;
                case 2:
                    _License.IssueReason = clsLicense.enIssueReason.Renew;
                    break;
                case 3:
                    _License.IssueReason = clsLicense.enIssueReason.LostReplacement;
                    break;
                case 4:
                    _License.IssueReason = clsLicense.enIssueReason.DamagedReplacement;
                    break;
                default:
                    throw new Exception($"Unknown ApplicationTypeID: {applicationTypeID}");
            }
        }

        void FillDriverInfo()
        {
            _Driver.PersonID = ctrlDrivingLicenseApplicationInfo1.SelectedLocalDrivingLicenseAppInfo.ApplicantPersonID;
            _Driver.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _Driver.CreatedDate = DateTime.Now;
        }
        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            //I have to add check if person is already a driver in the system
            /*
            FillDriverInfo();

            if (!_Driver.Save())
            {
                MessageBox.Show("Add Driver Failed To Save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FillLicenseInfo();

            if (_License.Save())
            {
                MessageBox.Show("_License Saved", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (ctrlDrivingLicenseApplicationInfo1.SelectedLocalDrivingLicenseAppInfo.DoesPassTestType(clsTestType.enTestType.StreetTest))
                    ctrlDrivingLicenseApplicationInfo1.SelectedLocalDrivingLicenseAppInfo.SetComplete();
                btnIssueLicense.Enabled = false;
            }
            else
            {
                MessageBox.Show("_License Failed To Save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
            int LicenseID = _LocalDrivingLicenseApplication.IssueLicenseForTheFirtTime(txtNotes.Text.Trim(), clsGlobal.CurrentUser.UserID);

            if (LicenseID != -1)
            {
                MessageBox.Show("License Issued Successfully with License ID = " + LicenseID.ToString(),
                    "Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            else
            {
                MessageBox.Show("License Was not Issued ! ",
                 "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void frmIssueDriverLicenseFirstTime_Load(object sender, EventArgs e)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByID(_LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {

                MessageBox.Show("No Applicaiton with ID=" + _LocalDrivingLicenseApplicationID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


            if (!_LocalDrivingLicenseApplication.PassedAllTests())
            {

                MessageBox.Show("Person Should Pass All Tests First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            int LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();

            if (LicenseID != -1)
            {

                MessageBox.Show("Person already has License before with License ID=" + LicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;

            }

            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);

        }
    }
}
