using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    /// <summary>
    /// N1系列Model
    /// </summary>
    public class M_FaceGateDevice_Info
    {
        public int DeviceID { get; set; }
        public int PassagewayID { get; set; }
        public string PassagewayName { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public string DeviceIP { get; set; }
        public string DevicePort { get; set; }
        public string DeviceSN { get; set; }
        public string DeviceMAC { get; set; }
        public int EntryType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
