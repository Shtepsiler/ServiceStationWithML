using ClientPartAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using System.Text;
using Newtonsoft.Json;
using PARTS.BLL.Services.Interaces;

namespace ServiceStationTests.Tests.APIControllersTests.PartsAPITests
{
    public class CategoryControllerTests
    {
        private readonly Mock<ILogger<CategoryController>> _loggerMock;
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly Mock<ICategoryService> _serviceMock;
        private CategoryController _controller { get => new CategoryController(_serviceMock.Object, _loggerMock.Object, _cacheMock.Object); }

        public CategoryControllerTests()
        {
            _loggerMock = new Mock<ILogger<CategoryController>>();
            _cacheMock = new Mock<IDistributedCache>();
            _serviceMock = new Mock<ICategoryService>();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithCategoryList()
        {
            // Arrange
            var categories = new List<CategoryResponse>
            {
                new CategoryResponse { Id = Guid.NewGuid(), Title = "Category X" },
                new CategoryResponse { Id = Guid.NewGuid(), Title = "Category Y" }
            };
            _serviceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<CategoryResponse>>(okResult.Value);
            Assert.Equal(categories.Count, returnValue.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new CategoryResponse { Id = categoryId, Title = "Category X" };
            _serviceMock.Setup(service => service.GetByIdAsync(categoryId)).ReturnsAsync(category);

            // Act
            var result = await _controller.GetByIdAsync(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CategoryResponse>(okResult.Value);
            Assert.Equal(categoryId, returnValue.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _serviceMock.Setup(service => service.GetByIdAsync(categoryId)).ReturnsAsync((CategoryResponse)null);

            // Act
            var result = await _controller.GetByIdAsync(categoryId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostAsync_ReturnsCreated_WhenCategoryIsValid()
        {
            // Arrange
            var categoryRequest = new CategoryRequest { Title = "Category X" };
            var categoryResponse = new CategoryResponse { Id = Guid.NewGuid(), Title = "Category X" };

            _serviceMock.Setup(service => service.PostAsync(It.IsAny<CategoryRequest>()))
                .ReturnsAsync(categoryResponse);

            // Act
            var result = await _controller.PostAsync(categoryRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            var returnValue = Assert.IsType<CategoryResponse>(createdResult.Value);
            Assert.Equal(categoryResponse.Id, returnValue.Id);
            Assert.Equal(categoryResponse.Title, returnValue.Title);
        }

        [Fact]
        public async Task PostAsync_ReturnsBadRequest_WhenCategoryIsNull()
        {
            // Act
            var result = await _controller.PostAsync(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Обєкт Categoty є null", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNoContent_WhenCategoryIsUpdated()
        {
            // Arrange
            var categoryRequest = new CategoryRequest { Id = Guid.NewGuid(), Title = "Category X" };
            var categoryResponse = new CategoryResponse { Id = categoryRequest.Id, Title = "Category X" };

            _serviceMock.Setup(service => service.UpdateAsync(It.IsAny<CategoryRequest>())).ReturnsAsync(categoryResponse);

            // Act
            var result = await _controller.UpdateAsync(categoryRequest);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(204, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsBadRequest_WhenCategoryIsNull()
        {
            // Act
            var result = await _controller.UpdateAsync(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Обєкт Categoty є null", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsNoContent_WhenCategoryIsDeleted()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new CategoryResponse { Id = categoryId, Title = "Category X" };
            _serviceMock.Setup(service => service.GetByIdAsync(categoryId)).ReturnsAsync(category);

            // Act
            var result = await _controller.DeleteByIdAsync(categoryId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _serviceMock.Setup(service => service.GetByIdAsync(categoryId)).ReturnsAsync((CategoryResponse)null);

            // Act
            var result = await _controller.DeleteByIdAsync(categoryId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
