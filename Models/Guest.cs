using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wedding.Models
{
    public class Guest : BaseEntity
    {
        [Key]
        public int guestID { get; set; }
        public int userID { get; set; }
        public int weddingID { get; set; }

        public User User {get;set;}
        public Wedding Wedding {get;set;}
        }
    }