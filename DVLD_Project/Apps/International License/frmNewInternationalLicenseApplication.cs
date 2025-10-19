using DVLD_Buisness;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.International_Licenses;
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

namespace DVLD_Project.Apps.International_License
{
    public partial class frmNewInternationalLicenseApplication : Form
    {
        int _LicenseID = -1;
        clsLicense _License;
        clsInternationalLicense internationalLicense = new clsInternationalLicense();
        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _LicenseID = obj;
            if (_LicenseID == -1)
                return;

            _License = clsLicense.Find(_LicenseID);

            btnIssueLicense.Enabled = false;
            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = false;

            //Fill Application Info
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblFees.Text = clsApplicationType.FindApplicationTypeInfoByType
                (clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees.ToString();
            lblLocalLicenseID.Text = _LicenseID.ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToShortDateString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

            if (_License.LicenseClassInfo.ClassName != clsLicenseClass.Find(3).ClassName)
            {
                MessageBox.Show("You Can't Issue International License With This License Type",
                    "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_License.IsActive)
            {
                MessageBox.Show("You Can't Issue International License With This Non Active License",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_License.IsDetained)
            {
                MessageBox.Show("You Can't Issue International License With This Detained License",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(_License.DriverInfo.DriverID) != -1)
            {
                MessageBox.Show("This Driver Has Already An Active International License",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_License.ExpirationDate < DateTime.Now)
            {
                MessageBox.Show("This License Expaired!, Renew It First",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnIssueLicense.Enabled = true;

           
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Issue International License For This Local License", "Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                return;

            //base class
            internationalLicense.ApplicantPersonID = _License.DriverInfo.PersonID;
            internationalLicense.ApplicationDate = DateTime.Now;
            internationalLicense.ApplicationTypeID = clsApplicationType.FindApplicationTypeInfoByType
                (clsApplication.enApplicationType.NewInternationalLicense).ApplicationTypeID;
            internationalLicense.ApplicationStatus = clsApplication.enApplicationStatus.New;
            internationalLicense.LastStatusDate = DateTime.Now;
            internationalLicense.PaidFees = clsApplicationType.FindApplicationTypeInfoByType
                (clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees;
            internationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            //sub class
            internationalLicense.DriverID = _License.DriverInfo.DriverID;
            internationalLicense.IssuedUsingLocalLicenseID = _LicenseID;
            internationalLicense.IssueDate = DateTime.Now;
            internationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            internationalLicense.IsActive = true;

            if(!internationalLicense.Save() )
            {
                MessageBox.Show("Error : New International License Not Saved","Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("New International License Saved", "Confirm",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);


            btnIssueLicense.Enabled = false;
            llShowLicenseInfo.Enabled = true;
            ctrlDriverLicenseInfoWithFilter1.FilterEnable = false;

            lblApplicationID.Text = internationalLicense.ApplicationID.ToString();
            lblInternationalLicenseID.Text = internationalLicense.InternationalLicenseID.ToString();

        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(internationalLicense.InternationalLicenseID);
            frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(_License.DriverInfo.PersonID);
            frm.ShowDialog();
        }
    }
}
