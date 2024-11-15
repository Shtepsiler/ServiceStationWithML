namespace PARTS.BLL.DTOs.Requests
{
    public class SubModelRequest : BaseDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime Year { get; set; }
        public ModelRequest Model { get; set; }
        public List<VehicleRequest>? Vehicles { get; set; }
        public List<EngineRequest>? Engines { get; set; }

    }
}
