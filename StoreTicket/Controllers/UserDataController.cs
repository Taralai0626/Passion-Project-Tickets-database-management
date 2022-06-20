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
    public class UserDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/UserData/ListUsers
        [HttpGet]
        [ResponseType(typeof(PassionuserDto))]
        public IHttpActionResult ListUsers()
        {
            List<Passionuser> PassionUsers = db.PassionUsers.ToList();
            List<PassionuserDto> PassionuserDtos = new List<PassionuserDto>();

            PassionUsers.ForEach(u => PassionuserDtos.Add(new PassionuserDto()
            {
                UserId = u.UserId,
                UserFirstName = u.UserFirstName,
                UserLastName = u.UserLastName,
                AccountName = u.AccountName,
                PhoneNnumber = u.PhoneNnumber,
                EmailAddress = u.EmailAddress,
                BirthDate = u.BirthDate,
            }));

            return Ok(PassionuserDtos);
        }
        [HttpGet]
        [ResponseType(typeof(PassionuserDto))]
        public IHttpActionResult ListUsersForTicket(int id)
        {
            List<Passionuser> Passionusers = db.PassionUsers.Where(
                u => u.Tickets.Any(
                    t => t.TicketId == id)
                ).ToList();
            List<PassionuserDto> PassionuserDtos = new List<PassionuserDto>();

            Passionusers.ForEach(u => PassionuserDtos.Add(new PassionuserDto()
            {
                UserId = u.UserId,
                UserFirstName = u.UserFirstName,
                UserLastName = u.UserLastName,
                AccountName = u.AccountName,
                PhoneNnumber = u.PhoneNnumber,
                EmailAddress = u.EmailAddress,
                BirthDate = u.BirthDate,
            }));

            return Ok(PassionuserDtos);
        }


        /// <summary>
        /// Returns Keepers in the system not caring for a particular animal.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Keepers in the database not taking care of a particular animal
        /// </returns>
        /// <param name="id">Animal Primary Key</param>
        /// <example>
        /// GET: api/KeeperData/ListKeepersNotCaringForAnimal/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PassionuserDto))]
        public IHttpActionResult ListUsersNotHavingTicket(int id)
        {
            List<Passionuser> Passionusers = db.PassionUsers.Where(
                u => !u.Tickets.Any(
                    t => t.TicketId == id)
                ).ToList();
            List<PassionuserDto> PassionuserDtos = new List<PassionuserDto>();

            Passionusers.ForEach(u => PassionuserDtos.Add(new PassionuserDto()
            {
                UserId = u.UserId,
                UserFirstName = u.UserFirstName,
                UserLastName = u.UserLastName,
                AccountName = u.AccountName,
                PhoneNnumber = u.PhoneNnumber,
                EmailAddress = u.EmailAddress,
                BirthDate = u.BirthDate,
            }));

            return Ok(PassionuserDtos);
        }
        // GET: api/UserData/FindUser/5
        [ResponseType(typeof(PassionuserDto))]
        [HttpGet]
        public IHttpActionResult FindUser(int id)
        {
            Passionuser user = db.PassionUsers.Find(id);
            PassionuserDto passionuserDto = new PassionuserDto()
            {
                UserId = user.UserId,
                UserFirstName = user.UserFirstName,
                UserLastName = user.UserLastName,
                AccountName = user.AccountName,
                PhoneNnumber = user.PhoneNnumber,
                EmailAddress = user.EmailAddress,
                BirthDate = user.BirthDate,
            };
            if (user == null)
            {
                return NotFound();
            }

            return Ok(passionuserDto);
        }

        // POST: api/UserData/UpdateUser/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateUser(int id, Passionuser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/UserData/AddUser
        [ResponseType(typeof(Passionuser))]
        [HttpPost]
        public IHttpActionResult AddUser(Passionuser passionuser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PassionUsers.Add(passionuser);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = passionuser.UserId }, passionuser);
        }

        // POST: api/UserData/DeleteUser/5
        [ResponseType(typeof(Passionuser))]
        [HttpPost]
        public IHttpActionResult DeleteUser(int id)
        {
            Passionuser passionuser = db.PassionUsers.Find(id);
            if (passionuser == null)
            {
                return NotFound();
            }

            db.PassionUsers.Remove(passionuser);
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

        private bool UserExists(int id)
        {
            return db.PassionUsers.Count(e => e.UserId == id) > 0;
        }
    }
}