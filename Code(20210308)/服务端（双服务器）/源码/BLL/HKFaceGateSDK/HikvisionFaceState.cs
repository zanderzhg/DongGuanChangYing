/*************************************************************************************
    * CLR版 本：    4.0.30319.42000
    * 类 名 称：    HikvisionFaceState
    * 说   明：     FM(FaceMachine)
    * 作    者：    黄辉兴
    * 创建时间：    2019/1/14 17:32:35
    * 修改时间：    N/A
    * 修 改 人：    N/A
    * Copyright (C) 2019 德生科技
    * 德生科技版权所有
   *************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADServer.SDK.Model;

namespace FKY_CMP.Code.SDK
{
    public class HikvisionFaceState
    {
        #region 属性
        /// <summary>
        /// 设备IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 错误信息描述
        /// </summary>
        public string ExceptionMsg { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public uint ErrorCode { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string AdminName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 登录状态
        /// </summary>
        public bool IsLogin { get;  set; }
        /// <summary>
        /// 登录HandleID
        /// </summary>
        public int LoginID { get; set; }
        /// <summary>
        /// 判断是否成功
        /// </summary>
        public bool IsSucceed { get; set; }
        /// <summary>
        /// 下发卡判断是否成功
        /// </summary>
        public bool IsSuccSetCard { get; set; }
        /// <summary>
        /// 下发脸判断是否成功
        /// </summary>
        public bool IsSuccSetFace { get; set; }
        /// <summary>
        /// 人脸上传返回的PID
        /// </summary>
        public string UpLoadReturnID { get; set; }
        /// <summary>
        /// 门参数结构体
        /// </summary>
        public HikvisionSDK.NET_DVR_DOOR_CFG DoorCfg { get; set; }
        /// <summary>
        /// 卡参数结构体
        /// </summary>
        public HikvisionSDK.NET_DVR_CARD_CFG_V50 CardCfg { get; set; }
        /// <summary>
        /// 人脸参数结构体
        /// </summary>
        public HikvisionSDK.NET_DVR_FACE_PARAM_CFG FaceCfg { get; set; }
        /// <summary>
        /// 当前设备实例
        /// </summary>
        public HikvisionFaceMachine HkMachine { get; set; }
        /// <summary>
        /// 警报返回Model
        /// </summary>
        public HKAlarmRecordInfo AlarmRecordInfo { get; set; }
        /// <summary>
        /// 人脸闸机警报返回Model
        /// </summary>
        public HKAlarmRecordInfo FaceAlarmRecordInfo { get; set; }
        /// <summary>
        /// NVR警报返回Model
        /// </summary>
        public HKAlarmRecordInfo NVRAlarmRecordInfo { get; set; }
        #endregion

        public HikvisionFaceState(string ip,int port,string adminName, string password)
        {
            this.IP = ip;
            this.Port = port;
            this.AdminName = adminName;
            this.Password = password;

            this.IsLogin = false;
            this.LoginID = -1;
        }
    }
}
