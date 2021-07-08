using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using ADServer.Model;

namespace ADServer.BLL
{
    public partial class FKY_GA_Common
    {
        private string accessToken { get; set; }
        private int expireTime { get; set; }
        private DateTime createTime { get; set; }
        /// <summary>
        /// 小区名称
        /// </summary>
        private string Community { get; set; }

        /// <summary>
        /// SN码
        /// </summary>
        private string SerialNo { get; set; }

        private string Area { get; set; }

        private string path { get; set; }
        private string token_src = "/fkyun_ga/gazaglservice/login";
        private static string Visitor_src = "/fkyun_ga/gazaglservice/api/v1.0/saveVisitorRecord";

        private string OrgKey { get; set; }
        private string UploadName { get; set; }
        private string UploadPwd { get; set; }

        public FKY_GA_Common(string absPath, string _Community, string _SerialNo, string _Area, string _OrgKey, string _UploadName, string _UploadPwd)
        {
            this.path = absPath;
            this.Community = _Community;
            this.SerialNo = _SerialNo;
            this.Area = _Area;

            this.OrgKey = _OrgKey;
            this.UploadName = _UploadName;
            this.UploadPwd = _UploadPwd;
        }

        #region 字段修改
        public static void Motify_Visitor_Src(string visitor_src)
        {
            Visitor_src = visitor_src;
        }
        public void Motify_Path(string absPath)
        {
            this.path = absPath;
        }
        public void Motify_Community(string _Community)
        {
            this.Community = _Community;
        }
        public void Motify_SerialNo(string _SerialNo)
        {
            this.SerialNo = _SerialNo;
        }
        public void Motify_Area(string _Area)
        {
            this.Area = _Area;
        }
        public void Motify_OrgKey(string _OrgKey)
        {
            this.OrgKey = _OrgKey;
        }
        public void Motify_UploadName(string _UploadName)
        {
            this.UploadName = _UploadName;
        }
        public void Motify_UploadPwd(string _UploadPwd)
        {
            this.UploadPwd = _UploadPwd;
        }
        #endregion

        public string GetToken()
        {
            if (!string.IsNullOrEmpty(accessToken))//不为空
            {
                if (!isTokenOverTime())//是否过期
                {
                    return accessToken;
                }
                else
                {
                    Access_Token();
                    return accessToken;//有可能为空
                }
            }
            else
            {
                Access_Token();
                return accessToken;//有可能为空
            }
        }

        public bool isTokenOverTime()
        {
            if (this.createTime == null)
            {
                return true;
            }
            int cp = DateTime.Compare(this.createTime.AddSeconds(expireTime), DateTime.Now);
            if (cp < 0)
            {
                return true;//过期
            }
            return false;//没有过期
        }

        public bool Access_Token()
        {
            if (string.IsNullOrEmpty(path))
            {
                this.accessToken = "";
                this.expireTime = 0;
                return false;
            }

            try
            {
                string url = path + token_src;

                var loginObj = new
                {
                    orgKey = this.OrgKey,
                    login_name = this.UploadName,
                    pass_word = RSAHelper.RSAEncrypt(this.UploadPwd)
                };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(loginObj);

                HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(url, json, null, null, Encoding.UTF8, null);
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseText = reader.ReadToEnd().ToString();
                    Console.WriteLine(responseText);
                    JObject jo = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(responseText);

                    int statusCode = jo.Value<int>("statusCode");
                    if (statusCode == 200)
                    {
                        JObject result = jo.Value<JObject>("result");
                        this.accessToken = result.Value<string>("accessToken");
                        this.expireTime = result.Value<int>("expireTime");
                        this.createTime = DateTime.Now.AddSeconds(-2000);
                        return true;
                    }
                    else
                    {
                        this.accessToken = "";
                        this.expireTime = 0;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.accessToken = "";
                this.expireTime = 0;
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                return false;
            }
        }

        public bool UploadVisitor2GA(M_VisitList_Info model)
        {
            try
            {
                string url = path + Visitor_src;
                List<object> lRecord = new List<object>();
                var rec = new
                {
                    XM = RSAHelper.RSAEncrypt(model.VisitorName),
                    SFZH = RSAHelper.RSAEncrypt(model.CertNumber),
                    TYPE = 0,
                    XQ = this.Community,
                    SJ = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ZP = "",//压力大，先不传照片
                    INFO = new
                    {
                        //XQXX = new { },
                        //LDXX = new { },
                        //FJXX = new { },
                        //MJXX = new { },
                        //WYGSXX = new { },
                        //KFSXX = new { },
                        //ZHXX = new { },
                        FKXX = new
                        {
                            FKXM = RSAHelper.RSAEncrypt(model.VisitorName),
                            FKSFZH = RSAHelper.RSAEncrypt(model.CertNumber),
                            FWSY = model.ReasonName,
                            FWSJ = model.InTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                            FWR = model.Field2,
                            FKSJHM = RSAHelper.RSAEncrypt(model.VisitorTel)
                        },
                    },
                };
                lRecord.Add(rec);
                string _records = Newtonsoft.Json.JsonConvert.SerializeObject(lRecord);
                var obj = new
                {
                    ga_area = this.Area,
                    accessToken = GetToken(),
                    sn = this.SerialNo,
                    records = _records
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(url, json, null, null, Encoding.UTF8, null);
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseText = reader.ReadToEnd().ToString();
                    Console.WriteLine(responseText);
                    JObject jo = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(responseText);

                    int statusCode = jo.Value<int>("statusCode");
                    if (statusCode == 200)
                    {
                        return true;
                        //JObject result = jo.Value<JObject>("result");
                    }
                    else
                    {
                        FKY_WCFLibrary.WriteLog.Log4Local("上传失败:\r\n姓名:" + model.VisitorName + " 身份证号:" + model.CertNumber + " 访客单号:" + model.VisitNo + "\r\nJSON:" + responseText, true);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.accessToken = "";
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                return false;
            }
        }


    }
}
