//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThucHanh_22112024.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HANGSANXUAT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HANGSANXUAT()
        {
            this.SANXUATXEs = new HashSet<SANXUATXE>();
        }
    
        public int MaHSX { get; set; }
        public string TenHSX { get; set; }
        public string Diachi { get; set; }
        public string Tieusu { get; set; }
        public string Dienthoai { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SANXUATXE> SANXUATXEs { get; set; }
    }
}
