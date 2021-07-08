using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ADServer.Utils
{
    public class ApiTools
    {
        private static string msgModel = "{{\"code\":{0},\"message\":\"{1}\",\"data\":{2}}}";
        public ApiTools()
        {
        }

        public static string MsgFormat(ResponseCode code, string explanation, string result)
        {
            string r = @"^(\-|\+)?\d+(\.\d+)?$";
            string json = string.Empty;
            if (Regex.IsMatch(result, r) || result.ToLower() == "true" || result.ToLower() == "false" || result == "[]" || result.Contains('{'))
            {
                json = string.Format(msgModel, (int)code, explanation, result);
            }
            else
            {
                if (result.Contains('"'))
                {
                    json = string.Format(msgModel, (int)code, explanation, result);
                }
                else
                {
                    json = string.Format(msgModel, (int)code, explanation, "\"" + result + "\"");
                }
            }
            return json;
        }

        public enum ResponseCode
        {
            操作失败 = 400,
            成功 = 200,
            授权失败 = 201,
            内部错误 = 500,
        }
    }
}
