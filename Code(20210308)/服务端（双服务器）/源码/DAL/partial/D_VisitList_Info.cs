using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ADServer.DAL
{
    public partial class D_VisitList_Info
    {
        /// <summary>
        /// 查询详细记录
        /// </summary>
        public DataSet QueryVisitList_API(string strWhere, int pageIndex, int lines)
        {
            int pageSize = (pageIndex - 1) * lines;
            
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                if (pageSize != 0)
                {
                    pageSize += 1;
                }
                strSql.Append("select * from ( ");

                strSql.Append(" select ROW_NUMBER() OVER(Order by VisitId) AS RowId,d.visitno,d.visitorname,d.certnumber,d.Field2 as visitedName,CONVERT(varchar(100), d.intime, 120) as intime,CONVERT(varchar(100), d.outtime, 120) as outtime, ");

                strSql.Append(" d.visitorflag,d.CardNo,d.InDoorName,d.OutDoorName,");
                strSql.Append(" d.visitorsex,d.certkindname,d.visitorcompany,d.reasonname ,d.visitoraddress,d.belongslist,");
                strSql.Append(" d.visitortel,d.visitorcount,d.Field3 as visitedSex,d.Field4 visitedCompany,d.Field5 as visitedDept,d.Field6 as visitedPost,d.Field7 visitedRoomNum, ");
                strSql.Append(" d.Field8 as visitedTel,d.Field10 as visitedMobilePhone,d.carkind,d.carnumber,d.facescore,");
                strSql.Append(" d.BookNo,d.Field11,d.Field12 from f_visitlist_info d left join f_employ_info c on c.empno = d.empno ");
                strSql.Append(" left join f_department_info b on b.deptno = c.deptno ");
                strSql.Append(" left join f_company_info a on b.companyid = a.companyid ");
                strSql.Append(" left join f_operter_info e on d.operterid = e.operterid ");
                strSql.Append(" where 1=1");

                if (strWhere.Trim() != "")
                {
                    strSql.Append(strWhere);
                }
                strSql.Append(" ) as visitlist ");
                strSql.Append(" where RowId between " + pageSize + " and " + pageIndex * lines);

                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();

                strSql.Append(" select d.visitno,d.visitorname,d.certnumber,d.Field2 as visitedName,to_char(d.intime, 'yyyy-MM-dd HH24:MI:SS') as intime,to_char(d.outtime, 'yyyy-MM-dd HH24:MI:SS') as outtime, ");

                strSql.Append(" d.visitorflag,d.CardNo,d.InDoorName,d.OutDoorName,");
                strSql.Append(" d.visitorsex,d.certkindname,d.visitorcompany,d.reasonname ,d.visitoraddress,d.belongslist, ");
                strSql.Append(" d.visitortel,d.visitorcount,d.Field3 as visitedSex,d.Field4 visitedCompany,d.Field5 as visitedDept,d.Field6 as visitedPost,d.Field7 visitedRoomNum, ");
                strSql.Append(" d.Field8 as visitedTel,d.Field10 as visitedMobilePhone,d.carkind,d.carnumber,d.facescore,");
                strSql.Append(" d.BookNo,d.Field11,d.Field12 from f_visitlist_info d left join f_employ_info c on c.empno = d.empno ");
                strSql.Append(" left join f_department_info b on b.deptno = c.deptno ");
                strSql.Append(" left join f_company_info a on b.companyid = a.companyid ");
                strSql.Append(" left join f_operter_info e on d.operterid = e.operterid ");
                strSql.Append(" where 1=1");

                if (strWhere.Trim() != "")
                {
                    strSql.Append(strWhere);
                }
                strSql.Append(" order by VisitId limit " + lines + " offset " + pageSize);

                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
            }
        }


    }
}
