using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDENTITY.BLL.DTO.Requests
{
    public class SetPhoneNumberRequest
    {
       public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
    }
}
