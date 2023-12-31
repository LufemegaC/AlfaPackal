using AlfaPackalApi;
using AlfaPackalApi.Datos;
using AlfaPackalApi.Repositorio;
using AlfaPackalApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
 });

builder.Services.AddAutoMapper(typeof(MappingConfig));
//Servicios de PACs
builder.Services.AddScoped<IPacienteRepositorio, PacienteRepositorio>();
builder.Services.AddScoped<IEstudioRepositorio,  EstudioRepositorio>();
builder.Services.AddScoped<ISerieRepositorio,    SerieRepositorio>();
builder.Services.AddScoped<IImagenRepositorio,   ImagenRepositorio>();
// RIS
builder.Services.AddScoped<IDoctorRepositorio,   DoctorRepositorio>();
builder.Services.AddScoped<IPacienteRepositorio, PacienteRepositorio>();
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
