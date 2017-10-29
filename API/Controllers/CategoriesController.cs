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

namespace API.Controllers
{
    public class CategoriesController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/Categories
        public IQueryable<Category> GetCategories()
        {
            return db.Categories;
        }

        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult GetCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        // GET: api/Category/5/products
        [Route("api/categories/{id}/products")]
        public IEnumerable<ProductDTO> GetCategoryProducts(int id)
        {
            Category category =
                db.Categories
                .Include(c => c.Products).AsQueryable().Where(c => c.ID == id).First();
            IEnumerable<ProductDTO> products =
                (from p in category.Products.AsQueryable().Include(p => p.Ratings)
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
                     CompanyID = p.CompanyID,
                     Price = p.Price,
                     Website = p.Website,
                     AverageRating = p.Ratings.Count == 0 ? 0 : p.Ratings.Average(a => a != null ? a.Rating1 : 0).Value
                 });
            return products;
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.ID)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public IHttpActionResult PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.ID }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.ID == id) > 0;
        }
    }
}