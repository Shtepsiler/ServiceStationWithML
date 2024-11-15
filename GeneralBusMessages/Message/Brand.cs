namespace GeneralBusMessages.Message
{
    public class Brand
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; } 
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}
