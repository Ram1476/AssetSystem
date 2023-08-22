using AssetSystem;
using MVC_Asset.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PagedList.Mvc;
using PagedList;


namespace MVC_Asset.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AssetDetailController : Controller
    {
        AssetManagementDBEntities db = new AssetManagementDBEntities();
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AssetDB"].ConnectionString);
        string toError = ConfigurationManager.AppSettings["roError"];
        
        // GET: AssetDetail
        public ActionResult Index(int? i)
        {
            if (User.IsInRole("User"))
            {
                ViewBag.Role = "User";
            }
            else if (User.IsInRole("Admin"))
            {
                ViewBag.Role = "Admin";
            }
            else
            {
                ViewBag.Role = "Guest";
            }
            try
            {

                IEnumerable<AssetDetails> addAsset = null;
                HttpResponseMessage response = GlobalVariables.Clients.GetAsync("api/AssetDetails").Result;
                addAsset = response.Content.ReadAsAsync<IEnumerable<AssetDetails>>().Result;
                ViewBag.Message = TempData["Message"];
                List<AssetDetails> newDetails = new List<AssetDetails>();

                foreach (var item in addAsset)
                {
                    AssetDetails details = new AssetDetails();
                    details.AssetNo = item.AssetNo;
                    details.AssetId = item.AssetId;
                    details.AssetEntryID = item.AssetEntryID;
                    details.WarrantyEndDate = item.WarrantyEndDate;
                    details.WarrantyStartDate = item.WarrantyStartDate;
                    details.AssetType = item.AssetType;
                    details.Isdeleted = item.Isdeleted;
                    details.Attachment = item.Attachment;
                    details.PurchaseDate = item.PurchaseDate;
                    details.Serial_No = item.Serial_No;
                    details.Remarks = item.Remarks;
                    details.AssetDescription = item.AssetDescription;
                    details.Attachment = item.Attachment;
                    //if (item.Attachment != null)
                    //{
                    //    details.ImagePath = Encoding.ASCII.GetString(item.Attachment);
                    //}

                    newDetails.Add(details);
                }

                return View(newDetails.ToPagedList(i ?? 1, 5)) ;
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect(toError);
            }
        }

        public ActionResult Create(int id = 0)
        {
            try
            {
                var newEntry = ViewGenerator();
                ViewBag.Message = new SelectList(newEntry, "AssetType", "AssetType");

                if (id == 0)
                {

                    return View(new AssetDetails());
                }
                else
                {
                    HttpResponseMessage response = GlobalVariables.Clients.GetAsync("api/AssetDetails/" + id.ToString()).Result;
                    var result = response.Content.ReadAsAsync<AssetDetails>().Result;
                    
                    return View(result);
                }
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect(toError);

            }
        }

        [HttpPost]
        public ActionResult Create(AssetDetails Asset, List<HttpPostedFileBase> Attach)
        {
            try
            {
                MultipleAttachment newAttach = new MultipleAttachment();

                if (Attach != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(Attach[0].FileName);
                    string extension = Path.GetExtension(Attach[0].FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                    Asset.ImagePath = "~/image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/image/"), fileName);
                    Attach[0].SaveAs(fileName);
                    //Asset.Attachment = new byte[Attach.ContentLength];
                    //Attach.InputStream.Read(Asset.Attachment, 0, Attach.ContentLength);

                    //Asset.Attachment = Encoding.ASCII.GetBytes(Asset.ImagePath);
                    Asset.Attachment = Asset.ImagePath;
                    //Asset.Attachment = new byte[Attach.ContentLength];
                    //Attach.InputStream.Read(Asset.Attachment, 0, Attach.ContentLength);
                    string addPath = string.Empty;




                    if (Asset.AssetEntryID == 0)
                    {
                        HttpResponseMessage response = GlobalVariables.Clients.PostAsJsonAsync("api/AssetDetails", Asset).Result;
                        var result = response.Content.ReadAsAsync<IList<AssetDetails>>();

                        if (response.IsSuccessStatusCode)
                        {
                            var findTable = db.AssetDetails.Where (x => x.AssetNo == Asset.AssetNo).FirstOrDefault();
                            if (Attach.Count > 1)
                            {
                                foreach (var item in Attach)
                                {
                                    string attachName = Path.GetFileNameWithoutExtension(item.FileName);
                                    string attachextension = Path.GetExtension(item.FileName);
                                    attachName = attachName + DateTime.Now.ToString("yymmssfff") + attachextension;
                                    Asset.ImagePath = "~/image/" + attachName;
                                    attachName = Path.Combine(Server.MapPath("~/image/"), attachName);
                                    item.SaveAs(attachName);
                                    addPath = addPath + Asset.ImagePath + ";";
                                }
                                newAttach.AssetId = findTable.AssetEntryID;
                                newAttach.Attachments = addPath;

                                db.MultipleAttachments.Add(newAttach);
                                db.SaveChanges();
                            }

                            if (result != null)
                            {
                                TempData["SuccessMessage"] = "Saved SuccessFully";
                            }
                            else
                            {
                                TempData["ErrorMesssage"] = "No Data Available To Save ";
                            }

                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server Error Occured, Please check with the Admin!");
                        }
                    }
                    else
                    {
                        HttpResponseMessage response = GlobalVariables.Clients.PutAsJsonAsync("api/AssetDetails/" + Asset.AssetEntryID, Asset).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "Updated SuccessFully";
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server Error Occured, Please check with the Admin!");
                        }
                    }

                    
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect(toError);
            }
        }

        public ActionResult Edit(int id = 0)
        {
            try
            {


                var newEntry = ViewGenerator();
                ViewBag.Message = new SelectList(newEntry, "AssetType", "AssetType");
                if (id == 0)
                {

                    return View(new AssetDetails());
                }
                else
                {
                    List<int> addAsset = new List<int> { 1 };
                    HttpResponseMessage response = GlobalVariables.Clients.GetAsync("api/AssetDetails/" + id.ToString()).Result;
                    var result = response.Content.ReadAsAsync<AssetDetails>().Result;

                    List<AssetDetails> newDetails = new List<AssetDetails>();
                    AssetDetails details = new AssetDetails();
                    foreach (var item in addAsset)
                    {

                        details.AssetNo = result.AssetNo;
                        details.AssetId = result.AssetId;
                        details.AssetEntryID = result.AssetEntryID;
                        details.WarrantyEndDate = result.WarrantyEndDate;
                        details.WarrantyStartDate = result.WarrantyStartDate;
                        details.AssetType = result.AssetType;
                        details.Isdeleted = result.Isdeleted;
                        details.Attachment = result.Attachment;
                        details.PurchaseDate = result.PurchaseDate;
                        details.Serial_No = result.Serial_No;
                        details.Remarks = result.Remarks;
                        details.AssetDescription = result.AssetDescription;
                        details.Attachment = result.Attachment;
                        //if (result.Attachment != null)
                        //{
                        //    details.ImagePath = Encoding.ASCII.GetString(result.Attachment);
                        //}
                    }
                    return View(details);

                }
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect(toError);
            }
        }

        [HttpPost]
        public ActionResult Edit(AssetDetails Asset, HttpPostedFileBase Attach)
        {
            try
            {


                var newEntry = ViewGenerator();
                ViewBag.Message = new SelectList(newEntry, "AssetType", "AssetType");
                if (Attach != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(Attach.FileName);
                    string extension = Path.GetExtension(Attach.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                    Asset.ImagePath = "~/image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/image/"), fileName);
                    Attach.SaveAs(fileName);
                    //Asset.Attachment = new byte[Attach.ContentLength];
                    //Attach.InputStream.Read(Asset.Attachment, 0, Attach.ContentLength);

                    //Asset.Attachment = Encoding.ASCII.GetBytes(Asset.ImagePath);
                    Asset.Attachment = Asset.ImagePath;
                    //Asset.Attachment = new byte[Attach.ContentLength];
                    //Attach.InputStream.Read(Asset.Attachment, 0, Attach.ContentLength);
                }
                else
                {
                    var alternate = db.AssetDetails.Where(x => x.AssetEntryID == Asset.AssetEntryID).FirstOrDefault();
                    Asset.Attachment = alternate.Attachment;
                }
                HttpResponseMessage response = GlobalVariables.Clients.PutAsJsonAsync("api/AssetDetails/" + Asset.AssetEntryID, Asset).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Updated SuccessFully";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server Error Occured, Please check with the Admin!");
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect(toError);
            }
        }
        public  JsonResult Delete(string AssetNo,int id = 0)
        {
            string deleteStatus = "";
            if (id != 0)
            {
                var check =  db.UserAssets.Where(x => x.AssetDetail == AssetNo).FirstOrDefault();
                var getValue = db.AssetDetails.Where(x => x.AssetEntryID == id).FirstOrDefault();
                if (getValue != null)
                {
                    deleteStatus = getValue.Isdeleted;
                }
                if (check != null)
                {
                    if (deleteStatus.ToLower() == "yes")
                    {
                        return Json("2", JsonRequestBehavior.AllowGet);
                    }
                    return Json("1", JsonRequestBehavior.AllowGet);
                } 
                else
                {
                    if (deleteStatus.ToLower() == "yes")
                    {
                        return Json("2", JsonRequestBehavior.AllowGet);
                    }
                    return Json("3", JsonRequestBehavior.AllowGet);
                }
               
                //try
                //{
                //    HttpResponseMessage response = GlobalVariables.Clients.DeleteAsync("api/AssetDetails/" + id.ToString()).Result;
                //    TempData["SuccessMessage"] = "Record Deleted Successfully";
                //    return RedirectToAction("Index");
                //}
                //catch (Exception ex)
                //{
                //    TempData["errorMessage"] = ex.Message;
                //    return Redirect(toError);
                //}
            }
            else
            {
                TempData["SuccessMessage"] = "No Record Found";
                return Json("Index");
            }

        }

        [HttpGet]
        public ActionResult Deleted(int id)
        {
            //List<AssetDetail> user = new List<AssetDetail>();
            var user = db.AssetDetails.Where(x => x.AssetEntryID == id).FirstOrDefault();

            return PartialView(user); 
        }

        [HttpPost]
        public JsonResult Deleted(AssetDetail Asset)
        {
            AssetDetail newAsset = db.AssetDetails.Where(x => x.AssetEntryID == Asset.AssetEntryID).FirstOrDefault();
            newAsset.Remarks = "Deleted Reason - " + Asset.Remarks;
            newAsset.Isdeleted = "Yes";

            db.SaveChanges();
            return Json(newAsset,JsonRequestBehavior.AllowGet);
        }

        public class AssetDropDown
        {
            public string AssetType { get; set; }

            public string AssetPrefix { get; set; }
        }

        public JsonResult GetPrefix(string AType)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                List<AssetDeclaration> value = new List<AssetDeclaration>();
                List<AssetDetail> check = db.AssetDetails.Where(y => y.AssetType == AType).ToList();
                AssetDetail Asset = new AssetDetail();

                if (check.Count <= 0)
                {
                    value = db.AssetDeclarations.Where(x => x.AssertType == AType).ToList();
                    if (value.Count <= 0)
                    {

                        check.Add(Asset);
                        return Json(check, JsonRequestBehavior.AllowGet);
                    }
                    return Json(value, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    AssetDeclaration assetdeclare = new AssetDeclaration();
                   
                    foreach (var j in check)
                    {
                        assetdeclare.AssetId = Convert.ToInt32(j.AssetId);
                        assetdeclare.AssertType = j.AssetType;
                        assetdeclare.AssertPrefix = j.AssetNo;

                        value.Add(assetdeclare);
                    }


                    return Json(value, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Json("Error Fetching the Data",JsonRequestBehavior.AllowGet);
            }

        }

        public List<AssetDropDown> ViewGenerator()
        {
            List<AssetDropDown> newEntry = new List<AssetDropDown>();

            try
            {
               
                SqlCommand command = new SqlCommand("Select AssertPrefix,AssertType from AssetDeclaration", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    //AssetDetails.Add(reader["AssertType"].ToString());
                    AssetDropDown assetDrop = new AssetDropDown()
                    {
                        AssetType = reader["AssertType"].ToString(),
                        AssetPrefix = reader["AssertPrefix"].ToString()
                    };
                    newEntry.Add(assetDrop);
                }
                reader.Close();
                connection.Close();

                return newEntry;
            }
            catch
            {
                AssetDropDown assetDrop = new AssetDropDown()
                {
                    AssetType = "Error Fetching the Preferred Data",
                    AssetPrefix = "Error",
                };
                newEntry.Add(assetDrop);
                return  newEntry;
            }
        }
    }
}