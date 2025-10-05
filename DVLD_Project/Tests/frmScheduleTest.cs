using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Tests
{
    public partial class frmScheduleTest : Form
    {
        private int _LocalDrivingLicenseAppID = -1;
        private clsTestType.enTestType _TestTypeID;
        clsTestAppointment _Appointment;
        int _AppointmentID=-1;
        //private clsLocalDrivingLicenseApplication.enMode _Mode;
        enum enMode { AddNew = 0, Update = 1 }
        enMode Mode = enMode.AddNew;
        /*
        public frmScheduleTest(int LocalDrivingLicenseAppID, clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _LocalDrivingLicenseAppID = LocalDrivingLicenseAppID;
            _TestTypeID = TestTypeID;
            Mode = enMode.AddNew;
        }
        public frmScheduleTest(clsTestAppointment Appointment, clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestAppointment = Appointment;
            _TestTypeID = TestTypeID;
            Mode = enMode.Update;

        }
        */
        public frmScheduleTest(int LocalDrivingLicenseAppID, clsTestType.enTestType TestTypeID, int appointmentID=-1)
        {
            InitializeComponent();

            _LocalDrivingLicenseAppID = LocalDrivingLicenseAppID;
            _TestTypeID = TestTypeID;            
            _AppointmentID = appointmentID;
        }
        public frmScheduleTest(clsTestAppointment Appointment, clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _Appointment = Appointment;
            _TestTypeID = TestTypeID;
            Mode = enMode.Update;

        }
        private void crlScheduleTest1_Load(object sender, EventArgs e)
        {

        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            /*
            if (Mode == enMode.Update)
            {
                crlScheduleTest1.LoadData(_TestAppointment, _TestTypeID);
            }
            else
                crlScheduleTest1.LoadData(_LocalDrivingLicenseAppID, _TestTypeID);
            */
            crlScheduleTest1.TestTypeID = _TestTypeID;
            crlScheduleTest1.LoadData(_LocalDrivingLicenseAppID,_AppointmentID);
        }
    }
}
