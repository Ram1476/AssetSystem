using AssetSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssetSystem.Classes
{
    public class AssignToUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAssetId { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<int> AssetEntryId { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public Nullable<int> UserEntryID { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public string UserName { get; set; }

        public string AssetType { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public string AssetDetail { get; set; }
        [Required(ErrorMessage = "This Field is Required")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> StartDate { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public string Remarks { get; set; }

        public virtual AssetDetail AssetDetail1 { get; set; }
        public virtual UserDetail UserDetail { get; set; }
    }
}