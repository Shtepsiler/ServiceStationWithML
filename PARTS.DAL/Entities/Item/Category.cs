using Microsoft.Identity.Client;

namespace PARTS.DAL.Entities.Item
{
    public class Category : Base
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid? CategoryImageId { get; set; }
        public CategoryImage? CategoryImage { get; set; }
        public List<Part>? Parts { get; set; }

    }
}
