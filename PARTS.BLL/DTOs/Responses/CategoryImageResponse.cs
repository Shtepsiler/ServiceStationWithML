namespace PARTS.BLL.DTOs.Responses
{
    public class CategoryImageResponse : BaseDTO
    {
        public string? SourceContentType { get; set; }
        public string? HashFileContent { get; set; }
        public int? Size { get; set; }
        public string? Path { get; set; }
        public string? Title { get; set; }

        public CategoryResponse? Category { get; set; }

    }
}
