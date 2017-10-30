using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using API.Models;
using API.Models.Entities;
using AutoMapper;
using API.Models.Dto;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UsersController : ApiController
    {
        private Entities2 db = new Entities2();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        { 
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(UserDTO))]
        public IHttpActionResult GetUser(int id)
        {

            /////ERROR
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            Mapper.Initialize(config => config.CreateMap<User, UserDTO>());
            UserDTO userDTO = Mapper.Map<User, UserDTO>(user);
            return Ok(userDTO);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.ID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users 
        [Route("api/users/login/")]
        public HttpResponseMessage PostUserLogin(User user)
        {  
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
             User userCorrect =
                db.Users.Where(u => u.Username.Equals(user.Username) && u.Password.Equals(user.Password)).FirstOrDefault();
            if(user == null || userCorrect == null)
            {
                //return  NotFound();
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            //return CreatedAtRoute("DefaultApi", new {controller = "users", id = user.ID }, userCorrect.First());
            Mapper.Initialize(conf => conf.CreateMap<User, UserDTO>());
            var usrDTO = Mapper.Map<User, UserDTO>(userCorrect);
            return Request.CreateResponse(HttpStatusCode.OK, usrDTO);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        [Route("api/users/register/")]
        public HttpResponseMessage PostUserRegister(User user)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            db.Users.Add(user);
            Mapper.Initialize(conf => conf.CreateMap<User, UserDTO>());
            var usrDTO = Mapper.Map<User, UserDTO>(user);
            return Request.CreateResponse(HttpStatusCode.OK, usrDTO);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            } 
            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.ID == id) > 0;
        }
    }
}