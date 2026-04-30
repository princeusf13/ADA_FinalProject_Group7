using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Final_Project.Data;
using Final_Project.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("FinalProject_DbContextConnection") ?? throw new InvalidOperationException("Connection string 'FinalProject_DbContextConnection' not found.");;

builder.Services.AddDbContext<FinalProject_DbContext>(options => options.UseSqlServer(connectionString));



//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FinalProject_DbContext>();
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // This line is crucial for your project!
    .AddEntityFrameworkStores<FinalProject_DbContext>();



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); // This MUST come before Authorization
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapRazorPages(); // THIS IS CRITICAL - Identity uses Razor Pages

app.Run();
