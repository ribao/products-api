using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductsWebAPI.Data;
using ProductsWebAPI.Model;
using ProductsWebAPI.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<ProductsDbContext>(options =>
    options.UseInMemoryDatabase("ProductsDb"));

builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

builder.Services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = "admin");

builder.Services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
    .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });

builder.Services
    .AddSwaggerGen(swagger =>
    {
        swagger.AddSecurityDefinition("TestAuthHandler", new OpenApiSecurityScheme
        {
            Name = "UserId",
            Type = SecuritySchemeType.ApiKey,
            Scheme = TestAuthHandler.AuthenticationScheme,
            BearerFormat = "Test",
            In = ParameterLocation.Header,
            Description = "This is a test auth handler.",
        });
        swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "TestAuthHandler"
                    }
                },
                []
            }
        });
    })
    .AddEndpointsApiExplorer()
    .AddAuthorization();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
    SeedData(dbContext);
}

// Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

// Health Check Endpoint (Anonymous)
app.MapGet("/api/health/status", () => Results.Ok("Healthy")).AllowAnonymous();

// Secure Endpoints

// Create a product
app.MapPost("/api/products", async (CreateProductCommand command, IMediator mediator) =>
{
    var product = await mediator.Send(command);
    return Results.Created($"/api/products/{product.Id}", product);
}).AllowAnonymous();

// Get all products
app.MapGet("/api/products", async (IMediator mediator) =>
{
    var products = await mediator.Send(new GetAllProductsQuery());
    return Results.Ok(products);
}).RequireAuthorization();

// Get products by color
app.MapGet("/api/products/color/{color}", async (string color, IMediator mediator) =>
{
    var products = await mediator.Send(new GetProductsByColorQuery(color));
    return Results.Ok(products);
}).AllowAnonymous();

// Get a product by ID
app.MapGet("/api/products/{id}", async (int id, IMediator mediator) =>
{
    var product = await mediator.Send(new GetProductByIdQuery(id));
    return product == null ? Results.NotFound() : Results.Ok(product);
}).AllowAnonymous();

app.Run();

// Add some test data to DB
void SeedData(ProductsDbContext dbContext)
{
    if (dbContext.Products.Any()) return;
    var products = new List<Product>
    {
        new() { Name = "Red iPhone", Color = "Red", Price = 1100 },
        new() { Name = "Blue iPhone", Color = "Blue", Price = 1200 },
        new() { Name = "Black iPhone", Color = "Black", Price = 1300 },
    };

    dbContext.Products.AddRange(products);
    dbContext.SaveChanges();
}



// Make the implicit Program class public so test projects can access it
public partial class Program { }