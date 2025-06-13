using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Core.Servicies;
using PayFlow.DOMAIN.Infrastructure.Data;
using PayFlow.DOMAIN.Infrastructure.Repositories;


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
builder.Services.AddTransient<IRetiroService, RetiroService>();
builder.Services.AddScoped<ICuentasRepository, CuentasRepository>();
builder.Services.AddScoped<IUsuarioDashboardService, UsuarioDashboardService>();
builder.Services.AddScoped<IValidacionManualService, ValidacionManualService>();
builder.Services.AddScoped<IHistorialValidacionesRepository, HistorialValidacionesRepository>();
builder.Services.AddScoped<IReporteFinancieroService, ReporteFinancieroService>();
builder.Services.AddTransient<JwtTokenGenerator>();
builder.Services.AddTransient<ICuentasRepository, CuentasRepository>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IDepositoService, DepositoService>();
builder.Services.AddHttpContextAccessor();

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
        ValidateLifetime = true, // Habilita la validacin de expiracin
        RequireExpirationTime = true, // Requiere que el token tenga tiempo de expiracin
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddHttpClient();

//Add swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "PayFlow API",
        Version = "v1"
    });

    // Configura la autenticación JWT para Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer",
        Description = "Ingresa el token JWT con el prefijo 'Bearer '"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
