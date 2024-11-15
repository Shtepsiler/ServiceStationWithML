﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDENTITY.DAL.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public Role(string Name) : base(Name)
        {
        }
    }
}
