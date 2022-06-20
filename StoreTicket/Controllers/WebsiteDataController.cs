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
using System.Diagnostics;

namespace StoreTicket.Controllers
{
    public class WebsiteDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/WebsiteData/ListWebsites
        [HttpGet]
        [ResponseType(typeof(WebsiteDto))]
        public IHttpActionResult ListWebsites()
        {   
            List<Website> Websites = db.Websites.ToList();
            List<WebsiteDto> WebsitesDtos = new List<WebsiteDto>();

            Websites.ForEach(w => WebsitesDtos.Add(new WebsiteDto()
            {
                WebsiteId = w.WebsiteId,
                WebsiteName = w.WebsiteName,
                WebsiteType = w.WebsiteType,
            }));

            return Ok(WebsitesDtos);
        }

        // GET: api/WebsiteData/FindWebsite/5
        [ResponseType(typeof(WebsiteDto))]
        [HttpGet]
        public IHttpActionResult FindWebsite(int id)
        {
            Website website = db.Websites.Find(id);
            WebsiteDto WebsiteDto = new WebsiteDto()
            {
                WebsiteId = website.WebsiteId,
                WebsiteName = website.WebsiteName,
                WebsiteType = website.WebsiteType,
            };

            if (website == null)
            {
                return NotFound();
            }

            return Ok(WebsiteDto);
        }

        // PUT: api/WebsiteData/Updatewebsite/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateWebsite(int id, Website website)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != website.WebsiteId)
            {
                return BadRequest();
            }

            db.Entry(website).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WebsiteExists(id))
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

        // POST: api/WebsiteData/Addwebsite
        [ResponseType(typeof(Website))]
        [HttpPost]
        public IHttpActionResult AddWebsite(Website website)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Websites.Add(website);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = website.WebsiteId }, website);
        }

        // DELETE: api/WebsiteData/DeleteWebsite/5
        [ResponseType(typeof(Website))]
        [HttpPost]
        public IHttpActionResult DeleteWebsite(int id)
        {
            Website website = db.Websites.Find(id);
            if (website == null)
            {
                return NotFound();
            }

            db.Websites.Remove(website);
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

        private bool WebsiteExists(int id)
        {
            return db.Websites.Count(e => e.WebsiteId == id) > 0;
        }
    }
}