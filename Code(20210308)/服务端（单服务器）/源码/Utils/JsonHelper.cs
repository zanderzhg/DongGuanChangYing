using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace ADServer.Utils
{
    /// <summary>  
    /// JSON辅助类  
    /// </summary>  
    public class JsonHelper
    {
        /// <summary>
        /// 将Json格式的时间字符串替换为"yyyy-MM-dd HH:mm:ss"格式的字符串
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string ReplaceJsonDateToDateString(string json)
        {
            return Regex.Replace(json, @"\\/Date\((\d+)\)\\/", match =>
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                dt = dt.ToLocalTime();
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            });
        }

        /// <summary>
        /// 对象转json文本
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(object obj)
        {
            var json = string.Empty;
            StringBuilder sd = new StringBuilder();
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.Serialize(obj, sd);
            }
            catch
            {
                return "";
            }
            return sd.ToString();
        }

        public static Dictionary<string, object> FromJson(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> jsonToken = serializer.DeserializeObject(json) as Dictionary<string, object>;

            return jsonToken;
        }

        public static object[] FromJsonList(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            object[] jsonToken = serializer.DeserializeObject(json) as object[];

            return jsonToken;
        }
    }
}
