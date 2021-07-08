using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.BLL
{
    public partial class B_ParkingStat_Info
    {
        private readonly DAL.D_ParkingStat_Info dal = new DAL.D_ParkingStat_Info();
        public B_ParkingStat_Info()
        { }

        public int Add(Model.M_ParkingStat_Info model)
        {
            return dal.Add(model);
        }

    }
}
