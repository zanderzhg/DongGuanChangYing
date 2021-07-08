using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.SDK.Model
{
    public class HKAlarmRecordInfo
    {
        /// <summary>
        /// 警报命令
        /// </summary>
        public enum AlarmCommand
        {
            AcsAlarmFailed=0,   //失败
            AcsAlarmSucc=1,    //成功
            AcsAlarmOverTime=2,    //卡号过期
            FaceMatch = 3 //人脸比对
        }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNum { get; set; }
        /// <summary>
        /// 卡类型:1-职工,7-来宾卡
        /// </summary>
        public byte CardType { get; set; }
        /// <summary>
        /// 警报触发时间
        /// </summary>
        public DateTime AlarmTime { get; set; }
        /// <summary>
        /// 网络摄像头的IP地址
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// NVR的IP地址
        /// </summary>
        public string NVRIP { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 警报类型
        /// </summary>
        public AlarmCommand AlarmType { get; set; }
        /// <summary>
        /// PID
        /// </summary>
        public string PID { get; set; }
        /// <summary>
        /// 相似度
        /// </summary>
        public float Similarity { get; set; }
        /// <summary>
        /// 人员类型：visitor,emp
        /// </summary>
        public string PersonnelType { get; set; }
        /// <summary>
        /// 自定义ID
        /// </summary>
        public string CustomHumanID { get; set; }

        public byte[] CapturePicBytes { get; set; }
    }
}
