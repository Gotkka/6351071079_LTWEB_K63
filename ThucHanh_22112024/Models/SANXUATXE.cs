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
    
    public partial class SANXUATXE
    {
        public int MaXe { get; set; }
        public int MaHSX { get; set; }
        public Nullable<System.DateTime> NgaySX { get; set; }
        public Nullable<int> SoLuong { get; set; }
    
        public virtual HANGSANXUAT HANGSANXUAT { get; set; }
        public virtual XEGANMAY XEGANMAY { get; set; }
    }
}
