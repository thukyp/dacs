using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddDefaultTokenProviders()
.AddDefaultUI()
.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.LogoutPath = $"/Identity/Account/AccessDenied";
});

// Add services to the container.


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<INguoiMuaRepository, NguoiMuaRepository>();
builder.Services.AddScoped<IThuGomRepository, ThuGomRepository>();
builder.Services.AddScoped<ISanPhamRepository, EFSanPhamRepository>();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); ;
app.UseAuthorization();
app.MapRazorPages();

app.UseEndpoints(endpoints =>
{

    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "Owner",
        pattern: "{area:exists}/{controller=Owner}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "KhachHang",
        pattern: "{area:exists}/{controller=KhachHang}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "QuanLyDH",
        pattern: "{area:exists}/{controller=QuanLyDH}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "QuanLyND",
        pattern: "{area:exists}/{controller=QuanLyND}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
    name: "QuanLyXNK",
    pattern: "{area:exists}/{controller=QuanLyXNK}/{action=Index}/{id?}");


    endpoints.MapControllerRoute(
name: "QuanLySP",
pattern: "{area:exists}/{controller=QuanLySP}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.Run();
