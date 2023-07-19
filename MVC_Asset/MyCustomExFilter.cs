using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Asset
{
    public class MyCustomExFilter : HandleErrorAttribute
    {
        string toError = ConfigurationManager.AppSettings["logError"];
        public override void OnException(ExceptionContext filterContext)
        {
            try
            {

                ViewResult visualTouser = new ViewResult();

                visualTouser.ViewName = "Error";
                filterContext.Result = visualTouser;
                filterContext.ExceptionHandled = true;
                Exception e = filterContext.Exception;

                

                var controller = filterContext.Controller as Controller;
                controller.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                controller.Response.TrySkipIisCustomErrors = true;
                filterContext.ExceptionHandled = true;

                var controllerName = (string)filterContext.RouteData.Values["controller"];
                var actionName = (string)filterContext.RouteData.Values["action"];
                var exception = filterContext.Exception;
                //need a model to pass exception data to error view
                var model = new HandleErrorInfo(exception, controllerName, actionName);

                var view = new ViewResult();
                view.ViewName = "Error";
                view.ViewData = new ViewDataDictionary();
                view.ViewData.Model = model;

                //copy any view data from original control over to error view
                //so they can be accessible.
                var viewData = controller.ViewData;
                if (viewData != null && viewData.Count > 0)
                {
                    viewData.ToList().ForEach(view.ViewData.Add);
                }

                //Instead of this
                ////filterContext.Result = view;
                //Allow the error view to display on the same URL the error occurred
                view.ExecuteResult(filterContext);
                FileStream fileStream = new FileStream(toError, FileMode.Append, FileAccess.Write);

                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine("Controller Name: " + filterContext.RouteData.Values["Controller"]);
                streamWriter.WriteLine("Action Name: " + filterContext.RouteData.Values["action"]);
                streamWriter.WriteLine("Message: " + e.Message + DateTime.Now.ToString("dd-MM-yyyy") + Environment.NewLine);
                streamWriter.Close();
                fileStream.Close();

            }
            catch (Exception ex)
            {
                ex.ToString();
                
            }


        }
    }
}