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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
