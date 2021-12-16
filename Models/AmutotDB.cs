using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Heart2HeartBackend.Models
{
    public class AmutotDB
    {
        private static bool local = false;
        private static string _conStr = null;


        private static SqlConnection con;
        private static SqlCommand command;



     

        static string strConLIVEDNS = ConfigurationManager.ConnectionStrings["strConLIVEDNS"].ConnectionString;


        static AmutotDB()
        {
        
                _conStr = strConLIVEDNS;

            con = new SqlConnection(_conStr);
            command = new SqlCommand();
            command.Connection = con;
        }


        public static List<Amutot> ExecReader(string commandString)
        {
            try
            {
                List<Amutot> listToReturn = new List<Amutot>();
                con.Open();
                command.CommandText = commandString;
                SqlDataReader dataReader;

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var AMUTA_ID = (int)dataReader["AMUTA_ID"];
                    var AMUTA_NAME = dataReader["AMUTA_NAME"].ToString();
                    var AMUTA_HP = dataReader["AMUTA_HP"].ToString();
                    var AMUTA_IMAGE = dataReader["AMUTA_IMAGE"].ToString();
                    var DESCRIPTION = dataReader["DESCRIPTION"].ToString();
                    var ADDRESS = dataReader["ADDRESS"].ToString();
                    var USER_ID = (int)dataReader["USER_ID"];
                    var DISPLAY = (int)dataReader["DISPLAY"];


                    listToReturn.Add(
                        new Amutot(AMUTA_ID, AMUTA_NAME, AMUTA_HP, AMUTA_IMAGE, DESCRIPTION, ADDRESS, USER_ID,DISPLAY)
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

        //INSERT USER
        public static int InsertNewAmuta(Amutot newAmuta)
        {
            try
            {
                command.CommandText = $"INSERT INTO AMUTOT " +
                                  $" (AMUTA_NAME, AMUTA_HP , AMUTA_IMAGE , DESCRIPTION ," +
                                  $" ADDRESS,USER_ID,DISPLAY) " +
                                  $" VALUES(N'{newAmuta.AmutaName}' , N'{newAmuta.AmutaHP}' ," +
                                  $" '{newAmuta.AmutaImage}' , N'{newAmuta.Description}' ," +
                                  $" N'{newAmuta.Address}' , N'{newAmuta.UserID}', {newAmuta.Display})";

                con.Open();
                command.ExecuteNonQuery();
                int amutaID = -1;

                //select the last inserted identity (use for auto identity table)
                command.CommandText = "SELECT SCOPE_IDENTITY() as [IDENTITY]";
                SqlDataReader dataReader;
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                    amutaID = int.Parse(dataReader["IDENTITY"].ToString());
                return amutaID;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public static int UpdateAmuta(Amutot AMUTA)
        {
            try
            {
                command.CommandText = $"UPDATE AMUTOT " +
                    $" SET AMUTA_NAME = N'{AMUTA.AmutaName}' , " +
                    $" AMUTA_HP = N'{AMUTA.AmutaHP}' , " +
                    $" AMUTA_IMAGE = '{AMUTA.AmutaImage}' ," +
                    $" DESCRIPTION = N'{AMUTA.Description}' , " +
                    $" ADDRESS = N'{AMUTA.Address}' " +
                    $" WHERE AMUTA_ID ={AMUTA.AmutaID}";

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
        //Admin approve amuta from display 0 to 1
        public static int UpdateAmutaDisplay(Amutot AMUTA)
        {
            try
            {
                command.CommandText = $"UPDATE AMUTOT " +
                    $" SET DISPLAY = 1 " +
                     $" WHERE AMUTA_ID = {AMUTA.AmutaID}";

                con.Open();
               int rowsAffected = command.ExecuteNonQuery();
              
                 command.CommandText = $"UPDATE USERS " +
                        $" SET USER_TYPE = 1 " +
                        $"WHERE USER_ID = {AMUTA.UserID}";
                
                rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
                
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            }
            


        public static int DeleteAmuta(int AMUTA_ID)
        {
            try
            {
                command.CommandText = $@"DELETE FROM AMUTOT WHERE AMUTA_ID = {AMUTA_ID}";
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

        public static Amutot GetAmutaById(int amutaid)
        {
            Amutot amutaToReturn = ExecReader($"SELECT * FROM AMUTOT   WHERE AMUTA_ID = {amutaid} AND DISPLAY = 1").FirstOrDefault();
            return amutaToReturn;
        }
        public static List<Amutot> GetAllAmutot()
        {

            List<Amutot> amutotToReturn = ExecReader("SELECT * FROM AMUTOT   WHERE DISPLAY=1");
            return amutotToReturn;
        }
        public static List<Amutot> GetAllAmutotForAdmin()
        {

            List<Amutot> listToReturn = ExecReader("SELECT * FROM AMUTOT   WHERE DISPLAY=0");
            return listToReturn;
        }
    
       


        public static Amutot GetAmutaByHP(string amutaHP)
        {
            Amutot amutaToReturn = ExecReader($"SELECT * FROM AMUTOT   WHERE AMUTA_HP = {amutaHP} ").FirstOrDefault();
            return amutaToReturn;
        }

    }
}