namespace PARTS.DAL.Entities.Item
{
    public class PartImage : Base
    {
        public string? SourceContentType { get; set; }
        public string? HashFileContent { get; set; }
        public int? Size { get; set; }
        public string? Path { get; set; }
        public string? Title { get; set; }
        public Guid? PartId { get; set; }
        public Part? Part { get; set; }
    }
}
