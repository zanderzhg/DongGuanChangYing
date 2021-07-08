using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using ADServer.DAL;
using System.Runtime.InteropServices;

namespace ADServer.BLL.TDZController
{
    public class TDZController
    {
        #region Fields
        UdpState udpState = new UdpState();
        public delegate void SetHttpParamDelegate(bool isSuccess, string msg, string ipaddress, string mac, int httpParamCount, string data);
        public SetHttpParamDelegate SetHttpParamHandler;
        public delegate void GetHttpParamDelegate(bool isSuccess, string msg, string ipaddress, string mac, int httpParamCount, bool isEnableDomain,string data);
        public GetHttpParamDelegate GetHttpParamHandler;
        public delegate void GetHttpEnableStatusDelegate(bool isSuccess, string msg, string ipaddress, string mac, bool enableOpenDoorUri, bool enableRecordUri, bool enableHeartbeatUri);
        public GetHttpEnableStatusDelegate GetHttpEnableStatusHandler;
        public delegate void SetHttpEnableStatusDelegate(bool isSuccess, string msg, string ipaddress, string mac);
        public SetHttpEnableStatusDelegate SetHttpEnableStatusHandler;
        public delegate void QrReceiveDelegate(bool isSuccess, string msg, string ipaddress, string mac);
        public QrReceiveDelegate QrReceiveHandler;
        public delegate void SetUploadICCardImageDelegate(bool isSuccess, string msg, string ipaddress, string mac);
        public SetUploadICCardImageDelegate SetUploadICCardImageHandler;

        IPEndPoint localIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
        public byte[] ReMACBuffer = new byte[6];
        private NET_CARD.NET_CARD_TIME m_struDeviceTime;
        public string deviceIP = string.Empty;
        public int devicePort =9998;

        private struct UdpState
        {
            public UdpClient udpClient;
            public IPEndPoint ipEndPoint;
            public int httpParamCount;
        }

        #endregion

        public TDZController(string deviceIP)
        {
            this.deviceIP = deviceIP;
        }

        public bool SetDoorDelay(byte delaySecond, out string errMsg)
        {
            errMsg = string.Empty;
            if (delaySecond>=20)
            {
                errMsg = "开门延迟不允许超过20秒";
                return false;
            }

            byte m_nLockDelay = delaySecond;

            NET_CARD.NET_CARD_OPEN_DELAY DoorDelay = new NET_CARD.NET_CARD_OPEN_DELAY();
            DoorDelay.Door01DelayTime = m_nLockDelay;//lock 1 delay
            DoorDelay.Door02DelayTime = m_nLockDelay;//lock 2 delay
            DoorDelay.Door03DelayTime = m_nLockDelay;//lock 3 delay
            DoorDelay.Door04DelayTime = m_nLockDelay;//lock 4 delay
            int nRet = NET_CARD.NET_CARD_SetOpenDelayTime(NET_CARD.DEVICE_NET_ACCESS, deviceIP, devicePort, ref DoorDelay, ref ReMACBuffer[0]);
            if (nRet == 0)
            {
                return true;
            }
            else
            {
                errMsg = nRet.ToString();
                return false ;
            }
        }

        public string ReadTime(out string errMsg)
        {
            errMsg = string.Empty;
            string DeviceTimeStr;
            Int32 nSize = Marshal.SizeOf(m_struDeviceTime);
            IntPtr ptrDeviceTime = Marshal.AllocHGlobal(nSize);
            Marshal.StructureToPtr(m_struDeviceTime, ptrDeviceTime, false);

            int nRet = NET_CARD.NET_CARD_DetectDeviceOnline(NET_CARD.DEVICE_NET_ACCESS, deviceIP, devicePort, ref ReMACBuffer[0], ptrDeviceTime);
            if (nRet == 0)
            {
                m_struDeviceTime = (NET_CARD.NET_CARD_TIME)Marshal.PtrToStructure(ptrDeviceTime, typeof(NET_CARD.NET_CARD_TIME));
                DeviceTimeStr = "20" + m_struDeviceTime.wYear.ToString("X2") + "-" + m_struDeviceTime.byMonth.ToString("X2") + "-" + m_struDeviceTime.byDay.ToString("X2") + " " + m_struDeviceTime.byHour.ToString("X2")
                                + ":" + m_struDeviceTime.byMinute.ToString("X2") + ":" + m_struDeviceTime.bySecond.ToString("X2");
                return DeviceTimeStr;
            }
            else
            {
                errMsg = nRet.ToString();
                return null;
            }
        }

        public bool SetTime(DateTime dt,out string errMsg)
        {
            errMsg = string.Empty;
            NET_CARD.NET_CARD_TIME struDeviceTime = new NET_CARD.NET_CARD_TIME();
            string strnew = dt.ToString("yyyy/MM/dd HH:mm:ss");

            string strTmp = strnew.Substring(2, 2);
            //byte kk = Convert.ToByte(strTmp, 16);
            struDeviceTime.wYear = Convert.ToByte(strTmp, 16);

            strTmp = strnew.Substring(5, 2);
            struDeviceTime.byMonth = Convert.ToByte(strTmp, 16);
            strTmp = strnew.Substring(8, 2);
            struDeviceTime.byDay = Convert.ToByte(strTmp, 16);
            strTmp = strnew.Substring(11, 2);
            struDeviceTime.byHour = Convert.ToByte(strTmp, 16);
            strTmp = strnew.Substring(14, 2);
            struDeviceTime.byMinute = Convert.ToByte(strTmp, 16);
            strTmp = strnew.Substring(17, 2);
            struDeviceTime.bySecond = Convert.ToByte(strTmp, 16);

            int nRet = NET_CARD.NET_CARD_SetDeviceTime(NET_CARD.DEVICE_NET_ACCESS, deviceIP, devicePort, ref  struDeviceTime, ref ReMACBuffer[0]);

            if (nRet == 0)
            {
                return true;
            }
            else
            {
                errMsg = nRet.ToString();
                return false;
            }
        }


        public string[] GetServerParam()
        {
            string m_sReturnError;
            byte[] RetChar = new byte[30];
            int nRetLen = 0;
            int nPos = 0;
            int nPort = 0;
            string[] strServerParamArray = new string[3];

            int nRet = NET_CARD.NET_CARD_Get_ALLServerIPandPort(NET_CARD.DEVICE_NET_ACCESS, deviceIP, devicePort, ref RetChar[0], ref nRetLen, ref ReMACBuffer[0]);
            if (nRetLen == 18)
            {
                nPort = 0x00;
                nPort |= RetChar[4];
                nPort <<= 8;
                nPort |= RetChar[5];
                nPos = 0;
                strServerParamArray[0] = RetChar[nPos].ToString("D") + "." + RetChar[nPos + 1].ToString("D") + "." + RetChar[nPos + 2].ToString("D") + "." + RetChar[nPos + 3].ToString("D") + ":" + nPort.ToString();
                nPos = 6;
                strServerParamArray[1] = RetChar[nPos].ToString("D") + "." + RetChar[nPos + 1].ToString("D") + "." + RetChar[nPos + 2].ToString("D") + "." + RetChar[nPos + 3].ToString("D") + ":" + nPort.ToString();
                nPos = 12;
                strServerParamArray[2] = RetChar[nPos].ToString("D") + "." + RetChar[nPos + 1].ToString("D") + "." + RetChar[nPos + 2].ToString("D") + "." + RetChar[nPos + 3].ToString("D") + ":" + nPort.ToString();
            }
            else
            {
                m_sReturnError = string.Format("{0:d}", nRet);
                strServerParamArray = new string[1] { m_sReturnError };
            }
            return strServerParamArray;
        }

        /// <summary>
        /// 设置UDP和TCP服务器
        /// </summary>
        /// <param name="setServerIP"></param>
        /// <param name="setServerPort"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool SetServerParam(string serverIP, int serverPort, out string errMsg)
        {
            errMsg = string.Empty;
            byte[] byteServerIp = new byte[4];

            string[] strIPv4 = new string[4];
            strIPv4 = serverIP.Split('.');
            if (strIPv4.Length != 4)
            {
                errMsg = "服务端IP地址有误";
                return false;
            }
            for (int kk = 0; kk < 4; kk++)
            {
                byteServerIp[kk] = Convert.ToByte(strIPv4[kk]);
            }
            int nRet = NET_CARD.NET_CARD_Set_RecordUpLoad_UDPIPAndPort(NET_CARD.DEVICE_NET_ACCESS, deviceIP, devicePort, byteServerIp, serverPort, ref ReMACBuffer[0]);
            if (nRet != 0)
            {
                errMsg = "设置UDP上传记录服务端失败，错误代码：" + nRet.ToString();
                return false;
            }
            nRet = NET_CARD.NET_CARD_Set_QR_ServerUDPIPAndPort(NET_CARD.DEVICE_NET_ACCESS, deviceIP, devicePort, byteServerIp, serverPort, ref ReMACBuffer[0]);
            if (nRet != 0)
            {
                errMsg = "设置UDP的QR服务端失败，错误代码：" + nRet.ToString();
                return false;
            }
            nRet = NET_CARD.NET_CARD_Set_QR_ServerTCPIPAndPort(NET_CARD.DEVICE_NET_ACCESS, deviceIP, devicePort, byteServerIp, serverPort, ref ReMACBuffer[0]);
            if (nRet != 0)
            {
                errMsg = "设置UDP的QR服务端失败，错误代码：" + nRet.ToString();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 开门延迟
        /// </summary>
        /// <param name="deviceIP"></param>
        /// <param name="devicePort"></param>
        /// <param name="delaySecond"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool SetDoorDelay( int delaySecond, out string errMsg)
        {
            errMsg = string.Empty;
            byte m_nLockDelay = 0;
            m_nLockDelay = (byte)delaySecond;

            NET_CARD.NET_CARD_OPEN_DELAY DoorDelay = new NET_CARD.NET_CARD_OPEN_DELAY();
            DoorDelay.Door01DelayTime = m_nLockDelay;//lock 1 delay
            DoorDelay.Door02DelayTime = m_nLockDelay;//lock 2 delay
            DoorDelay.Door03DelayTime = m_nLockDelay;//lock 3 delay
            DoorDelay.Door04DelayTime = m_nLockDelay;//lock 4 delay
            int nRet = NET_CARD.NET_CARD_SetOpenDelayTime(NET_CARD.DEVICE_NET_ACCESS, deviceIP, devicePort, ref DoorDelay, ref ReMACBuffer[0]);
            if (nRet == 0)
            {
                return true;
            }
            else
            {
                errMsg = nRet.ToString();
                return false;
            }
        }

        public void SetUploadICCardImage(byte uploadType)
        {
            udpState.udpClient = new UdpClient(localIPEndPoint);
            udpState.ipEndPoint = new IPEndPoint(IPAddress.Parse(deviceIP), devicePort);
            List<byte> sendByteList = new List<byte>() { 0x02, 0x00, 0x0C, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x11, 0x42, 0x0B, 0xFF, uploadType, 0xFF, 0x03 };
            byte[] sendBytes = sendByteList.ToArray();
            IAsyncResult ar = udpState.udpClient.BeginReceive(new AsyncCallback(SetUploadICCardImageCallBack), udpState);
            udpState.udpClient.BeginSend(sendBytes, sendBytes.Length, udpState.ipEndPoint, new AsyncCallback(SetHttpParamCallback), udpState);
            if (!ar.AsyncWaitHandle.WaitOne(1000))
            {
                if (SetUploadICCardImageHandler != null && !ar.IsCompleted)
                {
                    SetUploadICCardImageHandler(false, "网络连接超时", deviceIP, "");
                }
                udpState.udpClient.Close();
            }
        }

        private void SetUploadICCardImageCallBack(IAsyncResult ar)
        {
            UdpClient u = ((UdpState)(ar.AsyncState)).udpClient;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).ipEndPoint;

            bool isSuccess = false;
            string msg = string.Empty;
            string mac = string.Empty;
            try
            {
                byte[] receviceBytes = u.EndReceive(ar, ref e);

                byte[] startCodeByte = new byte[1];
                Array.Copy(receviceBytes, 0, startCodeByte, 0, 1);
                byte[] lengthByte = new byte[2];
                Array.Copy(receviceBytes, 1, lengthByte, 0, 2);
                byte[] macByte = new byte[6];
                Array.Copy(receviceBytes, 3, macByte, 0, 6);
                byte[] typeByte = new byte[1];
                Array.Copy(receviceBytes, 9, typeByte, 0, 1);
                byte[] commandByte = new byte[2];
                Array.Copy(receviceBytes, 10, commandByte, 0, 2);
                byte[] statusByte = new byte[1];
                Array.Copy(receviceBytes, 12, statusByte, 0, 1);
                byte[] checkByte = new byte[1];
                Array.Copy(receviceBytes, 13, checkByte, 0, 1);
                byte[] endCodeByte = new byte[1];
                Array.Copy(receviceBytes, 14, endCodeByte, 0, 1);

                if (startCodeByte[0] != 0x02)
                {
                    msg = "开始字节有误";
                }
                if (typeByte[0] != 0x11)
                {
                    msg = "非门禁控制器设备";
                    throw (new Exception());
                }
                if (statusByte[0] != 0x00)
                {
                    msg = "操作失败";
                    throw (new Exception());
                }
                if (!SysFunc.ByteArrayToHexStringNoSpace(commandByte).Equals("420B"))
                {
                    msg = "设置网络参数启用状态失败";
                    throw (new Exception());
                }
                mac = SysFunc.ByteArrayToHexStringNoSpace(macByte);
                isSuccess = true;
            }
            catch
            {
                isSuccess = false;
                if (msg.Equals(""))
                    msg = "解析异常";
            }
            if (SetUploadICCardImageHandler != null)
            {
                SetUploadICCardImageHandler(isSuccess, msg, e.Address.ToString(), mac);
            }
            u.Close();
            GC.Collect();

        }

        public void SetHttpEnableStatus(bool enableRecordUri, bool enableHeartbeatUri, bool enableOpenDoorUri)
        {
            udpState.udpClient = new UdpClient(localIPEndPoint);
            udpState.ipEndPoint = new IPEndPoint(IPAddress.Parse(deviceIP), devicePort);
            List<byte> sendByteList = new List<byte>() { 0x02, 0x00, 0x0C, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x11, 0x4A, 0x20, 0xFF,0x00, 0xFF, 0x03 };
            byte enableStatusByte=0x00;
            if (enableOpenDoorUri)
                enableStatusByte += 4;
            if (enableRecordUri)
                enableStatusByte += 1;
            if (enableHeartbeatUri)
                enableStatusByte += 2;
            sendByteList[13] = enableStatusByte;
            byte[] sendBytes = sendByteList.ToArray();
            IAsyncResult ar = udpState.udpClient.BeginReceive(new AsyncCallback(SetHttpEnableStatusCallback), udpState);
            udpState.udpClient.BeginSend(sendBytes, sendBytes.Length, udpState.ipEndPoint, new AsyncCallback(SetHttpParamCallback), udpState);
            if (!ar.AsyncWaitHandle.WaitOne(1000))
            {
                if (SetHttpEnableStatusHandler != null && !ar.IsCompleted)
                {
                    SetHttpEnableStatusHandler(false, "网络连接超时", deviceIP, "");
                }
                udpState.udpClient.Close();
            }
        }

        public void SetHttpEnableStatusCallback(IAsyncResult ar)
        {
            UdpClient u = ((UdpState)(ar.AsyncState)).udpClient;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).ipEndPoint;

            bool isSuccess = false;
            string strEnableBinary = string.Empty;
            string msg = string.Empty;
            string mac = string.Empty;
            try
            {
                byte[] receviceBytes = u.EndReceive(ar, ref e);

                byte[] startCodeByte = new byte[1];
                Array.Copy(receviceBytes, 0, startCodeByte, 0, 1);
                byte[] lengthByte = new byte[2];
                Array.Copy(receviceBytes, 1, lengthByte, 0, 2);
                byte[] macByte = new byte[6];
                Array.Copy(receviceBytes, 3, macByte, 0, 6);
                byte[] typeByte = new byte[1];
                Array.Copy(receviceBytes, 9, typeByte, 0, 1);
                byte[] commandByte = new byte[2];
                Array.Copy(receviceBytes, 10, commandByte, 0, 2);
                byte[] statusByte = new byte[1];
                Array.Copy(receviceBytes, 12, statusByte, 0, 1);
                byte[] checkByte = new byte[1];
                Array.Copy(receviceBytes, 13, checkByte, 0, 1);
                byte[] endCodeByte = new byte[1];
                Array.Copy(receviceBytes, 14, endCodeByte, 0, 1);

                if (startCodeByte[0] != 0x02)
                {
                    msg = "开始字节有误";
                }
                if (typeByte[0] != 0x11)
                {
                    msg = "非门禁控制器设备";
                    throw (new Exception());
                }
                if (statusByte[0] != 0x00)
                {
                    msg = "操作失败";
                    throw (new Exception());
                }
                if (!SysFunc.ByteArrayToHexStringNoSpace(commandByte).Equals("4A20"))
                {
                    msg = "设置网络参数启用状态失败";
                    throw (new Exception());
                }
                mac = SysFunc.ByteArrayToHexStringNoSpace(macByte);
                isSuccess = true;
            }
            catch
            {
                isSuccess = false;
                if (msg.Equals(""))
                    msg = "解析异常";
            }
            if (SetHttpEnableStatusHandler != null)
            {
                SetHttpEnableStatusHandler(isSuccess, msg, e.Address.ToString(), mac);
            }
            u.Close();
            GC.Collect();
        }


        public void GetHttpEnableStatus()
        {
            udpState.udpClient = new UdpClient(localIPEndPoint);
            udpState.ipEndPoint = new IPEndPoint(IPAddress.Parse(deviceIP), devicePort);
            List<byte> sendByteList = new List<byte>() { 0x02, 0x00, 0x0B, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x11, 0x4B, 0x20, 0xFF, 0xFF, 0x03 };
            byte[] sendBytes = sendByteList.ToArray();
            IAsyncResult ar = udpState.udpClient.BeginReceive(new AsyncCallback(GetHttpEnableStatusCallback), udpState);
            udpState.udpClient.BeginSend(sendBytes, sendBytes.Length, udpState.ipEndPoint, new AsyncCallback(SetHttpParamCallback), udpState);
            if (!ar.AsyncWaitHandle.WaitOne(500))
            {
                if (GetHttpEnableStatusHandler != null && !ar.IsCompleted)
                {
                    GetHttpEnableStatusHandler(false, "网络连接超时", deviceIP, "", false, false, false);
                }
                udpState.udpClient.Close();
            }
        }

        public void GetHttpEnableStatusCallback(IAsyncResult ar)
        {
            UdpClient u = ((UdpState)(ar.AsyncState)).udpClient;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).ipEndPoint;

            bool isSuccess = false;
            string strEnableBinary = string.Empty;
            string msg = string.Empty;
            string mac = string.Empty;
            bool enableParam1, enableParam2, enableParam3;
            enableParam1 = enableParam2 = enableParam3 = false;
            try
            {
                byte[] receviceBytes = u.EndReceive(ar, ref e);

                byte[] startCodeByte = new byte[1];
                Array.Copy(receviceBytes, 0, startCodeByte, 0, 1);
                byte[] lengthByte = new byte[2];
                Array.Copy(receviceBytes, 1, lengthByte, 0, 2);
                byte[] macByte = new byte[6];
                Array.Copy(receviceBytes, 3, macByte, 0, 6);
                byte[] typeByte = new byte[1];
                Array.Copy(receviceBytes, 9, typeByte, 0, 1);
                byte[] commandByte = new byte[2];
                Array.Copy(receviceBytes, 10, commandByte, 0, 2);
                byte[] statusByte = new byte[1];
                Array.Copy(receviceBytes, 12, statusByte, 0, 1);
                byte enableHttpParamByte = receviceBytes[13];
                byte[] checkByte = new byte[1];
                Array.Copy(receviceBytes, 14, checkByte, 0, 1);
                byte[] endCodeByte = new byte[1];
                Array.Copy(receviceBytes, 15, endCodeByte, 0, 1);

                if (startCodeByte[0] != 0x02)
                {
                    msg = "开始字节有误";
                }
                if (typeByte[0] != 0x11)
                {
                    msg = "非门禁控制器设备";
                    throw (new Exception());
                }
                if (statusByte[0] != 0x00)
                {
                    msg = "操作失败";
                    throw (new Exception());
                }
                if (!SysFunc.ByteArrayToHexStringNoSpace(commandByte).Equals("4B20"))
                {
                    msg = "读取网络参数失败";
                    throw (new Exception());
                }
                strEnableBinary = Convert.ToString(enableHttpParamByte, 2);
                strEnableBinary = strEnableBinary.PadLeft(4, '0');
                enableParam1 = strEnableBinary.Substring(1, 1) == "1" ? true : false;
                enableParam2 = strEnableBinary.Substring(2, 1) == "1" ? true : false;
                enableParam3 = strEnableBinary.Substring(3, 1) == "1" ? true : false;
                mac = SysFunc.ByteArrayToHexStringNoSpace(macByte);
                isSuccess = true;
            }
            catch
            {
                isSuccess = false;
                if (msg.Equals(""))
                    msg = "解析异常";
            }
            if (GetHttpEnableStatusHandler != null)
            {
                GetHttpEnableStatusHandler(isSuccess, msg, e.Address.ToString(), mac, enableParam1, enableParam2, enableParam3);
            }
            u.Close();
            GC.Collect();
        }

        /// <summary>
        /// 设置http参数
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="httpParamCount"></param>
        /// <param name="httpParamData"></param>
        /// <param name="port"></param>
        public void SetHttpParam(int httpParamCount, string httpParamData, bool isEnableDomainName=false)
        {
            udpState.udpClient = new UdpClient(localIPEndPoint);
            udpState.ipEndPoint = new IPEndPoint(IPAddress.Parse(deviceIP), devicePort);
            udpState.httpParamCount = httpParamCount;
            byte[] dataBytes = Encoding.ASCII.GetBytes(httpParamData);
            List<byte> sendByteList = new List<byte>() { 0x02, 0x00, 0x11, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x11, 0x45, 0x0D, 0xFF };
            sendByteList.Add(Convert.ToByte(httpParamCount));
            if (httpParamCount == 1)
            {
                if (!isEnableDomainName)
                {
                    sendByteList.Add(Convert.ToByte(dataBytes.Length + 1));
                    sendByteList.Add(0x00);
                }
                else
                {
                    sendByteList.Add(Convert.ToByte(dataBytes.Length + 1));
                    sendByteList.Add(0x01);
                }
            }
            else
                sendByteList.Add(Convert.ToByte(dataBytes.Length));
            sendByteList.AddRange(dataBytes);
            byte xorByte = XorCheck(sendByteList.ToArray());
            sendByteList.Add(0xff);
            sendByteList.Add(0x03);
            //设置长度
            byte length = (byte)(sendByteList.Count-4);
            string strLength = length.ToString("X4");
            byte[] lengthBytes=SysFunc.HexStringToByteArray(strLength);
            sendByteList[1] = lengthBytes[0];
            sendByteList[2] = lengthBytes[1];

            byte[] sendBytes = sendByteList.ToArray();
            IAsyncResult ar=udpState.udpClient.BeginReceive(new AsyncCallback(SetHttpParamReceiveCallback), udpState);
            udpState.udpClient.BeginSend(sendBytes, sendBytes.Length, udpState.ipEndPoint, new AsyncCallback(SetHttpParamCallback), udpState);
            if (!ar.AsyncWaitHandle.WaitOne(1000))
            {
                if (SetHttpParamHandler != null && !ar.IsCompleted)
                {
                    SetHttpParamHandler(false, "网络连接超时", deviceIP, "", httpParamCount, "");
                }
                udpState.udpClient.Close();
            }
        }

        /// <summary>
        /// 异或校验
        /// </summary>
        /// <param name="cmdBytes"></param>
        /// <returns></returns>
        private byte XorCheck(byte[] cmdBytes)
        {
            byte xorResult = cmdBytes[1];
            for (int i = 2; i < cmdBytes.Length; i++)
            {
                xorResult ^= cmdBytes[i];
            }
            return xorResult;
        }

        private void SetHttpParamCallback(IAsyncResult ar)
        {
            UdpClient u = ((UdpState)(ar.AsyncState)).udpClient;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).ipEndPoint;
            int isda=u.EndSend(ar);
        }

        private void SetHttpParamReceiveCallback(IAsyncResult ar)
        {
            bool isSuccess = false;
            string msg = string.Empty;
            string data = string.Empty;
            string mac = string.Empty;
            UdpClient u = ((UdpState)(ar.AsyncState)).udpClient;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).ipEndPoint;
            int httpParamCount = ((UdpState)(ar.AsyncState)).httpParamCount;
            try
            {
                byte[] receviceBytes = u.EndReceive(ar, ref e);

                byte[] startCodeByte = new byte[1];
                Array.Copy(receviceBytes, 0, startCodeByte, 0, 1);
                byte[] lengthByte = new byte[2];
                Array.Copy(receviceBytes, 1, lengthByte, 0, 2);
                byte[] macByte = new byte[6];
                Array.Copy(receviceBytes, 3, macByte, 0, 6);
                byte[] typeByte = new byte[1];
                Array.Copy(receviceBytes, 9, typeByte, 0, 1);
                byte[] commandByte = new byte[2];
                Array.Copy(receviceBytes, 10, commandByte, 0, 2);
                byte[] statusByte = new byte[1];
                Array.Copy(receviceBytes, 12, statusByte, 0, 1);
                byte[] checkByte = new byte[1];
                Array.Copy(receviceBytes, 13, checkByte, 0, 1);
                byte[] endCodeByte = new byte[1];
                Array.Copy(receviceBytes, 14, endCodeByte, 0, 1);

                if (startCodeByte[0] != 0x02)
                {
                    msg = "开始字节有误";
                }
                if (typeByte[0] != 0x11)
                {
                    msg = "非门禁控制器设备";
                    throw (new Exception());
                }
                if (statusByte[0] != 0x00)
                {
                    msg = "操作失败";
                    throw (new Exception());
                }
                switch (SysFunc.ByteArrayToHexStringNoSpace(commandByte))
                {
                    case "450D":
                        {
                            msg = "设置网络参数成功";
                            
                            break;
                        }
                    case "450E":
                        {
                            msg = "读取网络参数成功";
                            break;
                        }
                }
                isSuccess = true;
                mac = SysFunc.ByteArrayToHexStringNoSpace(macByte);
            }
            catch
            {
                isSuccess = false;
                if (msg.Equals(""))
                    msg = "解析异常";
            }
            if (SetHttpParamHandler != null)
            {
                SetHttpParamHandler(isSuccess, msg, e.Address.ToString(), mac, httpParamCount, data);
            }
            u.Close();
            GC.Collect();
        }

        public void GetHttpParam(int httpParamCount, bool isFirstHttpParam = false)
        {
            udpState = new UdpState();
            udpState.udpClient = new UdpClient(localIPEndPoint);
            udpState.ipEndPoint = new IPEndPoint(IPAddress.Parse(deviceIP), devicePort);
            udpState.httpParamCount = httpParamCount;
            List<byte> sendByteList = new List<byte>() { 0x02, 0x00, 0x0C, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x11, 0x45, 0x0E, 0xFF };
            sendByteList.Add(Convert.ToByte(httpParamCount));
            byte xorByte = XorCheck(sendByteList.ToArray());
            sendByteList.Add(0xff);
            sendByteList.Add(0x03);
            //设置长度
            byte length = (byte)(sendByteList.Count - 4);
            string strLength = length.ToString("X4");
            byte[] lengthBytes = SysFunc.HexStringToByteArray(strLength);
            sendByteList[1] = lengthBytes[0];
            sendByteList[2] = lengthBytes[1];

            byte[] sendBytes = sendByteList.ToArray();
            IAsyncResult ar = udpState.udpClient.BeginReceive(new AsyncCallback(GetHttpParamReceiveCallback), udpState);
            udpState.udpClient.BeginSend(sendBytes, sendBytes.Length, udpState.ipEndPoint, new AsyncCallback(SetHttpParamCallback), udpState);
            if (!ar.AsyncWaitHandle.WaitOne(300))
            {
                if (GetHttpParamHandler != null && !ar.IsCompleted)
                {

                    GetHttpParamHandler(false, "网络连接超时", deviceIP, "", httpParamCount, false, "");
                }
                udpState.udpClient.Close();
            }
        }

        private void GetHttpParamReceiveCallback(IAsyncResult ar)
        {
            bool isSuccess = false;
            string msg = string.Empty;
            string strHttpParam = string.Empty;
            bool isEnableDomainName = false;
            string mac = string.Empty;
            UdpClient u = ((UdpState)(ar.AsyncState)).udpClient;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).ipEndPoint;
            int httpParamCount = ((UdpState)(ar.AsyncState)).httpParamCount;
            try
            {
                byte[] receviceBytes = u.EndReceive(ar, ref e);

                byte[] startCodeByte = new byte[1];
                Array.Copy(receviceBytes, 0, startCodeByte, 0, 1);
                byte[] lengthByte = new byte[2];
                Array.Copy(receviceBytes, 1, lengthByte, 0, 2);
                byte[] macByte = new byte[6];
                Array.Copy(receviceBytes, 3, macByte, 0, 6);
                byte[] typeByte = new byte[1];
                Array.Copy(receviceBytes, 9, typeByte, 0, 1);
                byte[] commandByte = new byte[2];
                Array.Copy(receviceBytes, 10, commandByte, 0, 2);
                byte[] statusByte = new byte[1];
                Array.Copy(receviceBytes, 12, statusByte, 0, 1);
                int dataBytesLength = receviceBytes.Length - 13-2;
                byte[] dataByte = new byte[dataBytesLength];
                Array.Copy(receviceBytes, 13 ,dataByte, 0, dataBytesLength);
                byte[] checkByte = new byte[1];
                Array.Copy(receviceBytes, 13 + dataBytesLength, checkByte, 0, 1);
                byte[] endCodeByte = new byte[1];
                Array.Copy(receviceBytes, 14 + dataBytesLength, endCodeByte, 0, 1);

                if (startCodeByte[0] != 0x02)
                {
                    msg = "开始字节有误";
                }
                if (typeByte[0] != 0x11)
                {
                    msg = "非门禁控制器设备";
                    throw (new Exception());
                }
                if (statusByte[0] != 0x00)
                {
                    msg = "操作失败";
                    throw (new Exception());
                }
                switch (SysFunc.ByteArrayToHexStringNoSpace(commandByte))
                {
                    case "450D":
                        {
                            msg = "设置网络参数成功";

                            break;
                        }
                    case "450E":
                        {
                            msg = "读取网络参数成功";
                            if (httpParamCount==1)
                            {
                                int length = dataByte[0]-1;
                                if (dataByte[1] == 0x00)
                                    isEnableDomainName = false;
                                else
                                    isEnableDomainName = true;
                                byte[] urlParamBytes = new byte[length];
                                Array.Copy(dataByte, 2, urlParamBytes, 0, length);
                                strHttpParam = Encoding.ASCII.GetString(urlParamBytes);
                            }
                            else
                            {
                                int length = dataByte[0];
                                byte[] urlParamBytes = new byte[length];
                                Array.Copy(dataByte, 1, urlParamBytes, 0, length);
                                strHttpParam = Encoding.ASCII.GetString(urlParamBytes);
                            }
                            break;
                        }
                }
                isSuccess = true;
                mac = SysFunc.ByteArrayToHexStringNoSpace(macByte);
            }
            catch
            {
                isSuccess = false;
                if (msg.Equals(""))
                    msg = "解析异常";
            }
            if (GetHttpParamHandler != null)
            {
                GetHttpParamHandler(isSuccess, msg, e.Address.ToString(), mac, httpParamCount,isEnableDomainName, strHttpParam);
            }
            u.Close();
            GC.Collect();
        }

        #region UDP通讯二维码解析，未完成。建议使用TCP通讯接口
        //public void StartQrReceive(string localServerIP,int localPort)
        //{
        //    udpState = new UdpState();
        //    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(localServerIP), localPort);
        //    udpState.udpClient = new UdpClient(ipEndPoint);
        //    udpState.udpClient.BeginReceive(new AsyncCallback(QrReceiveCallBack), udpState);
        //}

        //private void QrReceiveCallBack(IAsyncResult ar)
        //{
        //    UdpClient u = ((UdpState)(ar.AsyncState)).udpClient;
        //    IPEndPoint e = ((UdpState)(ar.AsyncState)).ipEndPoint;

        //    bool isSuccess = false;
        //    string msg = string.Empty;
        //    string mac = string.Empty;
        //    byte doorNum = 0;
        //    try
        //    {
        //        byte[] receviceBytes = u.EndReceive(ar, ref e);

        //        byte[] startCodeByte = new byte[1];
        //        Array.Copy(receviceBytes, 0, startCodeByte, 0, 1);
        //        byte[] lengthByte = new byte[2];
        //        Array.Copy(receviceBytes, 1, lengthByte, 0, 2);
        //        byte[] macByte = new byte[6];
        //        Array.Copy(receviceBytes, 3, macByte, 0, 6);
        //        byte[] typeByte = new byte[1];
        //        Array.Copy(receviceBytes, 9, typeByte, 0, 1);
        //        byte[] commandByte = new byte[2];
        //        Array.Copy(receviceBytes, 10, commandByte, 0, 2);
        //        byte[] statusByte = new byte[1];
        //        Array.Copy(receviceBytes, 12, statusByte, 0, 1);
        //        byte doorNumByte = receviceBytes[13];

        //        int cardNoBytesLength = receviceBytes.Length - 14 - 2;
        //        byte[] cardNoBytes = new byte[cardNoBytesLength];
        //        Array.Copy(receviceBytes, 14, cardNoBytes, 0, cardNoBytesLength);

        //        byte[] checkByte = new byte[1];
        //        Array.Copy(receviceBytes, 14 + cardNoBytesLength, checkByte, 0, 1);
        //        byte[] endCodeByte = new byte[1];
        //        Array.Copy(receviceBytes, 15 + cardNoBytesLength, endCodeByte, 0, 1);

        //        if (startCodeByte[0] != 0x02)
        //        {
        //            msg = "开始字节有误";
        //        }
        //        if (typeByte[0] != 0x11)
        //        {
        //            msg = "非门禁控制器设备";
        //            throw (new Exception());
        //        }
        //        if (statusByte[0] != 0x00)
        //        {
        //            msg = "操作失败";
        //            throw (new Exception());
        //        }
        //        switch (SysFunc.ByteArrayToHexStringNoSpace(commandByte))
        //        {
        //            case "5506":
        //                {
        //                    msg = "二维码刷卡事件";
        //                    break;
        //                }
        //        }
        //        isSuccess = true;
        //        mac = SysFunc.ByteArrayToHexStringNoSpace(macByte);
        //    }
        //    catch
        //    {
        //        isSuccess = false;
        //        msg = "解析异常";
        //    }
        //}
        #endregion
    }
}
