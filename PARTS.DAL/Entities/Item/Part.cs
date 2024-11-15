namespace PARTS.DAL.Entities.Item
{
    public class Part : Base
    {
        public string? PartNumber { get; set; }
        public string? ManufacturerNumber { get; set; }
        public string? Description { get; set; }
        public string? PartName { get; set; }
        public bool? IsUniversal { get; set; }
        public int? PriceRegular { get; set; }
        public string? PartTitle { get; set; }
        public string? PartAttributes { get; set; }
        public bool? IsMadeToOrder { get; set; }
        public string? FitNotes { get; set; }
        public int? Count { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? PartImageId { get; set; }


        public Brand? Brand { get; set; }
        public Category? Category { get; set; }
        public PartImage? PartImage { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

    }
}
