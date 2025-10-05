using DVLD_Buisness;
using DVLD_Project.Global_Classes;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_Buisness.clsTestType;

namespace DVLD_Project.Tests
{
    public partial class frmListTestAppointments : Form
    {
        int _LocalDrivingLicenseApplicationID = -1;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApp = new clsLocalDrivingLicenseApplication();
        clsTestType.enTestType _TestTypeID;
        
        public frmListTestAppointments(int LDLA_ID,clsTestType.enTestType testTypeID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LDLA_ID;
            _TestTypeID = testTypeID;
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _LoadTestTypeImageAndTitle()
        {
            switch (_TestTypeID)
            {

                case clsTestType.enTestType.VisionTest:
                    {
                        lblTitle.Text = "Vision Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;
                    }

                case clsTestType.enTestType.WrittenTest:
                    {
                        lblTitle.Text = "Written Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;
                    }
                case clsTestType.enTestType.StreetTest:
                    {
                        lblTitle.Text = "Street Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                    }
            }
        }
        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();

            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);
            _LocalDrivingLicenseApp = clsLocalDrivingLicenseApplication.FindByID(_LocalDrivingLicenseApplicationID);

            DataTable _dtTestAppointmentsPerTestType = clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_LocalDrivingLicenseApplicationID, _TestTypeID);

            if(_dtTestAppointmentsPerTestType != null) 
            dgvLicenseTestAppointments.DataSource = _dtTestAppointmentsPerTestType;

            if (dgvLicenseTestAppointments.Rows.Count > 0)
            {
                dgvLicenseTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvLicenseTestAppointments.Columns[0].Width = 150;

                dgvLicenseTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvLicenseTestAppointments.Columns[1].Width = 200;

                dgvLicenseTestAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvLicenseTestAppointments.Columns[2].Width = 150;

                dgvLicenseTestAppointments.Columns[3].HeaderText = "Is Locked";
                dgvLicenseTestAppointments.Columns[3].Width = 100;
            }

            lblRecordsCount.Text = dgvLicenseTestAppointments.RowCount.ToString();
        }
        private void RefreshDGVLicenseTestAppointments()
        {
            DataTable _dtTestAppointmentsPerTestType = clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_LocalDrivingLicenseApplicationID, _TestTypeID);

            if (_dtTestAppointmentsPerTestType != null)
                dgvLicenseTestAppointments.DataSource = _dtTestAppointmentsPerTestType;

            if (dgvLicenseTestAppointments.Rows.Count > 0)
            {
                dgvLicenseTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvLicenseTestAppointments.Columns[0].Width = 150;

                dgvLicenseTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvLicenseTestAppointments.Columns[1].Width = 200;

                dgvLicenseTestAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvLicenseTestAppointments.Columns[2].Width = 150;

                dgvLicenseTestAppointments.Columns[3].HeaderText = "Is Locked";
                dgvLicenseTestAppointments.Columns[3].Width = 100;
            }

            lblRecordsCount.Text = dgvLicenseTestAppointments.RowCount.ToString();
        }
        private clsApplication CreateNewAppForRetakeTest()
        {
            clsApplication _ReNewApp = new clsApplication();
            _ReNewApp.ApplicantPersonID = _LocalDrivingLicenseApp.ApplicantPersonID;
            _ReNewApp.ApplicationDate = DateTime.Now;
            _ReNewApp.ApplicationTypeID = clsApplicationType.GetApplicationTypeInfoByID(7).ApplicationTypeID;
            _ReNewApp.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _ReNewApp.LastStatusDate = DateTime.Now;
            _ReNewApp.PaidFees = clsApplicationType.GetApplicationTypeInfoByID(7).ApplicationFees;
            _ReNewApp.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if(_ReNewApp.Save())
            {
                return _ReNewApp;
            }
            return null;
        }
        //I have to test this if it work or not!!!!!!!!!!!!!!
        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            /*
            if (_LocalDrivingLicenseApp.IsThereAnActiveScheduledTest(_TestTypeID))
            {
                MessageBox.Show("This Person Has Schedualed Test", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_LocalDrivingLicenseApp.DoesPassTestType(_TestTypeID))
            {
                MessageBox.Show("This Person Passed This Test", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //hadi fach taykon daz yad dar test 
            if (_LocalDrivingLicenseApp.DoesAttendTestType(_TestTypeID))
            {
                clsApplication _ReNewApp = CreateNewAppForRetakeTest();
                clsTestAppointment testAppointment = clsTestAppointment.GetLastTestAppointment(_LocalDrivingLicenseApplicationID, _TestTypeID);

                if (_ReNewApp == null)
                {
                    MessageBox.Show("New Application Retake Test Not Added", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //I Linke Between Test Appointment And New App Retake Test
                testAppointment.RetakeTestApplicationID = _ReNewApp.ApplicationID;


            }


            //Else I Will Open Schadual Test To Set New Appointment for this test!!!!

            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID);
            frm.ShowDialog();
            RefreshDGVLicenseTestAppointments();
            */

            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByID(_LocalDrivingLicenseApplicationID);

            if(localDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestTypeID))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTest LastTest = localDrivingLicenseApplication.GetLastTestPerTestType(_TestTypeID);

            if(LastTest == null)
            {
                frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID);
                frm.ShowDialog();

                //Refresh
                RefreshDGVLicenseTestAppointments();
                return;
            }

            if(LastTest.TestResult)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest frm2 = new frmScheduleTest(LastTest.TestAppointmentInfo.LocalDrivingLicenseApplicationID, _TestTypeID);
            frm2.ShowDialog();
            RefreshDGVLicenseTestAppointments();
        }

        private void ctrlDrivingLicenseApplicationInfo1_Load(object sender, EventArgs e)
        {

        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTakeTest frm = new frmTakeTest(_LocalDrivingLicenseApplicationID, _TestTypeID);
            frm.ShowDialog();

            

            RefreshDGVLicenseTestAppointments();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgvLicenseTestAppointments.SelectedRows[0];

            // Example: get ID from the first cell
            int id = Convert.ToInt32(row.Cells[0].Value);
            clsTestAppointment appointment = clsTestAppointment.Find(id);
            frmScheduleTest frm = new frmScheduleTest(appointment, _TestTypeID);
            frm.ShowDialog();
            RefreshDGVLicenseTestAppointments();
        }
    }
}
