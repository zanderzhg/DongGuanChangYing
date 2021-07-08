using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using ADServer.Model;
using ADServer.DAL;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace ADServer.BLL
{
    public partial class B_Booking_Info
    {
        private readonly DAL.D_Booking_Info dal = new DAL.D_Booking_Info();
        public B_Booking_Info()
        { }

        /// <summary>
        /// 通过预约二维码获取对应的预约记录
        /// </summary>
        /// <param name="qrCode"></param>
        /// <returns></returns>
        public M_Booking_Info GetModelByQRCode(string qrCode, int bookflag)
        {
            return dal.GetModelByQRCode(qrCode, bookflag);
        }

        /// <summary>
        /// 根据预约号，更新预约标识
        /// </summary>
        /// <param name="bookno"></param>
        /// <param name="newflag"></param>
        public void UpdateBookFlag(string bookno, int newflag)
        {
            dal.UpdateBookFlag(bookno, newflag);
        }

        /// <summary>
        /// 返回预约号
        /// </summary>
        /// <returns></returns>
        public string GetBookNo()
        {
            return dal.GetBookNo();
        }

        public int Add(Model.M_Booking_Info model)
        {
            return dal.Add(model);
        }

        public bool UpdateBookRecordFlag(int id)
        {
            string key = "1";
            try
            {
                string url = string.Empty;
                if (SysFunc.GetParamValue("OpenWXSaaS").ToString() == "1" && SysFunc.GetParamValue("OrgId").ToString() != "")
                {
                    url = (string)SysFunc.GetParamValue("FKServiceUrl") + "/wxapi/updateSignAppointment/" + key + "/" + (string)SysFunc.GetParamValue("WeixinAccount") + "/" + SysFunc.GetParamValue("OrgId").ToString() + "/changeBooking/" + id;
                }
                else 
                { 
                    url = (string)SysFunc.GetParamValue("FKServiceUrl") + "/wxapi/index.php?key=tecsun&func=changeBooking&token="
                    + (string)SysFunc.GetParamValue("WeixinAccount")
                    + "&id=" + id;
                }

                HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponse(url, null, null, null);

                string responseText;
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseText = reader.ReadToEnd().ToString();
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = serializer.DeserializeObject(responseText) as Dictionary<string, object>;

                string code = string.Empty;
                if (SysFunc.GetParamValue("OpenWXSaaS").ToString() == "1" && SysFunc.GetParamValue("OrgId").ToString() != "")
                    code = json["statusCode"].ToString();
                else
                    code = json["code"].ToString();

                string message = json["message"].ToString();
                if (code == "200")
                {
                    return true;
                }
                else
                {
                    if (code == "401")
                    {
                        //没有查询到数据
                        return false;
                    }
                    else
                    {
                        if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                        {
                            Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                        }
                        string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                        if (!File.Exists(file))
                        {
                            FileStream fs = new FileStream(file, FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write("Exception : code:" + code + ",message:" + message);
                            sw.Close();
                            fs.Close();
                        }

                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                }
                string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                if (!File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write("Exception :" + ex.Message);
                    sw.Close();
                    fs.Close();
                }
                return false;
            }
        }

        public List<M_Booking_Info> QueryWeiXinGrantCode(string companyWeiXinAccount)
        {
            string key = "1";
            string url = string.Empty;
            List<M_Booking_Info> result = new List<M_Booking_Info>();
            if (SysFunc.GetParamValue("OpenWXSaaS").ToString() == "1" && SysFunc.GetParamValue("OrgId").ToString() != "")//SaaS微信预约
            {
                url = (string)SysFunc.GetParamValue("FKServiceUrl") + "/wxapi/getAllAppointmentList/" + key + "/" + companyWeiXinAccount + "/" + SysFunc.GetParamValue("OrgId").ToString() + "/getBooking/" + 1;
                result = WeiXinSaaSGrantQuery(url);
            }
            else 
            {
                url = (string)SysFunc.GetParamValue("FKServiceUrl") + "/wxapi/index.php?key=tecsun&func=getBooking&token=" + companyWeiXinAccount + "&pageInt=1&areaTag" + SysFunc.GetParamValue("AreaTag");
                result = WeiXinGrantQuery(url);
            }
            return result;
        }
        /// <summary>
        /// 获取预约信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<M_Booking_Info> WeiXinGrantQuery(string url) 
        {
            try
            {
                HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponse(url, null, null, null);

                string responseText;
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseText = reader.ReadToEnd().ToString();
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = serializer.DeserializeObject(responseText) as Dictionary<string, object>;
                string code = json["code"].ToString();
                string message = json["message"].ToString();
                if (code == "200")
                {
                    List<M_Booking_Info> bookingList = new List<M_Booking_Info>();
                    object[] data = json["data"] as object[];
                    for (int i = 0; i < data.Length; i++)
                    {
                        M_Booking_Info booking = new M_Booking_Info();

                        object oRecord = data[i];
                        Dictionary<string, object> record = oRecord as Dictionary<string, object>;
                        booking.id = int.Parse(record["id"].ToString());
                        booking.BookNo = new B_Booking_Info().GetBookNo(); ;
                        booking.CertNumber = record["strIdCertNo"] != null ? record["strIdCertNo"].ToString() : "";
                        booking.BookName = record["strVisitorName"] != null ? record["strVisitorName"].ToString() : "";
                        booking.BookTel = record["strTel"] != null ? record["strTel"].ToString() : "";
                        booking.VisitorCompany = record["strVisitorCompany"] != null ? record["strVisitorCompany"].ToString() : "";
                        booking.QRCode = record["strQRCode"] != null ? record["strQRCode"].ToString() : "";
                        booking.BookReason = record["strReason"] != null ? record["strReason"].ToString() : "";

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
                        else
                        {
                            booking.BookSex = "";
                        }

                        booking.Emptel = record["strBookTel"] != null ? record["strBookTel"].ToString() : "";
                        if (booking.Emptel != "")
                        {
                            DataSet empdata = new BLL.B_Employ_Info().GetListWhereTel(booking.Emptel);
                            if (empdata.Tables[0].Rows.Count > 0)
                            {
                                booking.EmpNo = Convert.ToInt32(empdata.Tables[0].Rows[0]["EmpNo"]);
                                booking.Empname = empdata.Tables[0].Rows[0]["EmpName"] != null ? empdata.Tables[0].Rows[0]["EmpName"].ToString() : "";
                            }
                            else
                            {
                                continue;//找不到被访人
                            }
                        }
                        else
                        {
                            booking.EmpNo = -1;
                            continue;//找不到被访人
                        }

                        booking.BookTime = DateTime.Parse(record["strApplyTime"].ToString());
                        if (record["strValidTimeStart"] != null)
                        {
                            booking.BookTimeStart = DateTime.Parse(record["strValidTimeStart"].ToString());
                            booking.BookDate = DateTime.Now;
                        }
                        else
                        {
                            booking.BookTimeStart = DateTime.Now;
                            booking.BookDate = DateTime.Now;
                        }
                        if (record["strValidTimeEnd"] != null)
                            booking.ValidTimeEnd = DateTime.Parse(record["strValidTimeEnd"].ToString());
                        else
                            booking.ValidTimeEnd = DateTime.Now.AddDays(+1).Date;
                        booking.LicensePlate = record["strLicensePlate"] != null ? record["strLicensePlate"].ToString() : "";
                        booking.VisitNum = record["iVisitNum"] != null ? int.Parse(record["iVisitNum"].ToString()) : 1;
                        booking.quyu = record["quyu"] != null ? record["quyu"].ToString() : "";
                        booking.area = record["area"] != null ? record["area"].ToString() : "";

                        booking.BookFlag = 0;

                        bookingList.Add(booking);

                    }

                    return bookingList;
                }
                else
                {
                    if (code == "401")
                    {
                        //MessageBox.Show("下载预约记录失败！错误详情：没有查询到数据");
                        return null;
                    }
                    else
                    {
                        if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                        {
                            Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                        }
                        string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                        if (!File.Exists(file))
                        {
                            FileStream fs = new FileStream(file, FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write("Exception : " + message);
                            sw.Close();
                            fs.Close();
                        }

                        //MessageBox.Show("下载预约记录失败！错误详情：" + message);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                }
                string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                if (!File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write("Exception : " + ex.Message);
                    sw.Close();
                    fs.Close();
                }
                return null;
            }

        }
        /// <summary>
        /// SaaS获取预约信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<M_Booking_Info> WeiXinSaaSGrantQuery(string url) 
        {
            try
            {
                HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponse(url, null, null, null);

                string responseText;
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseText = reader.ReadToEnd().ToString();
                }
                ReturnData<Booking[]> result = JsonConvert.DeserializeObject<ReturnData<Booking[]>>(responseText);
                if (result.statusCode == "200")
                {
                    List<M_Booking_Info> bookingList = new List<M_Booking_Info>();
                    if (result.data != null)
                    {
                        foreach (Booking item in result.data)
                        {
                            M_Booking_Info booking = new M_Booking_Info();
                            booking.id = (int)item.id;
                            booking.BookNo = new B_Booking_Info().GetBookNo(); ;
                            booking.CertNumber = item.strIdCertNo;
                            booking.BookName = item.strVisitorName;
                            booking.BookTel = item.strTel;
                            booking.VisitorCompany = item.strVisitorCompany;
                            booking.QRCode = item.strQRCode;
                            booking.BookReason = item.strReason;
                            booking.BookSex = item.strSex;
                            booking.Emptel = item.strBookTel;
                            if (booking.Emptel != "")
                            {
                                DataSet empdata = new BLL.B_Employ_Info().GetListWhereTel(booking.Emptel);
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
                            DateTime dateTimeApply = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                            long lTime1 = long.Parse(item.strApplyTime + "0000");
                            TimeSpan toNow = new TimeSpan(lTime1);
                            booking.BookTime = dateTimeApply.Add(toNow);

                            if (item.strValidTimeStart != null)
                            {
                                DateTime strValidTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                                long lTime = long.Parse(item.strValidTimeStart + "0000");
                                TimeSpan toStartNow = new TimeSpan(lTime);
                                booking.BookTimeStart = strValidTimeStart.Add(toStartNow);
                                booking.BookDate = DateTime.Now;
                            }
                            else
                            {
                                booking.BookTimeStart = DateTime.Now;
                                booking.BookDate = DateTime.Now;
                            }
                            if (item.strValidTimeEnd != null)
                            {
                                DateTime strValidTimeEnd = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                                long lTime = long.Parse(item.strValidTimeEnd + "0000");
                                TimeSpan toEndNow = new TimeSpan(lTime);
                                booking.ValidTimeEnd = strValidTimeEnd.Add(toEndNow);
                            }
                            else
                                booking.ValidTimeEnd = DateTime.Now.AddDays(+1).Date;
                            booking.LicensePlate = item.strLicensePlate;
                            booking.VisitNum = item.iVisitNum == null ? 1 : item.iVisitNum;
                            booking.VisitorCompany = item.strVisitorCompany;
                            booking.BookFlag = 0;

                            bookingList.Add(booking);
                        }
                        return bookingList;
                    }
                    else 
                    {
                        return null;
                    }
                }
                else
                {
                    if (result.statusCode == "401")
                    {
                        //MessageBox.Show("下载预约记录失败！错误详情：没有查询到数据");
                        return null;
                    }
                    else
                    {
                        if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                        {
                            Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                        }
                        string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                        if (!File.Exists(file))
                        {
                            FileStream fs = new FileStream(file, FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write("Exception : " + result.message);
                            sw.Close();
                            fs.Close();
                        }

                        //MessageBox.Show("下载预约记录失败！错误详情：" + message);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                }
                string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                if (!File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write("Exception : " + ex.Message);
                    sw.Close();
                    fs.Close();
                }
                return null;
            }

        }
        /// <summary>
        /// 获取待通过微信预约系统通知被防人的记录
        /// </summary>
        /// <returns></returns>
        public List<M_Booking_Info> GetNotifyEmpRecords()
        {
            return dal.GetNotifyEmpRecords();
        }

        /// <summary>
        /// 根据预约号，更新通知被防人标识
        /// </summary>
        /// <param name="bookno"></param>
        /// <param name="newflag"></param>
        public void UpdateNotifyEmp(string bookno, int newflag)
        {
            dal.UpdateNotifyEmp(bookno, newflag);
        }

        public void UpdateNotifyEmpByIdCard(string cardId, int newflag) 
        {
            dal.UpdateNotifyEmpByIdCard(cardId, newflag);
        }

        public bool NotifyEmp(string bookno, string empTel, string visitrorName, string visitorTel, string doorName, int weixinid)
        {
            try
            {
                string key = "1";
                string url = string.Empty;
                List<M_Booking_Info> result = new List<M_Booking_Info>();
                IDictionary<string, string> parameters = null;
                if (SysFunc.GetParamValue("OpenWXSaaS").ToString() == "1" && SysFunc.GetParamValue("OrgId").ToString() != "")//SaaS微信预约
                {
                    url = (string)SysFunc.GetParamValue("FKServiceUrl") + "/wxapi/sendAppointment/" + key + "/" + (string)SysFunc.GetParamValue("WeixinAccount") + "/" + SysFunc.GetParamValue("OrgId").ToString() + "/sendBooking/" + weixinid;
                }
                else
                {
                    Uri u = new Uri((string)SysFunc.GetParamValue("FKServiceUrl"));
                    url = u.AbsoluteUri.TrimEnd(u.LocalPath.ToCharArray()) + "/pushMessage.html";
                    parameters = new Dictionary<string, string>();
                    parameters.Add("token", (string)SysFunc.GetParamValue("WeixinAccount"));
                    parameters.Add("e_phone", empTel);
                    parameters.Add("v_name", visitrorName);
                    parameters.Add("v_phone", visitorTel);
                    parameters.Add("message", "从'" + doorName + "'进入");
                }
                HttpWebResponse response = null;
                if (SysFunc.GetParamValue("OpenWXSaaS").ToString() == "1" && SysFunc.GetParamValue("OrgId").ToString() != "")//SaaS微信预约
                {
                    response = HttpWebResponseUtility.CreateGetHttpResponse(url, null, null, null);
                }
                else
                {
                    response = HttpWebResponseUtility.CreatePostHttpResponse(url, parameters, null, null, Encoding.UTF8, null);
                }

                string responseText;
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseText = reader.ReadToEnd().ToString();
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = serializer.DeserializeObject(responseText) as Dictionary<string, object>;
                string code = string.Empty;
                string message = string.Empty;
                if (SysFunc.GetParamValue("OpenWXSaaS").ToString() == "1" && SysFunc.GetParamValue("OrgId").ToString() != "")//SaaS微信预约
                {
                    code = json["statusCode"].ToString();
                    message = json["message"].ToString();
                }
                else
                {
                    code = json["status"].ToString();
                    message = json["info"].ToString();
                }
                if (code == "200")
                {
                    UpdateNotifyEmp(bookno, 0);

                    return true;

                }
                else
                {
                    if (code == "401")
                    {
                        //没有查询到数据
                        return false;
                    }
                    else
                    {
                        if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                        {
                            Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                        }
                        string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                        if (!File.Exists(file))
                        {
                            FileStream fs = new FileStream(file, FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write("Exception : code:" + code + ",message:" + message);
                            sw.Close();
                            fs.Close();
                        }

                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                }
                string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                if (!File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write("Exception :" + ex.Message);
                    sw.Close();
                    fs.Close();
                }
                return false;
            }
        }

        public Model.M_Booking_Info GetModelByPlateNumber(string plateNumber, int bookflag)
        {
            return dal.GetModelByPlateNumber(plateNumber, bookflag);
        }
    }
}
