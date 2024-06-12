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
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
//Configuracion de mapeo
builder.Services.AddAutoMapper(typeof(MappingConfig)); 
//Configuracion de servicio de acceso al contexto de Session
builder.Services.AddHttpContextAccessor();
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
// Servicios generales, mas de un repositorio
builder.Services.AddSingleton<IServiceAPI, ServiceAPI>(); // Inyeccion de dependencia para estudio
//Usuario 
builder.Services.AddHttpClient<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(100);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IGeneralAPIServices, GeneralAPIService>(); // Servicios de validacion por API
// Servicios de administracion DICOM
builder.Services.AddSingleton<IDicomValidationService, DicomValidationService>(); // Inyeccion de dependencia para Validacion Dicom
builder.Services.AddSingleton<IDicomDecompositionService,DicomDecompositionService>(); // Inyeccion de dependencia para Decomposicion Dicom
builder.Services.AddSingleton<IDicomOrchestrator, DicomOrchestrator>(); // Inyeccion de dependencia orchestador
//builder.Services.AddSingleton<IDicomImageFinderService, DicomImageFinderService>(); // Inyeccion de dependencia orchestador
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                                    .AddCookie(options =>
                                    {
                                        options.Cookie.HttpOnly = true;
                                        options.ExpireTimeSpan = TimeSpan.FromMinutes(100);
                                        options.LoginPath = "/Usuario/Login";
                                        options.AccessDeniedPath = "/Usuario/AccesoDenegado";
                                        options.SlidingExpiration = true;
                                    });

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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
