using AlfaPackalApi;
using AlfaPackalApi.Datos;
using AlfaPackalApi.Repositorio;
using AlfaPackalApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(Option =>
{
    Option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
 });

builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<IEstudioRepositorio, EstudioRepositorio>();
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
