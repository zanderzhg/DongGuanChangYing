using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Npgsql;
using ADServer.Model;
using System.Collections.Generic;

namespace ADServer.DAL
{
    /// <summary>
    /// 数据访问类:M_Card_Info
    /// </summary>
    public partial class D_Card_Info
    {
        public D_Card_Info()
        { }

        /// <summary>
        /// 得到所有已关联的IC卡信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet getCardInfo(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.id,a.cardid,a.cardtype,a.startdate,a.enddate,");
            strSql.Append(" b.visitno,b.visitorname,b.certnumber ");
            strSql.Append(" from f_card_info a,f_visitlist_info b ");
            strSql.Append(" where a.visitnonow = b.visitno " + strWhere);

            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

        /// <summary>
        /// 根据访客单号，得到IC卡号
        /// </summary>
        /// <param name="visitnonow"></param>
        /// <returns></returns>
        public string GetCardNoByVisitNoNow(string visitnonow)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select cardid from f_card_info ");
            strSql.Append(" where visitnonow = '" + visitnonow.ToString() + "'");

            if (DbHelperSQL.DbType == 1)
            {
                object obj = DbHelperSQL.GetSingle(strSql.ToString());
                if (obj == null)
                {
                    return "";
                }
                else
                {
                    return obj.ToString();
                }
            }
            else
            {
                DataSet ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
                return ds.Tables[0].Rows[0][0].ToString();
            }
        }

        /// <summary>
        /// 根据IC卡号，得到访客单号
        /// </summary>
        /// <param name="visitnonow"></param>
        /// <returns></returns>
        public string GetVisitNoNowByCardNo(string cardid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select visitnonow from f_card_info ");
            strSql.Append(" where cardid = '" + cardid + "'");

            if (DbHelperSQL.DbType == 1)
            {
                object obj = DbHelperSQL.GetSingle(strSql.ToString());
                if (obj == null)
                {
                    return "";
                }
                else
                {
                    return obj.ToString();
                }
            }
            else
            {
                DataSet ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
                return ds.Tables[0].Rows[0][0].ToString();
            }
        }


        /// <summary>
        /// 根据访客单号
        /// 判断该访客是否已关联临时卡
        /// （用于身份证签离时，提示回收临时卡）
        /// </summary>
        /// <param name="visitno"></param>
        /// <returns></returns>
        public bool bExistsRelation(string visitno)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * ");
            strSql.Append(" from f_visitlist_info a,f_card_info b ");
            strSql.Append(" where a.visitno = b.visitnonow and a.cardtype = b.cardtype ");
            strSql.Append(" and b.cardtype = '临时卡' ");
            strSql.Append(" and a.visitno = '" + visitno.ToString() + "'");

            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Exists(strSql.ToString());
            }
            else
            {
                object obj = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 根据访客姓名与证件号码
        /// 判断该访客是否已关联常访卡
        /// </summary>
        /// <param name="name"></param>
        /// <param name="certnumber"></param>
        public bool bExistsRelation(string name, string certnumber)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * ");
            strSql.Append(" from f_visitlist_info a,f_card_info b ");
            strSql.Append(" where a.visitno = b.visitnonow and a.cardtype = b.cardtype ");
            strSql.Append(" and b.cardtype = '常访卡' ");
            strSql.Append(" and a.visitorname = '" + name.ToString() + "'");
            strSql.Append(" and a.certnumber = '" + certnumber.ToString() + "'");

            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Exists(strSql.ToString());
            }
            else
            {
                object obj = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 根据IC卡号，更新常访卡的关联
        /// </summary>
        /// <param name="cardid"></param>
        public void UpdateVisitNoNowByCardId(string cardid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update f_card_info ");
            strSql.Append(" set visitnonow = (select top 1 b.visitno from f_card_info a,f_visitlist_info b where a.cardid =  b.cardno and ");
            strSql.Append(" a.cardid = '" + cardid + "' order by b.visitid desc) ");
            strSql.Append(" where cardid = '" + cardid + "'");

            if (DbHelperSQL.DbType == 1)
            {
                DbHelperSQL.ExecuteSql(strSql.ToString());
            }
            else
            {
                new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }

        }

        /// <summary>
        /// 根据Ic卡号，判断是否还在有效期
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public Boolean bIcCardCanUse(string cardid)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select * from f_card_info ");
                strSql.Append(" where cardid = '" + cardid + "'");
                strSql.Append(" and enddate >getdate()");
                return DbHelperSQL.Exists(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select * from f_card_info ");
                strSql.Append(" where cardid = '" + cardid + "'");
                strSql.Append(" and enddate >localtimestamp(0)");
                object obj = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 重置IC卡
        /// </summary>
        /// <param name="cardno"></param>
        public void ResetCardInfo(string cardno)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" delete from f_card_info ");
            strSql.Append(" where cardid = '" + cardno + "'");
            if (DbHelperSQL.DbType == 1)
            {
                DbHelperSQL.ExecuteSql(strSql.ToString());
            }
            else
            {
                new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }

        }

        /// <summary>
        /// 重置临时卡
        /// </summary>
        /// <param name="visitno"></param>
        public void ResetTempCardInfoByVisitno(string visitno)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append(" update f_card_info ");
            //strSql.Append(" set usestatus = 0,cardtype = '',visitnonow = '',");
            //strSql.Append(" startdate = null,enddate = null ");
            //strSql.Append(" where VisitNoNow = '" + visitno + "' and CardType like '%临时%'");

            //DbHelperSQL.ExecuteSql(strSql.ToString());

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" delete from f_card_info ");
            strSql.Append(" where VisitNoNow = '" + visitno + "' and CardType like '%临时%'");

            if (DbHelperSQL.DbType == 1)
            {
                DbHelperSQL.ExecuteSql(strSql.ToString());
            }
            else
            {
                new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
            }

        }

        /// <summary>
        /// 是否存在该卡号的记录
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public Boolean GetExistsCard(string cardid, string cardtype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from f_card_info ");
            strSql.Append(" where cardid = '" + cardid + "'");
            if (cardtype == "IC卡")
            {
                strSql.Append(" and cardtype like '%卡%'");
            }
            else if (cardtype == "身份证")
            {
                strSql.Append(" and cardtype like '%身份证%'");
            }
            else if (cardtype == "访客单")
            {
                strSql.Append(" and cardtype like '%访客单%'");
            }

            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Exists(strSql.ToString());
            }
            else
            {
                object obj = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public string GetVisitNoByPlate(string plate)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select top 1 visitno");
                strSql.Append(" from F_VisitList_Info");
                strSql.Append(" where carnumber = @carnumber");
                strSql.Append(" and visitorflag = 0 ");
                strSql.Append(" order by intime desc");

                SqlParameter[] parameters = {
					new SqlParameter("@carnumber", SqlDbType.VarChar,20)
                                             };
                parameters[0].Value = plate;

                return Convert.ToString(DbHelperSQL.GetSingle(strSql.ToString()));
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select visitno");
                strSql.Append(" from F_VisitList_Info");
                strSql.Append(" where carnumber = @carnumber");
                strSql.Append(" and visitorflag = 0 ");
                strSql.Append(" order by intime desc limit 1");

                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@carnumber", DbType.StringFixedLength,20)
                                                               };
                parameters[0].Value = plate;


                DataSet ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }


        /// <summary>
        /// 根据卡号得到编号ID
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public int GetID(string cardid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select id from f_card_info ");
            strSql.Append(" where cardid = '" + cardid + "'");

            if (DbHelperSQL.DbType == 1)
            {
                object obj = DbHelperSQL.GetSingle(strSql.ToString());
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
                DataSet ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
        }

        #region  Method


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.M_Card_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_Card_Info(");
                strSql.Append("CardId,UseStatus,CardType,VisitNoNow,StartDate,EndDate,Disposable,GrantDoorMsg)");
                strSql.Append(" values (");
                strSql.Append("@CardId,@UseStatus,@CardType,@VisitNoNow,@StartDate,@EndDate,@Disposable,@GrantDoorMsg)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@CardId", SqlDbType.VarChar,30),
					new SqlParameter("@UseStatus", SqlDbType.VarChar,2),
					new SqlParameter("@CardType", SqlDbType.VarChar,20),
					new SqlParameter("@VisitNoNow", SqlDbType.VarChar,30),
					new SqlParameter("@StartDate", SqlDbType.DateTime),
					new SqlParameter("@EndDate", SqlDbType.DateTime),
                    new SqlParameter("@Disposable", SqlDbType.VarChar,2),
                    new SqlParameter("@GrantDoorMsg", SqlDbType.VarChar,100)
                                            };
                parameters[0].Value = model.CardId;
                parameters[1].Value = model.UseStatus;
                parameters[2].Value = model.CardType;
                parameters[3].Value = model.VisitNoNow;
                parameters[4].Value = model.StartDate;
                parameters[5].Value = model.EndDate;
                parameters[6].Value = model.Disposable;
                parameters[7].Value = model.GrantDoorMsg;

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
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_Card_Info(");
                strSql.Append("CardId,UseStatus,CardType,VisitNoNow,StartDate,EndDate,Disposable,GrantDoorMsg)");
                strSql.Append(" values (");
                strSql.Append("@CardId,@UseStatus,@CardType,@VisitNoNow,@StartDate,@EndDate,@Disposable,@GrantDoorMsg)");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@CardId", DbType.StringFixedLength,30),
					new NpgsqlParameter("@UseStatus", DbType.StringFixedLength,2),
					new NpgsqlParameter("@CardType", DbType.StringFixedLength,20),
					new NpgsqlParameter("@VisitNoNow", DbType.StringFixedLength,30),
					new NpgsqlParameter("@StartDate", DbType.DateTime),
					new NpgsqlParameter("@EndDate", DbType.DateTime),
                    new NpgsqlParameter("@Disposable", DbType.StringFixedLength,2),
                    new NpgsqlParameter("@GrantDoorMsg", DbType.StringFixedLength,100)
                                               };
                parameters[0].Value = model.CardId;
                parameters[1].Value = model.UseStatus;
                parameters[2].Value = model.CardType;
                parameters[3].Value = model.VisitNoNow;
                parameters[4].Value = model.StartDate;
                parameters[5].Value = model.EndDate;
                parameters[6].Value = model.Disposable;
                parameters[7].Value = model.GrantDoorMsg;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                return ret;
            }

        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.M_Card_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update F_Card_Info set ");
            strSql.Append("CardId=@CardId,");
            strSql.Append("UseStatus=@UseStatus,");
            strSql.Append("CardType=@CardType,");
            strSql.Append("VisitNoNow=@VisitNoNow,");
            strSql.Append("StartDate=@StartDate,");
            strSql.Append("EndDate=@EndDate,");
            strSql.Append("Disposable=@Disposable,");
            strSql.Append("GrantDoorMsg=@GrantDoorMsg");
            strSql.Append(" where id=@id");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@CardId", SqlDbType.VarChar,30),
					new SqlParameter("@UseStatus", SqlDbType.VarChar,2),
					new SqlParameter("@CardType", SqlDbType.VarChar,20),
					new SqlParameter("@VisitNoNow", SqlDbType.VarChar,30),
					new SqlParameter("@StartDate", SqlDbType.DateTime),
					new SqlParameter("@EndDate", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@Disposable", SqlDbType.VarChar,2),
                    new SqlParameter("@GrantDoorMsg", SqlDbType.VarChar,100)
                                            };
                parameters[0].Value = model.CardId;
                parameters[1].Value = model.UseStatus;
                parameters[2].Value = model.CardType;
                parameters[3].Value = model.VisitNoNow;
                parameters[4].Value = model.StartDate;
                parameters[5].Value = model.EndDate;
                parameters[6].Value = model.id;
                parameters[7].Value = model.Disposable;
                parameters[8].Value = model.GrantDoorMsg;

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
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@CardId", DbType.StringFixedLength,30),
					new NpgsqlParameter("@UseStatus", DbType.StringFixedLength,2),
					new NpgsqlParameter("@CardType", DbType.StringFixedLength,20),
					new NpgsqlParameter("@VisitNoNow", DbType.StringFixedLength,30),
					new NpgsqlParameter("@StartDate", DbType.DateTime),
					new NpgsqlParameter("@EndDate", DbType.DateTime),
					new NpgsqlParameter("@id", DbType.Int32,4),
                    new NpgsqlParameter("@Disposable", DbType.StringFixedLength,2),
                    new NpgsqlParameter("@GrantDoorMsg", DbType.StringFixedLength,100)
                                               };
                parameters[0].Value = model.CardId;
                parameters[1].Value = model.UseStatus;
                parameters[2].Value = model.CardType;
                parameters[3].Value = model.VisitNoNow;
                parameters[4].Value = model.StartDate;
                parameters[5].Value = model.EndDate;
                parameters[6].Value = model.id;
                parameters[7].Value = model.Disposable;
                parameters[8].Value = model.GrantDoorMsg;

                int rows = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);

                if (rows > 0)
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
        /// 删除一条数据
        /// </summary>
        public bool DeleteByCardNum(string cardNum)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from F_Card_Info ");
                strSql.Append(" where cardid=@cardid");
                SqlParameter[] parameters = {
					new SqlParameter("@cardid", SqlDbType.VarChar)
                                        };
                parameters[0].Value = cardNum;

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
                strSql.Append("delete from F_Card_Info ");
                strSql.Append(" where cardid=@cardid");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@cardid", DbType.StringFixedLength)
                                        };
                parameters[0].Value = cardNum;

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
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from F_Card_Info ");
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
                strSql.Append("delete from F_Card_Info ");
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
            strSql.Append("delete from F_Card_Info ");
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
        /// 得到一个对象实体
        /// </summary>
        public Model.M_Card_Info GetModel(int id)
        {
            Model.M_Card_Info model = new Model.M_Card_Info();
            DataSet ds = null;

            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 id,CardId,UseStatus,CardType,VisitNoNow,StartDate,EndDate,Disposable,GrantDoorMsg,GrantElevatorMsg from F_Card_Info ");
                strSql.Append(" where id=@id");
                SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
                                        };
                parameters[0].Value = id;

                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select id,CardId,UseStatus,CardType,VisitNoNow,StartDate,EndDate,Disposable,GrantDoorMsg,GrantElevatorMsg from F_Card_Info ");
                strSql.Append(" where id=@id limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@id", DbType.Int32,4)
                                        };
                parameters[0].Value = id;

                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CardId"] != null && ds.Tables[0].Rows[0]["CardId"].ToString() != "")
                {
                    model.CardId = ds.Tables[0].Rows[0]["CardId"].ToString();
                }
                if (ds.Tables[0].Rows[0]["UseStatus"] != null && ds.Tables[0].Rows[0]["UseStatus"].ToString() != "")
                {
                    model.UseStatus = ds.Tables[0].Rows[0]["UseStatus"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CardType"] != null && ds.Tables[0].Rows[0]["CardType"].ToString() != "")
                {
                    model.CardType = ds.Tables[0].Rows[0]["CardType"].ToString();
                }
                if (ds.Tables[0].Rows[0]["VisitNoNow"] != null && ds.Tables[0].Rows[0]["VisitNoNow"].ToString() != "")
                {
                    model.VisitNoNow = ds.Tables[0].Rows[0]["VisitNoNow"].ToString();
                }
                if (ds.Tables[0].Rows[0]["StartDate"] != null && ds.Tables[0].Rows[0]["StartDate"].ToString() != "")
                {
                    model.StartDate = DateTime.Parse(ds.Tables[0].Rows[0]["StartDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EndDate"] != null && ds.Tables[0].Rows[0]["EndDate"].ToString() != "")
                {
                    model.EndDate = DateTime.Parse(ds.Tables[0].Rows[0]["EndDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Disposable"] != null && ds.Tables[0].Rows[0]["Disposable"].ToString() != "")
                {
                    model.Disposable = ds.Tables[0].Rows[0]["Disposable"].ToString();
                }
                if (ds.Tables[0].Rows[0]["GrantDoorMsg"] != null && ds.Tables[0].Rows[0]["GrantDoorMsg"].ToString() != "")
                {
                    model.GrantDoorMsg = ds.Tables[0].Rows[0]["GrantDoorMsg"].ToString();
                }
                if (ds.Tables[0].Rows[0]["GrantElevatorMsg"] != null && ds.Tables[0].Rows[0]["GrantElevatorMsg"].ToString() != "")
                {
                    model.GrantElevatorMsg = ds.Tables[0].Rows[0]["GrantElevatorMsg"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.M_Card_Info GetModelByCardId(string cardID)
        {
            Model.M_Card_Info model = new Model.M_Card_Info();
            DataSet ds = null;

            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 id,CardId,UseStatus,CardType,VisitNoNow,StartDate,EndDate,Disposable,GrantDoorMsg,GrantElevatorMsg from F_Card_Info ");
                strSql.Append(" where CardId=@CardId order by StartDate desc");
                SqlParameter[] parameters = {
					new SqlParameter("@CardId", SqlDbType.VarChar,30)
                                        };
                parameters[0].Value = cardID;

                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select id,CardId,UseStatus,CardType,VisitNoNow,StartDate,EndDate,Disposable,GrantDoorMsg,GrantElevatorMsg from F_Card_Info ");
                strSql.Append(" where CardId=@CardId order by StartDate desc limit 1 ");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@CardId", DbType.StringFixedLength,256)
                                        };
                parameters[0].Value = cardID;

                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CardId"] != null && ds.Tables[0].Rows[0]["CardId"].ToString() != "")
                {
                    model.CardId = ds.Tables[0].Rows[0]["CardId"].ToString();
                }
                if (ds.Tables[0].Rows[0]["UseStatus"] != null && ds.Tables[0].Rows[0]["UseStatus"].ToString() != "")
                {
                    model.UseStatus = ds.Tables[0].Rows[0]["UseStatus"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CardType"] != null && ds.Tables[0].Rows[0]["CardType"].ToString() != "")
                {
                    model.CardType = ds.Tables[0].Rows[0]["CardType"].ToString();
                }
                if (ds.Tables[0].Rows[0]["VisitNoNow"] != null && ds.Tables[0].Rows[0]["VisitNoNow"].ToString() != "")
                {
                    model.VisitNoNow = ds.Tables[0].Rows[0]["VisitNoNow"].ToString();
                }
                if (ds.Tables[0].Rows[0]["StartDate"] != null && ds.Tables[0].Rows[0]["StartDate"].ToString() != "")
                {
                    model.StartDate = DateTime.Parse(ds.Tables[0].Rows[0]["StartDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EndDate"] != null && ds.Tables[0].Rows[0]["EndDate"].ToString() != "")
                {
                    model.EndDate = DateTime.Parse(ds.Tables[0].Rows[0]["EndDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Disposable"] != null && ds.Tables[0].Rows[0]["Disposable"].ToString() != "")
                {
                    model.Disposable = ds.Tables[0].Rows[0]["Disposable"].ToString();
                }
                if (ds.Tables[0].Rows[0]["GrantDoorMsg"] != null && ds.Tables[0].Rows[0]["GrantDoorMsg"].ToString() != "")
                {
                    model.GrantDoorMsg = ds.Tables[0].Rows[0]["GrantDoorMsg"].ToString();
                }
                if (ds.Tables[0].Rows[0]["GrantElevatorMsg"] != null && ds.Tables[0].Rows[0]["GrantElevatorMsg"].ToString() != "")
                {
                    model.GrantElevatorMsg = ds.Tables[0].Rows[0]["GrantElevatorMsg"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象List
        /// </summary>
        public List<M_Card_Info> GetListByVisitNo(string visitNo)
        {
            DataSet ds = null;

            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select id,CardId,UseStatus,CardType,VisitNoNow,StartDate,EndDate,Disposable,GrantDoorMsg,GrantElevatorMsg from F_Card_Info ");
                strSql.Append(" where VisitNoNow=@VisitNoNow order by StartDate desc");
                SqlParameter[] parameters = {
					new SqlParameter("@VisitNoNow", SqlDbType.VarChar,30)
                                        };
                parameters[0].Value = visitNo;

                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select id,CardId,UseStatus,CardType,VisitNoNow,StartDate,EndDate,Disposable,GrantDoorMsg,GrantElevatorMsg from F_Card_Info ");
                strSql.Append(" where VisitNoNow=@VisitNoNow order by StartDate desc");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@VisitNoNow", DbType.StringFixedLength,256)
                                        };
                parameters[0].Value = visitNo;

                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            List<M_Card_Info> cardList = new List<M_Card_Info>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Model.M_Card_Info model = new Model.M_Card_Info();
                    if (row["id"] != null && row["id"].ToString() != "")
                    {
                        model.id = int.Parse(row["id"].ToString());
                    }
                    if (row["CardId"] != null && row["CardId"].ToString() != "")
                    {
                        model.CardId = row["CardId"].ToString();
                    }
                    if (row["UseStatus"] != null && row["UseStatus"].ToString() != "")
                    {
                        model.UseStatus = row["UseStatus"].ToString();
                    }
                    if (row["CardType"] != null && row["CardType"].ToString() != "")
                    {
                        model.CardType = row["CardType"].ToString();
                    }
                    if (row["VisitNoNow"] != null && row["VisitNoNow"].ToString() != "")
                    {
                        model.VisitNoNow = row["VisitNoNow"].ToString();
                    }
                    if (row["StartDate"] != null && row["StartDate"].ToString() != "")
                    {
                        model.StartDate = DateTime.Parse(row["StartDate"].ToString());
                    }
                    if (row["EndDate"] != null && row["EndDate"].ToString() != "")
                    {
                        model.EndDate = DateTime.Parse(row["EndDate"].ToString());
                    }
                    if (row["Disposable"] != null && row["Disposable"].ToString() != "")
                    {
                        model.Disposable = row["Disposable"].ToString();
                    }
                    if (row["GrantDoorMsg"] != null && row["GrantDoorMsg"].ToString() != "")
                    {
                        model.GrantDoorMsg = row["GrantDoorMsg"].ToString();
                    }
                    if (row["GrantElevatorMsg"] != null && row["GrantElevatorMsg"].ToString() != "")
                    {
                        model.GrantElevatorMsg = row["GrantElevatorMsg"].ToString();
                    }
                    cardList.Add(model);
                }
            }
            return cardList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,CardId,UseStatus,CardType,VisitNoNow,StartDate,EndDate,Disposable,GrantDoorMsg");
            strSql.Append(" FROM F_Card_Info ");
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
                return new PostgreHelper().ExecuteQuery(DAL.DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select ");
                if (Top > 0)
                {
                    strSql.Append(" top " + Top.ToString());
                }
                strSql.Append(" id,CardId,UseStatus,CardType,VisitNoNow,StartDate,EndDate,Disposable ");
                strSql.Append(" FROM F_Card_Info ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                strSql.Append(" order by " + filedOrder);
                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select ");

                strSql.Append(" id,CardId,UseStatus,CardType,VisitNoNow,StartDate,EndDate,Disposable ");
                strSql.Append(" FROM F_Card_Info ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                strSql.Append(" order by " + filedOrder);
                if (Top > 0)
                {
                    strSql.Append(" limit " + Top.ToString());
                }
                return new PostgreHelper().ExecuteQuery(DAL.DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

        /// <summary>
        /// 获取过期卡
        /// </summary>
        /// <returns></returns>
        public DataSet GetOverdueList()
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select * from f_card_info ");
                strSql.Append(" where enddate <getdate()");

                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select * from f_card_info ");
                strSql.Append(" where enddate <localtimestamp(0)");

                return new PostgreHelper().ExecuteQuery(DAL.DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

        #endregion  Method
    }
}
