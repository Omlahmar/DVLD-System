using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace DVLD_DataAccess
{
    public class clsPersonData
    {
        public static int AddNewPerson(string FirstName, string SecondName,
          string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
          short Gendor, string Address, string Phone, string Email,
           int NationalityCountryID, string ImagePath)
        {
            int newPersonID = -1;

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_AddNewPerson", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    cmd.Parameters.AddWithValue("@SecondName", SecondName);
                    cmd.Parameters.AddWithValue("@ThirdName", ThirdName);
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", Gendor);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@Phone", Phone);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                    cmd.Parameters.AddWithValue("@ImagePath", ImagePath);

                    // OUTPUT parameter
                    SqlParameter outputIdParam = new SqlParameter("@NewPersonID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputIdParam);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    newPersonID = (int)outputIdParam.Value;

                }
            }
            return newPersonID;
           

        }

        public static bool UpdatePerson(int PersonID, string FirstName, string SecondName,
         string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
         short Gendor, string Address, string Phone, string Email,
          int NationalityCountryID, string ImagePath)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand("SP_UpdatePerson",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    cmd.Parameters.AddWithValue("@FirstName", FirstName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SecondName", SecondName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ThirdName", ThirdName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastName", LastName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NationalNo", NationalNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gendor", Gendor);
                    cmd.Parameters.AddWithValue("@Address", Address ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Phone", Phone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                    cmd.Parameters.AddWithValue("@ImagePath", ImagePath ?? (object)DBNull.Value);

                    conn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            return (rowsAffected > 0);

        }
        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SP_GetAllPeople", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }

        public static bool GetPersonInfoByID(int PersonID, ref string FirstName, ref string SecondName,
         ref string ThirdName, ref string LastName, ref string NationalNo, ref DateTime DateOfBirth,
          ref short Gendor, ref string Address, ref string Phone, ref string Email,
          ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SP_GetPersonInfoByID",connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            FirstName = (string)reader["FirstName"];
                            SecondName = (string)reader["SecondName"];

                            //ThirdName: allows null in database so we should handle null
                            if (reader["ThirdName"] != DBNull.Value)
                            {
                                ThirdName = (string)reader["ThirdName"];
                            }
                            else
                            {
                                ThirdName = "";
                            }

                            LastName = (string)reader["LastName"];
                            NationalNo = (string)reader["NationalNo"];
                            DateOfBirth = (DateTime)reader["DateOfBirth"];
                            Gendor = (byte)reader["Gendor"];
                            Address = (string)reader["Address"];
                            Phone = (string)reader["Phone"];


                            //Email: allows null in database so we should handle null
                            if (reader["Email"] != DBNull.Value)
                            {
                                Email = (string)reader["Email"];
                            }
                            else
                            {
                                Email = "";
                            }

                            NationalityCountryID = (int)reader["NationalityCountryID"];

                            //ImagePath: allows null in database so we should handle null
                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                ImagePath = (string)reader["ImagePath"];
                            }
                            else
                            {
                                ImagePath = "";
                            }

                        }
                        else
                        {
                            // The record was not found
                            isFound = false;
                        }

                         
                    }
                }
            }
            return isFound;
            
        
        }

        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName,
       ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
        ref short Gendor, ref string Address, ref string Phone, ref string Email,
        ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_GetPersonInfoByNationalNo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            FirstName = (string)reader["FirstName"];
                            SecondName = (string)reader["SecondName"];

                            //ThirdName: allows null in database so we should handle null
                            if (reader["ThirdName"] != DBNull.Value)
                            {
                                ThirdName = (string)reader["ThirdName"];
                            }
                            else
                            {
                                ThirdName = "";
                            }

                            LastName = (string)reader["LastName"];
                            NationalNo = (string)reader["NationalNo"];
                            DateOfBirth = (DateTime)reader["DateOfBirth"];
                            Gendor = (byte)reader["Gendor"];
                            Address = (string)reader["Address"];
                            Phone = (string)reader["Phone"];


                            //Email: allows null in database so we should handle null
                            if (reader["Email"] != DBNull.Value)
                            {
                                Email = (string)reader["Email"];
                            }
                            else
                            {
                                Email = "";
                            }

                            NationalityCountryID = (int)reader["NationalityCountryID"];

                            //ImagePath: allows null in database so we should handle null
                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                ImagePath = (string)reader["ImagePath"];
                            }
                            else
                            {
                                ImagePath = "";
                            }

                        }
                        else
                        {
                            // The record was not found
                            isFound = false;
                        }

                    }
                }
            }
            return isFound;
        }

        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;

            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_IsPersonExist",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NationalNo", NationalNo);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                        isFound = Convert.ToBoolean(Convert.ToInt32(result));
                }
            }
            return isFound;
        }

        public static bool DeletePerson(int PersonID)
        {
            int rowsAffected = 0;
            using(SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_DeletePerson",conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            return rowsAffected > 0;
        }
    }

}

