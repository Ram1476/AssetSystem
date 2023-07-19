using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace MVC_Asset.Models
{
    public class Checking
    {
        public int  RoleId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}