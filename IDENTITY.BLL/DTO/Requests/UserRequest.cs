namespace IDENTITY.BLL.DTO.Requests
{
    public class UserRequest
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? EmailConfirmed { get; set; }
    }
}
