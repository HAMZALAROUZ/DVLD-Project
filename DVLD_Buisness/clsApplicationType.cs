using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsApplicationType
    {
        public enum enMode{ AddNew = 0, Update = 1 };

        public enMode Mode = enMode.AddNew;
        public int ApplicationTypeID {  get; set; }
        public string ApplicationTypeTitle { get; set; }

        public float ApplicationFees { get; set; }

        public clsApplicationType() 
        {
            this.ApplicationTypeID = -1;
            this.ApplicationFees = -1;
            this.ApplicationTypeTitle = "";
            this.Mode = enMode.AddNew;
        }

        clsApplicationType(int applicationTypeID, string applicationTypeTitle, float applicationFees)
        {
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationTypeTitle = applicationTypeTitle;
            this.ApplicationFees = applicationFees;
            this.Mode = enMode.Update;
        }

        public static clsApplicationType GetApplicationTypeInfoByID(int applicationTypeID)
        {
            string applicationTypeTitle = "";
            float applicationFees = -1;

            if(clsApplicationTypeData.GetApplicationTypeInfoByID(applicationTypeID,ref applicationTypeTitle,ref applicationFees))
            {
                return new clsApplicationType(applicationTypeID, applicationTypeTitle, applicationFees);
            }
            return null;
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypeData.GetAllApplicationTypes();
        }

        bool _AddNewApplicationType()
        {
            this.ApplicationTypeID = clsApplicationTypeData.AddNewApplicationType(this.ApplicationTypeTitle, this.ApplicationFees);
            return (this.ApplicationTypeID != -1);
        }

        bool _UpdateApplicationType()
        {
            return clsApplicationTypeData.UpdateApplicationType(this.ApplicationTypeID, this.ApplicationTypeTitle, this.ApplicationFees);
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewApplicationType())
                    {
                        this.Mode  = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateApplicationType();                    
            }
            return false;
        }
    }
    
}
