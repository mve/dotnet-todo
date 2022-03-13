using api.Models;
using api.Services;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy =
            null); // Dit zorgt ervoor dat de hoofdletters worden behouden aan het begin van woorden.

builder.Services.Configure<TodoDatabaseSettings>(builder.Configuration.GetSection("TodoDatabase"));

builder.Services.AddSingleton<TodoService>();

var app = builder.Build();

app.UseHttpsRedirection();

// Set up CORS.
app.UseCors(policy =>
    policy.WithOrigins("https://localhost:7220")
        .AllowAnyMethod()
        .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization, "x-custom-header")
        .AllowCredentials());

app.UseAuthorization();

app.MapControllers();

app.Run();