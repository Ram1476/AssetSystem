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
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class UserDetailsController : ApiController
    {
        private AssetDBEntities db = new AssetDBEntities();

        // GET: api/UserDetails
        public IHttpActionResult GetUserDetails()
        {
            var userEntry = (from user in db.UserDetails
                             select new Classes.UserEntry
                             {
                                 UserEntryId = user.UserEntryId,
                                 FirstName= user.FirstName,
                                 LastName= user.LastName, 
                                 CorpId= user.CorpId,
                                 EmployeeId= user.EmployeeId,
                                 EmailAddress= user.EmailAddress,
                                 ReportingTo= user.ReportingTo,
                                 IsActive= user.IsActive,
                                 Remarks= user.Remarks,

                             }).ToList();

            return Ok(userEntry);
            
        }

        // GET: api/UserDetails/5
        [ResponseType(typeof(UserDetail))]
        public IHttpActionResult GetUserDetail(int id)
        {
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return NotFound();
            }

            return Ok(userDetail);
        }

        // PUT: api/UserDetails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserDetail(int id, UserDetail userDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userDetail.UserEntryId)
            {
                return BadRequest();
            }

            db.Entry(userDetail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDetailExists(id))
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

        // POST: api/UserDetails
        [ResponseType(typeof(UserDetail))]
        public IHttpActionResult PostUserDetail(UserDetail userDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserDetails.Add(userDetail);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userDetail.UserEntryId }, userDetail);
        }

        // DELETE: api/UserDetails/5
        [ResponseType(typeof(UserDetail))]
        public IHttpActionResult DeleteUserDetail(int id)
        {
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return NotFound();
            }

            db.UserDetails.Remove(userDetail);
            db.SaveChanges();

            return Ok(userDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserDetailExists(int id)
        {
            return db.UserDetails.Count(e => e.UserEntryId == id) > 0;
        }
    }
}