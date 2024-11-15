using System.Text.Json.Serialization;

namespace PARTS.BLL.DTOs.Responses
{
    public class VehicleResponse : BaseDTO
    {
        public string? FullModelName { get; set; }
        public string? VIN { get; set; }
        public DateTime? Year { get; set; }
        public string? URL { get; set; }
        [JsonIgnore]
        public MakeResponse? Make { get; set; }
        [JsonIgnore]
        public ModelResponse? Model { get; set; }
        [JsonIgnore]
        public SubModelResponse? SubModel { get; set; }
        [JsonIgnore]
        public EngineResponse? Engine { get; set; }
        [JsonIgnore]
        public List<PartResponse>? Parts { get; set; }
    }
}
