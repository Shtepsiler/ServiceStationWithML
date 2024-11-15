using Moq;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceStationTests.Mocks.PartsBLL
{
    public class MockBrandService : Mock<IBrandService>
    {
        public MockBrandService MockGetAllAsync(IEnumerable<BrandResponse> result)
        {
            Setup(x => x.GetAllAsync()).ReturnsAsync(result);
            return this;
        }

        public MockBrandService MockGetByIdAsync(BrandResponse result)
        {
            Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(result);
            return this;
        }

        public MockBrandService MockPostAsync(BrandResponse result)
        {
            Setup(x => x.PostAsync(It.IsAny<BrandRequest>())).ReturnsAsync(result);
            return this;
        }

        public MockBrandService MockUpdateAsync(BrandResponse result)
        {
            Setup(x => x.UpdateAsync(It.IsAny<BrandRequest>())).ReturnsAsync(result);
            return this;
        }

        public MockBrandService MockDeleteByIdAsync()
        {
            Setup(x => x.DeleteByIdAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            return this;
        }

    }
}
