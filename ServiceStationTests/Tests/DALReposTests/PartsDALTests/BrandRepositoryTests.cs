using PARTS.DAL.Entities.Item;
using PARTS.DAL.Excepstions;
using PARTS.DAL.Repositories;
using ServiceStationTests.Mocks.PartsDAl;

namespace ServiceStationTests.Tests.DALReposTests.PartsDALTests
{
    public class BrandRepositoryTests
    {
        private BrandRepository _brandRepository;
        private MockPartsDBContext<Brand> _mockContext;
        [Fact]
        public async Task GetAsync_ReturnsAllBrands()
        {
            // Arrange

            _mockContext = new MockPartsDBContext<Brand>(GetData());
            var context = _mockContext.GetPartsDBContext();
            _brandRepository = new BrandRepository(context);

            // Act
            var result = await _brandRepository.GetAsync();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectBrand()
        {
            // Arrange
            var data = GetData();
            _mockContext = new MockPartsDBContext<Brand>(data);
            var context = _mockContext.GetPartsDBContext();
            _brandRepository = new BrandRepository(context);

            var invalidId = data[0].Id;

            // Act & Assert
            var result = await _brandRepository.GetByIdAsync(invalidId);


            // Assert
            Assert.Equal("Brand1", result.Title);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsEntityNotFoundException_ForInvalidId()
        {
            // Arrange

            _mockContext = new MockPartsDBContext<Brand>(GetData());
            var context = _mockContext.GetPartsDBContext();
            _brandRepository = new BrandRepository(context);

            var invalidId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _brandRepository.GetByIdAsync(invalidId));
        }

        [Fact]
        public async Task InsertAsync_AddsNewBrand()
        {
            // Arrange

            _mockContext = new MockPartsDBContext<Brand>(GetData());
            var context = _mockContext.GetPartsDBContext();
            _brandRepository = new BrandRepository(context);

            var newBrand = new Brand { Id = Guid.NewGuid(), Title = "Brand4" };

            // Act
            await _brandRepository.InsertAsync(newBrand);

            // Assert
            var brands = await _brandRepository.GetAsync();
            Assert.Equal(4, brands.Count());
            Assert.Contains(brands, b => b.Title == "Brand4");
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingBrand()
        {
            // Arrange
            var data = GetData();
            _mockContext = new MockPartsDBContext<Brand>(data);
            var context = _mockContext.GetPartsDBContext();
            _brandRepository = new BrandRepository(context);

            var brandToUpdate = data[0];
            brandToUpdate.Title = "UpdatedBrand";

            // Act
            await _brandRepository.UpdateAsync(brandToUpdate);

            // Assert
            var updatedBrand = await _brandRepository.GetByIdAsync(brandToUpdate.Id);
            Assert.Equal("UpdatedBrand", updatedBrand.Title);
        }

        [Fact]
        public async Task DeleteAsync_RemovesBrand()
        {
            // Arrange
            var data = GetData();
            _mockContext = new MockPartsDBContext<Brand>(data);
            var context = _mockContext.GetPartsDBContext();
            _brandRepository = new BrandRepository(context);

            var idToDelete = data[0].Id;

            // Act
            await _brandRepository.DeleteAsync(idToDelete);

            // Assert
            var brands = await _brandRepository.GetAsync();
            Assert.Equal(2, brands.Count());
            Assert.DoesNotContain(brands, b => b.Id == idToDelete);
        }

        private List<Brand> GetData()
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
