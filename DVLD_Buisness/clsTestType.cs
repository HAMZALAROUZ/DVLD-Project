using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsTestType
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };
        public enTestType TestTypeID {  get; set; }
        public string TestTypeTitle {  get; set; }
        public string TestTypeDescription {  get; set; }
        public float TestTypeFees {  get; set; }

        public clsTestType()
        {
            TestTypeID = enTestType.VisionTest;
            TestTypeTitle = "";
            TestTypeDescription = "";
            TestTypeFees = 0;
            Mode = enMode.AddNew;
        }

        clsTestType(enTestType testTypeID, string testTypeTitle, string testTypeDescription, float testTypeFees)
        {
            Mode = enMode.Update;
            TestTypeID = testTypeID;
            TestTypeTitle = testTypeTitle;
            TestTypeDescription = testTypeDescription;
            TestTypeFees = testTypeFees;
        }

        bool _AddNewTestType()
        {
            this.TestTypeID =(enTestType) clsTestTypeData.AddNewTestType(this.TestTypeTitle,this.TestTypeDescription,this.TestTypeFees);
            return ((int)this.TestTypeID != -1);
        }

        bool _UpdateTestType()
        {
            return clsTestTypeData.UpdateTestType((int)this.TestTypeID,this.TestTypeTitle,this.TestTypeDescription,this.TestTypeFees);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewTestType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateTestType();
            }
            return false;
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();
        }
        public static clsTestType Find(int TestTypeID)
        {
            return Find((enTestType)TestTypeID);
        }
        public static clsTestType Find(enTestType TestTypeID)
        {
            string Title = ""; string Description = ""; float Fees = 0;

            if(clsTestTypeData.GetTestTypeInfoByID((int)TestTypeID,ref Title,ref Description, ref Fees))
            {
                return new clsTestType(TestTypeID,Title,Description,Fees);
            }
            return null;
        }
    }
}
