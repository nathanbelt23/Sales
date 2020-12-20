
namespace SalesBackEnd.Controllers
{

    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using Sales.Common.Models;
    using SalesBackEnd.Models;

    public class ProductsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            return View(await db.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductView product)
        {
            if (ModelState.IsValid)
            {


                var pic = string.Empty;
                var folder = "~/Content/Products";

                if (product.ImageFile != null)
                {
                    pic = Helpers.FilesHelper.UploadPhoto(product.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var productIns = ToProduct(product, pic);
                db.Products.Add(productIns);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        private Product ToProduct(ProductView proView, string pic)
        {



            return new Product
            {
                ProductId = proView.ProductId,
                Description = proView.Description,
                Remarks = proView.Remarks,
                ImagePath = pic,
                Price = proView.Price,
                IsVariable = proView.IsVariable,
                PublishOn = proView.PublishOn
            };
        }

        private ProductView ToViewProduct(Product pro, string aux)
        {



            return new ProductView
            {
                ProductId = pro.ProductId,
                Description = pro.Description,
                Remarks = pro.Remarks,
                ImagePath = pro.ImagePath,
                Price = pro.Price,
                IsVariable = pro.IsVariable,
                PublishOn = pro.PublishOn
            };
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(ToViewProduct(product, ""));
        }

        // POST: Products/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( ProductView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.ImagePath;
                var folder = "~/Content/Products";

                if (view.ImageFile != null)
                {
                    pic = Helpers.FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }



                var product = ToProduct(view, pic);

                db.Entry(product).State = EntityState.Modified;


                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(view);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
