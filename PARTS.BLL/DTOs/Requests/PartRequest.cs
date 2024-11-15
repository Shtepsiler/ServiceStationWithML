namespace PARTS.BLL.DTOs.Requests
{
    public class PartRequest : BaseDTO
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
        public Guid? CategoryId { get; set; }

        public BrandRequest? Brand { get; set; }
        public CategoryRequest? Category { get; set; }

    }
}
