namespace PARTS.BLL.DTOs.Responses
{
    public class SubModelResponse : BaseDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime Year { get; set; }
        public ModelResponse Model { get; set; }
        public List<VehicleResponse>? Vehicles { get; set; }
        public List<EngineResponse>? Engines { get; set; }

    }
}
