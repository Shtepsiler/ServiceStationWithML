namespace PARTS.DAL.Entities.Item
{
    public class CategoryImage : Base
    {
        public string? SourceContentType { get; set; }
        public string? HashFileContent { get; set; }
        public int? Size { get; set; }
        public string? Path { get; set; }
        public string? Title { get; set; }
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
