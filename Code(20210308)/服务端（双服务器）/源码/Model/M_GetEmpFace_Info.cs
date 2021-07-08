using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public partial class M_GetEmpFace_Info : M_GetEmpCount_Info
    {
        public string sqldoawaywithvalue { get; set; }

        public string sqlquerynumvalue { get; set; }
    }

}
