using Heart2HeartBackend.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Heart2HeartBackend.Models
{
    public class UsersDB
    {

        private static bool local = false;
        private static string _conStr = null;


        private static SqlConnection con;
        private static SqlCommand command;



      
        static string strConLIVEDNS = ConfigurationManager.ConnectionStrings["strConLIVEDNS"].ConnectionString;


        static UsersDB()
        {
     
                _conStr = strConLIVEDNS;

            con = new SqlConnection(_conStr);
            command = new SqlCommand();
            command.Connection = con;
        }

        private static List<Users> ExecReader(string commandString)
        {
            try
            {
                List<Users> listToReturn = new List<Users>();
                con.Open();
                command.CommandText = commandString;
                SqlDataReader dataReader;

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var USER_ID = (int)dataReader["USER_ID"];
                    var USER_NAME = dataReader["USER_NAME"].ToString();
                    var PASSWORD = dataReader["PASSWORD"].ToString();
                    var EMAIL = dataReader["EMAIL"].ToString();
                    var FIRST_NAME = dataReader["FIRST_NAME"].ToString();
                    var LAST_NAME = dataReader["LAST_NAME"].ToString();
                    var USER_IMAGE = dataReader["USER_IMAGE"].ToString();
                    var GENDER = (char)(dataReader["GENDER"]).ToString()[0];
                    var USER_TYPE = (int)dataReader["USER_TYPE"];
                    var DESCRIPTION = dataReader["DESCRIPTION"].ToString();

                    listToReturn.Add(
                        new Users(USER_ID, USER_NAME, PASSWORD, EMAIL, FIRST_NAME, LAST_NAME, USER_IMAGE, GENDER, USER_TYPE, DESCRIPTION)
                        );
                }

                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        public SqlConnection connect()
        {

            SqlConnection con = new SqlConnection(_conStr);
            con.Open();
            return con;
        }// connect to database

        public Messages GetMessages(int id)
        {
            Messages m = null;
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand($"SELECT * FROM MESSAGES WHERE TO_USER =N'{id}' ", con);
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                m = new Messages(Convert.ToInt32(reader["FROM_USER"]), Convert.ToInt32(reader["TO_USER"]), Convert.ToString(reader["DATE_TIME"]),  Convert.ToString(reader["MESSAGE_DESC"]));

            }
            con.Close();
            return m;
        }

        


        internal static Users GetUserById(int userid)
        {
            Users userToReturn = ExecReader($"SELECT * FROM USERS   WHERE USER_ID = {userid} ").FirstOrDefault();
            return userToReturn;
        }
        public Object getUser(string email, string pass) // show user by id
        {
            Users u = null;
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand($"SELECT * FROM USERS WHERE EMAIL =N'{email}' and PASSWORD ='{pass}' ", con);
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                u = new Users(Convert.ToInt32(reader["USER_ID"]), Convert.ToString(reader["USER_NAME"]), Convert.ToString(reader["PASSWORD"]), Convert.ToString(reader["EMAIL"]), Convert.ToString(reader["FIRST_NAME"]), Convert.ToString(reader["LAST_NAME"]), Convert.ToString(reader["USER_IMAGE"]), Convert.ToChar(reader["GENDER"]), Convert.ToInt32(reader["USER_TYPE"]), Convert.ToString(reader["DESCRIPTION"]));

            }
            con.Close();
            if (u == null)
            {
                Object a = null;
                con.Open();
                SqlCommand comm1 = new SqlCommand($"SELECT * FROM AMUTOT WHERE EMAIL =N'{email}' and PASSWORD ='{pass}' ", con);
                reader = comm1.ExecuteReader();
                while (reader.Read())
                {
                    a = new Amutot(Convert.ToInt32(reader["AMUTA_ID"]), Convert.ToString(reader["AMUTA_NAME"]), Convert.ToString(reader["AMUTA_HP"]), Convert.ToString(reader["AMUTA_IMAGE"]), Convert.ToString(reader["DESCRIPTION"]), Convert.ToString(reader["ADDRESS"]), Convert.ToInt32(reader["USER_ID"]),Convert.ToInt32(reader["DISPLAY"]));

                }
                return a;
            }
            return u;



        }


        //INSERT USER
        public static int InsertNewUser(Users newUser)
        {

            try
            {
                command.CommandText = $"INSERT INTO USERS " +
                                  $" (USER_NAME, PASSWORD , EMAIL , FIRST_NAME ," +
                                  $" LAST_NAME , USER_IMAGE , GENDER , " +
                                  $" USER_TYPE , DESCRIPTION) " +
                                  $" VALUES(N'{newUser.UserName}' , N'{newUser.Password}' ," +
                                  $" '{newUser.Email}' , N'{newUser.FirstName}' ," +
                                  $" N'{newUser.LastName}' , N'{newUser.UserImage}' ," +
                                  $" '{newUser.Gender}' , {newUser.UserType} ," +
                                  $" N'{newUser.Description}') ";

                con.Open();
                command.ExecuteNonQuery();
                int userId = -1;

                //select the last inserted identity (use for auto identity table)
                command.CommandText = "SELECT SCOPE_IDENTITY() as [IDENTITY]";
                SqlDataReader dataReader;
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                    userId = int.Parse(dataReader["IDENTITY"].ToString());
                return userId;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        //READ USER BY EMAIL AND PASSWORD
        public static Users GetUserByEmailAndPassword(string email, string password)
        {
            Users user = ExecReader($"SELECT * FROM USERS WHERE USERS.EMAIL = '{email}' AND USERS.PASSWORD = '{password}'").FirstOrDefault();
            return user;
        }

        //READ USER BY EMAIL
        public static Users GetUserByEmail(string email)
        {//injection
            Users user = ExecReader($"SELECT * FROM USERS WHERE USERS.EMAIL = '{email}'").FirstOrDefault();

            return user;
        }

        //READ ALL USERS
        public static List<Users> GetAllUsers()
        {
            List<Users> listToReturn = ExecReader("SELECT * FROM USERS");

            return listToReturn;
        }


        //UPDATE USER
        public static int UpdateUser(Users user)
        {
            try
            {
                command.CommandText = $"UPDATE USERS " +
                    $" SET USER_NAME = N'{user.UserName}' , " +
                    $" PASSWORD = N'{user.Password}' , " +
                    $" EMAIL = '{user.Email}' ," +
                    $" FIRST_NAME = N'{user.FirstName}' , " +
                    $" LAST_NAME = N'{user.LastName}' ," +
                    $" USER_IMAGE = N'{user.UserImage}' ," +
                    $" GENDER = '{user.Gender}' ," +
                    $" USER_TYPE = {user.UserType} ," +
                    $" DESCRIPTION = N'{user.Description}' " +
                    $" WHERE USER_ID = {user.UserId}";

                con.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }


        //DELETE USER
        public static int DeleteUser(int USER_ID)
        {
            try
            {
                command.CommandText = $@"DELETE FROM USERS WHERE USER_ID = {USER_ID}";
                con.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public static int UserForgotPassword(string Email, string Password)
        {
            try
            {
                command.CommandText = $"UPDATE USERS " +
                    $" SET  PASSWORD = N'{Password}'  " +
                    $" WHERE EMAIL = '{Email}'";

                con.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }

}