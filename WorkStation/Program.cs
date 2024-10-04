using WorkStation.Models;
using WorkStation.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Habilitar servicios de sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duración de la sesión inactiva antes de que expire
    options.Cookie.HttpOnly = true; // Seguridad: solo accesible a través de HTTP
    options.Cookie.IsEssential = true; // Esencial para que la sesión funcione incluso con políticas de cookies estrictas
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registrar MiDicomClient como la implementación de IDicomClient
builder.Services.AddScoped<ModalitySCU>(provider =>
{
    var ipAddress = LocalUtility.GetLocalIPAddress_Real();
    var port = LocalUtility.GetServerPort(0);
    var useTls = false;
    var callingAe = "SCU";
    var calledAe = "S01P001";

    return new ModalitySCU(ipAddress, port, useTls, callingAe, calledAe);
});

var app = builder.Build();
// Habilitar el middleware de sesión
app.UseSession();

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
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
