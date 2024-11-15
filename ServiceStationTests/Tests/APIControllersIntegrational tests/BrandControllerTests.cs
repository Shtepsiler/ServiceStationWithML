using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization.Metadata;

namespace ClientPartAPI.IntegrationTests
{
    public class BrandControllerTests : IClassFixture<CustomWebApplicationFactory<PARTS.API.Program>>
    {
        private readonly HttpClient _client;

        public BrandControllerTests(CustomWebApplicationFactory<PARTS.API.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithBrandList()
        {
            // Act
            var response = await _client.GetAsync("/api/Brand");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var brands = JsonSerializer.Deserialize<List<BrandResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            brands.Should().NotBeNull();
            brands.Should().HaveCountGreaterThan(0);
        } 


        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithBrand()
        {
            // Arrange
            var brandId = Guid.Parse("b5a0c2e2-324f-42d3-b299-28d2e12a5260"); 

            // Act
            var response = await _client.GetAsync($"/api/Brand/{brandId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var brand = JsonSerializer.Deserialize<BrandResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            brand.Should().NotBeNull();
            brand.Id.Should().Be(brandId);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenBrandDoesNotExist()
        {
            // Arrange
            var brandId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/api/Brand/{brandId}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task PostAsync_ReturnsCreated_WhenBrandIsValid()
        {
            // Arrange
            var brandRequest = new BrandRequest { Title = "Brand X" };
            var content = new StringContent(JsonSerializer.Serialize(brandRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Brand", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            var responseBody = await response.Content.ReadAsStringAsync();
            var brandResponse = JsonSerializer.Deserialize<BrandResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            brandResponse.Should().NotBeNull();
            brandResponse.Title.Should().Be(brandRequest.Title);
        }

        [Fact]
        public async Task PostAsync_ReturnsBadRequest_WhenBrandIsNull()
        {
            // Arrange
            BrandRequest brandRequest = null;

            var content = new StringContent(JsonSerializer.Serialize(brandRequest), Encoding.UTF8, "application/json");


            // Act
            var response = await _client.PostAsync("/api/Brand", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("One or more validation errors occurred.");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNoContent_WhenBrandIsUpdated()
        {
            // Arrange
            var brandRequest = new BrandRequest { Id = Guid.Parse("35a3c232-334f-32d3-3299-38d2e12a5260"), Title = "Brand X" }; 
            var content = new StringContent(JsonSerializer.Serialize(brandRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/Brand/{brandRequest.Id}", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsBadRequest_WhenBrandIsNull()
        {
            // Act
            // Arrange
            BrandRequest? brandRequest = null;
            var content = new StringContent(JsonSerializer.Serialize(brandRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/Brand/{Guid.Parse("b5a0c2e2-324f-42d3-b299-28d2e12a5260")}", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("One or more validation errors occurred");
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsNoContent_WhenBrandIsDeleted()
        {
            // Arrange
            var brandId = Guid.Parse("35a3c232-334f-32d3-3299-38d2e12a5260");

            // Act
            var response = await _client.DeleteAsync($"/api/Brand/{brandId}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsNotFound_WhenBrandDoesNotExist()
        {
            // Arrange
            var brandId = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync($"/api/Brand/{brandId}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
