using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using ADServer.Model;
using ADServer.DAL;

namespace ADServer.BLL
{
    public class SMS
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern System.IntPtr GetForegroundWindow();//子线程MessageBox显示在主线程

        private D_SMS_Account dal = new D_SMS_Account();

        #region 第一信息信息接口调用

        /// <summary>
        /// 以 HTTP 的 POST 提交方式 发送短信(ASP.NET的网页或是C#的窗体，均可使用该方法)
        /// </summary>
        /// <param name="mobile">要发送的手机号码</param>
        /// <param name="msg">要发送的内容</param>
        /// <returns>发送结果</returns>
        public static string SendMsg(string mobile, string msg, string name, string pwd, string sign)
        {
            //string name = "登陆账号"; name="tecsun"
            //string pwd = "接口密码（28位的）";//登陆web平台 http://sms.1xinxi.cn  在管理中心--基本资料--接口密码（28位） 如登陆密码修改，接口密码会发生改变，请及时修改程序
            //pwd="173D4057BAB038D1B8166A86F1C7"
            //string sign = "签名";   sign="tecsun"          //一般为企业简称
            StringBuilder arge = new StringBuilder();

            arge.AppendFormat("name={0}", name.Trim());
            arge.AppendFormat("&pwd={0}", pwd.Trim());
            arge.AppendFormat("&content={0}", msg);
            arge.AppendFormat("&mobile={0}", mobile);
            arge.AppendFormat("&sign={0}", sign);
            arge.Append("&type=pt");
            string serverUrl = "http://sms.1xinxi.cn/asmx/smsservice.aspx";

            string resp = PushToWeb(serverUrl, arge.ToString(), Encoding.UTF8);

            System.IntPtr IntPart = GetForegroundWindow();
            WindowWrapper ParentFrm = new WindowWrapper(IntPart);

            if (resp.Split(',')[0] == "0")
            {
                return "";
            }
            else
            {
                //提交失败，可能余额不足，或者敏感词汇等等
            }

            return resp;//是一串 以逗号隔开的字符串。阅读文档查看响应的意思
        }

        /// <summary>
        /// HTTP POST方式
        /// </summary>
        /// <param name="weburl">POST到的网址</param>
        /// <param name="data">POST的参数及参数值</param>
        /// <param name="encode">编码方式</param>
        /// <returns></returns>
        private static string PushToWeb(string weburl, string data, Encoding encode)
        {
            byte[] byteArray = encode.GetBytes(data);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(weburl));
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();

            //接收返回信息：
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader aspx = new StreamReader(response.GetResponseStream(), encode);
            return aspx.ReadToEnd();
        }

        #endregion


        public M_SMS_Account GetModel()
        {
            return dal.GetModel();
        }

        public bool Add(M_SMS_Account model)
        {
            return dal.Add(model);
        }

        public bool Update(M_SMS_Account model)
        {
            return dal.Update(model);
        }
    }
}
