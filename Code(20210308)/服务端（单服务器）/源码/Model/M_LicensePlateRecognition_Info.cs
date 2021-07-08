using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public class M_LicensePlateRecognition_Info
    {
        public string visitorName { get; set; }
        public string passType { get; set; }
        public string passNo { get; set; }
        public string visitorPhone { get; set; }
        public string visitorType { get; set; }
        public string certificateType { get; set; }
        public string certificateNo { get; set; }
        public string peopleCount { get; set; }
        public string personId { get; set; }
        public string bookStartTime { get; set; }
        public string bookEndTime { get; set; }
        public string remark { get; set; }
    }
}
