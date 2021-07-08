using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public class M_Employ_Info
    {
        public M_Employ_Info()
        { }
        #region Model
        private int _empno;
        private string _empname;
        private string _empnamepinyin;
        private string _empsex;
        private string _empfloor;
        private string _emproomcode;
        private string _emptel;
        private string _empmobile;
        private string _empexttel;
        private string _empposition;
        private byte[] _empphoto;
        private string _empmemu;
        private int? _deptno;
        private int? _companyid;
        private int? _isVip;
        private string _pwd;
        private string _empcardno;
        private int? _weixinid;
        private string _sjid;

        /// <summary>
        /// 
        /// </summary>
        public int EmpNo
        {
            set { _empno = value; }
            get { return _empno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmpName
        {
            set { _empname = value; }
            get { return _empname; }
        }

        /// <summary>
        /// 姓名拼音（暂只支持postgresql）
        /// </summary>
        public string EmpNamePinYin
        {
            get { return _empnamepinyin; }
            set { _empnamepinyin = value; }
        }
        public string EmployPhotoName { get; set; }
        //public byte[] EmployPhotoImg { get; set; }
        public string EmployNo { get; set; }
        public DateTime EmployCreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EmpSex
        {
            set { _empsex = value; }
            get { return _empsex; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmpFloor
        {
            set { _empfloor = value; }
            get { return _empfloor; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmpRoomCode
        {
            set { _emproomcode = value; }
            get { return _emproomcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmpTel
        {
            set { _emptel = value; }
            get { return _emptel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmpMobile
        {
            set { _empmobile = value; }
            get { return _empmobile; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmpExtTel
        {
            set { _empexttel = value; }
            get { return _empexttel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmpPosition
        {
            set { _empposition = value; }
            get { return _empposition; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] EmpPhoto
        {
            set { _empphoto = value; }
            get { return _empphoto; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmpMemu
        {
            set { _empmemu = value; }
            get { return _empmemu; }
        }
        /// <summary>
        /// 员工卡号
        /// </summary>
        public string EmpCardno
        {
            get { return _empcardno; }
            set { _empcardno = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DeptNo
        {
            set { _deptno = value; }
            get { return _deptno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CompanyId
        {
            set { _companyid = value; }
            get { return _companyid; }
        }
        public int? IsVip
        {
            get { return _isVip; }
            set { _isVip = value; }
        }

        /// <summary>
        /// 预约网站密码
        /// </summary>
        public string Pwd
        {
            get { return _pwd; }
            set { _pwd = value; }
        }

        /// <summary>
        /// 微信预约系统上的员工id
        /// </summary>
        public int? WeixinId
        {
            get { return _weixinid; }
            set { _weixinid = value; }
        }

        /// <summary>
        /// 盛炬一卡通平台上的员工id
        /// </summary>
        public string SjId
        {
            get { return _sjid; }
            set { _sjid = value; }
        }

        public string iStatus { get; set; }

        public string Empcard_ac_grantmsg { get; set; }

        public DateTime EmpCard_ac_enddate { get; set; }

        public int iPush { get; set; }

        /// <summary>
        /// N系列人脸闸机下发的卡号
        /// </summary>
        public string Emp_N_Face_Card_Num { get; set; }

        public string LicensePlate { get; set; }//车牌
        public string EmpNum { get; set; }//工号
        #endregion Model


        public M_Employ_Info(M_DingEmp model,int companyId,int departId)
        {
            this.EmpName = model.name;
            this.EmpMobile = model.mobile;
            this.EmpNum = model.jobnumber;
        }
    }
}

