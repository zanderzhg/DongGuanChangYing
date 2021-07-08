/*************************************************************************************
    * CLR版 本：    4.0.30319.42000
    * 类 名 称：    HikvisionFaceEventArgs
    * 说   明：     N/A
    * 作    者：    黄辉兴
    * 创建时间：    2019/1/14 17:31:54
    * 修改时间：    N/A
    * 修 改 人：    N/A
    * Copyright (C) 2019 德生科技
    * 德生科技版权所有
   *************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FKY_CMP.Code.SDK
{
    public class HikvisionFaceEventArgs:EventArgs
    {
         /// <summary>
        /// 提示信息
        /// </summary>
        public string msg;

        /// <summary>
        /// 状态封装类
        /// </summary>
        public HikvisionFaceState state;

        /// <summary>
        /// 是否已处理过
        /// </summary>
        public bool IsHandled { get; set; }

        public HikvisionFaceEventArgs(string msg)
        {
            this.msg = msg;
            IsHandled = false;
        }
        public HikvisionFaceEventArgs(HikvisionFaceState state)
        {
            this.state = state;
            IsHandled = false;
        }
        public HikvisionFaceEventArgs(string msg, HikvisionFaceState state)
        {
            this.msg = msg;
            this.state = state;
            IsHandled = false;
        }
    }
}
