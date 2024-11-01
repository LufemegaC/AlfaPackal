using AlfaPackalApi;
using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Models.DicomList;
using Api_PACsServer.Orchestrators;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Api_PACsServer.Repositories.DicomSupport;
using Api_PACsServer.Repositories.IRepository.DicomSupport;
using Api_PACsServer.Repositorio.Cargas;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Repositorio.Pacs;
using Api_PACsServer.Repository.Authentication;
using Api_PACsServer.Repository.DataAccess;
using Api_PACsServer.Repository.IRepository.Authentication;
using Api_PACsServer.Services;
using Api_PACsServer.Services.IService;
using Api_PACsServer.Services.IService.Dicom;
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
builder.Services.AddIdentity<SystemUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAutoMapper(typeof(MappingConfig));

// Repositories for load info
builder.Services.AddScoped<IStudyDetailsRepository, StudyDetailsRepository>();
builder.Services.AddScoped<ISerieDetailsRepository, SerieDetailsRepository>();
builder.Services.AddScoped<IInstanceDetailsRepository, InstanceDetailsRepository>();
// Repositories for main entities
builder.Services.AddScoped<IStudyRepository, StudyRepository>();
builder.Services.AddScoped<ISerieRepository, SerieRepository>();
builder.Services.AddScoped<IInstanceRepository, InstanceRepository>();
// Repositories for dicom support
builder.Services.AddScoped<IStudyModalityRepository, StudyModalityRepository>();
// Repositories for access
builder.Services.AddScoped<ILocalDicomServerRepostory, LocalDicomServerRepostory>();
//builder.Services.AddScoped<IInstitutionRespository, InstitutionRespository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Service for entities
builder.Services.AddScoped<IStudyService, StudyService>();
builder.Services.AddScoped<ISerieService, SerieService>();
builder.Services.AddScoped<IInstanceService, InstanceService>();
// services
builder.Services.AddScoped<IDicomFileService, AzureDicomFileService>();
// Session 
builder.Services.AddScoped<ISessionService, SessionService>();
// Orchestrator
builder.Services.AddScoped<IDicomOrchestrator, DicomOrchestrator>();
builder.Services.AddScoped<IAuthOrchestrator, AuthOrchestrator>();

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
