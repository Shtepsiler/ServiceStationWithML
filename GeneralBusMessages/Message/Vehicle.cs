namespace GeneralBusMessages.Message
{
    public class Vehicle 
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; } 
        public string VIN { get; set; }
        public DateTime? Year { get; set; }
        public Guid? MakeId { get; set; }
        public Guid? ModelId { get; set; }
        public Guid? SubModelId { get; set; }
        public Guid? EngineId { get; set; }
        public string? URL { get; set; }
      
    }
}
