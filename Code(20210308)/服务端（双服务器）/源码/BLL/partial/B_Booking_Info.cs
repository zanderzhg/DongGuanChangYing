using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ADServer.BLL
{
    public partial class B_Booking_Info
    {
        public int Add_API(Model.M_Booking_Info model)
        {
            return dal.Add_API(model);
        }

        public int Delete_API(string bookno)
        {
            return dal.Delete_API(bookno);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update_API(Model.M_Booking_Info model)
        {
            return dal.Update_API(model);
        }

        /// <summary>
        /// 得到预约的详细信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetBookingInfo_API(string strWhere, int pageIndex, int lines)
        {
            return dal.GetBookingInfo_API(strWhere, pageIndex, lines);
        }

    }
}
