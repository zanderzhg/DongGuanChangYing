using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public class M_Flow
    {
        public M_Flow()
        { }

        /// <summary>
        /// 刷卡日期(格式：yyyymmdd)
        /// </summary>
        public string trdate;
        /// 刷卡时间(格式：hh24miss)
        /// </summary>
        public string trtime;
        /// <summary>
        /// 事件类型
        /// </summary>
        public string etype;
        /// <summary>
        /// 事件名称
        /// </summary>
        public string ename;
        /// <summary>
        /// 身份证序列号
        /// </summary>
        public string cardserial;
        /// <summary>
        /// <summary>
        /// 终端编号
        /// </summary>
        public string tdcode;
        /// <summary>
        /// 门点号
        /// </summary>
        public string doornum;

    }
}
