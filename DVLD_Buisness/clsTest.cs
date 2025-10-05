using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsTest
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult {  get; set; }
        public clsTestAppointment TestAppointmentInfo;
        
        public string Notes { get; set; }//Allow Null
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo;

        

        public clsTest()
        {
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = false;
            Notes = "";
            CreatedByUserID = -1;
            Mode = enMode.AddNew;
            
        }
        clsTest(int testID, int testAppointmentID, bool result, string notes, int createdByUserID)
        {
            Mode = enMode.Update;            
            TestID = testID;
            TestAppointmentID = testAppointmentID;
            TestResult = result;
            Notes = notes;
            CreatedByUserID = createdByUserID;

            //TestAppointmentInfo = testAppointmentInfo;
            //CreatedByUserInfo = createdByUserInfo;
        }

        public static clsTest Find(int TestID)
        {
            int TestAppointmentID = -1;
            bool TestResult = false; string Notes = ""; int CreatedByUserID = -1;

            if(clsTestData.GetTestInfoByID(TestID,ref TestAppointmentID,ref TestResult,ref Notes,ref CreatedByUserID))
            {
                return new clsTest(TestID,TestAppointmentID,TestResult,Notes,CreatedByUserID);
            }
            return null;
        }

        public static clsTest FindLastTestByPersonAndTestTypeAndLicenseClass(int PersonID, int LicenseClassID, clsTestType.enTestType TestTypeID)
        {
            int TestID = -1;
            int TestAppointmentID = -1;
            bool TestResult = false; string Notes = ""; int CreatedByUserID = -1;

            if(clsTestData.GetLastTestByPersonAndTestTypeAndLicenseClass(PersonID, LicenseClassID, (int)TestTypeID,ref TestID,ref TestAppointmentID,ref TestResult,ref Notes,ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }
            return null;
        }

        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();
        }
        bool _AddNewTest()
        {
            this.TestID = clsTestData.AddNewTest(this.TestAppointmentID,this.TestResult,this.Notes,this.CreatedByUserID);
            return (this.TestID != -1);
        }
        bool _UpdateTest()
        {
            return clsTestData.UpdateTest(this.TestID, this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewTest())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateTest();
            }
            return false;
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }
    }
}
