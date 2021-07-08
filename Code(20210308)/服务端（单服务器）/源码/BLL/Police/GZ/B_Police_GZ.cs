using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Data;

namespace ADServer.BLL
{
    public class B_Police_GZ
    {
        DAL.D_Police_GZ dal = new DAL.D_Police_GZ();

        /// <summary>
        /// 注册单位
        /// </summary>
        /// <param name="unitName"></param>
        /// <param name="unitAddress"></param>
        /// <param name="dealerCode"></param>
        /// <param name="dealerName"></param>
        /// <param name="kscode"></param>
        /// <param name="ksArea"></param>
        /// <param name="legalName"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        public static bool RegisUnit(string url, string unitName, string unitAddress, string dealerCode, string dealerName, string kscode, string ksArea, string legalName, string contact, ref string message, ref string data)
        {
            try
            {
                url += "/api/company-service/company";

                var rt = new
                {
                    name = unitName,
                    address = unitAddress,
                    dealerCode = dealerCode,
                    dealerName = dealerName,
                    kscode = kscode,
                    area = ksArea,
                    legalName = legalName,
                    contact = contact
                };

                string parameters = JsonConvert.SerializeObject(rt);


                HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(url, parameters, null, null, Encoding.UTF8, null);

                string responseText;
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseText = reader.ReadToEnd().ToString();
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = serializer.DeserializeObject(responseText) as Dictionary<string, object>;

                string status = json["status"].ToString();
                message = json["message"].ToString();
                data = json["data"].ToString();
                if (status == "1")
                {
                    return true;
                }
                else
                {
                    if (!Directory.Exists(Application.StartupPath + "\\Logs\\Police"))
                    {
                        Directory.CreateDirectory(Application.StartupPath + "\\Logs\\Police");
                    }
                    string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string file = Application.StartupPath + "\\Logs\\Police\\" + nowTime + ".txt";
                    if (!File.Exists(file))
                    {
                        FileStream fs = new FileStream(file, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write("Exception : status:" + status + ",message:" + message);
                        sw.Close();
                        fs.Close();
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(Application.StartupPath + "\\Logs\\plate"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Logs\\plate");
                }
                string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string file = Application.StartupPath + "\\Logs\\plate\\" + nowTime + ".txt";
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

        public bool UploadVisitlist(string url, string dataJson, ref string message, ref string data)
        {
            try
            {
                url += "/api/visitor-service/visitor";

                HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(url, dataJson, null, null, Encoding.UTF8, null);

                string responseText;
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseText = reader.ReadToEnd().ToString();
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = serializer.DeserializeObject(responseText) as Dictionary<string, object>;

                string status = json["status"].ToString();
                message = json["message"].ToString();
                if (status == "1")
                {
                    return true;
                }
                else
                {
                    if (!Directory.Exists(Application.StartupPath + "\\Logs\\Police"))
                    {
                        Directory.CreateDirectory(Application.StartupPath + "\\Logs\\Police");
                    }
                    string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string file = Application.StartupPath + "\\Logs\\Police\\" + nowTime + ".txt";
                    if (!File.Exists(file))
                    {
                        FileStream fs = new FileStream(file, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write("Exception : status:" + status + ",message:" + message);
                        sw.Close();
                        fs.Close();
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(Application.StartupPath + "\\Logs\\Police"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Logs\\Police");
                }
                string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string file = Application.StartupPath + "\\Logs\\Police\\" + nowTime + ".txt";
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


        public DataSet GetUploadList()
        {
            return dal.GetUploadList();
        }
    }
}
