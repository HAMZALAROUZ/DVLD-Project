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

namespace DVLD_Project.Apps.ReplaceLostOrDamagedLicense
{
    public partial class frmReplaceLostOrDamagedLicenseApplication : Form
    {
        private clsLicense _NewLicense;
        clsLicense _License;
        public frmReplaceLostOrDamagedLicenseApplication()
        {
            InitializeComponent();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
             _License = clsLicense.Find(obj);

            if (_License != null)
            {                
                if (!_License.IsActive)
                {
                    MessageBox.Show("Error : You Can't Renew This License, It's Not Active!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    btnIssueReplacement.Enabled = false;                    
                    llShowLicenseInfo.Enabled = false;
                    return;
                }
                llShowLicenseHistory.Enabled = true;
            }

            lblApplicationDate.Text = DateTime.Now.ToShortDateString();

            if(rbDamagedLicense.Checked) 
            lblApplicationFees.Text = clsApplicationType.FindApplicationTypeInfoByType(clsApplication.enApplicationType.ReplaceDamagedDrivingLicense).ApplicationFees.ToString();
            else
            lblApplicationFees.Text = clsApplicationType.FindApplicationTypeInfoByType(clsApplication.enApplicationType.ReplaceLostDrivingLicense).ApplicationFees.ToString();

            lblOldLicenseID.Text = _License.LicenseID.ToString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

            btnIssueReplacement.Enabled = true;
            
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Load(object sender, EventArgs e)
        {
            rbLostLicense.Checked = true;
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Replace The License?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if (rbDamagedLicense.Checked)
                _NewLicense = _License.Replace(clsLicense.enIssueReason.DamagedReplacement,clsGlobal.CurrentUser.UserID);
            else
                _NewLicense = _License.Replace(clsLicense.enIssueReason.LostReplacement, clsGlobal.CurrentUser.UserID);

            

            if(_NewLicense != null)
            {
                MessageBox.Show("License Replaced","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error : License Not Replaced","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }           
            ctrlDriverLicenseInfoWithFilter1.FilterEnable = false;
            btnIssueReplacement.Enabled = false;
            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true;

            lblApplicationID.Text = _NewLicense.ApplicationID.ToString();
            lblRreplacedLicenseID.Text = _NewLicense.LicenseID.ToString();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_NewLicense.LicenseID);
            frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.LicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }
    }
}
