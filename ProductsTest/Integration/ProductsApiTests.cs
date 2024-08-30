
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProductsWebAPI.Model;
using ProductsWebAPI.Security;

namespace ProductsTest.Integration;

public class ProductsApiTests
{
    [Fact]
    public async Task HealthCheck_ReturnsHealthyStatus()
    {
        await using var application = new TestApplication();
        using var client =  application.CreateClient();
        var response = await client.GetAsync("/api/health/status");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().Be("\"Healthy\"");
    }

    [Fact]
    public async Task When_Authorised_ReturnsAllProducts()
    {
        await using var application = new TestApplication();
        using var client =  application.CreateClient();
        client.DefaultRequestHeaders.Add(FakeAuthHandler.ApiKey, "TestUser");
        var response = await client.GetAsync("api/products");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<Product[]>();
        result.Should().HaveCount(3);
    }
    
    
    [Fact]
    public async Task When_Authorised_AddProduct()
    {
        await using var application = new TestApplication();
        using var client =  application.CreateClient();
        client.DefaultRequestHeaders.Add(FakeAuthHandler.ApiKey, "TestUser");
        var response = await client.PostAsync("api/products", JsonContent.Create(new CreateProductCommand("Test Product", "Blue", 1)));
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<Product>();
        result.Name.Should().Be("Test Product");
       
        var allResponse = await client.GetAsync("api/products");
        allResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var allResult = await allResponse.Content.ReadFromJsonAsync<Product[]>();
        allResult.Should().HaveCount(4);
    }
    
    [Fact]
    public async Task ValidateNewProduct_ReturnsBadRequest()
    {
        await using var application = new TestApplication();
        using var client =  application.CreateClient();
        client.DefaultRequestHeaders.Add(FakeAuthHandler.ApiKey, "TestUser");
        var response = await client.PostAsync("api/products", JsonContent.Create(new CreateProductCommand("Test Product", "Blue", 0)));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task When_NotAuthorised_ReturnsUnauthorizedStatus()
    {
        await using var application = new TestApplication();
        using var client =  application.CreateClient();
        var response =await client.GetAsync("api/products");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}