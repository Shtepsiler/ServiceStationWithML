namespace GeneralBusMessages.Message
{
    public class MechanicsTasks
    {
        public Guid Id { get; set; }
        public Guid MechanicId { get; set; }
        public Guid? JobId { get; set; }
        public Job? Job { get; set; }
        public string Task { get; set; }
        public string? Status { get; set; }


    }

}
