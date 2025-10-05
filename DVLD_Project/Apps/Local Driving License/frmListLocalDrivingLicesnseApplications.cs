using DVLD_Buisness;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.Locale_License;
using DVLD_Project.Tests;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD_Project.Apps.Local_Driving_License
{
    public partial class frmListLocalDrivingLicesnseApplications : Form
    {
        private DataTable _dtAllLocalDrLiApp;
        private clsLocalDrivingLicenseApplication _LDLA = new clsLocalDrivingLicenseApplication();
        public frmListLocalDrivingLicesnseApplications()
        {
            InitializeComponent();
        }

        private void frmListLocalDrivingLicesnseApplications_Load(object sender, EventArgs e)
        {
            _dtAllLocalDrLiApp = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();
            dgvLocalDrivingLicenseApplications.DataSource = _dtAllLocalDrLiApp;
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();

            if (dgvLocalDrivingLicenseApplications.Rows.Count > 0)
            {
                dgvLocalDrivingLicenseApplications.Columns[0].HeaderText = "L.D.L.AppID";
                dgvLocalDrivingLicenseApplications.Columns[0].Width = 120;

                dgvLocalDrivingLicenseApplications.Columns[1].HeaderText = "Driving Class";
                dgvLocalDrivingLicenseApplications.Columns[1].Width = 300;

                dgvLocalDrivingLicenseApplications.Columns[2].HeaderText = "National No.";
                dgvLocalDrivingLicenseApplications.Columns[2].Width = 150;

                dgvLocalDrivingLicenseApplications.Columns[3].HeaderText = "Full Name";
                dgvLocalDrivingLicenseApplications.Columns[3].Width = 350;

                dgvLocalDrivingLicenseApplications.Columns[4].HeaderText = "Application Date";
                dgvLocalDrivingLicenseApplications.Columns[4].Width = 170;

                dgvLocalDrivingLicenseApplications.Columns[5].HeaderText = "Passed Tests";
                dgvLocalDrivingLicenseApplications.Columns[5].Width = 150;
            }

            cbFilterBy.SelectedIndex = 0;//None
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {            
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
            else if (txtFilterValue.Text.Trim() != "")
                txtFilterValue.Text = "";
        }
        private void RefreshDgvLDLA()
        {
            frmListLocalDrivingLicesnseApplications_Load(null, null);
        }
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";


            switch (cbFilterBy.Text)
            {
                case "L.D.L.AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;
                case "National No.":
                    FilterColumn = "NationalNo";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Status":
                    FilterColumn = "Status";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }
            if (txtFilterValue.Text.Trim() == "" || cbFilterBy.Text == "None")
            {
                _dtAllLocalDrLiApp.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
                return;

            }
            if (FilterColumn == "LocalDrivingLicenseApplicationID")
            {
                if (clsValidation.IsNumber(txtFilterValue.Text.Trim()))
                    _dtAllLocalDrLiApp.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            }
            else
            {
                _dtAllLocalDrLiApp.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());
            }

            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication();
            frm.ShowDialog();
            RefreshDgvLDLA();
        }

        private void CancelApplicaitonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure Do You Want To Cancel This Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }

            int LDL_ID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            _LDLA = clsLocalDrivingLicenseApplication.FindByID(LDL_ID);

            if (_LDLA != null)
            {
                if (_LDLA.SetCancel())
                {
                    MessageBox.Show("Application Cancelled Successfuly", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshDgvLDLA();
                }
                else
                {
                    MessageBox.Show("Application Not Cancelled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
        private void ResetcmsApplications()
        {
            editToolStripMenuItem.Enabled = true;
            CancelApplicaitonToolStripMenuItem.Enabled = true;
            ScheduleTestsMenue.Enabled = true;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
            showDetailsToolStripMenuItem.Enabled = true;
            DeleteApplicationToolStripMenuItem.Enabled = true;
            showLicenseToolStripMenuItem.Enabled = true;
            showPersonLicenseHistoryToolStripMenuItem.Enabled = true;
        }
        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            /*
            ResetcmsApplications();

            int LDL_ID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            int PassedTest = clsTest.GetPassedTestCount(LDL_ID);

            if (clsLocalDrivingLicenseApplication.FindByID(LDL_ID).ApplicationStatus == clsApplication.enApplicationStatus.Cancelled)
            {
                editToolStripMenuItem.Enabled = false;
                CancelApplicaitonToolStripMenuItem.Enabled = false;
                ScheduleTestsMenue.Enabled = false;
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                showLicenseToolStripMenuItem.Enabled = false;
                return;
            }
            if (PassedTest <= 2)
            {
                //ScheduleTestsMenue.Enabled = true;
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                showLicenseToolStripMenuItem.Enabled = false;

                scheduleVisionTestToolStripMenuItem.Enabled = (PassedTest == 0);
                scheduleWrittenTestToolStripMenuItem.Enabled = (PassedTest == 1);
                scheduleStreetTestToolStripMenuItem.Enabled = (PassedTest == 2);
                return;
            }

            ScheduleTestsMenue.Enabled = false;

            _LocalDrivingLicenseApp = clsLocalDrivingLicenseApplication.FindByID(LDL_ID);
            int LicenseID = clsLicense.FindActiveLicenseID(_LocalDrivingLicenseApp.ApplicantPersonID, _LocalDrivingLicenseApp.LicenseClassID);

            if (LicenseID != -1)
            {
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                editToolStripMenuItem.Enabled = false;
                CancelApplicaitonToolStripMenuItem.Enabled = false;
                DeleteApplicationToolStripMenuItem.Enabled = false;
            }
            else
            {
                //issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
                showLicenseToolStripMenuItem.Enabled = false;

            }
            */

            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = 
                                        clsLocalDrivingLicenseApplication.FindByID
                                                (LocalDrivingLicenseApplicationID);

            int TotalPassedTests = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[5].Value;

            bool LicenseExists = LocalDrivingLicenseApplication.IsLicenseIssued();

            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (TotalPassedTests ==3) && !LicenseExists;

            showLicenseToolStripMenuItem.Enabled = LicenseExists;
            editToolStripMenuItem.Enabled = !LicenseExists && (LocalDrivingLicenseApplication.ApplicationStatus == clsLocalDrivingLicenseApplication.enApplicationStatus.New);
            ScheduleTestsMenue.Enabled = !LicenseExists;

            CancelApplicaitonToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            DeleteApplicationToolStripMenuItem.Enabled =
                (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            bool PassedVisionTest = clsLocalDrivingLicenseApplication.DoesPassTestType(LocalDrivingLicenseApplicationID,clsTestType.enTestType.VisionTest);
            bool PassedWrittenTest = clsLocalDrivingLicenseApplication.DoesPassTestType(LocalDrivingLicenseApplicationID, clsTestType.enTestType.WrittenTest);
            bool PassedStreetTest = clsLocalDrivingLicenseApplication.DoesPassTestType(LocalDrivingLicenseApplicationID, clsTestType.enTestType.StreetTest);

            ScheduleTestsMenue.Enabled = (!PassedVisionTest ||  !PassedVisionTest || !PassedStreetTest) && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            if (ScheduleTestsMenue.Enabled)
            {
                //To Allow Schdule vision test, Person must not passed the same test before.
                scheduleVisionTestToolStripMenuItem.Enabled = !PassedVisionTest;

                //To Allow Schdule written test, Person must pass the vision test and must not passed the same test before.
                scheduleWrittenTestToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;

                //To Allow Schdule steet test, Person must pass the vision * written tests, and must not passed the same test before.
                scheduleStreetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;

            }
        }
        private void _SchedualeTest(clsTestType.enTestType testType)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmListTestAppointments frm = new frmListTestAppointments(LocalDrivingLicenseApplicationID, testType);
            frm.ShowDialog();

            RefreshDgvLDLA();
        }
        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //int Id = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            //frmListTestAppointments frm = new frmListTestAppointments(Id, clsTestType.enTestType.VisionTest);
            //frm.ShowDialog();
            //RefreshDgvLDLA();

            _SchedualeTest(clsTestType.enTestType.VisionTest);
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //int Id = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            //frmListTestAppointments frm = new frmListTestAppointments(Id, clsTestType.enTestType.WrittenTest);
            //frm.ShowDialog();
            //RefreshDgvLDLA();

            _SchedualeTest(clsTestType.enTestType.WrittenTest);
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //int Id = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            //frmListTestAppointments frm = new frmListTestAppointments(Id, clsTestType.enTestType.StreetTest);
            //frm.ShowDialog();
            //RefreshDgvLDLA();

            _SchedualeTest(clsTestType.enTestType.StreetTest);
        }

        private void DeleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Delete This Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }

            int LDL_ID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            _LDLA = clsLocalDrivingLicenseApplication.FindByID(LDL_ID);

            if (_LDLA != null)
            {
                if (_LDLA.DeleteLocalDrivingLicenseApplication())
                {
                    MessageBox.Show("Local Driving License Deleted Successfuly", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshDgvLDLA();
                }
                else
                {
                    MessageBox.Show("Local Driving License Not Deleted, Related To The System", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                MessageBox.Show("Local Driving License Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDL_ID = -1; // default if no value

            if (dgvLocalDrivingLicenseApplications.CurrentRow != null &&
                dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value != null &&
                dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value != DBNull.Value)
            {
                LDL_ID = Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            }
            if (LDL_ID != -1)
            {
                //First Issue Licence
                frmIssueDriverLicenseFirstTime frm = new frmIssueDriverLicenseFirstTime(LDL_ID);
                frm.ShowDialog();
                RefreshDgvLDLA();
            }
            //This When he/she Takes the last test and passed ,then i have to make application status = completed
            //if (_LocalDrivingLicenseApp.DoesPassTestType(clsTestType.enTestType.StreetTest))
            //    _LocalDrivingLicenseApp.SetComplete();            
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDL_ID = -1; // default if no value
            clsLocalDrivingLicenseApplication LDLA = new clsLocalDrivingLicenseApplication();
            int LicenseID = -1;

            if (dgvLocalDrivingLicenseApplications.CurrentRow != null &&
                dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value != null &&
                dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value != DBNull.Value)
            {
                LDL_ID = Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            }
            else
            {
                MessageBox.Show("Failed To Get Local Driving Application ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (LDL_ID != -1)
            {
                LDLA = clsLocalDrivingLicenseApplication.FindByID(LDL_ID);
                LicenseID = clsLicense.FindActiveLicenseID(LDLA.ApplicantPersonID, LDLA.LicenseClassID);

                if (LicenseID != -1)
                {
                    frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
                    frm.ShowDialog();
                }
            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDL_ID = -1;
            clsLocalDrivingLicenseApplication LDLA;

            if (dgvLocalDrivingLicenseApplications.CurrentRow != null &&
                dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value != null &&
                dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value != DBNull.Value)
            {
                LDL_ID = Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            }
            else
            {
                MessageBox.Show("Failed To Get Local Driving Application ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (LDL_ID != -1)
            {
                LDLA = clsLocalDrivingLicenseApplication.FindByID(LDL_ID);
                if (LDLA != null)
                {
                    frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(LDLA.ApplicantPersonID);
                    frm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Failed To Get Local Driving Application Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLA_ID = -1;
            LDLA_ID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LDLA = clsLocalDrivingLicenseApplication.FindByID(LDLA_ID);
            frmLocalDrivingLicenseApplicationInfo frm =
                        new frmLocalDrivingLicenseApplicationInfo(LDLA.ApplicationID);
            frm.ShowDialog();

            RefreshDgvLDLA();
        }
    }
}
