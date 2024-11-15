using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDENTITY.BLL.DTO.Requests
{
    public class ConfirmChangeEmailRequest
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
