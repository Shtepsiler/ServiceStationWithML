namespace PARTS.BLL.DTOs.Requests
{
    public class VehicleRequest : BaseDTO
    {
        public string? FullModelName { get; set; }
        public string? VIN { get; set; }
        public DateTime? Year { get; set; }
        public MakeRequest? Make { get; set; }
        public ModelRequest? Model { get; set; }
        public SubModelRequest? SubModel { get; set; }
        public EngineRequest? Engine { get; set; }
        public string? URL { get; set; }
        public List<PartRequest>? Parts { get; set; }
    }
}
