using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Dto
{
    public class CommentDTO
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string Comment1 { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public String Name { get; set; }
        public String UserAvatar { get; set; }
    }
}