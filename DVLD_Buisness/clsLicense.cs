using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Buisness.clsLicense;

namespace DVLD_Buisness
{
    public class clsLicense
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };
        public enIssueReason IssueReason = enIssueReason.FirstTime;
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClass { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public float PaidFees { get; set; }
        public bool IsActive { get; set; }

        public int CreatedByUserID { get; set; }

        public clsDriver DriverInfo;
        public clsLicenseClass LicenseClassInfo;
        public clsDetainedLicense DetainedLicenseInfo;


        public string IssueReasonText
        {
            get
            {
                return GetIssueReasonText(this.IssueReason);
            }
        }
        public bool IsDetained
        {
            get { return clsDetainedLicense.IsLicenseDetained(this.LicenseID); }
        }
        public clsLicense()
        {
            LicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            LicenseClass = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            Notes = "";
            PaidFees = 0;
            IsActive = false;
            IssueReason = enIssueReason.FirstTime;
            CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }

        clsLicense(int licenseID, int applicationID, int driverID, int licenseClass, DateTime issueDate, DateTime expirationDate, string notes, float paidFees, bool isActive, enIssueReason issueReason, int createdByUserID)
        {
            Mode = enMode.Update;
            LicenseID = licenseID;
            ApplicationID = applicationID;
            DriverID = driverID;
            LicenseClass = licenseClass;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes;
            PaidFees = paidFees;
            IsActive = isActive;
            IssueReason = issueReason;
            CreatedByUserID = createdByUserID;

            DriverInfo = clsDriver.GetDriverInfoByDriverID(this.DriverID);
            LicenseClassInfo = clsLicenseClass.Find(this.LicenseClass);
            DetainedLicenseInfo = clsDetainedLicense.FindByLicenseID(this.LicenseID);

        }
        public string GetIssueReasonText(enIssueReason issueReason)
        {
            switch (issueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Damaged Replacement";
                case enIssueReason.LostReplacement:
                    return "Lost Replacement";
                default:
                    return "unkown";
            }
        }
        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1; int DriverID = -1; int LicenseClass = -1;
            DateTime IssueDate = DateTime.Now; DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0; bool IsActive = true; int CreatedByUserID = 1;
            byte IssueReason = 1;

            if (clsLicenseData.GetLicenseInfoByID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass,
            ref IssueDate, ref ExpirationDate, ref Notes,
            ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))

                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass,
                                     IssueDate, ExpirationDate, Notes,
                                     PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);
            else
                return null;
        }

        bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseData.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClass,
               this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees,
               this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);


            return (this.LicenseID != -1);
        }
        bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(this.ApplicationID, this.LicenseID, this.DriverID, this.LicenseClass,
              this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees,
              this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLicense();

            }

            return false;
        }

        public static int FindActiveLicenseID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
        }
        public static DataTable GetDriverLicenses(int DriverID)
        {
            return clsLicenseData.GetDriverLicenses(DriverID);
        }

        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();
        }

        public static bool IsLicenseExistsByPersonID(int PersonID, int LicenseClassID)
        {
            return FindActiveLicenseID(PersonID, LicenseClassID) != -1;
        }

        public bool IsLicenseExpired()
        {
            return (this.ExpirationDate < DateTime.Now);
        }

        public bool ActivateLicense()
        {
            return clsLicenseData.ActivateLicense(this.LicenseID);
        }
        public bool DeactivateLicense()
        {
            return clsLicenseData.DeactivateLicense(this.LicenseID);
        }
        public int Detain(float FineFees, int CreatedByUserID)
        {
            clsDetainedLicense detainedLicense = new clsDetainedLicense();

            detainedLicense.LicenseID = this.LicenseID;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = FineFees;
            detainedLicense.CreatedByUserID = CreatedByUserID;
            //detainedLicense.IsReleased = false;

            if(!detainedLicense.Save() )
            {
                return -1;
            }            

            return detainedLicense.DetainID;


        }
        public bool ReleaseDetainedLicense(int ReleasedByUserID, ref int ApplicationID)
        {


            clsDetainedLicense detainedLicense = clsDetainedLicense.FindByLicenseID(this.LicenseID);

            if (detainedLicense == null)
            {
                return false;
            }

            clsApplication _application = new clsApplication();


            _application.ApplicantPersonID = this.DriverInfo.PersonID;
            _application.ApplicationDate = DateTime.Now;
            _application.ApplicationTypeID = clsApplicationType.FindApplicationTypeInfoByType(clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).ApplicationTypeID;
            _application.LastStatusDate = DateTime.Now;
            _application.PaidFees = clsApplicationType.FindApplicationTypeInfoByID(_application.ApplicationTypeID).ApplicationFees;
            _application.CreatedByUserID = ReleasedByUserID;

            if(!_application.Save())
            {
                return false;
            }

            ApplicationID = _application.ApplicationID;

                        
            if (!detainedLicense.ReleaseDetainedLicense(ReleasedByUserID, ApplicationID))
            {
                return false;
            }

            return true;
        }
        public clsLicense RenewLicense(string Notes, int CreatedByUserID)
        {
            clsLicense _NewLicense = new clsLicense();

            _NewLicense = this;

            
            _NewLicense.IssueDate = DateTime.Now;
            _NewLicense.ExpirationDate = DateTime.Now.AddYears(clsLicenseClass.Find(this.LicenseClassInfo.LicenseClassID).DefaultValidityLength);
            _NewLicense.IssueReason = clsLicense.enIssueReason.Renew;
            _NewLicense.Notes = Notes;
            _NewLicense.CreatedByUserID = CreatedByUserID;
            _NewLicense.Mode = enMode.AddNew;

            return _NewLicense;
        }
        public clsLicense Replace(enIssueReason IssueReason, int CreatedByUserID)
        {
            clsApplication _application = new clsApplication();


            _application.ApplicantPersonID = this.DriverInfo.PersonID;
            _application.ApplicationDate = DateTime.Now;
            _application.ApplicationTypeID = (int)IssueReason;
            _application.LastStatusDate = DateTime.Now;
            _application.PaidFees = clsApplicationType.FindApplicationTypeInfoByID((int)IssueReason).ApplicationFees;
            _application.CreatedByUserID = CreatedByUserID;

            if (!_application.Save())
            {                
                return null;
            }

            clsLicense _NewLicense = new clsLicense();           


            _NewLicense.DriverID = this.DriverID;
            _NewLicense.LicenseClass = this.LicenseClass;
            _NewLicense.IssueDate = this.IssueDate;
            _NewLicense.ExpirationDate = this.ExpirationDate;
            _NewLicense.Notes = this.Notes;
            _NewLicense.PaidFees = this.PaidFees;
            _NewLicense.ApplicationID = _application.ApplicationID;
            _NewLicense.IssueReason = IssueReason;
            _NewLicense.IsActive = true;
            _NewLicense.CreatedByUserID = CreatedByUserID;

            if (!_NewLicense.Save())
            {
                return null;
            }

            DeactivateLicense();

            return _NewLicense;

        }
    }
}
