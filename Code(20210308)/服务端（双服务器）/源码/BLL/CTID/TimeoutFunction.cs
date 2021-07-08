using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.BLL.CTID
{
    public class TimeoutFunction
    {
        public static Boolean Execute(Func<byte[], byte[], double> timeoutMethod, byte[] nowPhotobyte, byte[] matchPhotobyte, out double result, TimeSpan timeout)
        {
            var asyncResult = timeoutMethod.BeginInvoke(nowPhotobyte, matchPhotobyte, null, null);
            if (!asyncResult.AsyncWaitHandle.WaitOne(timeout, false))
            {
                result = -1;
                return false;
            }
            result = timeoutMethod.EndInvoke(asyncResult);
            return true;
        }
    }
}
