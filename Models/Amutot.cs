using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heart2HeartBackend.Models
{
    public class Amutot
    {
        private int amutaID;
        private string amutaName;
        private string amutaHP;
        private string amutaImage;
        private string description;
        private string address;
        private int userID;
        private int display;





        //props
        public int AmutaID { get => amutaID; set => amutaID = value; }
        public string AmutaName { get => amutaName; set => amutaName = value; }
        public string AmutaHP { get => amutaHP; set => amutaHP = value; }
        public string AmutaImage { get => amutaImage; set => amutaImage = value; }
        public string Description { get => description; set => description = value; }
        public string Address { get => address; set => address = value; }
        public int UserID { get => userID; set => userID = value; }
        public int Display { get => display; set => display = value; }

        //ctor
        public Amutot(int amutaID, string amutaName, string amutaHP, string amutaImage, string description, string address, int userID,int display)
        {
            this.amutaID = amutaID;
            this.amutaName = amutaName;
            this.amutaHP = amutaHP;
            this.amutaImage = amutaImage;
            this.description = description;
            this.address = address;
            this.userID = userID;
            this.display = display;
        }
    }
}