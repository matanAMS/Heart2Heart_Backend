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
    public class PostsDB
    {

        private static bool local = false;
        private static string _conStr = null;


        private static SqlConnection con;
        private static SqlCommand command;


       
        private static string strConLIVEDNS = ConfigurationManager.ConnectionStrings["strConLIVEDNS"].ConnectionString;


        static PostsDB()
        {
       
            _conStr = strConLIVEDNS;

            con = new SqlConnection(_conStr);
            command = new SqlCommand();
            command.Connection = con;
        }

        public static List<Comments> GetCommentsForPost(int postid)
        {
            try
            {
                List<Comments> listToReturn = ExecReaderComments($"SELECT * FROM CommentsView WHERE POST_ID={postid} ORDER BY DATE_TIME DESC");
                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private static List<Posts> ExecReader(string commandString)
        {
            try
            {

                List<Posts> listToReturn = new List<Posts>();
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Open();
                command.CommandText = commandString;
                SqlDataReader dataReader;

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var POST_ID = (int)dataReader["POST_ID"];
                    var USER_ID = (int)dataReader["USER_ID"];
                    var DESCRIPTION = dataReader["DESCRIPTION"].ToString();
                    var POST_IMAGE = (dataReader["POST_IMAGE"] != DBNull.Value ? dataReader["POST_IMAGE"] : string.Empty).ToString();
                    //var TRACK_CODE = (int)(dataReader["TRACK_CODE"] != DBNull.Value ? dataReader["TRACK_CODE"] : -1);
                    var UPLOAD_DATE = (dataReader["UPLOAD_DATE"]!= DBNull.Value ? dataReader["UPLOAD_DATE"] : string.Empty).ToString();
                    var USER_NAME = dataReader["USER_NAME"].ToString();
                    var USER_IMAGE = (dataReader["USER_IMAGE"] != DBNull.Value ? dataReader["USER_IMAGE"] : string.Empty).ToString();


                    listToReturn.Add(new Posts(POST_ID, USER_ID, DESCRIPTION, POST_IMAGE, UPLOAD_DATE, USER_NAME,USER_IMAGE));
                }

                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public static int InsertNewComment(Comments comment)
        {
            try
            {
                command.CommandText = $"INSERT INTO COMMENTS " +
                                  $" (USER_ID,POST_ID,DATE_TIME,MESSAGE) " +
                                  $" VALUES({comment.UserId}, {comment.PostId} , N'{comment.Date}' , N'{comment.Message}') ";

                con.Open();
                command.ExecuteNonQuery();
                return comment.PostId;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private static List<Comments> ExecReaderComments(string commandString)
        {
            try
            {

                List<Comments> listToReturn = new List<Comments>();
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Open();
                command.CommandText = commandString;
                SqlDataReader dataReader;

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var POST_ID = (int)dataReader["POST_ID"];
                    var USER_ID = (int)dataReader["USER_ID"];
                
                    var DATE_TIME = (dataReader["DATE_TIME"] != DBNull.Value ? dataReader["DATE_TIME"] : string.Empty).ToString();
                    var USER_NAME = dataReader["USER_NAME"].ToString();
                    var MESSAGE = (dataReader["MESSAGE"] != DBNull.Value ? dataReader["MESSAGE"] : string.Empty).ToString();


                    listToReturn.Add(new Comments(POST_ID, USER_ID, DATE_TIME, USER_NAME, MESSAGE));
                }

                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }



        public static List<Posts> GetPostById(int userid)
        {
            List<Posts> listToReturn = new List<Posts>();
            SqlConnection con = new SqlConnection(_conStr);
            con.Open();
            SqlCommand comm = new SqlCommand($"SELECT * FROM POSTS WHERE USER_ID =N'{userid}' ", con);
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                listToReturn.Add(new Posts(Convert.ToInt32(reader["POST_ID"]), Convert.ToInt32(reader["USER_ID"]), Convert.ToString(reader["DESCRIPTION"]), Convert.ToString(reader["POST_IMAGE"]),  Convert.ToString(reader["UPLOAD_DATE"]), Convert.ToString(reader["USER_NAME"]), Convert.ToString(reader["POST_IMAGE"])));

            }
            con.Close();
            return listToReturn;

        }

        public static List<Posts> PostsOfAmutotByUserId(int id)
        {
            List<Posts> listToReturn = new List<Posts>();
            SqlConnection con = new SqlConnection(_conStr);
            con.Open();
            SqlCommand comm = new SqlCommand($"SELECT * FROM POSTS WHERE USER_ID =N'{id}' ", con);
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                listToReturn.Add(new Posts(Convert.ToInt32(reader["POST_ID"]), Convert.ToInt32(reader["USER_ID"]), Convert.ToString(reader["DESCRIPTION"]), Convert.ToString(reader["POST_IMAGE"]), Convert.ToString(reader["UPLOAD_DATE"]), Convert.ToString(reader["USER_NAME"]), Convert.ToString(reader["POST_IMAGE"])));

            }
            con.Close();
            return listToReturn;
        }

        //READ ALL POSTS
        public static List<Posts> GetAllPosts()
        {
            try
            {
                List<Posts> listToReturn = ExecReader("SELECT * FROM POSTbyUSER WHERE USER_TYPE=1 ORDER BY UPLOAD_DATE DESC");
                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public static List<Posts> GetAllPostsForAmutot()
        {
            try
            {
                List<Posts> listToReturn = ExecReader("SELECT * FROM POSTbyUSER  ORDER BY UPLOAD_DATE DESC");
                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }



        //INSERT POST
        public static int InsertNewPost(Posts newPost)
        {
            try
            {
                command.CommandText = $"INSERT INTO POSTS " +
                                  $" (USER_ID,DESCRIPTION,POST_IMAGE,UPLOAD_DATE,USER_NAME) " +
                                  $" VALUES({newPost.UserId}, N'{newPost.Description}' , N'{newPost.PostImage}' , '{newPost.UploadDate}','{newPost.User_Name}' ) ";

                con.Open();
                command.ExecuteNonQuery();
                int postId = -1;

                //select the last inserted identity (use for auto identity table)
                command.CommandText = "SELECT SCOPE_IDENTITY() as [IDENTITY]";
                SqlDataReader dataReader;
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                    postId = int.Parse(dataReader["IDENTITY"].ToString());
                return postId;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }


        //UPDATE POST
        public static int UpdatePost(Posts post)
        {
            try
            {
                command.CommandText = $"UPDATE POSTS SET DESCRIPTION = N'{post.Description}' , " +
                    $" USER_ID = {post.UserId} , POST_IMAGE = N'{post.PostImage}  WHERE POST_ID = {post.PostId}";


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




        //DELETE POST
        public static int DeletePost(int POST_ID)
        {
            try
            {
                command.CommandText = $@"DELETE FROM POSTS WHERE POST_ID = {POST_ID}";

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