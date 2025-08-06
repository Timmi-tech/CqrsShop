using System.Text.Json.Serialization;
using Application.Features.Authentication.Commands.RegisterUser;
using Application.Interfaces;
using DotNetEnv;
using Infrastructure.Extensions;
using Presentation.Middlewares;
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Host.ConfigureSerilogService();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigurePostGressContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.ConfigureSwagger(); 
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserHandler).Assembly));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>(); 
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseSwagger(); 
app.UseSwaggerUI(s => 
{ 
s.SwaggerEndpoint("/swagger/v1/swagger.json", "CqrsShop v1"); 

}); 

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();
app.Run();