using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Asset.Models
{
    public class SearchPage
    {

        public int UserAssetId { get; set; }

 
        public Nullable<int> AssetEntryId { get; set; }

 
        public Nullable<int> UserEntryID { get; set; }


        public string UserName { get; set; }

        public string AssetType { get; set; }


        public string AssetDetail { get; set; }
       
        public string StartDate { get; set; }

        public string EndDate { get; set; }
        public string Remarks { get; set; }
    }
}