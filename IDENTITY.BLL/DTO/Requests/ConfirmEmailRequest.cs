namespace IDENTITY.BLL.DTO.Requests
{
    public class ConfirmEmailRequest
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
    }
}
