using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Npgsql;
using NpgsqlTypes;

namespace ADServer.DAL
{
    public partial class D_Booking_Info
    {
        public D_Booking_Info()
        { }

        public Model.M_Booking_Info GetModelByQRCode(string qrCode, int bookflag)
        {
            Model.M_Booking_Info model = new Model.M_Booking_Info();
            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 id,BookNo,BookName,BookSex,BookTel,BookDate,BookDateEnd,BookReason,BookBelongs,BookNum,BookMenu,CertKindName,CertNumber,EmpNo,BookFlag,QRCode from F_Booking_Info ");
                strSql.Append(" where QRCode=@QRCode and bookflag=" + bookflag + " order by id desc");
                SqlParameter[] parameters = {
					new SqlParameter("@QRCode", SqlDbType.VarChar,30)
                                        };
                parameters[0].Value = qrCode;

                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select id,BookNo,BookName,BookSex,BookTel,BookDate,BookDateEnd,BookReason,BookBelongs,BookNum,BookMenu,CertKindName,CertNumber,EmpNo,BookFlag,QRCode from F_Booking_Info ");
                strSql.Append(" where QRCode=@QRCode and bookflag=" + bookflag + " order by id desc limit 1 ");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@QRCode", DbType.StringFixedLength,30)
                                        };
                parameters[0].Value = qrCode;

                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BookNo"] != null && ds.Tables[0].Rows[0]["BookNo"].ToString() != "")
                {
                    model.BookNo = ds.Tables[0].Rows[0]["BookNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BookName"] != null && ds.Tables[0].Rows[0]["BookName"].ToString() != "")
                {
                    model.BookName = ds.Tables[0].Rows[0]["BookName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BookSex"] != null && ds.Tables[0].Rows[0]["BookSex"].ToString() != "")
                {
                    model.BookSex = ds.Tables[0].Rows[0]["BookSex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BookTel"] != null && ds.Tables[0].Rows[0]["BookTel"].ToString() != "")
                {
                    model.BookTel = ds.Tables[0].Rows[0]["BookTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BookDate"] != null && ds.Tables[0].Rows[0]["BookDate"].ToString() != "")
                {
                    model.BookDate = DateTime.Parse(ds.Tables[0].Rows[0]["BookDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BookReason"] != null && ds.Tables[0].Rows[0]["BookReason"].ToString() != "")
                {
                    model.BookReason = ds.Tables[0].Rows[0]["BookReason"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BookBelongs"] != null && ds.Tables[0].Rows[0]["BookBelongs"].ToString() != "")
                {
                    model.BookBelongs = ds.Tables[0].Rows[0]["BookBelongs"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BookNum"] != null && ds.Tables[0].Rows[0]["BookNum"].ToString() != "")
                {
                    model.BookNum = int.Parse(ds.Tables[0].Rows[0]["BookNum"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BookMenu"] != null && ds.Tables[0].Rows[0]["BookMenu"].ToString() != "")
                {
                    model.BookMenu = ds.Tables[0].Rows[0]["BookMenu"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CertKindName"] != null && ds.Tables[0].Rows[0]["CertKindName"].ToString() != "")
                {
                    model.CertKindName = ds.Tables[0].Rows[0]["CertKindName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CertNumber"] != null && ds.Tables[0].Rows[0]["CertNumber"].ToString() != "")
                {
                    model.CertNumber = ds.Tables[0].Rows[0]["CertNumber"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpNo"] != null && ds.Tables[0].Rows[0]["EmpNo"].ToString() != "")
                {
                    model.EmpNo = int.Parse(ds.Tables[0].Rows[0]["EmpNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BookFlag"] != null && ds.Tables[0].Rows[0]["BookFlag"].ToString() != "")
                {
                    model.BookFlag = int.Parse(ds.Tables[0].Rows[0]["BookFlag"].ToString());
                }
                if (ds.Tables[0].Rows[0]["QRCode"] != null && ds.Tables[0].Rows[0]["QRCode"].ToString() != "")
                {
                    model.QRCode = ds.Tables[0].Rows[0]["QRCode"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BookDate"] != null && ds.Tables[0].Rows[0]["BookDate"].ToString() != "")
                {
                    model.ValidTimeStart = DateTime.Parse(ds.Tables[0].Rows[0]["BookDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BookDateEnd"] != null && ds.Tables[0].Rows[0]["BookDateEnd"].ToString() != "")
                {
                    model.ValidTimeEnd = DateTime.Parse(ds.Tables[0].Rows[0]["BookDateEnd"].ToString());
                }
                model.BookType = 3;
                return model;
            }
            else
            {
                return null;
            }
        }

        public List<Model.M_Booking_Info> GetNotifyEmpRecords()
        {
            List<Model.M_Booking_Info> noticeList = new List<Model.M_Booking_Info>();
            DataSet ds = null;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Bookno,BookName,BookTel,Empno,Qrcode,winxinid from F_Booking_Info where notifyemp=1 order by id desc");
            if (DbHelperSQL.DbType == 1)
            {
                ds = DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Model.M_Booking_Info model = new Model.M_Booking_Info();
                    if (row["Bookno"] != null && row["Bookno"].ToString() != "")
                    {
                        model.BookNo = row["Bookno"].ToString();
                    }
                    if (row["BookName"] != null && row["BookName"].ToString() != "")
                    {
                        model.BookName = row["BookName"].ToString();
                    }
                    if (row["BookTel"] != null && row["BookTel"].ToString() != "")
                    {
                        model.BookTel = row["BookTel"].ToString();
                    }
                    if (row["Qrcode"] != null && row["Qrcode"].ToString() != "")
                    {
                        model.QRCode = row["Qrcode"].ToString();
                    }
                    if (row["EmpNo"] != null && row["EmpNo"].ToString() != "")
                    {
                        model.EmpNo = int.Parse(row["EmpNo"].ToString());
                        Model.M_Employ_Info emp = new BLL.B_Employ_Info().GetModel((int)model.EmpNo);
                        if (emp != null)
                        {
                            model.Empname = emp.EmpName;
                            model.Emptel = emp.EmpMobile;
                            model.BookType = 3;
                        }
                    }
                    if (row["winxinid"] != null && row["winxinid"].ToString() != "")
                    {
                        model.WeiXinId = Convert.ToInt32(row["winxinid"]);
                    }

                    noticeList.Add(model);
                }
            }

            return noticeList;
        }

        /// <summary>
        /// 根据预约号，更新预约标识
        /// </summary>
        /// <param name="bookno"></param>
        /// <param name="newflag"></param>
        public void UpdateBookFlag(string bookno, int newflag)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update f_booking_info ");
            strSql.Append(" set bookflag = " + newflag);
            strSql.Append(" where bookno = '" + bookno + "'");

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
        /// 根据预约号，更新通知被防人标识
        /// </summary>
        /// <param name="bookno"></param>
        /// <param name="newflag"></param>
        public void UpdateNotifyEmp(string bookno, int newflag)
        {
            StringBuilder strSqlNotifyEmp = new StringBuilder();
            strSqlNotifyEmp.Append(" update f_booking_info ");
            strSqlNotifyEmp.Append(" set notifyemp = 0");
            strSqlNotifyEmp.Append(" where bookno = '" + bookno + "'");

            if (DbHelperSQL.DbType == 1)
            {
                DbHelperSQL.ExecuteSql(strSqlNotifyEmp.ToString());
            }
            else
            {
                new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSqlNotifyEmp.ToString());
            }
        }

        /// <summary>
        /// 根据预约号，更新通知被防人标识
        /// </summary>
        /// <param name="bookno"></param>
        /// <param name="newflag"></param>
        public void UpdateNotifyEmpByIdCard(string cardId, int newflag)
        {
            StringBuilder strSqlNotifyEmp = new StringBuilder();
            strSqlNotifyEmp.Append(" update f_booking_info ");
            strSqlNotifyEmp.Append(" set notifyemp = " + newflag);
            strSqlNotifyEmp.Append(" where Qrcode = '" + cardId + "'");

            if (DbHelperSQL.DbType == 1)
            {
                DbHelperSQL.ExecuteSql(strSqlNotifyEmp.ToString());
            }
            else
            {
                new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSqlNotifyEmp.ToString());
            }
        }

        /// <summary>
        /// 返回预约号
        /// </summary>
        /// <returns></returns>
        public string GetBookNo()
        {
            if (DbHelperSQL.DbType == 1)
            {
                using (SqlConnection connection = DataBase.conn())
                {
                    connection.Open();
                    SqlCommand MyCommand = new SqlCommand("proc_get_bookno", connection);
                    MyCommand.CommandType = CommandType.StoredProcedure;

                    MyCommand.Parameters.Add(new SqlParameter("@bookno", SqlDbType.VarChar, 20));
                    MyCommand.Parameters["@bookno"].Direction = ParameterDirection.Output;

                    MyCommand.ExecuteNonQuery();
                    string nr = Convert.ToString(MyCommand.Parameters["@bookno"].Value);
                    return nr;
                }
            }
            else
            {
                DataSet ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.StoredProcedure, "proc_get_bookno", null);
                return ds.Tables[0].Rows[0][0].ToString();
            }

        }

        public Model.M_Booking_Info GetModelByPlateNumber(string plateNumber, int bookflag)
        {
            Model.M_Booking_Info model = new Model.M_Booking_Info();
            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 id,BookNo,BookName,BookSex,BookTel,BookDate,BookDateEnd,BookReason,BookBelongs,BookNum,BookMenu,CertKindName,CertNumber,EmpNo,BookFlag,QRCode from F_Booking_Info ");
                strSql.Append(" where licenseplate=@licenseplate and bookflag=" + bookflag + " order by id desc");
                SqlParameter[] parameters = {
					new SqlParameter("@licenseplate", SqlDbType.VarChar,32)
                                        };
                parameters[0].Value = plateNumber;

                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select id,BookNo,BookName,BookSex,BookTel,BookDate,BookDateEnd,BookReason,BookBelongs,BookNum,BookMenu,CertKindName,CertNumber,EmpNo,BookFlag,QRCode from F_Booking_Info ");
                strSql.Append(" where licenseplate=@licenseplate and bookflag=" + bookflag + " order by id desc limit 1 ");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@licenseplate", DbType.StringFixedLength,32)
                                        };
                parameters[0].Value = plateNumber;

                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BookNo"] != null && ds.Tables[0].Rows[0]["BookNo"].ToString() != "")
                {
                    model.BookNo = ds.Tables[0].Rows[0]["BookNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BookName"] != null && ds.Tables[0].Rows[0]["BookName"].ToString() != "")
                {
                    model.BookName = ds.Tables[0].Rows[0]["BookName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BookSex"] != null && ds.Tables[0].Rows[0]["BookSex"].ToString() != "")
                {
                    model.BookSex = ds.Tables[0].Rows[0]["BookSex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BookTel"] != null && ds.Tables[0].Rows[0]["BookTel"].ToString() != "")
                {
                    model.BookTel = ds.Tables[0].Rows[0]["BookTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BookDate"] != null && ds.Tables[0].Rows[0]["BookDate"].ToString() != "")
                {
                    model.BookDate = DateTime.Parse(ds.Tables[0].Rows[0]["BookDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BookReason"] != null && ds.Tables[0].Rows[0]["BookReason"].ToString() != "")
                {
                    model.BookReason = ds.Tables[0].Rows[0]["BookReason"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BookBelongs"] != null && ds.Tables[0].Rows[0]["BookBelongs"].ToString() != "")
                {
                    model.BookBelongs = ds.Tables[0].Rows[0]["BookBelongs"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BookNum"] != null && ds.Tables[0].Rows[0]["BookNum"].ToString() != "")
                {
                    model.BookNum = int.Parse(ds.Tables[0].Rows[0]["BookNum"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BookMenu"] != null && ds.Tables[0].Rows[0]["BookMenu"].ToString() != "")
                {
                    model.BookMenu = ds.Tables[0].Rows[0]["BookMenu"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CertKindName"] != null && ds.Tables[0].Rows[0]["CertKindName"].ToString() != "")
                {
                    model.CertKindName = ds.Tables[0].Rows[0]["CertKindName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CertNumber"] != null && ds.Tables[0].Rows[0]["CertNumber"].ToString() != "")
                {
                    model.CertNumber = ds.Tables[0].Rows[0]["CertNumber"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpNo"] != null && ds.Tables[0].Rows[0]["EmpNo"].ToString() != "")
                {
                    model.EmpNo = int.Parse(ds.Tables[0].Rows[0]["EmpNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BookFlag"] != null && ds.Tables[0].Rows[0]["BookFlag"].ToString() != "")
                {
                    model.BookFlag = int.Parse(ds.Tables[0].Rows[0]["BookFlag"].ToString());
                }
                if (ds.Tables[0].Rows[0]["QRCode"] != null && ds.Tables[0].Rows[0]["QRCode"].ToString() != "")
                {
                    model.QRCode = ds.Tables[0].Rows[0]["QRCode"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BookDate"] != null && ds.Tables[0].Rows[0]["BookDate"].ToString() != "")
                {
                    model.ValidTimeStart = DateTime.Parse(ds.Tables[0].Rows[0]["BookDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BookDateEnd"] != null && ds.Tables[0].Rows[0]["BookDateEnd"].ToString() != "")
                {
                    model.ValidTimeEnd = DateTime.Parse(ds.Tables[0].Rows[0]["BookDateEnd"].ToString());
                }
                model.BookType = 3;
                return model;
            }
            else
            {
                return null;
            }
        }

        public int Add(Model.M_Booking_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_Booking_Info(");
                strSql.Append("BookNo,BookName,BookSex,BookTel,BookDate,BookDateEnd,BookReason,BookBelongs,BookNum,BookMenu,CertKindName,CertNumber,EmpNo,BookFlag,BookTime,Operter,IsMainVisitor,ExamineHandler,BookRoleFlag,QRCode,LicensePlate,BookTimeStart,BookCompany,emptel,winxinid,quyu,area)");
                strSql.Append(" values (");
                strSql.Append("@BookNo,@BookName,@BookSex,@BookTel,@BookDate,@BookDateEnd,@BookReason,@BookBelongs,@BookNum,@BookMenu,@CertKindName,@CertNumber,@EmpNo,@BookFlag,@BookTime,@Operter,@IsMainVisitor,@ExamineHandler,@BookRoleFlag,@QRCode,@LicensePlate,@BookTimeStart,@BookCompany,@emptel,@winxinid,@quyu,@area)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@BookNo", SqlDbType.VarChar,20),
					new SqlParameter("@BookName", SqlDbType.VarChar,30),
					new SqlParameter("@BookSex", SqlDbType.VarChar,4),
					new SqlParameter("@BookTel", SqlDbType.VarChar,20),
					new SqlParameter("@BookDate", SqlDbType.DateTime),
                    new SqlParameter("@BookDateEnd", SqlDbType.DateTime),
					new SqlParameter("@BookReason", SqlDbType.VarChar,20),
					new SqlParameter("@BookBelongs", SqlDbType.VarChar,100),
					new SqlParameter("@BookNum", SqlDbType.Int,4),
					new SqlParameter("@BookMenu", SqlDbType.VarChar,1000),
					new SqlParameter("@CertKindName", SqlDbType.VarChar,30),
					new SqlParameter("@CertNumber", SqlDbType.VarChar,30),
					new SqlParameter("@EmpNo", SqlDbType.Int,4),
					new SqlParameter("@BookFlag", SqlDbType.Int,4),
                    new SqlParameter("@BookTime",SqlDbType.DateTime),
                    new SqlParameter("@Operter",SqlDbType.Int),
                    new SqlParameter("@IsMainVisitor",SqlDbType.Int),
                    new SqlParameter("@ExamineHandler",SqlDbType.Int),
                    new SqlParameter("@BookRoleFlag", SqlDbType.Int,4),
                    new SqlParameter("@QRCode", SqlDbType.VarChar,30),
                    new SqlParameter("@LicensePlate", SqlDbType.VarChar,32),
                    new SqlParameter("@BookTimeStart",SqlDbType.DateTime),
                    new SqlParameter("@BookCompany", SqlDbType.VarChar,30),
                    new SqlParameter("@emptel", SqlDbType.VarChar,20),
                    new SqlParameter("@winxinid",SqlDbType.Int),
                    new SqlParameter("@quyu", SqlDbType.VarChar,32),
                    new SqlParameter("@area", SqlDbType.VarChar,32)
            };
                parameters[0].Value = model.BookNo;
                parameters[1].Value = model.BookName;
                parameters[2].Value = model.BookSex;
                parameters[3].Value = model.BookTel;
                parameters[4].Value = model.BookDate;
                parameters[5].Value = model.ValidTimeEnd;
                parameters[6].Value = model.BookReason;
                parameters[7].Value = model.BookBelongs;
                parameters[8].Value = model.BookNum;
                parameters[9].Value = model.BookMenu;
                parameters[10].Value = model.CertKindName;
                parameters[11].Value = model.CertNumber;
                parameters[12].Value = model.EmpNo;
                parameters[13].Value = model.BookFlag;
                parameters[14].Value = model.BookTime;
                parameters[15].Value = model.Operter;
                parameters[16].Value = 0;
                parameters[17].Value = 0;
                parameters[18].Value = 1;
                parameters[19].Value = model.QRCode;
                parameters[20].Value = model.LicensePlate;
                parameters[21].Value = model.BookTimeStart;
                parameters[22].Value = model.VisitorCompany;
                parameters[23].Value = model.Emptel;
                parameters[24].Value = model.id;
                parameters[25].Value = model.quyu;
                parameters[26].Value = model.area;
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
                strSql.Append("insert into F_Booking_Info(");
                strSql.Append("BookNo,BookName,BookSex,BookTel,BookDate,BookDateEnd,BookReason,BookBelongs,BookNum,BookMenu,CertKindName,CertNumber,EmpNo,BookFlag,BookTime,Operter,IsMainVisitor,ExamineHandler,BookRoleFlag,QRCode,LicensePlate,BookTimeStart,BookCompany,emptel,winxinid,quyu,area)");
                strSql.Append(" values (");
                strSql.Append("@BookNo,@BookName,@BookSex,@BookTel,@BookDate,@BookDateEnd,@BookReason,@BookBelongs,@BookNum,@BookMenu,@CertKindName,@CertNumber,@EmpNo,@BookFlag,@BookTime,@Operter,@IsMainVisitor,@ExamineHandler,@BookRoleFlag,@QRCode,@LicensePlate,@BookTimeStart,@BookCompany,@emptel,@winxinid,@quyu,@area)");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@BookNo", DbType.StringFixedLength,20),
					new NpgsqlParameter("@BookName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@BookSex", DbType.StringFixedLength,4),
					new NpgsqlParameter("@BookTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@BookDate", DbType.DateTime),
                    new NpgsqlParameter("@BookDateEnd", DbType.DateTime),
					new NpgsqlParameter("@BookReason", DbType.StringFixedLength,20),
					new NpgsqlParameter("@BookBelongs", DbType.StringFixedLength,100),
					new NpgsqlParameter("@BookNum", DbType.Int32,4),
					new NpgsqlParameter("@BookMenu", DbType.StringFixedLength,1000),
					new NpgsqlParameter("@CertKindName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@CertNumber", DbType.StringFixedLength,30),
					new NpgsqlParameter("@EmpNo", DbType.Int32,4),
					new NpgsqlParameter("@BookFlag", DbType.Int32,4),
                    new NpgsqlParameter("@BookTime",DbType.DateTime),
                    new NpgsqlParameter("@Operter",DbType.Int32),
                    new NpgsqlParameter("@IsMainVisitor",NpgsqlDbType.Bit),
                    new NpgsqlParameter("@ExamineHandler",DbType.Int32),
                    new NpgsqlParameter("@BookRoleFlag",DbType.Int32),
                    new NpgsqlParameter("@QRCode", DbType.StringFixedLength,30),
                    new NpgsqlParameter("@LicensePlate", DbType.StringFixedLength,32),
                    new NpgsqlParameter("@BookTimeStart", DbType.DateTime),
                    new NpgsqlParameter("@BookCompany",DbType.StringFixedLength,30),
                    new NpgsqlParameter("@emptel", DbType.StringFixedLength,20),
                    new NpgsqlParameter("@winxinid",DbType.Int32),
                    new NpgsqlParameter("@quyu", DbType.StringFixedLength,32),
                    new NpgsqlParameter("@area", DbType.StringFixedLength,32)
            };
                parameters[0].Value = model.BookNo;
                parameters[1].Value = model.BookName;
                parameters[2].Value = model.BookSex;
                parameters[3].Value = model.BookTel;
                parameters[4].Value = model.BookDate;
                parameters[5].Value = model.ValidTimeEnd;
                parameters[6].Value = model.BookReason;
                parameters[7].Value = model.BookBelongs;
                parameters[8].Value = model.BookNum;
                parameters[9].Value = model.BookMenu;
                parameters[10].Value = model.CertKindName;
                parameters[11].Value = model.CertNumber;
                parameters[12].Value = model.EmpNo;
                parameters[13].Value = model.BookFlag;
                parameters[14].Value = model.BookTime;
                parameters[15].Value = model.Operter;
                parameters[16].Value = 0;
                parameters[17].Value = 0;
                parameters[18].Value = 1;
                parameters[19].Value = model.QRCode;
                parameters[20].Value = model.LicensePlate;
                parameters[21].Value = model.BookTimeStart;
                parameters[22].Value = model.VisitorCompany;
                parameters[23].Value = model.Emptel;
                parameters[24].Value = model.id;
                parameters[25].Value = model.quyu;
                parameters[26].Value = model.area;
                int rows = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                return rows;
            }
        }

    }
}
