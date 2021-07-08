using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ADServer.Model;
using WG3000_COMM.Core;
using System.Windows.Forms;

namespace ADServer.BLL
{
    /// <summary>
    /// M_Card_Info
    /// </summary>
    public partial class B_Card_Info
    {
        private static readonly DAL.D_Card_Info dal = new DAL.D_Card_Info();
        #region 盛炬门禁、梯控

        bool bShowErr = false;//正在显示错误信息

        ADSHalDataStruct.ADS_Comadapter m_comAdatpter = new ADSHalDataStruct.ADS_Comadapter();
        ADSHalDataStruct.ADS_CommunicationParameter m_comm = new ADSHalDataStruct.ADS_CommunicationParameter();
        ADSHalDataStruct.ADS_ControllerInformation[] m_controllers = new ADSHalDataStruct.ADS_ControllerInformation[256];

        List<string> disconnectedConfig = new List<string>();//掉线的控制机器集合

        #endregion
        public B_Card_Info()
        { }

        /// <summary>
        /// 得到所有已关联的IC卡信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet getCardInfo(string strWhere)
        {
            return dal.getCardInfo(strWhere);
        }

        /// <summary>
        /// 根据访客单号，得到IC卡号
        /// </summary>
        /// <param name="visitnonow"></param>
        /// <returns></returns>
        public string GetCardNoByVisitNoNow(string visitnonow)
        {
            return dal.GetCardNoByVisitNoNow(visitnonow);
        }

        /// <summary>
        /// 根据IC卡号，得到访客单号
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public string GetVisitNoNowByCardNo(string cardid)
        {
            return dal.GetVisitNoNowByCardNo(cardid);
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
            return dal.bExistsRelation(visitno);
        }

        /// <summary>
        /// 根据访客姓名与证件号码
        /// 判断该访客是否已关联常访卡或临时卡
        /// </summary>
        /// <param name="name"></param>
        /// <param name="certnumber"></param>
        public bool bExistsRelation(string name, string certnumber)
        {
            return dal.bExistsRelation(name, certnumber);
        }

        /// <summary>
        /// 根据IC卡号，更新常访卡的关联
        /// </summary>
        /// <param name="cardid"></param>
        public void UpdateVisitNoNowByCardId(string cardid)
        {
            dal.UpdateVisitNoNowByCardId(cardid);
        }

        /// <summary>
        /// 根据Ic卡号，判断是否已过有效期
        /// 返回值：true，未过期
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public Boolean bIcCardCanUse(string cardid)
        {
            return dal.bIcCardCanUse(cardid);
        }

        /// <summary>
        /// 根据IC卡号，重置IC卡
        /// </summary>
        /// <param name="cardno"></param>
        public void ResetCardInfo(string cardno)
        {
            dal.ResetCardInfo(cardno);
        }

        /// <summary>
        /// 重置临时卡
        /// </summary>
        /// <param name="visitno"></param>
        public void ResetTempCardInfoByVisitno(string visitno)
        {
            dal.ResetTempCardInfoByVisitno(visitno);
        }

        /// <summary>
        /// 是否存在该卡号的记录
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public Boolean GetExistsCard(string cardid, string cardtype)
        {
            return dal.GetExistsCard(cardid, cardtype);
        }

        public string GetVisitNoByPlate(string carno)
        {
            return dal.GetVisitNoByPlate(carno);
        }


        /// <summary>
        /// 根据卡号得到编号ID
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public int GetID(string cardid)
        {
            return dal.GetID(cardid);
        }

        #region  Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.M_Card_Info model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.M_Card_Info model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            return dal.Delete(id);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteByCardNum(string cardNum)
        {
            return dal.DeleteByCardNum(cardNum);
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            return dal.DeleteList(idlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.M_Card_Info GetModel(int id)
        {
            return dal.GetModel(id);
        }

        /// <summary>
        /// 得到一个对象实体By卡号
        /// </summary>
        public Model.M_Card_Info GetModelByCardId(string cardID)
        {
            return dal.GetModelByCardId(cardID);
        }

        /// <summary>
        /// 得到一个对象List
        /// </summary>
        public List<M_Card_Info> GetListByVisitNo(string visitNo)
        {
            return dal.GetListByVisitNo(visitNo);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }

        /// <summary>
        /// 获取过期的卡
        /// </summary>
        /// <returns></returns>
        public DataSet GetOverdueList()
        {
            return dal.GetOverdueList();
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.M_Card_Info> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.M_Card_Info> DataTableToList(DataTable dt)
        {

            List<Model.M_Card_Info> modelList = new List<Model.M_Card_Info>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Model.M_Card_Info model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Model.M_Card_Info();
                    if (dt.Rows[n]["id"] != null && dt.Rows[n]["id"].ToString() != "")
                    {
                        model.id = int.Parse(dt.Rows[n]["id"].ToString());
                    }
                    if (dt.Rows[n]["CardId"] != null && dt.Rows[n]["CardId"].ToString() != "")
                    {
                        model.CardId = dt.Rows[n]["CardId"].ToString();
                    }
                    if (dt.Rows[n]["UseStatus"] != null && dt.Rows[n]["UseStatus"].ToString() != "")
                    {
                        model.UseStatus = dt.Rows[n]["UseStatus"].ToString();
                    }
                    if (dt.Rows[n]["CardType"] != null && dt.Rows[n]["CardType"].ToString() != "")
                    {
                        model.CardType = dt.Rows[n]["CardType"].ToString();
                    }
                    if (dt.Rows[n]["VisitNoNow"] != null && dt.Rows[n]["VisitNoNow"].ToString() != "")
                    {
                        model.VisitNoNow = dt.Rows[n]["VisitNoNow"].ToString();
                    }
                    if (dt.Rows[n]["StartDate"] != null && dt.Rows[n]["StartDate"].ToString() != "")
                    {
                        model.StartDate = DateTime.Parse(dt.Rows[n]["StartDate"].ToString());
                    }
                    if (dt.Rows[n]["EndDate"] != null && dt.Rows[n]["EndDate"].ToString() != "")
                    {
                        model.EndDate = DateTime.Parse(dt.Rows[n]["EndDate"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        /// <summary>
        /// 删除过期的门禁卡
        /// </summary>
        public void DealWGOverdueCard()
        {
            DataSet ds = GetOverdueList();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string cardId = row["CardId"].ToString();

                    //取消卡的门禁权限
                    List<M_WG_Config> wgConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='WG'");
                    foreach (M_WG_Config cancelConfig in wgConfigList)
                    {
                        wgCancelCard(cardId, int.Parse(cancelConfig.Sn), cancelConfig.IpAddress, int.Parse(cancelConfig.Port));
                    }

                    //取消卡的梯控权限
                    List<M_BuildingPermission> sjEleConfigList = new B_WG_Config().GetBuildingPermissionsFull("");
                    foreach (M_BuildingPermission permission in sjEleConfigList)
                    {
                        int id = GetID(cardId);
                        M_Card_Info card = GetModel(id);
                        string[] pIds = card.GrantElevatorMsg.Split(',');
                        bool findGrantId = false;
                        for (int p = 0; p < pIds.Length; p++)
                        {
                            if (pIds[p] == permission.Id.ToString()) //判断是否授权的权限
                            {
                                findGrantId = true;
                                break;
                            }
                        }

                        if (findGrantId)
                        {
                            M_WG_Config elevatorConfig = new B_WG_Config().GetModelList("sn='" + permission.DeviceId + "'")[0];
                            sjCancelCard(cardId, int.Parse(elevatorConfig.Sn), elevatorConfig.IpAddress);
                        }

                    }

                    ResetCardInfo(cardId);
                }
            }
        }

        private void wgCancelCard(string cardId, int ControllerSN, string IP, int Port)
        {
            string s = cardId;
            UInt32 cardid;
            if (!UInt32.TryParse(s, System.Globalization.NumberStyles.Integer, null, out cardid))
            {
                //MessageBox.Show("failed\r\n");
                return;
            }

            using (wgMjControllerPrivilege pri = new wgMjControllerPrivilege())
            {
                uint registerCardId = uint.Parse(cardId);            //指定注册卡卡号
                if (pri.DelPrivilegeOfOneCardIP(ControllerSN, IP, Port, cardid) >= 0)
                {

                }
                else
                {

                }
            }
        }

        /// <summary>
        /// 删除过期的门禁卡
        /// </summary>
        public void DealSJOverdueCard()
        {
            DataSet ds = GetOverdueList();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string cardId = row["CardId"].ToString();

                    //取消卡的门禁权限
                    List<M_WG_Config> sjConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='SJ'");
                    foreach (M_WG_Config cancelConfig in sjConfigList)
                    {
                        sjCancelCard(cardId, int.Parse(cancelConfig.Sn), cancelConfig.IpAddress);
                    }

                    //取消卡的梯控权限
                    List<M_BuildingPermission> sjEleConfigList = new B_WG_Config().GetBuildingPermissionsFull("");
                    foreach (M_BuildingPermission permission in sjEleConfigList)
                    {
                        int id = GetID(cardId);
                        M_Card_Info card = GetModel(id);
                        string[] pIds = card.GrantElevatorMsg.Split(',');
                        bool findGrantId = false;
                        for (int p = 0; p < pIds.Length; p++)
                        {
                            if (pIds[p] == permission.Id.ToString()) //判断是否授权的权限
                            {
                                findGrantId = true;
                                break;
                            }
                        }

                        if (findGrantId)
                        {
                            M_WG_Config elevatorConfig = new B_WG_Config().GetModelList("sn='" + permission.DeviceId + "'")[0];
                            sjCancelCard(cardId, int.Parse(elevatorConfig.Sn), elevatorConfig.IpAddress);
                        }

                    }

                    ResetCardInfo(cardId);
                }
            }
        }

        private void sjCancelCard(string cardId, int ControllerSN, string IP)
        {
            /// <summary>
            /// 门禁控制器是否连接
            /// </summary>
            bool m_bConnected = false;

            // 连接
            try
            {
                m_comAdatpter.address = 0;
                m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
                m_comAdatpter.port = 0;

                m_comm.deviceAddr = ADSHelp.IP2Int(IP);
                m_comm.password = (ushort)(Convert.ToUInt16("0"));
                //使用UDP通讯
                m_comm.reserve = new byte[3];
                m_comm.reserve[0] = (byte)1;
                m_comm.devicePort = (ushort)65001;

                int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                //ADSHelp.PromptResult(iResult, true);

                if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                {
                    //string cMsg = "\r\n已连接门禁控制器信息：\r\n";
                    m_bConnected = true;
                }
                else
                {
                    m_bConnected = false;

                }
            }
            catch
            {
                m_bConnected = false;
            }

            if (m_bConnected)
            {

                ADSHalDataStruct.ADS_CardHolder card = new ADSHalDataStruct.ADS_CardHolder();
                card.cardNumber.LoNumber = Convert.ToUInt32(cardId);
                card.cardNumber.HiNumber = Convert.ToUInt32("0");

                int iResult = ADSHalAPI.ADS_DeleteCardHolder(ref m_comAdatpter, ref m_comm, ref card);
                //ADSHelp.PromptResult(iResult, true);
            }
        }

        /// <summary>
        /// 删除过期的门禁卡
        /// </summary>
        public void DealTDZOverdueCard()
        {
            DataSet ds = GetOverdueList();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string cardId = row["CardId"].ToString();
                    ResetCardInfo(cardId);
                }
            }
        }

        #endregion  Method

        /// <summary>
        /// 检查是否具备权限
        /// </summary>
        /// <param name="ReaderNo"></param>
        /// <param name="config"></param>
        /// <param name="acMsg"></param>
        /// <returns></returns>
        public static bool CheckAccessMsg(string doorNo, M_WG_Config config, string acMsg)
        {
            if (!string.IsNullOrEmpty(acMsg))
            {
                string[] arrayAcMsg = acMsg.Split('&');
                foreach (var ac in arrayAcMsg)
                {
                    if (ac.Split('_')[0].Equals(config.Id.ToString()))
                    {
                        if (ac.Split(':')[1].Contains(doorNo))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static bool DelectEntryAccess(M_Card_Info cardInfo,M_WG_Config configInfo)
        {
            string oldFullDoorMsg = cardInfo.GrantDoorMsg;
            string[] arrayDoorMsg = oldFullDoorMsg.Split('&');
            foreach (var doorMsg in arrayDoorMsg)
            { 
                if(doorMsg.Split('_')[0].Equals(configInfo.Id.ToString()))
                {
                    string newDoorMsg=configInfo.Id+"_"+configInfo.Sn+":2";
                    cardInfo.GrantDoorMsg = oldFullDoorMsg.Replace(doorMsg, newDoorMsg);
                    break;
                }
            }
            return dal.Update(cardInfo);
        }
    }
}
