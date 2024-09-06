using FellowOakDicom.Network.Client;
using WorkStation.Models;
using static Utileria.GeneralFunctions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registrar MiDicomClient como la implementación de IDicomClient
builder.Services.AddScoped<IDicomClient>(provider =>
    DicomClientFactory.Create(
               GetLocalIPAddress_Real(), // Direccion IP
               GetServerPort(0), // Puerto
               false,// useTIs ( Seguridad adicional )
               "SCU", //CallingAe
               "S01P001" //CalledAe //' S05P004
           )
);

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
