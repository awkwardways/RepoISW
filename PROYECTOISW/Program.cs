using Microsoft.EntityFrameworkCore;
using PROYECTOISW.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using PROYECTOISW.Servicios;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ProyectoiswContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MiDB"));
});
builder.Services.AddScoped<IServicioRC, ServicioRC>();

//Agregar Autenticacion con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "MiCookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.AccessDeniedPath = "/Usuario/IniciarSesion";
        options.LoginPath = "/Usuario/IniciarSesion";
        options.LogoutPath = "/Usuario/IniciarSesion";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

//Agregar 
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=IniciarSesion}/{id?}");

app.Run();
