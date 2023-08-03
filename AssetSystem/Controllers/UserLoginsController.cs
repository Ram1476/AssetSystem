using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using AssetSystem.Models;

namespace AssetSystem.Controllers
{
    [EnableCors(origins: "http://localhost:8000", headers: "*", methods: "*")]
    public class UserLoginsController : ApiController
    {
        private AssetDBEntities db = new AssetDBEntities();

        // GET: api/UserLogins
       
        public IHttpActionResult GetUserLogins()
        {
            try
            { 
                var userEntry = (from user in db.UserLogins
                                 select new UserLogins
                                 {
                                     Id = user.UserId,
                                     Name = user.UserName,
                                     Password = user.UserPassword,
                                     RoleId = user.RoleId,
                                 }).ToList();
                return Ok(userEntry);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: api/UserLogins/5
        [ResponseType(typeof(UserLogin))]
        public IHttpActionResult GetUserLogin(int id)
        {
            UserLogin userLogin = db.UserLogins.Find(id);
            if (userLogin == null)
            {
                return NotFound();
            }

            return Ok(userLogin);
        }

        // PUT: api/UserLogins/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserLogin(int id, UserLogin userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userLogin.UserId)
            {
                return BadRequest();
            }

            db.Entry(userLogin).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserLoginExists(id))
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

        // POST: api/UserLogins
        [ResponseType(typeof(UserLogin))]
        public IHttpActionResult PostUserLogin(UserLogin userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserLogins.Add(userLogin);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userLogin.UserId }, userLogin);
        }

        // DELETE: api/UserLogins/5
        [ResponseType(typeof(UserLogin))]
        public IHttpActionResult DeleteUserLogin(int id)
        {
            UserLogin userLogin = db.UserLogins.Find(id);
            if (userLogin == null)
            {
                return NotFound();
            }

            db.UserLogins.Remove(userLogin);
            db.SaveChanges();

            return Ok(userLogin);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserLoginExists(int id)
        {
            return db.UserLogins.Count(e => e.UserId == id) > 0;
        }
    }
}