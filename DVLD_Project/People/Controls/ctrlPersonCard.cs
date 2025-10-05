using DVLD_Buisness;
using DVLD_Project.People;
using DVLD_Project.Properties;
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

namespace DVLD_Project
{
    public partial class ctrlPersonCard : UserControl
    {
        private clsPerson _Person;
        private int _PersonID = -1;
        
        public int PersonID
        {
            get { return _PersonID; }
        }
        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }

        public ctrlPersonCard()
        {
            InitializeComponent();
            
        }

        private void _LoadPersonImage()
        {
            if(_Person.ImagePath != null && _Person.ImagePath != "")
            {
                if(File.Exists(_Person.ImagePath))
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
        private void _FillPersonInfo()
        {
            llEditPersonInfo.Enabled = true;
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblNationalNo.Text = _Person.NationalNo;
            lblFullName.Text = _Person.FullName;
            lblGender.Text = _Person.Gender == 0 ? "Male" : "Female";
            lblEmail.Text = _Person.Email;
            lblPhone.Text = _Person.Phone;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblCountry.Text = clsCountry.Find(_Person.NationalityCountryID).CountryName;
            lblAddress.Text = _Person.Address;
            _LoadPersonImage();
        }
        public void ResetPersonInfo()
        {
            _PersonID = -1;
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblFullName.Text = "[????]";            
            lblGender.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pbPersonImage.Image = Resources.Male_512;
        }
        public void LoadData(int _PersonID)
        {
            _Person = clsPerson.Find(_PersonID);

            if (_Person != null)
            {
                _FillPersonInfo();
            }
            else
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with PersonID = " + _PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        public void LoadData(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);

            if (_Person != null)
            {
                _FillPersonInfo();
            }
            else
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with NationalNo = " + NationalNo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            //if (!int.TryParse(lblPersonID.Text, out int PersonID))
            //    return;

            //clsPerson Person = clsPerson.Find(PersonID);
            //if (Person != null)
            //{
            //    frmShowPersonInfo fr = this.FindForm() as frmShowPersonInfo;                
            //    frmAddUpdatePerson frm = new frmAddUpdatePerson(Person);                
            //    //I subscribe To DataBack Of AddUpdatePerson
            //    frm.DataBack += fr.GetUpdatedPersonData; 

            //    frm.ShowDialog();
            //}

            frmAddUpdatePerson frm = new frmAddUpdatePerson(_Person);
            frm.ShowDialog();

            ///like refresh
            LoadData(_Person.PersonID);
        }

        
        
    }
}
