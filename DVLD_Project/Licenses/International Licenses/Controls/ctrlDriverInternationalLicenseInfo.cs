using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.International_Licenses.Controls
{
    public partial class ctrlDriverInternationalLicenseInfo : UserControl
    {
        int _InternationalLicenseID = -1;
        clsInternationalLicense _InternationalLicense;
        clsLicense _LocalLicenseInfo;
        clsPerson _Person;
        public int InternationalLicenseID
        {
            get {  return _InternationalLicenseID; }
        }

        public clsInternationalLicense InternationalLicense
        {
            get { return _InternationalLicense; }
        }

        public ctrlDriverInternationalLicenseInfo()
        {
            InitializeComponent();
        }
        private void _LoadPersonImage()
        {
            

            if (_Person.ImagePath != null && _Person.ImagePath != "")
            {
                if (File.Exists(_Person.ImagePath))
                {
                    pbPersonImage.ImageLocation = _Person.ImagePath;
                }
                else
                {
                    MessageBox.Show("Could not find this image: = " + _Person.ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (_Person.Gender == 0)
                    pbPersonImage.Image = Properties.Resources.Male_512;
                else
                    pbPersonImage.Image = Properties.Resources.Female_512;
            }
        }
        public void LoadData(int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;

            _InternationalLicense = clsInternationalLicense.Find(InternationalLicenseID);
            
            _LocalLicenseInfo = clsLicense.Find(clsLicense.FindActiveLicenseID(_InternationalLicense.DriverInfo.PersonID, 3));
            _Person = clsPerson.Find(_InternationalLicense.ApplicantPersonID);

            if(_InternationalLicense == null || _LocalLicenseInfo == null || _Person == null)
            {
                return;
            }


            lblFullName.Text = _InternationalLicense.ApplicantFullName;
            lblInternationalLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
            lblLocalLicenseID.Text = _LocalLicenseInfo.LicenseID.ToString();
            lblNationalNo.Text = _Person.NationalNo;
            lblGendor.Text = (_Person.Gender == 0) ? "Male" : "Female";
            lblIssueDate.Text = _InternationalLicense.IssueDate.ToShortDateString();
            lblApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
            lblIsActive.Text = (_InternationalLicense.IsActive) ? "Yes" : "No";
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblDriverID.Text = _InternationalLicense.DriverID.ToString();
            lblExpirationDate.Text = _InternationalLicense.ExpirationDate.ToShortDateString();

            _LoadPersonImage();
        }
    }
}
