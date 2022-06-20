using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StoreTicket.Models
{
    public class Passionuser
    {
        [Key]
        public int UserId { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string AccountName { get; set; }
        //user can create a random name

        public string PhoneNnumber { get; set; }

        public string EmailAddress { get; set; }

        public DateTime BirthDate { get; set; }
        // a user can have many tickets
        // a ticke
        public ICollection<Ticket> Tickets { get; set; }
    }

    public class PassionuserDto
    {
        [Key]
        public int UserId { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string AccountName { get; set; }
        //user can create a random name

        public string PhoneNnumber { get; set; }

        public string EmailAddress { get; set; }

        public DateTime BirthDate { get; set; }
        // a user can have many tickets
        // a ticke
    }
}