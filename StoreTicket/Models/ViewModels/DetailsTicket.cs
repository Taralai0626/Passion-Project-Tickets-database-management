using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreTicket.Models.ViewModels
{
    public class DetailsTicket
    {
        public TicketDto SelectedTicket { get; set; }
        public IEnumerable<PassionuserDto> ResponsibleUsers { get; set; }

        public IEnumerable<PassionuserDto> AvailableUsers { get; set; }
    }
}