using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.BLL.CTID
{
   public partial class FKY_CTID_URL
    {
       public static string GetAccessTokenURL(string url, string client_id, string client_secret)
       {
           string tempRUL = url + "/getaccesstoken?clientId=" + client_id + "&clientSecret=" + client_secret;
           return tempRUL;
       }

       public static string GetSimpauthURL(string url)
       {
           string tempRUL = url + "/simpauth";
           return tempRUL;
       }


    }
}
