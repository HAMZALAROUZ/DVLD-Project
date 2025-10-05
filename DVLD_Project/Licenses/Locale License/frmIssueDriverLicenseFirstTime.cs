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
        public frmIssueDriverLicenseFirstTime(int LDL_ID)
        {
            InitializeComponent();
            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(LDL_ID);
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
            //I have to think about some conditions
            //if i have already a driver with same personid do allow to add or driver table just once
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
        }
    }
}
