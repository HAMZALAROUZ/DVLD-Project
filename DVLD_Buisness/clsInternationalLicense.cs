using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLD_Buisness
{
    public class clsInternationalLicense : clsApplication
    {
        public enum enMode {AddNew = 0,Update =1}
        public enMode Mode = enMode.AddNew;

        public int InternationalLicenseID { set; get; }
        public int DriverID { set; get; }
        public clsDriver DriverInfo { set; get; }
        public int IssuedUsingLocalLicenseID { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public bool IsActive { set; get; }

        public clsInternationalLicense()
        {
            InternationalLicenseID = -1;
            DriverID = -1;
            IssuedUsingLocalLicenseID = -1;

            IssueDate = DateTime.MaxValue;
            ExpirationDate = DateTime.MaxValue;
            IsActive = false;
            Mode = enMode.AddNew;
        }

        clsInternationalLicense(int _InternationalLicenseID,int _DriverID,int _IssuedUsingLocalLicenseID,DateTime _IssueDate,DateTime _ExpirationDate, bool _IsActive
            ,int _ApplicationID,int _CreatedByUserID,int _ApplicantPersonID,DateTime _ApplicationDate, DateTime _LastStatusDate,enApplicationStatus _ApplicationStatus,float _PaidFees)
        {
            this.InternationalLicenseID= _InternationalLicenseID;
            this.DriverID= _DriverID;
            this.IssuedUsingLocalLicenseID= _IssuedUsingLocalLicenseID;
            this.IssueDate= _IssueDate;
            this.ExpirationDate= _ExpirationDate;
            this.IsActive = _IsActive;
            this.CreatedByUserID= _CreatedByUserID;
            this.ApplicationID= _ApplicationID;
            Mode = enMode.Update;

            base.ApplicationID = _ApplicationID;
            base.CreatedByUserID = _CreatedByUserID;            
            base.ApplicationStatus = _ApplicationStatus;
            base.LastStatusDate = _LastStatusDate;
            base.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            base.ApplicantPersonID = _ApplicantPersonID;
            base.ApplicationDate = _ApplicationDate;
            base.PaidFees = _PaidFees;

            
            DriverInfo = clsDriver.GetDriverInfoByDriverID(_DriverID);
        }

        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1; int IssuedUsingLocalLicenseID = -1;
            DateTime IssueDate = DateTime.Now; DateTime ExpirationDate = DateTime.Now;
            bool IsActive = true; int CreatedByUserID = 1;

            if(clsInternationalLicenseData.GetInternationalLicenseInfoByID(InternationalLicenseID,ref ApplicationID,ref DriverID,ref IssuedUsingLocalLicenseID,ref IssueDate,ref ExpirationDate,ref IsActive,ref CreatedByUserID))
            {
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

                return new clsInternationalLicense(InternationalLicenseID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive,ApplicationID,CreatedByUserID,Application.ApplicantPersonID,Application.ApplicationDate,Application.LastStatusDate,Application.ApplicationStatus,Application.PaidFees);
            }
            return null;
        }

        bool _AddNewInternationalLicense()
        {
            this.InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense(this.ApplicationID,this.DriverID,this.IssuedUsingLocalLicenseID,this.IssueDate,this.ExpirationDate,this.IsActive,this.CreatedByUserID);
            return (this.InternationalLicenseID != -1);
        }

        bool _UpdateInternationalLicense()
        {
            return clsInternationalLicenseData.UpdateInternationalLicense(this.InternationalLicenseID, this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);
        }

        public bool Save()
        {
            base.Mode = (clsApplication.enMode)Mode;

            if(!base.Save()) return false;
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewInternationalLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateInternationalLicense();
            }
            return false;
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(DriverID);
        }
        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }

        //public static clsInternationalLicense Find(int InternationalLicenseID)
        //{
        //    int ApplicationID = -1;
        //    int DriverID = -1; int IssuedUsingLocalLicenseID = -1;
        //    DateTime IssueDate = DateTime.Now; DateTime ExpirationDate = DateTime.Now;
        //    bool IsActive = true; int CreatedByUserID = 1;

        //    if(clsInternationalLicenseData.GetInternationalLicenseInfoByID(InternationalLicenseID,ref ApplicationID,ref DriverID,ref IssuedUsingLocalLicenseID,ref IssueDate,ref ExpirationDate,ref IsActive,ref CreatedByUserID))
        //    {
        //        return new clsInternationalLicense(InternationalLicenseID,DriverID,IssuedUsingLocalLicenseID,IssueDate,ExpirationDate,IsActive,ApplicationID,CreatedByUserID);
        //    }
        //    return null;
        //}
    }
}
