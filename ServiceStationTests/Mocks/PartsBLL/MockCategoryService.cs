using Moq;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceStationTests.Mocks.PartsBLL
{
    public class MockCategoryService : Mock<ICategoryService>
    {
        public MockCategoryService MockGetAllAsync(IEnumerable<CategoryResponse> result)
        {
            Setup(x => x.GetAllAsync()).ReturnsAsync(result);
            return this;
        }

        public MockCategoryService MockGetByIdAsync(CategoryResponse result)
        {
            Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(result);
            return this;
        }

        public MockCategoryService MockPostAsync(CategoryResponse result)
        {
            Setup(x => x.PostAsync(It.IsAny<CategoryRequest>())).ReturnsAsync(result);
            return this;
        }

        public MockCategoryService MockUpdateAsync(CategoryResponse result)
        {
            Setup(x => x.UpdateAsync(It.IsAny<CategoryRequest>())).ReturnsAsync(result);
            return this;
        }

        public MockCategoryService MockDeleteByIdAsync()
        {
            Setup(x => x.DeleteByIdAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            return this;
        }
    }
}
