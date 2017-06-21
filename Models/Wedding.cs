using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wedding.Models
{
    public class Wedding : BaseEntity
    {
        public int weddingID { get; set; }
        [Required(ErrorMessage="Wedder one name cannot be less than 2 characters!")]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string wed_one { get; set; }

        [Required(ErrorMessage="Wedder two name cannot be less than 2 characters!")]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string wed_two { get; set; }

        [Required(ErrorMessage="Date cannout be left blank and should be from the future!")]
        public DateTime date { get; set; }

        [Required(ErrorMessage = "address cannot be left empty and should be more than 2 charcters")]
        [MinLength(2)]
        public string address { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public  int userId{get; set;}
        public List<Guest> WedGuestList {get;set;}
        //here we created a list name Trans of type Transaction
        // this list will get filled as we create users
        public Wedding(){
            WedGuestList = new List<Guest>();
        }

    }
}