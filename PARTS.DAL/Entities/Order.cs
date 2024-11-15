using PARTS.DAL.Entities.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARTS.DAL.Entities
{
    public class Order: Base
    {
        public Guid? СustomerId { get; set; }
        public List<Part> Parts { get; set; } = new List<Part>();

    }
}
