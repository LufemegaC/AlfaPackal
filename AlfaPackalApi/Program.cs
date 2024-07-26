using AlfaPackalApi;
using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Repositorio;
using Api_PACsServer.Repositorio.Cargas;
using Api_PACsServer.Repositorio.IRepositorio;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Repositorio.Pacs;
using Api_PACsServer.Services;
using Api_PACsServer.Services.IService;
using Api_PACsServer.Services.IService.Pacs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Numerics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingresar Bearer [space] tuToken \r\n\r\n " +
                      "Ejemplo: Bearer 123456abcder",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id= "Bearer"
                },
                Scheme = "oauth2",
                Name="Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
 });
// Relacion de IdentityUser con DBContext
builder.Services.AddIdentity<UsuarioSistema, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddAutoMapper(typeof(MappingConfig));



// Repositorios para informacion de carga
builder.Services.AddScoped<IEstudioCargaRepositorio, EstudioCargaRepositorio>();
builder.Services.AddScoped<ISerieCargaRepositorio, SerieCargaRepositorio>();
builder.Services.AddScoped<IImagenCargaRepositorio, ImagenCargaRepositorio>();
// Repositorios de entidades de PACs
builder.Services.AddScoped<IPacienteRepositorio, PacienteRepositorio>();
builder.Services.AddScoped<IEstudioRepositorio,  EstudioRepositorio>();
builder.Services.AddScoped<ISerieRepositorio,    SerieRepositorio>();
builder.Services.AddScoped<IImagenRepositorio,   ImagenRepositorio>();
// Servicios de actualizacion de info de carga
//builder.Services.AddScoped<IFileSizeService, FileSizeService>();
// Servicios de para manejo de entidades
builder.Services.AddScoped<IEstudioService, EstudioService>();
builder.Services.AddScoped<ISerieService, SerieService>();
builder.Services.AddScoped<IImagenService, ImagenService>();
// Servicio de validacion
builder.Services.AddScoped<IGeneralAPIService, GeneralAPIService>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
//builder.Services.AddScoped<IDoctorRepositorio,   DoctorRepositorio>();
builder.Services.AddScoped<IWhiteListRepositorio, WhiteListRepositorio>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
