using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    #region 获取token
    public class DingToken
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string access_token { get; set; }
    }
    #endregion

    #region 获取部门
    public class RetuenDepartment
    {
        public int errcode { get; set; }
        public List<M_DingDepartment> department { get; set; }
        public string errmsg { get; set; }
    }
    public class M_DingDepartment
    {
        public bool createDeptGroup{get;set;}
        public string name{get;set;}
        public int id{get;set;}
        public bool autoAddUser { get; set; }
    }
    #endregion

    #region 获取员工
    public class ReturnEmp 
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public bool hasMore { get; set; }
        public List<M_DingEmp> userlist { get; set; }
    }

    public class M_DingEmp
    {
        public string unionid { get; set; }
        public string openId { get; set; }
        public string remark { get; set; }
        public string userid { get; set; }
        public bool isBoss { get; set; }
        public string tel { get; set; }
        public int[] department { get; set; }
        public string workPlace { get; set; }
        public string email { get; set; }
        public long order { get; set; }
        public bool isLeader { get; set; }
        public string mobile { get; set; }
        public bool active { get; set; }
        public bool isAdmin { get; set; }
        public string avatar { get; set; }
        public bool isHide { get; set; }
        public string jobnumber { get; set; }
        public string name { get; set; }
        public string stateCode { get; set; }
        public string position { get; set; }
    }
    #endregion

    #region 获取员工数量
    public class DingUseCount
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public int count { get; set; }
    }
    #endregion
}
