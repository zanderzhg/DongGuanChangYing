using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADServer.BLL;

namespace ADServer.Model
{
    public class M_Booking_Info
    {
        public M_Booking_Info()
        { }
        #region Model
        private int _id;
        private string _bookno;
        private string _bookname;
        private string _booksex;
        private string _booktel;
        private DateTime? _bookdate;
        private DateTime? _validTimeStart;
        private DateTime? _validTimeEnd;
        private string _bookreason;
        private string _bookbelongs;
        private int? _booknum;
        private string _bookmenu;
        private string _certkindname;
        private string _certnumber;
        private int? _empno;
        private string _empname;
        private string _emptel;
        private int? _bookflag;
        private DateTime _booktime;
        private int _operter;
        private int _bookType;
        private string _visitorCompany;
        private string _QRCode;
        private string _LicensePlate;
        private int _visitNum;

        public override string ToString()
        {
            if (Empname != "" && Empname != null)
            {
                return Empname;
            }
            else
            {
                M_Employ_Info emp = new B_Employ_Info().GetModel((int)_empno);
                return emp.EmpName;
            }
        }
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
        public string BookNo
        {
            set { _bookno = value; }
            get { return _bookno; }
        }
        /// <summary>
        /// 访客姓名
        /// </summary>
        public string BookName
        {
            set { _bookname = value; }
            get { return _bookname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BookSex
        {
            set { _booksex = value; }
            get { return _booksex; }
        }
        /// <summary>
        /// 访客联系电话
        /// </summary>
        public string BookTel
        {
            set { _booktel = value; }
            get { return _booktel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string VisitorCompany
        {
            get { return _visitorCompany; }
            set { _visitorCompany = value; }
        }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? BookDate
        {
            set { _bookdate = value; }
            get { return _bookdate; }
        }
        /// <summary>
        /// 有效开始时间
        /// </summary>
        public DateTime? ValidTimeStart
        {
            get { return _validTimeStart; }
            set { _validTimeStart = value; }
        }
        /// <summary>
        /// 有效结束时间
        /// </summary>
        public DateTime? ValidTimeEnd
        {
            get { return _validTimeEnd; }
            set { _validTimeEnd = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BookReason
        {
            set { _bookreason = value; }
            get { return _bookreason; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BookBelongs
        {
            set { _bookbelongs = value; }
            get { return _bookbelongs; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BookNum
        {
            set { _booknum = value; }
            get { return _booknum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BookMenu
        {
            set { _bookmenu = value; }
            get { return _bookmenu; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CertKindName
        {
            set { _certkindname = value; }
            get { return _certkindname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CertNumber
        {
            set { _certnumber = value; }
            get { return _certnumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? EmpNo
        {
            set { _empno = value; }
            get { return _empno; }
        }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string Empname
        {
            get { return _empname; }
            set { _empname = value; }
        }
        /// <summary>
        /// 员工电话
        /// </summary>
        public string Emptel
        {
            get { return _emptel; }
            set { _emptel = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BookFlag
        {
            set { _bookflag = value; }
            get { return _bookflag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime BookTime
        {
            set { _booktime = value; }
            get { return _booktime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Operter
        {
            set { _operter = value; }
            get { return _operter; }
        }
        /// <summary>
        /// 预约类型 1、本地预约 2、网页预约 3、微信预约
        /// </summary>
        public int BookType
        {
            get { return _bookType; }
            set { _bookType = value; }
        }

        /// <summary>
        /// 二维码号码
        /// </summary>
        public string QRCode
        {
            get { return _QRCode; }
            set { _QRCode = value; }
        }

        public string LicensePlate
        {
            get { return _LicensePlate; }
            set { _LicensePlate = value; }
        }

        /// <summary>
        /// 来访人数
        /// </summary>
        public int VisitNum
        {
            get { return _visitNum; }
            set { _visitNum = value; }
        }

        public DateTime BookTimeStart { get; set; }
        public int WeiXinId { get; set; }

        public string quyu { get; set; }
        public string area { get; set; }
        #endregion Model
    }

    public class Booking
    {
        public long id { get; set; }
        public string strIdCertNo { get; set; }
        public string strVisitorCompany { get; set; }
        public string strVisitorName { get; set; }
        public string strTel { get; set; }
        public string strSex { get; set; }
        public string strQRCode { get; set; }
        public string strBookName { get; set; }
        public string strBookTel { get; set; }
        public long strValidTimeStart { get; set; }
        public long strValidTimeEnd { get; set; }
        public long strApplyTime { get; set; }
        public string strReason { get; set; }
        public string iVisitorType { get; set; }
        public int iVisitNum { get; set; }
        public string strLicensePlate { get; set; }
        public string strVisitorPhoto { get; set; }
        public byte[] imgBytes;
    }

    class ReturnData<T>
    {
        public string statusCode { get; set; }
        public string message { get; set; }
        public int totalCount { get; set; }
        public T data { get; set; }
    }
}
