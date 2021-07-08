/*************************************************************************************
    * CLR版 本：    4.0.30319.42000
    * 类 名 称：    HikvisionFaceMachine
    * 说   明：     N/A
    * 作    者：    黄辉兴
    * 创建时间：    2019/1/14 9:22:17
    * 修改时间：    2019/8/3 11:33:07
    * 修 改 人：    黄辉兴
    * Copyright (C) 2019 德生科技
    * 德生科技版权所有
   *************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Xml;
using System.Windows.Forms;
using System.Threading;
using ADServer.SDK.Model;
using System.Threading.Tasks;
using System.IO;

namespace FKY_CMP.Code.SDK
{
    public class HikvisionFaceMachine 
    {
        #region 字段
        private List<HikvisionFaceState> _states = new List<HikvisionFaceState>();
        private HikvisionSDK.RemoteConfigCallback receiveGetCardCallBack;
        private HikvisionSDK.RemoteConfigCallback receiveSetCardCallBack;
        private HikvisionSDK.RemoteConfigCallback receiveGetFaceCallBack;
        private HikvisionSDK.RemoteConfigCallback receiveSetFaceCallBack;
        private HikvisionSDK.MSGCallBack_V31 alarmMsgCallBack;
        private HikvisionSDK.NET_DVR_CARD_CFG_V50 sendStruCardCfg;
        private HikvisionSDK.NET_DVR_FACE_PARAM_CFG sendStruFace;
        public HikvisionFaceState state;
        private static ManualResetEvent upLoadFaceDone;

        bool isSetCardComplete = false;
        bool isSetFaceComplete = false;
        int getCardCfgHandle = -1;
        int setCardCfgHandle = -1;
        int getFaceCfgHandle = -1;
        int setFaceCfgHandle = -1;
        int upLoadToNVRHandle = -1;
        private bool disposed = false;
        #endregion

        #region 属性
        public bool IsInit { get; private set; }

        public int Count
        {
            get { return _states.Count; }
        }
        #endregion

        #region Enum
        /// <summary>
        /// 修改卡命令
        /// </summary>
        public enum ModifyCommand
        {
            AddCard,
            ModifyCard,
            DeleteCard
        }
        #endregion

        #region 构造器

        public HikvisionFaceMachine()
        {
            IsInit = false;
            Init();
        }
        #endregion

        #region Mothod
        /// <summary>
        /// SDK初始化
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            IsInit = HikvisionSDK.NET_DVR_Init();
            //保存SDK日志 To save the SDK log
            HikvisionSDK.NET_DVR_SetLogToFile(3, Application.StartupPath + "\\Logs\\N1_Logs\\", true);
            return this.IsInit;
        }

        /// <summary>
        /// 释放SDK资源
        /// </summary>
        /// <returns></returns>
        public static bool CleanUp()
        {
            return HikvisionSDK.NET_DVR_Cleanup();
        }

        /// <summary>
        /// 登录设备
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="adminName"></param>
        /// <param name="password"></param>
        /// <param name="exceptionMsg"></param>
        /// <returns></returns>
        public bool Login(string ip, int port, string adminName, string password)
        {
            state = new HikvisionFaceState(ip, port, adminName, password);
            if (!IsInit)
            {
                state.ExceptionMsg = "SDK未初始化";
                return false;
            }
            HikvisionSDK.NET_DVR_SetConnectTime(2500, 1);
            HikvisionSDK.NET_DVR_SetReconnect(5000, 1);
            //注册设备
            HikvisionSDK.NET_DVR_USER_LOGIN_INFO loginInfo = new HikvisionSDK.NET_DVR_USER_LOGIN_INFO();
            loginInfo.bUseAsynLogin = false;
            loginInfo.sDeviceAddress = ip;
            loginInfo.wPort = (short)port; ;
            loginInfo.sUserName = adminName;
            loginInfo.sPassword = password;

            HikvisionSDK.NET_DVR_DEVICEINFO_V40 struDeviceInfoV40 = new HikvisionSDK.NET_DVR_DEVICEINFO_V40();
            state.LoginID = HikvisionSDK.NET_DVR_Login_V40(ref loginInfo, ref struDeviceInfoV40);

            if (state.LoginID < 0)
            {
                uint lastErr = HikvisionSDK.NET_DVR_GetLastError();
                state.ErrorCode = lastErr;
                if (lastErr == 7)
                {
                    state.ExceptionMsg = "登录设备失败，设备不在线或网络原因引起的连接超时;";
                }
                else
                {
                    state.ExceptionMsg = "登录设备失败，错误代码为:" + lastErr.ToString();
                }
                state.IsLogin = false;
                _states.Add(state);
                return false;
            }
            state.HkMachine = this;
            state.ExceptionMsg = null;
            state.IsLogin = true;
            RaiseLogined(state);
            return true;
        }

        /// <summary>
        /// 设备是否在线
        /// </summary>
        /// <returns></returns>
        public bool DeviceIsOnline()
        {
            return HikvisionSDK.NET_DVR_RemoteControl(state.LoginID, HikvisionSDK.NET_DVR_CHECK_USER_STATUS, new IntPtr(), 0);
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="loginID"></param>
        /// <returns></returns>
        public bool LoginOut()
        {
            if (HikvisionSDK.NET_DVR_Logout(state.LoginID))
            {
                //HikvisionSDK.NET_DVR_Cleanup();
                RaiseLogOuted(state);
                return true;
            }
            return false;
        }

        //时间校准
        public bool CalibratTime(DateTime dtParam)
        {
            HikvisionSDK.NET_DVR_TIME srtTimeCfg = new HikvisionSDK.NET_DVR_TIME();
            srtTimeCfg.dwYear = (uint)dtParam.Year;
            srtTimeCfg.dwMonth = (uint)dtParam.Month;
            srtTimeCfg.dwDay = (uint)dtParam.Day;
            srtTimeCfg.dwHour = (uint)dtParam.Hour;
            srtTimeCfg.dwMinute = (uint)dtParam.Minute;
            srtTimeCfg.dwSecond = (uint)dtParam.Second;
            uint dwSize = (uint)Marshal.SizeOf(srtTimeCfg);
            IntPtr ptrTimeCfg = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(srtTimeCfg, ptrTimeCfg, false);
            bool result = HikvisionSDK.NET_DVR_SetDVRConfig(state.LoginID, HikvisionSDK.NET_DVR_SET_TIMECFG, 0, ptrTimeCfg, dwSize);
            Marshal.FreeHGlobal(ptrTimeCfg);
            ptrTimeCfg = IntPtr.Zero;
            if (!result)
            {
                state.ErrorCode = HikvisionSDK.NET_DVR_GetLastError();
                state.ExceptionMsg = "校准时间失败，错误代码为：" + HikvisionSDK.NET_DVR_GetLastError().ToString();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取设备时间
        /// </summary>
        /// <returns></returns>
        public DateTime? GetTime()
        {
            HikvisionSDK.NET_DVR_TIME struGetTimeCfg = new HikvisionSDK.NET_DVR_TIME();
            uint dwReturned = 0;
            uint dwSize = (uint)Marshal.SizeOf(struGetTimeCfg);
            IntPtr ptrTimeCfg = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(struGetTimeCfg, ptrTimeCfg, false);
            bool result = HikvisionSDK.NET_DVR_GetDVRConfig(state.LoginID, HikvisionSDK.NET_DVR_GET_TIMECFG, 0, ptrTimeCfg, dwSize, ref dwReturned);

            if (!result)
            {
                Marshal.FreeHGlobal(ptrTimeCfg);
                ptrTimeCfg = IntPtr.Zero;
                state.ErrorCode = HikvisionSDK.NET_DVR_GetLastError();
                state.ExceptionMsg = "获取时间失败，错误代码为：" + HikvisionSDK.NET_DVR_GetLastError().ToString();
                return null;
            }
            struGetTimeCfg = (HikvisionSDK.NET_DVR_TIME)Marshal.PtrToStructure(ptrTimeCfg, typeof(HikvisionSDK.NET_DVR_TIME));
            Marshal.FreeHGlobal(ptrTimeCfg);
            ptrTimeCfg = IntPtr.Zero;
            DateTime dt = new DateTime((int)struGetTimeCfg.dwYear, (int)struGetTimeCfg.dwMonth, (int)struGetTimeCfg.dwDay, (int)struGetTimeCfg.dwHour, (int)struGetTimeCfg.dwMinute, (int)struGetTimeCfg.dwSecond);
            return dt;
        }

        /// <summary>
        /// 获取门参数
        /// </summary>
        /// <param name="state"></param>
        /// <param name="adminID"></param>
        /// <param name="doorNo"></param>
        /// <returns></returns>
        public bool GetDoorConfig(int adminID, int doorNo = 1)
        {
            HikvisionSDK.NET_DVR_DOOR_CFG struDoorCfg = new HikvisionSDK.NET_DVR_DOOR_CFG();
            uint dwReturned = 0;
            uint dwSize = (uint)Marshal.SizeOf(struDoorCfg); struDoorCfg.dwSize = dwSize;
            IntPtr ptrDoorCfg = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(struDoorCfg, ptrDoorCfg, false);

            if (!HikvisionSDK.NET_DVR_GetDVRConfig(adminID, HikvisionSDK.NET_DVR_GET_DOOR_CFG, doorNo, ptrDoorCfg, dwSize, ref dwReturned))
            {
                state.ErrorCode = HikvisionSDK.NET_DVR_GetLastError();
                state.ExceptionMsg = "获取门参数失败，错误代码为：" + HikvisionSDK.NET_DVR_GetLastError().ToString();
                Marshal.FreeHGlobal(ptrDoorCfg);
                ptrDoorCfg = IntPtr.Zero;
                return false;
            }
            else
            {
                struDoorCfg = (HikvisionSDK.NET_DVR_DOOR_CFG)Marshal.PtrToStructure(ptrDoorCfg, typeof(HikvisionSDK.NET_DVR_DOOR_CFG));
                Marshal.FreeHGlobal(ptrDoorCfg);
                ptrDoorCfg = IntPtr.Zero;
                state.DoorCfg = struDoorCfg;
                RaiseGetDoorConfiged(state);
                return true;
            }
        }

        /// <summary>
        /// 设置门参数
        /// </summary>
        /// <returns></returns>
        public bool SetDoorConfig(HikvisionSDK.NET_DVR_DOOR_CFG doorInfo)
        {
            HikvisionSDK.NET_DVR_DOOR_CFG struDoorCfg = doorInfo;

            uint dwSize = (uint)Marshal.SizeOf(struDoorCfg);
            struDoorCfg.dwSize = dwSize;
            IntPtr ptrDoorCfg = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(struDoorCfg, ptrDoorCfg, false);
            int doorNo = 1;
            if (!HikvisionSDK.NET_DVR_SetDVRConfig(state.LoginID, HikvisionSDK.NET_DVR_SET_DOOR_CFG, doorNo, ptrDoorCfg, dwSize))
            {
                state.ErrorCode = HikvisionSDK.NET_DVR_GetLastError();
                state.ExceptionMsg = "设置门禁参数失败，错误代码为:" + HikvisionSDK.NET_DVR_GetLastError().ToString();
                Marshal.FreeHGlobal(ptrDoorCfg);
                ptrDoorCfg = IntPtr.Zero;
                return false;
            }
            else
            {
                Marshal.FreeHGlobal(ptrDoorCfg);
                ptrDoorCfg = IntPtr.Zero;
                return true;
            }
        }

        #region 获取卡号
        /// <summary>
        /// 获取所有卡号
        /// </summary>
        /// <param name="state"></param>
        /// <param name="userHandle">this.Handle</param>
        /// <returns></returns>
        public bool GetAllCard(IntPtr userHandle)
        {
            HikvisionSDK.NET_DVR_CARD_CFG_COND struCond = new HikvisionSDK.NET_DVR_CARD_CFG_COND();

            struCond.dwSize = (uint)Marshal.SizeOf(struCond);
            struCond.wLocalControllerID = 0;
            struCond.dwCardNum = 0xffffffff;
            struCond.byCheckCardNo = 1;
            int dwSize = Marshal.SizeOf(struCond);
            IntPtr ptrStruCond = Marshal.AllocHGlobal(dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);
            receiveGetCardCallBack = null;
            receiveGetCardCallBack = new HikvisionSDK.RemoteConfigCallback(GetCardCallBack);

            getCardCfgHandle = HikvisionSDK.NET_DVR_StartRemoteConfig(state.LoginID, (uint)HikvisionSDK.NET_DVR_StartRemoteCommands.NET_DVR_GET_CARD_CFG_V50, ptrStruCond, dwSize, receiveGetCardCallBack, userHandle);
            Marshal.FreeHGlobal(ptrStruCond);
            if (getCardCfgHandle == -1)
            {
                state.ErrorCode = HikvisionSDK.NET_DVR_GetLastError();
                state.ExceptionMsg = "请求设备数据失败，错误代码为： " + HikvisionSDK.NET_DVR_GetLastError();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取卡的回调函数
        /// </summary>
        /// <param name="state"></param>
        /// <param name="dwType"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUserData"></param>
        private void GetCardCallBack(uint dwType, IntPtr lpBuffer, uint dwBufLen, IntPtr pUserData)
        {
            if (pUserData == null)
            {
                return;
            }
            HikvisionSDK.NET_DVR_CARD_CFG_V50 struCardCfg;
            if (dwType == (uint)HikvisionSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_DATA)
            {
                struCardCfg = new HikvisionSDK.NET_DVR_CARD_CFG_V50();
                struCardCfg = (HikvisionSDK.NET_DVR_CARD_CFG_V50)Marshal.PtrToStructure(lpBuffer, typeof(HikvisionSDK.NET_DVR_CARD_CFG_V50));
                state.CardCfg = struCardCfg;
                RaiseReceiveCardConfiged(state);
            }
            else if (dwType == (uint)HikvisionSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_STATUS)
            {
                uint dwStatus = (uint)Marshal.ReadInt32(lpBuffer);
                if (dwStatus == (uint)HikvisionSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_SUCCESS)
                {
                    HikvisionSDK.NET_DVR_StopRemoteConfig(getCardCfgHandle);
                }
                else if (dwStatus == (uint)HikvisionSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_FAILED)
                {
                    uint ErrorCode = (uint)Marshal.ReadInt32(lpBuffer + 1);
                    string cardNumber = Marshal.PtrToStringAnsi(lpBuffer + 2);
                    state.ErrorCode = HikvisionSDK.NET_DVR_GetLastError();
                    state.ExceptionMsg = string.Format("建立长连接已成功,但卡号“{0}”下发异常,错误代码为：{1}！", cardNumber, ErrorCode);
                    RaiseReceiveCardConfiged(state);
                }
            }
        }
        #endregion

        #region 增删改卡号
        /// <summary>
        /// 修改卡信息，包括增，删，改功能
        /// </summary>
        /// <param name="state"></param>
        /// <param name="userHandle"></param>
        /// <param name="commandArg"></param>
        /// <param name="cardInfo"></param>
        /// <returns></returns>
        public bool ModifyCardInfo(IntPtr userHandle, ModifyCommand commandArg, HikvisionSDK.NET_DVR_CARD_CFG_V50 cardInfo, int timeout = 5000)
        {
            HikvisionSDK.NET_DVR_CARD_CFG_COND struCond = new HikvisionSDK.NET_DVR_CARD_CFG_COND();
            struCond.dwSize = (uint)Marshal.SizeOf(struCond);
            struCond.dwCardNum = 1;
            struCond.byCheckCardNo = 1;
            struCond.wLocalControllerID = 0;
            int dwSize = Marshal.SizeOf(struCond);
            IntPtr ptrStruCond = Marshal.AllocHGlobal(dwSize);
            Marshal.StructureToPtr(struCond, ptrStruCond, false);

            receiveSetCardCallBack = null;
            receiveSetCardCallBack = new HikvisionSDK.RemoteConfigCallback(SetCardCallBack);
            isSetCardComplete = false;
            state.IsSuccSetCard = false;

            setCardCfgHandle = HikvisionSDK.NET_DVR_StartRemoteConfig(state.LoginID, (uint)HikvisionSDK.NET_DVR_StartRemoteCommands.NET_DVR_SET_CARD_CFG_V50, ptrStruCond, dwSize, receiveSetCardCallBack, userHandle);
            Marshal.FreeHGlobal(ptrStruCond);
            if (setCardCfgHandle == -1)
            {
                state.ExceptionMsg = "建立发送卡号的远程连接失败，错误代码为： " + HikvisionSDK.NET_DVR_GetLastError();
                return false;
            }
            state.CardCfg = cardInfo;
            RaiseStartRemoteConfiged(state);
            HikvisionSDK.NET_DVR_CARD_CFG_V50 struCardCfg = cardInfo;
            struCardCfg.dwSize = (uint)Marshal.SizeOf(struCardCfg);
            int modifyParamType = 0;
            switch (commandArg)
            {
                case ModifyCommand.AddCard:
                case ModifyCommand.ModifyCard:
                    modifyParamType = 1 + 2 + 4 + 8 + 20 + 80 + 100 + 400 + 800 + 1000;
                    struCardCfg.byCardValid = 1;//1
                    break;
                case ModifyCommand.DeleteCard:
                    modifyParamType = 1;
                    struCardCfg.byCardValid = 0;
                    string strCardNum = Encoding.Default.GetString(struCardCfg.byCardNo).Replace("\0", "");
                    //删除卡号先删除人脸
                    if (!DelFace(strCardNum))
                    {
                        return false;
                    }
                    break;
            }
            struCardCfg.dwModifyParamType = (uint)modifyParamType;
            state.CardCfg = struCardCfg;
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            Task<bool>[] tasks = new Task<bool>[2];
            tasks[0] = new Task<bool>(() => { return SendCardData(struCardCfg); }, token);
            tasks[0].Start();
            tasks[1] = new Task<bool>(() =>
            {
                while (!isSetCardComplete) { }
                return true;
            }, token);
            tasks[1].Start();
            Task.WaitAll(tasks, timeout);
            if (tasks[0].IsCompleted && tasks[0].Result && isSetCardComplete && state.IsSuccSetCard)
            {
                return true;
            }
            else
            {
                tokenSource.Cancel();
                return false;
            }
        }

        /// <summary>
        /// 发送卡信息
        /// </summary>
        /// <param name="struCardCfg"></param>
        private bool SendCardData(HikvisionSDK.NET_DVR_CARD_CFG_V50 struCardCfg)
        {
            sendStruCardCfg = struCardCfg;
            uint dwSize = (uint)Marshal.SizeOf(struCardCfg);
            struCardCfg.dwSize = dwSize;
            IntPtr ptrStruCard = Marshal.AllocHGlobal((int)dwSize);
            Marshal.StructureToPtr(struCardCfg, ptrStruCard, false);
            if (!HikvisionSDK.NET_DVR_SendRemoteConfig(setCardCfgHandle, 3, ptrStruCard, dwSize))
            {
                Marshal.FreeHGlobal(ptrStruCard);
                state.ExceptionMsg = "向远程设备发送/设置卡号失败，错误代码为： " + HikvisionSDK.NET_DVR_GetLastError();
                return false;
            }
            Marshal.FreeHGlobal(ptrStruCard);
            return true;
        }

        /// <summary>
        /// 设置卡的回调函数
        /// </summary>
        /// <param name="state"></param>
        /// <param name="dwType"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUserData"></param>
        private void SetCardCallBack(uint dwType, IntPtr lpBuffer, uint dwBufLen, IntPtr pUserData)
        {
            if (pUserData == null)
            {
                return;
            }
            if (dwType != (uint)HikvisionSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_STATUS)
            {
                return;
            }
            uint dwStatus = (uint)Marshal.ReadInt32(lpBuffer);
            if (dwStatus == (uint)HikvisionSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_PROCESSING)
            {
            }
            else if (dwStatus == (uint)HikvisionSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_FAILED)
            {
                HikvisionSDK.NET_DVR_CARD_CFG_V50? struTemp = sendStruCardCfg;
                if (null != struTemp)
                {
                    state.ExceptionMsg = string.Format("卡号 {0} 下发失败，错误代码：{1}", System.Text.Encoding.GetEncoding("GB2312").GetString(sendStruCardCfg.byCardNo).TrimEnd('\0'), HikvisionSDK.NET_DVR_GetLastError());
                    state.IsSuccSetCard = false;
                    RaiseSendCardConfiged(state);
                    return;
                }
            }
            else if (dwStatus == (uint)HikvisionSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_SUCCESS)
            {
                //表示获取和配置成功并且结束；
                state.IsSuccSetCard = true;
                RaiseSendCardConfiged(state);
            }
            else if (dwStatus == (uint)HikvisionSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_EXCEPTION)
            {
                //异常，主要为设备或网络原因导致的意外性质的异常
                state.ExceptionMsg = string.Format("但卡号 {0} 下发异常,错误代码：{1}！", System.Text.Encoding.GetEncoding("GB2312").GetString(sendStruCardCfg.byCardNo).TrimEnd('\0'), HikvisionSDK.NET_DVR_GetLastError());
                state.IsSuccSetCard = false;
                RaiseSendCardConfiged(state);
            }
        }

        public bool StopCardRemote(int waitTime=100)
        {
            bool stopSetCardRemote = false;
            bool stopSetFaceRemote = false;
            if (setCardCfgHandle >= 0)
            {
                IntPtr ptr = IntPtr.Zero;
                bool result=HikvisionSDK.NET_DVR_GetRemoteConfigState(setCardCfgHandle, ref ptr);
                Marshal.FreeHGlobal(ptr);
                if (result)
                {
                    stopSetCardRemote = HikvisionSDK.NET_DVR_StopRemoteConfig(setCardCfgHandle);
                }
            }
            else
            {
                stopSetCardRemote = true;
            }
            if (setFaceCfgHandle >= 0)
            {
                IntPtr ptr = IntPtr.Zero;
                bool result = HikvisionSDK.NET_DVR_GetRemoteConfigState(setFaceCfgHandle, ref ptr);
                Marshal.FreeHGlobal(ptr);
                if (result)
                {
                    stopSetFaceRemote = HikvisionSDK.NET_DVR_StopRemoteConfig(setFaceCfgHandle);
                }
            }
            else
            {
                stopSetFaceRemote = true;
            }
            if (stopSetCardRemote || stopSetFaceRemote)
                Thread.Sleep(waitTime);
            return stopSetCardRemote && stopSetFaceRemote;
        }


        public int GetCardRemoteState()
        {
            IntPtr ptr = IntPtr.Zero;
            if (HikvisionSDK.NET_DVR_GetRemoteConfigState(setCardCfgHandle, ref ptr))
            {
                int i = (int)ptr;
                Marshal.FreeHGlobal(ptr);
                return i;
            }
            Marshal.FreeHGlobal(ptr);
            return 0;
        }

        #endregion


        #region 删除人脸
        /// <summary>
        /// 删除人脸
        /// </summary>
        /// <param name="strCardNo">卡号</param>
        /// <param name="byMode">模式，默认为0，按卡号删除人脸</param>
        /// <returns></returns>
        public bool DelFace(string strCardNo, byte byMode = 0)
        {
            HikvisionSDK.NET_DVR_FACE_PARAM_CTRL struDelFaceParam = new HikvisionSDK.NET_DVR_FACE_PARAM_CTRL();
            struDelFaceParam.Init();
            struDelFaceParam.dwsize = (uint)Marshal.SizeOf(struDelFaceParam);
            struDelFaceParam.byMode = 0;

            if (struDelFaceParam.byMode == 0)
            {
                HikvisionSDK.NET_DVR_FACE_PARAM_BYCARD struDelByCard = new HikvisionSDK.NET_DVR_FACE_PARAM_BYCARD();
                struDelByCard.Init();
                byte[] byCardNo = System.Text.Encoding.Default.GetBytes(strCardNo);
                byCardNo.CopyTo(struDelByCard.byCardNo, 0);
                struDelByCard.byEnableCardReader[0] = 1;
                struDelByCard.byFaceID[0] = 1;

                int dwByCardSize = Marshal.SizeOf(struDelByCard);
                IntPtr ptrFaceByCard = Marshal.AllocHGlobal(dwByCardSize);
                Marshal.StructureToPtr(struDelByCard, ptrFaceByCard, false);

                //结构体数据拷贝到联合体里面
                struDelFaceParam.struProcessMode = (HikvisionSDK.NET_DVR_DEL_FACE_PARAM_MODE)Marshal.PtrToStructure(ptrFaceByCard, typeof(HikvisionSDK.NET_DVR_DEL_FACE_PARAM_MODE));
                Marshal.FreeHGlobal(ptrFaceByCard);
            }
            if (struDelFaceParam.byMode == 1) //按读卡器删除
            {
                HikvisionSDK.NET_DVR_FACE_PARAM_BYREADER struDelByReader = new HikvisionSDK.NET_DVR_FACE_PARAM_BYREADER();
                struDelByReader.dwCardReaderNo = 1; //人脸读卡器编号
                struDelByReader.byClearAllCard = 1; //是否删除所有卡的人脸信息：0- 按卡号删除人脸信息，1- 删除所有卡的人脸信息 

                int dwByReaderSize = Marshal.SizeOf(struDelByReader);
                IntPtr ptrFaceByReader = Marshal.AllocHGlobal(dwByReaderSize);
                Marshal.StructureToPtr(struDelByReader, ptrFaceByReader, false);

                //结构体数据拷贝到联合体里面
                struDelFaceParam.struProcessMode = (HikvisionSDK.NET_DVR_DEL_FACE_PARAM_MODE)Marshal.PtrToStructure(ptrFaceByReader, typeof(HikvisionSDK.NET_DVR_DEL_FACE_PARAM_MODE));
                Marshal.FreeHGlobal(ptrFaceByReader);
            }
            int dwSize = Marshal.SizeOf(struDelFaceParam);
            IntPtr ptrFaceDelCtrl = Marshal.AllocHGlobal(dwSize);
            Marshal.StructureToPtr(struDelFaceParam, ptrFaceDelCtrl, false);

            bool bFaceDel = HikvisionSDK.NET_DVR_RemoteControl(state.LoginID, HikvisionSDK.NET_DVR_DEL_FACE_PARAM_CFG, ptrFaceDelCtrl, (uint)dwSize);

            Marshal.FreeHGlobal(ptrFaceDelCtrl);
            return bFaceDel;
        }
        #endregion

        #region 获取人脸
        /// <summary>
        /// 获取人脸
        /// </summary>
        /// <param name="state"></param>
        /// <param name="userHandle"></param>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        public bool GetFace(IntPtr userHandle, string CardNum)
        {
            HikvisionSDK.NET_DVR_FACE_PARAM_COND struFace = new HikvisionSDK.NET_DVR_FACE_PARAM_COND();
            struFace.Init();
            struFace.dwSize = (uint)Marshal.SizeOf(struFace);
            byte[] byTemp = Encoding.GetEncoding("GB2312").GetBytes(CardNum);
            for (int i = 0; i < byTemp.Length && i < struFace.byCardNo.Length; i++)
            {
                struFace.byCardNo[i] = byTemp[i];
            }
            struFace.byEnableCardReader[0] = 1;
            struFace.dwFaceNum = 0xffffffff;
            struFace.byFaceID = 1;
            struFace.byFaceDataType = 1;
            int dwSize = Marshal.SizeOf(struFace);
            IntPtr ptrStruFace = Marshal.AllocHGlobal(dwSize);
            Marshal.StructureToPtr(struFace, ptrStruFace, false);
            receiveGetFaceCallBack = null;
            receiveGetFaceCallBack = new HikvisionSDK.RemoteConfigCallback(GetFaceCallBack);

            getCardCfgHandle = HikvisionSDK.NET_DVR_StartRemoteConfig(state.LoginID, HikvisionSDK.NET_DVR_GET_FACE_PARAM_CFG, ptrStruFace, dwSize, receiveGetFaceCallBack, userHandle);
            Marshal.FreeHGlobal(ptrStruFace);
            if (getCardCfgHandle == -1)
            {
                state.ErrorCode = HikvisionSDK.NET_DVR_GetLastError();
                state.ExceptionMsg = "建立获取人脸参数的远程连接失败，错误代码为： " + HikvisionSDK.NET_DVR_GetLastError();
                return false;
            }
            RaiseStartRemoteConfiged(state);
            return true;
        }

        /// <summary>
        /// 获取人脸的回调函数
        /// </summary>
        /// <param name="state"></param>
        /// <param name="dwType"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUserData"></param>
        private void GetFaceCallBack(uint dwType, IntPtr lpBuffer, uint dwBufLen, IntPtr pUserData)
        {
            if (pUserData == null)
            {
                return;
            }

            if (dwType == (uint)HikvisionSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_DATA)
            {
                HikvisionSDK.NET_DVR_FACE_PARAM_CFG faceInfo = new HikvisionSDK.NET_DVR_FACE_PARAM_CFG();
                faceInfo = (HikvisionSDK.NET_DVR_FACE_PARAM_CFG)Marshal.PtrToStructure(lpBuffer, typeof(HikvisionSDK.NET_DVR_FACE_PARAM_CFG));
                state.FaceCfg = faceInfo;
                //byte[] bit = new byte[faceInfo.dwFaceLen];
                //Marshal.Copy(faceInfo.pFaceBuffer, bit, 0, (int)faceInfo.dwFaceLen);
            }
            else if (dwType == (uint)HikvisionSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_STATUS)
            {
                uint dwStatus = (uint)Marshal.ReadInt32(lpBuffer);
                if (dwStatus == (uint)HikvisionSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_SUCCESS)
                {
                    state.IsSucceed = true;
                    RaiseReceiveFaceConfiged(state);
                }
                else if (dwStatus == (uint)HikvisionSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_FAILED)
                {
                    uint ErrorCode = (uint)Marshal.ReadInt32(lpBuffer + 1);
                    string cardNumber = Marshal.PtrToStringAnsi(lpBuffer + 2);
                    state.ExceptionMsg = string.Format("卡号“{0}”获取人脸参数失败，错误代码为：{1}", cardNumber, ErrorCode.ToString());
                    state.IsSucceed = false;
                    RaiseReceiveFaceConfiged(state);
                }
            }
        }
        #endregion

        #region 下发人脸
		 
        /// <summary>
        /// 下发人脸
        /// </summary>
        /// <param name="state"></param>
        /// <param name="userHandle"></param>
        /// <param name="CardNum"></param>
        /// <param name="faceArg"></param>
        /// <returns></returns>
        public bool SendFace(IntPtr userHandle, string CardNum, byte[] faceArg,int timeout=5000)
        {
            HikvisionSDK.NET_DVR_FACE_PARAM_COND struFaceCfg = new HikvisionSDK.NET_DVR_FACE_PARAM_COND();
            struFaceCfg.Init();
            struFaceCfg.dwSize = (uint)Marshal.SizeOf(struFaceCfg);
            byte[] byTemp = Encoding.GetEncoding("GB2312").GetBytes(CardNum);
            for (int i = 0; i < byTemp.Length && i < struFaceCfg.byCardNo.Length; i++)
            {
                struFaceCfg.byCardNo[i] = byTemp[i];
            }

            struFaceCfg.byEnableCardReader[0] = 1;
            struFaceCfg.dwFaceNum = 1;
            struFaceCfg.byFaceID = 1;
            struFaceCfg.byFaceDataType = 1;
            int dwSize = Marshal.SizeOf(struFaceCfg);
            IntPtr ptrStruFace = Marshal.AllocHGlobal(dwSize);
            Marshal.StructureToPtr(struFaceCfg, ptrStruFace, false);
            receiveSetFaceCallBack = null;
            receiveSetFaceCallBack = new HikvisionSDK.RemoteConfigCallback(SendFaceCallback);

            setFaceCfgHandle = HikvisionSDK.NET_DVR_StartRemoteConfig(state.LoginID, HikvisionSDK.NET_DVR_SET_FACE_PARAM_CFG, ptrStruFace, dwSize, receiveSetFaceCallBack, userHandle);
            if (setFaceCfgHandle == -1)
            {
                state.ExceptionMsg = "建立下发人脸参数的远程连接失败，错误代码为： " + HikvisionSDK.NET_DVR_GetLastError();
                return false;
            }
            Marshal.FreeHGlobal(ptrStruFace);
            sendStruFace = new HikvisionSDK.NET_DVR_FACE_PARAM_CFG();
            sendStruFace.Init();
            sendStruFace.dwSize = (uint)Marshal.SizeOf(sendStruFace);
            byTemp = Encoding.GetEncoding("GB2312").GetBytes(CardNum);
            for (int i = 0; i < byTemp.Length && i < sendStruFace.byCardNo.Length; i++)
            {
                sendStruFace.byCardNo[i] = byTemp[i];
            }
            uint length;
            sendStruFace.pFaceBuffer = BytesToIntptr(faceArg, out length);
            sendStruFace.byEnableCardReader[0] = 1;
            sendStruFace.dwFaceLen = length;
            sendStruFace.byFaceID = 1;
            sendStruFace.byFaceDataType = 1;
            int dwSize2 = Marshal.SizeOf(sendStruFace);
            IntPtr ptrStruFace2 = Marshal.AllocHGlobal(dwSize2);
            Marshal.StructureToPtr(sendStruFace, ptrStruFace2, false);
            state.IsSucceed = false;
            state.IsSuccSetFace = false;
            isSetFaceComplete = false;
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            Task<bool>[] tasks = new Task<bool>[2];
            tasks[0] = new Task<bool>(() =>
            {
                if (!HikvisionSDK.NET_DVR_SendRemoteConfig(setFaceCfgHandle, 0x9, ptrStruFace2, (uint)dwSize2))
                {
                    Marshal.FreeHGlobal(sendStruFace.pFaceBuffer);
                    Marshal.FreeHGlobal(ptrStruFace2);
                    state.ErrorCode = HikvisionSDK.NET_DVR_GetLastError();
                    state.ExceptionMsg = "下发人脸参数的失败，错误代码为： " + HikvisionSDK.NET_DVR_GetLastError();
                    return false;
                }
                Marshal.FreeHGlobal(sendStruFace.pFaceBuffer);
                Marshal.FreeHGlobal(ptrStruFace2);
                return true;
            }, token);
            tasks[0].Start();
            tasks[1] = new Task<bool>(() =>
            {
                while (!isSetFaceComplete) { }
                return true;
            }, token);
            tasks[1].Start();
            Task.WaitAll(tasks, timeout);
            if (tasks[0].IsCompleted && tasks[0].Result && isSetFaceComplete && state.IsSuccSetFace)
            {
                return true;
            }
            else
            {
                tokenSource.Cancel();
                return false;
            }
        }

        /// <summary>
        /// byte数组转指针
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private IntPtr BytesToIntptr(byte[] bytes, out uint length)
        {
            int size = bytes.Length;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            length = (uint)size;
            Marshal.Copy(bytes, 0, buffer, size);
            return buffer;
        }

        /// <summary>
        /// 下发人脸的回调函数
        /// </summary>
        /// <param name="state"></param>
        /// <param name="dwType"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUserData"></param>
        private void SendFaceCallback(uint dwType, IntPtr lpBuffer, uint dwBufLen, IntPtr pUserData)
        {
            if (pUserData == null)
            {
                return;
            }
            if (dwType == (uint)HikvisionSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_DATA)//信息数据
            {
                HikvisionSDK.NET_DVR_FACE_PARAM_STATUS struCardCfg_status = new HikvisionSDK.NET_DVR_FACE_PARAM_STATUS();
                struCardCfg_status = (HikvisionSDK.NET_DVR_FACE_PARAM_STATUS)Marshal.PtrToStructure(lpBuffer, typeof(HikvisionSDK.NET_DVR_FACE_PARAM_STATUS));
                byte stat = struCardCfg_status.byCardReaderRecvStatus[0];
                if (stat != 1)
                {
                    switch (stat)
                    {
                        //case 2:
                        //    state.ExceptionMsg = string.Format("卡号{0}的人脸下发失败，错误代码：{1},录入的人脸质量差，请重拍", System.Text.Encoding.GetEncoding("GB2312").GetString(sendStruCardCfg.byCardNo).TrimEnd('\0'), stat);
                        //    break;
                        //case 9:
                        //    state.ExceptionMsg = string.Format("卡号{0}的人脸下发失败，错误代码：{1},录入的人眼间距过小，请重拍", System.Text.Encoding.GetEncoding("GB2312").GetString(sendStruCardCfg.byCardNo).TrimEnd('\0'), stat);
                        //    break;
                        //case 16:
                        //    state.ExceptionMsg = string.Format("卡号{0}的人脸下发失败，错误代码：{1}，人脸检测失败，请重拍", System.Text.Encoding.GetEncoding("GB2312").GetString(sendStruCardCfg.byCardNo).TrimEnd('\0'), stat);
                        //    break;
                        //default:
                        //    state.ExceptionMsg = string.Format("卡号{0}的人脸下发失败，错误代码：{1}", System.Text.Encoding.GetEncoding("GB2312").GetString(sendStruCardCfg.byCardNo).TrimEnd('\0'), stat);
                        //    break;

                        case 2:
                            state.ExceptionMsg = string.Format("错误代码{0},录入的人脸质量差，请重拍",  stat);
                            break;
                        case 9:
                            state.ExceptionMsg = string.Format("错误代码{0},录入的人眼间距过小，请重拍",  stat);
                            break;
                        case 16:
                            state.ExceptionMsg = string.Format("错误代码{0}，人脸检测失败，请重拍",  stat);
                            break;
                        default:
                            state.ExceptionMsg = string.Format("错误代码{0}",stat);
                            break;
                    }
                    state.IsSuccSetFace = false;
                    RaiseSendFaceConfiged(state);
                    //IntPtr prt = IntPtr.Zero;
                    //bool ss = HikvisionSDK.NET_DVR_GetRemoteConfigState(state.LoginID, ref prt);
                    //HikvisionSDK.NET_DVR_StopRemoteConfig(setFaceCfgHandle);
                }
                else
                {
                    state.IsSuccSetFace = true;
                    RaiseSendFaceConfiged(state);
                }
            }
        }
        #endregion

        #region XML透传
        public string XMLPassThrough(string strRequestUrl, string strInputParam)
        {
            HikvisionSDK.NET_DVR_XML_CONFIG_INPUT pInputXml = new HikvisionSDK.NET_DVR_XML_CONFIG_INPUT();
            Int32 nInSize = Marshal.SizeOf(pInputXml);
            pInputXml.dwSize = (uint)nInSize;

            uint dwRequestUrlLen = (uint)strRequestUrl.Length;
            pInputXml.lpRequestUrl = Marshal.StringToHGlobalAnsi(strRequestUrl);
            pInputXml.dwRequestUrlLen = dwRequestUrlLen;

            int length = 0;
            pInputXml.lpInBuffer = StringUTF8ToInptr(strInputParam, out length);
            pInputXml.dwInBufferSize = (uint)length;

            HikvisionSDK.NET_DVR_XML_CONFIG_OUTPUT pOutputXml = new HikvisionSDK.NET_DVR_XML_CONFIG_OUTPUT();
            pOutputXml.dwSize = (uint)Marshal.SizeOf(pInputXml);
            pOutputXml.lpOutBuffer = Marshal.AllocHGlobal(3 * 1024 * 1024);
            pOutputXml.dwOutBufferSize = 3 * 1024 * 1024;
            pOutputXml.lpStatusBuffer = Marshal.AllocHGlobal(4096 * 4);
            pOutputXml.dwStatusSize = 4096 * 4;

            if (!HikvisionSDK.NET_DVR_STDXMLConfig(state.LoginID, ref pInputXml, ref pOutputXml))
            {
                Marshal.FreeHGlobal(pInputXml.lpRequestUrl);
                Marshal.FreeHGlobal(pOutputXml.lpOutBuffer);
                Marshal.FreeHGlobal(pOutputXml.lpStatusBuffer);
                uint errorCode = HikvisionSDK.NET_DVR_GetLastError();
                state.ExceptionMsg = "XML透传失败，错误代码为: " + errorCode.ToString();
                return null;
            }

            string strOutXML = IntptrToStringUTF8(pOutputXml.lpOutBuffer);

            //状态值，未用到
            //Marshal.PtrToStringAnsi(pOutputXml.lpStatusBuffer);

            Marshal.FreeHGlobal(pInputXml.lpRequestUrl);
            Marshal.FreeHGlobal(pOutputXml.lpOutBuffer);
            Marshal.FreeHGlobal(pOutputXml.lpStatusBuffer);
            return strOutXML;
        }

        /// <summary>
        /// 是否支持人脸比对功能
        /// </summary>
        /// <returns></returns>
        public bool? IsSupportFaceCompare_NVR()
        {
            string strRequestUrl = "GET /ISAPI/Intelligent/FDLib/capabilities";
            string strOutXML = XMLPassThrough(strRequestUrl, "");
            XmlDocument xmlDoc = new XmlDocument();
            if (strOutXML == null)
            {
                return null;
            }
            xmlDoc.LoadXml(strOutXML);
            bool isSuportAnalysisFace = Convert.ToBoolean(((XmlElement)xmlDoc.GetElementsByTagName("isSuportAnalysisFace")[0]).InnerText);
            bool isSuportFCSearch = Convert.ToBoolean(((XmlElement)xmlDoc.GetElementsByTagName("isSuportFCSearch")[0]).InnerText);
            bool isSupportFDLibEachImport = Convert.ToBoolean(((XmlElement)xmlDoc.GetElementsByTagName("isSupportFDLibEachImport")[0]).InnerText);
            if (isSuportAnalysisFace && isSuportFCSearch && isSupportFDLibEachImport)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 增、删、查、改人脸库
        /// <summary>
        /// 修改人脸库
        /// </summary>
        /// <param name="faceLibId">FDID</param>
        /// <param name="faceLibName">名称</param>
        /// <param name="thresholdValue">阈值</param>
        /// <returns></returns>
        public bool ModifyFaceLibrary(string faceLibName, string FDID, string thresholdValue = "30")
        {
            string strRequestUrl = "GET /ISAPI/Intelligent/FDLib";
            string strOutXML = XMLPassThrough(strRequestUrl, "");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strOutXML);
            XmlNodeList xnList = xmlDoc.GetElementsByTagName("FDID");
            if (xnList.Count > 0)
            {
                for (int i = 0; i < xnList.Count; i++)
                {
                    string tempFDID = ((XmlElement)xnList[i]).InnerText;
                    if (tempFDID != FDID)
                    {
                        continue;
                    }
                    strRequestUrl = "PUT /ISAPI/Intelligent/FDLib";
                    string path = Application.StartupPath + "\\HikvisionConfig\\ModifyFDLibList.xml";
                    XmlDocument modifyXmlDoc = new XmlDocument();
                    modifyXmlDoc.Load(path);

                    ((XmlElement)modifyXmlDoc.GetElementsByTagName("id")[0]).InnerText = i.ToString();
                    ((XmlElement)modifyXmlDoc.GetElementsByTagName("name")[0]).InnerText = faceLibName;
                    ((XmlElement)modifyXmlDoc.GetElementsByTagName("thresholdValue")[0]).InnerText = thresholdValue;
                    ((XmlElement)modifyXmlDoc.GetElementsByTagName("FDID")[0]).InnerText = FDID;
                    string strInputXML = modifyXmlDoc.InnerXml;
                    XMLPassThrough(strRequestUrl, strInputXML);
                }
            }
            return true;
        }

        /// <summary>
        /// 新增人脸库
        /// </summary>
        /// <param name="faceLibId">FDID</param>
        /// <param name="faceLibName">名称</param>
        /// <param name="thresholdValue">阈值，默认为30</param>
        /// <returns></returns>
        public bool AddFaceLibrary(string faceLibName, out string FDID, string thresholdValue = "30")
        {
            string strRequestUrl = "POST /ISAPI/Intelligent/FDLib";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Application.StartupPath + "\\HikvisionConfig\\CreateFDLibList.xml");
            ((XmlElement)xmlDoc.GetElementsByTagName("id")[0]).InnerText = "1";
            ((XmlElement)xmlDoc.GetElementsByTagName("name")[0]).InnerText = faceLibName;
            ((XmlElement)xmlDoc.GetElementsByTagName("thresholdValue")[0]).InnerText = thresholdValue;
            string strInputXML = xmlDoc.InnerXml;
            string strOutXML = XMLPassThrough(strRequestUrl, strInputXML);
            FDID = null;
            if (strOutXML == null)
            {
                return false;
            }
            XmlDocument outXmlDoc = new XmlDocument();
            outXmlDoc.LoadXml(strOutXML);
            XmlNodeList xnList = outXmlDoc.GetElementsByTagName("FDID");
            if (xnList.Count > 0)
            {
                FDID = ((XmlElement)xnList[0]).InnerText;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除人脸库
        /// </summary>
        /// <returns></returns>
        public void DelFaceLibrary(string FDID)
        {
            string strRequestUrl = string.Format("DELETE /ISAPI/Intelligent/FDLib/{0}", FDID);
            XMLPassThrough(strRequestUrl, "");
        }
        #endregion

        #region NVR人脸库图片操作

        /// <summary>
        /// 上传一张人脸图片到NVR
        /// </summary>
        /// <param name="byImage"></param>
        /// <param name="humanType">人员类型：visitor；emp</param>
        /// <param name="strImageName"></param>
        /// <param name="strTel"></param>
        /// <param name="FDID"></param>
        /// <returns></returns>
        public bool UpLoadFaceNVR(byte[] byImage, string humanType, string strImageName, string strTel, string FDID)
        {
            HikvisionSDK.NET_DVR_FACELIB_COND struFaceConditin = new HikvisionSDK.NET_DVR_FACELIB_COND();
            struFaceConditin.dwSize = (uint)Marshal.SizeOf(struFaceConditin);
            struFaceConditin.szFDID = FDID;
            struFaceConditin.byConcurrent = 0;
            struFaceConditin.byCover = 0;
            struFaceConditin.byIdentityKey = "FKY";
            IntPtr ptrStruFace = Marshal.AllocHGlobal((Int32)struFaceConditin.dwSize);
            Marshal.StructureToPtr(struFaceConditin, ptrStruFace, false);
            IntPtr outIntPtr = new IntPtr();

            upLoadToNVRHandle = HikvisionSDK.NET_DVR_UploadFile_V40(state.LoginID, HikvisionSDK.IMPORT_DATA_TO_FACELIB, ptrStruFace, struFaceConditin.dwSize, null, ref outIntPtr, 0);
            Marshal.FreeHGlobal(ptrStruFace);
            if (upLoadToNVRHandle < 0)
            {
                state.IsSucceed = false;
                state.ExceptionMsg = "建立上传NVR人脸库的连接失败，错误代码：" + HikvisionSDK.NET_DVR_GetLastError();
                HikvisionSDK.NET_DVR_UploadClose(state.LoginID);
                return false;
            }
            HikvisionSDK.NET_DVR_SEND_PARAM_IN struFaceParam = new HikvisionSDK.NET_DVR_SEND_PARAM_IN();
            struFaceParam.Init();
            uint length;
            struFaceParam.pSendData = BytesToIntptr(byImage, out length);
            struFaceParam.dwSendDataLen = length;
            struFaceParam.struTime = ConvertDataTimeToStruTime_V30(DateTime.Now);
            struFaceParam.byPicType = 1;
            struFaceParam.dwPicMangeNo = 0;
            byte[] temp = Encoding.ASCII.GetBytes(strImageName);
            for (int i = 0; i < temp.Length; i++)
            {
                struFaceParam.sPicName[i] = temp[i];
            }
            struFaceParam.dwPicDisplayTime = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Application.StartupPath + "\\HikvisionConfig\\FaceAppendData.xml");
            if (!strImageName.Trim().Equals(""))
            {
                XmlElement xe = (XmlElement)xmlDoc.GetElementsByTagName("name")[0];
                xe.InnerText = strImageName;
            }
            if (strTel != null&&!strTel.Trim().Equals(""))
            {
                XmlElement xe = (XmlElement)xmlDoc.GetElementsByTagName("phoneNumber")[0];
                xe.InnerText = strTel;
            }
            XmlElement xe2 = (XmlElement)xmlDoc.GetElementsByTagName("value")[0];
            xe2.InnerText = humanType;
            string strXML = xmlDoc.InnerXml;
            int nameByLength = 0;
            struFaceParam.pSendAppendData = StringUTF8ToInptr(strXML, out nameByLength);
            struFaceParam.dwSendAppendDataLen = (uint)nameByLength;

            IntPtr nullPtr = new IntPtr();
            int sendDataHandle = HikvisionSDK.NET_DVR_UploadSend(upLoadToNVRHandle, ref struFaceParam, ref nullPtr);
            if (sendDataHandle < 0)
            {
                state.IsSucceed = false;
                state.ExceptionMsg = "上传人脸图片到NVR人脸库失败，错误代码：" + HikvisionSDK.NET_DVR_GetLastError();
                RaiseFaceUpLoaded(state);
                HikvisionSDK.NET_DVR_UploadClose(upLoadToNVRHandle);
                return false;
            }
            Thread thr = new Thread(GetUploadStateNVR);
            thr.IsBackground = true;
            thr.Start();
            upLoadFaceDone = new ManualResetEvent(false);
            upLoadFaceDone.WaitOne();
            if (this.state.IsSucceed)
            {
                HikvisionSDK.NET_DVR_UPLOAD_FILE_RET struUploadResult = new HikvisionSDK.NET_DVR_UPLOAD_FILE_RET();
                struUploadResult.Init();
                int size = Marshal.SizeOf(struUploadResult);
                IntPtr lpOutBuffer = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(struUploadResult, lpOutBuffer, false);
                if (!HikvisionSDK.NET_DVR_GetUploadResult(upLoadToNVRHandle, lpOutBuffer, (uint)size))
                {
                    state.IsSucceed = false;
                    state.ExceptionMsg = "获取返回ID失败,请重新上传";
                }
                else
                {
                    struUploadResult = (HikvisionSDK.NET_DVR_UPLOAD_FILE_RET)Marshal.PtrToStructure(lpOutBuffer, typeof(HikvisionSDK.NET_DVR_UPLOAD_FILE_RET));
                    string pid = Encoding.UTF8.GetString(struUploadResult.sUrl);
                    state.UpLoadReturnID = pid.Replace("\0", "");
                }
                Marshal.FreeHGlobal(lpOutBuffer);
            }
            RaiseFaceUpLoaded(state);
            HikvisionSDK.NET_DVR_UploadClose(upLoadToNVRHandle);
            return this.state.IsSucceed;
        }

        /// <summary>
        /// 获取NVR上传人脸状态
        /// </summary>
        private void GetUploadStateNVR()
        {
            int returnValue = -1;
            int process = 0;
            for (int i = 15; i >= 0; i--)
            {
                returnValue = HikvisionSDK.NET_DVR_GetUploadState(upLoadToNVRHandle, out process);
                Thread.Sleep(100);
                switch (returnValue)
                {
                    case 1:
                        i = 0;

                        state.IsSucceed = true;
                        break;
                    case 2:
                        continue;
                    case 3:
                        state.IsSucceed = false;
                        state.ExceptionMsg = "上传失败";
                        i = 0;
                        break;
                    case 4:
                        state.IsSucceed = false;
                        state.ExceptionMsg = "网络断开，状态未知";
                        i = 0;
                        break;
                    default:
                        state.IsSucceed = false;
                        state.ExceptionMsg = "上传失败，返回状态代码为:“" + returnValue + "”";
                        continue;
                }
            }
            upLoadFaceDone.Set();
        }

        /// <summary>
        /// 删除NVR人脸库图片
        /// </summary>
        /// <param name="FDID"></param>
        /// <param name="PID"></param>
        public void DelFaceNVR(string FDID, string PID)
        {
            string strRequestUrl = string.Format("DELETE /ISAPI/Intelligent/FDLib/{0}/picture/{1}", FDID, PID);
            XMLPassThrough(strRequestUrl, "");
        }

        /// <summary>
        /// 修改NVR人脸图片
        /// </summary>
        /// <param name="byImage"></param>
        /// <param name="humanType"></param>
        /// <param name="strImageName"></param>
        /// <param name="strTel"></param>
        /// <param name="FDID"></param>
        /// <param name="PID"></param>
        /// <returns></returns>
        public bool ModifyFaceNVR(byte[] byImage, string humanType, string strImageName, string strTel, string FDID, string PID)
        {
            DelFaceNVR(FDID, PID);
            bool result = UpLoadFaceNVR(byImage, humanType, strImageName, strTel, FDID);
            return result;
        }

        #region 布防、撤防
        /// <summary>
        /// 布防
        /// </summary>
        /// <returns></returns>
        public bool SetAlarm()
        {
            HikvisionSDK.NET_DVR_SETUPALARM_PARAM struAlarmParam = new HikvisionSDK.NET_DVR_SETUPALARM_PARAM();
            struAlarmParam.dwSize = (uint)Marshal.SizeOf(struAlarmParam);
            struAlarmParam.byLevel = 1; //0- 一级布防,1- 二级布防
            struAlarmParam.byAlarmInfoType = 1;//智能交通设备有效，新报警信息类型
            struAlarmParam.byFaceAlarmDetection = 1;//1-人脸侦测

            alarmMsgCallBack = new HikvisionSDK.MSGCallBack_V31(MsgCallback_V31);
            if (!HikvisionSDK.NET_DVR_SetDVRMessageCallBack_V31(alarmMsgCallBack, IntPtr.Zero))
            {
                state.ExceptionMsg = "布防失败，错误号：" + HikvisionSDK.NET_DVR_GetLastError();
                return false;
            }
            int alarmHandle = HikvisionSDK.NET_DVR_SetupAlarmChan_V41(state.LoginID, ref struAlarmParam);
            if (alarmHandle < 0)
            {
                state.ExceptionMsg = "布防失败，错误号：" + HikvisionSDK.NET_DVR_GetLastError();
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 撤防
        /// </summary>
        /// <returns></returns>
        public bool CloseAlarm()
        {
            return HikvisionSDK.NET_DVR_CloseAlarmChan_V30(state.LoginID);
        }

        public bool MsgCallback_V31(int lCommand, ref HikvisionSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            //通过lCommand来判断接收到的报警信息类型，不同的lCommand对应不同的pAlarmInfo内容
            switch (lCommand)
            {
                case HikvisionSDK.COMM_ALARM: //(DS-8000老设备)移动侦测、视频丢失、遮挡、IO信号量等报警信息
                    break;
                case HikvisionSDK.COMM_ALARM_V30://移动侦测、视频丢失、遮挡、IO信号量等报警信息
                    break;
                case HikvisionSDK.COMM_ALARM_RULE://进出区域、入侵、徘徊、人员聚集等行为分析报警信息
                    break;
                case HikvisionSDK.COMM_UPLOAD_PLATE_RESULT://交通抓拍结果上传(老报警信息类型)
                    break;
                case HikvisionSDK.COMM_ITS_PLATE_RESULT://交通抓拍结果上传(新报警信息类型)
                    break;
                case HikvisionSDK.COMM_ALARM_PDC://客流量统计报警信息
                    break;
                case HikvisionSDK.COMM_ITS_PARK_VEHICLE://客流量统计报警信息
                    break;
                case HikvisionSDK.COMM_DIAGNOSIS_UPLOAD://VQD报警信息
                    break;
                case HikvisionSDK.COMM_UPLOAD_FACESNAP_RESULT://人脸抓拍结果信息
                    break;
                case HikvisionSDK.COMM_SNAP_MATCH_ALARM://人脸比对结果信息
                    string ip = state.IP;
                    ProcessCommAlarm_FaceMatch(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case HikvisionSDK.COMM_ALARM_FACE_DETECTION://人脸侦测报警信息
                    break;
                case HikvisionSDK.COMM_ALARMHOST_CID_ALARM://报警主机CID报警上传
                    break;
                case HikvisionSDK.COMM_ALARM_ACS://门禁主机报警上传
                    ProcessCommAlarm_AcsAlarm(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case HikvisionSDK.COMM_ID_INFO_ALARM://身份证刷卡信息上传
                    break;
                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// 人脸比对结果信息
        /// </summary>
        /// <param name="pAlarmer"></param>
        /// <param name="pAlarmInfo"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUser"></param>
        private void ProcessCommAlarm_FaceMatch(ref HikvisionSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            HikvisionSDK.NET_VCA_FACESNAP_MATCH_ALARM struFaceMatchAlarm = new HikvisionSDK.NET_VCA_FACESNAP_MATCH_ALARM();
            uint dwSize = (uint)Marshal.SizeOf(struFaceMatchAlarm);
            struFaceMatchAlarm = (HikvisionSDK.NET_VCA_FACESNAP_MATCH_ALARM)Marshal.PtrToStructure(pAlarmInfo, typeof(HikvisionSDK.NET_VCA_FACESNAP_MATCH_ALARM));

            if (struFaceMatchAlarm.fSimilarity > 0.3)
            {
                //报警设备IP地址
                string strIP = System.Text.Encoding.UTF8.GetString(pAlarmer.sDeviceIP).TrimEnd('\0');
                //前端设备IP地址
                string strSourceIP = System.Text.Encoding.ASCII.GetString(struFaceMatchAlarm.struSnapInfo.struDevInfo.struDevIP.sIpV4).TrimEnd('\0');
                //设备名称
                string machineName = pAlarmer.sDeviceName;

                //保存抓拍人脸子图图片数据
                //if ((struFaceMatchAlarm.struSnapInfo.dwSnapFacePicLen != 0) && (struFaceMatchAlarm.struSnapInfo.pBuffer1 != IntPtr.Zero))
                //{
                //    string str = ".\\picture\\FaceMatch_FacePic_[" + strIP + "]_lUerID_[" + pAlarmer.lUserID + "]_" + iPicNumber + ".jpg";
                //    FileStream fs = new FileStream(str, FileMode.Create);
                //    int iLen = (int)struFaceMatchAlarm.struSnapInfo.dwSnapFacePicLen;
                //    byte[] by = new byte[iLen];
                //    Marshal.Copy(struFaceMatchAlarm.struSnapInfo.pBuffer1, by, 0, iLen);
                //    fs.Write(by, 0, iLen);
                //    fs.Close();
                //}

                //保存比对结果人脸库人脸图片数据
                //if ((struFaceMatchAlarm.struBlackListInfo.dwBlackListPicLen != 0) && (struFaceMatchAlarm.struBlackListInfo.pBuffer1 != IntPtr.Zero))
                //{
                //    string str = ".\\picture\\FaceMatch_BlackListPic_[" + strIP + "]_lUerID_[" + pAlarmer.lUserID + "]" +
                //        "_fSimilarity[" + struFaceMatchAlarm.fSimilarity + "]_" + iPicNumber + ".jpg";
                //    FileStream fs = new FileStream(str, FileMode.Create);
                //    int iLen = (int)struFaceMatchAlarm.struBlackListInfo.dwBlackListPicLen;
                //    byte[] by = new byte[iLen];
                //    Marshal.Copy(struFaceMatchAlarm.struBlackListInfo.pBuffer1, by, 0, iLen);
                //    fs.Write(by, 0, iLen);
                //    fs.Close();
                //}

                //抓拍时间：年月日时分秒
                string strTimeYear = ((struFaceMatchAlarm.struSnapInfo.dwAbsTime >> 26) + 2000).ToString();
                string strTimeMonth = ((struFaceMatchAlarm.struSnapInfo.dwAbsTime >> 22) & 15).ToString("d2");
                string strTimeDay = ((struFaceMatchAlarm.struSnapInfo.dwAbsTime >> 17) & 31).ToString("d2");
                string strTimeHour = ((struFaceMatchAlarm.struSnapInfo.dwAbsTime >> 12) & 31).ToString("d2");
                string strTimeMinute = ((struFaceMatchAlarm.struSnapInfo.dwAbsTime >> 6) & 63).ToString("d2");
                string strTimeSecond = ((struFaceMatchAlarm.struSnapInfo.dwAbsTime >> 0) & 63).ToString("d2");
                string strTime = strTimeYear + "-" + strTimeMonth + "-" + strTimeDay + " " + strTimeHour + ":" + strTimeMinute + ":" + strTimeSecond;

                HKAlarmRecordInfo hkAlarmRecord = new HKAlarmRecordInfo();
                hkAlarmRecord.CardNum = "";
                hkAlarmRecord.AlarmTime = Convert.ToDateTime(strTime);
                hkAlarmRecord.IP = strSourceIP;
                hkAlarmRecord.NVRIP = strIP;
                //hkAlarmRecord.DeviceName = GetPicCfg(struFaceMatchAlarm.struSnapInfo.struDevInfo);
                //if (hkAlarmRecord.DeviceName == null)
                //{
                hkAlarmRecord.DeviceName = strSourceIP;
                //}
                hkAlarmRecord.AlarmType = HKAlarmRecordInfo.AlarmCommand.FaceMatch;
                hkAlarmRecord.PID = IntptrToStringUTF8(struFaceMatchAlarm.struBlackListInfo.pPID, (int)struFaceMatchAlarm.struBlackListInfo.dwPIDLen);
                hkAlarmRecord.Similarity = struFaceMatchAlarm.fSimilarity;

                IntPtr extendInfoPtr = struFaceMatchAlarm.struBlackListInfo.struBlackListInfo.struAttribute.pPersonInfoExtend;
                int extendInfoPtrLen = (int)struFaceMatchAlarm.struBlackListInfo.struBlackListInfo.struAttribute.dwPersonInfoExtendLen;
                string strXML = IntptrToStringUTF8(extendInfoPtr, extendInfoPtrLen);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strXML);
                XmlElement xe = (XmlElement)xmlDoc.GetElementsByTagName("value")[0];
                if (xe == null) { return; }
                hkAlarmRecord.PersonnelType = xe.InnerText;

                state.NVRAlarmRecordInfo = hkAlarmRecord;
                RaiseNVRFaceMatched(state);
            }
        }

        /// <summary>
        /// 门禁主机报警上传
        /// </summary>
        /// <param name="pAlarmer"></param>
        /// <param name="pAlarmInfo"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUser"></param>
        private void ProcessCommAlarm_AcsAlarm(ref HikvisionSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            HikvisionSDK.NET_DVR_ACS_ALARM_INFO struAcsAlarm = new HikvisionSDK.NET_DVR_ACS_ALARM_INFO();
            uint dwSize = (uint)Marshal.SizeOf(struAcsAlarm);
            struAcsAlarm = (HikvisionSDK.NET_DVR_ACS_ALARM_INFO)Marshal.PtrToStructure(pAlarmInfo, typeof(HikvisionSDK.NET_DVR_ACS_ALARM_INFO));
            //人脸认证通过,触发事件
            if (struAcsAlarm.dwMajor == HikvisionSDK.MAJOR_EVENT && (struAcsAlarm.dwMinor == HikvisionSDK.MINOR_PEOPLE_AND_ID_CARD_COMPARE_PASS || struAcsAlarm.dwMinor == 0x08 || struAcsAlarm.dwMinor == 0x4b))
            {
                //报警设备IP地址
                string strIP = System.Text.Encoding.UTF8.GetString(pAlarmer.sDeviceIP).TrimEnd('\0');
                //设备名称
                string strDeviceName = pAlarmer.sDeviceName;

                byte[] picBytes = new byte[1];
                //保存抓拍图片
                if ((struAcsAlarm.dwPicDataLen != 0) && (struAcsAlarm.pPicData != IntPtr.Zero))
                {
                    //string path = ".\\picture\\Device_Acs_CapturePic_[" + strIP + "]_lUerID_[" + pAlarmer.lUserID + "]" + ".jpg";
                    //FileStream fs = new FileStream(path, FileMode.Create);
                    int iLen = (int)struAcsAlarm.dwPicDataLen;
                    picBytes = new byte[iLen];
                    Marshal.Copy(struAcsAlarm.pPicData, picBytes, 0, iLen);
                    //fs.Write(picBytes, 0, iLen);
                    //fs.Close();
                }

                //报警时间：年月日时分秒
                string strTimeYear = (struAcsAlarm.struTime.dwYear).ToString();
                string strTimeMonth = (struAcsAlarm.struTime.dwMonth).ToString("d2");
                string strTimeDay = (struAcsAlarm.struTime.dwDay).ToString("d2");
                string strTimeHour = (struAcsAlarm.struTime.dwHour).ToString("d2");
                string strTimeMinute = (struAcsAlarm.struTime.dwMinute).ToString("d2");
                string strTimeSecond = (struAcsAlarm.struTime.dwSecond).ToString("d2");
                string strTime = strTimeYear + "-" + strTimeMonth + "-" + strTimeDay + " " + strTimeHour + ":" + strTimeMinute + ":" + strTimeSecond;

                HKAlarmRecordInfo hkAlarmRecord = new HKAlarmRecordInfo();
                hkAlarmRecord.CardNum = System.Text.Encoding.UTF8.GetString(struAcsAlarm.struAcsEventInfo.byCardNo).TrimEnd('\0');
                hkAlarmRecord.CardType = struAcsAlarm.struAcsEventInfo.byCardType;
                hkAlarmRecord.AlarmTime = Convert.ToDateTime(strTime);
                hkAlarmRecord.IP = strIP;
                hkAlarmRecord.DeviceName = strDeviceName;
                hkAlarmRecord.CapturePicBytes = picBytes;

                if (struAcsAlarm.dwMinor == HikvisionSDK.MINOR_PEOPLE_AND_ID_CARD_COMPARE_PASS)
                {
                    hkAlarmRecord.AlarmType = HKAlarmRecordInfo.AlarmCommand.AcsAlarmSucc;
                }
                else if (struAcsAlarm.dwMinor == 0x4b)
                {
                    hkAlarmRecord.AlarmType = HKAlarmRecordInfo.AlarmCommand.AcsAlarmSucc;
                }
                else if (struAcsAlarm.dwMinor == 0x08)
                {
                    hkAlarmRecord.AlarmType = HKAlarmRecordInfo.AlarmCommand.AcsAlarmOverTime;
                }

                state.FaceAlarmRecordInfo = hkAlarmRecord;
                RaiseAcsAlarmed(state);
            }
        }

        /// <summary>
        /// 获取摄像头名称(通道名称)
        /// </summary>
        /// <param name="channelNum"></param>
        private string GetPicCfg(HikvisionSDK.NET_VCA_DEV_INFO struDevInfo)
        {

            byte channelNum = struDevInfo.byIvmsChannel;
            HikvisionSDK.NET_DVR_PICCFG_V40 struPicCfg = new HikvisionSDK.NET_DVR_PICCFG_V40();
            int dwSize = Marshal.SizeOf(struPicCfg);
            IntPtr ptrPicCfg = Marshal.AllocHGlobal(dwSize);
            Marshal.StructureToPtr(struPicCfg, ptrPicCfg, false);
            uint dwReturned = 0;
            if (!HikvisionSDK.NET_DVR_GetDVRConfig(state.LoginID, HikvisionSDK.NET_DVR_GET_PICCFG_V40, (int)channelNum, ptrPicCfg, (uint)dwSize, ref dwReturned))
            {
                state.ExceptionMsg = "获取摄像头信息失败! 错误代码：" + HikvisionSDK.NET_DVR_GetLastError();
                Marshal.FreeHGlobal(ptrPicCfg);
                if (!Directory.Exists(Application.StartupPath + "\\HKLogs"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\HKLogs");
                }
                string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string file = Application.StartupPath + "\\HKLogs\\" + nowTime + ".txt";
                if (!File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write("摄像头异常");
                    sw.Write(" 异常描述 : " + state.ExceptionMsg);
                    sw.Close();
                    fs.Close();
                }
                return null;
            }
            struPicCfg = (HikvisionSDK.NET_DVR_PICCFG_V40)Marshal.PtrToStructure(ptrPicCfg, typeof(HikvisionSDK.NET_DVR_PICCFG_V40));
            Marshal.FreeHGlobal(ptrPicCfg);
            string deviceName = Encoding.GetEncoding("GBK").GetString(struPicCfg.sChanName).TrimEnd('\0');
            return deviceName;
        }

        #endregion

        #endregion

        #region 时间类型转换
        /// <summary>
        /// Convert SDK Struct Time Struct To DataTime
        /// </summary>
        /// <param name="timeArg"></param>
        /// <returns>DataTime</returns>
        public static DateTime ConvertStruTimeToDataTime(HikvisionSDK.NET_DVR_TIME_EX timeArg)
        {
            DateTime resultDate = Convert.ToDateTime(timeArg.wYear.ToString() + "-" + timeArg.byMonth.ToString() + "-" + timeArg.byDay.ToString() +
                " " + timeArg.byHour.ToString() + ":" + timeArg.byMinute.ToString() + ":" + timeArg.bySecond.ToString());
            return resultDate;
        }

        /// <summary>
        /// Convert DataTime To SDK Struct Time
        /// </summary>
        /// <param name="timeArgs"></param>
        /// <returns></returns>
        public static HikvisionSDK.NET_DVR_TIME_EX ConvertDataTimeToStruTime(DateTime? timeArgs)
        {
            HikvisionSDK.NET_DVR_TIME_EX struTime = new HikvisionSDK.NET_DVR_TIME_EX();
            struTime.wYear = (ushort)timeArgs.Value.Year;
            struTime.byMonth = (byte)timeArgs.Value.Month;
            struTime.byDay = (byte)timeArgs.Value.Day;
            struTime.byHour = (byte)timeArgs.Value.Hour;
            struTime.byMinute = (byte)timeArgs.Value.Minute;
            struTime.bySecond = (byte)timeArgs.Value.Second;
            return struTime;
        }

        public HikvisionSDK.NET_DVR_TIME_V30 ConvertDataTimeToStruTime_V30(DateTime? timeArgs)
        {
            HikvisionSDK.NET_DVR_TIME_V30 struTime = new HikvisionSDK.NET_DVR_TIME_V30();
            struTime.wYear = (ushort)timeArgs.Value.Year;
            struTime.byMonth = (byte)timeArgs.Value.Month;
            struTime.byDay = (byte)timeArgs.Value.Day;
            struTime.byHour = (byte)timeArgs.Value.Hour;
            struTime.byMinute = (byte)timeArgs.Value.Minute;
            struTime.bySecond = (byte)timeArgs.Value.Second;
            struTime.wMilliSec = 0;
            return struTime;
        }
        #endregion

        #region 封送指针相关
        /// <summary>
        /// byte数组转指针
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public IntPtr StringUTF8ToInptr(string str, out int length)
        {
            byte[] utf8bytes = Encoding.UTF8.GetBytes(str);
            length = utf8bytes.Length;
            IntPtr ptr = Marshal.AllocHGlobal(utf8bytes.Length);
            Marshal.Copy(utf8bytes, 0, ptr, utf8bytes.Length);
            Marshal.WriteByte(ptr, utf8bytes.Length, 0);
            return ptr;
        }

        /// <summary>
        /// 指针转byte数组
        /// </summary>
        /// <param name="pNativeData"></param>
        /// <returns></returns>
        public string IntptrToStringUTF8(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) return null;
            List<byte> bytes = new List<byte>();
            for (int offset = 0; ; offset++)
            {
                byte b = Marshal.ReadByte(ptr, offset);
                if (b == 0) break;
                else bytes.Add(b);
            }
            return Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Count);
        }

        /// <summary>
        /// 指针转byte数组
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string IntptrToStringUTF8(IntPtr ptr, int length)
        {
            if (ptr == IntPtr.Zero) return null;
            List<byte> bytes = new List<byte>();
            for (int offset = 0; offset < length; offset++)
            {
                byte b = Marshal.ReadByte(ptr, offset);
                if (b == 0) break;
                else bytes.Add(b);
            }
            return Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Count);
        }

        #endregion



        #endregion

        #region Event
        /// <summary>
        /// 设备登录事件
        /// </summary>
        public event EventHandler<HikvisionFaceEventArgs> Logined;
        private void RaiseLogined(HikvisionFaceState state)
        {
            if (Logined != null)
            {
                Logined(this, new HikvisionFaceEventArgs(state));
            }
        }

        /// <summary>
        /// 设备登出事件
        /// </summary>
        public event EventHandler<HikvisionFaceEventArgs> LogOuted;
        private void RaiseLogOuted(HikvisionFaceState state)
        {
            if (LogOuted != null)
            {
                LogOuted(this, new HikvisionFaceEventArgs(state));
            }
        }

        /// <summary>
        /// 建立远程连接事件
        /// </summary>
        public event EventHandler<HikvisionFaceEventArgs> StartRemoteConfiged;
        private void RaiseStartRemoteConfiged(HikvisionFaceState state)
        {
            if (StartRemoteConfiged != null)
            {
                StartRemoteConfiged(this, new HikvisionFaceEventArgs(state));
            }
        }

        /// <summary>
        /// 获取门参数事件
        /// </summary>
        public event EventHandler<HikvisionFaceEventArgs> GetDoorConfiged;
        private void RaiseGetDoorConfiged(HikvisionFaceState state)
        {
            if (GetDoorConfiged != null)
            {
                GetDoorConfiged(this, new HikvisionFaceEventArgs(state));
            }
        }

        /// <summary>
        /// 接收卡信息事件
        /// </summary>
        public event EventHandler<HikvisionFaceEventArgs> ReceiveCardConfiged;
        private void RaiseReceiveCardConfiged(HikvisionFaceState state)
        {
            if (ReceiveCardConfiged != null)
            {
                ReceiveCardConfiged(this, new HikvisionFaceEventArgs(state));
            }
        }

        /// <summary>
        /// 设置卡信息事件
        /// </summary>
        public event EventHandler<HikvisionFaceEventArgs> SendCardConfiged;
        private void RaiseSendCardConfiged(HikvisionFaceState state)
        {
            isSetCardComplete = true;
            if (SendCardConfiged != null)
            {
                SendCardConfiged(this, new HikvisionFaceEventArgs(state));
            }
        }

        /// <summary>
        /// 获取人脸参数事件
        /// </summary>
        public event EventHandler<HikvisionFaceEventArgs> ReceiveFaceConfiged;
        private void RaiseReceiveFaceConfiged(HikvisionFaceState state)
        {
            if (ReceiveFaceConfiged != null)
            {
                ReceiveFaceConfiged(this, new HikvisionFaceEventArgs(state));
            }
        }

        /// <summary>
        /// 发送人脸参数事件
        /// </summary>
        public event EventHandler<HikvisionFaceEventArgs> SendFaceConfiged;
        private void RaiseSendFaceConfiged(HikvisionFaceState state)
        {
            isSetFaceComplete = true;
            if (SendFaceConfiged != null)
            {
                SendFaceConfiged(this, new HikvisionFaceEventArgs(state));
            }
        }

        /// <summary>
        /// NVR：已建立，上传人脸数据完成事件
        /// </summary>
        public event EventHandler<HikvisionFaceEventArgs> FaceUpLoaded;
        private void RaiseFaceUpLoaded(HikvisionFaceState state)
        {
            if (FaceUpLoaded != null)
            {
                FaceUpLoaded(this, new HikvisionFaceEventArgs(state));
            }
        }

        /// <summary>
        /// 门禁主机，刷脸过闸事件
        /// </summary>
        public event EventHandler<HikvisionFaceEventArgs> AcsAlarmed;
        private void RaiseAcsAlarmed(HikvisionFaceState state)
        {
            if (AcsAlarmed != null)
            {
                AcsAlarmed(this, new HikvisionFaceEventArgs(state));
            }
        }

        /// <summary>
        /// NVR上传人脸比对事件
        /// </summary>
        public event EventHandler<HikvisionFaceEventArgs> NVRFaceMatched;
        private void RaiseNVRFaceMatched(HikvisionFaceState state)
        {
            if (NVRFaceMatched != null)
            {
                NVRFaceMatched(this, new HikvisionFaceEventArgs(state));
            }
        }

        /// <summary>
        /// 异常事件
        /// </summary>
        public event EventHandler<HikvisionFaceEventArgs> OtherException;
        /// <summary>
        /// 触发异常事件
        /// </summary>
        /// <param name="state"></param>
        private void RaiseOtherException(HikvisionFaceState state, string descrip)
        {
            if (OtherException != null)
            {
                OtherException(this, new HikvisionFaceEventArgs(descrip, state));
            }
        }
        private void RaiseOtherException(HikvisionFaceState state)
        {
            RaiseOtherException(state, "");
        }

        #endregion

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            try
        //            {
        //                LoginOut();

        //            }
        //            catch (Exception e)
        //            {
        //                //TODO
        //                RaiseOtherException(state);
        //            }
        //        }
        //        disposed = true;
        //    }
        //}
    }
}
