using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tasks_MVC_1.Data;
using Tasks_MVC_1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<Users>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();

    if (!await roleManager.RoleExistsAsync("Manager"))
        await roleManager.CreateAsync(new IdentityRole("Manager"));

    if (!await roleManager.RoleExistsAsync("Employee"))
        await roleManager.CreateAsync(new IdentityRole("Employee"));

    string email = "admin@test.com";
    string password = "Admin123!";

    var user = await userManager.FindByEmailAsync(email);

    if (user == null)
    {
        user = new Users
        {
            UserName = email,
            Email = email
        };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Manager");
    }
}




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
