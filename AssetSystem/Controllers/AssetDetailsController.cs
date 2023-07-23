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
using AssetSystem.Classes;
using AssetSystem.Models;

namespace AssetSystem.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class AssetDetailsController : ApiController
    {
        private AssetDBEntities db = new AssetDBEntities();

        // GET: api/AssetDetails
        public IHttpActionResult GetAssetDetails()
        {
            var userEntry = (from user in db.AssetDetails   
                             select new Classes.AssetDefining
                             {
                                 AssetEntryID = user.AssetEntryID,
                                 AssetId = user.AssetId,
                                 AssetType = user.AssetType,
                                 AssetNo = user.AssetNo,
                                 AssetDescription = user.AssetDescription,
                                 PurchaseDate = (user.PurchaseDate),
                                 WarrantyStartDate = user.WarrantyStartDate,
                                 WarrantyEndDate = user.WarrantyEndDate,
                                 Serial_No = user.Serial_No,
                                 Remarks = user.Remarks,
                                 Isdeleted = user.Isdeleted,
                                 Attachment = (user.Attachment),
                                 
                             }).ToList();

            return Ok(userEntry);
        }

        // GET: api/AssetDetails/5
        [ResponseType(typeof(Models.AssetDetail))]
        public IHttpActionResult GetAssetDetail(int id)
        {
            Models.AssetDetail assetDetail = db.AssetDetails.Find(id);
            if (assetDetail == null)
            {
                return NotFound();
            }

            return Ok(assetDetail);
        }

        // PUT: api/AssetDetails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAssetDetail(int id, Models.AssetDetail assetDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != assetDetail.AssetEntryID)
            {
                return BadRequest();
            }

            db.Entry(assetDetail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetDetailExists(id))
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

        // POST: api/AssetDetails
        [ResponseType(typeof(Models.AssetDetail))]
        public IHttpActionResult PostAssetDetail(Models.AssetDetail assetDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AssetDetails.Add(assetDetail);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = assetDetail.AssetEntryID }, assetDetail);
        }

        // DELETE: api/AssetDetails/5
        [ResponseType(typeof(Models.AssetDetail))]
        public IHttpActionResult DeleteAssetDetail(int id)
        {
            Models.AssetDetail assetDetail = db.AssetDetails.Find(id);
            if (assetDetail == null)
            {
                return NotFound();
            }

            db.AssetDetails.Remove(assetDetail);
            db.SaveChanges();

            return Ok(assetDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AssetDetailExists(int id)
        {
            return db.AssetDetails.Count(e => e.AssetEntryID == id) > 0;
        }
    }
}