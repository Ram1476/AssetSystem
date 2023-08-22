using AssetSystem.Models;
using MVC_Asset.Models;
using System;
using System.Collections.Generic;
using System.Configuration; 
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using static MVC_Asset.Controllers.AssetDetailController;
using AssetDetail = MVC_Asset.Models.AssetDetail;
using UserAsset = MVC_Asset.Models.UserAsset;
using UserDetail = MVC_Asset.Models.UserDetail;

namespace MVC_Asset.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AssignToUserController : Controller
    {
        AssetManagementDBEntities db = new AssetManagementDBEntities();
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AssetDB"].ConnectionString);

        string apiConnection = ConfigurationManager.AppSettings["Address"];

        string toError = ConfigurationManager.AppSettings["roError"];
      
        // GET: UserAssign
        [HttpGet]
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
            ViewBag.Message = TempData["Message"];
            try
            {
                IEnumerable<AssignAsset> users = null;

                var response = GlobalVariables.Clients.GetAsync("api/UserAssets");

                response.Wait();

                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    using (System.Threading.Tasks.Task<IEnumerable<AssignAsset>> readjob = result.Content.ReadAsAsync<IEnumerable<AssignAsset>>())
                    {
                        readjob.Wait();
                        users = readjob.Result;
                    }
                }
                else
                {
                    users = Enumerable.Empty<AssignAsset>();

                    ModelState.AddModelError(string.Empty, "Server error occurred. Please contact admin for help");
                }
                return View(users.ToPagedList(i ?? 1,5));
            }
            catch(Exception ex)
            {
                
                TempData["errorMessage"] = ex.Message;
                return Redirect(toError);
            }
        }

        public ActionResult Create()
        {
            try
            {


                var newEntry = ViewGenerator();

                var assetEntryid = ViewAssetID();
                var userEntryid = ViewUserID();
                //var newAssetNo = ViewAssetNo();
                //ViewBag.AssetNo = new SelectList(newAssetNo, "AssetNo", "AssetNo");
                ViewBag.Message = new SelectList(newEntry, "AssetType", "AssetType");

                ViewBag.AssetEntryid = new SelectList(assetEntryid, "AssetEntryID", "AssetEntryID");

                ViewBag.UserEntryid = new SelectList(userEntryid, "UserEntryId", "UserEntryId");

                return View();
            }
            catch(Exception ex)
            {
                
                TempData["errorMessage"] = ex.Message;
                return Redirect(toError);
            }
        }
        [HttpPost]
        public ActionResult Create(AssignAsset AssignUser)
        {
            try
            {
                var splited = AssignUser.UserName.Split(' ');

                SqlCommand newCommand = new SqlCommand($"Select UserEntryId from UserDetail where FirstName = '{splited[0]}' and LastName = '{splited[1]}'", connection);
                connection.Open();
                SqlDataReader reader = newCommand.ExecuteReader();

                while (reader.Read())
                {
                    AssignUser.UserEntryID = (int)reader["UserEntryId"];
                }
                connection.Close();
                var newEntry = ViewGenerator();

                var assetEntryid = ViewAssetID();
                var userEntryid = ViewUserID();
                //var newAssetNo = ViewAssetNo();
                //ViewBag.AssetNo = new SelectList(newAssetNo, "AssetNo", "AssetNo");
                ViewBag.Message = new SelectList(newEntry, "AssetType", "AssetType");

                ViewBag.AssetEntryid = new SelectList(assetEntryid, "AssetEntryID", "AssetEntryID");

                ViewBag.UserEntryid = new SelectList(userEntryid, "UserEntryId", "UserEntryId");
                var postJob = GlobalVariables.Clients.PostAsJsonAsync<AssignAsset>("api/UserAssets", AssignUser);

                postJob.Wait();

                var postResult = postJob.Result;

                if (postResult.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Saved SuccessFully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMesssage"] = "No Data Available To Save ";
                }

                ModelState.AddModelError(string.Empty, "Server error occurred. Please contact admin for help");
                return View(AssignUser);
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

                var assetEntryid = ViewAssetID();
                var userEntryid = ViewUserID();
                //var newAssetNo = ViewAssetNo();
                //ViewBag.AssetNo = new SelectList(newAssetNo, "AssetNo", "AssetNo");
                ViewBag.Message = new SelectList(newEntry, "AssetType", "AssetType");

                ViewBag.AssetEntryid = new SelectList(assetEntryid, "AssetEntryID", "AssetEntryID");

                ViewBag.UserEntryid = new SelectList(userEntryid, "UserEntryId", "UserEntryId");
                if (id != 0)
                {


                    UserAsset assignUser = null;

                    HttpResponseMessage responseTask = GlobalVariables.Clients.GetAsync("api/UserAssets/ " + id.ToString()).Result;



                    if (responseTask.IsSuccessStatusCode)
                    {
                        assignUser = responseTask.Content.ReadAsAsync<UserAsset>().Result;
                        ViewBag.valueAsset = assignUser.AssetDetail;
                        //var readTask = result.Content.ReadAsAsync<UserAsset>();
                        return View(assignUser);
                        //assignUser = readTask.Result;

                        
                    }
                  
                    return View(assignUser);
                }
                else
                {
                    return View();
                }
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect(toError);
            }
        }

        [HttpPost]
        public ActionResult Edit(UserAsset assignUser, int id = 0)
        {
            try
            {


                var newEntry = ViewGenerator();
                var assetEntryid = ViewAssetID();
                var userEntryid = ViewUserID();
                //var newAssetNo = ViewAssetNo();
                //ViewBag.AssetNo = new SelectList(newAssetNo, "AssetNo", "AssetNo");
                ViewBag.Message = new SelectList(newEntry, "AssetType", "AssetType");

                ViewBag.AssetEntryid = new SelectList(assetEntryid, "AssetEntryID", "AssetEntryID");

                ViewBag.UserEntryid = new SelectList(userEntryid, "UserEntryId", "UserEntryId");
                var assetDetail = db.UserAssets.Where(x => x.UserAssetId== id).FirstOrDefault();
                if(id!=0) 
                {
                    assignUser.UserAssetId = id;
                }

                if (assignUser.AssetDetail == null)
                {
                    assignUser.AssetDetail = assetDetail.AssetDetail;
                }
                if (id != 0)
                {
                    var putTask = GlobalVariables.Clients.PutAsJsonAsync<UserAsset>("api/UserAssets/" + id.ToString(), assignUser);
                    putTask.Wait();

                    var result = putTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Updated SuccessFully";
                        return RedirectToAction("Index");
                    }

                    return View(assignUser);
                }
                else
                {
                    return View(new UserAsset());
                }
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect(toError);
            }

        }
        public ActionResult Delete(int id)
        {
            try
            {
                HttpResponseMessage response = GlobalVariables.Clients.DeleteAsync("api/UserAssets/" + id.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Record Deleted Successfully";
                }
                else
                {
                    TempData["SuccessMessage"] = "Record Not Deleted ";
                }

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect(toError);
            }
        }

        public JsonResult getname(string UserName)
        {
            //var emp = (from x in db.UserDetails where x.FirstName.StartsWith(ename) select new { label = x.FirstName }).ToList();
            //return Json(emp);
 
                var users = db.UserDetails.Where(u => u.FirstName.Contains(UserName) || u.LastName.Contains(UserName)).Select(u => new { label = u.FirstName + " " + u.LastName }).ToList();
                return Json(users, JsonRequestBehavior.AllowGet);
         
        }
        public List<AssetDropDown> ViewGenerator()
        {
            List<AssetDropDown> newEntry = new List<AssetDropDown>();
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

        public List<AssetDetail> ViewAssetID()
        {
            List<AssetDetail> entry = new List<AssetDetail>();

            var newID = db.AssetDetails.ToList();
            
            foreach (var item in newID)
            {
                AssetDetail add = new AssetDetail();

                add.AssetEntryID = item.AssetEntryID;

                entry.Add(add);
            }

            return entry;
        }

        public List<UserDetail> ViewUserID()
        {
            List<UserDetail> userDetails = new List<UserDetail>();

            var newUserId = db.UserDetails.ToList();

            foreach (var item in newUserId)
            {
                UserDetail newID = new UserDetail();

                newID.UserEntryId = item.UserEntryId;

                userDetails.Add(newID);
            }

            return userDetails;
        }


        public JsonResult GetUnnassigned(string Atype)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<AssetDetail> assetDetails = new List<AssetDetail>();

            SqlCommand command = new SqlCommand($"SELECT AssetEntryID,AssetNo from  AssetDetail  WHERE AssetType = '{Atype}' AND AssetNo NOT IN (select assetdetail from userAsset )", connection);
           
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var asset = new AssetDetail();

                asset.AssetNo = reader["AssetNo"].ToString();
                asset.AssetEntryID = (int)reader["AssetEntryID"];

                assetDetails.Add(asset);
            }
            connection.Close();
            return Json(assetDetails,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssetId(string Atype)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<AssetDetail> assetDetails = new List<AssetDetail>();

            SqlCommand command = new SqlCommand($"SELECT AssetEntryID from  AssetDetail  WHERE AssetNo = '{Atype}' ", connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var asset = new AssetDetail();

                
                asset.AssetEntryID = (int)reader["AssetEntryID"];

                assetDetails.Add(asset);
            }
            connection.Close();
            return Json(assetDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult  GetUserID(string Btype)
        {
            List<UserDetail> userDetails= new List<UserDetail>();

            SqlCommand command = new SqlCommand($" select  UserEntryID from UserDetail where FirstName = '{Btype}'");

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var asset = new UserDetail();

                asset.UserEntryId = Convert.ToInt32(reader["UserEntryID"]);

                userDetails.Add(asset);
            }
            connection.Close();

            return Json(userDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsAssetDetail(string AssetDetail) 
        {
                if (AssetDetail == " ")
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}