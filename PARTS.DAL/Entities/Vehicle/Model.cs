namespace PARTS.DAL.Entities.Vehicle
{
    public class Model : Base
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Seats { get; set; }
        public string? Doors { get; set; }
        public DateTime? Year { get; set; }
        public Make? Make { get; set; }
        public List<Vehicle>? Vehicles { get; set; } = new List<Vehicle>();
        public List<SubModel>? SubModels { get; set; } = new List<SubModel>();

    }
}
