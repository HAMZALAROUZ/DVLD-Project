using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsCountryData
    {
        
        public static bool GetCountryInfoByID(int CountryID,ref string CountryName)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Select * from Countries where CountryID = @CountryID";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@CountryID", CountryID);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    CountryName = (string)reader["CountryName"];

                    IsFound = true;
                }
                else
                {

                    IsFound = false;
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                //Error code
            }
            finally
            {
                connection.Close();
            }
            return IsFound;
        }

        public static bool GetCountryInfoByName(ref int CountryID, string CountryName)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Select * from Countries where CountryName = @CountryName";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    CountryID = (int)reader["CountryID"];

                    IsFound = true;
                }
                else
                {

                    IsFound = false;
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                //Error code
            }
            finally
            {
                connection.Close();
            }
            return IsFound;
        }

        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "select * from Countries order by CountryName";

            SqlCommand cmd = new SqlCommand(query, connection);

            

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);

                    
                }
                
                reader.Close() ;
            }
            catch (Exception ex)
            {
                //Error code
                return null;
            }
            finally
            {
                connection.Close();
            }
            return dt; 
        }
    }
}
