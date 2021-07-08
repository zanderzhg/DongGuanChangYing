using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using ADServer.Model;
using Newtonsoft.Json;

namespace ADServer.Interface
{
    public class DingTalkClient
    {
        public string Appkey = "dingjktdnviam5fl94fk";
        public string Appsecret = "c8xNpy6UMiHESMl98f506F3ojPDi_UtM09ojQualR3coD3oMR7X-tkHP0enqBYcR";
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {
            string token = string.Empty;
            string url = "https://oapi.dingtalk.com/gettoken?appkey=" + Appkey + "&appsecret=" + Appsecret;

            string responseText = HttpGet(url);
            DingToken dingToken = JsonConvert.DeserializeObject<DingToken>(responseText);
            if (dingToken.errcode == 0)
                token = dingToken.access_token;
            return token;

        }
        /// <summary>
        /// 获取钉钉部门信息
        /// </summary>
        public List<M_DingDepartment> GetDingDeptGroup() 
        {
            string Access_Token = GetAccessToken();
            string url = "https://oapi.dingtalk.com/department/list?access_token=" + Access_Token;
            string responseText = HttpGet(url);
            RetuenDepartment returnData = JsonConvert.DeserializeObject<RetuenDepartment>(responseText);
            if (returnData.errcode == 0)
                return returnData.department;
            else
                return new List<M_DingDepartment>();
        }
        /// <summary>
        /// 获取钉钉员工信息
        /// </summary>
        public List<M_DingEmp> GetDingEmploy()
        {
            List<M_DingEmp> result = new List<M_DingEmp>();
            List<M_DingDepartment> deptGroup = GetDingDeptGroup();
            deptGroup.ForEach(item => 
            {
                result.AddRange(GetPageEmploy(item.id, 0));
            });
            return result;
        }
        /// <summary>
        /// 分页获取员工 递归
        /// </summary>
        /// <param name="id"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<M_DingEmp> GetPageEmploy(int id, int size) 
        {
            List<M_DingEmp> result = new List<M_DingEmp>();
            string Access_Token = GetAccessToken();
            string url = "https://oapi.dingtalk.com/user/listbypage?access_token=" + Access_Token + "&department_id=" + id + "&offset=" + size + "&size=50";
            string responseText = HttpGet(url);
            ReturnEmp returnData = JsonConvert.DeserializeObject<ReturnEmp>(responseText);
            result.AddRange(returnData.userlist);
            if (returnData.hasMore) 
                GetPageEmploy(id, size++);
            return result;
        }
        /// <summary>
        /// 获取员工数量
        /// </summary>
        /// <returns></returns>
        public int GetDingUserCount()
        {
            int count = 0;
            string Access_Token = GetAccessToken();
            string url = "https://oapi.dingtalk.com/user/get_org_user_count?access_token=" + Access_Token + "&onlyActive=1";
            string responseText = HttpGet(url);
            DingUseCount dingCount = JsonConvert.DeserializeObject<DingUseCount>(responseText);
            if (dingCount.errcode == 0)
                count = dingCount.count;
            return count;
        }


        /// <summary>
        /// http GET
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string HttpGet(string url)
        {
            //创建请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //GET请求
            request.Method = "GET";
            request.ReadWriteTimeout = 5000;
            Encoding encode = Encoding.UTF8;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, encode);
            //返回内容
            return  myStreamReader.ReadToEnd();
        }
    }
}
