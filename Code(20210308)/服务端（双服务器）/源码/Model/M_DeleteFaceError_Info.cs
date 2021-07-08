using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public partial class M_DeleteFaceError_Info
    {
        public int id { get; set; }
        public int deviceid { get; set; }
        public int deltimes { get; set; }
        public string outerid { get; set; }
        public int delflag { get; set; }
    }
}
