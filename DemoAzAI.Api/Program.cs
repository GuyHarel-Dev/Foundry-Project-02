using DemoAzAI.Api.Extensions;
using DemoAzAI.Api.MiddleWares;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add this project services
builder.Services.AddOpenApi();
builder.Services.AddSwaggerDocumentation();

// Azure clients
builder.Services.AddAzureAiServices(builder.Configuration);

// Api services
builder.Services.AddApplicationServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerDocumentation();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseMiddleware<HttpErrorHandlingMiddleware>();

app.Run();
