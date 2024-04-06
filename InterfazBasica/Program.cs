using FellowOakDicom;
using FellowOakDicom.Network;
using FellowOakDicom.Network.Client;
using InterfazBasica.Service;
using InterfazBasica.Service.IService;
using InterfazBasica_DCStore;
using InterfazBasica_DCStore.Service;
using InterfazBasica_DCStore.Service.DicomServices;
using InterfazBasica_DCStore.Service.IDicomService;
using InterfazBasica_DCStore.Service.IService;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingConfig)); //Configuracion de mapeo
//***  Servicios de BDs y API ***//
// Estudio
builder.Services.AddHttpClient<IEstudioService, EstudioService>(); // Cliente Http de estudio
builder.Services.AddSingleton<IEstudioService, EstudioService>(); // Inyeccion de dependencia para estudio
// Paciente
builder.Services.AddHttpClient<IPacienteService, PacienteService>(); // Cliente Http de estudio
builder.Services.AddSingleton<IPacienteService, PacienteService>(); // Inyeccion de dependencia para estudio
// Serie
builder.Services.AddHttpClient<ISerieService, SerieService>(); // Cliente Http de estudio
builder.Services.AddSingleton<ISerieService, SerieService>(); // Inyeccion de dependencia para estudio
// Imagen
builder.Services.AddHttpClient<IImageService, ImagenService>(); // Cliente Http de estudio
builder.Services.AddSingleton<IImageService, ImagenService>(); // Inyeccion de dependencia para estudio
// Servicio de registros
builder.Services.AddSingleton<IServiceAPI, ServiceAPI>(); // Inyeccion de dependencia para estudio
builder.Services.AddSingleton<IValidationService, ValidationService>(); // Servicios de validacion por API
// Servicios de administracion DICOM
builder.Services.AddSingleton<IDicomValidationService, DicomValidationService>(); // Inyeccion de dependencia para Validacion Dicom
builder.Services.AddSingleton<IDicomDecompositionService,DicomDecompositionService>(); // Inyeccion de dependencia para Decomposicion Dicom
builder.Services.AddSingleton<IDicomOrchestrator, DicomOrchestrator>(); // Inyeccion de dependencia orchestador


var app = builder.Build();
ServiceLocator.SetServiceProvider(app.Services);
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
