using DVLD_Buisness;
using DVLD_Project.Global_Classes;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD_Project.People
{
    public partial class frmAddUpdatePerson : Form
    {
        public enum enMode { _AddNew = 0,_Update = 1 }
        private enMode _Mode = enMode._AddNew;

        public enum enGender {Male = 0,Female = 1}
        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode._AddNew;
           

        }
       
        clsPerson _Person = new clsPerson();
        int _PersonID = -1; 
        //string _PreviesImagePath = "";
        //string NewImagePath = "";

        public delegate void DataBackEventHandler(object sender, clsPerson Person);

        public event DataBackEventHandler DataBack;
        public frmAddUpdatePerson(clsPerson person)
        {
            InitializeComponent();
            _Person = person;
            _PersonID = person.PersonID;
            _Mode = enMode._Update;
            
        }
        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
            _Mode = enMode._Update;
           
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        
        private void txtNationalNo_Validated(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            if (clsPerson.isPersonExist(txtNationalNo.Text.Trim()) && txtNationalNo.Text.Trim() != _Person.NationalNo)
            {

                errorProvider1.SetError(txtNationalNo, "This NationalNo Is Used, Change it");
                txtNationalNo.Focus();
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, "");
            }
        }
        void _FillComboOfCountries()
        {
            DataTable dt = clsCountry.GetAllCountries();
            cbCountry.DataSource = dt;
            cbCountry.DisplayMember = "CountryName";
            cbCountry.ValueMember = "CountryID";
        }
        private void _ResetDefualtValues()
        {
            _FillComboOfCountries();

            if(_Mode == enMode._AddNew)
            {
                lblTitle.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else
            {
                lblTitle.Text = "Update Person";
            }

            rbMale.Checked = true;

            if (rbMale.Checked)
                pbPersonImage.Image = Properties.Resources.Male_512;
            else
                pbPersonImage.Image = Properties.Resources.Female_512;

            llRemoveImage.Visible = (pbPersonImage.ImageLocation != null);

            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";            
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";

            dtpDateOfBirth.MaxDate = DateTime.Today.AddYears(-18);
            cbCountry.SelectedIndex = cbCountry.FindString("Jordan");
        }
        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {

            _ResetDefualtValues();

            if (_Mode == enMode._Update)            
                _LoadData();
            
        }        

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = ofd.FileName;
                pbPersonImage.ImageLocation = selectedFilePath;
                llRemoveImage.Visible = true;

               // IsDefaultImage = false;//
            }


        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.Image = null;
            pbPersonImage.ImageLocation = null;


           // IsDefaultImage = true ;//

            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            llRemoveImage.Visible = false;


        }
        bool _HandlePersonImage()
        {
            
            if(_Person.ImagePath != pbPersonImage.ImageLocation)
            {
                if(_Person.ImagePath != "")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);

                    }
                    catch (IOException)
                    {
                        // We could not delete the file.
                        //log it later   
                    }
                }

                if(pbPersonImage.ImageLocation != null)
                {
                    string SourceImageFile = pbPersonImage.ImageLocation.ToString();
                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                    {
                        pbPersonImage.ImageLocation = SourceImageFile;
                        
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                
            }
            return true;
        }

        
        

        //This Method Called just in Edit/Update _Mode
        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);


            if (_Person == null )
            {
                MessageBox.Show("No Person with ID = " + _Person.PersonID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtAddress.Text = _Person.Address;
            txtEmail.Text = _Person.Email;
            txtPhone.Text = _Person.Phone;
            txtNationalNo.Text = _Person.NationalNo;
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            if (_Person.Gender == 0)            
                rbMale.Checked = true;            
            else            
                rbFemale.Checked = true;


            if (_Person.ImagePath != null && _Person.ImagePath != "" && File.Exists(_Person.ImagePath))
            {
                pbPersonImage.ImageLocation = _Person.ImagePath;
            }
            else
            {

                if (_Person.Gender == 0)            
                    pbPersonImage.Image = Properties.Resources.Male_512;

                else            
                    pbPersonImage.Image = Properties.Resources.Female_512;

            }

            lblPersonID.Text = _Person.PersonID.ToString();

            cbCountry.SelectedValue = _Person.NationalityCountryID;

            
            llRemoveImage.Visible = (!string.IsNullOrEmpty(_Person.ImagePath));

        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!_HandlePersonImage())
                return;
            _Person.NationalNo = txtNationalNo.Text;
            _Person.FirstName = txtFirstName.Text;
            _Person.SecondName = txtSecondName.Text;
            _Person.LastName = txtLastName.Text;
            _Person.Email = txtEmail.Text;
            _Person.Phone = txtPhone.Text;
            _Person.ThirdName = txtThirdName.Text;
            _Person.Address = txtAddress.Text;
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            
            if(pbPersonImage.ImageLocation != null)
            {
                _Person.ImagePath = pbPersonImage.ImageLocation.ToString();
            }
            else
            {
                _Person.ImagePath = "";
            }
            if (rbFemale.Checked)
                _Person.Gender = (short) enGender.Female;
            else _Person.Gender = (short)enGender.Male;

            _Person.NationalityCountryID = (int)cbCountry.SelectedValue;

            if(_Person.Save())
            {
                _Mode = enMode._Update;
                lblPersonID.Text = _Person.PersonID.ToString();
                MessageBox.Show("Person saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblTitle.Text = "Update Person";

                DataBack?.Invoke(this, _Person);
            }
            else
            {
                MessageBox.Show("Failed to save person", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void txtEmail_Validated(object sender, EventArgs e)
        {
            if(txtEmail.Text.Trim() == "") 
                { return; }

            if(!clsValidation.ValidateEmail(txtEmail.Text))
            {                
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }
            
        }

        private void txt_Validated(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;
            if (textBox.Text == string.Empty)
            {
                errorProvider1.SetError(textBox, "Set a Value");
            }
            else
            {
                errorProvider1.SetError(textBox, "");
            }
        }        

        private void frmAddUpdatePerson_FormClosing(object sender, FormClosingEventArgs e )
        {
            //DataBack?.Invoke(this,_Person);
        }

        private void rbMale_Click(object sender, EventArgs e)
        {
            if(pbPersonImage.ImageLocation == null)                           
               pbPersonImage.Image = Properties.Resources.Male_512;                
            
        }

        private void rbFemale_Click(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Female_512;
        }
    }
}
