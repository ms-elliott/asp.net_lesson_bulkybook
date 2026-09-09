using BulkyBook.Business.Services.IServices;
using BulkyBook.DataAccess.Data;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly IProductService _ProductSrevice;

        public ProductController(IProductService ProductService)
        {
            _ProductSrevice = ProductService;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _ProductSrevice.GetAllProductsAsync();
            return View("Index", products);
        }

        public async Task<IActionResult> Upsert()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Upsert")]
        public async Task<IActionResult> UpsertPOST(Product Product)
        { 
            if (ModelState.IsValid)
            {
                await _ProductSrevice.CreateProductAsync(Product);
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var Product = await _ProductSrevice.GetProductByIdAsync(id.Value);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("delete")]
        public async Task<IActionResult> DeletePOST(int id)
        {
            await _ProductSrevice.DeleteProductAsync(id);
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }

        #region API CALL
        public async Task<IActionResult> GetAll()
        {
            var products = await _ProductSrevice.GetAllProductsAsync(true);
            return Json(new { data = products });
        }
        #endregion
    }
}