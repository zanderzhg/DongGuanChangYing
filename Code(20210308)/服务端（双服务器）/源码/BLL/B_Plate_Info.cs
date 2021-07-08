using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using ADServer.DAL;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ADServer.BLL
{
    public class B_Plate_Info
    {
        private readonly DAL.D_Plate_Info dal = new DAL.D_Plate_Info();
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.M_Plate_Info GetModel(string plate)
        {
            return dal.GetModel(plate);
        }

        public bool updateIn(string type, int id)
        {
            return dal.updateIn(type, id);
        }

        public bool updateOut(string type, int id)
        {
            return dal.updateOut(type, id);
        }

        public bool delete(int id)
        {
            return dal.delete(id);
        }

        public int Add(Model.M_Plate_Info model)
        {
            return dal.Add(model);
        }

        public static List<string> WxGetCar()
        {
            try
            {
                string url = (string)SysFunc.GetParamValue("FKServiceUrl");
                Uri uri = new Uri(url);
                url = uri.AbsoluteUri.TrimEnd(uri.LocalPath.ToCharArray()) + "/getCar";

                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("token", (string)SysFunc.GetParamValue("WeixinAccount"));


                HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(url, parameters, null, null, Encoding.UTF8, null);

                string responseText;
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseText = reader.ReadToEnd().ToString();
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = serializer.DeserializeObject(responseText) as Dictionary<string, object>;

                string code = json["code"].ToString();
                string message = json["msg"].ToString();
                if (code == "200")
                {
                    return json["data"].ToString().Split(',').ToList();
                }
                else
                {
                    if (code == "401")
                    {
                        //没有查询到数据
                        return new List<string>();
                    }
                    else
                    {
                        if (!Directory.Exists(Application.StartupPath + "\\Logs\\Wx"))
                        {
                            Directory.CreateDirectory(Application.StartupPath + "\\Logs\\Wx");
                        }
                        string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss") + Guid.NewGuid().ToString();
                        string file = Application.StartupPath + "\\Logs\\Wx\\" + nowTime + ".txt";
                        if (!File.Exists(file))
                        {
                            FileStream fs = new FileStream(file, FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write("Exception : code:" + code + ",message:" + message);
                            sw.Close();
                            fs.Close();
                        }

                        return new List<string>();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(Application.StartupPath + "\\Logs\\Wx"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Logs\\Wx");
                }
                string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss") + Guid.NewGuid().ToString();
                string file = Application.StartupPath + "\\Logs\\Wx\\" + nowTime + ".txt";
                if (!File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write("Exception :" + ex.Message);
                    sw.Close();
                    fs.Close();
                }
                return new List<string>();
            }
        }

        public static bool GrantOGCar(List<string> plateList)
        {
            try
            {
                string url = "http://" + SysFunc.GetParamValue("OGIPServer").ToString() + ":" + SysFunc.GetParamValue("OGPortServer").ToString() + "/grantCar";

                var rt = new
                {
                    plateNumber = plateList,
                    inflag = 0,
                    outflag = 1,
                    starttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    endtime = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1).ToString("yyyy-MM-dd HH:mm:ss")
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

                string code = json["code"].ToString();
                string message = json["msg"].ToString();
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



    }
}
