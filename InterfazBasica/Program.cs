using FellowOakDicom.Network;
using FellowOakDicom.Network.Client;
using InterfazBasica.Service;
using InterfazBasica.Service.IService;
using InterfazBasica_DCStore;
using InterfazBasica_DCStore.Service.DicomServices;
using static Utileria.GeneralFunctions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingConfig)); //Configuracion de mapeo
builder.Services.AddHttpClient<IEstudioService, EstudioService>(); // Cliente Http de estudio
builder.Services.AddScoped<IEstudioService, EstudioService>(); // Inyeccion de dependencia para estudio

// 19/01/2024 Luis Felipe MG .- Retiro inyeccion de dependencia
//builder.Services.AddSingleton<IDicomServer>(dicomServer =>DicomServerFactory.Create<CStoreSCP>(GetServerPort(0))); // Configuracion de servidor DICOM


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
