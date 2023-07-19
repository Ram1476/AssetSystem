using AssetSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssetSystem.Classes
{
    public class AssetDefining
    {
        public int AssetEntryID { get; set; }
        public Nullable<int> AssetId { get; set; }
        public string AssetType { get; set; }
        public string AssetNo { get; set; }
        public string AssetDescription { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> PurchaseDate { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> WarrantyStartDate { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> WarrantyEndDate { get; set; }
        public string Serial_No { get; set; }
        public string Remarks { get; set; }
        public string Isdeleted { get; set; }
        public byte[] Attachment { get; set; }

        [NotMapped]
        public DateTime? LastModified { get; set; }
        public virtual AssetDeclaration AssetDeclaration { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserAsset> UserAssets { get; set; }
    }
}