using PARTS.DAL.Entities.Item;

namespace PARTS.DAL.Entities.Vehicle
{
    public class Vehicle : Base
    {
        public string? FullModelName
        {
            get
            {
                var makeTitle = Make?.Title ?? string.Empty;
                var modelTitle = Model?.Title ?? string.Empty;
                var subModelTitle = SubModel?.Title ?? string.Empty;
                return $"{makeTitle} {modelTitle} {subModelTitle}".Trim();
            }
            set { }
        }

        public string? VIN { get; set; }
        public DateTime? Year { get; set; }
        public Guid? MakeId { get; set; }
        public Guid? ModelId { get; set; }
        public Guid? SubModelId { get; set; }
        public Guid? EngineId { get; set; }

        public Make? Make { get; set; }
        public Model? Model { get; set; }
        public SubModel? SubModel { get; set; }
        public Engine? Engine { get; set; }

        public string? URL { get; set; }

        public List<Part>? Parts { get; set; } = new List<Part>();
    }
}
