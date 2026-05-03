using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task2_MVC_.Models;
using Task2_MVC_.Model;

public class ProductsController : Controller
{
    private readonly AppDb _context;
    private readonly IWebHostEnvironment _env;

    public ProductsController(AppDb context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public IActionResult Index()
    {
        var products = _context.products.Include(p => p.Category).ToList(); // ✅
        return View(products);
    }

    public IActionResult Create()
    {
        var test = _context.categories.ToList();
        ViewBag.Categories = new SelectList(_context.categories, "Id", "Name"); // ✅
        return View();
    }

    [HttpPost]
    public IActionResult Create(Products product, IFormFile imageFile)
    {
        if (imageFile != null)
        {
            string folder = Path.Combine(_env.WebRootPath, "images");
            string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            string path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            product.ImagePath = fileName;
        }

        _context.products.Add(product); 
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var product = _context.products.Find(id);

        if (product == null)
            return NotFound();

        ViewBag.Categories = new SelectList(_context.categories, "Id", "Name", product.CategoryId); 
        return View(product);
    }

    [HttpPost]
    public IActionResult Edit(Products product, IFormFile imageFile)
    {
        var existing = _context.products.AsNoTracking()
                            .FirstOrDefault(p => p.Id == product.Id);

        if (existing == null)
            return NotFound();

        if (imageFile != null)
        {
            string folder = Path.Combine(_env.WebRootPath, "images");
            string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            string path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            product.ImagePath = fileName;
        }
        else
        {
            product.ImagePath = existing.ImagePath;
        }

        _context.products.Update(product);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var product = _context.products.Find(id);

        if (product == null)
            return NotFound();

        _context.products.Remove(product);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}