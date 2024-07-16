using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MVC_TeddySmith_RunGroup.Data;
using MVC_TeddySmith_RunGroup.Helper;
using MVC_TeddySmith_RunGroup.Interfaces;
using MVC_TeddySmith_RunGroup.Models;
using MVC_TeddySmith_RunGroup.Repository;
using MVC_TeddySmith_RunGroup.Services;
using System;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

//-----**********if we dont buld that we cant accsess all of this*********--------------
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IClubRepository, ClubRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
//-------****************************************************************----------------


//Setting our app 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


//------ Identity Framework Ayarlar� //------
//appdbcontext in alt�nda olmal� identity ayarlar� !!!!!!!!!
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddMemoryCache(); //bunu eklemezsek de�i�ik bir hata alabiliriz
builder.Services.AddSession(); //cookie autentication (use it if possible rather than jwt Authorization)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();                   //cookie autentication(simplier than jwt Authorization)
//------ Identity Framework Ayarlar� //------
//sonras�nda proje sa� t�k- open in terminal -dotnet run seeddata yaz 


var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
   await Seed.SeedUsersAndRolesAsync(app);  //ilk ba�ta buray� kullanm�yoruz alttaki kod sat�r�n� kullan�yoruz. identity framework dan sonra buray� kullan�yoruz
   // Seed.SeedData(app);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //we need to add this to be able to use like a social media app like all user can edit theirs data but not others users data...
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
