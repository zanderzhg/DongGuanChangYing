using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ADServer.BLL.FKY_AI
{
    public partial class FaceHelper
    {
        private Baidu.Aip.Face.Face client;
        public bool Is_Online;
        public static bool Is_SDK_Init = false;

        public FaceHelper()
        {
            this.Is_Online = false;
            try
            {
                string APIKey = (string)ADServer.DAL.SysFunc.GetParamValue("BaiduApiKey");// System.Configuration.ConfigurationManager.AppSettings["BaiduApiKey"].ToString();
                string SecretKey = (string)ADServer.DAL.SysFunc.GetParamValue("BaiduSecretKey");// System.Configuration.ConfigurationManager.AppSettings["BaidusecretKey"].ToString();

                if (!string.IsNullOrEmpty(APIKey) && !string.IsNullOrEmpty(SecretKey))
                {
                    this.client = new Baidu.Aip.Face.Face(APIKey, SecretKey);
                    this.Is_Online = true;
                }
            }
            catch (Exception exception)
            {
                AddLog("FaceHelper_Error", exception.Message + "\r\n" + exception.StackTrace);
            }
        }

        public FaceHelper(string APIKey, string SecretKey)
        {
            this.Is_Online = false;
            try
            {
                this.client = new Baidu.Aip.Face.Face(APIKey, SecretKey);
                this.Is_Online = true;
            }
            catch (Exception exception)
            {
                AddLog("FaceHelper_Error", exception.Message + "\r\n" + exception.StackTrace);
            }
        }

        private static void AddLog(string fileName, string msg)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + @"Logs\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + fileName + ".txt";
                if (File.Exists(path))
                {
                    using (FileStream stream = new FileStream(path, FileMode.Append))
                    {
                        StreamWriter writer = new StreamWriter(stream);
                        writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "  ——  ");
                        writer.WriteLine(msg);
                        writer.WriteLine();
                        writer.Close();
                    }
                }
                else
                {
                    using (FileStream stream2 = new FileStream(path, FileMode.Create))
                    {
                        StreamWriter writer2 = new StreamWriter(stream2);
                        writer2.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "  ——  ");
                        writer2.WriteLine(msg);
                        writer2.WriteLine();
                        writer2.Close();
                    }
                }
            }
            catch (Exception)
            {
                AddLog(fileName, msg);
            }
        }

        public JObject FaceMatch(byte[] img1, byte[] img2)
        {
            try
            {
                JArray array1 = new JArray();
                JObject obj1 = new JObject {
                    { 
                        "image",
                        (JToken) Convert.ToBase64String(img1)
                    },
                    { 
                        "image_type",
                        "BASE64"
                    },
                    { 
                        "face_type",
                        "LIVE"
                    },
                    { 
                        "quality_control",
                        "LOW"
                    },
                    { 
                        "liveness_control",
                        "NONE"
                    }
                };
                array1.Add(obj1);
                JObject obj2 = new JObject {
                    { 
                        "image",
                        (JToken) Convert.ToBase64String(img2)
                    },
                    { 
                        "image_type",
                        "BASE64"
                    },
                    { 
                        "face_type",
                        "LIVE"
                    },
                    { 
                        "quality_control",
                        "LOW"
                    },
                    { 
                        "liveness_control",
                        "NONE"
                    }
                };
                array1.Add(obj2);
                JArray faces = array1;
                JObject json = JObject.Parse(this.client.Match(faces).ToString());
                return json;
            }
            catch (Exception exception)
            {
                AddLog("FaceMatch_Error", exception.Message + "\r\n" + exception.StackTrace);
            }
            return null;
        }
    }
}
