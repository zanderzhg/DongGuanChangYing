using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using TecsunVisitor.Controllers;
using TecsunVisitor.Models;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Threading;
using ADServer.Utils;
using ADServer.DAL;
using ADServer.BLL;
using System.Net;
using ADServer.Model;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ADServer.Interface
{
    public partial class VisitorInterface
    {
        public VisitorInterface()
        { }

        //<summary>
        //登录并获取授权令牌。
        //</summary>
        //<returns>登录成功返回 true，登录失败返回 false。</returns>
        protected static bool Authenticate(string token)
        {
            try
            {
                string key = ConfigurationManager.AppSettings["APIToken"].ToString(); //加解密密钥
                token = SysFunc.Decrypt(token, key);
                string date = token.Substring(0, 4);
                if (date != DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0'))
                {
                    return false;
                }
                else if (token.Substring(4) == key)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 验证账号id有效性
        /// </summary>
        /// <param name="token"></param>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string ValidateUser(string token, string username, string pwd)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                AccountController accountController = new AccountController();
                sys_User user = accountController.ValidateUser(username, pwd);
                if (user == null)
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "用户名密码验证失败", "");
                }
                else
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "验证有效", user.id.ToString());
                }
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }

        }

        /// <summary>
        /// 请求参数无效
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static string InvalidPostData(string errorMsg)
        {
            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "请求参数缺失", errorMsg);
        }

        /// <summary>
        /// 更新配置包
        /// </summary>
        /// <param name="token"></param>
        /// <param name="iUserID"></param>
        /// <param name="visionNo"></param>
        /// <returns></returns>
        public static string GetAccountConfigPack(string token, int iUserID, string visionNo)
        {
            if (!Authenticate(token))
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");

            ConfigPack c = new ConfigPack();
            c.UserId = iUserID;

            ParamController paramC = new ParamController();
            string mvisionNo = paramC.GetVisionNo(iUserID);
            if (mvisionNo != visionNo)
            {
                PersonController personC = new PersonController();
                c.InterviewedPersonList = personC.GetInterviewedPersonListByUserID(iUserID);
                c.Blacklist = personC.GetBlacklistByUserID(iUserID);
                c.Whitelist = personC.GetWhitelistByUserID(iUserID);
            }
            c.VersionNo = mvisionNo;
            JavaScriptSerializer js = new JavaScriptSerializer();

            string responseText = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "获取成功", js.Serialize(c));
            return JsonHelper.ReplaceJsonDateToDateString(responseText);
        }

        /// <summary>
        /// 上传一条来访记录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="iToPersonNo"></param>
        /// <param name="iUserID"></param>
        /// <param name="strVisitno"></param>
        /// <param name="strVisitorName"></param>
        /// <param name="strSex"></param>
        /// <param name="strVisitorAddress"></param>
        /// <param name="strCertKind"></param>
        /// <param name="strCertNo"></param>
        /// <param name="imgCertPhotoBase64"></param>
        /// <param name="strReason"></param>
        /// <param name="strTel"></param>
        /// <param name="strCompany"></param>
        /// <param name="strInDoorName"></param>
        /// <param name="strInTime"></param>
        /// <param name="imgInPhotoBase64"></param>
        /// <param name="strFaceResult"></param>
        /// <param name="strCarNumber"></param>
        /// <param name="strBelongings"></param>
        /// <param name="strOperterName"></param>
        /// <returns></returns>
        public static string PostAVisitorRecord(string token, int iToPersonNo, int iUserID, string strVisitno, string strVisitorName, string strSex, string strVisitorAddress, string strCertKind
            , string strCertNo, string imgCertPhotoBase64, string strReason, string strTel, string strCompany, string strInDoorName, string strInTime
            , string imgInPhotoBase64, string strFaceResult, string strCarNumber, string strBelongings, string strOperterName, string strMachineType)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");

            }

            try
            {
                AccountController accountController = new AccountController();
                sys_User user = accountController.GetUser(iUserID);
                if (user == null)
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "账号id不存在", "");
                }

                RecordController recordController = new RecordController();


                //照片存放的硬盘路径
                string photoPath = ConfigurationManager.AppSettings["PhotoPath"];
                string certPhotoPath = photoPath + "//CertPhoto";
                string catchPhotoPath = photoPath + "//CatchPhoto";

                if (!Directory.Exists(certPhotoPath))
                {
                    Directory.CreateDirectory(certPhotoPath);
                }
                if (!Directory.Exists(catchPhotoPath))
                {
                    Directory.CreateDirectory(catchPhotoPath);
                }

                if (imgCertPhotoBase64 != "")
                {
                    //imgCertPhotoBase64 = imgCertPhotoBase64.Replace("%2B", "+").Replace("%20", " ").Replace("%2F", "/").Replace("%3F", "?").Replace("%25", "%").Replace("%23", "#").Replace("%26", "&");

                    byte[] baseBytes = Convert.FromBase64String(imgCertPhotoBase64);
                    MemoryStream mStream = new MemoryStream();
                    mStream.Write(baseBytes, 0, baseBytes.Length);
                    mStream.Flush();
                    Bitmap img = new Bitmap(mStream);
                    img.Save(certPhotoPath + "//" + strVisitno + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                if (imgInPhotoBase64 != "")
                {
                    byte[] baseBytes = Convert.FromBase64String(imgInPhotoBase64);
                    MemoryStream mStream = new MemoryStream();
                    mStream.Write(baseBytes, 0, baseBytes.Length);
                    mStream.Flush();
                    Bitmap img = new Bitmap(mStream);
                    img.Save(catchPhotoPath + "//" + strVisitno + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                //strCertNo = desMethod.EncryptDES(strCertNo, "tecsun01");


                dt_VisitList visit = new dt_VisitList();
                visit.strVisitno = strVisitno;
                visit.strVisitorName = strVisitorName;
                visit.strSex = strSex;
                visit.strVisitorAddress = strVisitorAddress;
                visit.strCertKind = strCertKind;
                visit.strCertNo = strCertNo;
                visit.strReason = strReason;
                visit.strTel = strTel;
                visit.strCompany = strCompany;
                visit.strInDoorName = strInDoorName;

                visit.dtInTime = DateTime.Parse(strInTime);
                visit.strOperterName = strOperterName;
                visit.iToPersonNo = iToPersonNo;
                visit.strCarNumber = strCarNumber;
                visit.strBelongings = strBelongings;
                visit.strFaceResult = strFaceResult;
                visit.strMachineType = strMachineType;
                visit.iUserID = iUserID;

                long newId = recordController.AddARecord(visit);

                dt_Blacklist blacklist = new dt_Blacklist();
                blacklist.strCertNo = strCertNo;
                blacklist.strName = strVisitorName;
                blacklist.iUserID = iUserID;

                Thread thDealBlacklist = new Thread(DealBlacklist);
                thDealBlacklist.Start((object)blacklist);

                return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "上传成功", newId.ToString());
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 判断身份证号码是否为黑名单，并发送预警短信到安保人员手机
        /// </summary>
        private static void DealBlacklist(object oBlacklist)
        {
            dt_Blacklist blacklist = oBlacklist as dt_Blacklist;

            string msg = "";
            AccountController ac = new AccountController();
            sys_User user = ac.GetUser((int)blacklist.iUserID);
            if (user.iRight == 3)
            {
                msg = "用户[" + user.strUsername + "]";
            }
            else
            {
                msg = "管理员用户";
            }

            PersonController pc = new PersonController();

            if (pc.IsManageBlacklist(blacklist.strCertNo))
            {
                List<dt_SecurityPersonnel> bureauSpList = pc.GetManageReceiveTelList();
                List<dt_SecurityPersonnel> schoolSpList = pc.GetUserReceiveTelList((int)blacklist.iUserID);

                List<dt_SecurityPersonnel> spList = new List<dt_SecurityPersonnel>();
                spList.AddRange(bureauSpList);
                spList.AddRange(schoolSpList);

                foreach (dt_SecurityPersonnel sp in spList)
                {
                    dt_SmsList sms = new dt_SmsList();
                    sms.iUserID = (int)blacklist.iUserID;
                    sms.strContent = msg + "有黑名单人员[" + blacklist.strName + "]登记来访，请密切注意！-德生访客综合数据管理平台" + DateTime.Now.ToShortDateString();
                    sms.iToType = sp.iType;
                    sms.strToName = sp.strName;
                    sms.strToTel = sp.strTel;
                    sms.dtTime = DateTime.Now;
                    postSmsRecord(sms);
                }
            }
            if (pc.IsUserBlacklist(blacklist.strCertNo, (int)blacklist.iUserID))
            {
                List<dt_SecurityPersonnel> spList = pc.GetUserReceiveTelList((int)blacklist.iUserID);

                foreach (dt_SecurityPersonnel sp in spList)
                {
                    dt_SmsList sms = new dt_SmsList();
                    sms.iUserID = (int)blacklist.iUserID;
                    sms.strContent = msg + "有黑名单人员[" + blacklist.strName + "]登记来访，请密切注意！-德生访客综合数据管理平台" + DateTime.Now.ToShortDateString();
                    sms.iToType = sp.iType;
                    sms.strToName = sp.strName;
                    sms.strToTel = sp.strTel;
                    sms.dtTime = DateTime.Now;
                    postSmsRecord(sms);
                }
            }
        }

        private static bool postSmsRecord(dt_SmsList sms)
        {
            try
            {
                AccountController accountController = new AccountController();
                dt_SmsAccount smsAccount = accountController.GetSmsAccount();
                if (smsAccount != null)
                {
                    string resp = accountController.SendMsg(sms.strToTel, sms.strContent, smsAccount.strAccountName, smsAccount.strPwd, smsAccount.strSign);
                    if (resp == "")
                    {
                        RecordController recordController = new RecordController();
                        long newId = recordController.AddSmsRecord(sms);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 签离一条来访记录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="strVisitno"></param>
        /// <param name="strIdCertNumber"></param>
        /// <param name="strOutTime"></param>
        /// <param name="strOutDoorName"></param>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public static string SignOutAVisitorRecord(string token, string strVisitno, string strIdCertNumber, string strOutTime, string strOutDoorName, int iUserID)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                RecordController recordController = new RecordController();

                DateTime dtOutTime = DateTime.Parse(strOutTime);
                string ret = recordController.SignOutAVisitorRecord(strVisitno, strIdCertNumber, dtOutTime, strOutDoorName, iUserID);

                if (ret == "签离操作成功")
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, ret, "");
                }

                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, ret, "");
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 查询在指定日期上已签离的来访记录编号
        /// </summary>
        /// <param name="token"></param>
        /// <param name="strOutDate"></param>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public static string QueryLeavedVisitorRecord(string token, string strOutDate, int iUserID)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                if (strOutDate != null && strOutDate != "")
                {
                    DateTime dtOutDate = DateTime.Parse(strOutDate);

                    RecordController rc = new RecordController();
                    List<dt_VisitList> visitRecordList = new List<dt_VisitList>();
                    visitRecordList = rc.GetRecordsByOutDate(strOutDate, iUserID);
                    List<string> visitNoList = new List<string>();
                    foreach (dt_VisitList vi in visitRecordList)
                    {
                        if (vi.dtOutTime.Value.Date == dtOutDate.Date)
                            visitNoList.Add(vi.strVisitno);
                    }


                    JavaScriptSerializer js = new JavaScriptSerializer();
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "获取成功", js.Serialize(visitNoList));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 查询预约被访人的记录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="strIdCertNo"></param>
        /// <param name="strTel"></param>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public static string QueryBookRecords(string token, string strIdCertNo, string strTel, int iUserID)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            AccountController accountController = new AccountController();
            sys_User user = accountController.GetUser(iUserID);
            if (user == null)
            {
                return null;
            }

            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                BookingController bc = new BookingController();

                List<dt_Booking> bookingList = new List<dt_Booking>();

                if (strIdCertNo != "")
                {
                    bookingList = bc.GetBookingByIdCertNo(strIdCertNo, iUserID);
                    if (bookingList.Count > 0)
                    {

                        string responseText = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "获取成功", js.Serialize(bookingList));
                        responseText = JsonHelper.ReplaceJsonDateToDateString(responseText);

                        return responseText;
                    }
                }

                if (strTel != "")
                {
                    bookingList = bc.GetBookingByTel(strTel, iUserID);
                    if (bookingList.Count > 0)
                    {
                        string responseText = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "获取成功", js.Serialize(bookingList));
                        responseText = JsonHelper.ReplaceJsonDateToDateString(responseText);

                        return responseText;
                    }
                }

                return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "获取成功", js.Serialize(bookingList));
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 修改预约记录标识为已使用
        /// </summary>
        /// <param name="token"></param>
        /// <param name="bookRecordId"></param>
        /// <returns></returns>
        public static string UpdateBookRecordFlag(string token, int bookRecordId)
        {
            PostRet pr = new PostRet();

            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                BookingController bc = new BookingController();
                bc.UpdateRecordFlag(bookRecordId);

                return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "修改成功", "");
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 查询在访的记录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="strIdCertNo"></param>
        /// <param name="strTel"></param>
        /// <returns></returns>
        public static string QueryDuringVisitRecords(string token, string strIdCertNo, string strTel)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                RecordController rc = new RecordController();
                List<dt_VisitList> visitRecordList = new List<dt_VisitList>();
                if (strIdCertNo != null && strIdCertNo != "")
                {
                    visitRecordList = rc.GetRecordsByIdCertNo(strIdCertNo);
                }
                if (strTel != null && strTel != "")
                {
                    visitRecordList = rc.GetRecordsByTel(strTel);
                }
                //return visitRecordList.FindAll(delegate(dt_VisitList r) { return r.dtOutTime == null; });
                return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "获取成功", js.Serialize(visitRecordList.FindAll(delegate(dt_VisitList r) { return r.dtOutTime == null; })));
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 查询IC卡绑定的记录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="strICCard"></param>
        /// <returns></returns>
        public static string QueryVisitorInfoRecordsByICCard(string token, string strICCard)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                string resultJson = GetVisitorInfo(strICCard);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "获取成功", resultJson);
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }


        private static string GetVisitorInfo(string strICCard)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            BLL.B_VisitList_Info bll_visitList = new B_VisitList_Info();
            BLL.B_Card_Info bll_card_info = new B_Card_Info();
            string visitno = bll_card_info.GetVisitNoNowByCardNo(strICCard);
            System.Data.DataTable lastVisitInfo = bll_visitList.GetVisitedLastInfo(visitno, 4).Tables[0];
            object obj = null;

            if (lastVisitInfo.Rows.Count > 0)
            {
                #region 显示访客图片

                byte[] imageData = null;
                try
                {
                    imageData = (byte[])(lastVisitInfo.Rows[0]["证件头像"]);
                }
                catch
                {
                    imageData = new byte[1];
                }
                #endregion
                //显示访客信息
                obj = new
                {
                    visitorInfo = new
                    {
                        name = lastVisitInfo.Rows[0]["姓名"].ToString(),
                        sex = lastVisitInfo.Rows[0]["性别"].ToString(),
                        idcode = lastVisitInfo.Rows[0]["证件号码"].ToString(),
                        address = lastVisitInfo.Rows[0]["家庭住址"].ToString(),
                        certkind = lastVisitInfo.Rows[0]["证件类型"].ToString(),
                        visitorcount = lastVisitInfo.Rows[0]["来访人数"].ToString(),
                        visitBelong = lastVisitInfo.Rows[0]["来访事由"].ToString(),
                        visitReason = lastVisitInfo.Rows[0]["携带物品"].ToString(),
                        visitedTime = lastVisitInfo.Rows[0]["intime"].ToString(),
                        image = Convert.ToBase64String(imageData)
                    },
                    visitedInfo = new
                    {
                        visitedName = lastVisitInfo.Rows[0]["empname"].ToString(),
                        visitedSex = lastVisitInfo.Rows[0]["empsex"].ToString(),
                        visitedCompany = lastVisitInfo.Rows[0]["companyname"].ToString(),
                        visitedDept = lastVisitInfo.Rows[0]["deptname"].ToString(),
                        visitedPosition = lastVisitInfo.Rows[0]["empposition"].ToString(),
                        visitedRoomCode = lastVisitInfo.Rows[0]["emproomcode"].ToString(),
                        visitedTel = lastVisitInfo.Rows[0]["emptel"].ToString(),
                        visitedExtTel = lastVisitInfo.Rows[0]["empexttel"].ToString(),
                        visitedMobile = lastVisitInfo.Rows[0]["empmobile"].ToString(),
                        iEmployId = Convert.ToInt32(lastVisitInfo.Rows[0]["empno"].ToString())
                    }
                };
            }
            return js.Serialize(obj);
        }


        /// <summary>
        /// 隐患上报
        /// </summary>
        /// <param name="token"></param>
        /// <param name="iUserID"></param>
        /// <param name="strTitle"></param>
        /// <param name="strContent"></param>
        /// <param name="strOperterName"></param>
        /// <returns></returns>
        public static string PostHiddenDanger(string token, int iUserID, string strTitle, string strContent, string strOperterName)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                AccountController accountController = new AccountController();
                sys_User user = accountController.GetUser(iUserID);
                if (user == null)
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "账号id不存在", "");
                }

                dt_HiddenDanger hd = new dt_HiddenDanger();
                hd.iUserID = iUserID;
                hd.strTitle = strTitle;
                hd.strContent = strContent;
                hd.strOperterName = strOperterName;
                hd.dtAddTime = DateTime.Now;

                PersonController pc = new PersonController();
                List<dt_SecurityPersonnel> spList = pc.GetUserReceiveTelList(iUserID);

                foreach (dt_SecurityPersonnel sp in spList)
                {
                    dt_SmsList sms = new dt_SmsList();
                    sms.iUserID = iUserID;
                    sms.strContent = user.strUsername + "有隐患信息上报到平台，正文[" + strContent + "]，请密切注意！-德生访客综合数据管理平台" + DateTime.Now.ToShortDateString();
                    sms.iToType = sp.iType;
                    sms.strToName = sp.strName;
                    sms.strToTel = sp.strTel;
                    sms.dtTime = DateTime.Now;
                    postSmsRecord(sms);
                }

                RecordController rc = new RecordController();
                rc.AddHiddenDanger(hd);

                return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "成功", "");
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 上报门禁刷卡记录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="strUsername"></param>
        /// <param name="strCardId"></param>
        /// <param name="dtRecordTime"></param>
        /// <param name="strDoorName"></param>
        /// <param name="strEvent"></param>
        /// <param name="strVisitorName"></param>
        /// <param name="strEmpName"></param>
        /// <param name="iPersonType"></param>
        /// <returns></returns>
        public static string PostAccessDoorRecord(string token, string strUsername, string strCardId, string dtRecordTime, string strDoorName, string strEvent, string strVisitorName, string strEmpName, int iPersonType)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                AccountController accountController = new AccountController();
                sys_User user = accountController.GetUser(strUsername);
                if (user == null)
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "账号id不存在", "");
                }

                dt_AccessDoorList ad = new dt_AccessDoorList();
                ad.iUserID = user.id;
                ad.strCardId = strCardId;
                ad.dtRecordTime = DateTime.Parse(dtRecordTime);
                ad.strDoorName = strDoorName;
                ad.strEvent = strEvent;
                ad.strVisitorName = strVisitorName;
                ad.strEmpName = strEmpName;
                ad.iPersonType = iPersonType;

                RecordController rc = new RecordController();
                long newId = rc.AddAccessDoorRecord(ad);

                return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "上传成功", "");

            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 访客离开
        /// </summary>
        private static int VisitorLeave(M_FaceBarrier_Info m)
        {
            //string visitNo = bll_visitList.GetVisitNoByCertNo(certNo);   //查看是否有这个证件的未出访记录

            if (!string.IsNullOrEmpty(m.visitno))
            {
                BLL.B_VisitList_Info bll_visitList = new B_VisitList_Info();
                bll_visitList.doLeaveFace(m.visitno, m.recordtime.ToString(), m.devicename);
                return 0;
            }
            else
            {
                throw new Exception();
            }
        }



        /// <summary>
        /// 离开
        /// </summary>
        private static int EmployeeLeave(string empno)
        {
            BLL.B_FaceCompare_Info bll_FaceCompare = new B_FaceCompare_Info();
            string id = bll_FaceCompare.GetIdByEmpNo(empno);   //查看是否有这个证件的未出访记录
            if (!string.IsNullOrEmpty(id))
            {
                bll_FaceCompare.EmpdoLeave(empno);
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 人脸数据入库
        /// </summary>
        /// <param name="fbInfo"></param>
        /// <returns></returns>
        public static string PostFaceBarrierRecord(M_FaceBarrier_Info fbInfo)
        {
            if (!Authenticate(fbInfo.token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                B_FaceCompare_Info rc = new B_FaceCompare_Info();
                if (fbInfo.persontype == 0)
                {
                    try
                    {
                        B_Department_Info bll_department_Info = new B_Department_Info();
                        fbInfo.empno = fbInfo.outerID;
                        fbInfo.department = bll_department_Info.GetDeptNameByEmpNo(Convert.ToInt32(fbInfo.empno));
                    }
                    catch { }
                }
                if (fbInfo.compareresult != 1)
                {
                    fbInfo.matchimg = null;
                    fbInfo.persontype = 1;
                    fbInfo.visitorname = "未匹配";
                    fbInfo.certnumber = string.Empty; //身份证号码
                    fbInfo.outerID = string.Empty;//outerID
                    fbInfo.visitno = string.Empty; //访客单号
                    fbInfo.empno = "-1"; //员工ID
                }
                //B_PeopleStatus_Info ps = new B_PeopleStatus_Info();
                long newId = rc.Add(fbInfo);
                if (fbInfo.compareresult == 1)
                {
                    if (fbInfo.machinecode == "0")//出
                    {
                        if (fbInfo.persontype == 1)//人员类别，0为员工，1为访客
                        {
                            VisitorLeave(fbInfo);

                            LogNet.WriteLog("服务端", "刷脸离开记录[" + fbInfo.visitorname + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                        }
                        else
                        {
                            //EmployeeLeave(fbInfo.empno);
                            //ps.Delete(fbInfo.empno);
                            LogNet.WriteLog("服务端", "刷脸离开记录[" + fbInfo.visitorname + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                        }
                    }
                    else
                    {
                        //if (!string.IsNullOrEmpty(fbInfo.visitno) && fbInfo.persontype == 1)
                        //{
                        //    BLL.B_VisitList_Info bll_visitList = new B_VisitList_Info();
                        //    bll_visitList.ProcessNoAuthenticateFace(fbInfo.visitno);
                        //}//这代码有bug，屏了

                        LogNet.WriteLog("服务端", "刷脸进入记录[" + fbInfo.visitorname + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                    }
                }
                else
                {
                    LogNet.WriteLog("服务端", "刷脸记录compareresult为0,不处理[" + fbInfo.visitorname + "_" + fbInfo.outerID + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                }

                return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "上传成功", "");

            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        public static string DeleteAEmployeeRecord(string token, string EmployNo)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                B_Employ_Info rc = new B_Employ_Info();
                if (rc.Delete_pf(EmployNo))
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "上传成功", "");
                }
                else
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "操作失败,编号：" + EmployNo + "不存在", "");
                }
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }


        /// <summary>
        /// 发送心跳包
        /// </summary>
        /// <param name="token"></param>
        /// <param name="strUsername"></param>
        /// <param name="strMachineCode"></param>
        /// <param name="strIp"></param>
        /// <param name="strMachineType"></param>
        /// <returns></returns>
        public static string PostHeartBeat(string token, int iUserID, string strMachineCode, string strIp, string strLanIp, string strMachineType, string address)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                AccountController accountController = new AccountController();
                sys_User user = accountController.GetUser(iUserID);
                if (user == null)
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "账号id不存在", "");
                }

                dt_Machine machine = new dt_Machine();
                machine.iUserId = iUserID;
                machine.strMachineCode = strMachineCode;
                machine.strAddress = address;
                machine.strIp = strIp;
                machine.strLanIp = strLanIp;
                machine.strMachineType = strMachineType;
                machine.iStatus = 1;

                MachineController mc = new MachineController();
                mc.UpdateMachineRecord(machine);

                return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "发送成功", "");

            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 通过百度API查询当前IP地址的地理地址信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetLocation(string ip, string ak)
        {
            try
            {
                string url = "http://api.map.baidu.com/location/ip?ip=" + ip + "&ak=" + ak + "&coor=BD09ll";

                IDictionary<string, string> parameters = new Dictionary<string, string>();

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(url, parameters, null, null, Encoding.UTF8, null);
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseText = reader.ReadToEnd().ToString();
                    Dictionary<string, object> jsonToken = serializer.DeserializeObject(responseText) as Dictionary<string, object>;
                    Dictionary<string, object> dicData = jsonToken["content"] as Dictionary<string, object>;
                    string address = dicData["address"].ToString();

                    return address;
                }
            }
            catch
            {
                return "";
            }
        }

        public static string CheckInByFace(string token, string strFaceId, string strIndoorName, string strInTime)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                bool ret = new BLL.B_VisitList_Info().CheckInByFace(int.Parse(strFaceId), strIndoorName, strInTime);
                if (ret)
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "上传成功", "");
                }
                else
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "上传失败", "");
                }
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        public static string CheckOutByFace(string token, string strFaceId, string strOutdoorName, string strOutTime)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                bool ret = new BLL.B_VisitList_Info().CheckOutByFace(uint.Parse(strFaceId), strOutdoorName, strOutTime);
                if (ret)
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "上传成功", "");
                }
                else
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "签离失败", "");
                }
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        public static string CTIDSimpauth(string token, string strVisitorName, string strIdCertNo, string strHeadPhoto, string username, string pwd, ADServer.BLL.CTID.FKY_CTID fky_CTID, ADServer.BLL.FKY_AI.FaceHelper face)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                #region 1、验证账号是否可用

                AccountController accountController = new AccountController();
                ReturnMessage user = accountController.ValidateCTIDUser(username, pwd);

                if (user == null)
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "账号id不存在", "");
                }

                if (user.id == -1 || user.id == -2)
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, user.Message, "");
                }

                #endregion

                #region 2、查询是否第一次验证

                dt_Face_Info dfi = accountController.GetFaceInfo(strIdCertNo, strVisitorName);
                bool isFirstVerify = true;
                string CTIDMatchPhotoPath = string.Empty;//库的照片
                byte[] matchPhotobyte = null;
                byte[] nowPhotobyte = null;//现场POST过来的

                #region CTIDPhoto文件夹是否存在
                string photoPath = ConfigurationManager.AppSettings["PhotoPath"];
                string CTIDPhotoPath = photoPath + "//CTIDPhoto";

                if (!Directory.Exists(CTIDPhotoPath))
                {
                    Directory.CreateDirectory(CTIDPhotoPath);
                }
                #endregion


                if (dfi == null)//第一次
                {
                    isFirstVerify = true;
                    accountController.AddStatisticsCount(0, user.id);//调用计费
                }
                else
                {
                    #region 拿图片
                    try
                    {
                        isFirstVerify = false;
                        CTIDMatchPhotoPath = CTIDPhotoPath + "//" + dfi.facePhotoName + ".jpg";//库的照片
                        matchPhotobyte = SysFunc.GetImage(CTIDMatchPhotoPath);
                        accountController.AddStatisticsCount(1, user.id);//调用计费
                    }
                    catch (Exception ex)
                    {
                        isFirstVerify = true;
                        TecsunPlatform.Common.Logger.Write("身份认证服务请求异常：找不到库图片\r\n" + ex.ToString());
                        accountController.AddStatisticsCount(0, user.id);//调用计费
                    }
                    #endregion
                }
                #endregion


                #region 更新到人脸库模型
                string guidPhoto = Guid.NewGuid().ToString();//GUID

                dt_Face_Info CTIDFace = new dt_Face_Info();
                CTIDFace.idNumber = strIdCertNo;
                CTIDFace.type = string.Empty;//留空
                CTIDFace.facePhotoName = guidPhoto + "_" + DateTime.Now.Date.ToString("yyyy-MM-dd");
                CTIDFace.name = strVisitorName;
                #endregion


                #region 2.1计费
                if (isFirstVerify)
                {
                    #region 第一次验证，调用CTID
                    Newtonsoft.Json.Linq.JObject jsonSimpauth = fky_CTID.Simpauth(strIdCertNo, strVisitorName, strHeadPhoto);

                    if (jsonSimpauth != null)
                    {
                        //Console.WriteLine("第一次JSON" + jsonSimpauth.ToString());
                        int retCode = jsonSimpauth.Value<int>("retCode");
                        string retMessage = jsonSimpauth.Value<string>("retMessage");
                        string certToken = jsonSimpauth.Value<string>("certToken");
                        //string apiVersion = jsonSimpauth.Value<string>("apiVersion");
                        //string resStr = jsonSimpauth.Value<string>("resStr");
                        var o = new { };
                        string resjson = Newtonsoft.Json.JsonConvert.SerializeObject(o);
                        if (retCode == 0)//成功就写入数据库
                        {
                            if (strHeadPhoto != "")
                            {
                                byte[] baseBytes = Convert.FromBase64String(strHeadPhoto);
                                MemoryStream mStream = new MemoryStream();
                                mStream.Write(baseBytes, 0, baseBytes.Length);
                                mStream.Flush();
                                Bitmap img = new Bitmap(mStream);
                                img.Save(CTIDPhotoPath + "//" + CTIDFace.facePhotoName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                                accountController.AddFaceInfo(CTIDFace);
                            }

                            return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, retMessage, resjson);
                        }
                        else
                        {
                            TecsunPlatform.Common.Logger.Write("CTID_Simpauth:\r\n身份证信息认证失败！\r\n返回码：" + retCode + "\r\n详细信息：" + retMessage + "\r\n原始信息：\r\n" + jsonSimpauth.ToString());
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, retMessage, "");
                        }
                    }
                    else
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口AccessToken失败", "");
                    }
                    #endregion
                }
                else
                {
                    #region 多次使用CTID
                    double score = -1;
                    nowPhotobyte = Convert.FromBase64String(strHeadPhoto);//现场POST过来的

                    #region 防止超时
                    Func<byte[], byte[], double> timeoutMethod = delegate(byte[] b1, byte[] b2)
            {
                double s = -1;
                while (s == -1)//防止超过QPS
                {
                    Newtonsoft.Json.Linq.JObject json = face.FaceMatch(nowPhotobyte, matchPhotobyte);
                    switch (json.Value<int>("error_code"))
                    {
                        case 0:
                            {
                                try
                                {
                                    Newtonsoft.Json.Linq.JObject result = json.Value<Newtonsoft.Json.Linq.JObject>("result");
                                    if (result != null)
                                    {
                                        if (!string.IsNullOrEmpty(result.Value<string>("score")))
                                        {
                                            s = result.Value<double>("score");
                                        }
                                        else
                                        {
                                            s = -3;
                                        }
                                    }
                                    else
                                    {
                                        s = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    s = 0;
                                }
                            }
                            break;
                        case 18:
                            //Console.WriteLine("Open api qps request limit reached");
                            s = -1;
                            break;
                        default:
                            //Console.WriteLine(json.Value<string>("error_msg"));
                            s = 0;
                            break;
                    }
                }
                return s;
            };
                    #endregion

                    ADServer.BLL.CTID.TimeoutFunction.Execute(timeoutMethod, nowPhotobyte, matchPhotobyte, out score, TimeSpan.FromSeconds(60));
                    var o = new
                    {
                        score = score
                    };
                    string resjson = Newtonsoft.Json.JsonConvert.SerializeObject(o);
                    string path = CTIDPhotoPath + "//" + CTIDFace.facePhotoName + ".jpg";

                    //Console.WriteLine("认证分值:" + score);
                    if (score != -1 && score != -3)
                    {
                        if (score >= 80)
                        {
                            if (strHeadPhoto != "")
                            {
                                #region 获得上次写入的时间
                                try
                                {
                                    string[] facePhotoInfo = dfi.facePhotoName.Split('_');
                                    if (facePhotoInfo.Length > 1)
                                    {
                                        DateTime lastTime = DateTime.Parse(facePhotoInfo[1]);
                                        if (DateTime.Compare(DateTime.Now.Date, lastTime) > 0)
                                        {
                                            accountController.AddFaceInfo(CTIDFace);
                                            CTID_SimpauthPhoto(strHeadPhoto, path, CTIDMatchPhotoPath);
                                        }
                                    }
                                    else
                                    {
                                        accountController.AddFaceInfo(CTIDFace);
                                        CTID_SimpauthPhoto(strHeadPhoto, path, CTIDMatchPhotoPath);
                                    }
                                }
                                catch (Exception)
                                {
                                    accountController.AddFaceInfo(CTIDFace);
                                    CTID_SimpauthPhoto(strHeadPhoto, path, CTIDMatchPhotoPath);
                                }
                                #endregion
                            }
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "身份证信息认证通过", resjson);
                        }
                        else
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "身份认证结果不匹配", resjson);
                        }
                    }
                    else
                    {
                        if (score == -3)
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "不能使用相同的照片重复认证，请重新拍照！", resjson);
                        }
                        else
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口AccessToken失败", "");
                        }
                    }

                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                TecsunPlatform.Common.Logger.Write("身份认证服务请求异常：\r\n" + ex.ToString());
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }
        private static void CTID_SimpauthPhoto(string strHeadPhoto, string path, string CTIDMatchPhotoPath)
        {
            try
            {
                byte[] baseBytes = Convert.FromBase64String(strHeadPhoto);
                MemoryStream mStream = new MemoryStream();
                mStream.Write(baseBytes, 0, baseBytes.Length);
                mStream.Flush();
                Bitmap img = new Bitmap(mStream);
                img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);//CTIDPhotoPath + "//" + CTIDFace.facePhotoName + ".jpg"

                if (File.Exists(CTIDMatchPhotoPath))
                {
                    File.Delete(CTIDMatchPhotoPath);
                }
            }
            catch (Exception ex)
            {
                TecsunPlatform.Common.Logger.Write(ex.ToString());
            }
        }

        public static string GetVisitedInfo(string token, string visitNo)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                M_VisitList_Info model = new BLL.B_VisitList_Info().GetModelByVisitNo(visitNo);
                JavaScriptSerializer js = new JavaScriptSerializer();
                return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "发送成功", js.Serialize(model));
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        public static bool ValidateToken(string token)
        {
            return Authenticate(token);
        }

        public static bool VailatePlateToken(string token, string secret)
        {
            Match mc = SysFunc.MacDate(token);
            if (!string.IsNullOrEmpty(mc.Value))
            {
                if (mc.Value == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    string newSec = token.Substring(0, mc.Index);
                    if (newSec == secret)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

