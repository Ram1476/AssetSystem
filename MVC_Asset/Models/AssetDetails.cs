using AssetSystem.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVC_Asset.Models
{
    public class AssetDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssetEntryID { get; set; }
        [Required]
        public Nullable<int> AssetId { get; set; }
        [Required]
        
        public string AssetType { get; set; }
        [Required]
        public string AssetNo { get; set; }
        [Required]
        public string AssetDescription { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> PurchaseDate { get ; set; } 

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> WarrantyStartDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        public Nullable<System.DateTime> WarrantyEndDate { get; set; }

        public string Serial_No { get; set; }

        public string Remarks { get; set; }
        public string Isdeleted { get; set; }

        [MaxLength]
        public string Attachment { get; set; }

        [NotMapped]
        public string ImagePath { get; set; }

       

    }
   
}