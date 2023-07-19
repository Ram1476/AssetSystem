using MVC_Asset.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;



namespace MVC_Asset.Controllers
{
    [Authorize(Roles = "Admin")]
    [HandleError]
    public class AssetDeclarationController : Controller
    {
        AssetManagementDBEntities db = new AssetManagementDBEntities();
        string toError = ConfigurationManager.AppSettings["roError"];


        // GET: AssetDeclaration
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
            IEnumerable<AssetDeclaration> addAsset = null;
            HttpResponseMessage response = GlobalVariables.Clients.GetAsync("api/AssetDeclarations").Result;
            if (response != null)
            {
                addAsset = response.Content.ReadAsAsync<IEnumerable<AssetDeclaration>>().Result;
                ViewBag.Message = TempData["Message"];
                return View(addAsset.ToPagedList(i ?? 1, 5));
            }
            return View();
            //else
            //{
            //    throw new Exception();
            //}



            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("", ex);
            //    TempData["errorMessage"] = ex.Message;
            //    MyCustomExFilter customFilter = new MyCustomExFilter();


            //    return View("Error");
            //}
        }

        public ActionResult Create(int id = 0)
        {
            try
            {
                if (id == 0)
                {

                    return View(new AssetDeclaration());
                }
                else
                {
                    HttpResponseMessage response = GlobalVariables.Clients.GetAsync("api/AssetDeclarations/" + id.ToString()).Result;
                    return View(response.Content.ReadAsAsync<AssetDeclaration>().Result);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View("Error");
            }

        }
        
        [HttpPost]
        public ActionResult Create(AssetDeclaration Asset)
        {
            try
            {
                if (Asset.AssetId == 0)
                {
                    HttpResponseMessage response = GlobalVariables.Clients.PostAsJsonAsync("api/AssetDeclarations", Asset).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Saved SuccessFully";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server Error Occured, Please check with the Admin!");
                    }
                }
                else
                {
                    return View();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //TempData["errorMessage"] = ex.Message;
                //return Redirect(toError);
                ModelState.AddModelError("", ex);
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        public ActionResult Edit(int id = 0)
        {
            if (id != 0)
            {
                HttpResponseMessage response = GlobalVariables.Clients.GetAsync("api/AssetDeclarations/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<AssetTypes>().Result);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(AssetDeclaration Asset,int id = 0)
        {
            if (Asset != null && id != 0)
            {
                HttpResponseMessage response = GlobalVariables.Clients.PutAsJsonAsync("api/AssetDeclarations/" + Asset.AssetId, Asset).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Updated SuccessFully";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server Error Occured, Please check with the Admin!");
                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            try
            {


                HttpResponseMessage response = GlobalVariables.Clients.DeleteAsync("api/AssetDeclarations/" + id.ToString()).Result;
                TempData["SuccessMessage"] = "Record Deleted Successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //TempData["errorMessage"] = ex.Message;
                //return Redirect(toError);

                ModelState.AddModelError("", ex);
                return RedirectToAction("Index");
            }
        }


        public JsonResult IsUserExists(string AssertType)
        {
            //check if any of the AssertType matches the AssertType specified in the Parameter using the ANY extension method.  
            return Json(!db.AssetDeclarations.Any(x => x.AssertType == AssertType), JsonRequestBehavior.AllowGet);
        }


        public JsonResult IsPrefixExists(string AssertPrefix)
        {
            //check if any of the AssertType matches the AssertType specified in the Parameter using the ANY extension method.  
            return Json(!db.AssetDeclarations.Any(x => x.AssertPrefix == AssertPrefix), JsonRequestBehavior.AllowGet);
        }
    }
}