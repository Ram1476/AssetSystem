using AssetSystem.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using MVC_Asset.Models;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography.Xml;

namespace MVC_Asset.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserEntryController : Controller
    {
        AssetManagementDBEntities db = new AssetManagementDBEntities();
        string sqlConnection = ConfigurationManager.ConnectionStrings["AssetDB"].ConnectionString;

        string apiConnection = ConfigurationManager.AppSettings["Address"];
        string toIndex = ConfigurationManager.AppSettings["ToIndex"];

        string toError = ConfigurationManager.AppSettings["toError404"];

        // GET: UserEntry
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
            IEnumerable<AddUsers> users = null;

            var response = GlobalVariables.Clients.GetAsync("api/UserDetails");

            response.Wait();

            var result = response.Result;

            if (result.IsSuccessStatusCode)
            {
                using (System.Threading.Tasks.Task<IEnumerable<AddUsers>> readjob = result.Content.ReadAsAsync<IEnumerable<AddUsers>>())
                {
                    readjob.Wait();
                    users = readjob.Result;
                }
            }
            else
            {
                if (result.StatusCode == HttpStatusCode.NotFound)
                {

                    return Redirect(toError);
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Redirect("Error");
                }


            }
            return View(users.ToPagedList(i ?? 1, 5));

            //catch(Exception ex) 
            //{
            //    TempData["errorMessage"] = ex.Message;
            //    return Redirect(toError);
            //}

        }


        public ActionResult Create()
        {

            var usersList = DropDown();
            ViewBag.Users = new SelectList(usersList, "FullName", "FullName");
            return View();


        }


        [HttpPost]
        public ActionResult Create(AddUsers users)
        {
            users.CorpId = "NLTD" + " - " + users.CorpId;
            UserDetail newAddon = new UserDetail();

            var usersList = DropDown();

            ViewBag.Users = new SelectList(usersList, "FullName", "FullName");




            if (users.ReportingTo != null)
            {
                var postJob = GlobalVariables.Clients.PostAsJsonAsync<AddUsers>("api/UserDetails", users);
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
                return View(users);

            }
            else
            {
                users.ReportingTo = "Shoba";
                var postJob = GlobalVariables.Clients.PostAsJsonAsync<AddUsers>("api/UserDetails", users);
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
                return View(users);
            }



        }


        public ActionResult Edit(int id = 0)
        {

            if (id != 0)
            {
                UserDetail user = null;

                var responseTask = GlobalVariables.Clients.GetAsync("api/UserDetails/" + id.ToString());

                var usersList = DropDown();
               

                ViewBag.Users = new SelectList(usersList, "FullName", "FullName");

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {

                    var readTask = result.Content.ReadAsAsync<UserDetail>();
                    user = readTask.Result;
                    if (user.ReportingTo == "Shoba")
                    {
                        ViewBag.value = "Shoba";
                    }
                    
                    
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["errorMessage"] = "No Data Found For your Request";
                    return View("Error");
                }
                return View(user);

            }
            else
            {
                return View("Error");
            }

        }
        [HttpPost]
        public ActionResult Edit(AddUsers users, int id = 0)
        {
           
            if (id != 0)
            {
                if(users.ReportingTo != null)
                {
                    var putTask = GlobalVariables.Clients.PutAsJsonAsync<AddUsers>("api/UserDetails/" + id.ToString(), users);
                    putTask.Wait();

                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Updated SuccessFully";
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    users.ReportingTo = "Shoba";
                    var putTask = GlobalVariables.Clients.PutAsJsonAsync<AddUsers>("api/UserDetails/" + id.ToString(), users);
                    putTask.Wait();

                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Updated SuccessFully";
                        return RedirectToAction("Index");
                    }

                }

                users.FullName = users.FirstName + " " + users.LastName;


                var usersList = DropDown();

                ViewBag.Users = new SelectList(usersList, "FullName", "FullName");

                

                return View(users);
            }
            else
            {
                return View(new AddUsers());
            }


        }

        public JsonResult Delete(int id)
        {
            
            var statusCheck = db.UserDetails.Where(x => x.UserEntryId == id).ToList();
            var assignCheck = db.UserAssets.Where(x => x.UserEntryID == id).ToList();
            if (statusCheck.Count() > 0)
            {
               if (statusCheck[0].IsActive.ToLower() == "no") 
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (assignCheck.Count() > 0)
                    {
                        return Json("2", JsonRequestBehavior.AllowGet);
                    }
                    return Json("3", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("3", JsonRequestBehavior.AllowGet);
            


            //HttpResponseMessage response = GlobalVariables.Clients.DeleteAsync("api/UserDetails/" + id.ToString()).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    TempData["SuccessMessage"] = "Record Deleted Successfully";
            //}
            //else
            //{
            //    TempData["SuccessMessage"] = "Record Not Deleted ";
            //}

            //return Json("Index");

        }

        public ActionResult Deleted(int id)
        {
           var user = db.UserDetails.Where(x => x.UserEntryId == id).FirstOrDefault();
            if (user != null)
            {
                user.IsActive = "No";
                db.SaveChanges();
                TempData["SuccessMessage"] = "Record Deleted Successfully";
            }
            else
            {
                TempData["SuccessMessage"] = "Record Not Deleted ";
            }
            return RedirectToAction("Index");
        }

        public List<UserDetail> DropDown()
        {
            var add = db.UserDetails.ToList();
            List<UserDetail> usersList = new List<UserDetail>();


            foreach (var k in add)
            {
                UserDetail userDetail = new UserDetail();

                userDetail.UserEntryId = k.UserEntryId;
                userDetail.FirstName = k.FirstName;
                userDetail.LastName = k.LastName;
                userDetail.CorpId = k.CorpId;
                userDetail.EmployeeId = k.EmployeeId;
                userDetail.EmailAddress = k.EmailAddress;
                userDetail.ReportingTo = k.ReportingTo;
                userDetail.IsActive = k.IsActive;
                userDetail.FullName = k.FirstName + " " + k.LastName;

                usersList.Add(userDetail);
            }
            return usersList;
        }

        public JsonResult IsEmpIDExists(int EmployeeId)
        {
            //check if any of the AssertType matches the AssertType specified in the Parameter using the ANY extension method.  
            return Json(!db.UserDetails.Any(x => x.EmployeeId == EmployeeId), JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult Search()
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
            //IEnumerable<UserDetail> users = null;
            //users = db.UserDetails.Where(x => x.IsActive == $"{requested}").ToList();
            return View();
        }
        [HttpGet]
        public ActionResult SearchResult(string request)
        {
            try
            {
                IEnumerable<UserDetail> user = null;

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
                user = db.UserDetails.Where(x => x.IsActive == request).ToList();
                return PartialView(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult AllDetails(int id)
        {
            var user = db.UserDetails.Where(x => x.UserEntryId == id).FirstOrDefault();

            return PartialView(user);
        }

    }
}