using AssetSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Asset.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;
using AssetSystem.Controllers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Security;

namespace MVC_Asset.Controllers
{
    
    public class UserLoginController : Controller
    {
        // GET: UserLogin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Verify(Models.Checking users)
        {
            List<Models.UserLogins> userLogins= new List<Models.UserLogins>();
            HttpResponseMessage response = GlobalVariables.Clients.GetAsync("api/UserLogins").Result;
            string toIndex = ConfigurationManager.AppSettings["toIndex"];
            string toSearch = ConfigurationManager.AppSettings["toSearch"];

            try
            {


                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    userLogins = JsonConvert.DeserializeObject<List<Models.UserLogins>>(data);

                    foreach (var item in userLogins)
                    {
                        if (item.Name == users.Name && item.Password == users.Password)
                        {
                            var ticket = new FormsAuthenticationTicket(users.Name, true, 3000);
                            string Encrypt = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, Encrypt);
                            cookie.Expires = DateTime.Now.AddHours(3000);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);
                            if (item.RoleId != null)
                            {
                                users.RoleId = Convert.ToInt32(item.RoleId);
                            }
                            else
                            {
                                users.RoleId = 0;
                            }

                        }
                    }
                    if (users.RoleId == 1)
                    {

                        TempData["users"] = users.RoleId;
                        TempData["Message"] = "Admin";
                        return Redirect(toIndex);
                    }
                    else if (users.RoleId == 2)
                    {
                        //FormsAuthentication.SetAuthCookie(users.Name, true);
                        TempData["users"] = users.RoleId;
                        TempData["Message"] = "User";
                        return Redirect(toSearch);
                    }
                    else
                    {
                        TempData["users"] = users.RoleId;
                        TempData["Message"] = "* Invalid UserName Or PassWord ";
                        return RedirectToAction("Login");
                    }


                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
               
            }
           
            
            
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
           
            return View("Login");
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}