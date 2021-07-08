using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ADServer.Model;

namespace ADServer.BLL
{
    public partial class B_DeleteFaceError_Info
    {
        private readonly DAL.D_DeleteFaceError_Info dal = new DAL.D_DeleteFaceError_Info();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(M_DeleteFaceError_Info model)
        {
            return dal.Add(model);
        }
        public M_DeleteFaceError_Info GetModel(string outerid)
        {
            return dal.GetModel(outerid);
        }

        public DataSet GetList(string where)
        {
            return dal.GetList(where);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            return dal.Delete(id);
        }
        public bool AddTimes(int time,int id)
        {
            return dal.AddTimes(time,id);
        }


    }
}
