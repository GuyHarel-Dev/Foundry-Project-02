using DemoAzAI.Api.Extensions;
using DemoAzAI.Api.MiddleWares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add this project services
builder.Services.AddOpenApi();
builder.Services.AddSwaggerDocumentation();

// Azure clients
builder.Services.AddAzureOpenAiProjectResponsesClient(builder.Configuration);
builder.Services.AddAzureDocIntelligenceClient();
builder.Services.AddAzureOpenAiEmbeddingClient(builder.Configuration);

// Azure services
builder.Services.AddDocIntelligenceService();
builder.Services.AddAzureChatFoundryService();
builder.Services.AddEmbeddingService();
builder.Services.AddEmbeddingStoreService();
builder.Services.AddSemanticSearchService();

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
