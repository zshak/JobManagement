using FluentValidation;
using JobManagementApi.Extensions;
using JobManagementApi.Middlewares;
using JobManagementApi.Services;
using JobManagementApi.Models;
using JobManagementApi.Models.Connections;
using JobManagementApi.Models.DTOS;
using JobManagementApi.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<Connector>(builder.Configuration.GetSection("Connection"));
builder.Services.AddOptions();
builder.Services.AddServices();


var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();