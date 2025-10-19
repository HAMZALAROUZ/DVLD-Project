using DVLD_Buisness;
using DVLD_Project.Apps.International_License;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses.Locale_License;
using DVLD_Project.People;
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

namespace DVLD_Project.Licenses.International_Licenses
{
    public partial class frmListInternationalLicesnseApplications : Form
    {
        DataTable _dtAllInternationalLicense;

        int InternationalLicenseID = -1;
        clsInternationalLicense InternationalLicense;
        public frmListInternationalLicesnseApplications()
        {
            InitializeComponent();
        }

        private void frmListInternationalLicesnseApplications_Load(object sender, EventArgs e)
        {
            _dtAllInternationalLicense = clsInternationalLicense.GetAllInternationalLicenses();
            dgvInternationalLicenses.DataSource = _dtAllInternationalLicense;
            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
            cbFilterBy.SelectedIndex = 0;
            if (dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenses.Columns[0].Width = 160;

                dgvInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[1].Width = 150;

                dgvInternationalLicenses.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicenses.Columns[2].Width = 130;

                dgvInternationalLicenses.Columns[3].HeaderText = "L.License ID";
                dgvInternationalLicenses.Columns[3].Width = 130;

                dgvInternationalLicenses.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[4].Width = 180;

                dgvInternationalLicenses.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[5].Width = 180;

                dgvInternationalLicenses.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[6].Width = 120;
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedItem.ToString() == "None")
            {
                cbIsReleased.Visible = false;
                txtFilterValue.Visible = false;
            }
            if (cbFilterBy.SelectedItem.ToString() == "Is Active")
            {
                cbIsReleased.Visible = true;
                txtFilterValue.Visible = false;
            }
            else
            {
                cbIsReleased.Visible = false;
                txtFilterValue.Visible = true;
            }

            _dtAllInternationalLicense.DefaultView.RowFilter = "";
            lblInternationalLicensesRecords.Text = _dtAllInternationalLicense.Rows.Count.ToString();
        }
  
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFilterBy.SelectedItem.ToString())
            {
                case "International License ID":
                    FilterColumn = "InternationalLicenseID";
                    break;
                case "Application ID":
                    FilterColumn = "ApplicationID";
                    break;
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;
                case "Local License ID":
                    FilterColumn = "IssuedUsingLocalLicenseID";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllInternationalLicense.DefaultView.RowFilter = "";
                lblInternationalLicensesRecords.Text = _dtAllInternationalLicense.Rows.Count.ToString();
                return;
            }


            if (clsValidation.IsNumber(txtFilterValue.Text.Trim()))
                _dtAllInternationalLicense.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());




            lblInternationalLicensesRecords.Text = _dtAllInternationalLicense.Rows.Count.ToString();
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIsReleased.SelectedItem.ToString() == "All")
            {
                _dtAllInternationalLicense.DefaultView.RowFilter = "";

            }
            else if (cbIsReleased.SelectedItem.ToString() == "No")
            {
                _dtAllInternationalLicense.DefaultView.RowFilter = string.Format("[{0}] = {1}", "IsActive", 0);
            }
            else
            {
                _dtAllInternationalLicense.DefaultView.RowFilter = string.Format("[{0}] = {1}", "IsActive", 1);
            }

            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
        }

        private void PesonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternationalLicenseID =(int) dgvInternationalLicenses.CurrentRow.Cells[0].Value;
            InternationalLicense = clsInternationalLicense.Find(InternationalLicenseID);

            if(InternationalLicenseID !=-1 && InternationalLicense != null)
            {
                frmShowPersonInfo frm = new frmShowPersonInfo(InternationalLicense.DriverInfo.PersonID);
                frm.ShowDialog();
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternationalLicenseID = (int)dgvInternationalLicenses.CurrentRow.Cells[0].Value;
            
            if (InternationalLicenseID != -1)
            {
                frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
                frm.ShowDialog();
            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternationalLicenseID = (int)dgvInternationalLicenses.CurrentRow.Cells[0].Value;
            InternationalLicense = clsInternationalLicense.Find(InternationalLicenseID);

            if (InternationalLicenseID != -1 && InternationalLicense != null)
            {
                frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(InternationalLicense.DriverInfo.PersonID);
                frm.ShowDialog();
            }
        }

        private void btnNewApplication_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApplication frm = new frmNewInternationalLicenseApplication();
            frm.ShowDialog();
        }
    }
}
