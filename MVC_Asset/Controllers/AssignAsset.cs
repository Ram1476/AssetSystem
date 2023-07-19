using MVC_Asset.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Asset.Controllers
{
    public class AssignAsset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAssetId { get; set; }

        [Required]
        public Nullable<int> AssetEntryId { get; set; }

        [Required]
        public Nullable<int> UserEntryID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string AssetType { get; set; }

        [Required]
        [Remote("IsAssetDetail", "AssignToUser", ErrorMessage = "The AssetDetail Field is Required ")]
        public string AssetDetail { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> StartDate { get; set; }


        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Remarks { get; set; }

      
    }
}