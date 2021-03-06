﻿namespace Sales.API.Controllers
{

    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Sales.API.Helpers;
    using Sales.Common.Models;
    using Sales.Domain.Models;
    [Authorize]
    public class ProductsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            return db.Products.OrderBy(P => P.Description);
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        [HttpPut]
  
        public async Task<IHttpActionResult> PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            if (product.ImageArray != null && product.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(product.ImageArray);
                //  Guid  codigo alfa numerici que no  se  repite
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Products";
                var folder2 = "\\Content\\Products";

                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhotoURLFIN(stream, $"{"g:\\ProyectosXamarin\\Sales\\SalesBackEnd"}\\{folder2.Replace("~", "")}", file);

                if (response)
                {
                    product.ImagePath = fullPath;
                }
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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

            return Ok(product);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {

            product.IsVariable = true;
            //HORA  DE  LONDRES NO SE EN QUE SERVER  VA ESTAR
            product.PublishOn = DateTime.Now.ToUniversalTime();
            product.ImagePath = "";

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (product.ImageArray != null && product.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(product.ImageArray);
                //  Guid  codigo alfa numerici que no  se  repite
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Products";
                var folder2 = "\\Content\\Products";

                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhotoURLFIN(stream, $"{"g:\\ProyectosXamarin\\Sales\\SalesBackEnd"}\\{folder2.Replace("~", "")}", file);

                if (response)
                {
                    product.ImagePath = fullPath;
                }
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

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
            return db.Products.Count(e => e.ProductId == id) > 0;
        }
    }
}