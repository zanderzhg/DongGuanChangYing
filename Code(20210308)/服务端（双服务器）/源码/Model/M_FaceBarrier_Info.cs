using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public partial class M_FaceBarrier_Info
    {
        public string token { get; set; }
        public DateTime? recordtime { get; set; }
        public byte[] compareimg { get; set; }
        public byte[] matchimg { get; set; }
        public string visitno { get; set; }
        public string certnumber { get; set; }
        /// <summary>
        /// 机器码，一般用作进出标识0进，1出
        /// </summary>
        public string machinecode { get; set; }
        public string visitorname { get; set; }
        public string empno { get; set; }
        public string comparescore { get; set; }
        public string department { get; set; }
        /// <summary>
        /// 人员类别，0为员工，1为访客
        /// </summary>
        public int persontype { get; set; }
        public string passageway { get; set; }
        public string devicename { get; set; }
        public string deviceIP { get; set; }
        public string devicetype { get; set; }

        public string deviceID { get; set; }
        public string outerID { get; set; }

        public int compareresult { get; set; }

    }
}
