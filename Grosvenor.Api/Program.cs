using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.
// This ensures that services and repositories can be resolved automatically.
builder.Services.AddScoped<IDishManager, DishManager>();
builder.Services.AddScoped<IServer, Server>();
builder.Services.AddScoped<IDishRepository, DishRepository>();

// Add Swagger and configure it to use XML comments for documentation.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Link Swagger to the XML documentation file.
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure Swagger UI for development environment only.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enables Swagger middleware for API documentation.
    app.UseSwaggerUI(); // Configures Swagger UI for interactive documentation.
}

app.UseHttpsRedirection(); // Enforces HTTPS for all requests.

// Map the `/order` endpoint to process meal orders.
app.MapGet("/order", (string unparsedOrder, IServer server) =>
    {
        try
        {
            // Process the order using the server and return the result.
            var result = new { data = server.TakeOrder(unparsedOrder) };
            return Results.Ok(result); // Returns HTTP 200 OK with the processed result.
        }
        catch (Exception e)
        {
            // Handle exceptions and return an error response.
            return Results.BadRequest(e);
        }
    })
    /// <summary>
    /// Processes a meal order and returns the result.
    /// </summary>
    /// <param name="unparsedOrder">The order string, e.g., "morning, 1, 2, 3".</param>
    /// <param name="server">The server responsible for processing the order.</param>
    /// <returns>A JSON object containing the processed order or error details.</returns>
    /// <response code="200">Order successfully processed.</response>
    /// <response code="400">Error occurred while processing the order.</response>
    .WithName("TakeOrder") // Assigns a name to the endpoint.
    .WithOpenApi(); // Includes this endpoint in Swagger documentation.

app.Run(); // Starts the application and listens for incoming requests.