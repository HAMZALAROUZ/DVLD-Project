using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsDetainedLicense
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int DetainID {  get; set; }
        public int LicenseID {  get; set; }
        public clsLicense LicenseIDInfo {  get; set; }
        public DateTime DetainDate {  get; set; }
        public float FineFees {  get; set; }
        public int CreatedByUserID {  get; set; }
        public clsUser CreatedByUserInfo { get; set; }
        public bool IsReleased { set; get; }
        public DateTime ReleaseDate { set; get; }//can be null
        public int ReleasedByUserID {  set; get; }//can be null
        public clsUser ReleasedByUserInfo { get; set; }
        public int ReleaseApplicationID {  get; set; }//can be null
        public clsApplication ReleaseApplicationIDInfo { get; set; }  
        
        public clsDetainedLicense()
        {
            DetainID = -1;
            LicenseID = -1;
            DetainDate = DateTime.Now;
            FineFees = 0;
            CreatedByUserID = -1;
            IsReleased = false;
            ReleaseDate = DateTime.Now;
            ReleasedByUserID = -1;
            ReleaseApplicationID = -1;
            Mode = enMode.AddNew;
        }

        clsDetainedLicense(int detainID, int licenseID, DateTime detainDate, float fineFees, int createdByUserID, bool isReleased, DateTime releaseDate, int releasedByUserID, int releaseApplicationID)
        {
            Mode = enMode.Update;
            DetainID = detainID;
            LicenseID = licenseID;            
            DetainDate = detainDate;
            FineFees = fineFees;
            CreatedByUserID = createdByUserID;
            
            IsReleased = isReleased;
            ReleaseDate = releaseDate;
            ReleasedByUserID = releasedByUserID;
            ReleaseApplicationID = releaseApplicationID;

            CreatedByUserInfo = clsUser.GetUserInfoByUserID(this.CreatedByUserID);
            ReleasedByUserInfo = clsUser.GetUserInfoByUserID(this.ReleasedByUserID);
            ReleaseApplicationIDInfo = clsApplication.GetApplicationInfoByID(this.ReleaseApplicationID);
        }

        bool _AddNewDetainedLicense()
        {
            this.DetainID = clsDetainedLicenseData.AddNewDetainedLicense(this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);
            return (this.DetainID != -1);
        }
        bool _UpdateDetainedLicense()
        {
            return clsDetainedLicenseData.UpdateDetainedLicense(this.DetainID, this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);
        }

        bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewDetainedLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateDetainedLicense();
            }

            return false;
        }

        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();
        }

        public static bool IsLicenseDetained(int LicenseId)
        {
            return clsDetainedLicenseData.IsLicenseDetained(LicenseId);
        }

        public static clsDetainedLicense FindByLicenseID(int LicenseId)
        {
            int DetainID = -1; DateTime DetainDate = DateTime.Now;
            float FineFees = 0; int CreatedByUserID = -1;
            bool IsReleased = false; DateTime ReleaseDate = DateTime.MaxValue;
            int ReleasedByUserID = -1; int ReleaseApplicationID = -1;
            
            if (clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseID(LicenseId,
            ref DetainID, ref DetainDate,
            ref FineFees, ref CreatedByUserID,
            ref IsReleased, ref ReleaseDate,
            ref ReleasedByUserID, ref ReleaseApplicationID))
            {
                return new clsDetainedLicense(DetainID,
                     LicenseId, DetainDate,
                     FineFees, CreatedByUserID,
                     IsReleased, ReleaseDate,
                     ReleasedByUserID, ReleaseApplicationID);
            }
            return null;
        }

        public static clsDetainedLicense FindByDetainedID(int DetainID)
        {
            int LicenseId = -1; DateTime DetainDate = DateTime.Now;
            float FineFees = 0; int CreatedByUserID = -1;
            bool IsReleased = false; DateTime ReleaseDate = DateTime.MaxValue;
            int ReleasedByUserID = -1; int ReleaseApplicationID = -1;

            if (clsDetainedLicenseData.GetDetainedLicenseInfoByID(DetainID,
            ref LicenseId, ref DetainDate,
            ref FineFees, ref CreatedByUserID,
            ref IsReleased, ref ReleaseDate,
            ref ReleasedByUserID, ref ReleaseApplicationID))
            {
                return new clsDetainedLicense(DetainID,
                     LicenseId, DetainDate,
                     FineFees, CreatedByUserID,
                     IsReleased, ReleaseDate,
                     ReleasedByUserID, ReleaseApplicationID);
            }
            return null;
        }
        public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID)
        {
            return clsDetainedLicenseData.ReleaseDetainedLicense(this.DetainID, ReleasedByUserID, ReleaseApplicationID);
        }
    }
}
