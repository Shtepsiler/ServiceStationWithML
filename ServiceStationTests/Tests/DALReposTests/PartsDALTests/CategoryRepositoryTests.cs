using PARTS.DAL.Entities.Item;
using PARTS.DAL.Excepstions;
using PARTS.DAL.Repositories;
using ServiceStationTests.Mocks.PartsDAl;

namespace ServiceStationTests.Tests.DALReposTests.PartsDALTests
{
    public class CategoryRepositoryTests
    {
        private CategoryRepository _categoryRepository;
        private MockPartsDBContext<Category> _mockContext;
        [Fact]
        public async Task GetAsync_ReturnsAllBrands()
        {
            // Arrange

            _mockContext = new MockPartsDBContext<Category>(GetData());
            var context = _mockContext.GetPartsDBContext();
            _categoryRepository = new CategoryRepository(context);

            // Act
            var result = await _categoryRepository.GetAsync();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectBrand()
        {
            // Arrange
            var data = GetData();
            _mockContext = new MockPartsDBContext<Category>(data);
            var context = _mockContext.GetPartsDBContext();
            _categoryRepository = new CategoryRepository(context);

            var invalidId = data[0].Id;

            // Act & Assert
            var result = await _categoryRepository.GetByIdAsync(invalidId);


            // Assert
            Assert.Equal("Category1", result.Title);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsEntityNotFoundException_ForInvalidId()
        {
            // Arrange

            _mockContext = new MockPartsDBContext<Category>(GetData());
            var context = _mockContext.GetPartsDBContext();
            _categoryRepository = new CategoryRepository(context);

            var invalidId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _categoryRepository.GetByIdAsync(invalidId));
        }

        [Fact]
        public async Task InsertAsync_AddsNewBrand()
        {
            // Arrange

            _mockContext = new MockPartsDBContext<Category>(GetData());
            var context = _mockContext.GetPartsDBContext();
            _categoryRepository = new CategoryRepository(context);

            var newBrand = new Category { Id = Guid.NewGuid(), Title = "Category4" };

            // Act
            await _categoryRepository.InsertAsync(newBrand);

            // Assert
            var brands = await _categoryRepository.GetAsync();
            Assert.Equal(4, brands.Count());
            Assert.Contains(brands, b => b.Title == "Category4");
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingBrand()
        {
            // Arrange
            var data = GetData();
            _mockContext = new MockPartsDBContext<Category>(data);
            var context = _mockContext.GetPartsDBContext();
            _categoryRepository = new CategoryRepository(context);

            var brandToUpdate = data[0];
            brandToUpdate.Title = "UpdatedCategory";

            // Act
            await _categoryRepository.UpdateAsync(brandToUpdate);

            // Assert
            var updatedBrand = await _categoryRepository.GetByIdAsync(brandToUpdate.Id);
            Assert.Equal("UpdatedCategory", updatedBrand.Title);
        }

        [Fact]
        public async Task DeleteAsync_RemovesBrand()
        {
            // Arrange
            var data = GetData();
            _mockContext = new MockPartsDBContext<Category>(data);
            var context = _mockContext.GetPartsDBContext();
            _categoryRepository = new CategoryRepository(context);

            var idToDelete = data[0].Id;

            // Act
            await _categoryRepository.DeleteAsync(idToDelete);

            // Assert
            var brands = await _categoryRepository.GetAsync();
            Assert.Equal(2, brands.Count());
            Assert.DoesNotContain(brands, b => b.Id == idToDelete);
        }

        private List<Category> GetData()
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
