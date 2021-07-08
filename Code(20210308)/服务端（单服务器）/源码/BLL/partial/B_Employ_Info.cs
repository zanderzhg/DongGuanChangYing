using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ADServer.BLL
{
    public partial class B_Employ_Info
    {
        public Model.M_Employ_Info GetModel_API(string phone)
        {
            return dal.GetModel_API(phone);
        }

        /// <summary>
        /// 是否存在此被访人电话号码的记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean ExistEmpPhone_API(string phone)
        {
            return dal.ExistEmpPhone_API(phone);
        }

        public bool Delete_API(string phone)
        {
            return dal.Delete_API(phone);
        }

        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update_API(Model.M_Employ_Info model)
        {
            return dal.Update_API(model);
        }



        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList_API(string strWhere, int pageIndex, int lines)
        {
            return dal.GetList_API(strWhere, pageIndex, lines);
        }



    }
}
