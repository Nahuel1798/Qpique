using Microsoft.EntityFrameworkCore;
using Qpique.Web.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Conexion con la base de datos
builder.Services.AddDbContext<QpiqueDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 36)))); // Cambiá la versión si es necesario

// Configurar autorización por políticas de rol
// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options => {
//         options.LoginPath = "/Usuarios/Login";
//         options.LogoutPath = "/Usuarios/Logout";
//         options.AccessDeniedPath = "/Usuarios/Denegado";
//     });

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
// });

var app = builder.Build();

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

//Autorizacion
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
