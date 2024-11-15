using Moq;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceStationTests.Mocks.PartsBLL
{
    public class MockPartService : Mock<IPartService>
    {
        public MockPartService MockGetAllAsync(IEnumerable<PartResponse> result)
        {
            Setup(x => x.GetAllAsync()).ReturnsAsync(result);
            return this;
        }

        public MockPartService MockGetByIdAsync(PartResponse result)
        {
            Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(result);
            return this;
        }

        public MockPartService MockPostAsync(PartResponse result)
        {
            Setup(x => x.PostAsync(It.IsAny<PartRequest>())).ReturnsAsync(result);
            return this;
        }

        public MockPartService MockUpdateAsync(PartResponse result)
        {
            Setup(x => x.UpdateAsync(It.IsAny<PartRequest>())).ReturnsAsync(result);
            return this;
        }

        public MockPartService MockDeleteByIdAsync()
        {
            Setup(x => x.DeleteByIdAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            return this;
        }

    }
}
