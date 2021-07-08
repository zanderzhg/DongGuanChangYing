using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADServer.DAL;
using ADServer.Model;

namespace ADServer.BLL
{
    public partial class B_PeopleStatus_Info
    {
        private readonly D_PeopleStatus_Info dal = new D_PeopleStatus_Info();
        public int Add(M_FaceBarrier_Info model)
        {
            return dal.Add(model);
        }
        public int Delete(string visitflag)
        {
            return dal.Delete(visitflag);
        }
        public Boolean Exist(M_FaceBarrier_Info model)
        {
            return dal.Exist(model);
        }
        public Boolean Exist(string model)
        {
            return dal.Exist(model);
        }
        public void UpdateTime(M_FaceBarrier_Info model)
        {
            dal.UpdateTime(model);
        }
    }
}
