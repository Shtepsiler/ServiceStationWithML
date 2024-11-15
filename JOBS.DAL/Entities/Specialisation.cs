using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOBS.DAL.Entities
{
    public class Specialisation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Mechanic> Mechanics { get; set; } = new();
    }
}
