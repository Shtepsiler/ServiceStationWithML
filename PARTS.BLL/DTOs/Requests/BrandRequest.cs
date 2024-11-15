namespace PARTS.BLL.DTOs.Requests
{
    public class BrandRequest : BaseDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public List<PartRequest>? Parts { get; set; }
    }
}
