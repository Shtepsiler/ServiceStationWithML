namespace PARTS.DAL.Entities
{
    public class Base
    {
        public Base()
        {
        }

        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

    }
}
