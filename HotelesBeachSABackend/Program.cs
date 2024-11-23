using HotelesBeachSABackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//inyeccion de dependencia al ORM EntityFramework Core
builder.Services.AddScoped<HotelesBeachSABackend.Data.DbContextHotelBeachSA>();




//servicios de ORM
builder.Services.AddDbContext<HotelesBeachSABackend.Data.DbContextHotelBeachSA>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("StringConexion"))
);

//CONFIGURAR JWT
//agregar interfa y objeto que lo implementan
builder.Services.AddScoped<IAutorizacionServices, AutorizacionServices>();

//se toma la llave para generar el token
var key = builder.Configuration.GetValue<string>("JwtSettings:Key");
var keyBytes = Encoding.ASCII.GetBytes(key);

//se configura el esquema de autenticacion
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false; //no requiere metadata
    config.SaveToken = true; // se alamcena el token
    config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true, //valida la key para le inicio de sesion
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes), // se asigna el valor para la key 
        ValidateIssuer = false, // no se valida el emisor
        ValidateAudience = false, //no s evaldia la audiencia
        ValidateLifetime = true, // se valida el teimpo de vida del token
        ClockSkew = TimeSpan.Zero // no debe de existir diferencia de desviacion para el tiempo del reloj
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//inicio servicios de correo
builder.Services.AddTransient<IEmailService, EmailService>();
//fin servicios de correo

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
