using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace IDENTITY.BLL.DTO.Requests
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
        
        
        [JsonIgnore]
        public string referer { get; set; }    

    }
}
