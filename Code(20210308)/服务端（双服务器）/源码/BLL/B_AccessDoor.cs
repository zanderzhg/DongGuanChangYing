using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ADServer.BLL
{
    class B_AccessDoor
    {
        public B_AccessDoor()
        {
        }

        private readonly DAL.D_AccessDoor dal = new DAL.D_AccessDoor();

        /// <summary>
        /// 增加门禁控制器
        /// </summary>
        /// <param name="ReasonName"></param>
        /// <returns></returns>
        public int AddAccessDoor(string ip, string port, string pwd)
        {
            return dal.AddAccessDoor(ip, port, pwd);
        }

        /// <summary>
        /// 删除一个门禁控制器
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns>成功返回1</returns>
        public bool DeleteAccessDoor(string ip)
        {
            return dal.DeleteAccessDoor(ip);
        }

        public DataSet GetAccessDoors()
        {
            return dal.GetAccessDoors();
        }
    }
}
