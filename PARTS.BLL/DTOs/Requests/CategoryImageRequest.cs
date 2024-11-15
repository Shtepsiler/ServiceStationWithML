namespace PARTS.BLL.DTOs.Requests
{
    public class CategoryImageRequest : BaseDTO
    {
        public string? SourceContentType { get; set; }
        public string? HashFileContent { get; set; }
        public int? Size { get; set; }
        public string? Path { get; set; }
        public string? Title { get; set; }

        public CategoryRequest? Categoty { get; set; }

    }
}
