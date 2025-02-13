using FellowOakDicom;
using FellowOakDicom.Network;
using FellowOakDicom.Network.Client;
using InterfazBasica.Service;
using InterfazBasica.Service.IService;
using InterfazBasica_DCStore;
using InterfazBasica_DCStore.Service;
using InterfazBasica_DCStore.Service.BackgroundServices;
using InterfazBasica_DCStore.Service.DicomServices;
using InterfazBasica_DCStore.Service.IService;
using InterfazBasica_DCStore.Service.IService.Dicom;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
//Configuracion de mapeo
builder.Services.AddAutoMapper(typeof(MappingConfig)); 
//Configuracion de servicio de acceso al contexto de Session
builder.Services.AddHttpContextAccessor();
//***  Servicios de BDs y API ***//
// Servicios generales, mas de un repositorio
//builder.Services.AddSingleton<IDicomWebService, DicomWebService>(); // Inyeccion de dependencia para estudio
//Usuario 
builder.Services.AddHttpClient<IUserService, UserService>();
builder.Services.AddScoped<IUserService, UserService>();
//Servicios DicomWeb
builder.Services.AddHttpClient<IDicomWebService, DicomWebService>();
builder.Services.AddSingleton<IDicomWebService,DicomWebService> ();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(100);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// ** Servicios de administracion DICOM
// Dicom Validation service
builder.Services.AddSingleton<IDicomValidationService, DicomValidationService>(); // Inyeccion de dependencia para Validacion Dicom
// Channels
builder.Services.AddHostedService<DicomChannel>();
builder.Services.AddHostedService<DicomBatchChannel>();
// Local storage 
builder.Services.AddSingleton<ILocalDicomStorageService, LocalDicomStorageService>();
// Decomposition dicom service
//builder.Services.AddSingleton<IDicomDecompositionService,DicomDecompositionService>(); // Inyeccion de dependencia para Decomposicion Dicom
builder.Services.AddSingleton<IDicomDecompositionService, DicomDecompositionService2>(); // Inyeccion de dependencia para Decomposicion Dicom

// Dicom web service
builder.Services.AddSingleton<IDicomWebService,DicomWebService> ();


//builder.Services.AddSingleton<IDicomOrchestrator, DicomOrchestrator>(); // Inyeccion de dependencia orchestador
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
