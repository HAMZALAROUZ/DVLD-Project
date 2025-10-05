using DVLD_Buisness;
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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Project.Apps.Local_Driving_License
{
    public partial class ctrlDrivingLicenseApplicationInfo : UserControl
    {
        private int _LocalDrivingLicenseAppID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApp = new clsLocalDrivingLicenseApplication();

        int _licenseID = -1;
        public int LocalDrivingLicenseID { get { return _LocalDrivingLicenseAppID; } }

        public clsLocalDrivingLicenseApplication SelectedLocalDrivingLicenseAppInfo { get { return _LocalDrivingLicenseApp; } }
        public ctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        private void ctrlApplicationBasicInfo1_Load(object sender, EventArgs e)
        {

        }

        private void _FillLocalDrivingLicenseApplicationInfo()
        {
            _licenseID = clsLicense.FindActiveLicenseID(_LocalDrivingLicenseApp.ApplicantPersonID, _LocalDrivingLicenseApp.LicenseClassID);

            llShowLicenceInfo.Enabled = (_licenseID != -1);

            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApp.LocalDrivingLicenseApplicationID.ToString();
            lblAppliedFor.Text = _LocalDrivingLicenseApp.LicenseClassInfo.ClassName.ToString();
            lblPassedTests.Text = clsTest.GetPassedTestCount(_LocalDrivingLicenseApp.LocalDrivingLicenseApplicationID).ToString() + "/3";

            ctrlApplicationBasicInfo1.LoadApplicationInfo(_LocalDrivingLicenseApp.ApplicationID);

        }
        private void _ResetLocalDrivingLicenseApplicationInfo()
        {
            lblLocalDrivingLicenseApplicationID.Text = "[???]";
            lblAppliedFor.Text = "[???]";
            lblPassedTests.Text = "0/3";
            llShowLicenceInfo.Enabled = false;
            ctrlApplicationBasicInfo1.ResetApplicationInfo();
        }

        public void LoadDataByAppID(int AppID)
        {
            _LocalDrivingLicenseApp = clsLocalDrivingLicenseApplication.FindByApplicationID(AppID);

            if (_LocalDrivingLicenseApp != null)
            {
                _FillLocalDrivingLicenseApplicationInfo();
            }
            else
            {
                _ResetLocalDrivingLicenseApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + _LocalDrivingLicenseAppID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public void LoadApplicationInfoByLocalDrivingAppID(int LDLA_ID) //Local Driving License ID
        {
            _LocalDrivingLicenseAppID = LDLA_ID;
            _LocalDrivingLicenseApp = clsLocalDrivingLicenseApplication.FindByID(LDLA_ID);

            if (_LocalDrivingLicenseApp != null)
            {
                _FillLocalDrivingLicenseApplicationInfo();
                //ctrlApplicationBasicInfo1.LoadApplicationInfo(_LocalDrivingLicenseApp.ApplicationID);
            }
            else
            {
                _ResetLocalDrivingLicenseApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + _LocalDrivingLicenseAppID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


        }
        private void ctrlDrivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {

        }

        private void llShowLicenceInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            frmShowLicenseInfo frm = new frmShowLicenseInfo(_licenseID);
            frm.ShowDialog();
        }
    }
}
