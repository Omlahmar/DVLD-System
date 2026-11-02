using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLD_DataAccess
{
    public class clsTestData
    {
        public static int AddNewTest(int TestAppointmentID, bool TestResult,
           string Notes, int CreatedByUserID)
        {
            int TestID = -1;

            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_AddNewTest",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    cmd.Parameters.AddWithValue("@TestResult", TestResult);
                    cmd.Parameters.AddWithValue("@Notes", Notes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    SqlParameter outputParam = new SqlParameter("@NewTestID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    cmd.ExecuteNonQuery();

                    TestID = (int)cmd.Parameters["@NewTestID"].Value;
                }
            }
            return TestID;
        }
        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult,
            string Notes, int CreatedByUserID)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_UpdateTest", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TestID", TestID);
                    cmd.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    cmd.Parameters.AddWithValue("@TestResult", TestResult);
                    cmd.Parameters.AddWithValue("@Notes", Notes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            return (rowsAffected > 0);


        }
        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestCount = 0;

            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_GetPassedTestCount",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // إضافة المعامل
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    // تنفيذ الاستعلام واسترجاع النتيجة
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        PassedTestCount = Convert.ToByte(result);
                    }
                }
            }
            return PassedTestCount;
        }

        public static bool GetTestInfoByID(int TestID,
           ref int TestAppointmentID, ref bool TestResult,
           ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_GetTestInfoByID",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // input parameter
                    cmd.Parameters.AddWithValue("@TestID", TestID);

                    // execute the reader
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            TestResult = (bool)reader["TestResult"];
                            Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : string.Empty;
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                        }
                        else
                        {
                            isFound = false;
                        }
                    }
                    }
            }
            return isFound;
        }
        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass
           (int PersonID, int LicenseClassID, int TestTypeID, ref int TestID,
             ref int TestAppointmentID, ref bool TestResult,
             ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_GetLastTestByPersonAndTestTypeAndLicenseClass", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // تمرير المعاملات
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    cmd.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // إذا وجد صف
                        {
                            TestID = reader.GetInt32(reader.GetOrdinal("TestID"));
                            TestAppointmentID = reader.GetInt32(reader.GetOrdinal("TestAppointmentID"));
                            TestResult = reader.GetBoolean(reader.GetOrdinal("TestResult"));
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"));
                            CreatedByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));

                            isFound = true;
                        }
                    }
                }
            }
            return isFound;
        }
    }
}
