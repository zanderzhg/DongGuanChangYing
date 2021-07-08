using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ADServer.BLL
{
    public class IdUnPack
    {
        [System.Runtime.InteropServices.DllImportAttribute("\\TDZ_DLL\\UnPack.dll")]
        public static extern int Unpack(byte[] wlt, byte[] bmp);
        //public static extern int Unpack(byte[] wlt, StringBuilder bmp);
    }
}
