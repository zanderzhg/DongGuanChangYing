using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ADServer.BLL.CTID
{
    public partial class FKY_CTID_Simpauth
    {

        public static JObject Simpauth(string url, string accessToken, int mode, string idNum, string fullname, string portrait)
        {
            //"authData": { "mode": 18, "idInfo": { "idNum": "44xxxxxxxxxxxxxxx7", "fullName": "xxx", "idStartDate": "20140730", "idEndDate": "00000000" }

            var idInfo = new
            {
                idNum = idNum,
                fullName = fullname
            };

            var authData = new
            {
                mode = mode,
                idInfo = idInfo,
                portrait = portrait
            };

            var postData = new
            {
                accessToken = accessToken,
                authData = authData
            };
            string strPostData = Newtonsoft.Json.JsonConvert.SerializeObject(postData);

            url = FKY_CTID_URL.GetSimpauthURL(url);

            string responseText = Simpauth_ResponseText(url, strPostData);

            if (string.IsNullOrEmpty(responseText))
            {
                return null;
            }
            try
            {
                JObject jsonToken = JObject.Parse(responseText);
                return jsonToken;
            }
            catch (Exception ex)
            {
                TecsunPlatform.Common.Logger.Write(ex.ToString());
                return null;
            }
            
        }


        private static string Simpauth_ResponseText(string url, string strPostData)
        {
            try
            {
                HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(url, strPostData, null, null, Encoding.UTF8, null);
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseText = reader.ReadToEnd().ToString();
                    return responseText;
                }
            }
            catch (Exception ex)
            {
                TecsunPlatform.Common.Logger.Write(ex.ToString());
                return string.Empty;
            }
        }

    }
}
