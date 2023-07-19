using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace MVC_Asset
{
    public static class GlobalVariables
    {
        public static HttpClient Clients = new HttpClient();
        static GlobalVariables()
        {
            string path = ConfigurationManager.AppSettings["Address"];
            Clients.BaseAddress = new Uri(path);
            Clients.DefaultRequestHeaders.Clear();
            Clients.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}