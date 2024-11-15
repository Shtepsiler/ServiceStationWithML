using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOBS.BLL.DTOs.Respponces
{
    public class RetrainRespponce
    {
        public List<Guid> ids { get; set; }
        public List<string> new_data { get; set; }
        public List<string> new_labels { get; set; }
    }
}
