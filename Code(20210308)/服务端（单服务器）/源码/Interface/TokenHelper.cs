using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using ADServer.Model;

namespace ADServer.BLL
{
    public class TokenHelper
    {
        public static string SecretKey = "TecsunPlatformServiceKey";//这个服务端加密秘钥 属于私钥
        private static JavaScriptSerializer myJson = new JavaScriptSerializer();

        public static string GenToken(M_Token_Info M)
        {
            IDateTimeProvider provider = new UtcDateTimeProvider();
            var now = provider.GetNow().Date.AddDays(1).AddMilliseconds(-1); 

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // or use JwtValidator.UnixEpoch
            int secondsSinceEpoch = (int)Math.Round((now - unixEpoch).TotalSeconds);

            var payload = new Dictionary<string, dynamic>
            {
                {"appid", M.appid},
                {"exp",secondsSinceEpoch }
            };
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            return encoder.Encode(payload, SecretKey);
        }

        public static M_Token_Info DecodeToken(string token)
        {
            try
            {
                var json = GetTokenJson(token);
                M_Token_Info info = myJson.Deserialize<M_Token_Info>(json);
                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetTokenJson(string token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                var json = decoder.Decode(token, SecretKey, verify: true);
                return json;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
