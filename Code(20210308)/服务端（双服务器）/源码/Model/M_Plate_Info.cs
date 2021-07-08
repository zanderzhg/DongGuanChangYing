using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
   public partial class M_Plate_Info
    {
       public int id { get; set; }
       public string platetype { get; set; }
       public string visitno { get; set; }
       public string plate { get; set; }
       public DateTime startdate { get; set; }
       public DateTime enddate { get; set; }
       public string inset { get; set; }
       public string outset { get; set; }
       public int isdelete { get; set; }
    }
}
