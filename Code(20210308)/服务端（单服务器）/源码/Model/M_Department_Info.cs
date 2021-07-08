using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public partial class M_Department_Info
    {
        public M_Department_Info()
        { }
        #region Model
        private int _deptno;
        private string _deptname;
        private string _depttel;
        private string _deptfloor;
        private int? _companyid;
        private int? handlerId;
        private string _sjid;

        /// <summary>
        /// 
        /// </summary>
        public int DeptNo
        {
            set { _deptno = value; }
            get { return _deptno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DeptName
        {
            set { _deptname = value; }
            get { return _deptname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DeptTel
        {
            set { _depttel = value; }
            get { return _depttel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DeptFloor
        {
            set { _deptfloor = value; }
            get { return _deptfloor; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CompanyId
        {
            set { _companyid = value; }
            get { return _companyid; }
        }

        public int? HandlerId
        {
            get { return handlerId; }
            set { handlerId = value; }
        }

        public string SjId
        {
            get { return _sjid; }
            set { _sjid = value; }
        }

        public int DingId { get; set; }

        #endregion Model

        public M_Department_Info(M_DingDepartment model, int comparyId)
        {
            this.DeptName = model.name;
            this.DingId = model.id;
            this.CompanyId = comparyId;
        }
    }
}
