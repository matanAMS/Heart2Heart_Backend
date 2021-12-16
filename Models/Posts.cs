using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heart2HeartBackend.Models
{
    public class Posts
    {
        //fields
        private int _postId;
        private int _userId;
        private string _user_name;
        private string _description;
        private string _postImage;
        //private int _trackCode;
        private string _uploadDate;
        private string _userImage;
        




        //props
        public int PostId { get => _postId; set => _postId = value; }
        public int UserId { get => _userId; set => _userId = value; }
        public string Description { get => _description; set => _description = value; }
        public string PostImage { get => _postImage; set => _postImage = value; }
        //public int TrackCode { get => _trackCode; set => _trackCode = value; }
        public string UploadDate { get => _uploadDate; set => _uploadDate = value; }
        public string User_Name { get => _user_name; set => _user_name = value; }
        public string UserImage { get => _userImage; set => _userImage = value; }

        public Posts(int postId, int userId,  string description, string postImage,  string uploadDate, string user_name,string userImage)
        {
            _postId = postId;
            _userId = userId;
            _description = description;
            _postImage = postImage;
            //_trackCode = trackCode;
            _uploadDate = uploadDate;
            _user_name = user_name;
            _userImage = userImage;
        }





        //ctor


    }
}