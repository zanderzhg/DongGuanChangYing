using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Npgsql;

namespace ADServer.DAL
{
    public partial class D_ParkingStat_Info
    {
        public int Add(Model.M_ParkingStat_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into F_Booking_Info(");
            strSql.Append("parkId,parkName,emptySpaceNum,TotalNum,time)");
            strSql.Append(" values (");
            strSql.Append("@parkId,@parkName,@emptySpaceNum,@TotalNum,@time)");
            if (DbHelperSQL.DbType == 1)
            {
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@parkId", SqlDbType.VarChar,30),
					new SqlParameter("@parkName", SqlDbType.VarChar,20),
					new SqlParameter("@emptySpaceNum", SqlDbType.Int),
					new SqlParameter("@TotalNum", SqlDbType.Int),
					new SqlParameter("@time", SqlDbType.DateTime),
            };
                parameters[0].Value = model.parkId;
                parameters[1].Value = model.parkName;
                parameters[2].Value = model.emptySpaceNum;
                parameters[3].Value = model.TotalNum;
                parameters[4].Value = model.time;

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
            else
            {
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@parkId", DbType.StringFixedLength,30),
					new NpgsqlParameter("@parkName", DbType.StringFixedLength,20),
					new NpgsqlParameter("@emptySpaceNum",DbType.Int32),
					new NpgsqlParameter("@TotalNum", DbType.Int32),
					new NpgsqlParameter("@time", DbType.DateTime),
            };
                parameters[0].Value = model.parkId;
                parameters[1].Value = model.parkName;
                parameters[2].Value = model.emptySpaceNum;
                parameters[3].Value = model.TotalNum;
                parameters[4].Value = model.time;

                int rows = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                return rows;
            }

        }
    }
}
