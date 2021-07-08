using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.BLL
{
    class ADSHelp
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern System.IntPtr GetForegroundWindow();//子线程MessageBox显示在主线程
        public static string Int2IP(UInt32 ipCode)
        {
            byte a = (byte)((ipCode & 0xFF000000) >> 24);
            byte b = (byte)((ipCode & 0x00FF0000) >> 16);
            byte c = (byte)((ipCode & 0x0000FF00) >> 8);
            byte d = (byte)(ipCode & 0x000000FF);
            string ipStr = String.Format("{0}.{1}.{2}.{3}", a, b, c, d);

            return ipStr;
        }

        public static UInt32 IP2Int(string ipStr)
        {
            string[] ip = ipStr.Split('.');
            uint ipCode = 0xFFFFFF00 | byte.Parse(ip[3]);
            ipCode = ipCode & 0xFFFF00FF | (uint.Parse(ip[2]) << 8);
            ipCode = ipCode & 0xFF00FFFF | (uint.Parse(ip[1]) << 16);
            ipCode = ipCode & 0x00FFFFFF | (uint.Parse(ip[0]) << 24);
            return ipCode;
        }

        static void BeepOK()
        {
            // 成功，短鸣一声
            Console.Beep(1500, 200);
        }

        static void BeepError()
        {
            // 测试失败，短鸣三声
            Console.Beep(2000, 50);
            System.Threading.Thread.Sleep(150);
            Console.Beep(2000, 50);
            System.Threading.Thread.Sleep(150);
            Console.Beep(2000, 50);
        }


        public static void PromptResult(int result, bool isPromptOnlyError)
        {
            System.IntPtr IntPart = GetForegroundWindow();
            WindowWrapper ParentFrm = new WindowWrapper(IntPart);

            if (result == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
            {
                BeepOK();

                if (!isPromptOnlyError)
                {
                    System.Windows.Forms.MessageBox.Show(ParentFrm, "操作成功");
                }
            }
            else
            {
                if (result == 14 || result == 15)
                {
                    return;
                }
                BeepError();
                System.Windows.Forms.MessageBox.Show(ParentFrm, "门禁操作失败：" + ADSHalAPI.ADS_Helper_GetErrorMessage((uint)result));
            }
        }

        public static string GetEventName(ADSHalConstant.ADS_EventType et)
        {
            switch (et)
            {
                case ADSHalConstant.ADS_EventType.ADS_ET_OUT_CARD: return "外部刷卡";
                case ADSHalConstant.ADS_EventType.ADS_ET_IN_CARD: return "内部刷卡";
                case ADSHalConstant.ADS_EventType.ADS_ET_OUT_CARD_OPEN: return "外部刷卡开门";
                case ADSHalConstant.ADS_EventType.ADS_ET_IN_CARD_OPEN: return "内部刷卡开门";
                case ADSHalConstant.ADS_EventType.ADS_ET_OUT_PASSWORD_OPEN: return "外部超级密码开门";
                case ADSHalConstant.ADS_EventType.ADS_ET_IN_PASSWORD_OPEN: return "内部超级密码开门";
                case ADSHalConstant.ADS_EventType.ADS_ET_BUTTON_OPEN: return "内部按钮开门";
                case ADSHalConstant.ADS_EventType.ADS_ET_ARM: return "布防";
                case ADSHalConstant.ADS_EventType.ADS_ET_DISARM: return "撤防";
                case ADSHalConstant.ADS_EventType.ADS_ET_INPUT_PASSWORD: return "输入密码";
                case ADSHalConstant.ADS_EventType.ADS_ET_SOFTWARE_OPEN: return "管理软件开门";
                case ADSHalConstant.ADS_EventType.ADS_ET_SOFTWARE_CLOSE: return "管理软件关门";
                case ADSHalConstant.ADS_EventType.ADS_ET_OUT_FORCE_OPEN: return "外部胁迫开门";
                case ADSHalConstant.ADS_EventType.ADS_ET_IN_FORCE_OPEN: return "内部胁迫开门";
                case ADSHalConstant.ADS_EventType.ADS_ET_OUT_INVALID_CARD: return "外部无效卡";
                case ADSHalConstant.ADS_EventType.ADS_ET_IN_INVALID_CARD: return "内部无效卡";
                case ADSHalConstant.ADS_EventType.ADS_ET_PASSWORD_ERROR: return "密码错误";
                case ADSHalConstant.ADS_EventType.ADS_ET_ILLEGAL_OPEN: return "非法开门";
                case ADSHalConstant.ADS_EventType.ADS_ET_OPEN_TIMEOUT: return "门开超时报警";
                case ADSHalConstant.ADS_EventType.ADS_ET_CTRL_STARTUP: return "控制器启动";
                case ADSHalConstant.ADS_EventType.ADS_ET_CTRL_BOX_OPEN: return "控制器箱被打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_CTRL_BOX_CLOSE: return "控制器箱被合上";
                case ADSHalConstant.ADS_EventType.ADS_ET_DEVICE_ONLINE: return "设备上线";
                case ADSHalConstant.ADS_EventType.ADS_ET_DEVICE_OFFLINE: return "设备离线";
                case ADSHalConstant.ADS_EventType.ADS_ET_READER_DISMANTLE: return "读卡器被拆除";
                case ADSHalConstant.ADS_EventType.ADS_ET_485_LOOP1_CONNECT: return "RS-485环路1接通";
                case ADSHalConstant.ADS_EventType.ADS_ET_485_LOOP1_DISCONNECT: return "RS-485环路1断开";
                case ADSHalConstant.ADS_EventType.ADS_ET_485_LOOP2_CONNECT: return "RS-485环路2接通";
                case ADSHalConstant.ADS_EventType.ADS_ET_485_LOOP2_DISCONNECT: return "RS-485环路2断开";
                case ADSHalConstant.ADS_EventType.ADS_ET_CTRL_CONNECT: return "建立连接";
                case ADSHalConstant.ADS_EventType.ADS_ET_CTRL_DISCONNECT: return "断开连接";
                case ADSHalConstant.ADS_EventType.ADS_ET_ENTER_CARD: return "进入刷卡开门";
                case ADSHalConstant.ADS_EventType.ADS_ET_ENTER_CARDnPWD: return "进入卡+密码";
                case ADSHalConstant.ADS_EventType.ADS_ET_ENTER_CARDOrPWD: return "进入卡或密码";
                case ADSHalConstant.ADS_EventType.ADS_ET_ENTER_CONST_OPEN: return "进入常开";
                case ADSHalConstant.ADS_EventType.ADS_ET_ENTER_CONST_CLOSE: return "进入常闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_ENTER_CARD_DATA: return "进入卡片数据";
                case ADSHalConstant.ADS_EventType.ADS_ET_DOOR_ON: return "门磁端口打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_DOOR_OFF: return "门磁端口关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_BUTTON_ON: return "按钮端口打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_BUTTON_OFF: return "按钮端口关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN1_ON: return "辅助输入1打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN1_OFF: return "辅助输入1关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN2_ON: return "辅助输入2打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN2_OFF: return "辅助输入2关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN3_ON: return "辅助输入3打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN3_OFF: return "辅助输入3关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN4_ON: return "辅助输入4打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN4_OFF: return "辅助输入4关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN5_ON: return "辅助输入5打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN5_OFF: return "辅助输入5关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN6_ON: return "辅助输入6打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN6_OFF: return "辅助输入6关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN7_ON: return "辅助输入7打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN7_OFF: return "辅助输入7关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN8_ON: return "辅助输入8打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_IN8_OFF: return "辅助输入8关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_DOOR_SHORT_CIRCUIT: return "门磁端口短路";
                case ADSHalConstant.ADS_EventType.ADS_ET_DOOR_OPEN_CIRCUIT: return "门磁端口开路";
                case ADSHalConstant.ADS_EventType.ADS_ET_DOOR_OVERFLOW: return "门磁端口上溢报警";
                case ADSHalConstant.ADS_EventType.ADS_ET_DOOR_UNDERFLOW: return "门磁端口下溢报警";
                case ADSHalConstant.ADS_EventType.ADS_ET_BUTTON_SHORT_CIRCUIT: return "按钮端口短路";
                case ADSHalConstant.ADS_EventType.ADS_ET_BUTTON_OPEN_CIRCUIT: return "按钮端口开路";
                case ADSHalConstant.ADS_EventType.ADS_ET_BUTTON_OVERFLOW: return "按钮端口上溢报警";
                case ADSHalConstant.ADS_EventType.ADS_ET_BUTTON_UNDERFLOW: return "按钮端口下溢报警";
                case ADSHalConstant.ADS_EventType.ADS_ET_LOCK_ON: return "电锁打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_LOCK_OFF: return "电锁关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT1_ON: return "辅助输出1打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT1_OFF: return "辅助输出1关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT2_ON: return "辅助输出2打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT2_OFF: return "辅助输出2关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT3_ON: return "辅助输出3打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT3_OFF: return "辅助输出3关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT4_ON: return "辅助输出4打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT4_OFF: return "辅助输出4关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT5_ON: return "辅助输出5打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT5_OFF: return "辅助输出5关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT6_ON: return "辅助输出6打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT6_OFF: return "辅助输出6关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT7_ON: return "辅助输出7打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT7_OFF: return "辅助输出7关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT8_ON: return "辅助输出8打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT8_OFF: return "辅助输出8关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT9_ON: return "辅助输出9打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT9_OFF: return "辅助输出9关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT10_ON: return "辅助输出10打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT10_OFF: return "辅助输出10关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT11_ON: return "辅助输出11打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT11_OFF: return "辅助输出11关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT12_ON: return "辅助输出12打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT12_OFF: return "辅助输出12关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT13_ON: return "辅助输出13打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT13_OFF: return "辅助输出13关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT14_ON: return "辅助输出14打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT14_OFF: return "辅助输出14关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT15_ON: return "辅助输出15打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT15_OFF: return "辅助输出15关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT16_ON: return "辅助输出16打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT16_OFF: return "辅助输出16关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT17_ON: return "辅助输出17打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT17_OFF: return "辅助输出17关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT18_ON: return "辅助输出18打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT18_OFF: return "辅助输出18关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT19_ON: return "辅助输出19打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT19_OFF: return "辅助输出19关闭";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT20_ON: return "辅助输出20打开";
                case ADSHalConstant.ADS_EventType.ADS_ET_AUX_OUT20_OFF: return "辅助输出20关闭";
            }

            return "";
        }
    }
}
