using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ADServer.DAL
{
    public class D_Police_GZ
    {
        public DataSet GetUploadList()
        {
            DataSet myds = new DataSet();

            if (DbHelperSQL.DbType == 1)
            {
                string sql = "SELECT VisitNo,CertNumber,VisitorName,VisitorSex,VisitorAddress,Codebar,VisitorPhoto,CertKindName,intime,reasonname,field2,VisitorTel FROM F_VisitList_Info where (NBUpload is null or NBUpload =0)";
                sql += " and convert(varchar(12),InTime,111) =convert(varchar(12),getdate(),111)";
                myds = DbHelperSQL.Query(sql);
            }
            else
            {
                string sql = "SELECT VisitNo,CertNumber,VisitorName,VisitorSex,VisitorAddress,Codebar,VisitorPhoto,CertKindName,intime,reasonname,field2,VisitorTel FROM F_VisitList_Info where (NBUpload is null or NBUpload =0)";
                sql += " and (InTime between CURRENT_DATE and CURRENT_DATE+integer '1')";
                myds = new PostgreHelper().ExecuteQuery(DAL.DataBase.postgreConn(), CommandType.Text, sql);
            }
            return myds;
        }

    }
}
