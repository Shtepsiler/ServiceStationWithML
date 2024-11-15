namespace PARTS.BLL.DTOs.Responses
{
    public class OrderResponse : BaseDTO
    {
        public Guid? СustomerId { get; set; }
        public IEnumerable<PartResponse>? Parts { get; set; }
    }
}
