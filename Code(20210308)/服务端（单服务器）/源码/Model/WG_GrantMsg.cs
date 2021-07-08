using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public class WG_GrantMsg
    {
        /// <summary>
        /// 控制器SN码
        /// </summary>
        public int Sn;
        /// <summary>
        /// 控制器IP地址
        /// </summary>
        public string IpAddress;
        /// <summary>
        /// 控制器端口号
        /// </summary>
        public int Port;

        public string CardID;
        public string Password;
        public DateTime DtStart;
        public DateTime DtEnd;
        public List<byte> DoorIdList = new List<byte>();
        /// <summary>
        /// 卡类型
        /// </summary>
        public string CardType;
        /// <summary>
        /// 梯控授权代码
        /// </summary>
        public int ElevatorCode;
    }
}
