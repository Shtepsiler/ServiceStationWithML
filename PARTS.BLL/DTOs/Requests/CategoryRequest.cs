namespace PARTS.BLL.DTOs.Requests
{
    public class CategoryRequest : BaseDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public CategoryImageRequest? CategoryImage { get; set; }
        public List<PartRequest>? Parts { get; set; }

    }
}
