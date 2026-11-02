using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestAppointmentData
    {
        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;

            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_GetTestID",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                    object result = cmd.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int value))
                    {
                        TestID = value;
                    }
                }
            }
            return TestID;
        }
        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dt = new DataTable();
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_GetApplicationTestAppointmentsPerTestType",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    

                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }
        public static bool GetTestAppointmentInfoByID(int TestAppointmentID,
          ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
          ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_GetTestAppointmentInfoByID",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            TestTypeID = (int)reader["TestTypeID"];
                            LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                            AppointmentDate = (DateTime)reader["AppointmentDate"];
                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsLocked = Convert.ToBoolean(reader["IsLocked"]);

                            // تحقق إذا العمود ممكن يكون null
                            if (reader["RetakeTestApplicationID"] != DBNull.Value)
                                RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                            else
                                RetakeTestApplicationID = -1;
                        }
                    }
                }
            }
            return isFound;
        }

        public static int AddNewTestAppointment(
            int TestTypeID, int LocalDrivingLicenseApplicationID,
            DateTime AppointmentDate, float PaidFees, int CreatedByUserID, int RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;

            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_AddNewTestAppointment",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    cmd.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                    cmd.Parameters.AddWithValue("@PaidFees", PaidFees);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    if (RetakeTestApplicationID == -1)

                        cmd.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
                    // تنفيذ الإجراء واسترجاع ID الموعد الجديد
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                        TestAppointmentID = Convert.ToInt32(result);
                }
            }
            return TestAppointmentID;
        }
        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID,
             DateTime AppointmentDate, float PaidFees,
             int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            int rowsAffected = 0;

            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_UpdateTestAppointment",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // تمرير المعاملات
                    cmd.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    cmd.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                    cmd.Parameters.AddWithValue("@PaidFees", Convert.ToDecimal(PaidFees));
                    cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    cmd.Parameters.AddWithValue("@IsLocked", IsLocked);

                    // إذا كان RetakeTestApplicationID فارغًا نمرر DBNull
                    if (RetakeTestApplicationID == -1)

                        cmd.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

                    // تنفيذ الإجراء وتسجيل عدد الصفوف المتأثرة
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }

            return rowsAffected > 0;
            
        }
        
    }
}
