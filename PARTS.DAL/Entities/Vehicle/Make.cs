namespace PARTS.DAL.Entities.Vehicle
{
    public class Make : Base
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Сountry { get; set; }
        public DateTime? Year { get; set; }

        public List<Vehicle>? Vehicles { get; set; } = new List<Vehicle>();
        public List<Model>? Models { get; set; } = new List<Model>();
        public List<Engine>? Engines { get; set; } = new List<Engine>();
    }
}
