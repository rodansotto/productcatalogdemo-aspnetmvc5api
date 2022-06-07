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
using MyMvc5App.Models;

namespace MyMvc5App.APIControllers
{
    public class AWProductsController : ApiController
    {
        private AdventureWorksDBContext db = new AdventureWorksDBContext();

        public AWProductsController()
        {
            // the below code is to fix the problem I'm having and this problem is explained 
            // and solution provided at https://stackoverflow.com/a/13077670/6231546
            db.Configuration.ProxyCreationEnabled = false;
        }

        public FilteredSortedPagedList<ProductListView> GetProducts([FromUri] FilterSortPageInfo fspInfo, bool includeImages = false)
        {
            if (fspInfo == null) fspInfo = new FilterSortPageInfo();

            fspInfo.Sort = fspInfo.Sort ?? "productNumber";

            // only include the columns we need to show on the list
            // also just include products with images
            var products = from p in db.Products
                           join ppp in db.ProductProductPhotoes on p.ProductID equals ppp.ProductID
                           join pp in db.ProductPhotoes on ppp.ProductPhotoID equals pp.ProductPhotoID
                           //where !pp.LargePhotoFileName.Equals("no_image_available_large.gif")
                           where ppp.ProductPhotoID != 1
                           select new ProductListView
                           {
                               ProductID = p.ProductID,
                               ProductNumber = p.ProductNumber,
                               Name = p.Name,
                               Color = p.Color,
                               StandardCost = p.StandardCost,
                               ListPrice = p.ListPrice,
                               Size = p.Size,
                               Weight = p.Weight,
                               ProductCategory = p.ProductCategory.Name,
                               ProductModel = p.ProductModel.Name,
                               LargePhoto = includeImages ? pp.LargePhoto : null
                           };

            // do a search
            if (fspInfo.Search && !string.IsNullOrEmpty(fspInfo.SearchValue))
            {
                products = products.Where(p => p.ProductNumber.Contains(fspInfo.SearchValue)
                    || p.Name.Contains(fspInfo.SearchValue)
                    || p.Color.Contains(fspInfo.SearchValue)
                    || p.Size.Contains(fspInfo.SearchValue)
                    || p.ProductCategory.Contains(fspInfo.SearchValue)
                    || p.ProductModel.Contains(fspInfo.SearchValue)
                );
            }

            // pass total records to the view
            fspInfo.TotalRecords = products.Count();

            // apply sort
            switch (fspInfo.Sort)
            {
                case "name":
                    products = products.OrderByWithDirection(p => p.Name, fspInfo.SortDir);
                    break;
                case "color":
                    products = products.OrderByWithDirection(p => p.Color, fspInfo.SortDir);
                    break;
                case "standardCost":
                    products = products.OrderByWithDirection(p => p.StandardCost, fspInfo.SortDir);
                    break;
                case "listPrice":
                    products = products.OrderByWithDirection(p => p.ListPrice, fspInfo.SortDir);
                    break;
                case "size":
                    products = products.OrderByWithDirection(p => p.Size, fspInfo.SortDir);
                    break;
                case "weight":
                    products = products.OrderByWithDirection(p => p.Weight, fspInfo.SortDir);
                    break;
                case "productCategory":
                    products = products.OrderByWithDirection(p => p.ProductCategory, fspInfo.SortDir);
                    break;
                case "productModel":
                    products = products.OrderByWithDirection(p => p.ProductModel, fspInfo.SortDir);
                    break;
                default: // "productNumber"
                    products = products.OrderByWithDirection(p => p.ProductNumber, fspInfo.SortDir);
                    break;
            }

            // only load a page of data
            products = products.Skip(fspInfo.PageSize * (fspInfo.Page - 1)).Take(fspInfo.PageSize);

            //System.Threading.Thread.Sleep(5000);
            return new FilteredSortedPagedList<ProductListView>
            {
                TotalRecords = fspInfo.TotalRecords,
                List = products.ToList()
            };
        }

        // GET: api/AWProducts/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/AWProducts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductID)
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

        // POST: api/AWProducts
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductID }, product);
        }

        // DELETE: api/AWProducts/5
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
            return db.Products.Count(e => e.ProductID == id) > 0;
        }
    }
}