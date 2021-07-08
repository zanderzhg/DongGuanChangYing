using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ADServer.DAL
{
    public partial class D_WG_Record
    {
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList_API(string strWhere, int pageIndex, int lines)
        {
            int pageSize = (pageIndex - 1) * lines;

            if (DbHelperSQL.DbType == 1)
            {
                if (pageSize != 0)
                {
                    pageSize += 1;
                }
                StringBuilder strSql = new StringBuilder();

                strSql.Append("select Id,CardId,CONVERT(varchar(100), RecordTime, 120) as RecordTime,DoorName,Event,VisitorName,PersonType,EmpName");
                strSql.Append(" FROM ( ");
                strSql.Append(" select *, ROW_NUMBER() OVER(Order by id) AS RowId from F_WG_Record_Info where 1=1 ");

                if (strWhere.Trim() != "")
                {
                    strSql.Append(strWhere);
                }
                strSql.Append(" ) as wg ");
                strSql.Append(" where RowId between " + pageSize + " and " + (pageIndex * lines));

                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select Id,CardId,to_char(RecordTime, 'yyyy-MM-dd HH24:MI:SS') as RecordTime,DoorName,Event,VisitorName,PersonType,EmpName");
                strSql.Append(" FROM F_WG_Record_Info where 1=1 ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(strWhere);
                }
                strSql.Append(" order by id asc limit " + lines + " offset " + pageSize);
                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

    }
}
