using System;
using System.Data;
using System.Collections.Generic;
using ADServer.Model;

namespace ADServer.BLL
{
    public partial class B_Department_Info
    {
        private readonly DAL.D_Department_Info dal = new DAL.D_Department_Info();
        public B_Department_Info()
        { }

        public Boolean Exists_wx(string deptName, string companyName)
        {
            return dal.Exists_wx(deptName, companyName);
        }

        public int Add_wx(M_Department_Info model)
        {
            return dal.Add_wx(model);
        }

        public M_Department_Info GetModel(string deptName, string companyName)
        {
            return dal.GetModel(deptName, companyName);
        }

        public M_Department_Info GetModel_SJP(string deptId)
        {
            return dal.GetModel_SJP(deptId);
        }

        public string GetDeptNameByEmpNo(int EmpNo)
        {
            return dal.GetDeptNameByEmpNo(EmpNo);
        }

    }
}
