using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsCountry
    {

        public int CountryID {  get; set; }
        public string CountryName { get; set; }

        public clsCountry()
        {
            CountryID = -1;
            CountryName = "";
        }

        clsCountry(int countryID, string countryName)
        {
            CountryID = countryID;
            CountryName = countryName;
        }
        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }
        public static clsCountry Find(int CountryID)
        {
            string CountryName = "";
            if(clsCountryData.GetCountryInfoByID(CountryID,ref CountryName))
            {
                return new clsCountry(CountryID,CountryName);
            }
            return null;
        }

        public static clsCountry Find(string CountryName)
        {
            int CountryID = -1;

            if (clsCountryData.GetCountryInfoByName(ref CountryID, CountryName))
            {
                return new clsCountry(CountryID, CountryName);
            }
            return null;
        }
    }
}
