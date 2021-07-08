using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ADServer.BLL.CTID
{
    public partial class FKY_CTID
    {
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string CTIDUrl { get; set; }
        public bool CTID_Init { get; set; }

        public FKY_CTID()
        {
            try
            {
                this.CTID_Init = false;
                this.ClientId = (string)ADServer.DAL.SysFunc.GetParamValue("CTIDClientId");// System.Configuration.ConfigurationManager.AppSettings["CTIDClientId"].ToString();
                this.ClientSecret = (string)ADServer.DAL.SysFunc.GetParamValue("CTIDClientSecret");// System.Configuration.ConfigurationManager.AppSettings["CTIDClientSecret"].ToString();
                this.CTIDUrl = (string)ADServer.DAL.SysFunc.GetParamValue("CTIDUrl");// System.Configuration.ConfigurationManager.AppSettings["CTIDUrl"].ToString();
                if (!string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(ClientSecret) && !string.IsNullOrEmpty(CTIDUrl))
                {
                    this.CTID_Init = true;
                }
            }
            catch (Exception ex)
            {
                TecsunPlatform.Common.Logger.Write(ex.ToString());
            }
        }

        public JObject Simpauth(string certNumber, string name, string portrait)
        {
            try
            {
                if (string.IsNullOrEmpty(this.CTIDUrl) || string.IsNullOrEmpty(this.ClientId) || string.IsNullOrEmpty(this.ClientSecret))
                {
                    return null;
                }
                JObject getAccessToken = FKY_CTID_AccessToken.GetAccessToken(this.CTIDUrl, this.ClientId, this.ClientSecret);
                int token_retCode = getAccessToken.Value<int>("retCode");
                if (token_retCode == 0)
                {
                    string accessToken = getAccessToken.Value<string>("accessToken");
                    JObject jsonSimpauth = FKY_CTID_Simpauth.Simpauth(this.CTIDUrl, accessToken, 0x42, certNumber, name, portrait);
                    return jsonSimpauth;
                }
                else
                {
                    string retMessage = getAccessToken.Value<string>("retMessage");
                    TecsunPlatform.Common.Logger.Write("CTID_AccessToken:\r\n身份证信息认证失败！\r\n返回码：" + token_retCode + "\r\n详细信息：" + retMessage + "\r\n原始信息：\r\n" + getAccessToken.ToString());
                    return null;
                }
            }
            catch (Exception ex)
            {
                TecsunPlatform.Common.Logger.Write(ex.ToString());
                return null;
            }
        }


    }
}
