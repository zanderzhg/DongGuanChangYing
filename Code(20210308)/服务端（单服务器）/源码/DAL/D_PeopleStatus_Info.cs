using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADServer.Model;
using System.Data.SqlClient;
using System.Data;

namespace ADServer.DAL
{
    public partial class D_PeopleStatus_Info
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(M_FaceBarrier_Info model)
        {
            ADServer.BLL.B_VisitList_Info visitList = new BLL.B_VisitList_Info();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into F_PeopleStatus_Info(");
            strSql.Append("visitorname,matchimg,department,persontype,recordtime,visitflag,reason)");
            strSql.Append(" values (");
            strSql.Append("@visitorname,@matchimg,@department,@persontype,@recordtime,@visitflag,@reason)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@visitorname",SqlDbType.NVarChar,32),
					new SqlParameter("@matchimg", SqlDbType.Image),
					new SqlParameter("@department", SqlDbType.NVarChar,20),
					new SqlParameter("@persontype", SqlDbType.Int,4),
					new SqlParameter("@recordtime", SqlDbType.DateTime),
					new SqlParameter("@visitflag", SqlDbType.NVarChar,64),
                    new SqlParameter("@reason", SqlDbType.NVarChar,64)
                                            };
            parameters[0].Value = model.visitorname;
            parameters[1].Value = model.matchimg;
            parameters[2].Value = model.department;
            parameters[3].Value = model.persontype;
            parameters[4].Value = model.recordtime;
            if (model.persontype == 0)
            {
                parameters[5].Value = model.empno;
                parameters[6].Value = "";
            }
            else
            {
                parameters[5].Value = model.visitno;
                parameters[6].Value = visitList.GetReason(model.visitno);
            }

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        public int Delete(string visitflag)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from F_PeopleStatus_Info ");
            strSql.Append(" where visitflag = '" + visitflag + "'");

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }
        public Boolean Exist(M_FaceBarrier_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            string visitflag = "";
            strSql.Append(" select 1 ");
            strSql.Append(" from F_PeopleStatus_Info");
            if (model.persontype == 0)
            {
                visitflag = model.empno;
            }
            else
            {
                visitflag = model.visitno;
            }
            strSql.Append(" where visitflag = '" + visitflag + "'");
            //strSql.Append(" and recordtime between '" + DateTime.Now.Date + "' and '" + DateTime.Now.Date.AddDays(1).AddMilliseconds(-1) + "'");

            return DbHelperSQL.Exists(strSql.ToString());
        }

        public Boolean Exist(string visitflag)
        {
            StringBuilder strSql = new StringBuilder();            
            strSql.Append(" select 1 ");
            strSql.Append(" from F_PeopleStatus_Info");
            strSql.Append(" where visitflag = '" + visitflag + "'");
            //strSql.Append(" and recordtime between '" + DateTime.Now.Date + "' and '" + DateTime.Now.Date.AddDays(1).AddMilliseconds(-1) + "'");

            return DbHelperSQL.Exists(strSql.ToString());
        }


        public void UpdateTime(M_FaceBarrier_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update F_PeopleStatus_Info ");
            strSql.Append(" set recordtime= '" + model.recordtime + "'");
            if (model.persontype == 0)
            {
                strSql.Append(" where visitflag ='" + model.empno + "'");
            }
            else
            {
                strSql.Append(" where visitflag ='" + model.visitno + "'");
            }
            

            if (DbHelperSQL.DbType == 1)
            {
                DbHelperSQL.ExecuteSql(strSql.ToString());
            }
            else
            {
                new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
            }
        }

    }
}
