using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Asset.Models
{
    [MetadataType(typeof(Assetmetadata))]
    public partial class AssetTypes
    {
        public int AssetId { get; set; }

        [Required]
        public string AssertType { get; set; }

        [Required]
        public string AssertPrefix { get; set; }
    }

    public class Assetmetadata
    {
        public int AssetId { get; set; }

        [Required]
        public string AssertType { get; set; }

        [Required]
        public string AssertPrefix { get; set; }
    }
   
}