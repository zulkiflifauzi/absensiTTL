//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AbsensiTTL.Repositories
{
    using System;
    using System.Collections.Generic;
    
    public partial class AuditedExc
    {
        public int AEID { get; set; }
        public Nullable<int> UserId { get; set; }
        public System.DateTime CheckTime { get; set; }
        public Nullable<int> NewExcID { get; set; }
        public Nullable<short> IsLeave { get; set; }
        public string UName { get; set; }
        public System.DateTime UTime { get; set; }
    }
}