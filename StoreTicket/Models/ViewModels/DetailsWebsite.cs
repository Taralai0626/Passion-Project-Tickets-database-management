using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreTicket.Models.ViewModels
{
    public class DetailsWebsite
    {
        //the species itself that we want to display
        public WebsiteDto SelectedWebsite { get; set; }

        public IEnumerable<WebsiteDto> ForUpdateWebsite { get; set; }
    

        //all of the related animals to that particular species
        public IEnumerable<TicketDto> RelatedTickets { get; set; }
    }
}