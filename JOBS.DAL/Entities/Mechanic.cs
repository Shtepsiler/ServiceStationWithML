using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOBS.DAL.Entities
{
    public class Mechanic
    {
        public Guid? MechanicId { get; set; }
        public Guid? SpecialisationId { get; set; }
        public Specialisation? Specialisation { get; set; }
        public List<MechanicsTasks?>? MechanicsTasks { get; set; } = new List<MechanicsTasks>();
    }
}
