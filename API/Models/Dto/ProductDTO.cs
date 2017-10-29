using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ProductDTO
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public  double Price { get; set; }
        public string Country { get; set; }
        public int CompanyID { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ComanyName { get; set; }
        public string ImgDefault { get; set; }
        public Nullable<double> AverageRating { get; set; }
    }
}