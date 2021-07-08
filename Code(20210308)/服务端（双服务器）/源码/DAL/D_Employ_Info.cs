using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ADServer.BLL;
using System.Web.Security;
using Npgsql;
using NpgsqlTypes;

namespace ADServer.DAL
{
    public partial class D_Employ_Info
    {
        public D_Employ_Info()
        { }

        #region 自定义方法

        /// <summary>
        /// 根据员工编号，删除该员工确认账号、来访信息
        /// </summary>
        /// <param name="empno"></param>
        public void deleteAllByEmpno(int empno)
        {
            StringBuilder strSql = new StringBuilder();

            ////删除来访信息
            //strSql.Length = 0;
            //strSql.Append(" delete from f_visitlist_info ");
            //strSql.Append(" where empno = " + empno.ToString());

            //if (DbHelperSQL.DbType == 1)
            //{
            //    DbHelperSQL.ExecuteSql(strSql.ToString());
            //}
            //else
            //{
            //    new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            //}


            //删除来访确认账号
            strSql.Length = 0;
            strSql.Append(" delete from emp where empno = " + empno.ToString());
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
        /// 得到员工信息模板
        /// </summary>
        public DataSet GetEmployCommon()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select companyname [单位名称(必填)],deptname as [部门名称(必填)],positionname as [职工职位],empname as [职工姓名(必填)],empsex as [性别],empfloor as [办公楼层],");
            strSql.Append(" emproomcode as [房间号],empmobile as [个人电话],emptel as [办公电话],empexttel as [分机号码],empcardno as [IC卡号],pwd as [预约网密码] ");
            strSql.Append(" from f_import_employ ");
            strSql.Append(" where 1=0 ");

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
        /// 存储过程处理导入员工数据
        /// </summary>
        /// <returns></returns>
        public string ProcDealImport()
        {
            if (DbHelperSQL.DbType == 1)
            {
                using (SqlConnection connection = DataBase.conn())
                {
                    connection.Open();
                    SqlCommand MyCommand = new SqlCommand("proc_deal_employer", connection);
                    MyCommand.CommandType = CommandType.StoredProcedure;

                    MyCommand.Parameters.Add(new SqlParameter("@ret", SqlDbType.VarChar, 100));
                    MyCommand.Parameters["@ret"].Direction = ParameterDirection.Output;

                    MyCommand.ExecuteNonQuery();
                    string nr = Convert.ToString(MyCommand.Parameters["@ret"].Value);
                    return nr;
                }
            }
            else
            {
                DataSet ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.StoredProcedure, "proc_deal_employer", null);
                return ds.Tables[0].Rows[0][0].ToString();
            }
        }

        /// <summary>
        /// 得到员工详细信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetListNew(int topNum, string strWhere)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top " + topNum);
                strSql.Append(" a.companyname,b.deptname,c.empposition,c.empname,c.empsex,");
                strSql.Append(" c.empfloor,c.emproomcode,c.empmobile,c.emptel,c.empexttel,c.empcardno,c.empcardenddate,a.companyid,b.deptno,c.empno,c.empmemu,c.empno ");
                strSql.Append(" from f_company_info a,f_department_info b,f_employ_info c ");
                strSql.Append(" where a.companyid = b.companyid and b.deptno = c.deptno and b.companyid = c.companyid ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" and " + strWhere);
                }

                strSql.Append(" order by c.empno ");
                //strSql.Append(" order by a.companyname,b.deptname,c.empname ");
                return DbHelperSQL.Query(strSql.ToString());

            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select a.companyname,b.deptname,c.empposition,c.empname,c.empsex,");
                strSql.Append(" c.empfloor,c.emproomcode,c.empmobile,c.emptel,c.empexttel,c.empcardno,c.empcardenddate,a.companyid,b.deptno,c.empno,c.empmemu,c.empno ");
                strSql.Append(" from f_company_info a,f_department_info b,f_employ_info c ");
                strSql.Append(" where a.companyid = b.companyid and b.deptno = c.deptno and b.companyid = c.companyid ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" and " + strWhere);
                }

                strSql.Append(" order by c.empno limit " + topNum);

                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }

        }

        public DataSet GetEmployBook(string empno)
        {
            string strSql = " select a.companyname,b.deptname,c.empname,c.empsex, c.empfloor,c.emproomcode,c.emptel,c.empmobile,c.empexttel,c.empposition,a.companyid,b.deptno,c.empno,a.CompanyName from f_company_info a,f_department_info b,f_employ_info c  where a.companyid = b.companyid and b.deptno = c.deptno and b.companyid = c.companyid  and  a.companyid = b.companyid and b.deptno = c.deptno and c.empno=" + empno + " order by a.companyname,b.deptname,c.empname  ";

            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Query(strSql);
            }
            else
            {
                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql);
            }
        }

        /// <summary>
        /// 得到员工详细信息  --开启来访确认功能
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetListByService(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.companyname,b.deptname,c.empname,c.empsex,");
            strSql.Append(" c.empfloor,c.emproomcode,c.emptel,c.empmobile,c.empexttel,c.empposition,case when e.Line=0 then '不在线' else '在线' end as line,a.companyid,b.deptno,c.empno,c.empmemu ");
            strSql.Append(" from f_company_info a,f_department_info b,f_employ_info c,EMP e  ");
            strSql.Append(" where a.companyid = b.companyid and b.deptno = c.deptno and b.companyid = c.companyid and e.empno=c.EmpNo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and " + strWhere);
            }

            strSql.Append(" order by a.companyname,b.deptname ");
            //strSql.Append(" order by a.companyname,b.deptname,c.empname ");

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
        /// 新增修改时，判断名字是否重复
        /// （同一公司，同一部门，姓名不能重复）
        /// </summary>
        /// <param name="belongName"></param>
        /// <returns></returns>
        public Boolean Exists(int companyid, int deptno, int empno, string empname)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * ");
            strSql.Append(" from f_company_info a,f_department_info b,f_employ_info c ");
            strSql.Append(" where a.companyid = b.companyid and b.deptno = c.deptno and b.companyid = c.companyid ");
            strSql.Append(" and a.companyid = " + companyid.ToString());
            strSql.Append(" and b.deptno = " + deptno.ToString());
            strSql.Append(" and c.empname = '" + empname.ToString() + "'");
            if (empno != 0)
                strSql.Append(" and c.empno <> " + empno.ToString());

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
        /// 判断手机号码是否存在
        /// </summary>
        /// <param name="empTel"></param>
        /// <returns></returns>
        public Boolean ExistTel(string empTel)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * ");
            strSql.Append(" from f_employ_info");
            strSql.Append(" where  EmpMobile='" + empTel + "'");
            //strSql.Append(" where  EmpMobile='" + empTel + "' and EmpNo<>'" + gEmployId + "'");

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
        /// 判断IC卡号是否存在
        /// </summary>
        /// <param name="empTel"></param>
        /// <returns></returns>
        public Boolean ExistICCardno(string cardno)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select empno ");
            strSql.Append(" from f_employ_info");
            strSql.Append(" where  EmpCardNo='" + cardno + "'");

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
        /// 判断是否存在同一手机号码
        /// </summary>
        /// <param name="empTel"></param>
        /// <returns></returns>
        public Boolean ExistTel(string empTel, string name)
        {
            if (DbHelperSQL.DbType == 1)
            {
                string checkNameSql = "select 1 from f_employ_info where EmpName='" + name + "'";
                object oPerson = DbHelperSQL.GetSingle(checkNameSql);
                if (oPerson == null)  //新加人员直接判断是否手机号码重复
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(" select * ");
                    strSql.Append(" from f_employ_info");
                    strSql.Append(" where  EmpMobile='" + empTel + "'");

                    return DbHelperSQL.Exists(strSql.ToString());
                }
                else
                {
                    string checkMyTelSql = "select EmpMobile from f_employ_info where EmpName='" + name + "'";
                    object oTel = DbHelperSQL.GetSingle(checkMyTelSql);
                    if (oTel != null)
                    {
                        string oldTel = oTel.ToString();
                        //if (oldTel != "")
                        //{
                        if (empTel == oldTel) //本人电话则不需判断
                        {
                            return false;
                        }
                        else
                        {
                            //判断新修改的电话号码是否已存在
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append(" select * ");
                            strSql.Append(" from f_employ_info");
                            strSql.Append(" where  EmpMobile='" + empTel + "'");

                            return DbHelperSQL.Exists(strSql.ToString());
                        }
                        //}
                        //return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                string checkNameSql = "select 1 from f_employ_info where EmpName='" + name + "'";
                object oPerson = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, checkNameSql);
                if (oPerson == null)  //新加人员直接判断是否手机号码重复
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(" select * ");
                    strSql.Append(" from f_employ_info");
                    strSql.Append(" where  EmpMobile='" + empTel + "'");

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
                else
                {
                    string checkMyTelSql = "select EmpMobile from f_employ_info where EmpName='" + name + "'";
                    object oTel = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, checkMyTelSql);
                    if (oTel != null)
                    {
                        string oldTel = oTel.ToString();
                        if (empTel == oldTel) //本人电话则不需判断
                        {
                            return false;
                        }
                        else
                        {
                            //判断新修改的电话号码是否已存在
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append(" select * ");
                            strSql.Append(" from f_employ_info");
                            strSql.Append(" where  EmpMobile='" + empTel + "'");

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
                    else
                    {
                        return false;
                    }
                }
            }

        }

        /// <summary>
        /// 判断是否存在同一手机号码
        /// </summary>
        /// <param name="empTel"></param>
        /// <returns></returns>
        public Boolean ExistTel(string empTel, int empNo)
        {
            if (DbHelperSQL.DbType == 1)
            {
                string checkNameSql = "select 1 from f_employ_info where EmpMobile='" + empTel + "' and EmpNo<>" + empNo;
                object oPerson = DbHelperSQL.GetSingle(checkNameSql);
                if (oPerson != null)  //判断是否手机号码重复
                {
                    return true;
                }
                return false;
            }
            else
            {
                string checkNameSql = "select 1 from f_employ_info where EmpMobile='" + empTel + "' and EmpNo<>" + empNo;
                object oPerson = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, checkNameSql);
                if (oPerson != null)  //判断是否手机号码重复
                {
                    return true;
                }
                return false;
            }

        }

        /// <summary>
        /// 判断是否存在同一手机号码
        /// </summary>
        /// <param name="empTel"></param>
        /// <returns></returns>
        public Boolean ExistICCardno(string cardno, string name)
        {
            if (DbHelperSQL.DbType == 1)
            {
                string checkNameSql = "select 1 from f_employ_info where EmpName='" + name + "'";
                object oPerson = DbHelperSQL.GetSingle(checkNameSql);
                if (oPerson == null)  //新加人员直接判断是否IC卡号重复
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(" select * ");
                    strSql.Append(" from f_employ_info");
                    strSql.Append(" where  EmpCardNo='" + cardno + "'");

                    return DbHelperSQL.Exists(strSql.ToString());
                }
                else
                {
                    string checkMyICSql = "select EmpCardNo from f_employ_info where EmpName='" + name + "'";
                    object oIC = DbHelperSQL.GetSingle(checkMyICSql);
                    if (oIC != null)
                    {
                        string oldIC = oIC.ToString();
                        //if (oldIC != "")
                        //{
                        if (cardno == oldIC) //本人IC卡号则不需判断
                        {
                            return false;
                        }
                        else
                        {
                            //判断新修改的IC卡号是否已存在
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append(" select * ");
                            strSql.Append(" from f_employ_info");
                            strSql.Append(" where  EmpCardNo='" + cardno + "'");

                            return DbHelperSQL.Exists(strSql.ToString());
                        }
                        //}
                        //return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                string checkNameSql = "select 1 from f_employ_info where EmpName='" + name + "'";
                object oPerson = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, checkNameSql);
                if (oPerson == null)  //新加人员直接判断是否IC卡号重复
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(" select * ");
                    strSql.Append(" from f_employ_info");
                    strSql.Append(" where  EmpCardNo='" + cardno + "'");

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
                else
                {
                    string checkMyICSql = "select EmpCardNo from f_employ_info where EmpName='" + name + "'";
                    object oIC = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, checkMyICSql);
                    if (oIC != null)
                    {
                        string oldIC = oIC.ToString();

                        if (cardno == oldIC) //本人IC卡号则不需判断
                        {
                            return false;
                        }
                        else
                        {
                            //判断新修改的IC卡号是否已存在
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append(" select * ");
                            strSql.Append(" from f_employ_info");
                            strSql.Append(" where  EmpCardNo='" + cardno + "'");

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
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 判断是否存在同一ic卡号码
        /// </summary>
        /// <param name="empTel"></param>
        /// <returns></returns>
        public Boolean ExistICCardno(string cardno, int empNo)
        {
            if (DbHelperSQL.DbType == 1)
            {
                string checkNameSql = "select 1 from f_employ_info where EmpCardNo='" + cardno + "' and EmpNo<>" + empNo;
                object oPerson = DbHelperSQL.GetSingle(checkNameSql);
                if (oPerson != null)  //判断是否IC卡号重复
                {
                    return true;
                }
                return false;
            }
            else
            {
                string checkNameSql = "select 1 from f_employ_info where EmpCardNo='" + cardno + "' and EmpNo<>" + empNo;
                object oPerson = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, checkNameSql);
                if (oPerson != null)  //判断是否IC卡号重复
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 根据员工姓名获取员工编号
        /// </summary>
        /// <returns></returns>
        public int GetEmpNo(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select empno from emp where name='" + name + "'");
            //            SqlParameter[] parameters = {
            //                    new SqlParameter("@name", SqlDbType.VarChar,30)
            //};
            //            parameters[0].Value = name;

            if (DbHelperSQL.DbType == 1)
            {
                return (int)DbHelperSQL.GetSingle(sb.ToString());
            }
            else
            {
                return (int)new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, sb.ToString());
            }
        }

        /// <summary>
        /// 根据员工卡卡号获取员工编号
        /// </summary>
        /// <returns></returns>
        public int GetEmpNoByCardno(string cardno)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select empno from F_Employ_Info where EmpCardNo='" + cardno + "'");
            //            SqlParameter[] parameters = {
            //                    new SqlParameter("@name", SqlDbType.VarChar,30)
            //};
            //            parameters[0].Value = name;

            if (DbHelperSQL.DbType == 1)
            {
                return (int)DbHelperSQL.GetSingle(sb.ToString());
            }
            else
            {
                return (int)new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, sb.ToString());
            }

        }

        #endregion


        #region  Method


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.M_Employ_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_Employ_Info(");
                strSql.Append("EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,Pwd,EmpCardNo)");
                strSql.Append(" values (");
                strSql.Append("@EmpName,@EmpSex,@EmpFloor,@EmpRoomCode,@EmpTel,@EmpMobile,@EmpExtTel,@EmpPosition,@EmpPhoto,@EmpMemu,@DeptNo,@CompanyId,@Pwd,@EmpCardNo)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@EmpName", SqlDbType.VarChar,30),
					new SqlParameter("@EmpSex", SqlDbType.VarChar,4),
					new SqlParameter("@EmpFloor", SqlDbType.VarChar,20),
					new SqlParameter("@EmpRoomCode", SqlDbType.VarChar,20),
					new SqlParameter("@EmpTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpMobile", SqlDbType.VarChar,20),
					new SqlParameter("@EmpExtTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPosition", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPhoto", SqlDbType.Image),
					new SqlParameter("@EmpMemu", SqlDbType.VarChar,100),
					new SqlParameter("@DeptNo", SqlDbType.Int,4),
					new SqlParameter("@CompanyId", SqlDbType.Int,4),
                    new SqlParameter("@Pwd",SqlDbType.VarChar,255),
                    new SqlParameter("@EmpCardNo", SqlDbType.VarChar,20)
                                            };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                //parameters[12].Value = desMethod.EncryptDES("123456", desMethod.strPwSalt);// "A23458DCECE31B0F2B1A5E41A185BE8C";  //旧默认密码123 ，md5加密
                parameters[12].Value = FormsAuthentication.HashPasswordForStoringInConfigFile("tecsunAppointment123456", "md5");
                parameters[13].Value = model.EmpCardno;
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
                strSql.Append("insert into F_Employ_Info(");
                strSql.Append("EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,Pwd,EmpCardNo,EmpNamePinYin)");
                strSql.Append(" values (");
                strSql.Append("@EmpName,@EmpSex,@EmpFloor,@EmpRoomCode,@EmpTel,@EmpMobile,@EmpExtTel,@EmpPosition,@EmpPhoto,@EmpMemu,@DeptNo,@CompanyId,@Pwd,@EmpCardNo,(select c1||c2 from get_py_zm(@EmpName)))");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmpName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@EmpSex", DbType.StringFixedLength,4),
					new NpgsqlParameter("@EmpFloor", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpRoomCode", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpMobile", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpExtTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPosition", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPhoto", NpgsqlDbType.Bytea),
					new NpgsqlParameter("@EmpMemu", DbType.StringFixedLength,100),
					new NpgsqlParameter("@DeptNo", DbType.Int32,4),
					new NpgsqlParameter("@CompanyId", DbType.Int32,4),
                    new NpgsqlParameter("@Pwd",DbType.StringFixedLength,255),
                    new NpgsqlParameter("@EmpCardNo", DbType.StringFixedLength,20)
                                               };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                //parameters[12].Value = desMethod.EncryptDES("123456", desMethod.strPwSalt);// "A23458DCECE31B0F2B1A5E41A185BE8C";  //旧默认密码123 ，md5加密
                parameters[12].Value = FormsAuthentication.HashPasswordForStoringInConfigFile("tecsunAppointment123456", "md5");
                parameters[13].Value = model.EmpCardno;
                object obj = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj.ToString());
                }
            }

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.M_Employ_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update F_Employ_Info set ");
                strSql.Append("EmpName=@EmpName,");
                strSql.Append("EmpSex=@EmpSex,");
                strSql.Append("EmpFloor=@EmpFloor,");
                strSql.Append("EmpRoomCode=@EmpRoomCode,");
                strSql.Append("EmpTel=@EmpTel,");
                strSql.Append("EmpMobile=@EmpMobile,");
                strSql.Append("EmpExtTel=@EmpExtTel,");
                strSql.Append("EmpPosition=@EmpPosition,");
                strSql.Append("EmpPhoto=@EmpPhoto,");
                strSql.Append("EmpMemu=@EmpMemu,");
                strSql.Append("DeptNo=@DeptNo,");
                strSql.Append("CompanyId=@CompanyId,");
                strSql.Append("EmpCardNo=@EmpCardNo ");

                strSql.Append(" where EmpNo=@EmpNo");
                SqlParameter[] parameters = {
					new SqlParameter("@EmpName", SqlDbType.VarChar,30),
					new SqlParameter("@EmpSex", SqlDbType.VarChar,4),
					new SqlParameter("@EmpFloor", SqlDbType.VarChar,20),
					new SqlParameter("@EmpRoomCode", SqlDbType.VarChar,20),
					new SqlParameter("@EmpTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpMobile", SqlDbType.VarChar,20),
					new SqlParameter("@EmpExtTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPosition", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPhoto", SqlDbType.Image),
					new SqlParameter("@EmpMemu", SqlDbType.VarChar,100),
					new SqlParameter("@DeptNo", SqlDbType.Int,4),
					new SqlParameter("@CompanyId", SqlDbType.Int,4),
                    new SqlParameter("@EmpCardNo", SqlDbType.VarChar,20),
					new SqlParameter("@EmpNo", SqlDbType.Int,4)
            };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                parameters[12].Value = model.EmpCardno;
                parameters[13].Value = model.EmpNo;

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
                strSql.Append("update F_Employ_Info set ");
                strSql.Append("EmpName=@EmpName,");
                strSql.Append("EmpSex=@EmpSex,");
                strSql.Append("EmpFloor=@EmpFloor,");
                strSql.Append("EmpRoomCode=@EmpRoomCode,");
                strSql.Append("EmpTel=@EmpTel,");
                strSql.Append("EmpMobile=@EmpMobile,");
                strSql.Append("EmpExtTel=@EmpExtTel,");
                strSql.Append("EmpPosition=@EmpPosition,");
                strSql.Append("EmpPhoto=@EmpPhoto,");
                strSql.Append("EmpMemu=@EmpMemu,");
                strSql.Append("DeptNo=@DeptNo,");
                strSql.Append("CompanyId=@CompanyId,");
                strSql.Append("EmpCardNo=@EmpCardNo, ");
                strSql.Append("EmpNamePinYin=(select c1||c2 from get_py_zm(EmpName)) ");


                strSql.Append(" where EmpNo=@EmpNo");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmpName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@EmpSex", DbType.StringFixedLength,4),
					new NpgsqlParameter("@EmpFloor", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpRoomCode", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpMobile", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpExtTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPosition", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPhoto", NpgsqlDbType.Bytea),
					new NpgsqlParameter("@EmpMemu", DbType.StringFixedLength,100),
					new NpgsqlParameter("@DeptNo", DbType.Int32,4),
					new NpgsqlParameter("@CompanyId", DbType.Int32,4),
                    new NpgsqlParameter("@EmpCardNo", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpNo", DbType.Int32,4)
            };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                parameters[12].Value = model.EmpCardno;
                parameters[13].Value = model.EmpNo;

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
        /// 设置员工卡的门禁期限
        /// </summary>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public bool SetCardEndDate(int empNo, DateTime? endDate)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update F_Employ_Info set ");
                strSql.Append("EmpCardEndDate=@EmpCardEndDate");
                strSql.Append(" where EmpNo=@EmpNo");
                SqlParameter[] parameters = {
					new SqlParameter("@EmpCardEndDate", SqlDbType.DateTime),
					new SqlParameter("@EmpNo", SqlDbType.Int,4)
            };
                if (endDate == null)
                    parameters[0].Value = DBNull.Value;
                else
                    parameters[0].Value = endDate;
                parameters[1].Value = empNo;

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
                strSql.Append("update F_Employ_Info set ");
                strSql.Append("EmpCardEndDate=@EmpCardEndDate");
                strSql.Append(" where EmpNo=@EmpNo");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmpCardEndDate", DbType.DateTime),
					new NpgsqlParameter("@EmpNo", DbType.Int32,4)
            };
                if (endDate == null)
                    parameters[0].Value = DBNull.Value;
                else
                    parameters[0].Value = endDate;
                parameters[1].Value = empNo;

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
        /// 重置预约网密码
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool ResetPwd(int empId, string pwd)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update F_Employ_Info set ");
                strSql.Append("Pwd=@Pwd ");
                strSql.Append(" where EmpNo=@EmpNo");
                SqlParameter[] parameters = {
					new SqlParameter("@Pwd", SqlDbType.VarChar,255),
					new SqlParameter("@EmpNo", SqlDbType.Int,4)
                                            };
                parameters[0].Value = FormsAuthentication.HashPasswordForStoringInConfigFile("tecsunAppointment" + pwd, "md5");
                parameters[1].Value = empId;

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
                strSql.Append("update F_Employ_Info set ");
                strSql.Append("Pwd=@Pwd ");
                strSql.Append(" where EmpNo=@EmpNo");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@Pwd", DbType.StringFixedLength,255),
					new NpgsqlParameter("@EmpNo", DbType.Int32,4)
                                               };
                parameters[0].Value = FormsAuthentication.HashPasswordForStoringInConfigFile("tecsunAppointment" + pwd, "md5");
                parameters[1].Value = empId;

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
        public bool Delete(int EmpNo)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from F_Employ_Info ");
                strSql.Append(" where EmpNo=@EmpNo");
                SqlParameter[] parameters = {
					new SqlParameter("@EmpNo", SqlDbType.Int,4)
                                        };
                parameters[0].Value = EmpNo;

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
                strSql.Append("delete from F_Employ_Info ");
                strSql.Append(" where EmpNo=@EmpNo");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmpNo", DbType.Int32,4)
                                        };
                parameters[0].Value = EmpNo;

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
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string EmpNolist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from F_Employ_Info ");
            strSql.Append(" where EmpNo in (" + EmpNolist + ")  ");

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
                int rows = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());

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
        /// 得到一个对象实体
        /// </summary>
        public Model.M_Employ_Info GetModel(int EmpNo)
        {
            Model.M_Employ_Info model = new Model.M_Employ_Info();
            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,EmpCardNo,empcard_ac_grantmsg,empcard_ac_enddate from F_Employ_Info ");
                strSql.Append(" where EmpNo=@EmpNo");
                SqlParameter[] parameters = {
					new SqlParameter("@EmpNo", SqlDbType.Int,4)
                                            };
                parameters[0].Value = EmpNo;
                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,EmpCardNo,empcard_ac_grantmsg,empcard_ac_enddate from F_Employ_Info ");
                strSql.Append(" where EmpNo=@EmpNo limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmpNo", DbType.Int32,4)
                                            };
                parameters[0].Value = EmpNo;
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["EmpNo"] != null && ds.Tables[0].Rows[0]["EmpNo"].ToString() != "")
                {
                    model.EmpNo = int.Parse(ds.Tables[0].Rows[0]["EmpNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmpName"] != null && ds.Tables[0].Rows[0]["EmpName"].ToString() != "")
                {
                    model.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpSex"] != null && ds.Tables[0].Rows[0]["EmpSex"].ToString() != "")
                {
                    model.EmpSex = ds.Tables[0].Rows[0]["EmpSex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpFloor"] != null && ds.Tables[0].Rows[0]["EmpFloor"].ToString() != "")
                {
                    model.EmpFloor = ds.Tables[0].Rows[0]["EmpFloor"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpRoomCode"] != null && ds.Tables[0].Rows[0]["EmpRoomCode"].ToString() != "")
                {
                    model.EmpRoomCode = ds.Tables[0].Rows[0]["EmpRoomCode"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpTel"] != null && ds.Tables[0].Rows[0]["EmpTel"].ToString() != "")
                {
                    model.EmpTel = ds.Tables[0].Rows[0]["EmpTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpMobile"] != null && ds.Tables[0].Rows[0]["EmpMobile"].ToString() != "")
                {
                    model.EmpMobile = ds.Tables[0].Rows[0]["EmpMobile"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpExtTel"] != null && ds.Tables[0].Rows[0]["EmpExtTel"].ToString() != "")
                {
                    model.EmpExtTel = ds.Tables[0].Rows[0]["EmpExtTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpPosition"] != null && ds.Tables[0].Rows[0]["EmpPosition"].ToString() != "")
                {
                    model.EmpPosition = ds.Tables[0].Rows[0]["EmpPosition"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpPhoto"] != null && ds.Tables[0].Rows[0]["EmpPhoto"].ToString() != "")
                {
                    model.EmpPhoto = (byte[])ds.Tables[0].Rows[0]["EmpPhoto"];
                }
                if (ds.Tables[0].Rows[0]["EmpMemu"] != null && ds.Tables[0].Rows[0]["EmpMemu"].ToString() != "")
                {
                    model.EmpMemu = ds.Tables[0].Rows[0]["EmpMemu"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DeptNo"] != null && ds.Tables[0].Rows[0]["DeptNo"].ToString() != "")
                {
                    model.DeptNo = int.Parse(ds.Tables[0].Rows[0]["DeptNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CompanyId"] != null && ds.Tables[0].Rows[0]["CompanyId"].ToString() != "")
                {
                    model.CompanyId = int.Parse(ds.Tables[0].Rows[0]["CompanyId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmpCardNo"] != null && ds.Tables[0].Rows[0]["EmpCardNo"].ToString() != "")
                {
                    model.EmpCardno = ds.Tables[0].Rows[0]["EmpCardNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["empcard_ac_grantmsg"] != null && ds.Tables[0].Rows[0]["empcard_ac_grantmsg"].ToString() != "")
                {
                    model.Empcard_ac_grantmsg = ds.Tables[0].Rows[0]["empcard_ac_grantmsg"].ToString();
                }
                if (ds.Tables[0].Rows[0]["empcard_ac_enddate"] != null && ds.Tables[0].Rows[0]["empcard_ac_enddate"].ToString() != "")
                {
                    model.EmpCard_ac_enddate = DateTime.Parse(ds.Tables[0].Rows[0]["empcard_ac_enddate"].ToString());
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
        public Model.M_Employ_Info GetModel(string empName, string empTel)
        {
            Model.M_Employ_Info model = new Model.M_Employ_Info();
            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId from F_Employ_Info ");
                strSql.Append(" where EmpName=@EmpName and EmpMobile=@EmpMobile");
                SqlParameter[] parameters = {
                new SqlParameter("@EmpName", SqlDbType.VarChar,30),
				new SqlParameter("@EmpMobile", SqlDbType.VarChar,20)
                                            };
                parameters[0].Value = empName;
                parameters[1].Value = empTel;
                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId from F_Employ_Info ");
                strSql.Append(" where EmpName=@EmpName and EmpMobile=@EmpMobile limit 1");
                NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@EmpName", DbType.StringFixedLength,30),
				new NpgsqlParameter("@EmpMobile", DbType.StringFixedLength,20)
                                            };
                parameters[0].Value = empName;
                parameters[1].Value = empTel;
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }


            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["EmpNo"] != null && ds.Tables[0].Rows[0]["EmpNo"].ToString() != "")
                {
                    model.EmpNo = int.Parse(ds.Tables[0].Rows[0]["EmpNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmpName"] != null && ds.Tables[0].Rows[0]["EmpName"].ToString() != "")
                {
                    model.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpSex"] != null && ds.Tables[0].Rows[0]["EmpSex"].ToString() != "")
                {
                    model.EmpSex = ds.Tables[0].Rows[0]["EmpSex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpFloor"] != null && ds.Tables[0].Rows[0]["EmpFloor"].ToString() != "")
                {
                    model.EmpFloor = ds.Tables[0].Rows[0]["EmpFloor"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpRoomCode"] != null && ds.Tables[0].Rows[0]["EmpRoomCode"].ToString() != "")
                {
                    model.EmpRoomCode = ds.Tables[0].Rows[0]["EmpRoomCode"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpTel"] != null && ds.Tables[0].Rows[0]["EmpTel"].ToString() != "")
                {
                    model.EmpTel = ds.Tables[0].Rows[0]["EmpTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpMobile"] != null && ds.Tables[0].Rows[0]["EmpMobile"].ToString() != "")
                {
                    model.EmpMobile = ds.Tables[0].Rows[0]["EmpMobile"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpExtTel"] != null && ds.Tables[0].Rows[0]["EmpExtTel"].ToString() != "")
                {
                    model.EmpExtTel = ds.Tables[0].Rows[0]["EmpExtTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpPosition"] != null && ds.Tables[0].Rows[0]["EmpPosition"].ToString() != "")
                {
                    model.EmpPosition = ds.Tables[0].Rows[0]["EmpPosition"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpPhoto"] != null && ds.Tables[0].Rows[0]["EmpPhoto"].ToString() != "")
                {
                    model.EmpPhoto = (byte[])ds.Tables[0].Rows[0]["EmpPhoto"];
                }
                if (ds.Tables[0].Rows[0]["EmpMemu"] != null && ds.Tables[0].Rows[0]["EmpMemu"].ToString() != "")
                {
                    model.EmpMemu = ds.Tables[0].Rows[0]["EmpMemu"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DeptNo"] != null && ds.Tables[0].Rows[0]["DeptNo"].ToString() != "")
                {
                    model.DeptNo = int.Parse(ds.Tables[0].Rows[0]["DeptNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CompanyId"] != null && ds.Tables[0].Rows[0]["CompanyId"].ToString() != "")
                {
                    model.CompanyId = int.Parse(ds.Tables[0].Rows[0]["CompanyId"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();

            if (DbHelperSQL.DbType == 1)
            {
                strSql.Append("select top 30 EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,iStatus,emp_n_face_card_num ");
                strSql.Append(" FROM F_Employ_Info ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                strSql.Append("select EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,iStatus,emp_n_face_card_num ");
                strSql.Append(" FROM F_Employ_Info");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                strSql.Append(" limit 30  ");
                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
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
                strSql.Append(" EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId ");
                strSql.Append(" FROM F_Employ_Info ");
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

                strSql.Append(" EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId ");
                strSql.Append(" FROM F_Employ_Info ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                strSql.Append(" order by " + filedOrder);
                if (Top > 0)
                {
                    strSql.Append(" limit " + Top.ToString());
                }
                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }

        }


        public Boolean ExistEmpId_pf(string EmployNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select 1 ");
            strSql.Append(" from f_employ_info");
            strSql.Append(" where EmployNo=@EmployNo");
            SqlParameter[] parameters = {
					new SqlParameter("@EmployNo", SqlDbType.VarChar,32)
                                        };
            parameters[0].Value = EmployNo;
            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Exists(strSql.ToString(), parameters);
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
        public int Add_pf(Model.M_Employ_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_Employ_Info(");
                strSql.Append("EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,Pwd,EmpCardNo,WeixinId,SjId,EmployNo,EmployPhotoName,EmployCreateTime)");
                strSql.Append(" values (");
                strSql.Append("@EmpName,@EmpSex,@EmpFloor,@EmpRoomCode,@EmpTel,@EmpMobile,@EmpExtTel,@EmpPosition,@EmpPhoto,@EmpMemu,@DeptNo,@CompanyId,@Pwd,@EmpCardNo,@WeixinId,@SjId,@EmployNo,@EmployPhotoName,@EmployCreateTime)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@EmpName", SqlDbType.VarChar,30),
					new SqlParameter("@EmpSex", SqlDbType.VarChar,4),
					new SqlParameter("@EmpFloor", SqlDbType.VarChar,20),
					new SqlParameter("@EmpRoomCode", SqlDbType.VarChar,20),
					new SqlParameter("@EmpTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpMobile", SqlDbType.VarChar,20),
					new SqlParameter("@EmpExtTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPosition", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPhoto", SqlDbType.Image),
					new SqlParameter("@EmpMemu", SqlDbType.VarChar,100),
					new SqlParameter("@DeptNo", SqlDbType.Int,4),
					new SqlParameter("@CompanyId", SqlDbType.Int,4),
                    new SqlParameter("@Pwd",SqlDbType.VarChar,255),
                    new SqlParameter("@EmpCardNo", SqlDbType.VarChar,20),
                    new SqlParameter("@WeixinId", SqlDbType.Int,4),
                    new SqlParameter("@SjId", SqlDbType.VarChar,200),
                    new SqlParameter("@EmployNo", SqlDbType.VarChar,32),
                    new SqlParameter("@EmployPhotoName", SqlDbType.VarChar,32),
                    new SqlParameter("@EmployCreateTime", SqlDbType.DateTime)
                                            };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                parameters[12].Value = FormsAuthentication.HashPasswordForStoringInConfigFile("tecsunAppointment123456", "md5");
                parameters[13].Value = model.EmpCardno;
                parameters[14].Value = model.WeixinId;
                parameters[15].Value = model.SjId;
                parameters[16].Value = model.EmployNo;
                parameters[17].Value = model.EmployPhotoName;
                parameters[18].Value = model.EmployCreateTime;

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
                strSql.Append("insert into F_Employ_Info(");
                strSql.Append("EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,Pwd,EmpCardNo,EmpNamePinYin,WeixinId,SjId,EmployNo,EmployPhotoName,EmployCreateTime)");
                strSql.Append(" values (");
                strSql.Append("@EmpName,@EmpSex,@EmpFloor,@EmpRoomCode,@EmpTel,@EmpMobile,@EmpExtTel,@EmpPosition,@EmpPhoto,@EmpMemu,@DeptNo,@CompanyId,@Pwd,@EmpCardNo,(select c1||c2 from get_py_zm(@EmpName)),@WeixinId,@SjId,@EmployNo,@EmployPhotoName,@EmployCreateTime)");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmpName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@EmpSex", DbType.StringFixedLength,4),
					new NpgsqlParameter("@EmpFloor", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpRoomCode", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpMobile", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpExtTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPosition", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPhoto", NpgsqlDbType.Bytea),
					new NpgsqlParameter("@EmpMemu", DbType.StringFixedLength,100),
					new NpgsqlParameter("@DeptNo", DbType.Int32,4),
					new NpgsqlParameter("@CompanyId", DbType.Int32,4),
                    new NpgsqlParameter("@Pwd",DbType.StringFixedLength,255),
                    new NpgsqlParameter("@EmpCardNo", DbType.StringFixedLength,20),
                    new NpgsqlParameter("@WeixinId", DbType.Int32,4),
                    new NpgsqlParameter("@SjId", DbType.StringFixedLength,200),
                    new NpgsqlParameter("@EmployNo", DbType.StringFixedLength,32),
                    new NpgsqlParameter("@EmployPhotoName", DbType.StringFixedLength,32),
                    new NpgsqlParameter("@EmployCreateTime", DbType.DateTime)
                                               };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                parameters[12].Value = FormsAuthentication.HashPasswordForStoringInConfigFile("tecsunAppointment123456", "md5");
                parameters[13].Value = model.EmpCardno;
                parameters[14].Value = model.WeixinId;
                parameters[15].Value = model.SjId;
                parameters[16].Value = model.EmployNo;
                parameters[17].Value = model.EmployPhotoName;
                parameters[18].Value = model.EmployCreateTime;

                object obj = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj.ToString());
                }
            }

        }
        public bool Update_pf(Model.M_Employ_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update F_Employ_Info set ");
                strSql.Append("EmpName=@EmpName,");
                strSql.Append("EmpSex=@EmpSex,");
                strSql.Append("EmpFloor=@EmpFloor,");
                strSql.Append("EmpRoomCode=@EmpRoomCode,");
                strSql.Append("EmpTel=@EmpTel,");
                strSql.Append("EmpMobile=@EmpMobile,");
                strSql.Append("EmpExtTel=@EmpExtTel,");
                strSql.Append("EmpPosition=@EmpPosition,");
                strSql.Append("EmpPhoto=@EmpPhoto,");
                strSql.Append("EmpMemu=@EmpMemu,");
                strSql.Append("DeptNo=@DeptNo,");
                strSql.Append("CompanyId=@CompanyId,");
                strSql.Append("EmpCardNo=@EmpCardNo,");
                strSql.Append("EmployPhotoName=@EmployPhotoName,");
                strSql.Append("EmployCreateTime=@EmployCreateTime ");

                strSql.Append(" where EmployNo=@EmployNo");
                SqlParameter[] parameters = {
					new SqlParameter("@EmpName", SqlDbType.VarChar,30),
					new SqlParameter("@EmpSex", SqlDbType.VarChar,4),
					new SqlParameter("@EmpFloor", SqlDbType.VarChar,20),
					new SqlParameter("@EmpRoomCode", SqlDbType.VarChar,20),
					new SqlParameter("@EmpTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpMobile", SqlDbType.VarChar,20),
					new SqlParameter("@EmpExtTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPosition", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPhoto", SqlDbType.Image),
					new SqlParameter("@EmpMemu", SqlDbType.VarChar,100),
					new SqlParameter("@DeptNo", SqlDbType.Int,4),
					new SqlParameter("@CompanyId", SqlDbType.Int,4),
                    new SqlParameter("@EmpCardNo", SqlDbType.VarChar,20),
                    new SqlParameter("@EmployPhotoName", SqlDbType.VarChar,32),
                    new SqlParameter("@EmployCreateTime", SqlDbType.DateTime),
                    new SqlParameter("@EmployNo", SqlDbType.VarChar,32)
            };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                parameters[12].Value = model.EmpCardno;
                parameters[13].Value = model.EmployPhotoName;
                parameters[14].Value = model.EmployCreateTime;
                parameters[15].Value = model.EmployNo;

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
                strSql.Append("update F_Employ_Info set ");
                strSql.Append("EmpName=@EmpName,");
                strSql.Append("EmpSex=@EmpSex,");
                strSql.Append("EmpFloor=@EmpFloor,");
                strSql.Append("EmpRoomCode=@EmpRoomCode,");
                strSql.Append("EmpTel=@EmpTel,");
                strSql.Append("EmpMobile=@EmpMobile,");
                strSql.Append("EmpExtTel=@EmpExtTel,");
                strSql.Append("EmpPosition=@EmpPosition,");
                strSql.Append("EmpPhoto=@EmpPhoto,");
                strSql.Append("EmpMemu=@EmpMemu,");
                strSql.Append("DeptNo=@DeptNo,");
                strSql.Append("CompanyId=@CompanyId,");
                strSql.Append("EmpCardNo=@EmpCardNo, ");
                strSql.Append("EmpNamePinYin=(select c1||c2 from get_py_zm(EmpName)) ");

                strSql.Append(" where WeixinId=@WeixinId");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmpName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@EmpSex", DbType.StringFixedLength,4),
					new NpgsqlParameter("@EmpFloor", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpRoomCode", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpMobile", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpExtTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPosition", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPhoto", NpgsqlDbType.Bytea),
					new NpgsqlParameter("@EmpMemu", DbType.StringFixedLength,100),
					new NpgsqlParameter("@DeptNo", DbType.Int32,4),
					new NpgsqlParameter("@CompanyId", DbType.Int32,4),
                    new NpgsqlParameter("@EmpCardNo", DbType.StringFixedLength,20),
					new NpgsqlParameter("@WeixinId", DbType.Int32,4)
            };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                parameters[12].Value = model.EmpCardno;
                parameters[13].Value = model.WeixinId;

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

        public bool Delete_pf(string EmployNo)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from F_Employ_Info ");
                strSql.Append(" where EmployNo=@EmployNo");
                SqlParameter[] parameters = {
					new SqlParameter("@EmployNo", SqlDbType.VarChar,32)
                                        };
                parameters[0].Value = EmployNo;

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
                strSql.Append("delete from F_Employ_Info ");
                strSql.Append(" where EmployNo=@EmployNo");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmployNo", DbType.StringFixedLength,32)
                                        };
                parameters[0].Value = EmployNo;

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
        public Model.M_Employ_Info GetModel_pf(string EmployNo)
        {
            Model.M_Employ_Info model = new Model.M_Employ_Info();
            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,EmpCardNo,WeixinId,EmployNo,EmployPhotoName,EmployCreateTime from F_Employ_Info ");
                strSql.Append(" where EmployNo=@EmployNo");
                SqlParameter[] parameters = {
					new SqlParameter("@EmployNo",  SqlDbType.VarChar,32)
                                            };
                parameters[0].Value = EmployNo;
                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,EmpCardNo,WeixinId,EmployNo,EmployPhotoName,EmployCreateTime from F_Employ_Info ");
                strSql.Append(" where EmployNo=@EmployNo limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmployNo",  DbType.StringFixedLength,32)
                                            };
                parameters[0].Value = EmployNo;
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["EmpNo"] != null && ds.Tables[0].Rows[0]["EmpNo"].ToString() != "")
                {
                    model.EmpNo = int.Parse(ds.Tables[0].Rows[0]["EmpNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmpName"] != null && ds.Tables[0].Rows[0]["EmpName"].ToString() != "")
                {
                    model.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpSex"] != null && ds.Tables[0].Rows[0]["EmpSex"].ToString() != "")
                {
                    model.EmpSex = ds.Tables[0].Rows[0]["EmpSex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpFloor"] != null && ds.Tables[0].Rows[0]["EmpFloor"].ToString() != "")
                {
                    model.EmpFloor = ds.Tables[0].Rows[0]["EmpFloor"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpRoomCode"] != null && ds.Tables[0].Rows[0]["EmpRoomCode"].ToString() != "")
                {
                    model.EmpRoomCode = ds.Tables[0].Rows[0]["EmpRoomCode"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpTel"] != null && ds.Tables[0].Rows[0]["EmpTel"].ToString() != "")
                {
                    model.EmpTel = ds.Tables[0].Rows[0]["EmpTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpMobile"] != null && ds.Tables[0].Rows[0]["EmpMobile"].ToString() != "")
                {
                    model.EmpMobile = ds.Tables[0].Rows[0]["EmpMobile"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpExtTel"] != null && ds.Tables[0].Rows[0]["EmpExtTel"].ToString() != "")
                {
                    model.EmpExtTel = ds.Tables[0].Rows[0]["EmpExtTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpPosition"] != null && ds.Tables[0].Rows[0]["EmpPosition"].ToString() != "")
                {
                    model.EmpPosition = ds.Tables[0].Rows[0]["EmpPosition"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpPhoto"] != null && ds.Tables[0].Rows[0]["EmpPhoto"].ToString() != "")
                {
                    model.EmpPhoto = (byte[])ds.Tables[0].Rows[0]["EmpPhoto"];
                }
                if (ds.Tables[0].Rows[0]["EmpMemu"] != null && ds.Tables[0].Rows[0]["EmpMemu"].ToString() != "")
                {
                    model.EmpMemu = ds.Tables[0].Rows[0]["EmpMemu"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DeptNo"] != null && ds.Tables[0].Rows[0]["DeptNo"].ToString() != "")
                {
                    model.DeptNo = int.Parse(ds.Tables[0].Rows[0]["DeptNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CompanyId"] != null && ds.Tables[0].Rows[0]["CompanyId"].ToString() != "")
                {
                    model.CompanyId = int.Parse(ds.Tables[0].Rows[0]["CompanyId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmpCardNo"] != null && ds.Tables[0].Rows[0]["EmpCardNo"].ToString() != "")
                {
                    model.EmpCardno = ds.Tables[0].Rows[0]["EmpCardNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["WeixinId"] != null && ds.Tables[0].Rows[0]["WeixinId"].ToString() != "")
                {
                    model.WeixinId = int.Parse(ds.Tables[0].Rows[0]["WeixinId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmployNo"] != null && ds.Tables[0].Rows[0]["EmployNo"].ToString() != "")
                {
                    model.EmployNo = ds.Tables[0].Rows[0]["EmployNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmployPhotoName"] != null && ds.Tables[0].Rows[0]["EmployPhotoName"].ToString() != "")
                {
                    model.EmployPhotoName = ds.Tables[0].Rows[0]["EmployPhotoName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmployCreateTime"] != null && ds.Tables[0].Rows[0]["EmployCreateTime"].ToString() != "")
                {
                    model.EmployCreateTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["EmployCreateTime"].ToString());
                }

                return model;
            }
            else
            {
                return null;
            }
        }


        public Boolean ExistEmpId_wx(int weixinId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select 1 ");
            strSql.Append(" from f_employ_info");
            strSql.Append(" where  weixinId=" + weixinId);

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

        public int Add_wx(Model.M_Employ_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_Employ_Info(");
                strSql.Append("EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,Pwd,EmpCardNo,WeixinId,SjId,EmpNum,LicensePlate)");
                strSql.Append(" values (");
                strSql.Append("@EmpName,@EmpSex,@EmpFloor,@EmpRoomCode,@EmpTel,@EmpMobile,@EmpExtTel,@EmpPosition,@EmpPhoto,@EmpMemu,@DeptNo,@CompanyId,@Pwd,@EmpCardNo,@WeixinId,@SjId,@EmpNum,@LicensePlate)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@EmpName", SqlDbType.VarChar,30),
					new SqlParameter("@EmpSex", SqlDbType.VarChar,4),
					new SqlParameter("@EmpFloor", SqlDbType.VarChar,20),
					new SqlParameter("@EmpRoomCode", SqlDbType.VarChar,20),
					new SqlParameter("@EmpTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpMobile", SqlDbType.VarChar,20),
					new SqlParameter("@EmpExtTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPosition", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPhoto", SqlDbType.Image),
					new SqlParameter("@EmpMemu", SqlDbType.VarChar,100),
					new SqlParameter("@DeptNo", SqlDbType.Int,4),
					new SqlParameter("@CompanyId", SqlDbType.Int,4),
                    new SqlParameter("@Pwd",SqlDbType.VarChar,255),
                    new SqlParameter("@EmpCardNo", SqlDbType.VarChar,20),
                    new SqlParameter("@WeixinId", SqlDbType.Int,4),
                    new SqlParameter("@SjId", SqlDbType.VarChar,200),
                    new SqlParameter("@EmpNum", SqlDbType.VarChar,64),
                    new SqlParameter("@LicensePlate", SqlDbType.VarChar,64),
                                            };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                //parameters[12].Value = desMethod.EncryptDES("123456", desMethod.strPwSalt);// "A23458DCECE31B0F2B1A5E41A185BE8C";  //旧默认密码123 ，md5加密
                parameters[12].Value = FormsAuthentication.HashPasswordForStoringInConfigFile("tecsunAppointment123456", "md5");
                parameters[13].Value = model.EmpCardno;
                parameters[14].Value = model.WeixinId;
                parameters[15].Value = model.SjId;
                parameters[16].Value = model.EmpNum;
                parameters[17].Value = model.LicensePlate;

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
                strSql.Append("insert into F_Employ_Info(");
                strSql.Append("EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,Pwd,EmpCardNo,EmpNamePinYin,WeixinId,SjId,EmpNum,LicensePlate)");
                strSql.Append(" values (");
                strSql.Append("@EmpName,@EmpSex,@EmpFloor,@EmpRoomCode,@EmpTel,@EmpMobile,@EmpExtTel,@EmpPosition,@EmpPhoto,@EmpMemu,@DeptNo,@CompanyId,@Pwd,@EmpCardNo,(select c1||c2 from get_py_zm(@EmpName)),@WeixinId,@SjId,@EmpNum,@LicensePlate)");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmpName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@EmpSex", DbType.StringFixedLength,4),
					new NpgsqlParameter("@EmpFloor", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpRoomCode", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpMobile", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpExtTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPosition", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPhoto", NpgsqlDbType.Bytea),
					new NpgsqlParameter("@EmpMemu", DbType.StringFixedLength,100),
					new NpgsqlParameter("@DeptNo", DbType.Int32,4),
					new NpgsqlParameter("@CompanyId", DbType.Int32,4),
                    new NpgsqlParameter("@Pwd",DbType.StringFixedLength,255),
                    new NpgsqlParameter("@EmpCardNo", DbType.StringFixedLength,20),
                    new NpgsqlParameter("@WeixinId", DbType.Int32,4),
                    new NpgsqlParameter("@SjId", DbType.StringFixedLength,200),
                    new NpgsqlParameter("@EmpNum", DbType.StringFixedLength,64),
                    new NpgsqlParameter("@LicensePlate", DbType.StringFixedLength,64)
                                               };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                //parameters[12].Value = desMethod.EncryptDES("123456", desMethod.strPwSalt);// "A23458DCECE31B0F2B1A5E41A185BE8C";  //旧默认密码123 ，md5加密
                parameters[12].Value = FormsAuthentication.HashPasswordForStoringInConfigFile("tecsunAppointment123456", "md5");
                parameters[13].Value = model.EmpCardno;
                parameters[14].Value = model.WeixinId;
                parameters[15].Value = model.SjId;
                parameters[16].Value = model.EmpNum;
                parameters[17].Value = model.LicensePlate;

                object obj = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj.ToString());
                }
            }
        }

        public bool Update_wx(Model.M_Employ_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update F_Employ_Info set ");
                strSql.Append("EmpName=@EmpName,");
                strSql.Append("EmpSex=@EmpSex,");
                strSql.Append("EmpFloor=@EmpFloor,");
                strSql.Append("EmpRoomCode=@EmpRoomCode,");
                strSql.Append("EmpTel=@EmpTel,");
                strSql.Append("EmpMobile=@EmpMobile,");
                strSql.Append("EmpExtTel=@EmpExtTel,");
                strSql.Append("EmpPosition=@EmpPosition,");
                strSql.Append("EmpPhoto=@EmpPhoto,");
                strSql.Append("EmpMemu=@EmpMemu,");
                strSql.Append("DeptNo=@DeptNo,");
                strSql.Append("CompanyId=@CompanyId,");
                strSql.Append("EmpCardNo=@EmpCardNo ");

                strSql.Append(" where WeixinId=@WeixinId");
                SqlParameter[] parameters = {
					new SqlParameter("@EmpName", SqlDbType.VarChar,30),
					new SqlParameter("@EmpSex", SqlDbType.VarChar,4),
					new SqlParameter("@EmpFloor", SqlDbType.VarChar,20),
					new SqlParameter("@EmpRoomCode", SqlDbType.VarChar,20),
					new SqlParameter("@EmpTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpMobile", SqlDbType.VarChar,20),
					new SqlParameter("@EmpExtTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPosition", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPhoto", SqlDbType.Image),
					new SqlParameter("@EmpMemu", SqlDbType.VarChar,100),
					new SqlParameter("@DeptNo", SqlDbType.Int,4),
					new SqlParameter("@CompanyId", SqlDbType.Int,4),
                    new SqlParameter("@EmpCardNo", SqlDbType.VarChar,20),
                    new SqlParameter("@WeixinId", SqlDbType.Int,4)
            };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                parameters[12].Value = model.EmpCardno;
                parameters[13].Value = model.WeixinId;

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
                strSql.Append("update F_Employ_Info set ");
                strSql.Append("EmpName=@EmpName,");
                strSql.Append("EmpSex=@EmpSex,");
                strSql.Append("EmpFloor=@EmpFloor,");
                strSql.Append("EmpRoomCode=@EmpRoomCode,");
                strSql.Append("EmpTel=@EmpTel,");
                strSql.Append("EmpMobile=@EmpMobile,");
                strSql.Append("EmpExtTel=@EmpExtTel,");
                strSql.Append("EmpPosition=@EmpPosition,");
                strSql.Append("EmpPhoto=@EmpPhoto,");
                strSql.Append("EmpMemu=@EmpMemu,");
                strSql.Append("DeptNo=@DeptNo,");
                strSql.Append("CompanyId=@CompanyId,");
                strSql.Append("EmpCardNo=@EmpCardNo, ");
                strSql.Append("EmpNamePinYin=(select c1||c2 from get_py_zm(EmpName)) ");

                strSql.Append(" where WeixinId=@WeixinId");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmpName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@EmpSex", DbType.StringFixedLength,4),
					new NpgsqlParameter("@EmpFloor", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpRoomCode", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpMobile", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpExtTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPosition", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPhoto", NpgsqlDbType.Bytea),
					new NpgsqlParameter("@EmpMemu", DbType.StringFixedLength,100),
					new NpgsqlParameter("@DeptNo", DbType.Int32,4),
					new NpgsqlParameter("@CompanyId", DbType.Int32,4),
                    new NpgsqlParameter("@EmpCardNo", DbType.StringFixedLength,20),
					new NpgsqlParameter("@WeixinId", DbType.Int32,4)
            };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                parameters[12].Value = model.EmpCardno;
                parameters[13].Value = model.WeixinId;

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
        public bool Delete_wx(int weixinId)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from F_Employ_Info ");
                strSql.Append(" where weixinid=@weixinid");
                SqlParameter[] parameters = {
					new SqlParameter("@weixinid", SqlDbType.Int,4)
                                        };
                parameters[0].Value = weixinId;

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
                strSql.Append("delete from F_Employ_Info ");
                strSql.Append(" where weixinid=@weixinid");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@weixinid", DbType.Int32,4)
                                        };
                parameters[0].Value = weixinId;

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

        public Model.M_Employ_Info GetModel_wx(int weixinId)
        {
            Model.M_Employ_Info model = new Model.M_Employ_Info();
            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,EmpCardNo,WeixinId from F_Employ_Info ");
                strSql.Append(" where WeixinId=@WeixinId");
                SqlParameter[] parameters = {
					new SqlParameter("@WeixinId", SqlDbType.Int,4)
                                            };
                parameters[0].Value = weixinId;
                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,EmpCardNo,WeixinId from F_Employ_Info ");
                strSql.Append(" where WeixinId=@WeixinId limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@WeixinId", DbType.Int32,4)
                                            };
                parameters[0].Value = weixinId;
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["EmpNo"] != null && ds.Tables[0].Rows[0]["EmpNo"].ToString() != "")
                {
                    model.EmpNo = int.Parse(ds.Tables[0].Rows[0]["EmpNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmpName"] != null && ds.Tables[0].Rows[0]["EmpName"].ToString() != "")
                {
                    model.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpSex"] != null && ds.Tables[0].Rows[0]["EmpSex"].ToString() != "")
                {
                    model.EmpSex = ds.Tables[0].Rows[0]["EmpSex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpFloor"] != null && ds.Tables[0].Rows[0]["EmpFloor"].ToString() != "")
                {
                    model.EmpFloor = ds.Tables[0].Rows[0]["EmpFloor"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpRoomCode"] != null && ds.Tables[0].Rows[0]["EmpRoomCode"].ToString() != "")
                {
                    model.EmpRoomCode = ds.Tables[0].Rows[0]["EmpRoomCode"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpTel"] != null && ds.Tables[0].Rows[0]["EmpTel"].ToString() != "")
                {
                    model.EmpTel = ds.Tables[0].Rows[0]["EmpTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpMobile"] != null && ds.Tables[0].Rows[0]["EmpMobile"].ToString() != "")
                {
                    model.EmpMobile = ds.Tables[0].Rows[0]["EmpMobile"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpExtTel"] != null && ds.Tables[0].Rows[0]["EmpExtTel"].ToString() != "")
                {
                    model.EmpExtTel = ds.Tables[0].Rows[0]["EmpExtTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpPosition"] != null && ds.Tables[0].Rows[0]["EmpPosition"].ToString() != "")
                {
                    model.EmpPosition = ds.Tables[0].Rows[0]["EmpPosition"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpPhoto"] != null && ds.Tables[0].Rows[0]["EmpPhoto"].ToString() != "")
                {
                    model.EmpPhoto = (byte[])ds.Tables[0].Rows[0]["EmpPhoto"];
                }
                if (ds.Tables[0].Rows[0]["EmpMemu"] != null && ds.Tables[0].Rows[0]["EmpMemu"].ToString() != "")
                {
                    model.EmpMemu = ds.Tables[0].Rows[0]["EmpMemu"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DeptNo"] != null && ds.Tables[0].Rows[0]["DeptNo"].ToString() != "")
                {
                    model.DeptNo = int.Parse(ds.Tables[0].Rows[0]["DeptNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CompanyId"] != null && ds.Tables[0].Rows[0]["CompanyId"].ToString() != "")
                {
                    model.CompanyId = int.Parse(ds.Tables[0].Rows[0]["CompanyId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmpCardNo"] != null && ds.Tables[0].Rows[0]["EmpCardNo"].ToString() != "")
                {
                    model.EmpCardno = ds.Tables[0].Rows[0]["EmpCardNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["WeixinId"] != null && ds.Tables[0].Rows[0]["WeixinId"].ToString() != "")
                {
                    model.WeixinId = int.Parse(ds.Tables[0].Rows[0]["WeixinId"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        public DataSet GetList_SJP(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Emp_ID,Dept_ID,Emp_Name,Emp_Sex,Emp_Phone ");
            strSql.Append(" FROM tbEmployee ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            return DbHelperSQL.Query_SJP(strSql.ToString());

        }

        public Boolean ExistEmpId_SJP(string sjid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select 1 ");
            strSql.Append(" from f_employ_info");
            strSql.Append(" where  sjid='" + sjid + "'");

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

        public Model.M_Employ_Info GetModel_SJP(string sjId)
        {
            Model.M_Employ_Info model = new Model.M_Employ_Info();
            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,EmpCardNo,WeixinId,SjId from F_Employ_Info ");
                strSql.Append(" where SjId=@SjId");
                SqlParameter[] parameters = {
					new SqlParameter("@SjId", SqlDbType.VarChar,200)
                                            };
                parameters[0].Value = sjId;
                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,EmpCardNo,WeixinId,SjId from F_Employ_Info ");
                strSql.Append(" where SjId=@SjId limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@SjId", DbType.StringFixedLength,200)
                                            };
                parameters[0].Value = sjId;
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["EmpNo"] != null && ds.Tables[0].Rows[0]["EmpNo"].ToString() != "")
                {
                    model.EmpNo = int.Parse(ds.Tables[0].Rows[0]["EmpNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmpName"] != null && ds.Tables[0].Rows[0]["EmpName"].ToString() != "")
                {
                    model.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpSex"] != null && ds.Tables[0].Rows[0]["EmpSex"].ToString() != "")
                {
                    model.EmpSex = ds.Tables[0].Rows[0]["EmpSex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpFloor"] != null && ds.Tables[0].Rows[0]["EmpFloor"].ToString() != "")
                {
                    model.EmpFloor = ds.Tables[0].Rows[0]["EmpFloor"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpRoomCode"] != null && ds.Tables[0].Rows[0]["EmpRoomCode"].ToString() != "")
                {
                    model.EmpRoomCode = ds.Tables[0].Rows[0]["EmpRoomCode"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpTel"] != null && ds.Tables[0].Rows[0]["EmpTel"].ToString() != "")
                {
                    model.EmpTel = ds.Tables[0].Rows[0]["EmpTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpMobile"] != null && ds.Tables[0].Rows[0]["EmpMobile"].ToString() != "")
                {
                    model.EmpMobile = ds.Tables[0].Rows[0]["EmpMobile"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpExtTel"] != null && ds.Tables[0].Rows[0]["EmpExtTel"].ToString() != "")
                {
                    model.EmpExtTel = ds.Tables[0].Rows[0]["EmpExtTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpPosition"] != null && ds.Tables[0].Rows[0]["EmpPosition"].ToString() != "")
                {
                    model.EmpPosition = ds.Tables[0].Rows[0]["EmpPosition"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpPhoto"] != null && ds.Tables[0].Rows[0]["EmpPhoto"].ToString() != "")
                {
                    model.EmpPhoto = (byte[])ds.Tables[0].Rows[0]["EmpPhoto"];
                }
                if (ds.Tables[0].Rows[0]["EmpMemu"] != null && ds.Tables[0].Rows[0]["EmpMemu"].ToString() != "")
                {
                    model.EmpMemu = ds.Tables[0].Rows[0]["EmpMemu"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DeptNo"] != null && ds.Tables[0].Rows[0]["DeptNo"].ToString() != "")
                {
                    model.DeptNo = int.Parse(ds.Tables[0].Rows[0]["DeptNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CompanyId"] != null && ds.Tables[0].Rows[0]["CompanyId"].ToString() != "")
                {
                    model.CompanyId = int.Parse(ds.Tables[0].Rows[0]["CompanyId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmpCardNo"] != null && ds.Tables[0].Rows[0]["EmpCardNo"].ToString() != "")
                {
                    model.EmpCardno = ds.Tables[0].Rows[0]["EmpCardNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["SjId"] != null && ds.Tables[0].Rows[0]["SjId"].ToString() != "")
                {
                    model.SjId = ds.Tables[0].Rows[0]["SjId"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        public bool Update_SJP(Model.M_Employ_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update F_Employ_Info set ");
                strSql.Append("EmpName=@EmpName,");
                strSql.Append("EmpSex=@EmpSex,");
                strSql.Append("EmpFloor=@EmpFloor,");
                strSql.Append("EmpRoomCode=@EmpRoomCode,");
                strSql.Append("EmpTel=@EmpTel,");
                strSql.Append("EmpMobile=@EmpMobile,");
                strSql.Append("EmpExtTel=@EmpExtTel,");
                strSql.Append("EmpPosition=@EmpPosition,");
                strSql.Append("EmpPhoto=@EmpPhoto,");
                strSql.Append("EmpMemu=@EmpMemu,");
                strSql.Append("DeptNo=@DeptNo,");
                strSql.Append("CompanyId=@CompanyId,");
                strSql.Append("EmpCardNo=@EmpCardNo ");

                strSql.Append(" where SjId=@SjId");
                SqlParameter[] parameters = {
					new SqlParameter("@EmpName", SqlDbType.VarChar,30),
					new SqlParameter("@EmpSex", SqlDbType.VarChar,4),
					new SqlParameter("@EmpFloor", SqlDbType.VarChar,20),
					new SqlParameter("@EmpRoomCode", SqlDbType.VarChar,20),
					new SqlParameter("@EmpTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpMobile", SqlDbType.VarChar,20),
					new SqlParameter("@EmpExtTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPosition", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPhoto", SqlDbType.Image),
					new SqlParameter("@EmpMemu", SqlDbType.VarChar,100),
					new SqlParameter("@DeptNo", SqlDbType.Int,4),
					new SqlParameter("@CompanyId", SqlDbType.Int,4),
                    new SqlParameter("@EmpCardNo", SqlDbType.VarChar,20),
                    new SqlParameter("@SjId", SqlDbType.VarChar,200)
            };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                parameters[12].Value = model.EmpCardno;
                parameters[13].Value = model.SjId;

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
                strSql.Append("update F_Employ_Info set ");
                strSql.Append("EmpName=@EmpName,");
                strSql.Append("EmpSex=@EmpSex,");
                strSql.Append("EmpFloor=@EmpFloor,");
                strSql.Append("EmpRoomCode=@EmpRoomCode,");
                strSql.Append("EmpTel=@EmpTel,");
                strSql.Append("EmpMobile=@EmpMobile,");
                strSql.Append("EmpExtTel=@EmpExtTel,");
                strSql.Append("EmpPosition=@EmpPosition,");
                strSql.Append("EmpPhoto=@EmpPhoto,");
                strSql.Append("EmpMemu=@EmpMemu,");
                strSql.Append("DeptNo=@DeptNo,");
                strSql.Append("CompanyId=@CompanyId,");
                strSql.Append("EmpCardNo=@EmpCardNo, ");
                strSql.Append("EmpNamePinYin=(select c1||c2 from get_py_zm(EmpName)) ");

                strSql.Append(" where SjId=@SjId");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmpName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@EmpSex", DbType.StringFixedLength,4),
					new NpgsqlParameter("@EmpFloor", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpRoomCode", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpMobile", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpExtTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPosition", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPhoto", NpgsqlDbType.Bytea),
					new NpgsqlParameter("@EmpMemu", DbType.StringFixedLength,100),
					new NpgsqlParameter("@DeptNo", DbType.Int32,4),
					new NpgsqlParameter("@CompanyId", DbType.Int32,4),
                    new NpgsqlParameter("@EmpCardNo", DbType.StringFixedLength,20),
					new NpgsqlParameter("@SjId", DbType.StringFixedLength,20)
            };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                parameters[12].Value = model.EmpCardno;
                parameters[13].Value = model.SjId;

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

        public Boolean isDeleted_SJP(string sjid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select 1 ");
            strSql.Append(" from tbDeleted_Employee");
            strSql.Append(" where Emp_ID='" + sjid + "'");

            return DbHelperSQL.Exists_SJP(strSql.ToString());

        }

        public bool Delete_SJP(string sjId)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from F_Employ_Info ");
                strSql.Append(" where sjid=@sjid");
                SqlParameter[] parameters = {
					new SqlParameter("@sjid", SqlDbType.VarChar,200)
                                        };
                parameters[0].Value = sjId;

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
                strSql.Append("delete from F_Employ_Info ");
                strSql.Append(" where sjid=@sjid");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@sjid", DbType.StringFixedLength,200)
                                        };
                parameters[0].Value = sjId;

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


        #endregion  Method

        public bool UpdateIPush(int empNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update F_Employ_Info ");
            strSql.Append(" set iPush=@iPush");
            strSql.Append(" where EmpNo=" + empNo);

            int ret = 0;
            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@iPush", SqlDbType.Int,4)
                };
                parameters[0].Value = 1;
                ret = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            }
            else
            {
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@iPush", DbType.Int32,4)
                                        };
                parameters[0].Value = 1;
                ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteData(int empNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" delete from F_Employ_Info ");
            strSql.Append(" where EmpNo=" + empNo);
            int ret = 0;
            if (DbHelperSQL.DbType == 1)
            {
                ret = DbHelperSQL.ExecuteSql(strSql.ToString(), null);
            }
            else
            {
                ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
            }
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetListWhereTel(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,Platformid,iStatus,iPush ");
            strSql.Append(" FROM F_Employ_Info  where EmpMobile ='" + strWhere + "'");


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
        /// 得到一个对象实体
        /// </summary>
        public Model.M_Employ_Info GetModelByCarNum(string carNum)
        {
            Model.M_Employ_Info model = new Model.M_Employ_Info();
            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,EmpCardNo,empcard_ac_grantmsg,empcard_ac_enddate from F_Employ_Info ");
                strSql.Append(" where empcarnum=@empcarnum");
                SqlParameter[] parameters = {
					new SqlParameter("@empcarnum",  SqlDbType.VarChar,32)
                                            };
                parameters[0].Value = carNum;
                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,EmpCardNo,empcard_ac_grantmsg,empcard_ac_enddate from F_Employ_Info ");
                strSql.Append(" where empcarnum=@empcarnum limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@empcarnum", DbType.StringFixedLength,32)
                                            };
                parameters[0].Value = carNum;
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["EmpNo"] != null && ds.Tables[0].Rows[0]["EmpNo"].ToString() != "")
                {
                    model.EmpNo = int.Parse(ds.Tables[0].Rows[0]["EmpNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmpName"] != null && ds.Tables[0].Rows[0]["EmpName"].ToString() != "")
                {
                    model.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpSex"] != null && ds.Tables[0].Rows[0]["EmpSex"].ToString() != "")
                {
                    model.EmpSex = ds.Tables[0].Rows[0]["EmpSex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpFloor"] != null && ds.Tables[0].Rows[0]["EmpFloor"].ToString() != "")
                {
                    model.EmpFloor = ds.Tables[0].Rows[0]["EmpFloor"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpRoomCode"] != null && ds.Tables[0].Rows[0]["EmpRoomCode"].ToString() != "")
                {
                    model.EmpRoomCode = ds.Tables[0].Rows[0]["EmpRoomCode"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpTel"] != null && ds.Tables[0].Rows[0]["EmpTel"].ToString() != "")
                {
                    model.EmpTel = ds.Tables[0].Rows[0]["EmpTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpMobile"] != null && ds.Tables[0].Rows[0]["EmpMobile"].ToString() != "")
                {
                    model.EmpMobile = ds.Tables[0].Rows[0]["EmpMobile"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpExtTel"] != null && ds.Tables[0].Rows[0]["EmpExtTel"].ToString() != "")
                {
                    model.EmpExtTel = ds.Tables[0].Rows[0]["EmpExtTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpPosition"] != null && ds.Tables[0].Rows[0]["EmpPosition"].ToString() != "")
                {
                    model.EmpPosition = ds.Tables[0].Rows[0]["EmpPosition"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpPhoto"] != null && ds.Tables[0].Rows[0]["EmpPhoto"].ToString() != "")
                {
                    model.EmpPhoto = (byte[])ds.Tables[0].Rows[0]["EmpPhoto"];
                }
                if (ds.Tables[0].Rows[0]["EmpMemu"] != null && ds.Tables[0].Rows[0]["EmpMemu"].ToString() != "")
                {
                    model.EmpMemu = ds.Tables[0].Rows[0]["EmpMemu"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DeptNo"] != null && ds.Tables[0].Rows[0]["DeptNo"].ToString() != "")
                {
                    model.DeptNo = int.Parse(ds.Tables[0].Rows[0]["DeptNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CompanyId"] != null && ds.Tables[0].Rows[0]["CompanyId"].ToString() != "")
                {
                    model.CompanyId = int.Parse(ds.Tables[0].Rows[0]["CompanyId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmpCardNo"] != null && ds.Tables[0].Rows[0]["EmpCardNo"].ToString() != "")
                {
                    model.EmpCardno = ds.Tables[0].Rows[0]["EmpCardNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["empcard_ac_grantmsg"] != null && ds.Tables[0].Rows[0]["empcard_ac_grantmsg"].ToString() != "")
                {
                    model.Empcard_ac_grantmsg = ds.Tables[0].Rows[0]["empcard_ac_grantmsg"].ToString();
                }
                if (ds.Tables[0].Rows[0]["empcard_ac_enddate"] != null && ds.Tables[0].Rows[0]["empcard_ac_enddate"].ToString() != "")
                {
                    model.EmpCard_ac_enddate = DateTime.Parse(ds.Tables[0].Rows[0]["empcard_ac_enddate"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="empNum"></param>
        /// <returns></returns>
        public Boolean ExistNum(string empNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * ");
            strSql.Append(" from f_employ_info");
            strSql.Append(" where  EmpNum=@EmpNum");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@EmpNum", SqlDbType.VarChar,64),
                                            };
                parameters[0].Value = empNum;

                return DbHelperSQL.Exists(strSql.ToString(), parameters);
            }
            else
            {
                NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@EmpNum", DbType.StringFixedLength,64),
                                               };

                parameters[0].Value = empNum;

                object obj = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
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

    }
}
