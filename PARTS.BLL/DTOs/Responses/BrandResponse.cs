using System.Text.Json.Serialization;

namespace PARTS.BLL.DTOs.Responses
{
    public class BrandResponse : BaseDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }

    }
}
