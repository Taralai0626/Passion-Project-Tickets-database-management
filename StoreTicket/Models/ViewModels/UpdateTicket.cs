using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreTicket.Models.ViewModels
{
    public class UpdateTicket
    {
        public TicketDto SelectedTicket { get; set; }

        public IEnumerable<WebsiteDto> WebsiteOptions { get; set; }
    }
}