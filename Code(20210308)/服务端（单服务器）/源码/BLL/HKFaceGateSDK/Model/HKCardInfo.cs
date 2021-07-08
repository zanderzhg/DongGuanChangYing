using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FKY_CMP.Code.SDK.Model
{
    public class HKCardInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string CardUserName { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 权限始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 权限终时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 最大可刷卡次数,0为不受限制
        /// </summary>
        public int MaxSwipeTime { get; set; }
        /// <summary>
        /// 卡用户的ID,约定前缀“1”为访客，“2”为职工，长度为9位，中间不足部分用0填充
        /// </summary>
        public int CardUserID { get; set; }
        /// <summary>
        /// 卡密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 将对象转换成V50版本的卡结构体
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static FKY_CMP.Code.SDK.HikvisionSDK.NET_DVR_CARD_CFG_V50 ConvertToNET_DVR_CARD_CFG_V50(HKCardInfo info,bool isDel=false)
        {
            FKY_CMP.Code.SDK.HikvisionSDK.NET_DVR_CARD_CFG_V50 struCardCfg = new HikvisionSDK.NET_DVR_CARD_CFG_V50();
            struCardCfg.Init();
            if (!isDel)
            {
                //约定前缀“1”为访客，“2”为职工，长度为9位，中间不足部分用0填充；
                if (info.CardUserID.ToString().Substring(0, 1).Equals("1"))
                {
                    struCardCfg.byCardType = 7;//4
                }
                else if (info.CardUserID.ToString().Substring(0, 1).Equals("2"))
                {
                    struCardCfg.byCardType = 1;//4
                }

                struCardCfg.struValid = new HikvisionSDK.NET_DVR_VALID_PERIOD_CFG();//2
                struCardCfg.struValid.byEnable = 1;
                struCardCfg.struValid.struBeginTime = HikvisionFaceMachine.ConvertDataTimeToStruTime(info.BeginTime);
                struCardCfg.struValid.struEndTime = HikvisionFaceMachine.ConvertDataTimeToStruTime(info.EndTime);
                struCardCfg.byDoorRight[0] = 1; //8
                struCardCfg.dwMaxSwipeTime = (uint)info.MaxSwipeTime; //20
                struCardCfg.wCardRightPlan[0] = 1;  //100
                struCardCfg.dwCardUserId = (uint)info.CardUserID;
                struCardCfg.dwEmployeeNo = (uint)info.CardUserID;

                byte[] byTemp = Encoding.GetEncoding("GB2312").GetBytes(info.CardNo);
                Array.Copy(byTemp, struCardCfg.byCardNo, byTemp.Length);  

                byTemp = Encoding.GetEncoding("GB2312").GetBytes(info.Password);
                Array.Copy(byTemp, struCardCfg.byCardPassword, byTemp.Length);  //80

                byTemp = Encoding.GetEncoding("GB2312").GetBytes(info.CardUserName);
                Array.Copy(byTemp, struCardCfg.byName, byTemp.Length);  //800
            }
            else
            {
                byte[] byTemp = Encoding.GetEncoding("GB2312").GetBytes(info.CardNo);
                Array.Copy(byTemp, struCardCfg.byCardNo, byTemp.Length);
            }
            return struCardCfg;
        }
    }
}
