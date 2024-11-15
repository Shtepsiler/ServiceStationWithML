using System.Text.Json.Serialization;

namespace IDENTITY.BLL.DTO.Requests
{
    public class UserSignInRequest
    {
        [JsonIgnore]
        public string? refererUrl { get; set; }
        public string Email { get; set; }
        public bool RememberMe { get; set; }
        public string Password { get; set; }
    }
}
