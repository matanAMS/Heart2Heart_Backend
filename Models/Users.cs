using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heart2HeartBackend.Models
{
    public class Users
    {
        //fields
        private int _userId;
        private string _userName;
        private string _password;
        private string _email;
        private string _firstName;
        private string _lastName;
        private string _userImage;
        private char _gender;
        private int _userType;
        private string _description;


        //props
        public int UserId { get => _userId; set => _userId = value; }
        public string UserName { get => _userName; set => _userName = value; }
        public string Password { get => _password; set => _password = value; }
        public string Email { get => _email; set => _email = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string UserImage { get => _userImage; set => _userImage = value; }
        public char Gender { get => _gender; set => _gender = value; }
        public int UserType { get => _userType; set => _userType = value; }
        public string Description { get => _description; set => _description = value; }


        //ctor
        public Users()
        {

        }
        public Users(int userId, string userName, string password, string email, string firstName, string lastName, string userImage, char gender, int userType, string description)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            UserImage = userImage;
            Gender = gender;
            UserType = userType;
            Description = description;
        }
    }
}