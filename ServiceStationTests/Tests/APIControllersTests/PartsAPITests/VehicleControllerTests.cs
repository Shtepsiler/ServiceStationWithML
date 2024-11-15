using ClientPartAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;

namespace ServiceStationTests.Tests.APIControllersTests.PartsAPITests
{
    public class VehicleControllerTests
    {
        private readonly Mock<ILogger<VehicleController>> _loggerMock;
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly Mock<IVehicleService> _serviceMock;
        private VehicleController _controller { get => new VehicleController(_loggerMock.Object, _cacheMock.Object, _serviceMock.Object); }

        public VehicleControllerTests()
        {
            _loggerMock = new Mock<ILogger<VehicleController>>();
            _cacheMock = new Mock<IDistributedCache>();
            _serviceMock = new Mock<IVehicleService>();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkResult_WithVehicleList()
        {
            // Arrange
            var vehicles = new List<VehicleResponse>
            {
                new VehicleResponse { Id = Guid.NewGuid(), FullModelName = "Model X" },
                new VehicleResponse { Id = Guid.NewGuid(), FullModelName = "Model Y" }
            };
            _serviceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(vehicles);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<VehicleResponse>>(okResult.Value);
            Assert.Equal(vehicles.Count, returnValue.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkResult_WithVehicle()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var vehicle = new VehicleResponse { Id = vehicleId, FullModelName = "Model X" };
            _serviceMock.Setup(service => service.GetByIdAsync(vehicleId)).ReturnsAsync(vehicle);

            // Act
            var result = await _controller.GetByIdAsync(vehicleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<VehicleResponse>(okResult.Value);
            Assert.Equal(vehicleId, returnValue.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenVehicleDoesNotExist()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            _serviceMock.Setup(service => service.GetByIdAsync(vehicleId)).ReturnsAsync((VehicleResponse)null);

            // Act
            var result = await _controller.GetByIdAsync(vehicleId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        public async Task PostAsync_ReturnsCreated_WhenVehicleIsValid()
        {
            // Arrange
            var vehicleRequest = new VehicleRequest { FullModelName = "Model X" };
            var vehicleResponse = new VehicleResponse { Id = Guid.NewGuid(), FullModelName = "Model X" };

            _serviceMock.Setup(service => service.PostAsync(It.IsAny<VehicleRequest>()))
                .ReturnsAsync(vehicleResponse);

            // Act
            var result = await _controller.PostAsync(vehicleRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            var returnValue = Assert.IsType<VehicleResponse>(createdResult.Value);
            Assert.Equal(vehicleResponse.Id, returnValue.Id);
            Assert.Equal(vehicleResponse.FullModelName, returnValue.FullModelName);
        }

        [Fact]
        public async Task PostAsync_ReturnsBadRequest_WhenVehicleIsNull()
        {
            // Arrange
            var vehicleResponse = new VehicleResponse { Id = Guid.NewGuid(), FullModelName = "Model X" };
            _serviceMock.Setup(service => service.PostAsync(It.IsAny<VehicleRequest>()))
                .ReturnsAsync(vehicleResponse);
            // Act
            var result = await _controller.PostAsync(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Обєкт Vehicle є null", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNoContent_WhenVehicleIsUpdated()
        {
            // Arrange
            var vehicleRequest = new VehicleRequest { Id = Guid.NewGuid(), FullModelName = "Model X" };
            var vehicleResponce = new VehicleResponse { Id = vehicleRequest.Id, FullModelName = "Model X" };

            _serviceMock.Setup(service => service.UpdateAsync(It.IsAny<VehicleRequest>())).ReturnsAsync(vehicleResponce);
            // Act
            var result = await _controller.UpdateAsync(vehicleRequest);

            // Assert
           var res = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(204,res.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsBadRequest_WhenVehicleIsNull()
        {
            // Act
            var result = await _controller.UpdateAsync(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Обєкт Vehicle є null", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsNoContent_WhenVehicleIsDeleted()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var vehicle = new VehicleResponse { Id = vehicleId, FullModelName = "Model X" };
            _serviceMock.Setup(service => service.GetByIdAsync(vehicleId)).ReturnsAsync(vehicle);

            // Act
            var result = await _controller.DeleteByIdAsync(vehicleId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsNotFound_WhenVehicleDoesNotExist()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            _serviceMock.Setup(service => service.GetByIdAsync(vehicleId)).ReturnsAsync((VehicleResponse)null);

            // Act
            var result = await _controller.DeleteByIdAsync(vehicleId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}


