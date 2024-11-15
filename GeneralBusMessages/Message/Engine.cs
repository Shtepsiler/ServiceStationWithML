namespace GeneralBusMessages.Message
{
    public class Engine
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int Cylinders { get; set; }
        public int Liter { get; set; }
        public DateTime? Year { get; set; }
        public string? Model { get; set; }
        public Guid? SubModelId { get; set; }
        public Guid? MakeId { get; set; }


    }
}
