using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    /// <summary>
    /// M_Card_Info:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class M_Card_Info
    {
        public M_Card_Info()
        { }
        #region Model
        private int _id;
        private string _cardid;
        private string _usestatus;
        private string _cardtype;
        private string _visitnonow;
        private DateTime? _startdate;
        private DateTime? _enddate;
        private string disposable;
        private string _grantdoormsg;
        private string _grantelevatormsg="";

        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CardId
        {
            set { _cardid = value; }
            get { return _cardid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UseStatus
        {
            set { _usestatus = value; }
            get { return _usestatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CardType
        {
            set { _cardtype = value; }
            get { return _cardtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string VisitNoNow
        {
            set { _visitnonow = value; }
            get { return _visitnonow; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartDate
        {
            set { _startdate = value; }
            get { return _startdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndDate
        {
            set { _enddate = value; }
            get { return _enddate; }
        }

        /// <summary>
        /// 临时卡是否限制一进一出，0：否，1：是
        /// </summary>
        public string Disposable
        {
            get { return disposable; }
            set { disposable = value; }
        }

        /// <summary>
        /// 门点授权信息
        /// </summary>
        public string GrantDoorMsg
        {
            get { return _grantdoormsg; }
            set { _grantdoormsg = value; }
        }

        /// <summary>
        /// 电梯授权信息
        /// </summary>
        public string GrantElevatorMsg
        {
            get { return _grantelevatormsg; }
            set { _grantelevatormsg = value; }
        }

        #endregion Model

    }
}
