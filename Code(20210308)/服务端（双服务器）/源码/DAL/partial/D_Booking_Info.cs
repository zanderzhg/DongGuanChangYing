using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace ADServer.DAL
{
    public partial class D_Booking_Info
    {
        public int Add_API(Model.M_Booking_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_Booking_Info(");
                strSql.Append("BookNo,BookName,BookSex,BookTel,BookDate,BookDateEnd,BookReason,BookBelongs,BookNum,BookMenu,CertKindName,CertNumber,EmpNo,BookFlag,BookTime,Operter,IsMainVisitor,ExamineHandler,BookRoleFlag,QRCode,LicensePlate,BookTimeStart,BookCompany,emptel)");
                strSql.Append(" values (");
                strSql.Append("@BookNo,@BookName,@BookSex,@BookTel,@BookDate,@BookDateEnd,@BookReason,@BookBelongs,@BookNum,@BookMenu,@CertKindName,@CertNumber,@EmpNo,@BookFlag,@BookTime,@Operter,@IsMainVisitor,@ExamineHandler,@BookRoleFlag,@QRCode,@LicensePlate,@BookTimeStart,@BookCompany,@emptel)");
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
                strSql.Append("BookNo,BookName,BookSex,BookTel,BookDate,BookDateEnd,BookReason,BookBelongs,BookNum,BookMenu,CertKindName,CertNumber,EmpNo,BookFlag,BookTime,Operter,IsMainVisitor,ExamineHandler,BookRoleFlag,QRCode,LicensePlate,BookTimeStart,BookCompany,emptel)");
                strSql.Append(" values (");
                strSql.Append("@BookNo,@BookName,@BookSex,@BookTel,@BookDate,@BookDateEnd,@BookReason,@BookBelongs,@BookNum,@BookMenu,@CertKindName,@CertNumber,@EmpNo,@BookFlag,@BookTime,@Operter,@IsMainVisitor,@ExamineHandler,@BookRoleFlag,@QRCode,@LicensePlate,@BookTimeStart,@BookCompany,@emptel)");
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

                int rows = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                return rows;
            }
        }
        public int Delete_API(string bookno)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from F_Booking_Info where bookno=@bookno");
            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@bookno", SqlDbType.VarChar,20),
            };
                parameters[0].Value = bookno;


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
					new NpgsqlParameter("@bookno", DbType.StringFixedLength,20),
            };
                parameters[0].Value = bookno;

                int rows = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                return rows;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update_API(Model.M_Booking_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update F_Booking_Info set ");
            strSql.Append("BookNo=@BookNo,");
            strSql.Append("BookName=@BookName,");
            strSql.Append("BookSex=@BookSex,");
            strSql.Append("BookTel=@BookTel,");
            strSql.Append("BookDate=@BookDate,");
            strSql.Append("BookReason=@BookReason,");
            strSql.Append("BookBelongs=@BookBelongs,");
            strSql.Append("BookNum=@BookNum,");
            strSql.Append("BookMenu=@BookMenu,");
            strSql.Append("CertKindName=@CertKindName,");
            strSql.Append("CertNumber=@CertNumber,");
            strSql.Append("EmpNo=@EmpNo,");
            strSql.Append("BookFlag=@BookFlag,");
            strSql.Append("Emptel=@Emptel,");
            strSql.Append("BookTimeStart=@BookTimeStart,");
            strSql.Append("BookDateEnd=@BookDateEnd,");
            strSql.Append("QRCode=@QRCode");
            strSql.Append(" where bookno=@bookno and bookflag = 0");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@BookNo", SqlDbType.VarChar,20),
					new SqlParameter("@BookName", SqlDbType.VarChar,30),
					new SqlParameter("@BookSex", SqlDbType.VarChar,4),
					new SqlParameter("@BookTel", SqlDbType.VarChar,20),
					new SqlParameter("@BookDate", SqlDbType.DateTime),
					new SqlParameter("@BookReason", SqlDbType.VarChar,20),
					new SqlParameter("@BookBelongs", SqlDbType.VarChar,100),
					new SqlParameter("@BookNum", SqlDbType.Int,4),
					new SqlParameter("@BookMenu", SqlDbType.VarChar,1000),
					new SqlParameter("@CertKindName", SqlDbType.VarChar,30),
					new SqlParameter("@CertNumber", SqlDbType.VarChar,30),
					new SqlParameter("@EmpNo", SqlDbType.Int,4),
					new SqlParameter("@BookFlag", SqlDbType.Int,4),
                    new SqlParameter("@QRCode", SqlDbType.VarChar,30),
					new SqlParameter("@bookno", SqlDbType.VarChar,20),
                    new SqlParameter("@Emptel", SqlDbType.VarChar,20),
                    new SqlParameter("@BookTimeStart", SqlDbType.DateTime),
                    new SqlParameter("@BookDateEnd", SqlDbType.DateTime),
                                            };
                parameters[0].Value = model.BookNo;
                parameters[1].Value = model.BookName;
                parameters[2].Value = model.BookSex;
                parameters[3].Value = model.BookTel;
                parameters[4].Value = model.BookDate;
                parameters[5].Value = model.BookReason;
                parameters[6].Value = model.BookBelongs;
                parameters[7].Value = model.BookNum.Value;
                parameters[8].Value = model.BookMenu;
                parameters[9].Value = model.CertKindName;
                parameters[10].Value = model.CertNumber;
                parameters[11].Value = model.EmpNo;
                parameters[12].Value = model.BookFlag;
                parameters[13].Value = model.QRCode;
                parameters[14].Value = model.BookNo;
                parameters[15].Value = model.Emptel;
                parameters[16].Value = model.BookTimeStart;
                parameters[17].Value = model.ValidTimeEnd;

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
					new NpgsqlParameter("@BookNo", DbType.StringFixedLength,20),
					new NpgsqlParameter("@BookName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@BookSex", DbType.StringFixedLength,4),
					new NpgsqlParameter("@BookTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@BookDate", DbType.DateTime),
					new NpgsqlParameter("@BookReason", DbType.StringFixedLength,20),
					new NpgsqlParameter("@BookBelongs", DbType.StringFixedLength,100),
					new NpgsqlParameter("@BookNum", DbType.Int32,4),
					new NpgsqlParameter("@BookMenu", DbType.StringFixedLength,1000),
					new NpgsqlParameter("@CertKindName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@CertNumber", DbType.StringFixedLength,30),
					new NpgsqlParameter("@EmpNo", DbType.Int32,4),
					new NpgsqlParameter("@BookFlag", DbType.Int32,4),
                    new NpgsqlParameter("@QRCode", DbType.StringFixedLength,30),
					new NpgsqlParameter("@bookno", DbType.StringFixedLength,20),
                    new NpgsqlParameter("@Emptel", DbType.StringFixedLength,20),
                    new NpgsqlParameter("@BookTimeStart", DbType.DateTime),
                    new NpgsqlParameter("@BookDateEnd", DbType.DateTime),
                                               };
                parameters[0].Value = model.BookNo;
                parameters[1].Value = model.BookName;
                parameters[2].Value = model.BookSex;
                parameters[3].Value = model.BookTel;
                parameters[4].Value = model.BookDate;
                parameters[5].Value = model.BookReason;
                parameters[6].Value = model.BookBelongs;
                parameters[7].Value = model.BookNum.Value;
                parameters[8].Value = model.BookMenu;
                parameters[9].Value = model.CertKindName;
                parameters[10].Value = model.CertNumber;
                parameters[11].Value = model.EmpNo;
                parameters[12].Value = model.BookFlag;
                parameters[13].Value = model.QRCode;
                parameters[14].Value = model.BookNo;
                parameters[15].Value = model.Emptel;
                parameters[16].Value = model.BookTimeStart;
                parameters[17].Value = model.ValidTimeEnd;

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
        /// 得到预约的详细信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="lines"> 每页显示的多少条</param>
        /// <returns></returns>
        public DataSet GetBookingInfo_API(string strWhere, int pageIndex, int lines)
        {
            int pageSize = (pageIndex - 1) * lines;

            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select bookno,bookname,booksex,certkindname,certnumber,booktel,bookdate,bookreason,bookbelongs,empname,emptel,booknum,bookmenu,bookflag,bookcompany,licenseplate, booktimestart,bookdateend from ( ");
                strSql.Append(" select a.bookno,a.bookname,a.booksex,a.certkindname,a.certnumber,");
                strSql.Append(" a.booktel,CONVERT(varchar(100), a.bookdate, 120) as bookdate,a.bookreason,a.bookbelongs,b.empname,a.emptel,a.booknum,a.bookmenu");//when 0 then '已预约' when 1 then '已到访' when -11 then '已到访' else '' end as '来访状态'
                strSql.Append(",a.bookflag,a.bookcompany,a.licenseplate,CONVERT(varchar(100), a.booktimestart, 120) as booktimestart,CONVERT(varchar(100), a.bookdateend, 120) as bookdateend, ROW_NUMBER() OVER(Order by id) AS RowId from f_booking_info a left join f_employ_info b");
                strSql.Append(" on a.empno = b.empno where (bookflag = 0 or bookflag = 1 or bookflag = -11)");
                if (strWhere != "")
                    strSql.Append(strWhere);

                strSql.Append(" ) as booking ");
                strSql.Append(" where RowId between " + pageSize + " and " + pageIndex * lines);

                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select a.bookno,a.bookname,a.booksex,a.certkindname,a.certnumber,");
                strSql.Append(" a.booktel,to_char(a.bookdate, 'yyyy-MM-dd HH24:MI:SS') as bookdate,a.bookreason,a.bookbelongs,b.empname,a.emptel,a.booknum,a.bookmenu");// then '已预约' when a.bookflag=1 then '已到访' when a.bookflag=-11 then '已到访' else '' end as 来访状态
                strSql.Append(",a.bookflag,a.bookcompany,a.licenseplate,to_char(a.booktimestart, 'yyyy-MM-dd HH24:MI:SS') as booktimestart,to_char(a.bookdateend, 'yyyy-MM-dd HH24:MI:SS') as bookdateend from f_booking_info a left join f_employ_info b");
                strSql.Append(" on a.empno = b.empno where (bookflag = 0 or bookflag = 1 or bookflag = -11)");

                if (strWhere != "")
                    strSql.Append(strWhere);

                strSql.Append(" order by a.id asc limit " + lines + " offset " + pageSize);

                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

    }
}
