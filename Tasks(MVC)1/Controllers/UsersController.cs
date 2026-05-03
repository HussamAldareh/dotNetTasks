using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Tasks_MVC_1.Models;
using System.Threading.Tasks;
using System.Linq;

public class UsersController : Controller
{
    private readonly UserManager<Users> _userManager;

    public UsersController(UserManager<Users> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Index(string search)
    {
        var users = _userManager.Users.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            users = users.Where(u => u.Name.Contains(search));
        }

        return View(users.ToList());
    }

    // CREATE - GET
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // CREATE - POST
    [HttpPost]
    public async Task<IActionResult> Create(Users model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _userManager.CreateAsync(model, model.PasswordHash);

        if (result.Succeeded)
            return RedirectToAction(nameof(Index));

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    // DETAILS
    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        return View(user);
    }

    // EDIT - GET
    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        return View(user);
    }

    // EDIT - POST
    [HttpPost]
    public async Task<IActionResult> Edit(Users model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByIdAsync(model.Id);

        if (user == null)
            return NotFound();

        user.Name = model.Name;
        user.Email = model.Email;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
            return RedirectToAction(nameof(Index));

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    // DELETE
    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        await _userManager.DeleteAsync(user);

        return RedirectToAction(nameof(Index));
    }
}