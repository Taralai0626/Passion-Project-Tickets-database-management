using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using StoreTicket.Models;
using System.Diagnostics;

namespace StoreTicket.Controllers
{
    public class TicketDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TicketData/ListTickets
        [HttpGet]
        [ResponseType(typeof(TicketDto))]
        public IHttpActionResult ListTickets()
        {
            List<Ticket> Tickets = db.Tickets.ToList();
            List<TicketDto> TicketDtos = new List<TicketDto>();

            Tickets.ForEach(t => TicketDtos.Add(new TicketDto()
            {
                TicketId = t.TicketId,
                WebsiteId = t.Website.WebsiteId,
                WebsiteName = t.Website.WebsiteName,
                EventName = t.EventName,
                EventType = t.EventType,
                EventVenue = t.EventVenue,
                EventLocation = t.EventLocation,
                EventDate = t.EventDate,
                TicketStartingPrice = t.TicketStartingPrice,
                TicketPhoto = t.TicketPhoto
            }));

            return Ok(TicketDtos);
        }

        [HttpGet]
        [ResponseType(typeof(TicketDto))]
        public IHttpActionResult ListTicketsForWebsite(int id)
        {
            List<Ticket> Tickets = db.Tickets.Where(t => t.WebsiteId == id).ToList();
            List<TicketDto> TicketDtos = new List<TicketDto>();

            Tickets.ForEach(t => TicketDtos.Add(new TicketDto()
            {
                TicketId = t.TicketId,
                WebsiteId = t.Website.WebsiteId,
                WebsiteName = t.Website.WebsiteName,
                EventName = t.EventName,
                EventType = t.EventType,
                EventVenue = t.EventVenue,
                EventLocation = t.EventLocation,
                EventDate = t.EventDate,
                TicketStartingPrice = t.TicketStartingPrice
            }));

            return Ok(TicketDtos);
        }

        [HttpGet]
        [ResponseType(typeof(TicketDto))]
        public IHttpActionResult ListTicketsForUser(int id)
        {
            //all ticketts that have Users which match with our ID`
            List<Ticket> Tickets = db.Tickets.Where(t => t.Users.Any(u => u.UserId == id)).ToList();
            List<TicketDto> TicketDtos = new List<TicketDto>();

            Tickets.ForEach(t => TicketDtos.Add(new TicketDto()
            {
                TicketId = t.TicketId,
                WebsiteId = t.Website.WebsiteId,
                WebsiteName = t.Website.WebsiteName,
                EventName = t.EventName,
                EventType = t.EventType,
                EventVenue = t.EventVenue,
                EventLocation = t.EventLocation,
                EventDate = t.EventDate,
                TicketStartingPrice = t.TicketStartingPrice
            }));

            return Ok(TicketDtos);
        }

        [HttpPost]
        [Route("api/Ticketdata/AssociateTicketWithUser/{ticketId}/{userId}")]   
        public IHttpActionResult AssociateTicketWithUser(int ticketId, int userId)
        {
            Ticket SelectedTicket = db.Tickets.Include(t => t.Users).Where(t => t.TicketId == ticketId).FirstOrDefault();
            Passionuser SelectedUser = db.PassionUsers.Find(userId);

            if (SelectedTicket == null || SelectedUser == null)
            {
                return NotFound();
            }

            SelectedTicket.Users.Add(SelectedUser);
            db.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("api/Ticketdata/UnAssociateTicketWithUser/{ticketId}/{userId}")]
        public IHttpActionResult UnAssociateTicketWithUser(int ticketId, int userId)
        {

            Ticket SelectedTicket = db.Tickets.Include(t => t.Users).Where(t => t.TicketId == ticketId).FirstOrDefault();
            Passionuser SelectedUser = db.PassionUsers.Find(userId);

            if (SelectedTicket == null || SelectedUser == null)
            {
                return NotFound();
            }

            //Debug.WriteLine("input animal id is: " + animalid);
            //Debug.WriteLine("selected animal name is: " + SelectedAnimal.AnimalName);
            ///Debug.WriteLine("input keeper id is: " + keeperid);
            //Debug.WriteLine("selected keeper name is: " + SelectedKeeper.KeeperFirstName);


            SelectedTicket.Users.Remove(SelectedUser);
            db.SaveChanges();

            return Ok();
        }



        // GET: api/TicketData/FindTicket/5

        [HttpGet]
        [ResponseType(typeof(TicketDto))]
        public IHttpActionResult FindTicket(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            TicketDto TicketDto = new TicketDto()
            {
                TicketId = ticket.TicketId,
                WebsiteId = ticket.Website.WebsiteId,
                WebsiteName = ticket.Website.WebsiteName,
                EventName = ticket.EventName,
                EventType = ticket.EventType,
                EventVenue = ticket.EventVenue,
                EventLocation = ticket.EventLocation,
                EventDate = ticket.EventDate,
                TicketStartingPrice = ticket.TicketStartingPrice,
                TicketPhoto = ticket.TicketPhoto,
            };
            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(TicketDto);
        }

        // post: api/TicketData/UpdateTicket/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTicket(int id, Ticket ticket)
        { //to do debug msg to figure out why the method is not working
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticket.TicketId)
            {
                return BadRequest();
            }


            db.Entry(ticket).State = EntityState.Modified;
            //db.Entry(animal).Property(a => a.AnimalHasPic).IsModified = false;
            db.Entry(ticket).Property(t => t.TicketPhoto).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public IHttpActionResult UploadTicketPic(int id)
        {

            //bool haspic = false;
            string TicketPhoto;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var ticketPic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (ticketPic.ContentLength > 0)
                    {
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif", "webp" };
                        var picOfTicket = Path.GetExtension(ticketPic.FileName).Substring(1);
                        //Check the picOfTicket of the file
                        if (valtypes.Contains(picOfTicket))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + picOfTicket;

                                //get a direct file path to ~/Content/tickets/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Tickets/"), fn);

                                //save the file
                                ticketPic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                //haspic = true;
                                TicketPhoto = picOfTicket;

                                //Update the t fields in the database
                                Ticket Selectedticket = db.Tickets.Find(id);
                               // Selectedticket.TicketHasPic = haspic;
                                Selectedticket.TicketPhoto = picOfTicket;
                                db.Entry(Selectedticket).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("ticket Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();

            }
        }

        // POST: api/TicketData/AddTicket
        [ResponseType(typeof(Ticket))]
        [HttpPost]
        public IHttpActionResult AddTicket(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tickets.Add(ticket);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ticket.TicketId }, ticket);
        }

        // DELETE: api/TicketData/DeleteTicket/5
        [ResponseType(typeof(Ticket))]
        [HttpPost]
        public IHttpActionResult DeleteTicket(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }

            if ( ticket.TicketPhoto != "")
            {
                //also delete image from path
                string path = HttpContext.Current.Server.MapPath("~/Content/Images/Tickets/" + id + "." + ticket.TicketPhoto);
                if (System.IO.File.Exists(path))
                {
                    Debug.WriteLine("File exists... preparing to delete!");
                    System.IO.File.Delete(path);
                }
            }
            db.Tickets.Remove(ticket);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TicketExists(int id)
        {
            return db.Tickets.Count(e => e.TicketId == id) > 0;
        }
    }
}