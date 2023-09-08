using System.Collections.Immutable;
using System;
using SanaTest.Api.V1.App_Start;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true);
builder.Services.AddDepedencesInjection();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddCors(o => o.AddPolicy(MyAllowSpecificOrigins, builder =>
{
  builder.AllowAnyOrigin().AllowAnyMethod() .AllowAnyHeader();
}));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
