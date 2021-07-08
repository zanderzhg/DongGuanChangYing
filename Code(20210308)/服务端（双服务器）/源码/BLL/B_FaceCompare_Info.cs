using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADServer.Model;
using ADServer.DAL;

namespace ADServer.BLL
{
    public partial class B_FaceCompare_Info
    {
        private readonly D_FaceCompare_Info dal = new D_FaceCompare_Info();
        public B_FaceCompare_Info()
        { }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(M_FaceBarrier_Info model)
        {
            return dal.Add(model);
        }

        public string GetIdByEmpNo(string empno)
        {
            return dal.GetIdByEmpNo(empno);
        }

        public void EmpdoLeave(string empno)
        {
            dal.EmpdoLeave(empno);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        //public bool Delete(int id)
        //{
        //    return dal.Delete(id);
        //}
    }
}
