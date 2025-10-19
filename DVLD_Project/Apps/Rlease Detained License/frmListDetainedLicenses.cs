using DVLD_Buisness;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.Detain_License;
using DVLD_Project.Licenses.Locale_License;
using DVLD_Project.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Apps.Rlease_Detained_License
{
    public partial class frmListDetainedLicenses : Form
    {
        DataTable _dtDetainedLicenses;
        public frmListDetainedLicenses()
        {
            InitializeComponent();
        }

        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            _dtDetainedLicenses = clsDetainedLicense.GetAllDetainedLicenses();
            dgvDetainedLicenses.DataSource = _dtDetainedLicenses;

            cbFilterBy.SelectedIndex = 0;

            if (_dtDetainedLicenses.Rows.Count > 0)
            {
                dgvDetainedLicenses.Columns[0].HeaderText = "D.ID";
                dgvDetainedLicenses.Columns[0].Width = 90;

                dgvDetainedLicenses.Columns[1].HeaderText = "L.ID";
                dgvDetainedLicenses.Columns[1].Width = 90;

                dgvDetainedLicenses.Columns[2].HeaderText = "D.Date";
                dgvDetainedLicenses.Columns[2].Width = 160;

                dgvDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvDetainedLicenses.Columns[3].Width = 110;

                dgvDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvDetainedLicenses.Columns[4].Width = 110;

                dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicenses.Columns[5].Width = 160;

                dgvDetainedLicenses.Columns[6].HeaderText = "N.No.";
                dgvDetainedLicenses.Columns[6].Width = 90;

                dgvDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvDetainedLicenses.Columns[7].Width = 330;

                dgvDetainedLicenses.Columns[8].HeaderText = "Rlease App.ID";
                dgvDetainedLicenses.Columns[8].Width = 150;

            }

            lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedItem.ToString() == "None")
            {
                cbIsReleased.Visible = false;
                txtFilterValue.Visible = false;
            }
            if (cbFilterBy.SelectedItem.ToString() == "Is Released")
            {
                cbIsReleased.Visible = true;
                txtFilterValue.Visible = false;
            }
            else
            {
                cbIsReleased.Visible = false;
                txtFilterValue.Visible = true;
            }

            _dtDetainedLicenses.DefaultView.RowFilter = "";
            lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();

        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIsReleased.SelectedItem.ToString() == "All")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = "";

            }
            else if (cbIsReleased.SelectedItem.ToString() == "No")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", "IsReleased", 0);
            }
            else
            {
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", "IsReleased", 1);
            }

            lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {

            string FilterColumn = "";

            switch (cbFilterBy.SelectedItem.ToString())
            {
                case "Detain ID":
                    FilterColumn = "DetainID";
                    break;
                case "National No.":
                    FilterColumn = "NationalNo";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Release Application ID":
                    FilterColumn = "ReleaseApplicationID";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = "";
                lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "DetainID" || FilterColumn == "ReleaseApplicationID")
            {
                if (clsValidation.IsNumber(txtFilterValue.Text.Trim()))
                    _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            }

            if (FilterColumn == "NationalNo" || FilterColumn == "FullName")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());
            }

            lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = -1;
            LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;

            if (LicenseID != -1)
            {
                frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication(LicenseID);
                frm.ShowDialog();
            }
            frmListDetainedLicenses_Load(null, null);
        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            releaseDetainedLicenseToolStripMenuItem.Enabled = !(bool)dgvDetainedLicenses.CurrentRow.Cells[3].Value;
        }

        private void PesonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = -1;
            int PersonID = -1;

            LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            PersonID = clsLicense.Find(LicenseID).DriverInfo.PersonID;

            if (PersonID != -1)
            {
                frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
                frm.ShowDialog();
            }

            frmListDetainedLicenses_Load(null, null);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = -1;
            LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;

            if (LicenseID != -1)
            {
                frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
                frm.ShowDialog();
            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = -1;
            int PersonID = -1;

            LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            PersonID = clsLicense.Find(LicenseID).DriverInfo.PersonID;

            if (PersonID != -1)
            {
                frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(PersonID);
                frm.ShowDialog();
            }
            frmListDetainedLicenses_Load(null, null);
        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication();
            frm.ShowDialog();

            frmListDetainedLicenses_Load(null, null);
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicenseApplication frm = new frmDetainLicenseApplication();
            frm.ShowDialog();

            frmListDetainedLicenses_Load(null, null);
        }
    }
}
