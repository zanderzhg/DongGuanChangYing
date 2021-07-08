using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ADServer.BLL.CTID
{
    public partial class FKY_CTID_AccessToken
    {
        public static JObject GetAccessToken(string url, string client_id, string client_secret)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("clientid", client_id);
            parameters.Add("clientsecret", client_secret);

            url = FKY_CTID_URL.GetAccessTokenURL(url, client_id, client_secret);
            string responseText = GetAccessToken_ResponseText(url, parameters);
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

        private static string GetAccessToken_ResponseText(string url, IDictionary<string, string> parameters)
        {
            try
            {
                HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponse(url, null, null, null);
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
