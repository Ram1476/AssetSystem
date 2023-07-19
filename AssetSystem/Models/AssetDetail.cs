//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AssetSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AssetDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AssetDetail()
        {
            this.UserAssets = new HashSet<UserAsset>();
        }
    
        public int AssetEntryID { get; set; }
        public Nullable<int> AssetId { get; set; }
        public string AssetType { get; set; }
        public string AssetNo { get; set; }
        public string AssetDescription { get; set; }
        public Nullable<System.DateTime> PurchaseDate { get; set; }
        public Nullable<System.DateTime> WarrantyStartDate { get; set; }
        public Nullable<System.DateTime> WarrantyEndDate { get; set; }
        public string Serial_No { get; set; }
        public string Remarks { get; set; }
        public string Isdeleted { get; set; }
        public byte[] Attachment { get; set; }
    
        public virtual AssetDeclaration AssetDeclaration { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserAsset> UserAssets { get; set; }
    }
}