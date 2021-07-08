using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    /// <summary>
    /// M_VisitList_Info:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class M_VisitList_Info
    {
        public M_VisitList_Info()
        { }
        #region Model
        private long _visitid;
        private string _visitno;
        private string _visitorname;
        private string _visitorsex;
        private string _visitorcompany;
        private string _visitortel;
        private string _visitoraddress;
        private byte[] _visitorphoto;
        private byte[] _visitorcertphoto;
        private byte[] _bVisitorphoto;
        private byte[] _bVisitorcertphoto;
        private int? _visitorcount;
        private string _reasonname;
        private string _belongslist;
        private string _certkindname;
        private string _certnumber;
        private string _cardtype;
        private string _cardno;
        private string _indoorname;
        private string _outdoorname;
        private int? _empno;
        private int? _visitorflag;
        private DateTime? _intime;
        private DateTime? _outtime;
        private int? _operterid;
        private string _carkind;
        private string _carnumber;
        private int? _upload;
        private int _grantAD;
        private string _field1;
        private string _field2;
        private string _field3;
        private string _field4;
        private string _field5;
        private string _field6;
        private string _field7;
        private string _field8;
        private string _field9;
        private string _field10;
        private string _field11;
        private string _field12;
        private byte[] _codebar;
        private string _qrCodeMsg;
        private byte[] _bqrImage;
        private byte[] _qrImage;
        private string _wgCardId = "";
        private int _empReception;
        private string _askStatusTs;
        private DateTime _askTimeTs;
        private string _telRecordFilename;
        private string _faceScore;
        private byte[] _fingerPrint;
        private string _idcertFingerCompare;
        private int _uploadFlag;

        /// <summary>
        /// 
        /// </summary>
        public long VisitId
        {
            set { _visitid = value; }
            get { return _visitid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string VisitNo
        {
            set { _visitno = value; }
            get { return _visitno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string VisitorName
        {
            set { _visitorname = value; }
            get { return _visitorname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string VisitorSex
        {
            set { _visitorsex = value; }
            get { return _visitorsex; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string VisitorCompany
        {
            set { _visitorcompany = value; }
            get { return _visitorcompany; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string VisitorTel
        {
            set { _visitortel = value; }
            get { return _visitortel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string VisitorAddress
        {
            set { _visitoraddress = value; }
            get { return _visitoraddress; }
        }
        /// <summary>
        /// 拍照图片
        /// </summary>
        public byte[] VisitorPhoto
        {
            set { _visitorphoto = value; }
            get { return _visitorphoto; }
        }
        /// <summary>
        /// 证件图片
        /// </summary>
        public byte[] VisitorCertPhoto
        {
            set { _visitorcertphoto = value; }
            get { return _visitorcertphoto; }
        }
        /// <summary>
        /// 拍照图片流
        /// </summary>
        public byte[] BVisitorPhoto
        {
            get { return _bVisitorphoto; }
            set { _bVisitorphoto = value; }
        }
        /// <summary>
        /// 证件图片流
        /// </summary>
        public byte[] BVisitorCertPhoto
        {
            get { return _bVisitorcertphoto; }
            set { _bVisitorcertphoto = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? VisitorCount
        {
            set { _visitorcount = value; }
            get { return _visitorcount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ReasonName
        {
            set { _reasonname = value; }
            get { return _reasonname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BelongsList
        {
            set { _belongslist = value; }
            get { return _belongslist; }
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
        public string CardType
        {
            set { _cardtype = value; }
            get { return _cardtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CardNo
        {
            set { _cardno = value; }
            get { return _cardno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InDoorName
        {
            set { _indoorname = value; }
            get { return _indoorname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OutDoorName
        {
            set { _outdoorname = value; }
            get { return _outdoorname; }
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
        /// 
        /// </summary>
        public int? VisitorFlag
        {
            set { _visitorflag = value; }
            get { return _visitorflag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? InTime
        {
            set { _intime = value; }
            get { return _intime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? OutTime
        {
            set { _outtime = value; }
            get { return _outtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OperterId
        {
            set { _operterid = value; }
            get { return _operterid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarKind
        {
            set { _carkind = value; }
            get { return _carkind; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarNumber
        {
            set { _carnumber = value; }
            get { return _carnumber; }
        }
        /// <summary>
        /// 公安网上传标识,1 表示已上传
        /// </summary>
        public int? Upload
        {
            set { _upload = value; }
            get { return _upload; }
        }
        /// <summary>
        /// 是否授权门禁 0：不授权，1：授权
        /// </summary>
        public int GrantAD
        {
            get { return _grantAD; }
            set { _grantAD = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Field1
        {
            set { _field1 = value; }
            get { return _field1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Field2
        {
            set { _field2 = value; }
            get { return _field2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Field3
        {
            set { _field3 = value; }
            get { return _field3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Field4
        {
            set { _field4 = value; }
            get { return _field4; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Field5
        {
            set { _field5 = value; }
            get { return _field5; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Field6
        {
            set { _field6 = value; }
            get { return _field6; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Field7
        {
            set { _field7 = value; }
            get { return _field7; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Field8
        {
            set { _field8 = value; }
            get { return _field8; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Field9
        {
            set { _field9 = value; }
            get { return _field9; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Field10
        {
            set { _field10 = value; }
            get { return _field10; }
        }
        public string Field11
        {
            get { return _field11; }
            set { _field11 = value; }
        }
        public string Field12
        {
            get { return _field12; }
            set { _field12 = value; }
        }
        /// <summary>
        /// wlt文件数据
        /// </summary>
        public byte[] Codebar
        {
            set { _codebar = value; }
            get { return _codebar; }
        }
        /// <summary>
        ///二维码文本信息
        /// </summary>
        public string QrCodeMsg
        {
            set { _qrCodeMsg = value; }
            get { return _qrCodeMsg; }
        }
        /// <summary>
        ///二维码图片流
        /// </summary>
        public byte[] BQrImage
        {
            set { _bqrImage = value; }
            get { return _bqrImage; }
        }
        /// <summary>
        ///二维码图片文件名
        /// </summary>
        public byte[] QrImage
        {
            set { _qrImage = value; }
            get { return _qrImage; }
        }
        /// <summary>
        /// 微耕门禁授权卡号
        /// </summary>
        public string WgCardId
        {
            get { return _wgCardId; }
            set { _wgCardId = value; }
        }

        /// <summary>
        /// 员工接访，0：否，1：是
        /// </summary>
        public int EmpReception
        {
            get { return _empReception; }
            set { _empReception = value; }
        }

        /// <summary>
        /// 终端回复结果
        /// </summary>
        public string AskStatusTs
        {
            get { return _askStatusTs; }
            set { _askStatusTs = value; }
        }

        /// <summary>
        /// 终端来访确认申请时间
        /// </summary>
        public DateTime AskTimeTs
        {
            get { return _askTimeTs; }
            set { _askTimeTs = value; }
        }

        /// <summary>
        /// 电话录音文件路径
        /// </summary>
        public string TelRecordFilename
        {
            get { return _telRecordFilename; }
            set { _telRecordFilename = value; }
        }

        /// <summary>
        /// 人证识别结果和分值
        /// </summary>
        public string FaceScore
        {
            get { return _faceScore; }
            set { _faceScore = value; }
        }

        /// <summary>
        /// 指纹图像
        /// </summary>
        public byte[] FingerPrint
        {
            get { return _fingerPrint; }
            set { _fingerPrint = value; }
        }

        /// <summary>
        /// 第三代身份证指纹和现场采集指纹的比对结果
        /// </summary>
        public string IdcertFingerCompare
        {
            get { return _idcertFingerCompare; }
            set { _idcertFingerCompare = value; }
        }

        public int UploadFlag
        {
            get { return _uploadFlag; }
            set { _uploadFlag = value; }
        }

        #endregion Model

    }
}
