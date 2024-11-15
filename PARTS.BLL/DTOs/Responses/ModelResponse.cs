namespace PARTS.BLL.DTOs.Responses
{
    public class ModelResponse : BaseDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Year { get; set; }
        public MakeResponse? Make { get; set; }

        public List<VehicleResponse>? Vehicles { get; set; }
        public List<SubModelResponse>? SubModels { get; set; }

    }
}
