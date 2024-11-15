namespace PARTS.DAL.Entities.Vehicle
{
    public class SubModel : Base
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Transmission { get; set; }
        public int? Weight { get; set; }

        public DateTime? Year { get; set; }
        public Model? Model { get; set; }
        public List<Vehicle>? Vehicles { get; set; } = new List<Vehicle>();
        public List<Engine>? Engines { get; set; } = new List<Engine>();

    }
}
