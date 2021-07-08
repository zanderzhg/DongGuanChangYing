using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public class M_WG_Config
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string machinecode;
        /// <summary>
        /// 关联的机器码配置
        /// </summary>
        public string Machinecode
        {
            get { return machinecode; }
            set { machinecode = value; }
        }

        private string sn;
        /// <summary>
        /// 控制器SN码
        /// </summary>
        public string Sn
        {
            get { return sn; }
            set { sn = value; }
        }

        private string ipAddress;
        /// <summary>
        /// 控制器IP地址
        /// </summary>
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        private string port;
        /// <summary>
        /// 控制器端口号
        /// </summary>
        public string Port
        {
            get { return port; }
            set { port = value; }
        }

        private string wgDoors = "";
        /// <summary>
        /// 门点号集合
        /// </summary>
        public string WGDoors
        {
            get { return wgDoors; }
            set { wgDoors = value; }
        }

        private string wgDoorNames = "";
        /// <summary>
        /// 门点名称集合
        /// </summary>
        public string WGDoorNames
        {
            get { return wgDoorNames; }
            set { wgDoorNames = value; }
        }

        private string wgCheckInOut;
        /// <summary>
        /// 登入点、签离点
        /// </summary>
        public string WGCheckInOut
        {
            get { return wgCheckInOut; }
            set { wgCheckInOut = value; }
        }

        private int passagewayId;
        /// <summary>
        /// 通道名称ID
        /// </summary>
        public int PassagewayId
        {
            get { return passagewayId; }
            set { passagewayId = value; }
        }

        private string passageway;
        /// <summary>
        /// 通道名称
        /// </summary>
        public string Passageway
        {
            get { return passageway; }
            set { passageway = value; }
        }

        private string manufactor;
        /// <summary>
        /// 门禁厂家，WG：微耕，SJ：盛炬门禁，SJ-Elevator：盛炬梯控
        /// </summary>
        public string Manufactor
        {
            get { return manufactor; }
            set { manufactor = value; }
        }

        public static AccessType StrConvertToAccessType(string strTypeParam)
        {
            AccessType accessType = AccessType.Unknow;
            try
            {
                string strFront = strTypeParam.Split('-')[0];
                string strACType = strTypeParam.Split('-')[1];
                if (!strFront.Equals("TSV"))
                {
                    return AccessType.Unknow;
                }
                accessType = (AccessType)Enum.Parse(typeof(AccessType), strACType);
            }
            catch
            {
                accessType = AccessType.Unknow;
            }
            return accessType;
        }
    }

    public class M_WG_Time
    {
        public M_WG_Time()
        { }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string sn;
        /// <summary>
        /// 门禁控制板SN码
        /// </summary>
        public string Sn
        {
            get { return sn; }
            set { sn = value; }
        }
        private string opendate;
        /// <summary>
        /// 开放时间
        /// </summary>
        public string Opendate
        {
            get { return opendate; }
            set { opendate = value; }
        }
        private DateTime timezone1from;
        /// <summary>
        /// 时区1开始时间
        /// </summary>
        public DateTime TimeZone1From
        {
            get { return timezone1from; }
            set { timezone1from = value; }
        }
        private DateTime timezone1to;
        /// <summary>
        /// 时区1结束时间
        /// </summary>
        public DateTime TimeZone1To
        {
            get { return timezone1to; }
            set { timezone1to = value; }
        }
        private DateTime timezone2from;
        /// <summary>
        /// 时区2开始时间
        /// </summary>
        public DateTime TimeZone2From
        {
            get { return timezone2from; }
            set { timezone2from = value; }
        }
        private DateTime timezone2to;
        /// <summary>
        /// 时区2结束时间
        /// </summary>
        public DateTime TimeZone2To
        {
            get { return timezone2to; }
            set { timezone2to = value; }
        }
        private DateTime timezone3from;
        /// <summary>
        /// 时区3开始时间
        /// </summary>
        public DateTime TimeZone3From
        {
            get { return timezone3from; }
            set { timezone3from = value; }
        }
        private DateTime timezone3to;
        /// <summary>
        /// 时区3结束时间
        /// </summary>
        public DateTime TimeZone3To
        {
            get { return timezone3to; }
            set { timezone3to = value; }
        }

    }

    public class M_PassageWay
    {
        public M_PassageWay()
        { }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name;
        /// <summary>
        /// 通道名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int actype;
        /// <summary>
        /// 门禁类型：1:微耕
        /// </summary>
        public int AcType
        {
            get { return actype; }
            set { actype = value; }
        }
    }

    public class M_BuildingPermission
    {
        public M_BuildingPermission()
        { }

        public override string ToString()
        {
            return name;
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string groupname;
        /// <summary>
        /// 权限组名称
        /// </summary>
        public string GroupName
        {
            get { return groupname; }
            set { groupname = value; }
        }

        private string name;
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string buildingName;
        /// <summary>
        /// 楼宇名称
        /// </summary>
        public string BuildingName
        {
            get { return buildingName; }
            set { buildingName = value; }
        }

        private string deviceid;
        /// <summary>
        /// 控制器设备id
        /// </summary>
        public string DeviceId
        {
            get { return deviceid; }
            set { deviceid = value; }
        }

        private string floors;
        /// <summary>
        /// 激活楼层号，例子格式：1,2,3,10
        /// </summary>
        public string Floors
        {
            get { return floors; }
            set { floors = value; }
        }

        private string floorRange;
        /// <summary>
        /// 楼层段
        /// </summary>
        public string FloorRange
        {
            get { return floorRange; }
            set { floorRange = value; }
        }

        public List<int> FoorArr
        {
            get
            {
                List<int> floorsList = new List<int>();
                if (floors != null && floors != "")
                {
                    string[] arr = floors.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        int foorNum = int.Parse(arr[i]);
                        floorsList.Add(foorNum);
                    }
                }

                return floorsList;
            }
        }

      
    }

    public enum AccessType
    {
        Unknow = 0,
        WG = 1,
        SJ = 2,
        TDZ = 3
    }

}
