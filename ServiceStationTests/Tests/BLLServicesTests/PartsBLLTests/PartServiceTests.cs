using AutoMapper;
using Moq;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services;
using PARTS.DAL.Entities;
using PARTS.DAL.Entities.Item;
using PARTS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ServiceStationTests.Tests.BLLServicesTests.PartsBLLTests
{
    public class PartServiceTests
    {
        private readonly Mock<IPartRepository> _mockRepository;
        private readonly IMapper _mapper;
        private  PartService _partService=> new PartService(_mockRepository.Object, _mapper);

        public PartServiceTests()
        {
            _mockRepository = new Mock<IPartRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PartRequest, Part>().ReverseMap();
                cfg.CreateMap<Part, PartResponse>().ReverseMap();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllParts()
        {
            // Arrange
            var parts = GetSampleParts();
            _mockRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(parts);

            // Act
            var result = await _partService.GetAllAsync();

            // Assert
            Assert.Equal(parts.Count(), result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectPart()
        {
            // Arrange
            var parts = GetSampleParts();
            var id = parts.First().Id;
            _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(parts.First());

            // Act
            var result = await _partService.GetByIdAsync(id);

            // Assert
            Assert.Equal(parts.First().PartNumber, result.PartNumber);
        }

        [Fact]
        public async Task PostAsync_AddsNewPart()
        {
            // Arrange
            var request = new PartRequest { PartNumber = "NewPart", CategoryId = Guid.NewGuid() };
            var partToAdd = _mapper.Map<Part>(request);

            _mockRepository.Setup(repo => repo.InsertAsync(partToAdd)).Returns(Task.CompletedTask);

            // Act
            var result = await _partService.PostAsync(request);

            // Assert
            Assert.Equal(request.PartNumber, result.PartNumber);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingPart()
        {
            // Arrange
            var parts = GetSampleParts();
            var partToUpdate = parts.First();
            var request = new PartRequest { Id = partToUpdate.Id, PartNumber = "UpdatedPart", CategoryId = partToUpdate.CategoryId };
            var updatedPart = _mapper.Map<Part>(request);

            _mockRepository.Setup(repo => repo.UpdateAsync(updatedPart)).Returns(Task.CompletedTask);

            // Act
            var result = await _partService.UpdateAsync(request);

            // Assert
            Assert.Equal(request.PartNumber, result.PartNumber);
        }

        [Fact]
        public async Task DeleteByIdAsync_RemovesPart()
        {
            // Arrange
            var parts = GetSampleParts();
            var idToDelete = parts.First().Id;

            _mockRepository.Setup(repo => repo.DeleteAsync(idToDelete)).Returns(Task.CompletedTask);

            // Act
            await _partService.DeleteByIdAsync(idToDelete);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(idToDelete), Times.Once);
        }

        [Fact]
        public async Task GetPartsByOrderIdAsync_ReturnsPartsForOrder()
        {
            // Arrange
            var parts = GetSampleParts();
            var orderId = parts.First().Orders.First().Id;
            _mockRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(parts);

            // Act
            var result = await _partService.GetPartsByOrderId(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        private IEnumerable<Part> GetSampleParts()
        {
            var categoryId = Guid.NewGuid();
            return new List<Part>
            {
                new Part { Id = Guid.NewGuid(), PartNumber = "Part1", CategoryId = categoryId, Orders = new List<Order> { new Order { Id = Guid.NewGuid() } } },
                new Part { Id = Guid.NewGuid(), PartNumber = "Part2", CategoryId = categoryId, Orders = new List<Order> { new Order { Id = Guid.NewGuid() } } },
                new Part { Id = Guid.NewGuid(), PartNumber = "Part3", CategoryId = categoryId, Orders = new List<Order> { new Order { Id = Guid.NewGuid() } } }
            };
        }
    }
}
