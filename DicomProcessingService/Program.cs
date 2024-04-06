using AutoMapper;
using DicomProcessingService;
using DicomProcessingService.Services;
using DicomProcessingService.Services.Interfaces;
using FellowOakDicom;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Dependenci Inyection
builder.Services.AddAutoMapper(typeof(MappingConfig)); //Configuracion de mapeo
builder.Services.AddHttpClient<IEstudioService, EstudioService>(); // Cliente Http de estudio
builder.Services.AddScoped<IEstudioService, EstudioService>(); // Inyeccion de dependencia para estudio
builder.Services.AddScoped<IDicomValidationService, DicomValidationService>();
builder.Services.AddScoped<IDicomDecompositionService, DicomDecompositionService>();
// **** Service locator
var services = new ServiceCollection();
//servicios aquí
services.AddSingleton<IDicomOrchestrator, DicomOrchestrator>(); 
//Proveedor de servicios
var serviceProvider = services.BuildServiceProvider();
// Establece el proveedor de servicios en el ServiceLocator
ServiceLocator.SetServiceProvider(serviceProvider);
// **** 


//var dicomBuilder = WebApplication.CreateBuilder(args);

//// Configuración de fo-dicom
//var setupBuilder = new DicomSetupBuilder();
//setupBuilder.RegisterServices(
//    services => { services.AddSingleton<IDicomOrchestrator, DicomOrchestrator>(); 
//    });
//// Configura opciones específicas de DICOM.
//setupBuilder.SkipValidation().Build();

// Esto podría ser un método para desactivar la validación de los datos DICOM.
// Construye la configuración y aplica las configuraciones.
//.SkipValidation() // Ejemplo de uso de un método de extensión para configurar opciones específicas
//.SetDicomServiceLogging(logDataPdus: true, logDimseDataset: true) // Configurar el registro

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

