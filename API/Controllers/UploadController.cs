using API.Models.Dto;
using API.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace API.Controllers
{
    public class UploadController : ApiController
    {
        Entities db = new Entities();
        [Route("api/upload/image/user/")]
        [AllowAnonymous]
        public HttpResponseMessage PostUserImage()
        {

            ////test multipart
            //string root = HttpContext.Current.Server.MapPath("~/App_Data");
            //var provider = new MultipartFormDataStreamProvider(root);

            //// Read the form data and return an async task.
            //var task = Request.Content.ReadAsMultipartAsync(provider).
            //    ContinueWith<HttpResponseMessage>(t =>
            //    {
            //        if (t.IsFaulted || t.IsCanceled)
            //        {
            //            Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
            //        }

            //// This illustrates how to get the file names.
            //foreach (MultipartFileData file in provider.FileData)
            //        {
            //            Trace.WriteLine(file.Headers.ContentDisposition.FileName);
            //            Trace.WriteLine("Server file path: " + file.LocalFileName);
            //        }
            //        string username = provider.FormData.GetValues("username")[0];

            //        Trace.WriteLine(string.Format("{0}: {1}", "username", username));
            //        //foreach (var key in provider.FormData.AllKeys)
            //        //{
            //        //    foreach (var val in provider.FormData.GetValues(key))
            //        //    {
            //        //        Trace.WriteLine(string.Format("{0}: {1}", key, val));
            //        //    }
            //        //}
            //        return Request.CreateResponse(HttpStatusCode.OK);
            //    });
            //return task;
            ///test multipart
            //string path = Directory.GetFiles("/Storage/UserImage/images.jpg").Select(Path.GetFileName).First();


            Dictionary<string, object> dict = new Dictionary<string, object>();
            var internalPath = ""; var externalPath = "null";
            ResponseDTO dto = null;
            try
            {
                var httpRequest = HttpContext.Current.Request;
                string username = httpRequest.Form.GetValues("username")[0];
                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  
                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg, .gif, .png.");
                            dto = new ResponseDTO()
                            {
                                Status = HttpStatusCode.NotAcceptable,
                                Message = message,
                                Data = ""
                            };
                            return Request.CreateResponse(dto);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");
                            dto = new ResponseDTO()
                            {
                                Status = HttpStatusCode.NotAcceptable,
                                Message = message,
                                Data = ""
                            };
                            return Request.CreateResponse(dto);
                        }
                        else
                        {
                            string folder = HttpContext.Current.Server.MapPath("~/Storage/UserImage/");
                            internalPath = HttpContext.Current.Server.MapPath("~/Storage/UserImage/" + username + extension);
                            Uri uri = System.Web.HttpContext.Current.Request.Url;
                            externalPath = "http://" + uri.Authority + "/" + "Storage/UserImage/" + username + extension;
                            IQueryable userIQuery = db.Users.Where(u => u.Username.Contains(username));
                            User user = db.Users.Where(u => u.Username.Contains(username)).FirstOrDefault();
                            if (user != null)
                            {
                                user.Avatar = externalPath;
                                db.Entry(user).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                dto = new ResponseDTO()
                                {
                                    Status = HttpStatusCode.NotAcceptable,
                                    Message = "Cannot find username: " + username + " on database.",
                                    Data = ""
                                };
                                return Request.CreateResponse(dto);
                            }
                            try
                            {
                                if (!Directory.Exists(folder))
                                {
                                    Directory.CreateDirectory(folder);
                                }
                            }
                            catch (Exception ex)
                            {
                                // handle them here
                            }
                            postedFile.SaveAs(internalPath);

                        }
                    }
                    var message1 = string.Format("Image Upload Successfully.");
                    dto = new ResponseDTO()
                    {
                        Status = HttpStatusCode.OK,
                        Message = "Upload successful",
                        Data = externalPath
                    };
                    return Request.CreateResponse(dto);
                }
                var res = string.Format("Please Upload a image.");
                dto = new ResponseDTO()
                {
                    Status = HttpStatusCode.NoContent,
                    Message = res,
                    Data = ""
                };
                return Request.CreateResponse(dto);
            }
            catch (Exception ex)
            {
                var res = string.Format("Exception: " + ex);
                dto = new ResponseDTO()
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = res,
                    Data = ""
                };
                return Request.CreateResponse(dto);
            }
        }
    }
}
