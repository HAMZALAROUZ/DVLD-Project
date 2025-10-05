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

namespace DVLD_Project.Licenses.Locale_License.Controls
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        int _licenseID=-1;

        clsLicense _license = new clsLicense();   
        
        clsPerson _Person = new clsPerson();

        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();           
        }

        public int LicenseID { get { return _licenseID; } }
        public clsLicense SelectedLicenseInfo { get { return _license; } }
        public clsPerson SelectedPersonInfo { get { return _Person; } }

        private void FillDriverLicenseInfo()
        {
            lblClass.Text = _license.LicenseClassInfo.ClassName;
            lblFullName.Text = _Person.FullName;
            lblLicenseID.Text = _licenseID.ToString();
            lblNationalNo.Text = _Person.NationalNo;
            lblGendor.Text = _Person.Gender == 1 ? "Female" : "Male";
            lblIssueDate.Text = _license.IssueDate.ToString("dd/MMM/yyyy");
            lblIssueReason.Text = _license.IssueReasonText;

            if (!string.IsNullOrEmpty(_license.Notes))
                lblNotes.Text = _license.Notes;
            else lblNotes.Text = "No Notes";

            lblIsActive.Text = _license.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = _Person.DateOfBirth.ToString("dd/MMM/yyyy");
            lblDriverID.Text = _license.DriverID.ToString();
            lblExpirationDate.Text = _license.ExpirationDate.ToString("dd/MMM/yyyy");
            lblIsDetained.Text = _license.IsDetained ? "Yes" : "No";
        }

        private void ResetDriverLicenseInfo()
        {
            lblClass.Text = "[???]";
            lblFullName.Text = "[????]";
            lblLicenseID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGendor.Text = "[????]";
            lblIssueDate.Text = "[????]";
            lblIssueReason.Text = "[????]";
            lblNotes.Text = "[????]";
            lblIsActive.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblDriverID.Text = "[????]";
            lblExpirationDate.Text = "[????]";
            lblIsDetained.Text = "[????]";
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
        public void LoadData(int LicenseID)
        {
            _licenseID = LicenseID;
            _license = clsLicense.Find(LicenseID);
            
            
            _Person = clsPerson.Find(_license.DriverInfo.PersonID);                                   

            if( _license != null && _Person != null)
            {
                FillDriverLicenseInfo();
                _LoadPersonImage();
            }
            else
            {
                ResetDriverLicenseInfo();
                MessageBox.Show("Sorry Driver License Not Found","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
        }
    }
}
