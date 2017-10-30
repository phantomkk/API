using API.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Dto
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Introduce { get; set; }
        public string Token { get; set; }
        public int Role { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
    }
}