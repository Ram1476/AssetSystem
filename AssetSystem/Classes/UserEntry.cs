using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetSystem.Classes
{
    public class UserEntry
    {
        public int UserEntryId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CorpId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string EmailAddress { get; set; }
        public string ReportingTo { get; set; }
        public string IsActive { get; set; }
        public string Remarks { get; set; }
    }
}