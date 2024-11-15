using PARTS.BLL.DTOs.Responses;

namespace PARTS.BLL.DTOs.Requests
{
    public class OrderRequest : BaseDTO
    {
        public Guid? СustomerId { get; set; }
        public IEnumerable<PartResponse>? Parts { get; set; }

    }
}
