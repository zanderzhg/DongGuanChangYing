using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ADServer.Model;
using Npgsql;

namespace ADServer.DAL
{
    public partial class D_WG_Record
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(M_WG_Record_Info model)
        {
            //if (Properties.Settings.Default.DbType == 1)
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_WG_Record_Info(");
                strSql.Append("CardId,RecordTime,DoorName,Event,VisitorName,PersonType,EmpName,controllerSN,controllerIP,doorIndex,isEntryEvent )");
                strSql.Append(" values (");
                strSql.Append("@CardId,@RecordTime,@DoorName,@Event,@VisitorName,@PersonType,@EmpName,@controllerSN,@controllerIP,@doorIndex,@isEntryEvent)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@CardId", SqlDbType.VarChar,50),
					new SqlParameter("@RecordTime", SqlDbType.DateTime),
					new SqlParameter("@DoorName", SqlDbType.VarChar,50),
					new SqlParameter("@Event", SqlDbType.VarChar,50),
                    new SqlParameter("@VisitorName", SqlDbType.VarChar,30),
                    new SqlParameter("@PersonType", SqlDbType.Int,4),
                    new SqlParameter("@EmpName", SqlDbType.VarChar,30),
                    new SqlParameter("@controllerSN", SqlDbType.VarChar,50),
                    new SqlParameter("@controllerIP", SqlDbType.VarChar,50),
                    new SqlParameter("@doorIndex", SqlDbType.Int,2),
                    new SqlParameter("@isEntryEvent", SqlDbType.Int,2)
                                            };
                parameters[0].Value = model.CardSNR;
                parameters[1].Value = model.RecordTime;
                parameters[2].Value = model.DoorName;
                parameters[3].Value = model.REvent;
                parameters[4].Value = model.VisitorName;
                parameters[5].Value = model.PersonType;
                parameters[6].Value = model.EmpName;
                parameters[7].Value = model.ControllerSN;
                parameters[8].Value = model.ControllerIP;
                parameters[9].Value = model.DoorIndex;
                parameters[10].Value = model.IsEntryEvent;

                object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt64(obj);
                }
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_WG_Record_Info(");
                strSql.Append("CardId,RecordTime,DoorName,Event,VisitorName,PersonType,EmpName,controllerSN,controllerIP,doorIndex,isEntryEvent )");
                strSql.Append(" values (");
                strSql.Append("@CardId,@RecordTime,@DoorName,@Event,@VisitorName,@PersonType,@EmpName,@controllerSN,@controllerIP,@doorIndex,@isEntryEvent )");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@CardId", DbType.StringFixedLength,50),
					new NpgsqlParameter("@RecordTime", DbType.DateTime),
					new NpgsqlParameter("@DoorName", DbType.StringFixedLength,50),
					new NpgsqlParameter("@Event", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@VisitorName", DbType.StringFixedLength,30),
                    new NpgsqlParameter("@PersonType", DbType.Int32,4),
                    new NpgsqlParameter("@EmpName", DbType.StringFixedLength,30),
                    new NpgsqlParameter("@controllerSN", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@controllerIP", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@doorIndex", DbType.Int32,2),
                    new NpgsqlParameter("@isEntryEvent", DbType.StringFixedLength,2)
                                            };
                parameters[0].Value = model.CardSNR;
                parameters[1].Value = model.RecordTime;
                parameters[2].Value = model.DoorName;
                parameters[3].Value = model.REvent;
                parameters[4].Value = model.VisitorName;
                parameters[5].Value = model.PersonType;
                parameters[6].Value = model.EmpName;
                parameters[7].Value = model.ControllerSN;
                parameters[8].Value = model.ControllerIP;
                parameters[9].Value = model.DoorIndex;
                parameters[10].Value = model.IsEntryEvent;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                return ret;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from F_WG_Record_Info ");
                strSql.Append(" where id=@id");
                SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
                                        };
                parameters[0].Value = id;

                int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from F_WG_Record_Info ");
                strSql.Append(" where id=@id");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@id", DbType.Int32,4)
                                        };
                parameters[0].Value = id;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                if (ret > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from F_WG_Record_Info ");
            strSql.Append(" where id in (" + idlist + ")  ");

            if (DbHelperSQL.DbType == 1)
            {
                int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
                if (ret > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere, int pageSize, int pageIndex, out int pageCount)
        {
            int allRowsCount = 0;
            pageCount = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) ");
            strSql.Append(" FROM F_WG_Record_Info ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (DbHelperSQL.DbType == 1)
            {
                allRowsCount = (int)(DbHelperSQL.GetSingle(strSql.ToString()));
            }
            else
            {
                DataSet ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
                allRowsCount = int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }

            if (allRowsCount > 0)
            {
                pageCount = allRowsCount / pageSize + 1;
                strSql = new StringBuilder();
                if (DbHelperSQL.DbType == 1)
                {
                    strSql.Append("SELECT * ");
                    strSql.Append("FROM (SELECT ROW_NUMBER() over(order by RecordTime) as row_num,");
                    strSql.Append("CardId,RecordTime,DoorName,Event,VisitorName,PersonType,EmpName,uploadpf,controllerSN,controllerIP,doorIndex,isEntryEvent ");
                    strSql.Append(" FROM F_WG_Record_Info) t");
                    strSql.Append(" FROM t WHERE t.row_num >" + pageSize * (pageIndex - 1) + " AND t.row_num<=" + pageSize * pageIndex);
                    if (!string.IsNullOrEmpty(strWhere.Trim()))
                        strSql.Append(" where " + strWhere);
                    return DbHelperSQL.Query(strSql.ToString());
                }
                else
                {
                    strSql.Append("SELECT CardId,RecordTime,DoorName,Event,VisitorName,PersonType,EmpName,uploadpf,controllerSN,controllerIP,doorIndex,isEntryEvent ");
                    strSql.Append(" FROM F_WG_Record_Info ");
                    if (!string.IsNullOrEmpty(strWhere.Trim()))
                        strSql.Append(" where 1=1 " + strWhere);
                    strSql.Append(" order by a.id asc limit " + pageSize * (pageIndex - 1) + " offset " + pageSize * pageIndex);
                    return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
                }
            }
            else
            {
                return new DataSet(); ;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,CardId,RecordTime,DoorName,Event,VisitorName,PersonType,EmpName,uploadpf,controllerSN,controllerIP,doorIndex,isEntryEvent ");
            strSql.Append(" FROM F_WG_Record_Info ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

        public void UpdateStatus(int recordId, int uploadpf)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update f_wg_record_info ");
            strSql.Append(" set uploadpf = " + uploadpf);
            strSql.Append(" where id  = " + recordId + "");

            if ((int)SysFunc.GetParamValue("DbType") == 1)
            {
                DbHelperSQL.ExecuteSql(strSql.ToString());
            }
            else
            {
                new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }

        }

    }
}
