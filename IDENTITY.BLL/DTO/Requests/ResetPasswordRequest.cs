using Microsoft.AspNetCore.Mvc;

namespace IDENTITY.BLL.DTO.Requests
{
    public class ResetPasswordRequest
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string NewPasword { get; set; }

    }
}
