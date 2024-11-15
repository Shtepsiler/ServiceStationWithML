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
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly IMapper _mapper;
        private CategoryService _categoryService=> new CategoryService(_mockRepository.Object, _mapper);

        public CategoryServiceTests()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CategoryRequest, Category>().ReverseMap();
                cfg.CreateMap<Category, CategoryResponse>().ReverseMap();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            // Arrange
            var categories = GetSampleCategories();
            _mockRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            Assert.Equal(categories.Count(), result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectCategory()
        {
            // Arrange
            var categories = GetSampleCategories();
            var id = categories.First().Id;
            _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(categories.First());

            // Act
            var result = await _categoryService.GetByIdAsync(id);

            // Assert
            Assert.Equal(categories.First().Title, result.Title);
        }

        [Fact]
        public async Task PostAsync_AddsNewCategory()
        {
            // Arrange
            var request = new CategoryRequest { Title = "NewCategory" };
            var categoryToAdd = _mapper.Map<Category>(request);

            _mockRepository.Setup(repo => repo.InsertAsync(categoryToAdd)).Returns(Task.CompletedTask);

            // Act
            var result = await _categoryService.PostAsync(request);

            // Assert
            Assert.Equal(request.Title, result.Title);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingCategory()
        {
            // Arrange
            var categories = GetSampleCategories();
            var categoryToUpdate = categories.First();
            var request = new CategoryRequest { Id = categoryToUpdate.Id, Title = "UpdatedCategory" };
            var updatedCategory = _mapper.Map<Category>(request);

            _mockRepository.Setup(repo => repo.UpdateAsync(updatedCategory)).Returns(Task.CompletedTask);

            // Act
            var result = await _categoryService.UpdateAsync(request);

            // Assert
            Assert.Equal(request.Title, result.Title);
        }

        [Fact]
        public async Task DeleteByIdAsync_RemovesCategory()
        {
            // Arrange
            var categories = GetSampleCategories();
            var idToDelete = categories.First().Id;

            _mockRepository.Setup(repo => repo.DeleteAsync(idToDelete)).Returns(Task.CompletedTask);

            // Act
            await _categoryService.DeleteByIdAsync(idToDelete);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(idToDelete), Times.Once);
        }

        private IEnumerable<Category> GetSampleCategories()
        {
            return new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Title = "Category1" },
                new Category { Id = Guid.NewGuid(), Title = "Category2" },
                new Category { Id = Guid.NewGuid(), Title = "Category3" }
            };
        }
    }
}
