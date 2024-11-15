namespace GeneralBusMessages.Message
{
    public class Make 
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; } 
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Year { get; set; }


    }
}
