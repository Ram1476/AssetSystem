//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVC_Asset.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AssetDeclaration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AssetDeclaration()
        {
            this.AssetDetails = new HashSet<AssetDetail>();
        }
    
        public int AssetId { get; set; }
        public string AssertType { get; set; }
        public string AssertPrefix { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssetDetail> AssetDetails { get; set; }
    }
}
