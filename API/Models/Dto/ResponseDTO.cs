using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace API.Models.Dto
{
    public class ResponseDTO
    {
        public HttpStatusCode Status { get; set; } 
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}