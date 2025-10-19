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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Project.Apps.Rlease_Detained_License
{
    public partial class frmReleaseDetainedLicenseApplication : Form
    {
        int _LicenseID = -1;
        public frmReleaseDetainedLicenseApplication()
        {
            InitializeComponent();

            btnRelease.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
        }
        public frmReleaseDetainedLicenseApplication(int _LicenseID)
        {
            InitializeComponent();

            btnRelease.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;

            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_LicenseID);
            ctrlDriverLicenseInfoWithFilter1.FilterEnable = false;
        }
        private void frmReleaseDetainedLicenseApplication_Load(object sender, EventArgs e)
        {
            
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _LicenseID = obj;

            if (_LicenseID == -1)
                return;

            if(!ctrlDriverLicenseInfoWithFilter1.LicenseInfo.IsDetained)
            {
                MessageBox.Show("Error : This License Not Detained","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                btnRelease.Enabled = false;
                llShowLicenseHistory.Enabled = true;
                llShowLicenseInfo.Enabled = true;
                return;
            }

            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true;

            clsDetainedLicense _DetainedLicense = clsDetainedLicense.FindByLicenseID(_LicenseID);

            if (_DetainedLicense == null)
            {
                MessageBox.Show("Error : Detained App Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            btnRelease.Enabled = true;

            lblDetainID.Text = _DetainedLicense.DetainID.ToString();
            lblDetainDate.Text = _DetainedLicense.DetainDate.ToShortDateString();
            lblApplicationFees.Text = clsApplicationType.FindApplicationTypeInfoByType(clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).ApplicationFees.ToString();
            lblLicenseID.Text = _LicenseID.ToString();
            lblCreatedByUser.Text = _DetainedLicense.CreatedByUserInfo.UserName;
            lblFineFees.Text = _DetainedLicense.FineFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblFineFees.Text) + Convert.ToSingle(lblApplicationFees.Text)).ToString();


        }

        private void btnRelease_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are You Sure You Want To Release This License?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;

            int ApplicationID = -1;

            if (!ctrlDriverLicenseInfoWithFilter1.LicenseInfo.ReleaseDetainedLicense(clsGlobal.CurrentUser.UserID, ref ApplicationID))
            {
                MessageBox.Show("Error : Release Process Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("License Released", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);

            lblApplicationID.Text = ApplicationID.ToString();
            btnRelease.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnable = false;

        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.LicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }
    }
}
