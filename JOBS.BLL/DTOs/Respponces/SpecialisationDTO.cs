using JOBS.BLL.Common.Mappings;
using JOBS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOBS.BLL.DTOs.Respponces
{
    public  class SpecialisationDTO : IMapFrom<Specialisation>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        

    }
}
