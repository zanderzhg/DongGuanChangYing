using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADServer.Model;
using ADServer.BLL;
using ADServer.Utils;
using ADServer.DAL;
using System.Data;
using Newtonsoft.Json.Linq;

namespace ADServer.Interface
{
    public partial class VisitorInterface
    {
        static B_VisitList_Info bll_visitlist = new B_VisitList_Info();
        static B_White_Info bll_white = new B_White_Info();
        static B_Black_Info bll_black = new B_Black_Info();
        static B_WG_Record bll_record = new B_WG_Record();

        static B_Booking_Info bll_booking = new B_Booking_Info();
        static B_Employ_Info bll_employ = new B_Employ_Info();
        static BLL.B_Company_Info bll_company = new B_Company_Info();
        static BLL.B_Department_Info bll_deptment = new B_Department_Info();
        //<summary>
        //登录并获取授权令牌。
        //</summary>
        //<returns>登录成功返回 true，登录失败返回 false。</returns>
        protected static bool AuthenToken(string token)
        {
            try
            {
                string orgStr = token;// SysFunc.CardNoDecryptByDES(token);
                TokenInfo t = AuthTokenHelper.DecodeToken(orgStr);

                return true;
            }
            catch (Exception ex)
            {
                FKY_WCFLibrary.WriteLog.Log4Local("DataSrv Token 错误：\r\n" + ex.ToString(), true);
                return false;
            }
        }

        public static string ReceiveAppointment_API(string token, Newtonsoft.Json.Linq.JArray lstVisitPersonVO, int flag)
        {
            try
            {
                if (!AuthenToken(token))
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
                }

                List<object> responseList = new List<object>();
                foreach (Newtonsoft.Json.Linq.JToken record in lstVisitPersonVO)
                {
                    M_Booking_Info booking = new M_Booking_Info();

                    booking.id = record.Value<int>("vRecordId");
                    booking.CertKindName = GetCertKind(record.Value<string>("vCertKindName").Trim());
                    booking.CertNumber = record.Value<string>("vCertNo").Trim();
                    booking.BookName = record.Value<string>("vName").Trim();
                    booking.BookTel = record.Value<string>("vPhone").Trim();
                    booking.VisitorCompany = record.Value<string>("vCompany");
                    booking.QRCode = record.Value<string>("vQRCode");
                    booking.BookReason = record.Value<string>("vReason");
                    booking.BookSex = record.Value<string>("vSex");

                    booking.BookBelongs = record.Value<string>("vBelongs");
                    booking.BookMenu = record.Value<string>("vMenu");//备注

                    if (booking.CertNumber.Length == 18)
                    {
                        string sex = booking.CertNumber.Substring(14, 3);
                        if (int.Parse(sex) % 2 == 0)//性别代码为偶数是女性奇数为男性
                        {
                            booking.BookSex = "女";
                        }
                        else
                        {
                            booking.BookSex = "男";
                        }
                    }

                    booking.Emptel = record.Value<string>("vEmpPhone");
                    booking.EmpNo = record.Value<int>("vEmpNo");
                    if (booking.EmpNo > 0)
                    {
                        M_Employ_Info orgEmpModel = bll_employ.GetModel(booking.EmpNo.Value);
                        if (orgEmpModel != null)
                        {
                            if (string.IsNullOrEmpty(booking.Emptel))
                            {
                                booking.Emptel = orgEmpModel.EmpMobile;
                            }
                            booking.Empname = orgEmpModel.EmpName;
                        }
                        else
                        {
                            #region 查找员工
                            if (!string.IsNullOrEmpty(booking.Emptel))
                            {
                                DataSet empdata = bll_employ.GetListWhereTel(booking.Emptel);
                                if (empdata.Tables[0].Rows.Count > 0)
                                {
                                    booking.EmpNo = Convert.ToInt32(empdata.Tables[0].Rows[0]["EmpNo"]);
                                    booking.Empname = empdata.Tables[0].Rows[0]["EmpName"] != null ? empdata.Tables[0].Rows[0]["EmpName"].ToString() : "";
                                }
                            }
                            else
                            {
                                booking.EmpNo = -1;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region 查找员工
                        if (!string.IsNullOrEmpty(booking.Emptel))
                        {
                            DataSet empdata = bll_employ.GetListWhereTel(booking.Emptel);
                            if (empdata.Tables[0].Rows.Count > 0)
                            {
                                booking.EmpNo = Convert.ToInt32(empdata.Tables[0].Rows[0]["EmpNo"]);
                                booking.Empname = empdata.Tables[0].Rows[0]["EmpName"] != null ? empdata.Tables[0].Rows[0]["EmpName"].ToString() : "";
                            }
                        }
                        else
                        {
                            booking.EmpNo = -1;
                        }
                        #endregion
                    }

                    //booking.BookTime = record.Value<DateTime>("vApplyTime");
                    booking.BookTime = DateTime.Now;
                    if (record["vStartTime"] != null)
                    {
                        booking.BookTimeStart = record.Value<DateTime>("vStartTime");
                    }
                    else
                    {
                        booking.BookTimeStart = DateTime.Now;
                        booking.BookDate = DateTime.Now;
                    }
                    if (record["vEndTime"] != null)
                        booking.ValidTimeEnd = record.Value<DateTime>("vEndTime");
                    else
                        booking.ValidTimeEnd = DateTime.Now.AddDays(+1).Date;
                    booking.LicensePlate = record.Value<string>("vLicensePlate");

                    if (!string.IsNullOrEmpty(booking.LicensePlate))
                    {
                        booking.LicensePlate = booking.LicensePlate.Trim().ToUpper();

                        if (booking.LicensePlate.Length > 8)
                        {
                            booking.LicensePlate = booking.LicensePlate.Substring(0, 8);
                        }
                    }

                    booking.BookNum = booking.VisitNum = record.Value<int>("vVisitorNum");
                    booking.BookFlag = 0;
                    booking.BookNo = record.Value<string>("vBookNo");

                    #region Switch处理
                    switch (flag)//1增 ,2 删，3 改
                    {
                        case 1:
                            {
                                //if (booking.ValidTimeStart == null || booking.ValidTimeEnd == null)
                                //{
                                //    LogNet.WriteLog("服务端", "预约记录[" + booking.BookName + "]有误,起始或结束时间不能为空", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                //    object o = new
                                //    {
                                //        resultCode = false,
                                //        visitorObj = record,
                                //        message = "起始或结束时间不能为空",
                                //    };
                                //    responseList.Add(o);
                                //    continue;
                                //}
                                if (string.IsNullOrEmpty(booking.BookTel))
                                {
                                    LogNet.WriteLog("服务端", "预约记录[" + booking.BookName + "]有误,来访电话不能为空", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                    object o = new
                                    {
                                        resultCode = false,
                                        visitorObj = record,
                                        message = "来访电话不能为空",
                                    };
                                    responseList.Add(o);
                                    continue;
                                }
                                else
                                {
                                    //生成预约记录
                                    string bookno = bll_booking.GetBookNo();    //生成预约号
                                    booking.BookDate = ((DateTime)booking.BookTimeStart).Date;
                                    booking.ValidTimeEnd = booking.ValidTimeEnd.Value;

                                    booking.BookNo = bookno;
                                    booking.BookNum = booking.VisitNum == 0 ? 1 : booking.VisitNum;
                                    booking.BookFlag = 0;

                                    bll_booking.Add_API(booking);
                                    LogNet.WriteLog("服务端", "预约记录有效[" + booking.BookName + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                                    object o = new
                                    {
                                        resultCode = true,
                                        vRecordId = booking.id,
                                        vBookNo = bookno,
                                        message = "新增成功"
                                    };
                                    responseList.Add(o);
                                }
                            }
                            break;
                        case 2:
                            {
                                string msg = "撤销成功";

                                try
                                {
                                    if (!string.IsNullOrEmpty(booking.BookNo))
                                    {
                                        int r = bll_booking.Delete_API(booking.BookNo);
                                    }
                                    else
                                    {
                                        msg = "撤销失败,vBookNo为空";
                                        LogNet.WriteLog("服务端", "撤销失败,vBookNo为空", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                    }
                                }
                                catch { }

                                object o = new
                                {
                                    resultCode = true,
                                    visitorObj = record,
                                    message = msg
                                };
                                responseList.Add(o);
                            }
                            break;
                        case 3:
                            {
                                try
                                {
                                    booking.BookDate = ((DateTime)booking.ValidTimeStart).Date;
                                    booking.ValidTimeEnd = (DateTime)booking.ValidTimeEnd;
                                    booking.BookNum = booking.VisitNum;
                                    booking.BookFlag = 0;
                                    bll_booking.Update_API(booking);
                                }
                                catch { }

                                LogNet.WriteLog("服务端", "预约记录修改成功[" + booking.BookName + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                object o = new
                                {
                                    resultCode = true,
                                    visitorObj = record,
                                    message = "修改成功"
                                };
                                responseList.Add(o);
                            }
                            break;
                        default:
                            { }
                            break;
                    }
                    #endregion

                }
                var ret = new
                {
                    code = ApiTools.ResponseCode.成功,
                    message = "推送成功",
                    data = responseList
                };
                string returnJson = Newtonsoft.Json.JsonConvert.SerializeObject(ret);
                return returnJson;// ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "推送成功", returnJson);
            }
            catch (Exception ex)
            {
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
        }

        private static string GetCertKind(string num)
        {
            string certKind = string.Empty;//访客证件类型（1 身份证 2护照 3驾照 ）
            switch (num)
            {
                case "1":
                    certKind = "身份证";
                    break;
                case "2":
                    certKind = "护照";
                    break;
                case "3":
                    certKind = "驾照";
                    break;
                case "4":
                    certKind = "港澳回乡证";
                    break;
                case "5":
                    certKind = "台胞证";
                    break;
                default:
                    certKind = "其他";
                    break;
            }
            return certKind;
        }

        public static string GetBookingInfo_API(string token, JObject json)
        {
            try
            {
                if (!AuthenToken(token))
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
                }
                string strWhere = VisitorInterface.GetBookingWhere(json);

                int pageIndex = json.Value<int>("pageIndex");
                int lines = json.Value<int>("lines");

                if (pageIndex <= 0)
                {
                    pageIndex = 1;
                }
                if (lines <= 0 || lines > 100)
                {
                    lines = 100;
                }

                DataTable dt = bll_booking.GetBookingInfo_API(strWhere, pageIndex, lines).Tables[0];

                var ret = new
                {
                    code = ApiTools.ResponseCode.成功,
                    message = "成功",
                    data = dt
                };
                string returnJson = Newtonsoft.Json.JsonConvert.SerializeObject(ret);

                return returnJson;

            }
            catch (Exception ex)
            {
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
        }

        public static string ReceiveWhite_API(string token, Newtonsoft.Json.Linq.JArray whiteList)
        {
            try
            {
                if (!AuthenToken(token))
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
                }

                List<object> responseList = new List<object>();
                foreach (Newtonsoft.Json.Linq.JToken white in whiteList)
                {
                    Model.M_White_Info model = new M_White_Info();
                    int id = white.Value<int>("wId");
                    model.CertType = GetCertKind(white.Value<string>("wCertType"));
                    model.CertNo = white.Value<string>("wCertNo");
                    model.Name = white.Value<string>("wName");                //姓名
                    model.Sex = white.Value<string>("wSex");
                    model.VisitCompany = white.Value<string>("wCompany");  //所在公司
                    model.Phone = white.Value<string>("wPhone");           //手机号码
                    model.OperateName = "第三方同步";

                    if (string.IsNullOrEmpty(model.CertType) || string.IsNullOrEmpty(model.CertNo) || string.IsNullOrEmpty(model.Name))
                    {
                        object o = new
                        {
                            resultCode = false,
                            whiteObj = white,
                            message = "缺少必填项"
                        };
                        responseList.Add(o);
                        continue;
                    }

                    string status = !string.IsNullOrEmpty(white.Value<string>("wStatus")) ? "" : white.Value<string>("wStatus");

                    switch (status)//1 正常 2 删除
                    {
                        case "2":
                            bll_white.Delete(model.CertNo);
                            var de = new
                            {
                                resultCode = true,
                                whiteObj = white,
                                message = "删除成功"
                            };
                            responseList.Add(de);
                            break;
                        default:
                            if (!bll_white.Exists(model.CertType, model.CertNo))
                            {
                                bll_white.Add(model);

                                object o = new
                                {
                                    resultCode = true,
                                    whiteObj = white,
                                    message = "新增成功"
                                };
                                responseList.Add(o);

                            }
                            else
                            {
                                bll_white.Update(model);

                                var o = new
                                {
                                    resultCode = true,
                                    whiteObj = white,
                                    message = "修改成功"
                                };
                                responseList.Add(o);
                            }
                            break;
                    }
                }
                var ret = new
                {
                    code = ApiTools.ResponseCode.成功,
                    message = "推送成功",
                    data = responseList
                };
                string returnJson = Newtonsoft.Json.JsonConvert.SerializeObject(ret);
                return returnJson;
            }
            catch (Exception ex)
            {
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
        }

        public static string ReceiveBlack_API(string token, Newtonsoft.Json.Linq.JArray blackList)
        {
            try
            {
                if (!AuthenToken(token))
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
                }

                List<object> responseList = new List<object>();
                foreach (Newtonsoft.Json.Linq.JToken black in blackList)
                {
                    Model.M_Black_Info model = new M_Black_Info();
                    int id = black.Value<int>("bId");
                    model.certkindname = GetCertKind(black.Value<string>("bCertType"));
                    model.certkindno = black.Value<string>("bCertNo");
                    model.name = black.Value<string>("bName");
                    model.sex = black.Value<string>("bSex");
                    model.blackreason = black.Value<string>("bReason");
                    model.entrydate = DateTime.Now.ToLocalTime();
                    model.opertername = "第三方同步";

                    if (string.IsNullOrEmpty(model.certkindno) || string.IsNullOrEmpty(model.certkindname) || string.IsNullOrEmpty(model.name))
                    {
                        object o = new
                        {
                            resultCode = false,
                            blackObj = black,
                            message = "缺少必填项"
                        };
                        responseList.Add(o);
                        break;
                    }
                    string status = string.IsNullOrEmpty(black.Value<string>("bStatus")) ? "" : black.Value<string>("bStatus");
                    switch (status)//1 正常 2 删除
                    {
                        case "2":
                            bll_black.Delete(model.certkindname, model.certkindno);
                            var de = new
                            {
                                resultCode = true,
                                blackObj = black,
                                message = "删除成功"
                            };
                            responseList.Add(de);
                            break;
                        default:
                            if (!bll_black.Exists(model.certkindname, model.certkindno))
                            {
                                bll_black.Add(model);

                                object o = new
                                {
                                    resultCode = true,
                                    blackObj = black,
                                    message = "新增成功"
                                };
                                responseList.Add(o);

                            }
                            else
                            {
                                bll_black.Update(model);

                                var o = new
                                {
                                    resultCode = true,
                                    blackObj = black,
                                    message = "修改成功"
                                };
                                responseList.Add(o);
                            }
                            break;
                    }
                }
                var ret = new
                {
                    code = ApiTools.ResponseCode.成功,
                    message = "推送成功",
                    data = responseList
                };
                string returnJson = Newtonsoft.Json.JsonConvert.SerializeObject(ret);
                return returnJson;
            }
            catch (Exception ex)
            {
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
        }

        public static string ReceiveEmploy_API(string token, Newtonsoft.Json.Linq.JArray empList)
        {
            try
            {
                if (!AuthenToken(token))
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
                }

                List<object> responseList = new List<object>();
                foreach (Newtonsoft.Json.Linq.JToken emp in empList)
                {
                    int id = emp.Value<int>("eId");
                    string strName = emp.Value<string>("eName").Trim();               //姓名
                    string strPhone = emp.Value<string>("ePhone").Trim();             //手机号码
                    string strOfficePhone = emp.Value<string>("eOfficePhone");        //办公电话
                    string strExtPhone = emp.Value<string>("eExtPhone");              //分机号码
                    string strRoomNumber = emp.Value<string>("eRoomNumber");          //房间号
                    //string strIdCard = emp.Value<string>("eIdCard");                  //身份证号码
                    string strIcCard = emp.Value<string>("eIcCard");                  //IC卡号
                    string strDepartment = emp.Value<string>("eDepartment").Trim();          //所在部门
                    string strCompany = emp.Value<string>("eCompany").Trim();               //所在公司
                    string sex = emp.Value<string>("eSex");                           //女，男
                    string licensePlate = emp.Value<string>("eLicensePlate");
                    string eEmpNum = emp.Value<string>("eEmpNum");
                    string EmpPosition = emp.Value<string>("ePosition");
                    string EmpMemu = emp.Value<string>("eMemu");
                    string status = emp.Value<string>("eStatus");
                    string EmpFloor = emp.Value<string>("eFloor");

                    if (!string.IsNullOrEmpty(status) && status == "1") //该员工删除
                    {
                        M_Employ_Info deleteEmp = bll_employ.GetModel_API(strPhone);

                        if (deleteEmp != null)
                        {
                            bll_employ.Delete_API(strPhone);
                            object o = new
                            {
                                resultCode = true,
                                eId = id,
                                message = "删除成功"
                            };
                            responseList.Add(o);
                        }
                    }
                    else
                    {
                        #region 新增单位部门
                        M_Company_Info curCompany = null;
                        M_Department_Info curDeptment = null;
                        if (!bll_company.Exists_wx(strCompany)) //判断所在公司是否存在
                        {
                            M_Company_Info newCompany = new M_Company_Info();
                            newCompany.CompanyName = strCompany;
                            bll_company.Add_wx(newCompany);
                        }
                        curCompany = bll_company.GetModel(strCompany);

                        if (!bll_deptment.Exists_wx(strDepartment, strCompany)) //判断所在部门是否存在
                        {
                            M_Department_Info newDept = new M_Department_Info();
                            newDept.CompanyId = curCompany.CompanyId;
                            newDept.DeptName = strDepartment;

                            bll_deptment.Add_wx(newDept);
                        }
                        curDeptment = bll_deptment.GetModel(strDepartment, strCompany);
                        #endregion

                        if (!bll_employ.ExistEmpPhone_API(strPhone))
                        {
                            #region 检测字段合法性
                            #region Iccard
                            if (!string.IsNullOrEmpty(strIcCard))
                            {
                                if (SysFunc.IsDangerSqlString(strIcCard))
                                {
                                    object oIccard = new
                                    {
                                        resultCode = false,
                                        eId = id,
                                        message = "IC卡号非法[" + strDepartment + "|" + strName + "|" + strIcCard + "]！"
                                    };
                                    responseList.Add(oIccard);
                                    continue;
                                }

                                if (bll_employ.ExistICCardno(strIcCard))
                                {
                                    object oEIccard = new
                                    {
                                        resultCode = false,
                                        eId = id,
                                        message = "已存在重复的IC卡号[" + strDepartment + "|" + strName + "|" + strIcCard + "]！"
                                    };
                                    responseList.Add(oEIccard);
                                    continue;
                                }
                            } 
                            #endregion
                            //工号
                            if (!string.IsNullOrEmpty(eEmpNum))
                            {
                                if (bll_employ.ExistNum(eEmpNum))
                                {
                                    object oEmpNum = new
                                    {
                                        resultCode = false,
                                        eId = id,
                                        message = "已存在重复的工号[" + strDepartment + "|" + strName + "|" + eEmpNum + "]！"
                                    };
                                    responseList.Add(oEmpNum);
                                    continue;
                                }
                            }
                            #endregion

                            //新增员工
                            M_Employ_Info newEmploy = new M_Employ_Info();
                            newEmploy.CompanyId = curCompany.CompanyId;
                            newEmploy.DeptNo = curDeptment.DeptNo;
                            newEmploy.EmpName = strName;
                            newEmploy.EmpMobile = strPhone;
                            newEmploy.EmpTel = strOfficePhone;
                            newEmploy.EmpExtTel = strExtPhone;
                            newEmploy.EmpRoomCode = strRoomNumber;
                            newEmploy.EmpCardno = strIcCard;
                            newEmploy.EmpSex = sex;

                            newEmploy.LicensePlate = licensePlate;
                            newEmploy.EmpNum = eEmpNum;
                            newEmploy.EmpPosition = EmpPosition;
                            newEmploy.EmpMemu = EmpMemu;

                            newEmploy.EmpFloor = EmpFloor;
                            //newEmploy.WeixinId = id;

                            bll_employ.Add_wx(newEmploy);//新增不需要修改，可直接使用wx

                            object o = new
                            {
                                resultCode = true,
                                eId = id,
                                //ePhone = strPhone,
                                message = "新增成功"
                            };
                            responseList.Add(o);
                        }
                        else
                        {
                            //更新员工
                            M_Employ_Info curEmploy = bll_employ.GetModel_API(strPhone);
                            curEmploy.CompanyId = curCompany.CompanyId;
                            curEmploy.DeptNo = curDeptment.DeptNo;
                            curEmploy.EmpName = strName;
                            curEmploy.EmpMobile = strPhone;
                            curEmploy.EmpTel = strOfficePhone;
                            curEmploy.EmpExtTel = strExtPhone;
                            curEmploy.EmpRoomCode = strRoomNumber;
                            curEmploy.EmpCardno = strIcCard;
                            curEmploy.EmpSex = sex;

                            curEmploy.LicensePlate = licensePlate;
                            curEmploy.EmpNum = eEmpNum;

                            curEmploy.EmpPosition = EmpPosition;
                            curEmploy.EmpMemu = EmpMemu;
                            curEmploy.EmpFloor = EmpFloor;

                            //curEmploy.WeixinId = id;

                            bll_employ.Update_API(curEmploy);
                            object o = new
                            {
                                resultCode = true,
                                eId = id,
                                //ePhone = strPhone,
                                message = "修改成功"
                            };
                            responseList.Add(o);
                        }
                    }
                }
                var ret = new
                {
                    code = ApiTools.ResponseCode.成功,
                    message = "推送成功",
                    data = responseList
                };
                string returnJson = Newtonsoft.Json.JsonConvert.SerializeObject(ret);
                return returnJson;
            }
            catch (Exception ex)
            {
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
        }

        public static string GetEmployInfo_API(string token, JObject json)
        {
            try
            {
                if (!AuthenToken(token))
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
                }
                string strWhere = VisitorInterface.GetEmployWhere(json);

                int pageIndex = json.Value<int>("pageIndex");
                int lines = json.Value<int>("lines");

                if (pageIndex <= 0)
                {
                    pageIndex = 1;
                }
                if (lines <= 0 || lines > 100)
                {
                    lines = 100;
                }

                DataTable dt = bll_employ.GetList_API(strWhere, pageIndex, lines).Tables[0];

                var ret = new
                {
                    code = ApiTools.ResponseCode.成功,
                    message = "成功",
                    data = dt
                };
                string returnJson = Newtonsoft.Json.JsonConvert.SerializeObject(ret);

                return returnJson;

            }
            catch (Exception ex)
            {
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
        }

        public static string GetWhiteInfo_API(string token, JObject json)
        {
            try
            {
                if (!AuthenToken(token))
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
                }
                string strWhere = VisitorInterface.GetWhiteWhere(json);

                int pageIndex = json.Value<int>("pageIndex");
                int lines = json.Value<int>("lines");

                if (pageIndex <= 0)
                {
                    pageIndex = 1;
                }
                if (lines <= 0 || lines > 100)
                {
                    lines = 100;
                }

                DataTable dt = bll_white.GetList_API(strWhere, pageIndex, lines).Tables[0];

                var ret = new
                {
                    code = ApiTools.ResponseCode.成功,
                    message = "成功",
                    data = dt
                };
                string returnJson = Newtonsoft.Json.JsonConvert.SerializeObject(ret);

                return returnJson;

            }
            catch (Exception ex)
            {
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
        }

        public static string GetBlackInfo_API(string token, JObject json)
        {
            try
            {
                if (!AuthenToken(token))
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
                }
                string strWhere = VisitorInterface.GetBlackWhere(json);

                int pageIndex = json.Value<int>("pageIndex");
                int lines = json.Value<int>("lines");

                if (pageIndex <= 0)
                {
                    pageIndex = 1;
                }
                if (lines <= 0 || lines > 100)
                {
                    lines = 100;
                }

                DataTable dt = bll_black.GetList_API(strWhere, pageIndex, lines).Tables[0];

                var ret = new
                {
                    code = ApiTools.ResponseCode.成功,
                    message = "成功",
                    data = dt
                };
                string returnJson = Newtonsoft.Json.JsonConvert.SerializeObject(ret);

                return returnJson;

            }
            catch (Exception ex)
            {
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
        }

        public static string GetVisitorInfo_API(string token, JObject json)
        {
            try
            {
                if (!AuthenToken(token))
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
                }
                string strWhere = VisitorInterface.GetVisitorWhere(json);

                int pageIndex = json.Value<int>("pageIndex");
                int lines = json.Value<int>("lines");

                if (pageIndex <= 0)
                {
                    pageIndex = 1;
                }
                if (lines <= 0 || lines > 100)
                {
                    lines = 100;
                }

                DataTable dt = bll_visitlist.QueryVisitList_API(strWhere, pageIndex, lines).Tables[0];

                var ret = new
                {
                    code = ApiTools.ResponseCode.成功,
                    message = "成功",
                    data = dt
                };
                string returnJson = Newtonsoft.Json.JsonConvert.SerializeObject(ret);

                return returnJson;

            }
            catch (Exception ex)
            {
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
        }

        public static string GetVisitorPhotoInfo_API(string token, string visitno)
        {
            try
            {
                if (!AuthenToken(token))
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
                }

                M_VisitList_Info photo = bll_visitlist.GetPhoto(visitno);
                if (photo == null)
                {
                    var retphoto = new
                     {
                         code = ApiTools.ResponseCode.成功,
                         message = "无法查询此单号信息",
                         data = new { }
                     };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(retphoto);
                }

                var ret = new
                {
                    code = ApiTools.ResponseCode.成功,
                    message = "成功",
                    data = new
                    {
                        VisitorPhoto = Convert.ToBase64String(photo.VisitorPhoto),
                        VisitorCertPhoto = Convert.ToBase64String(photo.VisitorCertPhoto),
                        QrImage = Convert.ToBase64String(photo.QrImage)
                    }
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ret);

            }
            catch (Exception ex)
            {
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
        }

        public static string GetAcInfo_API(string token, JObject json)
        {
            try
            {
                if (!AuthenToken(token))
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
                }
                string strWhere = VisitorInterface.GetAcWhere(json);

                int pageIndex = json.Value<int>("pageIndex");
                int lines = json.Value<int>("lines");

                if (pageIndex <= 0)
                {
                    pageIndex = 1;
                }
                if (lines <= 0 || lines > 100)
                {
                    lines = 100;
                }

                DataTable dt = bll_record.GetList_API(strWhere, pageIndex, lines).Tables[0];

                var ret = new
                {
                    code = ApiTools.ResponseCode.成功,
                    message = "成功",
                    data = dt
                };
                string returnJson = Newtonsoft.Json.JsonConvert.SerializeObject(ret);

                return returnJson;

            }
            catch (Exception ex)
            {
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
        }

        #region 查询条件解析
        public static string GetBookingWhere(JObject json)
        {
            string strWhere = string.Empty;
            string bookno = string.Empty;
            string bookname = string.Empty;
            string booktel = string.Empty;
            string bookdate = string.Empty;

            bookno = json.Value<string>("vBookNo");//bookno
            bookname = json.Value<string>("vName");
            booktel = json.Value<string>("vPhone");
            bookdate = json.Value<string>("vBookDate");

            if (!string.IsNullOrEmpty(bookno) && !SysFunc.IsDangerSqlString(bookno))
            {
                strWhere += " and a.bookno like '%" + bookno + "%'";
            }
            if (!string.IsNullOrEmpty(bookname) && !SysFunc.IsDangerSqlString(bookname))
            {
                strWhere += " and a.bookname like '%" + bookname + "%'";
            }
            if (!string.IsNullOrEmpty(booktel) && !SysFunc.IsDangerSqlString(booktel))
            {
                strWhere += " and a.booktel like '%" + booktel + "%'";
            }
            if (!string.IsNullOrEmpty(bookdate) && !SysFunc.IsDangerSqlString(bookdate))
            {
                strWhere += " and a.bookdate = '" + bookdate + "'";
            }
            return strWhere;
        }

        public static string GetEmployWhere(JObject json)
        {
            string strWhere = string.Empty;
            string EmpMobile = string.Empty;
            string EmpName = string.Empty;
            string DeptName = string.Empty;
            string CompanyName = string.Empty;
            string EmpNum = string.Empty;

            EmpMobile = json.Value<string>("ePhone");//EmpMobile
            EmpName = json.Value<string>("eName");
            DeptName = json.Value<string>("eDepartment");
            CompanyName = json.Value<string>("eCompany");
            EmpNum = json.Value<string>("eEmpNum");

            if (!string.IsNullOrEmpty(EmpMobile) && !SysFunc.IsDangerSqlString(EmpMobile))
            {
                strWhere += " and c.EmpMobile like '%" + EmpMobile + "%'";
            }
            if (!string.IsNullOrEmpty(EmpNum) && !SysFunc.IsDangerSqlString(EmpNum))
            {
                strWhere += " and c.EmpNum like '%" + EmpNum + "%'";
            }
            if (!string.IsNullOrEmpty(EmpName) && !SysFunc.IsDangerSqlString(EmpName))
            {
                strWhere += " and c.EmpName like '%" + EmpName + "%'";
            }
            if (!string.IsNullOrEmpty(DeptName) && !SysFunc.IsDangerSqlString(DeptName))
            {
                strWhere += " and b.DeptName like '%" + DeptName + "%'";
            }
            if (!string.IsNullOrEmpty(CompanyName) && !SysFunc.IsDangerSqlString(CompanyName))
            {
                strWhere += " and a.CompanyName = '" + CompanyName + "'";
            }
            return strWhere;
        }

        public static string GetWhiteWhere(JObject json)
        {
            string strWhere = string.Empty;
            string phone = string.Empty;
            string name = string.Empty;
            string certNo = string.Empty;
            string VisitCompany = string.Empty;

            phone = json.Value<string>("wPhone");
            name = json.Value<string>("wName");
            certNo = json.Value<string>("wCertNo");
            VisitCompany = json.Value<string>("wCompany");

            if (!string.IsNullOrEmpty(phone) && !SysFunc.IsDangerSqlString(phone))
            {
                strWhere += " and phone = '" + phone + "'";
            }
            if (!string.IsNullOrEmpty(name) && !SysFunc.IsDangerSqlString(name))
            {
                strWhere += " and name = '" + name + "'";
            }
            if (!string.IsNullOrEmpty(certNo) && !SysFunc.IsDangerSqlString(certNo))
            {
                strWhere += " and certNo = '" + certNo + "'";
            }
            if (!string.IsNullOrEmpty(VisitCompany) && !SysFunc.IsDangerSqlString(VisitCompany))
            {
                strWhere += " and VisitCompany = '" + VisitCompany + "'";
            }
            return strWhere;
        }

        public static string GetBlackWhere(JObject json)
        {
            string strWhere = string.Empty;
            string name = string.Empty;
            string certkindno = string.Empty;
            //string certkindname = string.Empty;

            name = json.Value<string>("bName");
            certkindno = json.Value<string>("bCertNo");
            //certkindname = json.Value<string>("bCertType");

            if (!string.IsNullOrEmpty(name) && !SysFunc.IsDangerSqlString(name))
            {
                strWhere += " and name = '" + name + "'";
            }
            if (!string.IsNullOrEmpty(certkindno) && !SysFunc.IsDangerSqlString(certkindno))
            {
                strWhere += " and certkindno = '" + certkindno + "'";
            }
            //if (!string.IsNullOrEmpty(certkindname) && !SysFunc.IsDangerSqlString(certkindname))
            //{
            //    strWhere += " and certkindname = '" + certkindname + "'";
            //}
            return strWhere;
        }

        public static string GetVisitorWhere(JObject json)
        {
            string strWhere = string.Empty;

            #region 查询记录

            string visitno = string.Empty;
            string visitorname = string.Empty;
            string certnumber = string.Empty;

            visitno = json.Value<string>("visitno");
            visitorname = json.Value<string>("visitorname");
            certnumber = json.Value<string>("certnumber");

            try
            {
                DateTime intimeStart = json.Value<DateTime>("intimeStart");
                DateTime intimeEnd = json.Value<DateTime>("intimeEnd");


                //来访时间\出访时间
                if (intimeStart != DateTime.MinValue && intimeEnd != DateTime.MinValue)
                {
                    strWhere += " and d.intime between '" + intimeStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + intimeEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }

            }
            catch { }

            try
            {
                DateTime outtimeStart = json.Value<DateTime>("outtimeStart");
                DateTime outtimeEnd = json.Value<DateTime>("outtimeEnd");
                if (outtimeStart != DateTime.MinValue && outtimeEnd != DateTime.MinValue)
                {
                    strWhere += " and d.outtime between '" + outtimeStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + outtimeEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }
            }
            catch { }


            if (!string.IsNullOrEmpty(visitno))
            {
                //访客单号
                strWhere += " and d.visitno like '%" + visitno + "%' ";
            }
            if (!string.IsNullOrEmpty(visitorname))
            {
                strWhere += " and (d.visitorname like '%" + visitorname + "%') ";
            }

            if (!string.IsNullOrEmpty(certnumber))
            {
                //证件号码
                strWhere += " and d.certnumber like '%" + certnumber + "%'";
            }

            #endregion

            return strWhere;
        }

        public static string GetAcWhere(JObject json)
        {
            string strWhere = string.Empty;
            string CardId = string.Empty;
            string DoorName = string.Empty;
            string VisitorName = string.Empty;
            string PersonType = string.Empty;
            string EmpName = string.Empty;

            CardId = json.Value<string>("acCardId");
            DoorName = json.Value<string>("acDoorName");
            VisitorName = json.Value<string>("acVisitorName");
            PersonType = json.Value<string>("acPersonType");
            EmpName = json.Value<string>("acEmpName");

            try
            {
                DateTime starttime = json.Value<DateTime>("acStartTime");
                DateTime endtime = json.Value<DateTime>("acEndTime");

                if (starttime != DateTime.MinValue && endtime != DateTime.MinValue)
                {
                    strWhere += " and RecordTime between '" + starttime.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + endtime.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                }
            }
            catch
            {
                Console.WriteLine("");
            }


            if (!string.IsNullOrEmpty(CardId) && !SysFunc.IsDangerSqlString(CardId))
            {
                strWhere += " and CardId = '" + CardId + "'";
            }
            if (!string.IsNullOrEmpty(DoorName) && !SysFunc.IsDangerSqlString(DoorName))
            {
                strWhere += " and DoorName = '" + DoorName + "'";
            }
            if (!string.IsNullOrEmpty(VisitorName) && !SysFunc.IsDangerSqlString(VisitorName))
            {
                strWhere += " and VisitorName = '" + VisitorName + "'";
            }
            if (!string.IsNullOrEmpty(PersonType) && !SysFunc.IsDangerSqlString(PersonType))
            {
                strWhere += " and PersonType = '" + PersonType + "'";
            }
            if (!string.IsNullOrEmpty(EmpName) && !SysFunc.IsDangerSqlString(EmpName))
            {
                strWhere += " and EmpName = '" + EmpName + "'";
            }
            return strWhere;
        }
        #endregion


        #region 失败的Json格式

        public static string ErrorPost(int _code, string msg, object _data)
        {
            var errObj = new
            {
                code = _code,
                message = msg,
                data = _data
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(errObj);
        }


        #endregion
    }
}
