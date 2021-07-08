using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ADServer.BLL
{
    public partial class B_VisitList_Info
    {
        /// <summary>
        /// 查询详细记录
        /// </summary>
        public DataSet QueryVisitList_API(string strWhere, int pageIndex, int lines)
        {
            return dal.QueryVisitList_API(strWhere, pageIndex, lines);
        }
    }
}
