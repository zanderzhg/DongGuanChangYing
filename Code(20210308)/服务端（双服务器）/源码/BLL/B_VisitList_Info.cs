using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADServer.DAL;
using ADServer.Model;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Web.Script.Serialization;
using System.Data;
using FKY_CMP.Code.SDK;
using FKY_CMP.Code.SDK.Model;

namespace ADServer.BLL
{
    public partial class B_VisitList_Info
    {
        private readonly DAL.D_VisitList_Info dal = new DAL.D_VisitList_Info();
        B_Employ_Info employ_bll = new B_Employ_Info();
        B_Card_Info card_bll = new B_Card_Info();

        public B_VisitList_Info()
        { }

        public string GetReason(string visitno)
        {
            return dal.GetReason(visitno);
        }

        /// <summary>
        /// 根据登记卡的门禁卡号，得到最近一条记录
        /// </summary>
        /// <param name="certno"></param>
        /// <returns>访客单号</returns>
        public string GetVisitNoByWgCardIDRecent(string cardId)
        {
            return dal.GetVisitNoByWgCardIDRecent(cardId);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.M_VisitList_Info model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 根据常访卡号、证件号码，得到最近一次来访信息
        /// 1、常访卡 2、证件号码
        /// </summary>
        /// <param name="certnumber"></param>
        /// <returns></returns>
        public System.Data.DataSet GetVisitedLastInfo(string keyWord, int i)
        {
            return dal.GetVisitedLastInfo(keyWord, i);
        }

        public Model.M_VisitList_Info GetPhoto(string visitno)
        {
            return dal.GetPhoto(visitno);
        }

        /// <summary>
        /// 查询详细记录
        /// </summary>
        public System.Data.DataSet QueryVisitList(string strWhere, bool isManage)
        {
            return dal.QueryVisitList(strWhere, isManage);
        }

        /// <summary>
        /// 根据访客单号得到标识
        /// </summary>
        /// <param name="visitno"></param>
        /// <returns></returns>
        public int GetVisitIdByVisitNo(string visitno)
        {
            return dal.GetVisitIdByVisitNo(visitno);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.M_VisitList_Info GetModel(long VisitId)
        {
            return dal.GetModel(VisitId);
        }

        /// <summary>
        /// 根据门禁记录自主签离
        /// </summary>
        /// <param name="visitno"></param>
        /// <returns></returns>
        public void WgDoLeave(string visitno, string door, string dttime)
        {
            dal.WgDoLeave(visitno, door, dttime);
            //int acType = (int)SysFunc.GetParamValue("AccessControlType");
            int faceServerType = (int)SysFunc.GetParamValue("FaceServerType");
            switch (faceServerType)
            {
                case 1:
                case 2:
                    CancelFace(visitno);
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }

        public string GetVisitNo()
        {
            return dal.GetVisitNo();
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(Model.M_VisitList_Info model)
        {
            return dal.Add(model);
        }


        /// <summary>
        /// 发送短信通知被访人
        /// </summary>
        /// <param name="type">1:登记通知，2：签离通知</param>
        /// <param name="visitno">访客单号</param>
        public void SendSMSToEmp(int type, string visitno)
        {
            M_SMS_Account smsAccount = new SMS().GetModel();
            if (smsAccount == null)
            {
                return;
            }

            string toTel = ""; //被访人手机号码
            M_VisitList_Info visitInfo = GetModel(dal.GetVisitIdByVisitNo(visitno));

            int empNo = -1;
            M_Employ_Info emp = null;
            if (visitInfo != null && visitInfo.EmpNo != null && visitInfo.Field10 != "" && visitInfo.Field10 != null && visitInfo.Field2 != "" && visitInfo.Field2 != null)
            {
                toTel = visitInfo.Field10;
            }
            else
            {
                if (visitInfo.EmpNo != null)
                {
                    emp = (M_Employ_Info)employ_bll.GetModel((int)visitInfo.EmpNo);
                    if (emp != null)
                    {
                        toTel = emp.EmpMobile;
                        empNo = emp.EmpNo;
                    }
                }
            }

            if (toTel != null && toTel != "")
            {
                string checkInContent = SysFunc.GetParamValue("SmsCheckInContent").ToString();
                string leaveContent = SysFunc.GetParamValue("SmsLeaveContent").ToString();

                checkInContent = checkInContent.Replace("@访客姓名", visitInfo.VisitorName);
                leaveContent = leaveContent.Replace("@访客姓名", visitInfo.VisitorName);

                if (empNo == -1 || emp.EmpMobile != null && emp.EmpMobile != "")
                {
                    try
                    {
                        string resp = "";
                        if (type == 1 && smsAccount.NoticeCheckin == "1")
                        {
                            resp = SMS.SendMsg(toTel, checkInContent, smsAccount.Accountname, smsAccount.Pwd, smsAccount.Sign);
                        }
                        else if (type == 2 && smsAccount.NoticeLeave == "1")
                        {
                            resp = SMS.SendMsg(toTel, leaveContent, smsAccount.Accountname, smsAccount.Pwd, smsAccount.Sign);
                        }

                        if (resp != "")
                        {
                            if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                            {
                                Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                            }
                            string nowTime = DateTime.Now.ToString("yyyyMMdd");
                            string file = Application.StartupPath + "\\Logs\\发送短信异常_" + nowTime + ".txt";
                            if (!File.Exists(file))
                            {
                                FileStream fs = new FileStream(file, FileMode.Create);
                                StreamWriter sw = new StreamWriter(fs);
                                sw.Write("发送短信通知被访人失败！报错详情：" + resp);
                                sw.Close();
                                fs.Close();
                            }
                        }
                        else
                        {
                            if (type == 1 && smsAccount.NoticeCheckin == "1")
                            {
                                //写入日志
                                //LogNet.WriteLog((string)SysFunc.GetParamValue("UserName"), "短信通知被访人[" + visitorName + "]来访", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                            else if (type == 2 && smsAccount.NoticeLeave == "1")
                            {
                                //写入日志
                                //LogNet.WriteLog((string)SysFunc.GetParamValue("UserName"), "短信通知被访人[" + visitorName + "]签离", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                        {
                            Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                        }
                        string nowTime = DateTime.Now.ToString("yyyyMMdd");
                        string file = Application.StartupPath + "\\Logs\\发送短信异常_" + nowTime + ".txt";
                        if (!File.Exists(file))
                        {
                            FileStream fs = new FileStream(file, FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write("发送短信通知被访人失败！报错详情：" + ex.ToString());
                            sw.Close();
                            fs.Close();
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 根据证件号码，得到是否存在未签离记录
        /// </summary>
        /// <param name="certno"></param>
        /// <returns>访客单号</returns>
        public string GetVisitNoByCertNo(string certnumber)
        {
            return dal.GetVisitNoByCertNo(certnumber);
        }

        /// <summary>
        /// 根据访客单号签离
        /// </summary>
        /// <param name="visitno"></param>
        /// <returns></returns>
        public void doLeave(string visitno, string datetime)
        {
            dal.doLeave(visitno, datetime);
            //Dictionary<string, string> visitLeaveInfo = new Dictionary<string, string>();
            //visitLeaveInfo.Add(visitno, datetime);
            //Thread threadUpload = new Thread(UploadLeaveInfoThread);
            //threadUpload.IsBackground = true;
            //threadUpload.Start((object)visitLeaveInfo);
            #region 发短信
            //SendSMSToEmp(2, visitno);
            #endregion
            CancelFace(visitno);
            //ADServer.Model.M_VisitorFaceRecognition_Info m = new B_VisitorFaceRecognition_Info().GetModel(visitno);
            //if (m.visitortype == "临时")
            //{
            //    DeleteVisitorFace(visitno);
            //}
            //else
            //{
            //    try
            //    {
            //        if (DateTime.Compare(m.enddate.Value, DateTime.Now) < 0)//过期
            //        {
            //            DeleteVisitorFace(visitno);
            //        }
            //    }
            //    catch (Exception)//无限期
            //    {

            //    }
            //}
        }

        BLL.B_Groble_Info bll_groble = new B_Groble_Info();
        B_VisitorFaceRecognition_Info bll_VisitorFaceRecognition = new B_VisitorFaceRecognition_Info();
        public void doLeave(string visitno, string datetime, string deviceName)
        {
            dal.doLeave(visitno, datetime, deviceName);

            CancelFace(visitno);

        }

        public void doLeaveFace(string visitno, string datetime, string deviceName)
        {
            dal.doLeaveFace(visitno, datetime, deviceName);

            CancelFace(visitno);

        }

        public void ProcessNoAuthenticateFace(string visitno)
        {
            ADServer.Model.M_VisitorFaceRecognition_Info m = bll_VisitorFaceRecognition.GetModel(visitno);
            if (m == null)
            {
                string msg = string.Empty;
                InitFaceSoloServer();
                List<int> deviceList = GetDevice(ref msg);
                if (deviceList != null)
                {
                    B_DeleteFaceError_Info delFaceError = new B_DeleteFaceError_Info();
                    foreach (var deviceID in deviceList)
                    {
                        M_DeleteFaceError_Info m_delFaceError = new M_DeleteFaceError_Info();
                        m_delFaceError.deviceid = deviceID;
                        m_delFaceError.deltimes = 0;
                        m_delFaceError.outerid = visitno;
                        delFaceError.Add(m_delFaceError);
                        LogNet.WriteLog("服务端", "刷脸进入异常处理记录[" + visitno + ":系统查找不到授权记录]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                    }
                }
            }
        }
        public void CancelFace(string visitno)
        {
            #region 取消人脸权限
            if (SysFunc.GetFunctionState("FaceBarrier") == "true")
            {
                ADServer.Model.M_VisitorFaceRecognition_Info m = bll_VisitorFaceRecognition.GetModel(visitno);
                if (m == null)
                {
                    string msg = string.Empty;
                    InitFaceSoloServer();
                    List<int> deviceList = GetDevice(ref msg);
                    if (deviceList != null)
                    {
                        B_DeleteFaceError_Info delFaceError = new B_DeleteFaceError_Info();
                        foreach (var deviceID in deviceList)
                        {
                            M_DeleteFaceError_Info m_delFaceError = new M_DeleteFaceError_Info();
                            m_delFaceError.deviceid = deviceID;
                            m_delFaceError.deltimes = 0;
                            m_delFaceError.outerid = visitno;
                            delFaceError.Add(m_delFaceError);
                            LogNet.WriteLog("服务端", "CancelFace异常处理记录[" + visitno + ":系统查找不到授权记录]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                        }
                    }
                    return;
                }
                else if (m.visitortype == "临时")
                {
                    M_Groble_Info model_groble = bll_groble.GetModel();
                    if (model_groble.LeaveAndCancel == "1")
                    {
                        DeleteVisitorFace(visitno);
                    }
                    else
                    {
                        try
                        {
                            if (DateTime.Compare(m.enddate.Value, DateTime.Now) < 0)//过期
                            {
                                DeleteVisitorFace(visitno);
                            }
                        }
                        catch (Exception)//无效期
                        {
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (DateTime.Compare(m.enddate.Value, DateTime.Now) < 0)//过期
                        {
                            DeleteVisitorFace(visitno);
                        }
                    }
                    catch (Exception)//无效期
                    {
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 根据主访人访客单号获取所有随访人信息
        /// </summary>
        /// <param name="visitno"></param>
        /// <returns></returns>
        public List<M_VisitList_Info> GetFollowListByMainNo(string visitno)
        {
            DataSet ds = dal.GetFollowListByMainNo(visitno);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<M_VisitList_Info> DataTableToList(DataTable dt)
        {
            List<M_VisitList_Info> modelList = new List<M_VisitList_Info>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                M_VisitList_Info model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new M_VisitList_Info();
                    if (dt.Rows[n]["VisitId"] != null && dt.Rows[n]["VisitId"].ToString() != "")
                    {
                        model.VisitId = long.Parse(dt.Rows[n]["VisitId"].ToString());
                    }
                    if (dt.Rows[n]["VisitNo"] != null && dt.Rows[n]["VisitNo"].ToString() != "")
                    {
                        model.VisitNo = dt.Rows[n]["VisitNo"].ToString();
                    }
                    if (dt.Rows[n]["VisitorName"] != null && dt.Rows[n]["VisitorName"].ToString() != "")
                    {
                        model.VisitorName = dt.Rows[n]["VisitorName"].ToString();
                    }
                    if (dt.Rows[n]["VisitorSex"] != null && dt.Rows[n]["VisitorSex"].ToString() != "")
                    {
                        model.VisitorSex = dt.Rows[n]["VisitorSex"].ToString();
                    }
                    if (dt.Rows[n]["VisitorCompany"] != null && dt.Rows[n]["VisitorCompany"].ToString() != "")
                    {
                        model.VisitorCompany = dt.Rows[n]["VisitorCompany"].ToString();
                    }
                    if (dt.Rows[n]["VisitorTel"] != null && dt.Rows[n]["VisitorTel"].ToString() != "")
                    {
                        model.VisitorTel = dt.Rows[n]["VisitorTel"].ToString();
                    }
                    if (dt.Rows[n]["VisitorAddress"] != null && dt.Rows[n]["VisitorAddress"].ToString() != "")
                    {
                        model.VisitorAddress = dt.Rows[n]["VisitorAddress"].ToString();
                    }
                    if (dt.Rows[n]["VisitorPhoto"] != null && dt.Rows[n]["VisitorPhoto"].ToString() != "")
                    {
                        model.VisitorPhoto = (byte[])dt.Rows[n]["VisitorPhoto"];
                    }
                    if (dt.Rows[n]["VisitorCertPhoto"] != null && dt.Rows[n]["VisitorCertPhoto"].ToString() != "")
                    {
                        model.VisitorCertPhoto = (byte[])dt.Rows[n]["VisitorCertPhoto"];
                    }
                    if (dt.Rows[n]["VisitorCount"] != null && dt.Rows[n]["VisitorCount"].ToString() != "")
                    {
                        model.VisitorCount = int.Parse(dt.Rows[n]["VisitorCount"].ToString());
                    }
                    if (dt.Rows[n]["ReasonName"] != null && dt.Rows[n]["ReasonName"].ToString() != "")
                    {
                        model.ReasonName = dt.Rows[n]["ReasonName"].ToString();
                    }
                    if (dt.Rows[n]["BelongsList"] != null && dt.Rows[n]["BelongsList"].ToString() != "")
                    {
                        model.BelongsList = dt.Rows[n]["BelongsList"].ToString();
                    }
                    if (dt.Rows[n]["CertKindName"] != null && dt.Rows[n]["CertKindName"].ToString() != "")
                    {
                        model.CertKindName = dt.Rows[n]["CertKindName"].ToString();
                    }
                    if (dt.Rows[n]["CertNumber"] != null && dt.Rows[n]["CertNumber"].ToString() != "")
                    {
                        model.CertNumber = dt.Rows[n]["CertNumber"].ToString();
                    }
                    if (dt.Rows[n]["CardType"] != null && dt.Rows[n]["CardType"].ToString() != "")
                    {
                        model.CardType = dt.Rows[n]["CardType"].ToString();
                    }
                    if (dt.Rows[n]["CardNo"] != null && dt.Rows[n]["CardNo"].ToString() != "")
                    {
                        model.CardNo = dt.Rows[n]["CardNo"].ToString();
                    }
                    if (dt.Rows[n]["InDoorName"] != null && dt.Rows[n]["InDoorName"].ToString() != "")
                    {
                        model.InDoorName = dt.Rows[n]["InDoorName"].ToString();
                    }
                    if (dt.Rows[n]["OutDoorName"] != null && dt.Rows[n]["OutDoorName"].ToString() != "")
                    {
                        model.OutDoorName = dt.Rows[n]["OutDoorName"].ToString();
                    }
                    if (dt.Rows[n]["EmpNo"] != null && dt.Rows[n]["EmpNo"].ToString() != "")
                    {
                        model.EmpNo = int.Parse(dt.Rows[n]["EmpNo"].ToString());
                    }
                    if (dt.Rows[n]["VisitorFlag"] != null && dt.Rows[n]["VisitorFlag"].ToString() != "")
                    {
                        model.VisitorFlag = int.Parse(dt.Rows[n]["VisitorFlag"].ToString());
                    }
                    if (dt.Rows[n]["InTime"] != null && dt.Rows[n]["InTime"].ToString() != "")
                    {
                        model.InTime = DateTime.Parse(dt.Rows[n]["InTime"].ToString());
                    }
                    if (dt.Rows[n]["OutTime"] != null && dt.Rows[n]["OutTime"].ToString() != "")
                    {
                        model.OutTime = DateTime.Parse(dt.Rows[n]["OutTime"].ToString());
                    }
                    if (dt.Rows[n]["OperterId"] != null && dt.Rows[n]["OperterId"].ToString() != "")
                    {
                        model.OperterId = int.Parse(dt.Rows[n]["OperterId"].ToString());
                    }
                    if (dt.Rows[n]["CarKind"] != null && dt.Rows[n]["CarKind"].ToString() != "")
                    {
                        model.CarKind = dt.Rows[n]["CarKind"].ToString();
                    }
                    if (dt.Rows[n]["CarNumber"] != null && dt.Rows[n]["CarNumber"].ToString() != "")
                    {
                        model.CarNumber = dt.Rows[n]["CarNumber"].ToString();
                    }
                    if (dt.Rows[n]["Field1"] != null && dt.Rows[n]["Field1"].ToString() != "")
                    {
                        model.Field1 = dt.Rows[n]["Field1"].ToString();
                    }
                    if (dt.Rows[n]["Field2"] != null && dt.Rows[n]["Field2"].ToString() != "")
                    {
                        model.Field2 = dt.Rows[n]["Field2"].ToString();
                    }
                    if (dt.Rows[n]["Field3"] != null && dt.Rows[n]["Field3"].ToString() != "")
                    {
                        model.Field3 = dt.Rows[n]["Field3"].ToString();
                    }
                    if (dt.Rows[n]["Field4"] != null && dt.Rows[n]["Field4"].ToString() != "")
                    {
                        model.Field4 = dt.Rows[n]["Field4"].ToString();
                    }
                    if (dt.Rows[n]["Field5"] != null && dt.Rows[n]["Field5"].ToString() != "")
                    {
                        model.Field5 = dt.Rows[n]["Field5"].ToString();
                    }
                    if (dt.Rows[n]["Field6"] != null && dt.Rows[n]["Field6"].ToString() != "")
                    {
                        model.Field6 = dt.Rows[n]["Field6"].ToString();
                    }
                    if (dt.Rows[n]["Field7"] != null && dt.Rows[n]["Field7"].ToString() != "")
                    {
                        model.Field7 = dt.Rows[n]["Field7"].ToString();
                    }
                    if (dt.Rows[n]["Field8"] != null && dt.Rows[n]["Field8"].ToString() != "")
                    {
                        model.Field8 = dt.Rows[n]["Field8"].ToString();
                    }
                    if (dt.Rows[n]["Field9"] != null && dt.Rows[n]["Field9"].ToString() != "")
                    {
                        model.Field9 = dt.Rows[n]["Field9"].ToString();
                    }
                    if (dt.Rows[n]["Field10"] != null && dt.Rows[n]["Field10"].ToString() != "")
                    {
                        model.Field10 = dt.Rows[n]["Field10"].ToString();
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        #region 删除人脸方法（海康）
        /// <summary>
        /// 海康人脸闸机删除卡
        /// </summary>
        /// <param name="cancelArg"></param>
        public string DelFace_HK(IntPtr handle, string cardNO, HikvisionFaceMachine hkcontroller)
        {
            HKCardInfo hkInfo = new HKCardInfo();
            hkInfo.CardNo = cardNO;
            bool isSucc = hkcontroller.ModifyCardInfo(handle, HikvisionFaceMachine.ModifyCommand.DeleteCard, HKCardInfo.ConvertToNET_DVR_CARD_CFG_V50(hkInfo, true));
            hkcontroller.StopCardRemote();
            if (!isSucc)
            {
                return ("人脸闸机 " + hkcontroller.state.IP + " " + hkcontroller.state.ExceptionMsg);
            }
            return "";
        }
        #endregion

        private void DeleteVisitorFace(string visitno)
        {
            if (SysFunc.GetFunctionState("FaceBarrier") == "true")
            {
                M_VisitList_Info visitInfo = GetModel(dal.GetVisitIdByVisitNo(visitno));
                M_VisitorFaceRecognition_Info m = new ADServer.BLL.B_VisitorFaceRecognition_Info().GetModel(visitno);
                if (m != null)
                {
                    int faceServerType = (int)SysFunc.GetParamValue("FaceServerType");
                    switch (faceServerType)
                    {
                        case 1:
                            IAsyncResult result = new Action(InitFaceSoloServer).BeginInvoke(new AsyncCallback((ar) =>
                            {
                                //连接服务器状态
                                System.ServiceModel.CommunicationState wcfState = FKY_WCFLibrary.PlatformServiceClient_Method.WcfState();

                                switch (wcfState)
                                {
                                    case System.ServiceModel.CommunicationState.Closed:
                                        break;
                                    case System.ServiceModel.CommunicationState.Closing:
                                        break;
                                    case System.ServiceModel.CommunicationState.Faulted:
                                        break;
                                    case System.ServiceModel.CommunicationState.Opening:
                                        break;
                                    default:
                                        if (m.visitortype == "临时" || m.enddate.HasValue ? DateTime.Compare(m.enddate.Value, DateTime.Now) > 0 : false)
                                        {
                                            Dictionary<int, string> resMsgs = DelFace_SoloServer(visitInfo, m);
                                        }
                                        break;
                                }
                            }), "人脸单机签离服务");
                            break;
                        case 2:

                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #region 删除人脸方法
        private Dictionary<int, string> DelFace_SoloServer(M_VisitList_Info visitInfo, M_VisitorFaceRecognition_Info m)
        {
            List<string> deviceIDlist = new List<string>(m.grantDeviceList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));//获取授权列表
            var deleteList = deviceIDlist.Select<string, int>(q => Convert.ToInt32(q)).ToList();
            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var deviceID in deleteList)
            {
                string outerIDList = m.outerid;
                string result = DelEmpFace(deviceID, outerIDList);
                DelFaceSuccess(result, deviceID, outerIDList);

                dic.Add(deviceID, result);
                LogNet.WriteLog((string)SysFunc.GetParamValue("UserName"), "访客人脸删除[" + deviceID + "_" + visitInfo.VisitorName.Trim() + "_" + visitInfo.VisitNo + "_" + result + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
            }
            new B_VisitorFaceRecognition_Info().Delete(visitInfo.VisitNo);
            return dic;
        }

        public string DelEmpFace(int deviceID, string dataList)
        {
            return FKY_WCFLibrary.PlatformServiceClient_Method.DelPersonSync(deviceID, dataList);
        }

        /// <summary>
        /// 签离错误数据生成
        /// </summary>
        /// <param name="result"></param>
        /// <param name="deviceID">设备ID</param>
        /// <param name="dataList">删除凭证</param>
        public void DelFaceSuccess(string result, int deviceID, string dataList)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(result);//删除失败
                int code = json.Value<int>("code");
                if (!Process_DelPersonSyncCode(code))
                {
                    B_DeleteFaceError_Info delFaceError = new B_DeleteFaceError_Info();
                    M_DeleteFaceError_Info m = new M_DeleteFaceError_Info();
                    m.deviceid = deviceID;
                    m.deltimes = 0;
                    m.outerid = dataList;
                    delFaceError.Add(m);
                    LogNet.WriteLog("服务端", "DelFaceSuccess签离错误数据生成异常处理记录[" + result + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                }
            }
            catch (Exception ex)
            {
                B_DeleteFaceError_Info delFaceError = new B_DeleteFaceError_Info();
                M_DeleteFaceError_Info m = new M_DeleteFaceError_Info();
                m.deviceid = deviceID;
                m.deltimes = 0;
                m.outerid = dataList;
                delFaceError.Add(m);
                LogNet.WriteLog("服务端", "DelFaceSuccess签离错误数据生成异常处理记录[错误数据生成异常:" + result + ex.Message + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                //FKY_WCFLibrary.WriteLog.Log4Local("错误数据生成异常：\r\n" + deviceID + "_" + dataList + "\r\n" + result + "\r\n" + ex.ToString());
            }
        }

        public bool Process_DelPersonSyncCode(int code)
        {
            bool result = false;
            switch (code)
            {
                case 0:
                case -1:
                case -2:
                case 1:
                case 2:
                    result = true;
                    break;
                case -3:
                case 3:
                case 4:
                case 5:
                case 6:
                default:
                    result = false;
                    break;
            }
            return result;
        }

        public void Process_DelErrorFace()
        {
            try
            {
                B_DeleteFaceError_Info bll_delFaceError = new B_DeleteFaceError_Info();
                DataSet dsErrorlist = bll_delFaceError.GetList(" delflag=0 ");//id,deviceid,deltimes,outerid,delflag
                foreach (DataRow row in dsErrorlist.Tables[0].Rows)
                {
                    M_DeleteFaceError_Info m = new M_DeleteFaceError_Info();
                    m.id = int.Parse(row["id"].ToString());
                    m.deviceid = int.Parse(row["deviceid"].ToString());
                    m.deltimes = int.Parse(row["deltimes"].ToString());
                    m.outerid = row["outerid"].ToString();
                    m.delflag = int.Parse(row["delflag"].ToString());
                    InitFaceSoloServer();
                    string result = DelEmpFace(m.deviceid, m.outerid);
                    Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(result);//删除失败
                    int code = json.Value<int>("code");
                    if (Process_DelPersonSyncCode(code))//删除成功
                    {
                        bll_delFaceError.Delete(m.id);
                    }
                    else//删除失败
                    {
                        if (m.deltimes > 10 && code != 4 && code != -3)//掉线
                        {
                            bll_delFaceError.Delete(m.id);
                        }
                        else
                        {
                            m.deltimes += 1;
                            bll_delFaceError.AddTimes(m.deltimes, m.id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString());
            }
        }

        public List<int> GetDevice(ref string msg)
        {
            return FKY_WCFLibrary.PlatformServiceClient_Method.GetDeviceID(ref msg);
        }

        #endregion


        private void InitFaceSoloServer()
        {
            string url = (string)SysFunc.GetParamValue("FaceServerInterface");
            FKY_WCFLibrary.PlatformServiceClient_Method.InitPlatformServiceClient(url);
        }

        public void UploadLeaveInfoThread(object o)
        {
            Dictionary<string, string> visitLeaveInfo = o as Dictionary<string, string>;
            foreach (var item in visitLeaveInfo)
            {
                if (UploadLeaveInfo(item.Key))
                {
                    dal.doLeave(item.Key, item.Value);
                }
            }

        }

        public void UploadWGLeaveInfoThread(object o)
        {
            Dictionary<string, string> visitLeaveInfo = o as Dictionary<string, string>;
            foreach (var item in visitLeaveInfo)
            {
                if (UploadLeaveInfo(item.Key))
                {

                }
            }

        }

        public bool UploadLeaveInfo(string visitno)
        {
            string url = SysFunc.GetParamValue("FaceServerInterface").ToString();
            if (!string.IsNullOrEmpty(visitno))
            {
                try
                {
                    M_DeleteOneVisitor_Info m = new M_DeleteOneVisitor_Info();
                    m.requestCode = "DeleteOneVisitorCode";
                    m.request_facilityname = "访客机";
                    m.sqlvisitornovalue = visitno;

                    HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(url, Newtonsoft.Json.JsonConvert.SerializeObject(m), null, null, Encoding.UTF8, null);

                    string responseText;
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseText = reader.ReadToEnd().ToString();
                    }

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Dictionary<string, object> json = serializer.DeserializeObject(responseText) as Dictionary<string, object>;

                    string code = json["resultCode"].ToString();
                    if (code == "200")
                    {
                        LogNet.WriteLog("服务端", "上传签离记录成功[访客单号_" + visitno + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                        return true;

                    }
                    else
                    {
                        LogNet.WriteLog("服务端", "上传签离记录失败[访客单号_" + visitno + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    LogNet.WriteLogToLocal("服务端", "上传签离记录失败[访客单号_" + visitno + ex.ToString() + "]");//写入日志
                    return false;
                }
                finally
                {
                    GC.Collect();
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断二维码是否有效
        /// </summary>
        /// <param name="visitno"></param>
        /// <returns></returns>
        public bool CheckQRCode(string qrCode)
        {
            return dal.CheckQRCode(qrCode);
        }

        /// <summary>
        /// 凭身份证号码进入
        /// </summary>
        /// <param name="idCertno"></param>
        /// <param name="indoorName"></param>
        /// <returns></returns>
        public bool CheckInByIdCertno(string idCertno, string name, string indoorName, string sex, string imgCertPhotoBase64)
        {
            Model.M_VisitList_Info visit = new Model.M_VisitList_Info();
            visit.VisitNo = GetVisitNo();

            visit.CertKindName = "身份证";
            visit.VisitorName = name;
            visit.VisitorSex = sex;
            visit.CertNumber = idCertno;
            visit.InDoorName = indoorName;
            visit.VisitorFlag = 0;
            byte[] baseBytes = Convert.FromBase64String(imgCertPhotoBase64);
            visit.VisitorCertPhoto = baseBytes;
            visit.InTime = DateTime.Now;
            visit.Upload = 0;

            long ret = Add(visit);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            //return dal.CheckInByIdCertno(idCertno, name, indoorName, imgCertPhotoBase64);
        }

        public void ClearFace(string certno)
        {
            dal.ClearFace(certno);
        }

        public DataSet GetFaceIdList(string strWhere)
        {
            return dal.GetFaceIdList(strWhere);
        }

        /// <summary>
        /// 凭人脸进入
        /// </summary>
        /// <param name="idCertno"></param>
        /// <param name="indoorName"></param>
        /// <returns></returns>
        public bool CheckInByFace(int faceId, string indoorName, string strInTime)
        {
            return dal.CheckInByFace(faceId, indoorName, strInTime);
        }

        /// <summary>
        /// 凭人脸签离
        /// </summary>
        /// <param name="idCertno"></param>
        /// <param name="indoorName"></param>
        /// <returns></returns>
        public bool CheckOutByFace(uint faceId, string indoorName, string strOutTime)
        {
            return dal.CheckOutByFace(faceId, indoorName, strOutTime);
        }

        public Model.M_VisitList_Info GetModelByVisitNo(string visitno)
        {
            return dal.GetModelByVisitNo(visitno);
        }
        public DataSet GetDoorName(string strWhere)
        {
            return dal.GetDoorName(strWhere);
        }

        public void UpdatePoliceFlag(string visitno, int flag)
        {
            dal.UpdatePoliceFlag(visitno, flag);
        }

    }
}
