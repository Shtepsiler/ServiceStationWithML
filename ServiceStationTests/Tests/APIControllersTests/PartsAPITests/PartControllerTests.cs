using ClientPartAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ServiceStationTests.Tests.APIControllersTests.PartsAPITests
{
    public class PartControllerTests
    {
        private readonly Mock<ILogger<PartController>> _loggerMock;
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly Mock<IPartService> _serviceMock;
        private PartController _controller => new PartController(_loggerMock.Object, _cacheMock.Object, _serviceMock.Object);

        public PartControllerTests()
        {
            _loggerMock = new Mock<ILogger<PartController>>();
            _cacheMock = new Mock<IDistributedCache>();
            _serviceMock = new Mock<IPartService>();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithPartList()
        {
            // Arrange
            var parts = new List<PartResponse>
            {
                new PartResponse { Id = Guid.NewGuid(), PartNumber = "Part1" },
                new PartResponse { Id = Guid.NewGuid(), PartNumber = "Part2" }
            };
            _serviceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(parts);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<PartResponse>>(okResult.Value);
            Assert.Equal(parts.Count, returnValue.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithPart()
        {
            // Arrange
            var partId = Guid.NewGuid();
            var part = new PartResponse { Id = partId, PartNumber = "Part1" };
            _serviceMock.Setup(service => service.GetByIdAsync(partId)).ReturnsAsync(part);

            // Act
            var result = await _controller.GetByIdAsync(partId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<PartResponse>(okResult.Value);
            Assert.Equal(partId, returnValue.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenPartDoesNotExist()
        {
            // Arrange
            var partId = Guid.NewGuid();
            _serviceMock.Setup(service => service.GetByIdAsync(partId)).ReturnsAsync((PartResponse)null);

            // Act
            var result = await _controller.GetByIdAsync(partId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostAsync_ReturnsCreated_WhenPartIsValid()
        {
            // Arrange
            var partRequest = new PartRequest { PartNumber = "Part1" };
            var partResponse = new PartResponse { Id = Guid.NewGuid(), PartNumber = "Part1" };

            _serviceMock.Setup(service => service.PostAsync(It.IsAny<PartRequest>()))
                .ReturnsAsync(partResponse);

            // Act
            var result = await _controller.PostAsync(partRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            var returnValue = Assert.IsType<PartResponse>(createdResult.Value);
            Assert.Equal(partResponse.Id, returnValue.Id);
            Assert.Equal(partResponse.PartNumber, returnValue.PartNumber);
        }

        [Fact]
        public async Task PostAsync_ReturnsBadRequest_WhenPartIsNull()
        {
            // Act
            var result = await _controller.PostAsync(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Обєкт Part є null", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNoContent_WhenPartIsUpdated()
        {
            // Arrange
            var partRequest = new PartRequest { Id = Guid.NewGuid(), PartNumber = "Part1" };
            var partResponse = new PartResponse { Id = partRequest.Id, PartNumber = "Part1" };
            _serviceMock.Setup(service => service.UpdateAsync(It.IsAny<PartRequest>())).ReturnsAsync(partResponse);

            // Act
            var result = await _controller.UpdateAsync(partRequest);

            // Assert
            var res = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(204, res.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsBadRequest_WhenPartIsNull()
        {
            // Act
            var result = await _controller.UpdateAsync(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Обєкт Part є null", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsNoContent_WhenPartIsDeleted()
        {
            // Arrange
            var partId = Guid.NewGuid();
            var part = new PartResponse { Id = partId, PartNumber = "Part1" };
            _serviceMock.Setup(service => service.GetByIdAsync(partId)).ReturnsAsync(part);

            // Act
            var result = await _controller.DeleteByIdAsync(partId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsNotFound_WhenPartDoesNotExist()
        {
            // Arrange
            var partId = Guid.NewGuid();
            _serviceMock.Setup(service => service.GetByIdAsync(partId)).ReturnsAsync((PartResponse)null);

            // Act
            var result = await _controller.DeleteByIdAsync(partId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetPartsByOrderIdAsync_ReturnsOkResult_WithPartsForOrder()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var parts = new List<PartResponse>
            {
                new PartResponse { Id = Guid.NewGuid(), PartNumber = "Part1" },
                new PartResponse { Id = Guid.NewGuid(), PartNumber = "Part2" }
            };
            _serviceMock.Setup(service => service.GetPartsByOrderId(orderId)).ReturnsAsync(parts);

            // Act
            var result = await _controller.GetPartsByOrderIdAsync(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<PartResponse>>(okResult.Value);
            Assert.Equal(parts.Count, returnValue.Count());
        }
    }
}
