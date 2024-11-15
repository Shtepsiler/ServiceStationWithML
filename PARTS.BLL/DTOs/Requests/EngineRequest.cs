namespace PARTS.BLL.DTOs.Requests
{
    public class EngineRequest : BaseDTO
    {
        public int Cylinders { get; set; }
        public int Liter { get; set; }
        public DateTime? Year { get; set; }
        public string? Model { get; set; }
        public SubModelRequest? SubModel { get; set; }
        public MakeRequest? Make { get; set; }

    }
}
