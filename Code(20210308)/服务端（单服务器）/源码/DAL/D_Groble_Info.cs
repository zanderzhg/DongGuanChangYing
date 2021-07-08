using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using Npgsql;
using ADServer.BLL;


namespace ADServer.DAL
{
    public partial class D_Groble_Info
    {
        public D_Groble_Info()
        { }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string machinecode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from F_Groble_Info");
            strSql.Append(" where machinecode=@machinecode ");
            //if (Properties.Settings.Default.DbType==1)
            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
                        new SqlParameter("@machinecode", SqlDbType.VarChar,30)
                                            };
                parameters[0].Value = machinecode;
                return DbHelperSQL.Exists(strSql.ToString(), parameters);
            }
            else
            {
                NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@machinecode", DbType.String,30)
                                               };
                parameters[0].Value = machinecode;
                DataSet ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == "0")
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
        /// 是否存在该记录--查找服务端是否设好
        /// </summary>
        public bool ExistsService(string isService)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select count(1) from F_Groble_Info");
                strSql.Append(" where set12=@set12 ");
                SqlParameter[] parameters = {
					new SqlParameter("@set12", SqlDbType.VarChar,40)			};
                parameters[0].Value = isService;

                return DbHelperSQL.Exists(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select count(1) from F_Groble_Info");
                strSql.Append(" where set12=@set12 ");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@set12", DbType.StringFixedLength,40)			};
                parameters[0].Value = isService;

                object obj = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                if (int.Parse(obj.ToString()) == 0)
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
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.M_Groble_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into F_Groble_Info(");
            strSql.Append("machinecode,port,LeaveType,ShowLeave,HideType,ShowLastVisit,MachineKind,SerialPort,PrintType,AutoLeave,Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9,Set10,Set11,Set12,Set13,Set14,Set15,Set16,Set17,Set18,Set19,Set20,Set21,RedPort,Equipment,EditVNum,OpenAc,AcRunServer,AcServerPath,AcInDoors,AcOutDoors,OpenWG,LeaveAndCancel,LtGrantdays,StGrantdays,PrintQRCode,CheckPwd,OpenEmpReception,OpenConfirmTS,OpenTelConfirm,OpenTelRecord,OpenPoliceUpload,PoliceServerPath,PoliceUploadType,OpenFaceRecognition,FaceThreshold,Finger,LedPort,LedBandrate)");
            strSql.Append(" values (");
            strSql.Append("@machinecode,@port,@LeaveType,@ShowLeave,@HideType,@ShowLastVisit,@MachineKind,@SerialPort,@PrintType,@AutoLeave,@Set1,@Set2,@Set3,@Set4,@Set5,@Set6,@Set7,@Set8,@Set9,@Set10,@Set11,@Set12,@Set13,@Set14,@Set15,@Set16,@Set17,@Set18,@Set19,@Set20,@Set21,@RedPort,@Equipment,@EditVNum,@OpenAc,@AcRunServer,@AcServerPath,@AcInDoors,@AcOutDoors,@OpenWG,@LeaveAndCancel,@LtGrantdays,@StGrantdays,@PrintQRCode,@CheckPwd,@OpenEmpReception,@OpenConfirmTS,@OpenTelConfirm,@OpenTelRecord,@OpenPoliceUpload,@PoliceServerPath,@PoliceUploadType,@OpenFaceRecognition,@FaceThreshold,@Finger,@LedPort,@LedBandrate)");
            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@machinecode", SqlDbType.VarChar,30),
                    new SqlParameter("@port", SqlDbType.Int,4),
                    new SqlParameter("@LeaveType", SqlDbType.VarChar,2),
                    new SqlParameter("@ShowLeave", SqlDbType.VarChar,2),
                    new SqlParameter("@HideType", SqlDbType.VarChar,2),
                    new SqlParameter("@ShowLastVisit", SqlDbType.VarChar,2),
                    new SqlParameter("@MachineKind", SqlDbType.VarChar,4),
                    new SqlParameter("@SerialPort", SqlDbType.Int,4),
                    new SqlParameter("@PrintType", SqlDbType.Int,4),
                    new SqlParameter("@AutoLeave", SqlDbType.Int,4),
                    new SqlParameter("@Set1", SqlDbType.VarChar,40),
                    new SqlParameter("@Set2", SqlDbType.VarChar,40),
                    new SqlParameter("@Set3", SqlDbType.VarChar,40),
                    new SqlParameter("@Set4", SqlDbType.VarChar,40),
                    new SqlParameter("@Set5", SqlDbType.VarChar,40),
                    new SqlParameter("@Set6", SqlDbType.VarChar,40),
                    new SqlParameter("@Set7", SqlDbType.VarChar,40),
                    new SqlParameter("@Set8", SqlDbType.VarChar,40),
                    new SqlParameter("@Set9", SqlDbType.VarChar,40),
                    new SqlParameter("@Set10", SqlDbType.VarChar,40),
                    new SqlParameter("@Set11", SqlDbType.VarChar,200),
                    new SqlParameter("@Set12", SqlDbType.VarChar,40),
                    new SqlParameter("@Set13", SqlDbType.VarChar,40),
                    new SqlParameter("@Set14", SqlDbType.VarChar,40),
                    new SqlParameter("@Set15", SqlDbType.VarChar,40),
                    new SqlParameter("@Set16", SqlDbType.VarChar,40),
                    new SqlParameter("@Set17", SqlDbType.VarChar,40),
                    new SqlParameter("@Set18", SqlDbType.VarChar,40),
                    new SqlParameter("@Set19", SqlDbType.VarChar,40),
                    new SqlParameter("@Set20", SqlDbType.VarChar,40),
                    new SqlParameter("@Set21", SqlDbType.VarChar,40),
                    new SqlParameter("@RedPort",SqlDbType.Int,4),
                    new SqlParameter("@Equipment",SqlDbType.VarChar,50),
                    new SqlParameter("@EditVNum",SqlDbType.VarChar,2),
                    new SqlParameter("@OpenAc",SqlDbType.VarChar,2),
                    new SqlParameter("@AcRunServer",SqlDbType.VarChar,2),
                    new SqlParameter("@AcServerPath",SqlDbType.VarChar,50),
                    new SqlParameter("@AcInDoors",SqlDbType.VarChar,200),
                    new SqlParameter("@AcOutDoors",SqlDbType.VarChar,200),
                    new SqlParameter("@OpenWG",SqlDbType.VarChar,2),
                    new SqlParameter("@LeaveAndCancel",SqlDbType.VarChar,2),
                    new SqlParameter("@LtGrantdays",SqlDbType.Int,4),
                    new SqlParameter("@StGrantdays",SqlDbType.Int,4),
                    new SqlParameter("@PrintQRCode",SqlDbType.VarChar,2),
                    new SqlParameter("@CheckPwd",SqlDbType.VarChar,2),
                    new SqlParameter("@OpenEmpReception",SqlDbType.VarChar,2),
                    new SqlParameter("@OpenConfirmTS",SqlDbType.VarChar,2),
                    new SqlParameter("@OpenTelConfirm",SqlDbType.VarChar,2),
                    new SqlParameter("@OpenTelRecord",SqlDbType.VarChar,2),
                    new SqlParameter("@OpenPoliceUpload",SqlDbType.VarChar,2),
                    new SqlParameter("@PoliceServerPath",SqlDbType.VarChar,200),
                    new SqlParameter("@PoliceUploadType",SqlDbType.Int,4),
                    new SqlParameter("@OpenFaceRecognition",SqlDbType.VarChar,2),
                    new SqlParameter("@FaceThreshold",SqlDbType.Float),
                    new SqlParameter("@Finger",SqlDbType.VarChar,20),
                    new SqlParameter("@LedPort",SqlDbType.VarChar,20),
                    new SqlParameter("@LedBandrate",SqlDbType.VarChar,20)
                                           };

                parameters[0].Value = model.machinecode;
                parameters[1].Value = model.port;
                parameters[2].Value = model.LeaveType;
                parameters[3].Value = model.ShowLeave;
                parameters[4].Value = model.HideType;
                parameters[5].Value = model.ShowLastVisit;
                parameters[6].Value = model.MachineKind;
                parameters[7].Value = model.SerialPort;
                parameters[8].Value = model.PrintType;
                parameters[9].Value = model.AutoLeave;
                parameters[10].Value = model.Set1;
                parameters[11].Value = model.Set2;
                parameters[12].Value = model.Set3;
                parameters[13].Value = model.Set4;
                parameters[14].Value = model.Set5;
                parameters[15].Value = model.Set6;
                parameters[16].Value = model.Set7;
                parameters[17].Value = model.Set8;
                parameters[18].Value = model.Set9;
                parameters[19].Value = model.Set10;
                parameters[20].Value = model.Set11;
                parameters[21].Value = model.Set12;
                parameters[22].Value = model.Set13;
                parameters[23].Value = model.Set14;
                parameters[24].Value = model.Set15;
                parameters[25].Value = model.Set16;
                parameters[26].Value = model.Set17;
                parameters[27].Value = model.Set18;
                parameters[28].Value = model.Set19;
                parameters[29].Value = model.Set20;
                parameters[30].Value = model.Set21;
                parameters[31].Value = model.Redport;
                parameters[32].Value = model.Equipment;
                parameters[33].Value = model.EditVNum;
                parameters[34].Value = model.OpenAc;
                parameters[35].Value = model.AcRunServer;
                parameters[36].Value = model.AcServerPath;
                parameters[37].Value = model.AcInDoors;
                parameters[38].Value = model.AcOutDoors;
                parameters[39].Value = model.OpenWG;
                parameters[40].Value = model.LeaveAndCancel;
                parameters[41].Value = model.LtGrantdays;
                parameters[42].Value = model.StGrantdays;
                parameters[43].Value = model.PrintQRCode;
                parameters[44].Value = model.CheckPwd;
                parameters[45].Value = model.OpenEmpRecption;
                parameters[46].Value = model.OpenConfirmTS;
                parameters[47].Value = model.OpenTelConfirm;
                parameters[48].Value = model.OpenTelRecord;
                parameters[49].Value = model.OpenPoliceUpload;
                parameters[50].Value = model.PoliceServerPath;
                parameters[51].Value = model.PoliceUploadType;
                parameters[52].Value = model.OpenFaceRecognition;
                parameters[53].Value = model.FaceThreshold;
                parameters[54].Value = model.Finger;
                parameters[55].Value = model.LedPort;
                parameters[56].Value = model.LedBandrate;

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
                DbParameter[] parameters = {
                    new NpgsqlParameter("@machinecode", DbType.StringFixedLength,30),
                    new NpgsqlParameter("@port", DbType.Int32,4),
                    new NpgsqlParameter("@LeaveType", DbType.StringFixedLength,2),
                    new NpgsqlParameter("@ShowLeave", DbType.StringFixedLength,2),
                    new NpgsqlParameter("@HideType", DbType.StringFixedLength,2),
                    new NpgsqlParameter("@ShowLastVisit", DbType.StringFixedLength,2),
                    new NpgsqlParameter("@MachineKind", DbType.StringFixedLength,4),
                    new NpgsqlParameter("@SerialPort", DbType.Int32,4),
                    new NpgsqlParameter("@PrintType", DbType.Int32,4),
                    new NpgsqlParameter("@AutoLeave", DbType.Int32,4),
                    new NpgsqlParameter("@Set1", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set2", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set3", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set4", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set5", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set6", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set7", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set8", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set9", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set10", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set11", DbType.StringFixedLength,200),
                    new NpgsqlParameter("@Set12", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set13", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set14", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set15", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set16", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set17", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set18", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set19", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set20", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set21", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@RedPort",DbType.Int32,4),
                    new NpgsqlParameter("@Equipment",DbType.StringFixedLength,50),
                    new NpgsqlParameter("@EditVNum",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@OpenAc",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@AcRunServer",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@AcServerPath",DbType.StringFixedLength,50),
                    new NpgsqlParameter("@AcInDoors",DbType.StringFixedLength,200),
                    new NpgsqlParameter("@AcOutDoors",DbType.StringFixedLength,200),
                    new NpgsqlParameter("@OpenWG",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@LeaveAndCancel",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@LtGrantdays",DbType.Int32,4),
                    new NpgsqlParameter("@StGrantdays",DbType.Int32,4),
                    new NpgsqlParameter("@PrintQRCode",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@CheckPwd",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@OpenEmpReception",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@OpenConfirmTS",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@OpenTelConfirm",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@OpenTelRecord",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@OpenPoliceUpload",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@PoliceServerPath",DbType.StringFixedLength,200),
                    new NpgsqlParameter("@PoliceUploadType",DbType.Int32,4),
                    new NpgsqlParameter("@OpenFaceRecognition",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@FaceThreshold",DbType.Decimal),
                    new NpgsqlParameter("@Finger",DbType.StringFixedLength,20),
                    new NpgsqlParameter("@LedPort",DbType.StringFixedLength,20),
                    new NpgsqlParameter("@LedBandrate",DbType.StringFixedLength,20)
                    
            };
                parameters[0].Value = model.machinecode;
                parameters[1].Value = model.port;
                parameters[2].Value = model.LeaveType;
                parameters[3].Value = model.ShowLeave;
                parameters[4].Value = model.HideType;
                parameters[5].Value = model.ShowLastVisit;
                parameters[6].Value = model.MachineKind;
                parameters[7].Value = model.SerialPort;
                parameters[8].Value = model.PrintType;
                parameters[9].Value = model.AutoLeave;
                parameters[10].Value = model.Set1;
                parameters[11].Value = model.Set2;
                parameters[12].Value = model.Set3;
                parameters[13].Value = model.Set4;
                parameters[14].Value = model.Set5;
                parameters[15].Value = model.Set6;
                parameters[16].Value = model.Set7;
                parameters[17].Value = model.Set8;
                parameters[18].Value = model.Set9;
                parameters[19].Value = model.Set10;
                parameters[20].Value = model.Set11;
                parameters[21].Value = model.Set12;
                parameters[22].Value = model.Set13;
                parameters[23].Value = model.Set14;
                parameters[24].Value = model.Set15;
                parameters[25].Value = model.Set16;
                parameters[26].Value = model.Set17;
                parameters[27].Value = model.Set18;
                parameters[28].Value = model.Set19;
                parameters[29].Value = model.Set20;
                parameters[30].Value = model.Set21;
                parameters[31].Value = model.Redport;
                parameters[32].Value = model.Equipment;
                parameters[33].Value = model.EditVNum;
                parameters[34].Value = model.OpenAc;
                parameters[35].Value = model.AcRunServer;
                parameters[36].Value = model.AcServerPath;
                parameters[37].Value = model.AcInDoors;
                parameters[38].Value = model.AcOutDoors;
                parameters[39].Value = model.OpenWG;
                parameters[40].Value = model.LeaveAndCancel;
                parameters[41].Value = model.LtGrantdays;
                parameters[42].Value = model.StGrantdays;
                parameters[43].Value = model.PrintQRCode;
                parameters[44].Value = model.CheckPwd;
                parameters[45].Value = model.OpenEmpRecption;
                parameters[46].Value = model.OpenConfirmTS;
                parameters[47].Value = model.OpenTelConfirm;
                parameters[48].Value = model.OpenTelRecord;
                parameters[49].Value = model.OpenPoliceUpload;
                parameters[50].Value = model.PoliceServerPath;
                parameters[51].Value = model.PoliceUploadType;
                parameters[52].Value = model.OpenFaceRecognition;
                parameters[53].Value = model.FaceThreshold;
                parameters[54].Value = model.Finger;
                parameters[55].Value = model.LedPort;
                parameters[56].Value = model.LedBandrate;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.M_Groble_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update F_Groble_Info set ");
            strSql.Append("port=@port,");
            strSql.Append("LeaveType=@LeaveType,");
            strSql.Append("ShowLeave=@ShowLeave,");
            strSql.Append("HideType=@HideType,");
            strSql.Append("ShowLastVisit=@ShowLastVisit,");
            strSql.Append("MachineKind=@MachineKind,");
            strSql.Append("SerialPort=@SerialPort,");
            strSql.Append("PrintType=@PrintType,");
            strSql.Append("AutoLeave=@AutoLeave,");
            strSql.Append("Set1=@Set1,");
            strSql.Append("Set2=@Set2,");
            strSql.Append("Set3=@Set3,");
            strSql.Append("Set4=@Set4,");
            strSql.Append("Set5=@Set5,");
            strSql.Append("Set6=@Set6,");
            strSql.Append("Set7=@Set7,");
            strSql.Append("Set8=@Set8,");
            strSql.Append("Set9=@Set9,");
            strSql.Append("Set10=@Set10,");
            strSql.Append("Set11=@Set11,");
            strSql.Append("Set12=@Set12,");
            strSql.Append("Set13=@Set13,");
            strSql.Append("Set14=@Set14,");
            strSql.Append("Set15=@Set15,");
            strSql.Append("Set16=@Set16,");
            strSql.Append("Set17=@Set17,");
            strSql.Append("Set18=@Set18,");
            strSql.Append("Set19=@Set19,");
            strSql.Append("Set20=@Set20,");
            strSql.Append("Set21=@Set21,");
            strSql.Append("RedPort=@RedPort,");
            strSql.Append("Equipment=@Equipment,");
            strSql.Append("EditVNum=@EditVNum,");
            strSql.Append("OpenAc=@OpenAc,");
            strSql.Append("AcRunServer=@AcRunServer,");
            strSql.Append("AcServerPath=@AcServerPath,");
            strSql.Append("AcInDoors=@AcInDoors,");
            strSql.Append("AcOutDoors=@AcOutDoors,");
            strSql.Append("OpenWG=@OpenWG,");
            strSql.Append("LeaveAndCancel=@LeaveAndCancel,");
            strSql.Append("LtGrantdays=@LtGrantdays,");
            strSql.Append("StGrantdays=@StGrantdays,");
            strSql.Append("PrintQRCode=@PrintQRCode,");
            strSql.Append("CheckPwd=@CheckPwd,");
            strSql.Append("OpenEmpReception=@OpenEmpReception,");
            strSql.Append("OpenConfirmTS=@OpenConfirmTS,");
            strSql.Append("TSIp=@TSIp,");
            strSql.Append("TSPort=@TSPort,");
            strSql.Append("OpenTelConfirm=@OpenTelConfirm,");
            strSql.Append("OpenTelRecord=@OpenTelRecord,");
            strSql.Append("OpenPoliceUpload=@OpenPoliceUpload,");
            strSql.Append("PoliceServerPath=@PoliceServerPath,");
            strSql.Append("PoliceUploadType=@PoliceUploadType,");
            strSql.Append("OpenFaceRecognition=@OpenFaceRecognition,");
            strSql.Append("FaceThreshold=@FaceThreshold,");
            strSql.Append("Finger=@Finger,");
            strSql.Append("LedPort=@LedPort,");
            strSql.Append("LedBandrate=@LedBandrate");
            strSql.Append(" where machinecode=@machinecode ");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@port", SqlDbType.Int,4),
					new SqlParameter("@LeaveType", SqlDbType.VarChar,2),
					new SqlParameter("@ShowLeave", SqlDbType.VarChar,2),
					new SqlParameter("@HideType", SqlDbType.VarChar,2),
					new SqlParameter("@ShowLastVisit", SqlDbType.VarChar,2),
					new SqlParameter("@MachineKind", SqlDbType.VarChar,4),
					new SqlParameter("@SerialPort", SqlDbType.Int,4),
					new SqlParameter("@PrintType", SqlDbType.Int,4),
					new SqlParameter("@AutoLeave", SqlDbType.Int,4),
					new SqlParameter("@Set1", SqlDbType.VarChar,40),
					new SqlParameter("@Set2", SqlDbType.VarChar,40),
					new SqlParameter("@Set3", SqlDbType.VarChar,40),
					new SqlParameter("@Set4", SqlDbType.VarChar,40),
					new SqlParameter("@Set5", SqlDbType.VarChar,40),
					new SqlParameter("@Set6", SqlDbType.VarChar,40),
					new SqlParameter("@Set7", SqlDbType.VarChar,40),
					new SqlParameter("@Set8", SqlDbType.VarChar,40),
					new SqlParameter("@Set9", SqlDbType.VarChar,40),
					new SqlParameter("@Set10", SqlDbType.VarChar,20),
                    new SqlParameter("@Set11", SqlDbType.VarChar,200),
					new SqlParameter("@Set12", SqlDbType.VarChar,40),
					new SqlParameter("@Set13", SqlDbType.VarChar,40),
					new SqlParameter("@Set14", SqlDbType.VarChar,40),
					new SqlParameter("@Set15", SqlDbType.VarChar,40),
					new SqlParameter("@Set16", SqlDbType.VarChar,40),
					new SqlParameter("@Set17", SqlDbType.VarChar,40),
					new SqlParameter("@Set18", SqlDbType.VarChar,40),
					new SqlParameter("@Set19", SqlDbType.VarChar,40),
					new SqlParameter("@Set20", SqlDbType.VarChar,40),
               		new SqlParameter("@Set21", SqlDbType.VarChar,40),
                    new SqlParameter("@RedPort",SqlDbType.Int,4),
                    new SqlParameter("@Equipment", SqlDbType.VarChar,50),
                    new SqlParameter("@machinecode",SqlDbType.VarChar,30),
                    new SqlParameter("@EditVNum",SqlDbType.VarChar,2),
                    new SqlParameter("@OpenAc",SqlDbType.VarChar,2),
                    new SqlParameter("@AcRunServer",SqlDbType.VarChar,2),
                    new SqlParameter("@AcServerPath",SqlDbType.VarChar,200),
                    new SqlParameter("@AcInDoors",SqlDbType.VarChar,200),
                    new SqlParameter("@AcOutDoors",SqlDbType.VarChar,200),
                    new SqlParameter("@OpenWG",SqlDbType.VarChar,2),
                    new SqlParameter("@LeaveAndCancel",SqlDbType.VarChar,2),
                    new SqlParameter("@LtGrantdays",SqlDbType.Int,4),
                    new SqlParameter("@StGrantdays",SqlDbType.Int,4),
                    new SqlParameter("@PrintQRCode",SqlDbType.VarChar,2),
                    new SqlParameter("@CheckPwd",SqlDbType.VarChar,2),
                    new SqlParameter("@OpenEmpReception",SqlDbType.VarChar,2),
                    new SqlParameter("@OpenConfirmTS",SqlDbType.VarChar,2),
                    new SqlParameter("@TSIp",SqlDbType.VarChar,50),
                    new SqlParameter("@TSPort",SqlDbType.VarChar,50),
                    new SqlParameter("@OpenTelConfirm",SqlDbType.VarChar,2),
                    new SqlParameter("@OpenTelRecord",SqlDbType.VarChar,2),
                    new SqlParameter("@OpenPoliceUpload",SqlDbType.VarChar,2),
                    new SqlParameter("@PoliceServerPath",SqlDbType.VarChar,200),
                    new SqlParameter("@PoliceUploadType",SqlDbType.Int,4),
                    new SqlParameter("@OpenFaceRecognition",SqlDbType.VarChar,2),
                    new SqlParameter("@FaceThreshold",SqlDbType.Float),
                    new SqlParameter("@Finger", SqlDbType.VarChar,20),
                    new SqlParameter("@LedPort", SqlDbType.VarChar,20),
                    new SqlParameter("@LedBandrate", SqlDbType.VarChar,20)
                      
           };

                parameters[0].Value = model.port;
                parameters[1].Value = model.LeaveType;
                parameters[2].Value = model.ShowLeave;
                parameters[3].Value = model.HideType;
                parameters[4].Value = model.ShowLastVisit;
                parameters[5].Value = model.MachineKind;
                parameters[6].Value = model.SerialPort;
                parameters[7].Value = model.PrintType;
                parameters[8].Value = model.AutoLeave;
                parameters[9].Value = model.Set1;
                parameters[10].Value = model.Set2;
                parameters[11].Value = model.Set3;
                parameters[12].Value = model.Set4;
                parameters[13].Value = model.Set5;
                parameters[14].Value = model.Set6;
                parameters[15].Value = model.Set7;
                parameters[16].Value = model.Set8;
                parameters[17].Value = model.Set9;
                parameters[18].Value = model.Set10;
                parameters[19].Value = model.Set11;
                parameters[20].Value = model.Set12;
                parameters[21].Value = model.Set13;
                parameters[22].Value = model.Set14;
                parameters[23].Value = model.Set15;
                parameters[24].Value = model.Set16;
                parameters[25].Value = model.Set17;
                parameters[26].Value = model.Set18;
                parameters[27].Value = model.Set19;
                parameters[28].Value = model.Set20;
                parameters[29].Value = model.Set21;
                parameters[30].Value = model.Redport;
                parameters[31].Value = model.Equipment;
                parameters[32].Value = model.machinecode;
                parameters[33].Value = model.EditVNum;
                parameters[34].Value = model.OpenAc;
                parameters[35].Value = model.AcRunServer;
                parameters[36].Value = model.AcServerPath;
                parameters[37].Value = model.AcInDoors;
                parameters[38].Value = model.AcOutDoors;
                parameters[39].Value = model.OpenWG;
                parameters[40].Value = model.LeaveAndCancel;
                parameters[41].Value = model.LtGrantdays;
                parameters[42].Value = model.StGrantdays;
                parameters[43].Value = model.PrintQRCode;
                parameters[44].Value = model.CheckPwd;
                parameters[45].Value = model.OpenEmpRecption;
                parameters[46].Value = model.OpenConfirmTS;
                parameters[47].Value = model.TSIp;
                parameters[48].Value = model.TSPort;
                parameters[49].Value = model.OpenTelConfirm;
                parameters[50].Value = model.OpenTelRecord;
                parameters[51].Value = model.OpenPoliceUpload;
                parameters[52].Value = model.PoliceServerPath;
                parameters[53].Value = model.PoliceUploadType;
                parameters[54].Value = model.OpenFaceRecognition;
                parameters[55].Value = model.FaceThreshold;
                parameters[56].Value = model.Finger;
                parameters[57].Value = model.LedPort;
                parameters[58].Value = model.LedBandrate;

                int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

                strSql = new StringBuilder();
                strSql.Append("update F_Groble_Info set ");
                strSql.Append("LeaveAndCancel='" + model.LeaveAndCancel + "'");
                DbHelperSQL.ExecuteSql(strSql.ToString()); //统一所有设备配置的临时卡一进一出权限

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
                                                   new NpgsqlParameter("@port", DbType.Int32,4),
					new NpgsqlParameter("@LeaveType", DbType.StringFixedLength,2),
					new NpgsqlParameter("@ShowLeave", DbType.StringFixedLength,2),
					new NpgsqlParameter("@HideType", DbType.StringFixedLength,2),
					new NpgsqlParameter("@ShowLastVisit", DbType.StringFixedLength,2),
					new NpgsqlParameter("@MachineKind", DbType.StringFixedLength,4),
					new NpgsqlParameter("@SerialPort", DbType.Int32,4),
					new NpgsqlParameter("@PrintType", DbType.Int32,4),
					new NpgsqlParameter("@AutoLeave", DbType.Int32,4),
					new NpgsqlParameter("@Set1", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set2", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set3", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set4", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set5", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set6", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set7", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set8", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set9", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set10", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@Set11", DbType.StringFixedLength,200),
					new NpgsqlParameter("@Set12", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set13", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set14", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set15", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set16", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set17", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set18", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set19", DbType.StringFixedLength,40),
					new NpgsqlParameter("@Set20", DbType.StringFixedLength,40),
               		new NpgsqlParameter("@Set21", DbType.StringFixedLength,40),
                    new NpgsqlParameter("@RedPort",DbType.Int32,4),
                    new NpgsqlParameter("@Equipment", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@machinecode",DbType.StringFixedLength,30),
                    new NpgsqlParameter("@EditVNum",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@OpenAc",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@AcRunServer",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@AcServerPath",DbType.StringFixedLength,200),
                    new NpgsqlParameter("@AcInDoors",DbType.StringFixedLength,200),
                    new NpgsqlParameter("@AcOutDoors",DbType.StringFixedLength,200),
                    new NpgsqlParameter("@OpenWG",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@LeaveAndCancel",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@LtGrantdays",DbType.Int32,4),
                    new NpgsqlParameter("@StGrantdays",DbType.Int32,4),
                    new NpgsqlParameter("@PrintQRCode",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@CheckPwd",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@OpenEmpReception",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@OpenConfirmTS",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@TSIp",DbType.StringFixedLength,50),
                    new NpgsqlParameter("@TSPort",DbType.StringFixedLength,50),
                    new NpgsqlParameter("@OpenTelConfirm",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@OpenTelRecord",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@OpenPoliceUpload",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@PoliceServerPath",DbType.StringFixedLength,200),
                    new NpgsqlParameter("@PoliceUploadType",DbType.Int32,4),
                    new NpgsqlParameter("@OpenFaceRecognition",DbType.StringFixedLength,2),
                    new NpgsqlParameter("@FaceThreshold",DbType.Decimal),
                    new NpgsqlParameter("@Finger",DbType.StringFixedLength,20),
                    new NpgsqlParameter("@LedPort",DbType.StringFixedLength,20),
                    new NpgsqlParameter("@LedBandrate",DbType.StringFixedLength,20)
           };

                parameters[0].Value = model.port;
                parameters[1].Value = model.LeaveType;
                parameters[2].Value = model.ShowLeave;
                parameters[3].Value = model.HideType;
                parameters[4].Value = model.ShowLastVisit;
                parameters[5].Value = model.MachineKind;
                parameters[6].Value = model.SerialPort;
                parameters[7].Value = model.PrintType;
                parameters[8].Value = model.AutoLeave;
                parameters[9].Value = model.Set1;
                parameters[10].Value = model.Set2;
                parameters[11].Value = model.Set3;
                parameters[12].Value = model.Set4;
                parameters[13].Value = model.Set5;
                parameters[14].Value = model.Set6;
                parameters[15].Value = model.Set7;
                parameters[16].Value = model.Set8;
                parameters[17].Value = model.Set9;
                parameters[18].Value = model.Set10;
                parameters[19].Value = model.Set11;
                parameters[20].Value = model.Set12;
                parameters[21].Value = model.Set13;
                parameters[22].Value = model.Set14;
                parameters[23].Value = model.Set15;
                parameters[24].Value = model.Set16;
                parameters[25].Value = model.Set17;
                parameters[26].Value = model.Set18;
                parameters[27].Value = model.Set19;
                parameters[28].Value = model.Set20;
                parameters[29].Value = model.Set21;
                parameters[30].Value = model.Redport;
                parameters[31].Value = model.Equipment;
                parameters[32].Value = model.machinecode;
                parameters[33].Value = model.EditVNum;
                parameters[34].Value = model.OpenAc;
                parameters[35].Value = model.AcRunServer;
                parameters[36].Value = model.AcServerPath;
                parameters[37].Value = model.AcInDoors;
                parameters[38].Value = model.AcOutDoors;
                parameters[39].Value = model.OpenWG;
                parameters[40].Value = model.LeaveAndCancel;
                parameters[41].Value = model.LtGrantdays;
                parameters[42].Value = model.StGrantdays;
                parameters[43].Value = model.PrintQRCode;
                parameters[44].Value = model.CheckPwd;
                parameters[45].Value = model.OpenEmpRecption;
                parameters[46].Value = model.OpenConfirmTS;
                parameters[47].Value = model.TSIp;
                parameters[48].Value = model.TSPort;
                parameters[49].Value = model.OpenTelConfirm;
                parameters[50].Value = model.OpenTelRecord;
                parameters[51].Value = model.OpenPoliceUpload;
                parameters[52].Value = model.PoliceServerPath;
                parameters[53].Value = model.PoliceUploadType;
                parameters[54].Value = model.OpenFaceRecognition;
                parameters[55].Value = model.FaceThreshold;
                parameters[56].Value = model.Finger;
                parameters[57].Value = model.LedPort;
                parameters[58].Value = model.LedBandrate;

                int rows = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);

                strSql = new StringBuilder();
                strSql.Append("update F_Groble_Info set ");
                strSql.Append("LeaveAndCancel='" + model.LeaveAndCancel + "'");
                new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());//统一所有设备配置的临时卡一进一出权限

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
        public Model.M_Groble_Info GetModel()
        {
            Model.M_Groble_Info model = new Model.M_Groble_Info();

            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 machinecode,port,LeaveType,ShowLeave,HideType,ShowLastVisit,MachineKind,SerialPort,PrintType,AutoLeave,RedPort,Equipment,EditVNum,OpenAc,AcRunServer,AcServerPath,AcInDoors,AcOutDoors,OpenWG,LeaveAndCancel,LtGrantdays,StGrantdays,PrintQRCode,CheckPwd,OpenEmpReception,OpenConfirmTS,TSIp,TSPort,OpenTelConfirm,OpenTelRecord,OpenPoliceUpload,PoliceServerPath,PoliceUploadType,OpenFaceRecognition,FaceThreshold,Finger,LedPort,LedBandrate,Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9,Set10,Set11,Set12,Set13,Set14,Set15,Set16,Set17,Set18,Set19,Set20,Set21 from F_Groble_Info ");
                ds = DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select machinecode,port,LeaveType,ShowLeave,HideType,ShowLastVisit,MachineKind,SerialPort,PrintType,AutoLeave,RedPort,Equipment,EditVNum,OpenAc,AcRunServer,AcServerPath,AcInDoors,AcOutDoors,OpenWG,LeaveAndCancel,LtGrantdays,StGrantdays,PrintQRCode,CheckPwd,OpenEmpReception,OpenConfirmTS,TSIp,TSPort,OpenTelConfirm,OpenTelRecord,OpenPoliceUpload,PoliceServerPath,PoliceUploadType,OpenFaceRecognition,FaceThreshold,Finger,LedPort,LedBandrate,Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9,Set10,Set11,Set12,Set13,Set14,Set15,Set16,Set17,Set18,Set19,Set20,Set21 from F_Groble_Info ");
                strSql.Append(" limit 1");
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["LeaveType"] != null && ds.Tables[0].Rows[0]["LeaveType"].ToString() != "")
                {
                    model.LeaveType = ds.Tables[0].Rows[0]["LeaveType"].ToString();
                }
            
                if (ds.Tables[0].Rows[0]["LeaveAndCancel"] != null && ds.Tables[0].Rows[0]["LeaveAndCancel"].ToString() != "")
                {
                    model.LeaveAndCancel = ds.Tables[0].Rows[0]["LeaveAndCancel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LtGrantdays"] != null && ds.Tables[0].Rows[0]["LtGrantdays"].ToString() != "")
                {
                    model.LtGrantdays = int.Parse(ds.Tables[0].Rows[0]["LtGrantdays"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Set2"] != null && ds.Tables[0].Rows[0]["Set2"].ToString() != "")
                {
                    model.Set2 = ds.Tables[0].Rows[0]["Set2"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到5s服务端通信ip和端口
        /// </summary>
        public Model.M_Groble_Info GetServiceModel(string isService)
        {
            Model.M_Groble_Info model = new Model.M_Groble_Info();
            DataSet ds = null;

            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 Set8,Set9 from F_Groble_Info ");
                strSql.Append(" where set12=@set12 ");
                SqlParameter[] parameters = {
					new SqlParameter("@set12", SqlDbType.VarChar,40)		
                                            };
                parameters[0].Value = isService;
                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select Set8,Set9 from F_Groble_Info ");
                strSql.Append(" where set12=@set12 limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@set12", DbType.StringFixedLength,40)		
                                            };
                parameters[0].Value = isService;

                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }


            if (ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[0].Rows[0]["Set8"] != null && ds.Tables[0].Rows[0]["Set8"].ToString() != "")
                {
                    model.Set8 = ds.Tables[0].Rows[0]["Set8"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Set9"] != null && ds.Tables[0].Rows[0]["Set9"].ToString() != "")
                {
                    model.Set9 = ds.Tables[0].Rows[0]["Set9"].ToString();
                }


                return model;
            }
            else
            {
                return null;
            }
        }


        public void UpdateLeaveAndCancel(string isLeaveAndCancel)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update F_Groble_Info set ");
                strSql.Append("leaveandcancel=@leaveandcancel");
                SqlParameter[] parameters = {
					new SqlParameter("@leaveandcancel", SqlDbType.VarChar,2)
                                                  };
                parameters[0].Value = isLeaveAndCancel;

                DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update F_Groble_Info set ");
                strSql.Append("leaveandcancel=@leaveandcancel");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@leaveandcancel", DbType.StringFixedLength,2)
                                                  };
                parameters[0].Value = isLeaveAndCancel;

                new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);

            }
        }

        public void UpdateGrantDays(decimal days)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update F_Groble_Info set ");
                strSql.Append("ltgrantdays=@ltgrantdays,set2=@set2");
                SqlParameter[] parameters = {
					new SqlParameter("@ltgrantdays", SqlDbType.Int,4),
                    new SqlParameter("@set2", SqlDbType.VarChar,20)
                                                  };
                parameters[0].Value = days;
                parameters[1].Value = days;

                DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update F_Groble_Info set ");
                strSql.Append("ltgrantdays=@ltgrantdays,set2=@set2");
                NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@ltgrantdays", DbType.Int32,4),
                    new NpgsqlParameter("@set2", DbType.StringFixedLength,20)
                                                  };
                parameters[0].Value = days;
                parameters[1].Value = days;

                new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }
        }

    }
}
