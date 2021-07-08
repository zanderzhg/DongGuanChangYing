using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public partial class M_Company_Info
    {
        public M_Company_Info()
        { }
        #region Model
        private int _companyid;
        private string _companyname;
        private string _companyfloor;
        /// <summary>
        /// 
        /// </summary>
        public int CompanyId
        {
            set { _companyid = value; }
            get { return _companyid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CompanyName
        {
            set { _companyname = value; }
            get { return _companyname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CompanyFloor
        {
            set { _companyfloor = value; }
            get { return _companyfloor; }
        }

       
        #endregion Model

        public M_Company_Info(M_DingDepartment model)
        {
            this.CompanyName = model.name;
        }

    }
}
