using InterfazDeUsuario.Services;
using InterfazDeUsuario.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Configuracion de servicio de acceso al contexto de Session
builder.Services.AddHttpContextAccessor();
// Registro de IHttpClientFactory
builder.Services.AddHttpClient<IDataService,DataService>();
builder.Services.AddSingleton<IDataService, DataService>();
// Registro de IHttpClientFactory
builder.Services.AddHttpClient<IWadoUriService, WadoUriService>();
builder.Services.AddSingleton<IWadoUriService, WadoUriService>();
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
