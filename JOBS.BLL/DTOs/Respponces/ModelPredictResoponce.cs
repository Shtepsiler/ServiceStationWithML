using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOBS.BLL.DTOs.Respponces
{
    public class ModelPredictResoponce
    {
        public string predicted_class { get; set; }
        public float confidence { get; set; }
    }
}
