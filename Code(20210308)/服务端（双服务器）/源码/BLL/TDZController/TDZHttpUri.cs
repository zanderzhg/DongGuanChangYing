using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.BLL.TDZController
{
    public class TDZHttpUri
    {
        private const string rootUri = "/tecsunapi/Visitor/TDZController/";
        public const string httpParamOpenDoor = rootUri + "openDoor";
        public const string httpParamDoorRecord = rootUri + "doorRecord";
        public const string httpParamHeartBeat = rootUri + "heartBeat";
        public const string httpParamAccountName = "UserName=Tecsun";
        public const string httpParamPassword = "PassWord=Tecsun002908";
    }
}
