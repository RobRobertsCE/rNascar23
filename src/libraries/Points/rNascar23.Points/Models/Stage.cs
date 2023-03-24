using System.Collections.Generic;

namespace rNascar23.Points.Models
{
    public class Stage
    {  
        public int race_id { get; set; }
        public int run_id { get; set; }
        public int stage_number { get; set; }
        public IList<StagePoints> StagePoints { get; set; }
    }
}
