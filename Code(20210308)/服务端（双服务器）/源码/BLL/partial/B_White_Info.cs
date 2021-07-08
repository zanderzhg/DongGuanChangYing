using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ADServer.BLL
{
    public class B_White_Info
    {
        public DAL.D_White_Info dal = new DAL.D_White_Info();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.M_White_Info model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.M_White_Info model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 判断是否重复
        /// </summary>
        /// <returns></returns>
        public Boolean Exists(string certkind, string certNum)
        {
            return dal.Exists(certkind, certNum);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string Phone)
        {
            return dal.Delete(Phone);
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
