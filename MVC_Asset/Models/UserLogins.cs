using AssetSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MVC_Asset.Models
{
    public class UserLogins
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Nullable<int> RoleId { get; set; }
    }
}