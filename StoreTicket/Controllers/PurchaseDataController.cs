using System;
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

namespace StoreTicket.Controllers
{
    public class PurchaseDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PurchaseData/Listpurchases
        [HttpGet]
        [ResponseType(typeof(PurchaseDto))]
        public IEnumerable<PurchaseDto> ListPurchases()
        {
            List<Purchase> Purchases = db.Purchases.ToList();
            List<PurchaseDto> PurchaseDtos = new List<PurchaseDto>();

            Purchases.ForEach(a => PurchaseDtos.Add(new PurchaseDto()
            {
                PurchaseId = a.PurchaseId,
                UserId = a.Users.UserId,
                TicketId = a.Tickets.TicketId,
                PurchaseDate = a.PurchaseDate,
                PurchasePrice = a.PurchasePrice,
                NumberOfTicket = a.NumberOfTicket,

            }));

            return PurchaseDtos;
        }

        [HttpGet]
        [ResponseType(typeof(PurchaseDto))]
        public IHttpActionResult ListPurchaseForUser(int id)
        {
            List<Purchase> Purchases = db.Purchases.Where(a => a.UserId == id).ToList();
            //List<Purchase> Purchases = db.Purchases.Where(a => a.TicketId == id).ToList();
            List<PurchaseDto> PurchaseDtos = new List<PurchaseDto>();

            Purchases.ForEach(a => PurchaseDtos.Add(new PurchaseDto()
            {
                PurchaseId = a.PurchaseId,
                UserId = a.Users.UserId,
                TicketId = a.Tickets.TicketId,
                PurchaseDate = a.PurchaseDate,
                PurchasePrice = a.PurchasePrice,
                NumberOfTicket = a.NumberOfTicket,
            }));

            return Ok(PurchaseDtos);
        }

        [HttpGet]
        [ResponseType(typeof(PurchaseDto))]
        public IHttpActionResult ListPurchaseForTicket(int id)
        {
            List<Purchase> Purchases = db.Purchases.Where(a => a.TicketId == id).ToList();
            //List<Purchase> Purchases = db.Purchases.Where(a => a.TicketId == id).ToList();
            List<PurchaseDto> PurchaseDtos = new List<PurchaseDto>();

            Purchases.ForEach(a => PurchaseDtos.Add(new PurchaseDto()
            {
                PurchaseId = a.PurchaseId,
                UserId = a.Users.UserId,
                TicketId = a.Tickets.TicketId,
                PurchaseDate = a.PurchaseDate,
                PurchasePrice = a.PurchasePrice,
                NumberOfTicket = a.NumberOfTicket,
            }));

            return Ok(PurchaseDtos);
        }

        // GET: api/PurchaseData/FindPurchase/5
        [ResponseType(typeof(Purchase))]
        [HttpGet]
        public IHttpActionResult FindPurchase(int id)
        {
            Purchase purchase = db.Purchases.Find(id);
            PurchaseDto PurchaseDto = new PurchaseDto()
            {
                PurchaseId = purchase.PurchaseId,
                UserId = purchase.Users.UserId,
                TicketId = purchase.Tickets.TicketId,
                PurchaseDate = purchase.PurchaseDate,
                PurchasePrice = purchase.PurchasePrice,
                NumberOfTicket = purchase.NumberOfTicket,

            };
            if (purchase == null)
            {
                return NotFound();
            }

            return Ok(PurchaseDto);
        }


        // PUT: api/PurchaseData/UpdatePurchase/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult PutPurchase(int id, Purchase purchase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != purchase.PurchaseId)
            {
                return BadRequest();
            }

            db.Entry(purchase).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
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

        // POST: api/PurchaseData/AddPurchase
        [ResponseType(typeof(Purchase))]
        public IHttpActionResult PostPurchase(Purchase purchase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Purchases.Add(purchase);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = purchase.PurchaseId }, purchase);
        }

        // DELETE: api/PurchaseData/DeletePurchase/5
        [ResponseType(typeof(Purchase))]
        [HttpPost]
        public IHttpActionResult DeletePurchase(int id)
        {
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return NotFound();
            }

            db.Purchases.Remove(purchase);
            db.SaveChanges();

            return Ok(purchase);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PurchaseExists(int id)
        {
            return db.Purchases.Count(e => e.PurchaseId == id) > 0;
        }
    }
}