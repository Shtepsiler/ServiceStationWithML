namespace PARTS.BLL.DTOs.Requests
{
    public class ModelRequest : BaseDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Year { get; set; }
        public MakeRequest? Make { get; set; }

        public List<VehicleRequest>? Vehicles { get; set; }
        public List<SubModelRequest>? SubModels { get; set; }

    }
}
