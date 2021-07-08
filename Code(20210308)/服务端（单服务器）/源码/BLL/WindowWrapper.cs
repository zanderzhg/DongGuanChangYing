using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.BLL
{
    /// <summary>
    ///  子线程中的MessageBox显示在主线程之上（模态）
    /// </summary>
    public class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        private System.IntPtr _hwnd;
        public WindowWrapper(System.IntPtr handle)
        {
            _hwnd = handle;
        }
        public System.IntPtr Handle
        {
            get { return _hwnd; }
        }


    }
}
