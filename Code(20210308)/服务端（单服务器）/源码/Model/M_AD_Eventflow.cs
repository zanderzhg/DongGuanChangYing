using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public class M_AD_Eventflow
    {
        private string sn;

        public string Sn
        {
            get { return sn; }
            set { sn = value; }
        }

        private string cardNo;

        public string CardNo
        {
            get { return cardNo; }
            set { cardNo = value; }
        }
        private string recordTime;

        public string RecordTime
        {
            get { return recordTime; }
            set { recordTime = value; }
        }
        private string rEvent;

        public string REvent
        {
            get { return rEvent; }
            set { rEvent = value; }
        }
        private string doorNo;

        public string DoorNo
        {
            get { return doorNo; }
            set { doorNo = value; }
        }
        private string readerNo;

        public string ReaderNo
        {
            get { return readerNo; }
            set { readerNo = value; }
        }
    }
}
