
using APICruzber.Datos;
using APICruzber.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using APICruzber.Controllers;
using APICruzber.Modelo;
using APICruzber.Connection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ConnectionBD>();
builder.Services.AddScoped<DatosCliente>();
builder.Services.AddScoped<ICliente, DatosCliente>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();