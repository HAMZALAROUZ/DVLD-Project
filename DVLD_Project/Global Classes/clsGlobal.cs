using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Project.Global_Classes
{
    public class clsGlobal
    {
        public static clsUser CurrentUser;

        public static bool DeleteAllTextData()
        {
            try
            {

            File.WriteAllText(@"D:\UserData.txt", string.Empty);
            }
            catch { return false; }
            return true;
        }
        public static bool RememberUsernameAndPassword(string Username, string Password)
        {

            DeleteAllTextData();
            string UserInfo = Username + "/#/" + Password;
            try
            {
                File.AppendAllText(@"D:\UserData.txt", UserInfo + Environment.NewLine);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            string firstLine = File.ReadLines(@"D:\UserData.txt").FirstOrDefault();
            if (firstLine != null)
            {

                string[] parts = firstLine.Split(new string[] { "/#/" }, StringSplitOptions.None);

                string username = parts[0];
                string password = parts[1];

                if (username != null && password != null)
                {
                    Username = username;
                    Password = password;

                }
                return true;
            }
            return false;
        }
    }
}
