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

namespace API.Controllers
{
    public class ProductsController : ApiController
    {
        Entities db = new Entities();

        public IEnumerable<ProductDTO> GetProducts()
        {
            IEnumerable<ProductDTO> products =
                (from p in db.Products
                 select new ProductDTO()
                 {
                     ID = p.ID,
                     Address = p.Address,
                     Code = p.Code,
                     Phone = p.Phone,
                     CategoryID = p.CategoryID,
                     Country = p.Country,
                     Description = p.Description,
                     Email = p.Email,
                     Name = p.Name,
                     ImgDefault = p.ImgDefault,
                     ComanyName = p.Company.Name,
                     CompanyID = p.CompanyID,
                     Price = p.Price,
                     Website = p.Website,
                     CategoryName = p.Category.Name,
                     AverageRating = p.Ratings.Count == 0 ? 0 : p.Ratings.Average(a => a != null ? a.Rating1 : 0).Value
                 });
            //Mapper
            //    .Initialize(cfg => cfg.CreateMap<Product, ProductDTO>()
            //    .ForMember(p => p.AverageRating,
            //    opt => opt.MapFrom(p => p.Ratings.Count == 0 ? 0 : (p.Ratings.Average(a => a == null ? 0 : a.Rating1).Value))));
            //var products = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(db.Products); 
            return products;

        }

        [Route("api/products/name/{name}")]
        public IEnumerable<ProductDTO> GetProductsByCategoryID(String name)
        {
            IEnumerable<ProductDTO> products =
                  (from p in db.Products
                   select new ProductDTO()
                   {
                       ID = p.ID,
                       Address = p.Address,
                       Code = p.Code,
                       Phone = p.Phone,
                       CategoryID = p.CategoryID,
                       Country = p.Country,
                       Description = p.Description,
                       Email = p.Email,
                       Name = p.Name,
                       CompanyID = p.CompanyID,
                       Price = p.Price,
                       Website = p.Website,
                       ImgDefault = p.ImgDefault,
                       ComanyName = p.Company.Name,
                       CategoryName = p.Category.Name,
                       AverageRating = p.Ratings.Average(a => a.Rating1).Value
                   }).Where(p => p.Name.Contains(name));
            return products;
        }

        [Route("api/products/{id}/comments")]
        public IEnumerable<CommentDTO> GetProductComments(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Comment, CommentDTO>().
            ForMember(dto => dto.Name, opt=>opt.MapFrom(c=>c.User.Name))
            .ForMember(dto => dto.UserAvatar, opt => opt.MapFrom(c => c.User.Avatar))
            );
            var comments = 
                Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(db.Comments.Where(c => c.ProductID == id));
            //return db.Comments.Where(c => c.ProductID == id).Include(c => c.User).AsEnumerable();
            return comments;
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            ProductDTO product = (from p in db.Products
                                  select new ProductDTO()
                                  {
                                      ID = p.ID,
                                      Address = p.Address,
                                      Code = p.Code,
                                      Phone = p.Phone,
                                      CategoryID = p.CategoryID,
                                      Country = p.Country,
                                      Description = p.Description,
                                      Email = p.Email,
                                      Name = p.Name,
                                      ImgDefault = p.ImgDefault,
                                      CategoryName = p.Category.Name,
                                      AverageRating = p.Ratings.Average(a => a.Rating1).Value,
                                      ComanyName = p.Company.Name,
                                      CompanyID = p.CompanyID,
                                      Price = p.Price,
                                      Website = p.Website
                                  }).Where(p => p.ID == id).First();
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // GET: api/Products/code
        [ResponseType(typeof(ProductDTO))]
        [Route("api/products/code/{code}")]
        public IHttpActionResult GetProductByCode(string code)
        {
            ProductDTO product = (from p in db.Products
                                  select new ProductDTO()
                                  {
                                      ID = p.ID,
                                      Address = p.Address,
                                      Code = p.Code,
                                      Phone = p.Phone,
                                      CategoryID = p.CategoryID,
                                      Country = p.Country,
                                      Description = p.Description,
                                      Email = p.Email,
                                      Name = p.Name,
                                      CategoryName = p.Category.Name,
                                      AverageRating = p.Ratings.Average(a => a.Rating1).Value,
                                      ComanyName = p.Company.Name,
                                      CompanyID = p.CompanyID,
                                      Price = p.Price,
                                      Website = p.Website
                                  }).Where(p => p.Code.Contains(code)).First();
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        [Route("GetMoreProducts")]
        public IEnumerable<ProductDTO> GetMoreProducts(int num, int size)
        {
            var products = from pro in db.Products.OrderBy(p => p.ID).Skip(size).Take(num)
                           select new ProductDTO()
                           {
                               ID = pro.ID,
                               Address = pro.Address,
                               Code = pro.Code,
                               Phone = pro.Phone,
                               CategoryID = pro.CategoryID,
                               Country = pro.Country,
                               Description = pro.Description,
                               Email = pro.Email,
                               Name = pro.Name,
                               CategoryName = pro.Category.Name,
                               AverageRating = pro.Ratings.Average(a => a.Rating1).Value,
                               ComanyName = pro.Company.Name,
                               CompanyID = pro.CompanyID,
                               Price = pro.Price,
                               Website = pro.Website
                           };

            return products;
        }
        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ID)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ID }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ID == id) > 0;
        }
    }
}