using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreTicket.Models.ViewModels
{
    public class DetailsUser
    {
        public PassionuserDto SelectedUser { get; set; }
        public IEnumerable<TicketDto> KeptTickets { get; set; }
    }
}