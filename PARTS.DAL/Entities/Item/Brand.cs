namespace PARTS.DAL.Entities.Item
{
    public class Brand : Base
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<Part>? Parts { get; set; }
    }
}
