using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ADServer.BLL
{
    public class B_VisitorFaceRecognition_Info
    {
        private readonly ADServer.DAL.D_VisitorFaceRecognition_Info dal = new ADServer.DAL.D_VisitorFaceRecognition_Info();
        public B_VisitorFaceRecognition_Info()
        { }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ADServer.Model.M_VisitorFaceRecognition_Info model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(ADServer.Model.M_VisitorFaceRecognition_Info model)
        {
            return dal.Update(model);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string visitorNo)
        {
            return dal.Delete(visitorNo);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ADServer.Model.M_VisitorFaceRecognition_Info GetModel(string visitno)
        {
            return dal.GetEntity(visitno);
        }

        public void Process_OverdueFaceList()
        {
            DataSet ds = GetOverdueFaceList();
            B_DeleteFaceError_Info delFaceError = new B_DeleteFaceError_Info();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string outerid = row["outerid"].ToString();
                string grantdevicelist = row["grantdevicelist"].ToString();
                List<string> deviceIDlist = new List<string>(grantdevicelist.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));//获取授权列表
                var deleteList = deviceIDlist.Select<string, int>(q => Convert.ToInt32(q)).ToList();

                foreach (var deviceID in deleteList)
                {
                    ADServer.Model.M_DeleteFaceError_Info m = new ADServer.Model.M_DeleteFaceError_Info();
                    m.deviceid = deviceID;
                    m.deltimes = 0;
                    m.outerid = outerid;
                    delFaceError.Add(m);
                    ADServer.DAL.LogNet.WriteLog("服务端", "Process_OverdueFaceList处理记录[过期处理：" + outerid + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                }
                Delete(outerid);
            }
        }

        /// <summary>
        /// 获取过期人脸
        /// </summary>
        /// <returns></returns>
        public DataSet GetOverdueFaceList()
        {
            return dal.GetOverdueFaceList();
        }


    }
}
