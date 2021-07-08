using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public partial class M_ParkingStat_Info
    {
        public int id { get; set; }
        public string parkId { get; set; }
        public string parkName { get; set; }
        public int emptySpaceNum { get; set; }
        public int TotalNum { get; set; }
        public DateTime time { get; set; }
    }
}
