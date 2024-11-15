using ClientPartAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace ServiceStationTests.Tests.APIControllersTests.PartsAPITests
{
    public class BrandControllerTests
    {
        private readonly Mock<ILogger<BrandController>> _loggerMock;
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly Mock<IBrandService> _serviceMock;
        private BrandController _controller => new BrandController(_serviceMock.Object, _loggerMock.Object, _cacheMock.Object);

        public BrandControllerTests()
        {
            _loggerMock = new Mock<ILogger<BrandController>>();
            _cacheMock = new Mock<IDistributedCache>();
            _serviceMock = new Mock<IBrandService>();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithBrandList()
        {
            // Arrange
            var brands = new List<BrandResponse>
            {
                new BrandResponse { Id = Guid.NewGuid(), Title = "Brand X" },
                new BrandResponse { Id = Guid.NewGuid(), Title = "Brand Y" }
            };
            _serviceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(brands);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<BrandResponse>>(okResult.Value);
            Assert.Equal(brands.Count, returnValue.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithBrand()
        {
            // Arrange
            var brandId = Guid.NewGuid();
            var brand = new BrandResponse { Id = brandId, Title = "Brand X" };
            _serviceMock.Setup(service => service.GetByIdAsync(brandId)).ReturnsAsync(brand);

            // Act
            var result = await _controller.GetByIdAsync(brandId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<BrandResponse>(okResult.Value);
            Assert.Equal(brandId, returnValue.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenBrandDoesNotExist()
        {
            // Arrange
            var brandId = Guid.NewGuid();
            _serviceMock.Setup(service => service.GetByIdAsync(brandId)).ReturnsAsync((BrandResponse)null);

            // Act
            var result = await _controller.GetByIdAsync(brandId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostAsync_ReturnsCreated_WhenBrandIsValid()
        {
            // Arrange
            var brandRequest = new BrandRequest { Title = "Brand X" };
            var brandResponse = new BrandResponse { Id = Guid.NewGuid(), Title = "Brand X" };

            _serviceMock.Setup(service => service.PostAsync(It.IsAny<BrandRequest>()))
                .ReturnsAsync(brandResponse);

            // Act
            var result = await _controller.PostAsync(brandRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            var returnValue = Assert.IsType<BrandResponse>(createdResult.Value);
            Assert.Equal(brandResponse.Id, returnValue.Id);
            Assert.Equal(brandResponse.Title, returnValue.Title);
        }

        [Fact]
        public async Task PostAsync_ReturnsBadRequest_WhenBrandIsNull()
        {
            // Act
            var result = await _controller.PostAsync(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Обєкт Brand є null", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNoContent_WhenBrandIsUpdated()
        {
            // Arrange
            var brandRequest = new BrandRequest { Id = Guid.NewGuid(), Title = "Brand X" };
            var brandResponce = new BrandResponse { Id = brandRequest.Id, Title = "Brand X" };
            _serviceMock.Setup(service => service.UpdateAsync(It.IsAny<BrandRequest>())).ReturnsAsync(brandResponce);

            // Act
            var result = await _controller.UpdateAsync(brandRequest);

            // Assert
            var res = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(204, res.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsBadRequest_WhenBrandIsNull()
        {
            // Act
            var result = await _controller.UpdateAsync(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Обєкт Brand є null", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsNoContent_WhenBrandIsDeleted()
        {
            // Arrange
            var brandId = Guid.NewGuid();
            var brand = new BrandResponse { Id = brandId, Title = "Brand X" };
            _serviceMock.Setup(service => service.GetByIdAsync(brandId)).ReturnsAsync(brand);

            // Act
            var result = await _controller.DeleteByIdAsync(brandId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsNotFound_WhenBrandDoesNotExist()
        {
            // Arrange
            var brandId = Guid.NewGuid();
            _serviceMock.Setup(service => service.GetByIdAsync(brandId)).ReturnsAsync((BrandResponse)null);

            // Act
            var result = await _controller.DeleteByIdAsync(brandId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
