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

namespace DVLD_Project.Tests.Controls
{
    public partial class ctrlSecheduledTest : UserControl
    {
        private int _LocalDrivingApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingApplication;

        private clsTestType _TestType;       

        public ctrlSecheduledTest()
        {
            InitializeComponent();
        }
        private void FillSecheduledWithData()
        {
            //_LocalDrivingLicenseApp.DoesAttendTestType(testtype)
            //_LocalDrivingLicenseApp.DoesPassTestType(testtype)
            //_LocalDrivingLicenseApp.DoesPersonHaveActiveApplication(ApplicationTypeID)
            //_LocalDrivingLicenseApp.GetActiveApplicationID(ApplicationTypeID)
            //_LocalDrivingLicenseApp.IsThereAnActiveScheduledTest(testtype)

            lblLocalDrivingLicenseAppID.Text = _LocalDrivingApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingApplication.LicenseClassInfo.ClassName;
            lblFullName.Text = _LocalDrivingApplication.ApplicantFullName;
            lblTrial.Text = _LocalDrivingApplication.TotalTrialsPerTest(_TestType.TestTypeID).ToString() + "/3";
            lblDate.Text = clsFormat.DateToShort(_LocalDrivingApplication.ApplicationDate);
            lblFees.Text = clsTestType.Find(_TestType.TestTypeID).TestTypeFees.ToString();

            lblTestID.Text = "Not Taken Yet";//
        }
        
        private void ResetSecheduled()
        {
            lblLocalDrivingLicenseAppID.Text = "[??]";
            lblDrivingClass.Text = "[???????]";
            lblFullName.Text = "[???????]";
            lblTrial.Text = "[??]";
            lblDate.Text = "[dd/mm/yyyy]";
            lblFees.Text = "[$$$]";
            lblTestID.Text = "Not Taken Yet";
        }
        public void LoadData(int LocalDrivingApplicationID,clsTestType.enTestType TestTypeID)
        {
            _TestType = clsTestType.Find(TestTypeID); //Test Type
            _LocalDrivingApplicationID = LocalDrivingApplicationID; //Local Driving Application Id
            _LocalDrivingApplication = clsLocalDrivingLicenseApplication.FindByID(LocalDrivingApplicationID);//Find Local Driving License Application

            if (_LocalDrivingApplication != null)
            {
                FillSecheduledWithData();
            }else
            {
                ResetSecheduled();
                MessageBox.Show("Local Driving Application ID Not Found", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            switch (TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    gbTestType.Text = "Vision Test";
                    pbTestTypeImage.Image = Properties.Resources.Vision_512;
                    break;
                case clsTestType.enTestType.WrittenTest:
                    gbTestType.Text = "Written Test";
                    pbTestTypeImage.Image = Properties.Resources.Written_Test_512;
                    break;
                case clsTestType.enTestType.StreetTest:
                    gbTestType.Text = "Driving Test";
                    pbTestTypeImage.Image = Properties.Resources.driving_test_512;
                    break;
            }
        }
    }
}
