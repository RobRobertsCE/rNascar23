using System;
using System.Collections.Generic;
using System.Text;

namespace rNascar23.Service.Flags.Data.Models
{
    public class FlagStateModel
    {
        public int lap_number { get; set; }
        public int flag_state { get; set; }
        public float elapsed_time { get; set; }
        public object comment { get; set; }
        public object beneficiary { get; set; }
        public float time_of_day { get; set; }
        public DateTime time_of_day_os { get; set; }
    }
}
