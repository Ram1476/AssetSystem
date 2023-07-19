using MVC_Asset.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.Net.Http;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace MVC_Asset.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class SearchController : Controller
    {
        AssetManagementDBEntities db = new AssetManagementDBEntities();
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AssetDB"].ConnectionString);

        string apiConnection = ConfigurationManager.AppSettings["Address"];
        // GET: Search

        public ActionResult Index(int? i, string search, string selectedValue)
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
            List<UserAsset> userAssetlist = new List<UserAsset>();
            string searchBy = selectedValue;
            ViewBag.Message = selectedValue;
            if (searchBy == "AssetNo")
            {
                try
                {
                   
                    string Assetno = search;
                    db.Configuration.ProxyCreationEnabled = false;
                    userAssetlist = db.UserAssets.Where(x => x.AssetDetail == Assetno || search == "").ToList();
                   
                    
                }
                catch (FormatException)
                {
                    Console.WriteLine($"{search} Is Not a Valid Input");
                }
                return View(userAssetlist.ToPagedList(i ?? 1, 3));
            }
            else if (searchBy == "User")
            {
                db.Configuration.ProxyCreationEnabled = false;
                userAssetlist = db.UserAssets.Where(x => x.UserName.StartsWith(search) || search == "").ToList();
                return View(userAssetlist.ToPagedList(i ?? 1, 3));
            }
            else
            {
                UserAsset user = new UserAsset();

                user.UserName = null;
                user.StartDate = null;
                user.EndDate = null;
                user.AssetType = null;
                user.AssetDetail = null;
                userAssetlist.Add(user);
                return View(userAssetlist.ToPagedList(i ?? 1, 3));
            }

        }

        public ActionResult GetAllValues(int? i)
        {
            List<UserAsset> userAsset = new List<UserAsset>();
            
            userAsset = db.UserAssets.ToList();
            return PartialView(userAsset.ToPagedList(i ?? 1, 3));

        }
        public JsonResult GetSearchingData(string searchBy, string searchValue)
        {
            SearchPage newValue = new SearchPage();
            List<SearchPage> searchList  = new List<SearchPage>();
            List<UserAsset> userAssetlist = new List<UserAsset>();
            if (searchBy == "AssetNo")
            {
                try
                {
                    string Assetno = searchValue;
                    userAssetlist = db.UserAssets.Where(x => x.AssetDetail == Assetno || searchValue == null).ToList();
                    if (userAssetlist.Count > 0)
                    {

                        newValue.UserAssetId = userAssetlist[0].UserAssetId;
                        newValue.UserName = userAssetlist[0].UserName;
                        newValue.AssetType = userAssetlist[0].AssetType;
                        newValue.AssetDetail = userAssetlist[0].AssetDetail;

                        newValue.StartDate = ((userAssetlist[0].StartDate).GetValueOrDefault().ToShortDateString());
                        if (((userAssetlist[0].EndDate).GetValueOrDefault().ToShortDateString()) != "01-01-0001")
                        {

                            newValue.EndDate = ((userAssetlist[0].EndDate).GetValueOrDefault().ToShortDateString());
                        }
                        else
                        {
                            newValue.EndDate = "";
                        }
                        
                    }
                    else
                    {
                        return Json(userAssetlist, JsonRequestBehavior.AllowGet);
                    }
                   
                }
                catch (FormatException)
                {
                    Console.WriteLine($"{searchValue} Is Not a Valid Input");
                }
                return Json((newValue, JsonRequestBehavior.AllowGet));
            }
            else if (searchBy == "User")
            {
                userAssetlist = db.UserAssets.Where(x => x.UserName.StartsWith(searchValue) || searchValue == null).ToList();

                if (userAssetlist.Count > 0)
                {
                     newValue.UserAssetId = userAssetlist[0].UserAssetId;
                    newValue.UserName = userAssetlist[0].UserName;
                    newValue.AssetType = userAssetlist[0].AssetType;
                    newValue.AssetDetail = userAssetlist[0].AssetDetail;

                    newValue.StartDate = ((userAssetlist[0].StartDate).GetValueOrDefault().ToShortDateString());
                    if (((userAssetlist[0].EndDate).GetValueOrDefault().ToShortDateString()) != "01-01-0001")
                    {

                        newValue.EndDate = ((userAssetlist[0].EndDate).GetValueOrDefault().ToShortDateString());
                    }
                    else
                    {
                        newValue.EndDate = "";
                    }
                    searchList.Add(newValue);
                    
                }
                else
                {
                    return Json(userAssetlist, JsonRequestBehavior.AllowGet);
                }

                return Json(searchList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                
                return Json(userAssetlist, JsonRequestBehavior.AllowGet);

            }
        }
       

        public PartialViewResult Details(string AssetDetail)
        {
            AssetDetail assetDetail = new AssetDetail();

            try
            {

                SqlCommand command = new SqlCommand($"select * from AssetDetail where AssetNo = '{AssetDetail}'", connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                BinaryFormatter binaryFormat = new BinaryFormatter();

                while (reader.Read())

                {

                    assetDetail.AssetType = reader["AssetType"].ToString();

                    assetDetail.AssetNo = reader["AssetNo"].ToString();

                    assetDetail.AssetDescription = reader["AssetDescription"].ToString();

                    assetDetail.PurchaseDate = Convert.ToDateTime(reader["PurchaseDate"]);

                    assetDetail.WarrantyStartDate = Convert.ToDateTime(reader["WarrantyStartDate"]);

                    assetDetail.WarrantyEndDate = Convert.ToDateTime(reader["WarrantyEndDate"]);

                    assetDetail.Serial_No = reader["Serial_No"].ToString();

                    assetDetail.Remarks = reader["Remarks"].ToString();

                    assetDetail.Isdeleted = reader["Isdeleted"].ToString();

                    MemoryStream memoryStream = new MemoryStream();

                    if (reader["Attachment"] != null)

                    {
                        binaryFormat.Serialize(memoryStream, reader["Attachment"]);

                        assetDetail.Attachment = memoryStream.ToArray();

                        assetDetail.ImagePath = Encoding.ASCII.GetString(memoryStream.ToArray());

                    }

                }

                List<AssetDetail> value = null;
                value = db.AssetDetails.Where(x => x.AssetNo == AssetDetail).ToList();
                if (value.Count() != 0)
                {
                    assetDetail.ImagePath = Encoding.ASCII.GetString(value[0].Attachment);
                }
                return PartialView(assetDetail);
            }
            catch(Exception ex)
            {

                return PartialView(assetDetail);
            }
        }

        public JsonResult getname(string search, string searchby)
        {
            if (searchby == "AssetNo")
            //var emp = (from x in db.UserDetails where x.FirstName.StartsWith(ename) select new { label = x.FirstName }).ToList();
            //return Json(emp);
            {
                var users = db.AssetDetails.Where(u => u.AssetNo.Contains(search)).Select(u => new { label = u.AssetNo }).ToList();
                return Json(users, JsonRequestBehavior.AllowGet);
            }
            else if (searchby == "User")
            {
                var users = db.UserDetails.Where(u => u.FirstName.Contains(search) || u.LastName.Contains(search)).Select(u => new { label = u.FirstName + " " + u.LastName }).ToList();
                return Json(users, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string users = null;
                return Json(users, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult UnAssign(int id = 0)
        {
            if (id != 0)
            {
                UserAsset newUserasset = db.UserAssets.Where(x => x.UserAssetId == id).FirstOrDefault();
                AssignAsset newAssign = new AssignAsset();
                newAssign.UserAssetId = newUserasset.UserAssetId;
                newAssign.AssetEntryId = newUserasset.AssetEntryId;
                newAssign.UserEntryID = newUserasset.UserEntryID;
                newAssign.UserName = newUserasset.UserName;
                newAssign.AssetType = newUserasset.AssetType;
                newAssign.AssetDetail = newUserasset.AssetDetail;
                newAssign.StartDate = Convert.ToDateTime( newUserasset.StartDate);
                if(Convert.ToDateTime(newUserasset.EndDate).ToString() == "01-01-0001")
                {
                    newAssign.EndDate = DateTime.Now;
                }
                else
                {
                    newAssign.EndDate = Convert.ToDateTime(newUserasset.EndDate);
                }
                newAssign.Remarks = newUserasset.Remarks;
                return PartialView(newAssign);
            }
            else
            {
                return PartialView(new AssignAsset());
            }
        }

        [HttpPost]
        public JsonResult UnAssign(UserAsset assignUser)
        {
            UserAsset newUserasset = db.UserAssets.Where(x => x.UserAssetId == assignUser.UserAssetId).FirstOrDefault();
            AssignAsset newAssign = new AssignAsset();
            newUserasset.UserAssetId = assignUser.UserAssetId;
            newUserasset.AssetEntryId = assignUser.AssetEntryId;
            newUserasset.UserEntryID = assignUser.UserEntryID;
            newUserasset.UserName = assignUser.UserName;
            newUserasset.AssetType = assignUser.AssetType;
            newUserasset.AssetDetail = assignUser.AssetDetail;
            newUserasset.StartDate = assignUser.StartDate;
            newUserasset.EndDate = assignUser.EndDate;
            newUserasset.Remarks = assignUser.Remarks;
            db.SaveChanges();
            return Json(newUserasset, JsonRequestBehavior.AllowGet);
        }
    }
}


