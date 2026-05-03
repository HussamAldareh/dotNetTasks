using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task2_MVC_.Model;
using Task2_MVC_.Models;

namespace Task2_MVC_.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDb _context;
        private readonly IWebHostEnvironment _env;

        public CategoriesController(AppDb context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var data = _context.categories.ToList(); // ✅ تعديل
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "images");
                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                category.ImagePath = fileName;
            }

            _context.categories.Add(category); // ✅ تعديل
            _context.SaveChanges();

            return RedirectToAction("Index"); // ✅ مهم
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.categories.Find(id); // ✅

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category, IFormFile imageFile)
        {
            var existing = _context.categories.AsNoTracking()
                                .FirstOrDefault(c => c.Id == category.Id);

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

                category.ImagePath = fileName;
            }
            else
            {
                category.ImagePath = existing.ImagePath;
            }

            _context.categories.Update(category); // ✅
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var category = _context.categories.Find(id); // ✅

            if (category == null)
                return NotFound();

            _context.categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}