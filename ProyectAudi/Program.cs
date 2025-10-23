using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using ProyectAudi.Data;
using ProyectAudi.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ProyectDbContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<EmailService>();
//builder.Services.AddScoped<IVirusScannerService>(provider =>
//    new VirusScannerService("408d12adf6fe1ef16af3faf5f9ddaffeeb0671a784c3ee85f54d0dbca8c725c7")); // Reemplazá con tu clave real

builder.Services.AddHttpClient(); // Necesario para VirusTotalNet

builder.Services.AddScoped<IVirusScannerService>(provider =>
{
    return new VirusScannerService("6799ae737f29d63232872e6541292cbd169c2fd2c8bd2c96b08bd32666a96a4a"); // Reemplazá con tu clave real
});


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});


var supportedCultures = new[] { new CultureInfo("es-ES") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es-ES"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Credenciales/Index"; // ruta al login
        options.AccessDeniedPath = "/Credenciales/AccesoDenegado"; // opcional
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // duración de la sesión
    });

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

//esto lo agregue para que funcione lo de habilitar o desabilidar los botones de editar, delete y details...

builder.Services.AddSession();





// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization(localizationOptions);


app.UseSession(); // ✅ necesario para HttpContext.Session
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Credenciales}/{action=Index}/{id?}");

app.Run();
