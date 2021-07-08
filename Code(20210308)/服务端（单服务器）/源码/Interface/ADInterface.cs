using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using ADServer.DAL;
using ADServer.Utils;
using System.Web.Script.Serialization;
using WG3000_COMM.Core;
using ADServer.BLL;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using ADServer.Model;
using System.Threading;
using System.Data;
using FKY_WCFLibrary;
using Newtonsoft.Json.Linq;

namespace ADServer.Interface
{
    public class ADInterface
    {
        public static ADSHalDataStruct.ADS_Comadapter m_comAdatpter = new ADSHalDataStruct.ADS_Comadapter();
        public static ADSHalDataStruct.ADS_CommunicationParameter m_comm = new ADSHalDataStruct.ADS_CommunicationParameter();
        public static ADSHalDataStruct.ADS_ControllerInformation[] m_controllers = new ADSHalDataStruct.ADS_ControllerInformation[256];
        private static B_Card_Info bll_card_info = new B_Card_Info();
        private static B_WG_Config bll_wgconfig = new B_WG_Config();

        public ADInterface()
        {
            m_comAdatpter.address = 0;
            m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
            m_comAdatpter.port = 0;
        }

        //<summary>
        //登录并获取授权令牌。
        //</summary>
        //<returns>登录成功返回 true，登录失败返回 false。</returns>
        protected static bool Authenticate(string token)
        {
            try
            {
                string key = ConfigurationManager.AppSettings["APIToken"].ToString(); //加解密密钥
                token = SysFunc.Decrypt(token, key);
                string date = token.Substring(0, 4);
                if (date != DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0'))
                {
                    return false;
                }
                else if (token.Substring(4) == key)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 请求参数无效
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static string InvalidPostData(string errorMsg)
        {
            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "请求参数缺失", errorMsg);
        }

        /// <summary>
        /// 搜索门禁控制器
        /// </summary>
        /// <param name="token"></param>
        /// <param name="strAdType"></param>
        /// <returns></returns>
        public static string SearchController(string token, string strAdType, bool needToken = true)
        {
            if (!Authenticate(token) && needToken)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                if (strAdType == "TSV-WG")
                {
                    System.Collections.ArrayList arrControllers = new System.Collections.ArrayList();

                    using (wgMjController control = new wgMjController())
                    {
                        control.SearchControlers(ref arrControllers);
                    }
                    if (arrControllers != null)
                    {
                        if (arrControllers.Count <= 0)
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "搜索成功", "没有找到控制器");
                        }

                        List<object> controllerList = new List<object>();


                        wgMjControllerConfigure conf;
                        for (int i = 0; i < arrControllers.Count; i++)
                        {
                            conf = (wgMjControllerConfigure)arrControllers[i];
                            var controller = new
                            {
                                sn = conf.controllerSN.ToString(),
                                ip = conf.ip.ToString(),         //IP
                                port = conf.port.ToString(),      //PORT
                                mask = conf.mask.ToString(),       //MASK,
                                gateway = conf.gateway.ToString(), //Gateway,
                                mac = conf.MACAddr,               //MAC
                                pcIPAddr = conf.pcIPAddr,          //Note [pcIPAddr]
                                adType = "TSV-WG"
                            };
                            controllerList.Add(controller);
                        }

                        var data = new
                        {
                            controller = controllerList
                        };

                        string dataJson = JsonHelper.ToJson(data);

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "搜索成功", dataJson);
                    }
                }
                else if (strAdType == "TSV-SJ")
                {
                    ADSHalDataStruct.ADS_ControllerInformation[] searchedDevs = new ADSHalDataStruct.ADS_ControllerInformation[256];
                    uint uRetCount = 0;
                    string resultString = "";
                    if (SearchDevices(ref searchedDevs, ref uRetCount, ref resultString))
                    {
                        if (uRetCount > 0)
                        {
                            List<object> controllerList = new List<object>();

                            for (uint uIndex = 0; uIndex < uRetCount; uIndex++)
                            {
                                var controller = new
                                {
                                    sn = searchedDevs[uIndex].deviceID.ToString(),
                                    ip = ADSHelp.Int2IP(searchedDevs[uIndex].commParam.deviceAddr),   //IP
                                    port = "65001",   //PORT
                                    adType = "TSV-SJ"
                                };
                                controllerList.Add(controller);
                            }

                            var data = new
                            {
                                controller = controllerList
                            };

                            string dataJson = JsonHelper.ToJson(data);

                            JavaScriptSerializer js = new JavaScriptSerializer();
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "搜索成功", dataJson);
                        }
                        else
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "搜索成功", "没有找到控制器");
                        }
                    }
                    else
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "搜索失败", resultString);
                    }

                }

                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁型号不存在", "");
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 搜索盛炬门禁控制器
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="count"></param>
        /// <param name="resultString"></param>
        /// <returns></returns>
        private static Boolean SearchDevices(ref ADSHalDataStruct.ADS_ControllerInformation[] infos,
            ref uint count, ref string resultString)
        {
            try
            {
                m_comAdatpter.address = 0;
                m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
                m_comAdatpter.port = 0;

                int info_size = Marshal.SizeOf(typeof(ADSHalDataStruct.ADS_ControllerInformation));
                IntPtr ptArray = Marshal.AllocHGlobal(info_size * 256);

                uint uRetCount = 0;
                int iResult = ADSHalAPI.ADS_SearchController(ref m_comAdatpter, 1, 256, ptArray, 256, ref uRetCount);
                if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS && uRetCount > 0)
                {
                    for (int i = 0; i < uRetCount; i++)
                    {
                        IntPtr ptr = (IntPtr)((UInt32)ptArray + i * info_size);
                        infos[i] = (ADSHalDataStruct.ADS_ControllerInformation)Marshal.PtrToStructure(ptr, typeof(ADSHalDataStruct.ADS_ControllerInformation));
                    }
                }

                Marshal.FreeHGlobal(ptArray);

                count = uRetCount;

                Thread.Sleep(2000);
                GC.Collect();

                return true;
            }
            catch (System.Exception ex)
            {
                resultString = ex.Message;
            }

            return false;
        }

        /// <summary>
        /// 修改控制器
        /// </summary>
        /// <param name="token"></param>
        /// <param name="strAdType"></param>
        /// <param name="strSN"></param>
        /// <param name="strIP"></param>
        /// <param name="strMac"></param>
        /// <param name="strMask"></param>
        /// <param name="strGateway"></param>
        /// <param name="strPort"></param>
        /// <param name="strPcIPAddr"></param>
        /// <returns></returns>
        public static string SetController(string token, string strAdType, string strSN, string strIP, string strPort, string strMac, string strMask, string strGateway, string strPcIPAddr)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                if (strAdType == "TSV-WG")
                {

                    using (wgMjController control = new wgMjController())
                    {
                        control.NetIPConfigure(strSN, strMac, strIP, strMask, strGateway, strPort, strPcIPAddr);
                    }

                    return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "修改成功", "");
                }
                else if (strAdType == "TSV-SJ")
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "当前门禁型号未开放此项功能", "");
                }

                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁型号不存在", "");
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 连接控制器
        /// </summary>
        /// <param name="token"></param>
        /// <param name="strAdType"></param>
        /// <param name="strSN"></param>
        /// <param name="strIP"></param>
        /// <param name="strPort"></param>
        /// <returns></returns>
        public static string ConnectController(string token, string strAdType, string strSN, string strIP, string strPort)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                if (strAdType == "TSV-WG")
                {
                    wgMjController wgController = new wgMjController();  //此窗体使用的控制器
                    //加载配置
                    wgController.ControllerSN = int.Parse(strSN);
                    wgController.IP = strIP;
                    wgController.PORT = int.Parse(strPort);
                    if (wgController.GetMjControllerRunInformationIP() > 0) //取控制器信息
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "连接成功", "");
                    }
                    else
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "连接失败", "");
                    }
                }
                else if (strAdType == "TSV-SJ")
                {
                    m_comAdatpter.address = 0;
                    m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
                    m_comAdatpter.port = 0;

                    m_comm.deviceAddr = ADSHelp.IP2Int(strIP);
                    m_comm.password = (ushort)(Convert.ToUInt16("0"));
                    //使用UDP通讯
                    m_comm.reserve = new byte[3];
                    m_comm.reserve[0] = (byte)1;
                    m_comm.devicePort = (ushort)65001;

                    int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                    ADSHelp.PromptResult(iResult, true);

                    if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                    {
                        initSJPermission(strSN, strIP);

                        return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "连接成功", "");
                    }
                    else
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "连接失败", "");
                    }
                }

                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁型号不存在", "");
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 初始化盛炬门禁控制器的部门门点权限
        /// </summary>
        /// <param name="config"></param>
        private static void initSJPermission(string strSN, string strIP)
        {
            /// <summary>
            /// 门禁控制器是否连接
            /// </summary>
            bool m_bConnected = false;

            ADSHalDataStruct.ADS_Comadapter m_comAdatpter = new ADSHalDataStruct.ADS_Comadapter();
            ADSHalDataStruct.ADS_CommunicationParameter m_comm = new ADSHalDataStruct.ADS_CommunicationParameter();
            ADSHalDataStruct.ADS_ControllerInformation[] m_controllers = new ADSHalDataStruct.ADS_ControllerInformation[256];

            m_comAdatpter.address = 0;
            m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
            m_comAdatpter.port = 0;

            m_comm.deviceAddr = ADSHelp.IP2Int(strIP);
            m_comm.password = (ushort)(Convert.ToUInt16("0"));
            //使用UDP通讯
            m_comm.reserve = new byte[3];
            m_comm.reserve[0] = (byte)1;
            m_comm.devicePort = (ushort)65001;

            if (m_bConnected)
            {
                //配置控制器门1、门2的默认门点组合权限
                string[] doorCode = new string[] { "1111", "1010", "0101", "1100", "0011", "1001", "0110", "1000", "0100", "0010", "0001", "1110", "0111", "1101", "1011" };
                for (int i = 0; i < doorCode.Length; i++)
                {
                    char[] codeArr = doorCode[i].ToCharArray();
                    int door1Entry = int.Parse(codeArr[0].ToString());
                    int door1Exit = int.Parse(codeArr[1].ToString());
                    int door2Entry = int.Parse(codeArr[2].ToString());
                    int door2Exit = int.Parse(codeArr[3].ToString());
                    ADSDoor.SetDoorCode(1, door1Entry, door1Exit, door2Entry, door2Exit, ref m_comAdatpter, ref m_comm);
                    ADSDoor.SetDoorCode(2, door1Entry, door1Exit, door2Entry, door2Exit, ref m_comAdatpter, ref m_comm);
                }
            }
        }

        /// <summary>
        /// 格式化控制器
        /// </summary>
        /// <param name="token"></param>
        /// <param name="strAdType"></param>
        /// <param name="strSN"></param>
        /// <param name="strIP"></param>
        /// <param name="strPort"></param>
        /// <returns></returns>
        public static string FormatController(string token, string strAdType, string strSN, string strIP, string strPort)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                if (strAdType == "TSV-WG")
                {
                    wgMjController wgController = new wgMjController();  //此窗体使用的控制器
                    //加载配置
                    wgController.ControllerSN = int.Parse(strSN);
                    wgController.IP = strIP;
                    wgController.PORT = int.Parse(strPort);
                    if (wgController.FormatIP() > 0)
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "格式化成功", "");
                    }
                    else
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "格式化失败", "");
                    }
                }
                else if (strAdType == "TSV-SJ")
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

                        m_comm.deviceAddr = ADSHelp.IP2Int(strIP);
                        m_comm.password = (ushort)(Convert.ToUInt16("0"));
                        //使用UDP通讯
                        m_comm.reserve = new byte[3];
                        m_comm.reserve[0] = (byte)1;
                        m_comm.devicePort = (ushort)65001;

                        int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                        ADSHelp.PromptResult(iResult, true);

                        if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                        {
                            //string cMsg = "\r\n已连接门禁控制器信息：\r\n";
                            m_bConnected = true;
                        }
                        else
                        {
                            //string cMsg = "\r\n连接门禁控制器失败！请检查配置！";
                            m_bConnected = false;
                        }

                        if (m_bConnected)
                        {
                            iResult = ADSHalAPI.ADS_FormatController(ref m_comAdatpter, ref m_comm, 20000);
                            ADSHelp.PromptResult(iResult, true);
                        }
                    }
                    catch
                    {
                        m_bConnected = false;
                    }

                    if (m_bConnected)
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "格式化成功", "");
                    }
                    else
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "格式化失败", "");
                    }
                }

                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁型号不存在", "");
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 以本机时间矫正控制器时间
        /// </summary>
        /// <param name="token"></param>
        /// <param name="strAdType"></param>
        /// <param name="strSN"></param>
        /// <param name="strIP"></param>
        /// <param name="strPort"></param>
        /// <returns></returns>
        public static string AdjustTimeController(string token, string strAdType, string strSN, string strIP, string strPort, string strTime)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                if (strAdType == "TSV-WG")
                {
                    wgMjController wgController = new wgMjController();  //此窗体使用的控制器
                    //加载配置
                    wgController.ControllerSN = int.Parse(strSN);
                    wgController.IP = strIP;
                    wgController.PORT = int.Parse(strPort);

                    DateTime dtTime;
                    if (DateTime.TryParse(strTime, out dtTime))
                    {
                        if (wgController.AdjustTimeIP(dtTime) > 0)
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "同步时间成功", "");
                        }
                        else
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "同步时间失败", "");
                        }
                    }
                    else
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "同步时间失败", "输入时间无效");
                    }
                }
                else if (strAdType == "TSV-SJ")
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

                        m_comm.deviceAddr = ADSHelp.IP2Int(strIP);
                        m_comm.password = (ushort)(Convert.ToUInt16("0"));
                        //使用UDP通讯
                        m_comm.reserve = new byte[3];
                        m_comm.reserve[0] = (byte)1;
                        m_comm.devicePort = (ushort)65001;

                        int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                        ADSHelp.PromptResult(iResult, true);

                        if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                        {
                            //string cMsg = "\r\n已连接门禁控制器信息：\r\n";
                            m_bConnected = true;
                        }
                        else
                        {
                            //string cMsg = "\r\n连接门禁控制器失败！请检查配置！";
                            m_bConnected = false;
                        }

                        ADSHalDataStruct.ADS_YMDHMS time = new ADSHalDataStruct.ADS_YMDHMS();

                        DateTime dtTime;
                        if (DateTime.TryParse(strTime, out dtTime))
                        {
                            time.year = (byte)(dtTime.Year - 2000);
                            time.month = (byte)dtTime.Month;
                            time.day = (byte)dtTime.Day;
                            time.hour = (byte)dtTime.Hour;
                            time.minute = (byte)dtTime.Minute;
                            time.sec = (byte)dtTime.Second;

                            iResult = ADSHalAPI.ADS_SetTime(ref m_comAdatpter, ref m_comm, ref time);
                            ADSHelp.PromptResult(iResult, true);
                        }
                        else
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "同步时间失败", "输入时间无效");
                        }
                    }
                    catch
                    {
                        m_bConnected = false;
                    }

                    if (m_bConnected)
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "同步时间成功", "");
                    }
                    else
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "同步时间失败", "");
                    }
                }

                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁型号不存在", "");
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        public static string GetTimeController(string token, string strAdType, string strSN, string strIP, string strPort)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                if (strAdType == "TSV-WG")
                {
                    wgMjController wgController = new wgMjController();  //使用的控制器
                    //加载配置
                    wgController.ControllerSN = int.Parse(strSN);
                    wgController.IP = strIP;
                    wgController.PORT = int.Parse(strPort);

                    if (wgController.GetMjControllerRunInformationIP() > 0)
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "获取时间成功", wgController.RunInfo.dtNow.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "获取时间失败", "");
                    }
                }
                else if (strAdType == "TSV-SJ")
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

                        m_comm.deviceAddr = ADSHelp.IP2Int(strIP);
                        m_comm.password = (ushort)(Convert.ToUInt16("0"));
                        //使用UDP通讯
                        m_comm.reserve = new byte[3];
                        m_comm.reserve[0] = (byte)1;
                        m_comm.devicePort = (ushort)65001;

                        int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                        ADSHelp.PromptResult(iResult, true);

                        if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                        {
                            //string cMsg = "\r\n已连接门禁控制器信息：\r\n";
                            m_bConnected = true;
                        }
                        else
                        {
                            //string cMsg = "\r\n连接门禁控制器失败！请检查配置！";
                            m_bConnected = false;
                        }

                        ADSHalDataStruct.ADS_YMDHMS time = new ADSHalDataStruct.ADS_YMDHMS();

                        iResult = ADSHalAPI.ADS_GetTime(ref m_comAdatpter, ref m_comm, ref time);
                        ADSHelp.PromptResult(iResult, true);
                        if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                        {
                            DateTime timeController = new DateTime(time.year + 2000, time.month, time.day, time.hour, time.minute, time.sec);

                            return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "获取时间成功", timeController.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "获取时间失败", "");
                        }
                    }
                    catch
                    {
                        m_bConnected = false;
                    }

                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "获取时间失败", "");
                }

                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁型号不存在", "");
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        public static string SetPeriod(string token, string strAdType, string strSN, string strIP, string strPort, string strConfigs)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                if (strAdType == "TSV-WG")
                {
                    wgMjController wgController = new wgMjController();  //此窗体使用的控制器
                    //加载配置
                    wgController.ControllerSN = int.Parse(strSN);
                    wgController.IP = strIP;
                    wgController.PORT = int.Parse(strPort);

                    object[] configs = JsonHelper.FromJsonList(strConfigs);
                    wgMjControllerTimeSegList controlTimeSegList = new wgMjControllerTimeSegList();

                    for (int i = 0; i < configs.Length; i++)
                    {
                        Dictionary<string, object> config = configs[i] as Dictionary<string, object>;

                        string dtFromTime1 = config["dtFromTime1"].ToString();
                        string dtToTime1 = config["dtToTime1"].ToString();
                        string dtFromTime2 = config["dtFromTime2"].ToString();
                        string dtToTime2 = config["dtToTime2"].ToString();
                        string dtFromTime3 = config["dtFromTime3"].ToString();
                        string dtToTime3 = config["dtToTime3"].ToString();
                        string strWeek = config["strWeek"].ToString();
                        int iPeroidId = int.Parse(config["iPeroidId"].ToString());


                        DateTime dtpTimeZoneFrom1;
                        DateTime dtpTimeZoneTo1;
                        DateTime dtpTimeZoneFrom2;
                        DateTime dtpTimeZoneTo2;
                        DateTime dtpTimeZoneFrom3;
                        DateTime dtpTimeZoneTo3;
                        bool isValidTimeFrom = DateTime.TryParse("2010-1-1 " + dtFromTime1, out dtpTimeZoneFrom1);
                        bool isValidTimeTo = DateTime.TryParse("2010-1-1 " + dtToTime1, out dtpTimeZoneTo1);
                        isValidTimeFrom = DateTime.TryParse("2010-1-1 " + dtFromTime2, out dtpTimeZoneFrom2);
                        isValidTimeTo = DateTime.TryParse("2010-1-1 " + dtToTime2, out dtpTimeZoneTo2);
                        isValidTimeFrom = DateTime.TryParse("2010-1-1 " + dtFromTime3, out dtpTimeZoneFrom3);
                        isValidTimeTo = DateTime.TryParse("2010-1-1 " + dtToTime3, out dtpTimeZoneTo3);

                        if (!isValidTimeFrom || !isValidTimeTo)
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "通行时间无效", "");
                        }

                        if (dtpTimeZoneFrom1 > dtpTimeZoneTo1)
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "开始时间不能晚于结束时间", "");
                        }

                        bool bMonday = false;
                        bool bTuesday = false;
                        bool bWednesday = false;
                        bool bThursday = false;
                        bool bFriday = false;
                        bool bSaturday = false;
                        bool bSunday = false;

                        if (strWeek.Length == 7)
                        {
                            char[] dateArr = strWeek.ToCharArray();

                            bMonday = dateArr[0] == '1' ? true : false;
                            bTuesday = dateArr[1] == '1' ? true : false;
                            bWednesday = dateArr[2] == '1' ? true : false;
                            bThursday = dateArr[3] == '1' ? true : false;
                            bFriday = dateArr[4] == '1' ? true : false;
                            bSaturday = dateArr[5] == '1' ? true : false;
                            bSunday = dateArr[6] == '1' ? true : false;
                        }
                        else
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "通行星期信息无效", "");
                        }



                        MjControlTimeSeg mjCI = new MjControlTimeSeg();
                        mjCI.ymdStart = new DateTime(2011, 3, 18, 18, 18, 0, 0);
                        mjCI.ymdEnd = new DateTime(2029, 12, 31, 14, 44, 0, 0);
                        mjCI.SegIndex = (byte)(iPeroidId);
                        mjCI.TotalLimittedAccess = 0;
                        mjCI.LimittedMode = 0;
                        mjCI.nextSeg = 0;
                        mjCI.weekdayControl = 0;
                        mjCI.weekdayControl += (byte)(bMonday ? (1 << 0) : 0);
                        mjCI.weekdayControl += (byte)(bTuesday ? (1 << 1) : 0);
                        mjCI.weekdayControl += (byte)(bWednesday ? (1 << 2) : 0);
                        mjCI.weekdayControl += (byte)(bThursday ? (1 << 3) : 0);
                        mjCI.weekdayControl += (byte)(bFriday ? (1 << 4) : 0);
                        mjCI.weekdayControl += (byte)(bSaturday ? (1 << 5) : 0);
                        mjCI.weekdayControl += (byte)(bSunday ? (1 << 6) : 0);
                        mjCI.hmsStart1 = dtpTimeZoneFrom1;
                        mjCI.hmsEnd1 = dtpTimeZoneTo1;
                        mjCI.hmsStart2 = dtpTimeZoneFrom2;
                        mjCI.hmsEnd2 = dtpTimeZoneTo2;
                        mjCI.hmsStart3 = dtpTimeZoneFrom3;
                        mjCI.hmsEnd3 = dtpTimeZoneTo3;
                        mjCI.LimittedAccess1 = (byte)(0);
                        mjCI.LimittedAccess2 = (byte)(0);
                        mjCI.LimittedAccess3 = (byte)(0);
                        //this.listBox2.Items.Add(this.comboBox57.Text.ToString() + " , " + System.BitConverter.ToString(mjCI.ToBytes()));
                        controlTimeSegList.AddItem(mjCI);
                    }

                    if (controlTimeSegList != null)
                    {
                        if (wgController.UpdateControlTimeSegListIP(controlTimeSegList.ToByte()) > 0)//上传时段列表
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "配置通行时间成功", "");
                        }
                        else
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "配置通行时间失败", "");
                        }
                    }
                    else
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "配置通行时间失败", "");
                    }
                }
                else if (strAdType == "TSV-SJ")
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "当前门禁型号未开放此项功能", "");
                }

                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁型号不存在", "");
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        public static string AddCard(string token, string strAdType, string strSN, string strIP, string strPort, string strCardno, string strDoor,
            string dtFromTime, string dtToTime, int iPeroidId, String strDeptId)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                char[] dateArr = strDoor.ToCharArray();

                bool doorIn = dateArr[0] == '1' ? true : false;
                bool doorOut = dateArr[1] == '1' ? true : false;

                DateTime fromTime;
                DateTime toTime;
                bool isValidTimeFrom = DateTime.TryParse(dtFromTime + " 00:00:00", out fromTime);
                bool isValidTimeTo = DateTime.TryParse(dtToTime + " 00:00:00", out toTime);

                if (!isValidTimeFrom || !isValidTimeTo)
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "通行时间无效", "");
                }


                if (strAdType == "TSV-WG")
                {
                    UInt32 cardid;
                    if (!UInt32.TryParse(strCardno, System.Globalization.NumberStyles.Integer, null, out cardid))
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "授权失败", "非法卡号");
                    }

                    MjRegisterCard mjrc = new MjRegisterCard();

                    //注册卡信息修改
                    mjrc.CardID = cardid; //卡号 
                    mjrc.Password = uint.Parse("368660"); //密码
                    mjrc.ymdStart = fromTime;  //起始日期
                    mjrc.ymdEnd = toTime;  //结束日期

                    if (doorIn)
                    {
                        mjrc.ControlSegIndexSet(1, (byte)iPeroidId); //授权时段
                    }
                    if (doorOut)
                    {
                        mjrc.ControlSegIndexSet(2, (byte)iPeroidId); //授权时段
                    }

                    int ret = -1;
                    using (wgMjControllerPrivilege pri = new wgMjControllerPrivilege())
                    {
                        ret = pri.AddPrivilegeOfOneCardIP(int.Parse(strSN), strIP, int.Parse(strPort), mjrc);
                        GC.Collect();
                    }
                    if (ret >= 0)
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "授权成功", "");
                    }
                    else
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "授权失败", "");
                    }
                }
                else if (strAdType == "TSV-SJ")
                {
                    UInt32 cardid;
                    if (!UInt32.TryParse(strCardno, System.Globalization.NumberStyles.Integer, null, out cardid))
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "授权失败", "非法卡号");
                    }

                    /// <summary>
                    /// 门禁控制器是否连接
                    /// </summary>
                    bool m_bConnected = false;

                    ADSHalDataStruct.ADS_Comadapter m_comAdatpter = new ADSHalDataStruct.ADS_Comadapter();
                    ADSHalDataStruct.ADS_CommunicationParameter m_comm = new ADSHalDataStruct.ADS_CommunicationParameter();
                    ADSHalDataStruct.ADS_ControllerInformation[] m_controllers = new ADSHalDataStruct.ADS_ControllerInformation[256];

                    m_comAdatpter.address = 0;
                    m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
                    m_comAdatpter.port = 0;

                    // 连接
                    try
                    {
                        m_comm.deviceAddr = ADSHelp.IP2Int(strIP);
                        //m_comm.devicePort = (ushort)(Convert.ToUInt16(accessDoor.port));
                        m_comm.password = (ushort)(Convert.ToUInt16("0"));
                        //使用UDP通讯
                        m_comm.reserve = new byte[3];
                        m_comm.reserve[0] = (byte)1;
                        m_comm.devicePort = (ushort)65001;

                        int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                        ADSHelp.PromptResult(iResult, true);

                        if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                        {
                            //string cMsg = "\r\n已连接门禁控制器信息：\r\n";
                            m_bConnected = true;
                        }
                        else
                        {
                            //string cMsg = "\r\n连接门禁控制器失败！请检查配置！";
                            m_bConnected = false;

                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "授权失败", "连接门禁控制器失败");
                        }
                    }
                    catch
                    {
                        //string cMsg = "连接门禁控制器失败！请检查配置！";
                        //this.Invoke(controllerMsg, new string[] { cMsg });
                        m_bConnected = false;
                    }

                    if (m_bConnected)
                    {
                        string doorCode = "";
                        if (doorIn)
                        {
                            doorCode = "1"; //授权时段
                        }
                        doorCode += "00";
                        if (doorOut)
                        {
                            doorCode += "1"; //授权时段
                        }

                        ADSHalDataStruct.ADS_CardHolder card = new ADSHalDataStruct.ADS_CardHolder();
                        card.cardNumber.LoNumber = Convert.ToUInt32(strCardno);
                        card.cardNumber.HiNumber = Convert.ToUInt32("0");
                        card.password = Convert.ToUInt32("0");
                        ushort grantDeptID = ushort.Parse("1" + doorCode);
                        card.departmentID = grantDeptID;  //部门id
                        card.groupNumber = 1; //特权卡，只受失效日期、权限和门点互锁的限制，不受通行时段、节假日、子设备工作模式、APB和刷卡次数等的影响。

                        DateTime endTime = toTime;
                        card.expirationDate.year = (byte)(endTime.Year - 2000);
                        card.expirationDate.month = (byte)endTime.Month;
                        card.expirationDate.day = (byte)endTime.Day;
                        card.expirationDate.hour = (byte)23;
                        card.expirationDate.minute = (byte)59;
                        card.expirationDate.sec = (byte)59;
                        uint uRetCount = 0;

                        int iResult = ADSHalAPI.ADS_SetCardHolders(ref m_comAdatpter, ref m_comm, ref card, 1, ref uRetCount);
                        if (iResult != (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "授权失败", ADSHalAPI.ADS_Helper_GetErrorMessage((uint)iResult));
                        }
                        else
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "授权成功", "");
                        }
                    }
                }

                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁型号不存在", "");
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        public static string DeleteCard(string token, string strAdType, string strSN, string strIP, string strPort, string strCardno)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                if (strAdType == "TSV-WG")
                {
                    UInt32 cardid;
                    if (!UInt32.TryParse(strCardno, System.Globalization.NumberStyles.Integer, null, out cardid))
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "取消卡权限失败", "非法卡号");
                    }

                    using (wgMjControllerPrivilege pri = new wgMjControllerPrivilege())
                    {
                        if (pri.DelPrivilegeOfOneCardIP(int.Parse(strSN), strIP, int.Parse(strPort), cardid) >= 0)
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "取消卡权限成功", "");
                        }
                        else
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "取消卡权限失败", "");
                        }
                    }
                }
                else if (strAdType == "TSV-SJ")
                {
                    /// <summary>
                    /// 门禁控制器是否连接
                    /// </summary>
                    bool m_bConnected = false;

                    // 连接

                    m_comm.deviceAddr = ADSHelp.IP2Int(strIP);
                    m_comm.password = (ushort)(Convert.ToUInt16("0"));
                    //使用UDP通讯
                    m_comm.reserve = new byte[3];
                    m_comm.reserve[0] = (byte)1;
                    m_comm.devicePort = (ushort)65001;

                    int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                    if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                    {
                        //string cMsg = "\r\n已连接门禁控制器信息：\r\n";
                        m_bConnected = true;
                    }
                    else
                    {
                        //string cMsg = "\r\n连接门禁控制器失败！请检查配置！";
                        m_bConnected = false;

                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "取消卡权限失败", "连接门禁控制器失败");
                    }

                    if (m_bConnected)
                    {

                        ADSHalDataStruct.ADS_CardHolder card = new ADSHalDataStruct.ADS_CardHolder();
                        card.cardNumber.LoNumber = Convert.ToUInt32(strCardno);
                        card.cardNumber.HiNumber = Convert.ToUInt32("0");

                        iResult = ADSHalAPI.ADS_DeleteCardHolder(ref m_comAdatpter, ref m_comm, ref card);

                        if (iResult != (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "取消卡权限失败", ADSHalAPI.ADS_Helper_GetErrorMessage((uint)iResult));
                        }
                        else
                        {
                            return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "取消卡权限成功", "");
                        }

                    }
                }

                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁型号不存在", "");
            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }
        }

        /// <summary>
        /// 获取门禁刷卡流水记录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="strAdType"></param>
        /// <param name="strSn"></param>
        /// <param name="strCardno"></param>
        /// <param name="dtFromTime"></param>
        /// <param name="dtToTime"></param>
        /// <param name="strEvent"></param>
        /// <returns></returns>
        public static string EventFlow(string token, string strAdType, string strSn, string strCardno, string dtFromTime, string dtToTime, string strEvent)
        {
            if (!Authenticate(token))
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
            }

            try
            {
                DateTime fromTime = dtFromTime != "" ? DateTime.Parse(dtFromTime) : DateTime.Now.AddYears(-1);
                DateTime toTime = dtFromTime != "" ? DateTime.Parse(dtToTime) : DateTime.Now;

                List<M_AD_Eventflow> adEfList = new List<M_AD_Eventflow>();

                string[] files = null;
                if (strAdType == "TSV-WG")
                {
                    files = Directory.GetFiles(Application.StartupPath + "\\AD-WG\\Eventflow", "*.adl", SearchOption.TopDirectoryOnly);
                }
                else if (strAdType == "TSV-SJ")
                {
                    files = Directory.GetFiles(Application.StartupPath + "\\AD-SJ\\Eventflow", "*.adl", SearchOption.TopDirectoryOnly);
                }
                else
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁型号不存在", "");
                }

                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        FileInfo file = new FileInfo(files[i]);
                        string filename = file.Name.Replace(".adl", "");
                        DateTime filedate;
                        bool ret = DateTime.TryParse(filename + " 00:00:00", out filedate);
                        if (ret)
                        {
                            if (fromTime <= filedate && filedate <= toTime)
                            {
                                string[] lines = File.ReadAllLines(files[i]);
                                for (int l = 0; l < lines.Length; l++)
                                {
                                    Dictionary<string, object> rEvt = JsonHelper.FromJson(lines[l]);
                                    if (rEvt != null)
                                    {
                                        M_AD_Eventflow ef = new M_AD_Eventflow();
                                        ef.Sn = rEvt["sn"].ToString();
                                        ef.CardNo = rEvt["cardNo"].ToString();
                                        ef.DoorNo = rEvt["doorNo"].ToString();
                                        ef.ReaderNo = rEvt["readerNo"].ToString();
                                        ef.RecordTime = rEvt["recordTime"].ToString();
                                        ef.REvent = rEvt["rEvent"].ToString();

                                        if ((strSn != "" ? ef.Sn == strSn : true)
                                            && (strCardno != "" ? ef.CardNo == strCardno : true)
                                            && (strEvent != "" ? ef.REvent.Contains(strEvent) : true))
                                        {
                                            adEfList.Add(ef);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                string efListJson = JsonHelper.ToJson(adEfList);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "查询门禁记录成功", efListJson);


            }
            catch (Exception ex)
            {
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", ex.Message.ToString());
            }

        }

        public static void AdWGRecord(object oConfig)
        {
            try
            {
                Dictionary<string, object> config = oConfig as Dictionary<string, object>;

                DataTable dtSwipeRecord;
                dtSwipeRecord = new DataTable("SwipeRecord");
                dtSwipeRecord.Columns.Add("f_Index", System.Type.GetType("System.UInt32"));//记录索引位
                dtSwipeRecord.Columns.Add("f_ReadDate", System.Type.GetType("System.DateTime"));  //刷卡日期时间
                dtSwipeRecord.Columns.Add("f_CardNO", System.Type.GetType("System.UInt32"));  //用户卡号
                dtSwipeRecord.Columns.Add("f_DoorNO", System.Type.GetType("System.UInt32"));  //门号
                dtSwipeRecord.Columns.Add("f_InOut", System.Type.GetType("System.UInt32"));// =0表示出门;  =1 表示进门
                dtSwipeRecord.Columns.Add("f_ReaderNO", System.Type.GetType("System.UInt32")); // 读卡器号
                dtSwipeRecord.Columns.Add("f_EventCategory", System.Type.GetType("System.UInt32")); // 事件类型
                dtSwipeRecord.Columns.Add("f_ReasonNo", System.Type.GetType("System.UInt32"));// 硬件原因
                dtSwipeRecord.Columns.Add("f_ControllerSN", System.Type.GetType("System.UInt32"));// 控制器序列号
                dtSwipeRecord.Columns.Add("f_RecordAll", System.Type.GetType("System.String")); // 完整记录值
                int num = -1;
                using (wgMjControllerSwipeOperate swipe4GetRecords = new wgMjControllerSwipeOperate())
                {
                    swipe4GetRecords.Clear();
                    num = swipe4GetRecords.GetSwipeRecords(int.Parse(config["sn"].ToString()), config["ip"].ToString(), int.Parse(config["port"].ToString()), ref dtSwipeRecord);

                    Thread.Sleep(500);
                    DAL.SysFunc.ClearMemory();//手动清理内存
                }

                if (num >= 0)
                {
                    if (num == 0)
                    {
                    }
                    else
                    {
                        for (int i = 0; i < num; i++)
                        {
                            string sn = dtSwipeRecord.Rows[i]["f_ControllerSN"].ToString();
                            string cardNo = dtSwipeRecord.Rows[i]["f_CardNO"].ToString();
                            string readerNo = dtSwipeRecord.Rows[i]["f_ReaderNO"].ToString();
                            string recordTime = dtSwipeRecord.Rows[i]["f_ReadDate"].ToString();

                            string doorNo = "";

                            switch (readerNo)
                            {
                                case "1":
                                    doorNo = "1";
                                    break;
                                case "2":
                                    doorNo = "1";
                                    break;
                                case "3":
                                    doorNo = "2";
                                    break;
                                case "4":
                                    doorNo = "2";
                                    break;
                                default:
                                    break;
                            }

                            wgMjControllerSwipeRecord mjrec = new wgMjControllerSwipeRecord();

                            mjrec.Update(dtSwipeRecord.Rows[i]["f_RecordAll"] as string); //用新的记录进行更新
                            string rEvent = "\r\n" + mjrec.ToDisplaySimpleInfo(true); //

                            var eventData = new
                            {
                                sn = sn,
                                cardNo = cardNo,
                                recordTime = recordTime,
                                rEvent = rEvent,
                                doorNo = doorNo,
                                readerNo = readerNo
                            };

                            string dataJson = JsonHelper.ToJson(eventData);

                            if (!Directory.Exists(Application.StartupPath + "\\AD-WG"))
                            {
                                Directory.CreateDirectory(Application.StartupPath + "\\AD-WG");
                            }
                            if (!Directory.Exists(Application.StartupPath + "\\AD-WG\\Eventflow"))
                            {
                                Directory.CreateDirectory(Application.StartupPath + "\\AD-WG\\Eventflow");
                            }

                            DateTime dtRecordTime = DateTime.Parse(recordTime);
                            string curDate = dtRecordTime.ToString("yyyy-MM-dd");
                            string file = Application.StartupPath + "\\AD-WG\\Eventflow\\" + curDate + ".adl";
                            if (!File.Exists(file))
                            {
                                FileStream fs = new FileStream(file, FileMode.Create);
                                StreamWriter sw = new StreamWriter(fs);
                                sw.Write(dataJson);
                                sw.Close();
                                fs.Close();

                                string[] lines = File.ReadAllLines(file);

                            }
                            else
                            {
                                StreamWriter sw = new StreamWriter(file, true);//true表示追加
                                sw.Write("\r\n" + dataJson);
                                sw.Flush();
                                sw.Close();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                }
                string nowTime = DateTime.Now.ToString("yyyyMMdd");
                string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                if (!File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(ex.Message);
                    sw.Write(ex.ToString());
                    sw.Close();
                    fs.Close();
                }
            }
        }

        public static void AdSJRecord(object oConfig)
        {
            try
            {
                Dictionary<string, object> config = oConfig as Dictionary<string, object>;

                string sn = config["sn"].ToString();
                string ip = config["ip"].ToString();

                m_comAdatpter.address = 0;
                m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
                m_comAdatpter.port = 0;

                /// <summary>
                /// 门禁控制器是否连接
                /// </summary>
                bool m_bConnected = false;

                // 连接
                m_comm.deviceAddr = ADSHelp.IP2Int(ip);
                //m_comm.devicePort = 65000;
                m_comm.devicePort = 65001;
                m_comm.password = (ushort)(Convert.ToUInt16("0"));

                //使用UDP通讯
                m_comm.reserve = new byte[3];
                m_comm.reserve[0] = (byte)1;


                //bConnecting = true;
                int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                if (iResult != 1)
                {
                }
                else
                {
                    m_bConnected = true;
                }

                if (m_bConnected)
                {
                    ADSHalDataStruct.ADS_Event[] acsevent = new ADSHalDataStruct.ADS_Event[100];
                    uint uRetCount = 0;

                    iResult = ADSHalAPI.ADS_ReadEvents(ref m_comAdatpter, ref m_comm, ref acsevent[0], 100, ref uRetCount);
                    //if (iConnectResult != 1)
                    //{
                    //    ADSHelp.PromptResult(iConnectResult, true);
                    //}

                    if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS && uRetCount > 0)
                    {
                        // 设置已经读取的事件数，防止重复读取
                        ADSHalAPI.ADS_IncreaseEventCount(ref m_comAdatpter, ref m_comm, uRetCount);

                        for (int i = 0; i < uRetCount; i++)
                        {
                            string eventName = ADSHelp.GetEventName((ADSHalConstant.ADS_EventType)acsevent[i].type);


                            byte doorNum = acsevent[i].logicSubDeviceAddress.logicSubDevNumber; //门点号

                            DateTime eventTime = new DateTime(acsevent[i].time.year + 2000, acsevent[i].time.month, acsevent[i].time.day, acsevent[i].time.hour, acsevent[i].time.minute, acsevent[i].time.sec);
                            string recordTime = eventTime.ToString();

                            string cardNo = acsevent[i].cardNumber.LoNumber.ToString();

                            string doorNo = "";
                            string readerNo = "";

                            switch (doorNum)
                            {
                                case 1:
                                    doorNo = "1";
                                    break;
                                case 2:
                                    doorNo = "2";
                                    break;
                                default:
                                    break;
                            }

                            if (eventName.Contains("外部"))
                            {
                                if (doorNum == 1)
                                {
                                    readerNo = "1";
                                }
                                else if (doorNum == 2)
                                {
                                    readerNo = "2";
                                }
                            }
                            else if (eventName.Contains("内部"))
                            {
                                if (doorNum == 1)
                                {
                                    readerNo = "3";
                                }
                                else if (doorNum == 2)
                                {
                                    readerNo = "4";
                                }
                            }

                            var eventData = new
                            {
                                sn = sn,
                                cardNo = cardNo,
                                recordTime = recordTime,
                                rEvent = eventName,
                                doorNo = doorNo,
                                readerNo = readerNo
                            };

                            string dataJson = JsonHelper.ToJson(eventData);

                            if (!Directory.Exists(Application.StartupPath + "\\AD-SJ"))
                            {
                                Directory.CreateDirectory(Application.StartupPath + "\\AD-SJ");
                            }
                            if (!Directory.Exists(Application.StartupPath + "\\AD-SJ\\Eventflow"))
                            {
                                Directory.CreateDirectory(Application.StartupPath + "\\AD-SJ\\Eventflow");
                            }
                            string curDate = DateTime.Now.ToString("yyyy-MM-dd");
                            string file = Application.StartupPath + "\\AD-SJ\\Eventflow\\" + curDate + ".adl";
                            if (!File.Exists(file))
                            {
                                FileStream fs = new FileStream(file, FileMode.Create);
                                StreamWriter sw = new StreamWriter(fs);
                                sw.Write(dataJson);
                                sw.Close();
                                fs.Close();

                                string[] lines = File.ReadAllLines(file);

                            }
                            else
                            {
                                StreamWriter sw = new StreamWriter(file, true);//true表示追加
                                sw.Write("\r\n" + dataJson);
                                sw.Flush();
                                sw.Close();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                }
                string nowTime = DateTime.Now.ToString("yyyyMMdd");
                string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                if (!File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(ex.Message);
                    sw.Write(ex.ToString());
                    sw.Close();
                    fs.Close();
                }
            }
        }

        #region 第三方门禁服务接口
        /// <summary>
        /// 获取门禁控制器配置参数
        /// </summary>
        /// <param name="dataJson"></param>
        /// <returns></returns>
        public static string GetAllControllerByAcType(JObject dataJson)
        {
            string result = string.Empty;
            try
            {
                string strACType = dataJson["acType"].ToString();

                AccessType accessType = 0;
                try
                {
                    accessType = M_WG_Config.StrConvertToAccessType(strACType);
                    if ((int)accessType <= 0)
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁类型错误", "");
                }
                catch
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁类型错误", "");
                }

                string data = string.Empty;
                switch (accessType)
                {
                    case AccessType.TDZ:
                        {
                            List<Model.M_WG_Config> controllerList = new BLL.B_WG_Config().GetModelListByACType(accessType);
                            List<object> controllerObjectList = new List<object>();
                            foreach (var info in controllerList)
                            {
                                var controller = new
                                {
                                    sn = info.Sn,
                                    ipAddress = info.IpAddress,
                                    port = info.Port,
                                    passageway_name = info.Passageway,
                                    adType = info.Manufactor,
                                    doorNames=new 
                                    {
                                        doorName1=info.WGDoorNames.Split(',')[0],
                                        doorName2=info.WGDoorNames.Split(',')[1]
                                    }
                                };
                                controllerObjectList.Add(controller);
                            }
                            var jsonObject = new
                            {
                                controllerList = controllerObjectList
                            };
                            data = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject);
                        }
                        result = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "数据请求成功", data);
                        break;
                    default:
                        result = ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁型号不存在", "");
                        break;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log4Local(DateTime.Now.ToString() + " " + ex.Message.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
            return result;
        }

        /// <summary>
        /// 门禁卡权限下发
        /// </summary>
        /// <param name="dataJson"></param>
        /// <returns></returns>
        public static string AuthAccessCard(JObject dataJson)
        {
            string result = string.Empty;

            string strACType = string.Empty;
            string cardNum = string.Empty;
            bool? isTemporaryCard = null;
            string startTimeStamp = string.Empty;
            string endTimeStamp = string.Empty;
            List<M_AccessControllerAuthorization_Info> authorizeControllerList = new List<M_AccessControllerAuthorization_Info>();
            try
            {
                #region 参数校验
                strACType = dataJson.Value<string>("acType");
                cardNum = dataJson.Value<string>("cardNum");
                int iTemporary = dataJson.Value<int>("isTemporaryCard");
                if (iTemporary == 1)
                    isTemporaryCard = true;
                else if (iTemporary == 0)
                    isTemporaryCard = false;
                else
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "isTemporaryCard参数有误", "");

                startTimeStamp = dataJson.Value<string>("startTimeStamp");
                endTimeStamp = dataJson.Value<string>("endTimeStamp");

                var authorizeJsonList = dataJson.Value<JArray>("authorizeList");
                foreach (var authorizeJson in authorizeJsonList)
                {
                    M_AccessControllerAuthorization_Info info = new M_AccessControllerAuthorization_Info();
                    info.Sn = authorizeJson.Value<string>("sn");
                    //info.IpAddress=authorizeJson["ipAddress"].ToString();
                    byte authorizeDoor = authorizeJson.Value<byte>("authorizeDoorIndex");
                    if (authorizeDoor > 15)
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门点授权参数无效", "");

                    string strBinary = Convert.ToString(authorizeDoor, 2).PadLeft(4, '0');  //转二进制，目前只有2门，预留4门
                    if (strBinary.Substring(3, 1).Equals("1"))
                        info.AddDoors(1);

                    if (strBinary.Substring(2, 1).Equals("1"))
                        info.AddDoors(2);

                    if (!info.IsValidValue())
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门点授权参数无效", "");

                    authorizeControllerList.Add(info);
                }
                AccessType accessType = 0;
                try
                {
                    accessType = M_WG_Config.StrConvertToAccessType(strACType);
                    if ((int)accessType <= 0)
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁类型错误", "");
                }
                catch
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁类型错误", "");
                }

                if (string.IsNullOrEmpty(strACType) || string.IsNullOrEmpty(cardNum) || string.IsNullOrEmpty(startTimeStamp) || string.IsNullOrEmpty(endTimeStamp) || !authorizeControllerList.Any())
                {
                    return ADInterface.InvalidPostData("");
                }
                #endregion

                M_Card_Info cardInfo = new M_Card_Info();
                switch (accessType)
                {
                    case AccessType.TDZ:
                        {
                            cardInfo.CardId = cardNum;
                            if (isTemporaryCard.Value)
                            {
                                cardInfo.CardType = "第三方临时卡";
                            }
                            else if (!isTemporaryCard.Value)
                            {
                                cardInfo.CardType = "第三方常访卡";
                            }
                            cardInfo.StartDate = SysFunc.StampToDateTime(startTimeStamp);
                            cardInfo.EndDate = SysFunc.StampToDateTime(endTimeStamp);
                            if (cardInfo.StartDate >= cardInfo.EndDate)
                                return ApiTools.MsgFormat(ApiTools.ResponseCode.授权失败, "时间参数有误", "");

                            B_WG_Config bll_wg_config = new B_WG_Config();
                            List<string> grantDoorMsg = new List<string>();
                            foreach (var authorizeController in authorizeControllerList)
                            {
                                M_WG_Config controllerInfo = bll_wg_config.GetModelBySn(authorizeController.Sn, "TDZ");
                                if (controllerInfo == null)
                                    return ApiTools.MsgFormat(ApiTools.ResponseCode.授权失败, "SN码为：'" + authorizeController.Sn + "'的门禁控制器不存在", "");
                                else
                                {
                                    string msg = controllerInfo.Id + "_" + controllerInfo.Sn + ":" + authorizeController.AuthorizeDoors;
                                    grantDoorMsg.Add(msg);
                                }
                            }
                            cardInfo.GrantDoorMsg = string.Join("&", grantDoorMsg);
                            break;
                        }
                }
                M_Card_Info oldCardInfo = bll_card_info.GetModelByCardId(cardNum);
                bool isSucc = false;
                if (oldCardInfo != null)
                {
                    cardInfo.id = oldCardInfo.id;
                    isSucc = bll_card_info.Update(cardInfo);
                }
                else
                {
                    isSucc = bll_card_info.Add(cardInfo) > 0 ? true : false;
                }
                if (isSucc)
                {
                    var jsonObject = new
                    {
                        cardNum = cardNum,
                        accessQrMsg = SysFunc.CardNoEncryptByDES(cardNum)
                    };
                    var data = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject);
                    result = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "授权成功", data);
                }
                else
                    result = ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "授权失败", "");
            }
            catch (Exception ex)
            {
                WriteLog.Log4Local(DateTime.Now.ToString() + " " + ex.Message.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
            return result;
        }

        /// <summary>
        /// 删除卡权限
        /// </summary>
        /// <param name="dataJson"></param>
        /// <returns></returns>
        public static string DeleteAccessCard(JObject dataJson)
        {
            string result = string.Empty;
            string strACType = string.Empty;
            string cardNum = string.Empty;
            try
            {
                #region 参数校验

                strACType = dataJson.Value<string>("acType");
                cardNum = dataJson.Value<string>("cardNum");

                AccessType accessType = 0;
                try
                {
                    accessType = M_WG_Config.StrConvertToAccessType(strACType);
                    if ((int)accessType <= 0)
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁类型错误", "");
                }
                catch
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁类型错误", "");
                }

                if (string.IsNullOrEmpty(strACType) || string.IsNullOrEmpty(cardNum))
                {
                    return ADInterface.InvalidPostData("");
                }
                #endregion

                M_Card_Info cardInfo = bll_card_info.GetModelByCardId(cardNum);
                if (cardInfo == null)
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "卡号不存在", "");
                }
                switch (accessType)
                {
                    case AccessType.TDZ:
                        {
                            //无下发动作
                            break;
                        }
                }
                bool isDelSucc = bll_card_info.DeleteByCardNum(cardNum);
                if (isDelSucc)
                    result = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "删除成功", "");
                else
                    result = ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "删除失败", "");
             
            }
            catch (Exception ex)
            {
                WriteLog.Log4Local(DateTime.Now.ToString() + " " + ex.Message.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
            return result;
        }

        /// <summary>
        /// 查询门禁卡的授权信息
        /// </summary>
        /// <param name="dataJson"></param>
        /// <returns></returns>
        public static string InquireAccessCardAuthorization(JObject dataJson)
        {
            string result = string.Empty;
            string strACType = string.Empty;
            string cardNum = string.Empty;
            try
            {
                #region 参数校验
                strACType = dataJson.Value<string>("acType");
                cardNum = dataJson.Value<string>("cardNum");
                AccessType accessType = 0;
                try
                {
                    accessType = M_WG_Config.StrConvertToAccessType(strACType);
                    if ((int)accessType <= 0)
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁类型错误", "");
                }
                catch
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁类型错误", "");
                }

                if (string.IsNullOrEmpty(strACType) || string.IsNullOrEmpty(cardNum))
                {
                    return ADInterface.InvalidPostData("");
                }
                #endregion

                M_Card_Info cardInfo = bll_card_info.GetModelByCardId(cardNum);
                if (cardInfo == null)
                {
                    return ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "卡号不存在", "");
                }

                string data = string.Empty;
                switch (accessType)
                {
                    case AccessType.TDZ:
                        {
                            string authAccessMsg = cardInfo.GrantDoorMsg.ToString();
                            List<string> authAccessMsgList = new List<string>(authAccessMsg.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries));
                            var accessAuthParseList = authAccessMsgList.Select(x => new { Sn = x.Split('_')[1], DoorList = x.Split(':')[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries) }).ToList();

                            List<Object> authObjectList = new List<object>();
                            foreach (var info in accessAuthParseList)
                            {
                                int doorCount = 0;
                                foreach (var door in info.DoorList)
                                {
                                    doorCount += 2 * (int.Parse(door) - 1);
                                }
                                var authObject = new
                                {
                                    sn = info.Sn,
                                    doors = doorCount
                                };
                                authObjectList.Add(authObject);
                            }

                            bool isTemporaryCard = false;
                            if (cardInfo.CardType.Contains("临时卡"))
                                isTemporaryCard = true;

                            var jsonObject = new
                            {
                                cardNum = cardNum,
                                isTemporaryCard = isTemporaryCard,
                                startTimeStamp = SysFunc.DateTimeToStamp(cardInfo.StartDate.Value),
                                endTimeStamp = SysFunc.DateTimeToStamp(cardInfo.EndDate.Value),
                                authorizeList = authObjectList
                            };
                            data = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject);
                            break;
                        }
                }
                result = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "查询门禁卡权限成功", data);
            }
            catch (Exception ex)
            {
                WriteLog.Log4Local(DateTime.Now.ToString() + " " + ex.Message.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
            return result;
        }

        public static string InquireAceessRecord(JObject dataJson)
        {
            string result = string.Empty;

            string cardNum = string.Empty;
            string startTimeStamp = string.Empty;
            string endTimeStamp = string.Empty;
            int pageSize = 0;
            int pageIndex = 0;
            int inOrOutEvent = -1;
            try
            {
                #region 参数校验
                cardNum = dataJson.Value<string>("cardNum");
                startTimeStamp = dataJson.Value<string>("startTimeStamp");
                endTimeStamp = dataJson.Value<string>("endTimeStamp");
                pageSize = dataJson.Value<int>("rowsCount");
                pageIndex = dataJson.Value<int>("pageIndex");
                inOrOutEvent = dataJson.Value<int>("inOrOutEvent");
                var controllerJsonList = dataJson.Value<JArray>("controllerList");

                if (string.IsNullOrEmpty(cardNum) || pageSize < 1 || pageSize > 100 || pageIndex < 1 || inOrOutEvent < 0 || inOrOutEvent > 2 ||
                    string.IsNullOrEmpty(startTimeStamp) || string.IsNullOrEmpty(endTimeStamp))
                {
                    return ADInterface.InvalidPostData("");
                }

                List<string> controllerSNList = new List<string>();
                foreach (var controllerJson in controllerJsonList)
                {
                    string strSN = controllerJson.Value<string>("controllerSN");
                    if (string.IsNullOrEmpty(strSN) || SysFunc.IsDangerSqlString(strSN))
                    {
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁参数无效", "");
                    }
                    controllerSNList.Add(strSN);
                }
                #endregion

                #region 查询记录
                DateTime startDT = DateTime.Now;
                DateTime endDT = DateTime.Now;
                try
                {
                    startDT = SysFunc.StampToDateTime(startTimeStamp);
                    endDT = SysFunc.StampToDateTime(endTimeStamp);

                    if (startDT >= endDT)
                        return ApiTools.MsgFormat(ApiTools.ResponseCode.授权失败, "时间参数有误", "");
                }
                catch
                {
                    return ADInterface.InvalidPostData("");
                }
                StringBuilder sbWhere = new StringBuilder();
                //时间
                sbWhere.Append(" AND recordtime between '" + startDT.ToString() + "' and '" + endDT.ToString() + "' ");
                //卡号
                sbWhere.Append(" AND cardid='" + cardNum + "' ");
                //SN
                if (controllerSNList.Any())
                {
                    sbWhere.Append(" AND controllersn in ('" + string.Join("','", controllerSNList) + "') ");
                }

                switch (inOrOutEvent)
                {
                    case 0: //进
                        sbWhere.Append(" AND isentryevent=1 ");
                        break;
                    case 1: //出
                        sbWhere.Append(" AND isentryevent=0 ");
                        break;
                    case 2: //进和出
                        sbWhere.Append(" AND isentryevent in (0,1) ");
                        break;
                }

                B_WG_Record recordBll = new B_WG_Record();
                int pageCount = 0;
                List<M_WG_Record_Info> recordList = recordBll.GetModelList(sbWhere.ToString(), pageSize, pageIndex, out pageCount);
                var inquireResult = new
                {
                    pageSize = pageSize,
                    pageIndex = pageIndex,
                    pageCount = pageCount,
                    recordList = recordList
                };
                string inquireResultJson = Newtonsoft.Json.JsonConvert.SerializeObject(inquireResult);
                result = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "查询门禁卡权限成功", inquireResultJson);
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog.Log4Local(DateTime.Now.ToString() + " " + ex.Message.ToString(), true);
                return ApiTools.MsgFormat(ApiTools.ResponseCode.内部错误, "调用接口异常", "");
            }
            return result;
        }

        #endregion
    }
}
