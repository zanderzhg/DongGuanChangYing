using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public partial class M_VisitorFaceRecognition_Info
    {
        public int id { get; set; }
        public int personType { get; set; }//访客、员工
        public string outerid { get; set; }//删除的ID
        public string visitortype { get; set; }//临时、常访
        public string visitno { get; set; }
        public int empno { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public string grantDeviceList { get; set; }
        public string personid { get; set; }
    }
}
