using PARTS.DAL.Entities.Item;
using PARTS.DAL.Excepstions;
using PARTS.DAL.Repositories;
using ServiceStationTests.Mocks.PartsDAl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ServiceStationTests.Tests.DALReposTests.PartsDALTests
{
    public class PartRepositoryTests
    {
        private PartRepository _partRepository;
        private MockPartsDBContext<Part> _mockContext;

        [Fact]
        public async Task GetAsync_ReturnsAllParts()
        {
            // Arrange
            _mockContext = new MockPartsDBContext<Part>(GetData());
            var context = _mockContext.GetPartsDBContext();
            _partRepository = new PartRepository(context);

            // Act
            var result = await _partRepository.GetAsync();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectPart()
        {
            // Arrange
            var data = GetData();
            _mockContext = new MockPartsDBContext<Part>(data);
            var context = _mockContext.GetPartsDBContext();
            _partRepository = new PartRepository(context);

            var validId = data[0].Id;

            // Act
            var result = await _partRepository.GetByIdAsync(validId);

            // Assert
            Assert.Equal("Part1", result.PartName);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsEntityNotFoundException_ForInvalidId()
        {
            // Arrange
            _mockContext = new MockPartsDBContext<Part>(GetData());
            var context = _mockContext.GetPartsDBContext();
            _partRepository = new PartRepository(context);

            var invalidId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _partRepository.GetByIdAsync(invalidId));
        }

        [Fact]
        public async Task InsertAsync_AddsNewPart()
        {
            // Arrange
            _mockContext = new MockPartsDBContext<Part>(GetData());
            var context = _mockContext.GetPartsDBContext();
            _partRepository = new PartRepository(context);

            var newPart = new Part
            {
                Id = Guid.NewGuid(),
                PartName = "Part4",
                PartNumber = "PN4",
                PriceRegular = 100,
                CategoryId = Guid.NewGuid(),
            };

            // Act
            await _partRepository.InsertAsync(newPart);

            // Assert
            var parts = await _partRepository.GetAsync();
            Assert.Equal(4, parts.Count());
            Assert.Contains(parts, p => p.PartName == "Part4");
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingPart()
        {
            // Arrange
            var data = GetData();
            _mockContext = new MockPartsDBContext<Part>(data);
            var context = _mockContext.GetPartsDBContext();
            _partRepository = new PartRepository(context);

            var partToUpdate = data[0];
            partToUpdate.PartName = "UpdatedPart";

            // Act
            await _partRepository.UpdateAsync(partToUpdate);

            // Assert
            var updatedPart = await _partRepository.GetByIdAsync(partToUpdate.Id);
            Assert.Equal("UpdatedPart", updatedPart.PartName);
        }

        [Fact]
        public async Task DeleteAsync_RemovesPart()
        {
            // Arrange
            var data = GetData();
            _mockContext = new MockPartsDBContext<Part>(data);
            var context = _mockContext.GetPartsDBContext();
            _partRepository = new PartRepository(context);

            var idToDelete = data[0].Id;

            // Act
            await _partRepository.DeleteAsync(idToDelete);

            // Assert
            var parts = await _partRepository.GetAsync();
            Assert.Equal(2, parts.Count());
            Assert.DoesNotContain(parts, p => p.Id == idToDelete);
        }

        private List<Part> GetData()
        {
            return new List<Part>
            {
                new Part
                {
                    Id = Guid.NewGuid(),
                    PartName = "Part1",
                    PartNumber = "PN1",
                    PriceRegular = 100,
                    CategoryId = Guid.NewGuid(),
                },
                new Part
                {
                    Id = Guid.NewGuid(),
                    PartName = "Part2",
                    PartNumber = "PN2",
                    PriceRegular = 200,
                    CategoryId = Guid.NewGuid(),
                },
                new Part
                {
                    Id = Guid.NewGuid(),
                    PartName = "Part3",
                    PartNumber = "PN3",
                    PriceRegular = 300,
                    CategoryId = Guid.NewGuid(),
                }
            };
        }
    }
}
