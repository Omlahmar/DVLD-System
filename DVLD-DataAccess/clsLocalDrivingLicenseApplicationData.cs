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
    public class clsLocalDrivingLicenseApplicationData
    {
        public static bool GetLocalDrivingLicenseApplicationInfoByID(
            int LocalDrivingLicenseApplicationID, ref int ApplicationID,
            ref int LicenseClassID)
        {

            bool isFound = false;
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_GetLocalDrivingLicenseApplicationInfoByID",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            ApplicationID = (int)reader["ApplicationID"];
                            LicenseClassID = (int)reader["LicenseClassID"];
                        }
                    }
                }
            }
            return isFound;
        }

        public static int AddNewLocalDrivingLicenseApplication(
           int ApplicationID, int LicenseClassID)
        {
            //this function will return the new person id if succeeded and -1 if not.
            int LocalDrivingLicenseApplicationID = -1;

            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_AddNewLocalDrivingLicenseApplication",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    cmd.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    var param = new SqlParameter("@LocalDrivingLicenseApplicationID", SqlDbType.Int);
                    param.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(param);

                    cmd.ExecuteNonQuery();

                    LocalDrivingLicenseApplicationID = (int)param.Value;
                }
            }
            return LocalDrivingLicenseApplicationID;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open ();
                using (SqlCommand cmd = new SqlCommand("SP_GetAllLocalDrivingLicenseApplications",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)

                    {
                        dt.Load(reader);
                    }

                }
            }
            return dt;

        }

        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool isFound = false;
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open ();
                using(SqlCommand cmd = new SqlCommand("SP_DoesAttendTestType",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // تمرير المعاملات حسب ما يتوقعه الإجراء المخزن
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    // نفترض أن الإجراء يرجع 1 أو 0، أو سجل إذا وجد
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        isFound = Convert.ToBoolean(result);
                    }
                }
            }
            return isFound;
        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            byte TotalTrialsPerTest = 0;
    
            using(SqlConnection conn = new SqlConnection (clsDataAccessSettings.ConnectionString))

            {
                conn.Open ();
                using(SqlCommand cmd = new SqlCommand("SP_TotalTrialsPerTest",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // تمرير المعاملات التي يتوقعها الإجراء المخزن
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    // نفترض أن الإجراء يرجع العدد (COUNT) كقيمة واحدة
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        TotalTrialsPerTest = Convert.ToByte(result);
                    }
                }
            }
            return TotalTrialsPerTest;
        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open ();
                using(SqlCommand cmd = new SqlCommand("SP_IsThereAnActiveScheduledTest",conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    // تمرير المعاملات المطلوبة
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    // نفترض أن الإجراء يرجع 1 أو 0 أو قيمة من نوع BIT
                    object dbResult = cmd.ExecuteScalar();

                    if (dbResult != null && dbResult != DBNull.Value)
                    {
                        Result = Convert.ToBoolean(dbResult);
                    }
                }
            }
            return Result;
        }
        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn
                    .Open ();
                using(SqlCommand cmd = new SqlCommand("SP_DoesPassTestType",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // تمرير المعاملات التي يتوقعها الإجراء المخزن
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    // نفترض أن الإجراء يرجع 1 (ناجح) أو 0 (راسب)
                    object dbResult = cmd.ExecuteScalar();

                    if (dbResult != null && dbResult != DBNull.Value)
                    {
                        Result = Convert.ToBoolean(dbResult);
                    }
                }
            }
            return Result;
        }
    }
}
