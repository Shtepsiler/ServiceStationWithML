namespace IDENTITY.BLL.DTO.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? EmailConfirmed { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public bool? hasPassword { get; set; } = null;
    }
}
