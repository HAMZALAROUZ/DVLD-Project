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

namespace DVLD_Project.User
{
    public partial class frmAddUpdateUsers : Form
    {
        clsUser _User = new clsUser();

        int _UserID = -1;
        public enum enMode { _AddNew = 0, _Update = 1 }
        private enMode _Mode = enMode._AddNew;

        public frmAddUpdateUsers()
        {
            InitializeComponent();
            _Mode = enMode._AddNew;
        }
        public frmAddUpdateUsers(int UserID)
        {
            InitializeComponent();
            _Mode = enMode._Update;
            _UserID = UserID;
        }
        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            //Add New Mode
            if (_Mode != enMode._Update)
            {
                if (clsUser.IsUserExistByPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Sorry This Person Is Already A User", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                
            }

            //Update Mode
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {
                tpLoginInfo.Enabled = true;
                tcUserInfo.SelectedTab = tpLoginInfo;
                btnSave.Enabled = true;

            }
            else
                MessageBox.Show("No person selected");
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _ResetDefualtValues()
        {
            if (_Mode == enMode._AddNew)
            {
                lbTitle.Text = "Add New User";
                this.Text = "Add New User";//this is the title of Form
                _User = new clsUser();

                btnSave.Enabled = false;
                tpLoginInfo.Enabled = false;
            }

            if (_Mode == enMode._Update)
            {
                lbTitle.Text = "Update User";
                this.Text = "Update User";
                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }

            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = true;
        }
        private void frmAddUpdateUsers_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (_Mode == enMode._Update)
            {
                _LoadData();
                btnSave.Enabled = _User != null;
            }
        }
        private void _LoadData()
        {

            _User = clsUser.GetUserInfoByUserID(_UserID);

            if (_User != null)
            {
                ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);
                lblUserID.Text = _User.UserID.ToString();
                txtUserName.Text = _User.UserName;
                txtPassword.Text = _User.Password;
                txtConfirmPassword.Text = _User.Password;
                chkIsActive.Checked = _User.IsActive;
                ctrlPersonCardWithFilter1.FilterEnable = false;
            }
            else
            {
                MessageBox.Show("User Not Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        

        private void FillUserRecord()
        {

            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text;
            _User.Password = txtPassword.Text;
            _User.IsActive = chkIsActive.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FillUserRecord();


            if (_User.Save())
            {
                lblUserID.Text = _User.UserID.ToString();
                //change form mode to update.
                _Mode = enMode._Update;
                lbTitle.Text = "Update User";
                this.Text = "Update User";

                MessageBox.Show("Record Saved successfully!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Record Failed To Save!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }


        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            
            if (txtUserName.Text.Trim() == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "Required");
                return;
            }
            else
                errorProvider1.SetError(txtUserName, "");
            if (_Mode == enMode._AddNew)
            {
                if (!clsUser.IsUserExist(txtUserName.Text.Trim()))
                {
                    errorProvider1.SetError(txtUserName, "");

                }
                else
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "username is used by another user");
                }
            }
            
            else
            {
                //it's update mode
                //here i check if the username is changed 
                if (_User.UserName != txtUserName.Text.Trim())
                {
                    //if changed then i have to check if this new username not used by another user
                    if (clsUser.IsUserExist(txtUserName.Text.Trim()))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "username is used by another user");
                        return ;
                    }
                    else
                    {
                        errorProvider1.SetError(txtUserName, "");
                    }
                }
                
            }
            
            
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            
            if (txtPassword.Text.Trim() == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Required");
                return;
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
            }
            if (txtPassword.Text.Trim() != "" && txtConfirmPassword.Text.Trim() != "")
            {
                if (!_IsPasswordMatch())
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtPassword, "Password Not Match");
                    errorProvider1.SetError(txtConfirmPassword, "Password Not Match");
                    return;
                }
                else
                {
                    errorProvider1.SetError(txtPassword, "");
                    errorProvider1.SetError(txtConfirmPassword, "");
                }
            }
            

        }
        private bool _IsPasswordMatch()
        {
            return (txtPassword.Text.Trim() == txtConfirmPassword.Text.Trim());
        }

        //private bool _IsAllValidated()
        //{
        //    if (!string.IsNullOrEmpty(errorProvider1.GetError(txtPassword)))
        //    {
        //        return false;
        //    }
        //    else if (!string.IsNullOrEmpty(errorProvider1.GetError(txtConfirmPassword)))
        //    {
        //        return false;
        //    }
        //    else if (!string.IsNullOrEmpty(errorProvider1.GetError(txtUserName)))
        //    {
        //        return false;
        //    }
        //    else if (string.IsNullOrEmpty(txtPassword.Text.Trim()) || string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()) || string.IsNullOrEmpty(txtUserName.Text.Trim()))
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            
            if (txtConfirmPassword.Text.Trim() == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Required");
                return;
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, "");
            }

            if (txtPassword.Text.Trim() != "" && txtConfirmPassword.Text.Trim() != "")
            {
                if (!_IsPasswordMatch())
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtPassword, "Password Not Match");
                    errorProvider1.SetError(txtConfirmPassword, "Password Not Match");
                    return;
                }
                else
                {
                    errorProvider1.SetError(txtPassword, "");
                    errorProvider1.SetError(txtConfirmPassword, "");
                }
            }
            

        }
    }
}
