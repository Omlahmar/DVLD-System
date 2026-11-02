using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsDetainedLicenseData
    {
        public static int AddNewDetainedLicense(
          int LicenseID, DateTime DetainDate,
          float FineFees, int CreatedByUserID)

        {
            int DetainID = -1;
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_AddNewDetainedLicense",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LicenseID", LicenseID);
                    cmd.Parameters.AddWithValue("@DetainDate", DetainDate);
                    cmd.Parameters.AddWithValue("@FineFees", FineFees);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    // Output parameter
                    SqlParameter outputParam = new SqlParameter("@NewDetainID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    cmd.ExecuteNonQuery();

                    DetainID = (int)outputParam.Value;
                }
            }
            return DetainID;
        }
        public static bool UpdateDetainedLicense(int DetainID,
          int LicenseID, DateTime DetainDate,
          float FineFees, int CreatedByUserID)
        {
            int rowsAffected = 0;
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_UpdateDetainedLicense",conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@DetainID", DetainID);
                    cmd.Parameters.AddWithValue("@LicenseID", LicenseID);
                    cmd.Parameters.AddWithValue("@DetainDate", DetainDate);
                    cmd.Parameters.AddWithValue("@FineFees", FineFees);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            return rowsAffected > 0;
        }
        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID,
        ref int DetainID, ref DateTime DetainDate,
        ref float FineFees, ref int CreatedByUserID,
        ref bool IsReleased, ref DateTime ReleaseDate,
        ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_GetDetainedLicenseInfoByLicenseID",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@LicenseID", LicenseID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DetainID = (int)reader["DetainID"];
                            DetainDate = (DateTime)reader["DetainDate"];
                            FineFees = Convert.ToSingle(reader["FineFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsReleased = (bool)reader["IsReleased"];

                            // ممكن يكون بعض الأعمدة NULL في قاعدة البيانات
                            ReleaseDate = reader["ReleaseDate"] != DBNull.Value ? (DateTime)reader["ReleaseDate"] : DateTime.MinValue;
                            ReleasedByUserID = reader["ReleasedByUserID"] != DBNull.Value ? (int)reader["ReleasedByUserID"] : -1;
                            ReleaseApplicationID = reader["ReleaseApplicationID"] != DBNull.Value ? (int)reader["ReleaseApplicationID"] : -1;

                            isFound = true;
                        }
                    }
                }
            }
            return isFound;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool isFound = false;
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_IsLicenseDetained", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LicenseID", LicenseID);
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                        isFound = Convert.ToInt32(result) == 1;
                }
            }
            return isFound;
        }

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_GetAllDetainedLicenses",conn))
                {
                    cmd.CommandType= CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }


    }
}
