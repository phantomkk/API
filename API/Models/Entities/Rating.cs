//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API.Models.Entities
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public partial class Rating
    {
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public Nullable<double> Rating1 { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]

        public virtual Product Product { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]

        public virtual User User { get; set; }
    }
}
