using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QpiqueWeb.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QpiqueWeb.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

// Inicializacion y conexion de la base de datos
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Se usa Mysql para la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30))));

// Filtro de errores
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configuracion de Identity(Usuario y Roles)
builder.Services.AddDefaultIdentity<Usuario>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddErrorDescriber<SpanishIdentityErrorDescriber>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configuracion autenticacion por cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Agregar autenticacion Jwt para Api
builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Agregar controladores MVC y servicios
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<JwtTokenService>();

var app = builder.Build();

// Crear roles si no existen 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = new[] { "Administrador", "Empleado" };

    foreach (var role in roles)
    {
        var roleExists = await roleManager.RoleExistsAsync(role);
        if (!roleExists)
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

// Configuracion HTTP
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware de la app
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Habilita login JWT o Cookies
app.UseAuthorization();

// Mapeo de rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllers();

app.Run();
