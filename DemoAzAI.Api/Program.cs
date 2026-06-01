using DemoAzAI.Api.Extensions;
using DemoAzAI.Api.MiddleWares;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Base api
builder.Services.AddOpenApi();
builder.Services.AddSwaggerDocumentation();


builder.Logging.AddSimpleConsole(options =>
{
    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff ";
    options.UseUtcTimestamp = true;
});

// Azure clients
builder.Services.AddAzureAiServices(builder.Configuration);

// Appication services
builder.Services.AddApplicationServices();
builder.Services.AddStoreServices();


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
