using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;
        public int UserID {  get; set; }
        public int PersonID {  get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public clsUser() 
        {
            UserID = -1;
            PersonID = -1;
            UserName = "";
            Password = "";
            IsActive = false;
            Mode = enMode.AddNew;
        }

        clsUser(int userID, int personID, string userName, string password, bool isActive)
        {            
            this.UserID = userID;
            this.PersonID = personID;
            this.UserName = userName;
            this.Password = password;
            this.IsActive = isActive;
            this.Mode = enMode.Update;
        }

        public static clsUser GetUserInfoByUserID(int UserID)
        {
            int personID = -1;
            string userName = "";
            string password = "";
            bool isActive =false;
            if(clsUserData.GetUserInfoByUserID(UserID,ref personID,ref userName,ref password,ref isActive))
            {
                return new clsUser(UserID,personID,userName,password,isActive);
            }
            return null;
        }

        public static clsUser GetUserInfoByPersonID(int personID)
        {
            int userID = -1;
            string userName = "";
            string password = "";
            bool isActive = false;

            if (clsUserData.GetUserInfoByPersonID( personID, ref userID, ref userName, ref password, ref isActive))
            {
                return new clsUser(userID, personID, userName, password, isActive);
            }
            return null;
        }

        public static clsUser GetUserInfoByUsernameAndPassword(string username, string password)
        {
            int userID = -1;           
            bool isActive = false;
            int personID = -1;

            if (clsUserData.GetUserInfoByUsernameAndPassword( username, password,ref userID,ref personID, ref isActive))
            {
                return new clsUser(userID, personID, username, password, isActive);
            }
            return null;
        }

        bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(this.PersonID, this.UserName, this.Password,this.IsActive);
            return (this.UserID != -1);
        }
        bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.UserID,this.PersonID, this.UserName, this.Password, this.IsActive);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateUser();
            }
            return false;
        }

        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }

        public static bool DeleteUser(int userID)
        {
            return clsUserData.DeleteUser(userID);
        }

        public static bool IsUserExist(int UserID)
        {
            return clsUserData.IsUserExist(UserID);
        }
        public static bool IsUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }

        public static bool IsUserExistByPersonID(int PersonID)
        {
            return clsUserData.IsUserExistForPersonID(PersonID);
        }

        public static bool DoesPersonHaveUser(int PersonID)
        {
            return clsUserData.DoesPersonHaveUser(PersonID);
        }

        public static bool ChangePassword(int UserID, string NewPassword)
        {
            return clsUserData.ChangePassword(UserID, NewPassword);
        }
    }
}
