using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.BLL
{
    public partial class B_Company_Info
    {
        private readonly DAL.D_Company_Info dal = new DAL.D_Company_Info();
        public B_Company_Info()
        { }

        /// <summary>
        /// 新增时，判断公司名称是否重复
        /// </summary>
        /// <param name="belongName"></param>
        /// <returns></returns>
        public Boolean Exists_wx(string name)
        {
            return dal.Exists_wx(name);
        }

        public int Add_wx(Model.M_Company_Info model)
        {
            return dal.Add_wx(model);
        }

        public Model.M_Company_Info GetModel(string name)
        {

            return dal.GetModel(name);
        }
    }
}
