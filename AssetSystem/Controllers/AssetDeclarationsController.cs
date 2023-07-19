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
using AssetSystem.Models;
using AssetSystem.Classes;

namespace AssetSystem.Controllers
{
    public class AssetDeclarationsController : ApiController
    {
        private AssetDBEntities db = new AssetDBEntities();

        // GET: api/AssetDeclarations
        public IHttpActionResult GetAssetDeclarations()
        {
            var userEntry = (from user in db.AssetDeclarations
                             select new AssetDeclare
                             {
                                 AssetId= user.AssetId,
                                 AssertType = user.AssertType,
                                 AssertPrefix = user.AssertPrefix,
                             }).ToList();

            return Ok(userEntry);

        }

        // GET: api/AssetDeclarations/5
        [ResponseType(typeof(AssetDeclaration))]
        public IHttpActionResult GetAssetDeclaration(int id)
        {
            AssetDeclaration assetDeclaration = db.AssetDeclarations.Find(id);
            if (assetDeclaration == null)
            {
                return NotFound();
            }

            return Ok(assetDeclaration);
        }

        // PUT: api/AssetDeclarations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAssetDeclaration(int id, AssetDeclaration assetDeclaration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != assetDeclaration.AssetId)
            {
                return BadRequest();
            }

            db.Entry(assetDeclaration).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetDeclarationExists(id))
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

        // POST: api/AssetDeclarations
        [ResponseType(typeof(AssetDeclaration))]
        public IHttpActionResult PostAssetDeclaration(AssetDeclaration assetDeclaration)
        {
            try
            {


                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.AssetDeclarations.Add(assetDeclaration);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = assetDeclaration.AssetId }, assetDeclaration);
            }
            catch(Exception ex)
            {
                return StatusCode(HttpStatusCode.Conflict); 
            }
         }

        // DELETE: api/AssetDeclarations/5
        [ResponseType(typeof(AssetDeclaration))]
        public IHttpActionResult DeleteAssetDeclaration(int id)
        {
            AssetDeclaration assetDeclaration = db.AssetDeclarations.Find(id);
            if (assetDeclaration == null)
            {
                return NotFound();
            }

            db.AssetDeclarations.Remove(assetDeclaration);
            db.SaveChanges();

            return Ok(assetDeclaration);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AssetDeclarationExists(int id)
        {
            return db.AssetDeclarations.Count(e => e.AssetId == id) > 0;
        }
    }
}