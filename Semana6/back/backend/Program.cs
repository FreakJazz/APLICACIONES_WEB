using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Agregar CORS para permitir peticiones desde Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("DesarrolloLocal", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("http://localhost:4200")  // URL del frontend Angular
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();                    // Permitir envío de cookies
    });
});

// Agregar servicios de controladores
builder.Services.AddControllers();

// Agregar servicios de vistas (Razor)
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddRazorPages();

var app = builder.Build();

// Usar CORS
app.UseCors("DesarrolloLocal");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Mapear controladores de API
app.MapControllers();

// Mapear rutas MVC tradicionales
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
