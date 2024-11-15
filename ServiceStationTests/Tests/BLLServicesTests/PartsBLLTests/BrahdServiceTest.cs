using AutoMapper;
using Moq;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services;
using PARTS.DAL.Entities.Item;
using PARTS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ServiceStationTests.Tests.BLLServicesTests.PartsBLLTests
{
    public class BrandServiceTests
    {
        private readonly Mock<IBrandRepository> _mockRepository;
        private readonly IMapper _mapper;
        private BrandService _brandService => new BrandService(_mockRepository.Object, _mapper);

        public BrandServiceTests()
        {
            _mockRepository = new Mock<IBrandRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BrandRequest, Brand>().ReverseMap();
                cfg.CreateMap<Brand, BrandResponse>().ReverseMap();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllBrands()
        {
            // Arrange
            var brands = GetSampleBrands();
            _mockRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(brands);

            // Act
            var result = await _brandService.GetAllAsync();

            // Assert
            Assert.Equal(brands.Count(), result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectBrand()
        {
            // Arrange
            var brands = GetSampleBrands();
            var id = brands.First().Id;
            _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(brands.First());

            // Act
            var result = await _brandService.GetByIdAsync(id);

            // Assert
            Assert.Equal(brands.First().Title, result.Title);
        }

        [Fact]
        public async Task PostAsync_AddsNewBrand()
        {
            // Arrange
            var request = new BrandRequest { Title = "NewBrand" };
            var brandToAdd = _mapper.Map<Brand>(request);

            _mockRepository.Setup(repo => repo.InsertAsync(brandToAdd)).Returns(Task.CompletedTask);

            // Act
            var result = await _brandService.PostAsync(request);

            // Assert
            Assert.Equal(request.Title, result.Title);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingBrand()
        {
            // Arrange
            var brands = GetSampleBrands();
            var brandToUpdate = brands.First();
            var request = new BrandRequest { Id = brandToUpdate.Id, Title = "UpdatedBrand" };
            var updatedBrand = _mapper.Map<Brand>(request);

            _mockRepository.Setup(repo => repo.UpdateAsync(updatedBrand)).Returns(Task.CompletedTask);

            // Act
            var result = await _brandService.UpdateAsync(request);

            // Assert
            Assert.Equal(request.Title, result.Title);
        }

        [Fact]
        public async Task DeleteByIdAsync_RemovesBrand()
        {
            // Arrange
            var brands = GetSampleBrands();
            var idToDelete = brands.First().Id;

            _mockRepository.Setup(repo => repo.DeleteAsync(idToDelete)).Returns(Task.CompletedTask);

            // Act
            await _brandService.DeleteByIdAsync(idToDelete);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(idToDelete), Times.Once);
        }

        private IEnumerable<Brand> GetSampleBrands()
        {
            return new List<Brand>
            {
                new Brand { Id = Guid.NewGuid(), Title = "Brand1" },
                new Brand { Id = Guid.NewGuid(), Title = "Brand2" },
                new Brand { Id = Guid.NewGuid(), Title = "Brand3" }
            };
        }
    }
}
