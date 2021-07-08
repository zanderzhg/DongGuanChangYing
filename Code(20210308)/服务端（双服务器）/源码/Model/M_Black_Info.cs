using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    /// <summary>
    /// M_Black_Info:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class M_Black_Info
    {
        public M_Black_Info()
        { }
        #region Model
        private int _blackid;
        private string _listkind;
        private string _certkindname;
        private string _certkindno;
        private string _name;
        private string _sex;
        private string _blackreason;
        private DateTime? _entrydate;
        private string _opertername;
        /// <summary>
        /// 
        /// </summary>
        public int blackid
        {
            set { _blackid = value; }
            get { return _blackid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string listkind
        {
            set { _listkind = value; }
            get { return _listkind; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string certkindname
        {
            set { _certkindname = value; }
            get { return _certkindname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string certkindno
        {
            set { _certkindno = value; }
            get { return _certkindno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string blackreason
        {
            set { _blackreason = value; }
            get { return _blackreason; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? entrydate
        {
            set { _entrydate = value; }
            get { return _entrydate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string opertername
        {
            set { _opertername = value; }
            get { return _opertername; }
        }

        #endregion Model

    }
}
