using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARTS.BLL.DTOs.Requests
{
    public class CreateVehicleRequest
    {
        public Guid? MakeId { get; set; }
        public Guid? ModelId { get; set; }
        public Guid? SubModelId { get; set; }
        public int Year { get; set; }
        public string VIN { get; set; }
    }
}
