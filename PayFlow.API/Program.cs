using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Core.Servicies;
using PayFlow.DOMAIN.Infrastructure.Data;
using PayFlow.DOMAIN.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;
var connectionString = config.GetConnectionString("DevConnection");
builder.Services.AddDbContext<PayflowContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddTransient<INotificacionesRepository, NotificacionesRepository>();
builder.Services.AddTransient<INotificacionService, NotificacionService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTransient<IUsuariosRepository, UsuariosRepository>();
builder.Services.AddTransient<IUsuariosService, UsuariosService>();
builder.Services.AddTransient<IAdministradoresRepository, AdministradoresRepository>();
builder.Services.AddTransient<ITransaccionesRepository, TransaccionesRepository>();
builder.Services.AddTransient<ITransaccionesService, TransaccionesService>();
builder.Services.AddTransient<IAdministradorService, AdministradorService>();
<<<<<<< HEAD
builder.Services.AddScoped<ICuentasRepository, CuentasRepository>();
builder.Services.AddScoped<IUsuarioDashboardService, UsuarioDashboardService>();
builder.Services.AddScoped<IValidacionManualService, ValidacionManualService>();
builder.Services.AddScoped<IHistorialValidacionesRepository, HistorialValidacionesRepository>();
builder.Services.AddScoped<IReporteFinancieroService, ReporteFinancieroService>();

=======
builder.Services.AddTransient<JwtTokenGenerator>();

//Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(bearer => {

    bearer.RequireHttpsMetadata = false;
    bearer.SaveToken = true;
    bearer.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["JWTSettings:SecretKey"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true, // Habilita la validación de expiración
        RequireExpirationTime = true // Requiere que el token tenga tiempo de expiración
    };
});

builder.Services.AddHttpClient();
>>>>>>> bb7536d3585a20b6fc434edcd3b09fcf90c48232

//Add swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "PayFlow API",
        Version = "v1"
    });
});

// Configuración de autenticación JWT

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Solo para desarrollo
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("clave-secreta-para-desarrollo")),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
<<<<<<< HEAD
app.UseAuthentication(); // agregado
=======

app.UseAuthentication();

>>>>>>> bb7536d3585a20b6fc434edcd3b09fcf90c48232
app.UseAuthorization();

app.MapControllers();

app.Run();
