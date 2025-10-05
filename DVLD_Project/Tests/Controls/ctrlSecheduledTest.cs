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

namespace DVLD_Project.Tests.Controls
{
    public partial class ctrlSecheduledTest : UserControl
    {
        private int _LDLA_ID = -1;
        private clsLocalDrivingLicenseApplication _LDLA;

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

            lblLocalDrivingLicenseAppID.Text = _LDLA.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LDLA.LicenseClassInfo.ClassName;
            lblFullName.Text = _LDLA.ApplicantFullName;
            lblTrial.Text = _LDLA.TotalTrialsPerTest(_TestType.TestTypeID).ToString() + "/3";
            lblDate.Text = _LDLA.ApplicationDate.ToString();
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
        public void LoadData(int LDLA_ID,clsTestType.enTestType TestTypeID)
        {
            _TestType = clsTestType.Find(TestTypeID); //Test Type
            _LDLA_ID = LDLA_ID; //Local Driving Application Id
            _LDLA = clsLocalDrivingLicenseApplication.FindByID(LDLA_ID);//Find Local Driving License Application

            if (_LDLA != null)
            {
                FillSecheduledWithData();
            }else
            {
                ResetSecheduled();
                MessageBox.Show("LDLA ID Not Found","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
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
