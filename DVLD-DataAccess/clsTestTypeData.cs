using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestTypeData
    {
        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_GetAllTestTypes",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)

                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            return dt;
        }

        public static bool GetTestTypeInfoByID(int TestTypeID,
          ref string TestTypeTitle, ref string TestDescription, ref float TestFees)
        {
            bool isFound = false;
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_GetTestTypeInfoByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            // The record was found
                            isFound = true;

                            TestTypeTitle = (string)reader["TestTypeTitle"];
                            TestDescription = (string)reader["TestTypeDescription"];
                            TestFees = Convert.ToSingle(reader["TestTypeFees"]);

                        }
                        else
                        {
                            // The record was not found
                            isFound = false;
                        }
                    }
                }
                return isFound;
            }

        }

        public static int AddNewTestType(string Title, string Description, float Fees)
        {
            int TestTypeID = -1;
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_AddNewTestType",conn))
                {
                    cmd.CommandType =CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TestTypeTitle", Title);
                    cmd.Parameters.AddWithValue("@TestTypeDescription", Description);
                    cmd.Parameters.AddWithValue("@ApplicationFees", Fees);

                    // المعامل الإخراجي
                    SqlParameter outputParam = new SqlParameter("@TestTypeID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    // تنفيذ الإجراء
                    cmd.ExecuteNonQuery();

                    // قراءة القيمة بعد التنفيذ
                    TestTypeID = Convert.ToInt32(outputParam.Value);

                }
            }
            return TestTypeID;
        }

        public static bool UpdateTestType(int TestTypeID, string Title, string Description, float Fees)
        {
            int rowsAffected = 0;

            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_UpdateTestType",conn))
                {
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    cmd.Parameters.AddWithValue("@TestTypeTitle", Title);
                    cmd.Parameters.AddWithValue("@TestTypeDescription", Description);
                    cmd.Parameters.AddWithValue("@TestTypeFees", Fees);

                    rowsAffected = cmd.ExecuteNonQuery();

                }
            }
            return rowsAffected > 0;
        }


    }
}
