using System;
using System.Collections.Generic;

namespace wedding.Models
{
    public class User : BaseEntity
    {
        public int userID { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public List<Guest> myGuest {get;set;}
        public List<Wedding> PlannedWedding {get;set;}
        //here we created a list name myguest of type Guest
        // this list will get filled as we create users
        public User(){
            myGuest = new List<Guest>();
            PlannedWedding = new List<Wedding>();
        }
        //here we created a list name myguest of type Guest
        // this list will get filled as we create users
        }
    }