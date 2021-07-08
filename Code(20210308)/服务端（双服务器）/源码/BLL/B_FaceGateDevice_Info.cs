using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ADServer.DAL;
using ADServer.Model;

namespace ADServer.BLL
{
    public class B_FaceGateDevice_Info
    {
        private readonly D_FaceGateDevice_Info dal = new D_FaceGateDevice_Info();

        public int Add(M_FaceGateDevice_Info model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int deviceID)
        {
            return dal.Delete(deviceID);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool  Update(M_FaceGateDevice_Info model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<M_FaceGateDevice_Info> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return null;
            //return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public M_FaceGateDevice_Info GetModel(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return DataTableToList(ds.Tables[0])[0];
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public M_FaceGateDevice_Info GetModelByID(int deviceID)
        {
            DataSet ds = dal.GetModelByID(deviceID);
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return DataTableToList(ds.Tables[0])[0];
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<M_FaceGateDevice_Info> DataTableToList(DataTable dt)
        {
            List<M_FaceGateDevice_Info> modelList = new List<M_FaceGateDevice_Info>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                M_FaceGateDevice_Info model=null;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new M_FaceGateDevice_Info();
                    if (dt.Rows[n]["device_id"] != null && dt.Rows[n]["device_id"].ToString() != "")
                    {
                        model.DeviceID = int.Parse(dt.Rows[n]["device_id"].ToString());
                    }

                    if (dt.Rows[n]["passageway_id"] != null && dt.Rows[n]["passageway_id"].ToString() != "")
                    {
                        model.PassagewayID = int.Parse(dt.Rows[n]["passageway_id"].ToString());
                    }

                    if (dt.Rows[n]["device_name"] != null && dt.Rows[n]["device_name"].ToString() != "")
                    {
                        model.DeviceName = dt.Rows[n]["device_name"].ToString();
                    }

                    if (dt.Rows[n]["device_type"] != null && dt.Rows[n]["device_type"].ToString() != "")
                    {
                        model.DeviceType = dt.Rows[n]["device_type"].ToString();
                    }

                    if (dt.Rows[n]["device_ip"] != null && dt.Rows[n]["device_ip"].ToString() != "")
                    {
                        model.DeviceIP = dt.Rows[n]["device_ip"].ToString();
                    }

                    if (dt.Rows[n]["device_port"] != null && dt.Rows[n]["device_port"].ToString() != "")
                    {
                        model.DevicePort = dt.Rows[n]["device_port"].ToString();
                    }

                    if (dt.Rows[n]["device_sn"] != null && dt.Rows[n]["device_sn"].ToString() != "")
                    {
                        model.DeviceSN = dt.Rows[n]["device_sn"].ToString();
                    }

                    if (dt.Rows[n]["device_mac"] != null && dt.Rows[n]["device_mac"].ToString() != "")
                    {
                        model.DeviceMAC = dt.Rows[n]["device_mac"].ToString();
                    }

                    if (dt.Rows[n]["entry_type"] != null && dt.Rows[n]["entry_type"].ToString() != "")
                    {
                        model.EntryType = int.Parse(dt.Rows[n]["entry_type"].ToString());
                    }

                    if (dt.Rows[n]["username"] != null && dt.Rows[n]["username"].ToString() != "")
                    {
                        model.Username = dt.Rows[n]["username"].ToString();
                    }

                    if (dt.Rows[n]["password"] != null && dt.Rows[n]["password"].ToString() != "")
                    {
                        model.Password = dt.Rows[n]["password"].ToString();
                    }
                    if (dt.Rows[n]["passageway_name"] != null && dt.Rows[n]["passageway_name"].ToString() != "")
                    {
                        model.PassagewayName = dt.Rows[n]["passageway_name"].ToString();
                    }
                    modelList.Add(model);
                    
                }
            }
            return modelList;
        }
    }
}
