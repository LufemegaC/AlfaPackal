using ServidorWADO.Services.IService;
using ServidorWADO.Services;
using FellowOakDicom;
using FellowOakDicom.Imaging.NativeCodec;
using FellowOakDicom.Imaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Agrego los servicios
builder.Services.AddScoped<IWadoHelperService, WadoHelperService>();
builder.Services.AddScoped<IDicomImageFinderService, DicomImageFinderService>();
//
builder.Services.AddCors(options =>
{
    options.AddPolicy("WadoCorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .WithMethods("GET")   // Solo permite el método GET
               .AllowAnyHeader();    // Permite cualquier encabezado
    });
});



// Initialize log manager.
new DicomSetupBuilder()
    //.RegisterServices(s => s.AddFellowOakDicom().AddLogging(config => config.AddConsole()))
    .RegisterServices(s => s
                    .AddFellowOakDicom()
                    .AddLogging()
                    .AddTranscoderManager<NativeTranscoderManager>()
                    .AddImageManager<WinFormsImageManager>()
                    )
    .Build();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
