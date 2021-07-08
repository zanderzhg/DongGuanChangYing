using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public class M_WG_Record_Info
    {
        public M_WG_Record_Info()
        { }

        /// <summary>
        /// 比较两记录是否属于同一条
        /// </summary>
        /// <param name="record1"></param>
        /// <param name="record2"></param>
        /// <returns></returns>
        public bool Compare(M_WG_Record_Info record)
        {
            if (record == null)
            {
                return false;
            }

            if (cardSNR == record.cardSNR && recordTime == record.recordTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }


        private string visitorName;

        public string VisitorName
        {
            get { return visitorName; }
            set { visitorName = value; }
        }

        private string empName;

        public string EmpName
        {
            get { return empName; }
            set { empName = value; }
        }

        private string cardSNR;

        public string CardSNR
        {
            get { return cardSNR; }
            set { cardSNR = value; }
        }
        private DateTime recordTime;

        public DateTime RecordTime
        {
            get { return recordTime; }
            set { recordTime = value; }
        }
        private string doorName;

        public string DoorName
        {
            get { return doorName; }
            set { doorName = value; }
        }

        private string rEvent;

        public string REvent
        {
            get { return rEvent; }
            set { rEvent = value; }
        }

        private int personType;
        /// <summary>
        /// 记录类型：0：访客，1：员工，2：工作人员开门
        /// </summary>
        public int PersonType
        {
            get { return personType; }
            set { personType = value; }
        }

        private int uploadpf;
        /// <summary>
        /// 是否已上传平台：0：未上传，1：已上传
        /// </summary>
        public int Uploadpf
        {
            get { return uploadpf; }
            set { uploadpf = value; }
        }

        /// <summary>
        /// 门禁板SN码
        /// </summary>
        public string ControllerSN { get; set; }
        /// <summary>
        /// 门禁板IP
        /// </summary>
        public string ControllerIP { get; set; }
        /// <summary>
        /// 门点号
        /// </summary>
        public int DoorIndex { get; set; }
        /// <summary>
        /// 是否进入事件
        /// </summary>
        public int IsEntryEvent { get; set; }
    }
}
