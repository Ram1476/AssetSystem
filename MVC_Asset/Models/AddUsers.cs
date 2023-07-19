using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Asset.Models
{
    public class AddUsers
    {
        [Required]
        public int UserEntryId { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string CorpId { get; set; }
        [Required]
        [Remote("IsEmpIDExists", "UserEntry", ErrorMessage = "EmployeeID  already  exist - Please Provide a New Value")]
        public Nullable<int> EmployeeId { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public string ReportingTo { get; set; }
        [Required]
        public string IsActive { get; set; }
        public string Remarks { get; set; }

        [NotMapped]
        public string FullName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserAsset> UserAssets { get; set; }

    }
}