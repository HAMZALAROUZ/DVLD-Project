using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.User
{
    public partial class FrmListUsers : Form
    {
        //I Get The Data From DataBase
        private DataTable _dtAllUsers = clsUser.GetAllUsers();

        public FrmListUsers()
        {
            InitializeComponent();
        }
        private void _Refresh_DGVUsers()
        {
            _dtAllUsers = clsUser.GetAllUsers();
            dgvUsers.DataSource = _dtAllUsers;

            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }

        private void FrmListUsers_Load(object sender, EventArgs e)
        {
            dgvUsers.DataSource = _dtAllUsers;
            cbFilterBy.SelectedIndex = 0;//None
            txtFilterValue.Visible = false;

            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddUpdateUsers frm = new frmAddUpdateUsers();
            frm.ShowDialog();

            _Refresh_DGVUsers();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {


            string FilterColumn = "";

            switch (cbFilterBy.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;
                case "UserName":
                    FilterColumn = "UserName";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvUsers.RowCount.ToString();
                return;
            }

            if (FilterColumn == "UserID" || FilterColumn == "PersonID")
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
            {
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            }

            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }
        private void _DefaultFilter()
        {
            _dtAllUsers.DefaultView.RowFilter = "";

            txtFilterValue.Text = string.Empty;
            cbIsActive.SelectedItem = "All";

            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            
             
            if(cbFilterBy.SelectedItem.ToString() == "None")
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = false;

                _DefaultFilter();
                return;
            }

            if(cbFilterBy.SelectedItem.ToString() != "Is Active")
            {
                txtFilterValue.Visible = true;
                cbIsActive.Visible = false;

                _DefaultFilter();

                
            }
            if(cbFilterBy.SelectedItem.ToString() == "Is Active")
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = true;

                _DefaultFilter();
            }
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsActive.Text;

            switch(FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;

            }

            if (FilterValue == "All")
                _dtAllUsers.DefaultView.RowFilter = "";
            else
                //in this case we deal with numbers not string.
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lblRecordsCount.Text = _dtAllUsers.Rows.Count.ToString();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            frmUserInfo frm = new frmUserInfo(UserID);
            frm.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddUpdateUsers frm = new frmAddUpdateUsers();
            frm.ShowDialog();

            _Refresh_DGVUsers();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUsers frm = new frmAddUpdateUsers((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _Refresh_DGVUsers();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = int.Parse(dgvUsers.CurrentRow.Cells[0].Value.ToString());


            if (!clsUser.DeleteUser(UserID))
            {
                MessageBox.Show("This User Is Related In The System", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
            else
            {
                MessageBox.Show("User Deleted", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _Refresh_DGVUsers();
            }
            
        }

        private void ChangePasswordtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            frmChangePassword frm = new frmChangePassword(UserID);
            frm.ShowDialog();

        }
    }
}
