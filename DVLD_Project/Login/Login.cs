using DVLD_Buisness;
using DVLD_Project.Global_Classes;
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

namespace DVLD_Project.Login
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Set Values First Please.", "Data Not Entred", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            

            clsUser _User = clsUser.GetUserInfoByUsernameAndPassword(txtUserName.Text.Trim(),txtPassword.Text.Trim());
            if (_User != null )
            {
                if(!_User.IsActive)
                {
                    MessageBox.Show("User Not Active.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                if (chkRememberMe.Checked)
                {
                    clsGlobal.RememberUsernameAndPassword(_User.UserName, _User.Password);
                }else
                {
                    clsGlobal.DeleteAllTextData();
                }

                clsGlobal.CurrentUser = _User;
                Form1 form1 = new Form1(_User);
                form1.ShowDialog();
            }
            else
            {
                MessageBox.Show("Incorrect username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            string UserName = "", Password ="";

            if(clsGlobal.GetStoredCredential(ref UserName, ref Password))
            {
                txtUserName.Text = UserName;
                txtPassword.Text = Password;
            }
        }
    }
}
