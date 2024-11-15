namespace GeneralBusMessages.Message
{
    public class Category
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; } 
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid? CategoryImageId { get; set; }
    }
}
