using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADServer.Model;
using System.Data;

namespace ADServer.BLL
{
    public class B_WG_Config
    {
        private readonly DAL.D_WG_Config dal = new DAL.D_WG_Config();
        public B_WG_Config()
        { }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.M_WG_Config model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            return dal.Delete(id);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(Model.M_WG_Config model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.M_WG_Config> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<Model.M_WG_Config> GetModelListByACType(AccessType actype)
        {
            List<M_WG_Config> configList = GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='" + actype.ToString() + "'");
            return configList;
        }

        public M_WG_Config GetModelBySn(string sn,string accessType)
        {
            DataSet ds = dal.GetListBySn(sn, accessType);
            List<Model.M_WG_Config> list = DataTableToList(ds.Tables[0]);
            if (list.Count > 0)
                return list[0];
            else
                return null;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.M_WG_Config> DataTableToList(DataTable dt)
        {
            List<Model.M_WG_Config> modelList = new List<Model.M_WG_Config>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                M_WG_Config model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new M_WG_Config();

                    if (dt.Rows[n]["id"] != null && dt.Rows[n]["id"].ToString() != "")
                    {
                        model.Id = int.Parse(dt.Rows[n]["id"].ToString());
                    }
                    if (dt.Rows[n]["machinecode"] != null && dt.Rows[n]["machinecode"].ToString() != "")
                    {
                        model.Machinecode = dt.Rows[n]["machinecode"].ToString();
                    }
                    if (dt.Rows[n]["sn"] != null && dt.Rows[n]["sn"].ToString() != "")
                    {
                        model.Sn = dt.Rows[n]["sn"].ToString();
                    }
                    if (dt.Rows[n]["ipAddress"] != null && dt.Rows[n]["ipAddress"].ToString() != "")
                    {
                        model.IpAddress = dt.Rows[n]["ipAddress"].ToString();
                    }
                    if (dt.Rows[n]["port"] != null && dt.Rows[n]["port"].ToString() != "")
                    {
                        model.Port = dt.Rows[n]["port"].ToString();
                    }
                    if (dt.Rows[n]["wgdoors"] != null && dt.Rows[n]["wgdoors"].ToString() != "")
                    {
                        model.WGDoors = dt.Rows[n]["wgdoors"].ToString();
                    }
                    if (dt.Rows[n]["wgdoornames"] != null && dt.Rows[n]["wgdoornames"].ToString() != "")
                    {
                        model.WGDoorNames = dt.Rows[n]["wgdoornames"].ToString();
                    }
                    if (dt.Rows[n]["wgcheckinout"] != null && dt.Rows[n]["wgcheckinout"].ToString() != "")
                    {
                        model.WGCheckInOut = dt.Rows[n]["wgcheckinout"].ToString();
                    }
                    if (dt.Rows[n]["name"] != null && dt.Rows[n]["name"].ToString() != "")
                    {
                        model.Passageway = dt.Rows[n]["name"].ToString();
                    }

                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public M_WG_Time GetWgTimeBySN(string sn)
        {
            DataTable dt = dal.GetWgTimeBySN(sn).Tables[0];
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                M_WG_Time model = new M_WG_Time();

                if (dt.Rows[0]["id"] != null && dt.Rows[0]["id"].ToString() != "")
                {
                    model.Id = int.Parse(dt.Rows[0]["id"].ToString());
                }
                if (dt.Rows[0]["sn"] != null && dt.Rows[0]["sn"].ToString() != "")
                {
                    model.Sn = dt.Rows[0]["sn"].ToString();
                }
                if (dt.Rows[0]["opendate"] != null && dt.Rows[0]["opendate"].ToString() != "")
                {
                    model.Opendate = dt.Rows[0]["opendate"].ToString();
                }
                if (dt.Rows[0]["timezone1from"] != null && dt.Rows[0]["timezone1from"].ToString() != "")
                {
                    model.TimeZone1From = DateTime.Parse(dt.Rows[0]["timezone1from"].ToString());
                }
                if (dt.Rows[0]["timezone1to"] != null && dt.Rows[0]["timezone1to"].ToString() != "")
                {
                    model.TimeZone1To = DateTime.Parse(dt.Rows[0]["timezone1to"].ToString());
                }
                if (dt.Rows[0]["timezone2from"] != null && dt.Rows[0]["timezone2from"].ToString() != "")
                {
                    model.TimeZone2From = DateTime.Parse(dt.Rows[0]["timezone2from"].ToString());
                }
                if (dt.Rows[0]["timezone2to"] != null && dt.Rows[0]["timezone2to"].ToString() != "")
                {
                    model.TimeZone2To = DateTime.Parse(dt.Rows[0]["timezone2to"].ToString());
                }
                if (dt.Rows[0]["timezone3from"] != null && dt.Rows[0]["timezone3from"].ToString() != "")
                {
                    model.TimeZone3From = DateTime.Parse(dt.Rows[0]["timezone3from"].ToString());
                }
                if (dt.Rows[0]["timezone3to"] != null && dt.Rows[0]["timezone3to"].ToString() != "")
                {
                    model.TimeZone3To = DateTime.Parse(dt.Rows[0]["timezone3to"].ToString());
                }

                return model;
            }
            else
            {
                return null;
            }
        }

        public int Add(Model.M_WG_Time model)
        {
            return dal.Add(model);
        }

        public void Update(Model.M_WG_Time model)
        {
            dal.Update(model);
        }

        public List<M_PassageWay> GetPassagewayList(string strWhere)
        {
            List<M_PassageWay> pList = new List<M_PassageWay>();

            DataTable dt = dal.GetPassagewayList(strWhere).Tables[0];
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                for (int n = 0; n < rowsCount; n++)
                {
                    M_PassageWay pw = new M_PassageWay();

                    if (dt.Rows[n]["id"] != null && dt.Rows[n]["id"].ToString() != "")
                    {
                        pw.Id = int.Parse(dt.Rows[n]["id"].ToString());
                    }

                    if (dt.Rows[n]["name"] != null && dt.Rows[n]["name"].ToString() != "")
                    {
                        pw.Name = dt.Rows[n]["name"].ToString();
                    }

                    pList.Add(pw);
                }
            }

            return pList;
        }

        public int AddPassageway(Model.M_PassageWay model)
        {
            return dal.AddPassageway(model);
        }

        public void UpdatePassageway(Model.M_PassageWay model)
        {
            dal.UpdatePassageway(model);
        }

        public void DeletePassageway(int id)
        {
            dal.DeletePassageway(id);
        }

        public Boolean ExistPassageway(string name, int id)
        {
            return dal.ExistPassageway(name, id);
        }

        /// <summary>
        /// 获取梯控授权权限组的全体信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<M_BuildingPermission> GetBuildingPermissionsFull(string strWhere)
        {
            List<M_BuildingPermission> pList = new List<M_BuildingPermission>();

            DataTable dt = dal.GetBuildingPermissionsFull(strWhere).Tables[0];
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                for (int n = 0; n < rowsCount; n++)
                {
                    M_BuildingPermission bp = new M_BuildingPermission();

                    if (dt.Rows[n]["permissionId"] != null && dt.Rows[n]["permissionId"].ToString() != "")
                    {
                        bp.Id = int.Parse(dt.Rows[n]["permissionId"].ToString());
                    }

                    if (dt.Rows[n]["groupname"] != null && dt.Rows[n]["groupname"].ToString() != "")
                    {
                        bp.GroupName = dt.Rows[n]["groupname"].ToString();
                    }

                    if (dt.Rows[n]["permissionname"] != null && dt.Rows[n]["permissionname"].ToString() != "")
                    {
                        bp.Name = dt.Rows[n]["permissionname"].ToString();
                    }

                    if (dt.Rows[n]["buildingname"] != null && dt.Rows[n]["buildingname"].ToString() != "")
                    {
                        bp.BuildingName = dt.Rows[n]["buildingname"].ToString();
                    }

                    if (dt.Rows[n]["deviceid"] != null && dt.Rows[n]["deviceid"].ToString() != "")
                    {
                        bp.DeviceId = dt.Rows[n]["deviceid"].ToString();
                    }

                    if (dt.Rows[n]["floors"] != null && dt.Rows[n]["floors"].ToString() != "")
                    {
                        bp.Floors = dt.Rows[n]["floors"].ToString();
                    }

                    if (dt.Rows[n]["floorsrange"] != null && dt.Rows[n]["floorsrange"].ToString() != "")
                    {
                        bp.FloorRange = dt.Rows[n]["floorsrange"].ToString();
                    }


                    pList.Add(bp);
                }
            }

            return pList;
        }
    }
}
