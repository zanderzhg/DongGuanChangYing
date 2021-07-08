using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADServer.DAL;

namespace ADServer.BLL.TDZController
{
    public class TDZHelper
    {
        #region Fields
        private static string openDoorDuration = "0000";
        #endregion

        #region TDZMethod
        public static void SetOpenDoorDuration(int duration)
        {
            SysFunc.SetParamValue("TDZOpenDoorDuration", duration.ToString());
            lock (openDoorDuration)
                openDoorDuration = Convert.ToString(duration, 16).PadLeft(4, '0');
        }

        public static void InitOpenDoorDuration()
        {
            openDoorDuration = SysFunc.GetParamValue("TDZOpenDoorDuration").ToString();
        }

        public static string CreateHeartbeatResponse(bool isSucc, bool isCalibrate = false)
        {
            if (isSucc)
            {
                if (!isCalibrate)
                {
                    var respon = new { ResultCode = "1" };
                    return SysFunc.ToJson(respon);
                }
                else
                {
                    var strTime = DateTime.Now.ToString("yyMMddHHmmss");
                    var respon = new { ResultCode = "1", CorrectTime = strTime };
                    return SysFunc.ToJson(respon);
                }
            }
            var responFalse = new { ResultCode = "0" };
            return SysFunc.ToJson(responFalse);
        }

        public static string CreateOpenDoorResponse(bool isSucc, string ActIndex = "1")
        {
            if (isSucc)
            {
                var respon = new { ResultCode = "1", ActIndex = ActIndex, Audio = "00", Time1 = openDoorDuration, Time2 = openDoorDuration, Msg = "验证成功" };
                return SysFunc.ToJson(respon);
            }
            else
            {
                var responFalse = new { ResultCode = "0" };
                return SysFunc.ToJson(responFalse);
            }
        }

        public static string CreateUploadRecordResponse(bool isSucc)
        {
            if (isSucc)
            {
                var respon = new { ResultCode = "1" };
                return SysFunc.ToJson(respon);
            }
            else
            {
                var responFalse = new { ResultCode = "0" };
                return SysFunc.ToJson(responFalse);
            }
        }

        /// <summary>
        /// 身份证二进制数据读取
        /// </summary>
        /// <param name="Recieve"></param>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        /// <param name="idNumber"></param>
        /// <param name="bmp"></param>
        public static void GetIDInfoCodeData(byte[] Recieve, ref string name, ref string sex, ref string idNumber, ref string birthday,
            ref string nation, ref string address, ref string issuing_date, ref string expired_date, ref string department, ref byte[] bmp)
        {
            //byte[] SendCommand =new byte[10];

            int pos = 0;

            byte[] IDCard_Name = new byte[30];		//Name
            byte[] IDCard_Sex = new byte[2];		//1 Male 2 Female 3 other ==1 男，2 女，3 其它
            byte[] IDCard_National = new byte[4];		//National ==民族 0-55
            byte[] IDCard_Birthday = new byte[16];		//birthday ==生日 19801020
            byte[] IDCard_Address = new byte[70];     //address ==住址 
            byte[] IDCard_IDNumber = new byte[36];     //IDNumber ==身份证号码
            byte[] IDCard_Issuing = new byte[30];     //Issuing authority ==签发机关
            byte[] IDCard_Validity_StartDate = new byte[16];    //Validity_Start ==有效开始日期
            byte[] IDCard_Validity_EndDate = new byte[16];      //Validity_Start ==有效结束日期   	

            byte[] photo = new byte[1024];
            bmp = new byte[38862];

            if (Recieve.Length < 256)//长度有问题
                return;

            byte[] IDData = new byte[Recieve.Length];

            //pos = 14;
            //readerNo = Recieve[13];//读头号；一个设备可带多个身份证阅读器，区分是哪个阅读器读上来的
            //System.arraycopy(Recieve, pos, IDData, 0, nDataLength);
            //Array.Copy(Recieve, pos, IDData, 0, nDataLength);
            Array.Copy(Recieve, pos, IDData, 0, Recieve.Length);

            pos = 0;
            Array.Copy(IDData, pos, IDCard_Name, 0, IDCard_Name.Length);


            name = Encoding.Unicode.GetString(IDCard_Name).Trim();

            pos = IDCard_Name.Length;
            Array.Copy(IDData, pos, IDCard_Sex, 0, IDCard_Sex.Length);
            pos += IDCard_Sex.Length;


            sex = Encoding.Unicode.GetString(IDCard_Sex).Trim();
            if (sex == "1")
            {
                sex = "男";
            }
            else if (sex == "2")
            {
                sex = "女";
            }
            else
                sex = "";

            Array.Copy(IDData, pos, IDCard_National, 0, IDCard_National.Length);
            pos += IDCard_National.Length;
            nation = GetNational(Encoding.Unicode.GetString(IDCard_National).Trim());

            Array.Copy(IDData, pos, IDCard_Birthday, 0, IDCard_Birthday.Length);
            pos += IDCard_Birthday.Length;
            birthday = Encoding.Unicode.GetString(IDCard_Birthday).Trim();

            Array.Copy(IDData, pos, IDCard_Address, 0, IDCard_Address.Length);
            pos += IDCard_Address.Length;
            address = Encoding.Unicode.GetString(IDCard_Address).Trim();

            Array.Copy(IDData, pos, IDCard_IDNumber, 0, IDCard_IDNumber.Length);
            pos += IDCard_IDNumber.Length;
            idNumber = Encoding.Unicode.GetString(IDCard_IDNumber).Trim();

            Array.Copy(IDData, pos, IDCard_Issuing, 0, IDCard_Issuing.Length);
            pos += IDCard_Issuing.Length;
            department = Encoding.Unicode.GetString(IDCard_Issuing).Trim();

            Array.Copy(IDData, pos, IDCard_Validity_StartDate, 0, IDCard_Validity_StartDate.Length);
            pos += IDCard_Validity_StartDate.Length;
            issuing_date = Encoding.Unicode.GetString(IDCard_Validity_StartDate).Trim();

            Array.Copy(IDData, pos, IDCard_Validity_EndDate, 0, IDCard_Validity_EndDate.Length);
            pos += IDCard_Validity_EndDate.Length;
            expired_date = Encoding.Unicode.GetString(IDCard_Validity_EndDate).Trim();

            if (IDData.Length > 256)
                Array.Copy(IDData, 256, photo, 0, 1024);
            else
                bmp = null;
            int ret = IdUnPack.Unpack(photo, bmp);
        }

        #region 民族
        /// <summary>
        /// 获取民族
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetNational(string code)
        {
            string result = string.Empty;
            switch (code)
            {
                case "01":
                    result = "汉族";
                    break;
                case "02":
                    result = "蒙古族";
                    break;
                case "03":
                    result = "回族";
                    break;
                case "04":
                    result = "藏族";
                    break;
                case "05":
                    result = "维吾尔族";
                    break;
                case "06":
                    result = "苗族";
                    break;
                case "07":
                    result = "彝族";
                    break;
                case "08":
                    result = "壮族";
                    break;
                case "09":
                    result = "布依族";
                    break;
                case "10":
                    result = "朝鲜族";
                    break;
                case "11":
                    result = "满族";
                    break;
                case "12":
                    result = "侗族";
                    break;
                case "13":
                    result = "瑶族";
                    break;
                case "14":
                    result = "白族";
                    break;
                case "15":
                    result = "土家族";
                    break;
                case "16":
                    result = "哈尼族";
                    break;
                case "17":
                    result = "哈萨克族";
                    break;
                case "18":
                    result = "傣族";
                    break;
                case "19":
                    result = "黎族";
                    break;
                case "20":
                    result = "僳僳族";
                    break;
                case "21":
                    result = "佤族";
                    break;
                case "22":
                    result = "畲族";
                    break;
                case "23":
                    result = "高山族";
                    break;
                case "24":
                    result = "拉祜族";
                    break;
                case "25":
                    result = "水族";
                    break;
                case "26":
                    result = "东乡族";
                    break;
                case "27":
                    result = "纳西族";
                    break;
                case "28":
                    result = "景颇族";
                    break;
                case "29":
                    result = "柯尔克孜族";
                    break;
                case "30":
                    result = "土族";
                    break;
                case "31":
                    result = "达斡尔族";
                    break;
                case "32":
                    result = "仫佬族";
                    break;
                case "33":
                    result = "羌族";
                    break;
                case "34":
                    result = "布朗族";
                    break;
                case "35":
                    result = "撒拉族";
                    break;
                case "36":
                    result = "毛难族";
                    break;
                case "37":
                    result = "仡佬族";
                    break;
                case "38":
                    result = "锡伯族";
                    break;
                case "39":
                    result = "阿昌族";
                    break;
                case "40":
                    result = "普米族";
                    break;
                case "41":
                    result = "塔吉克族";
                    break;
                case "42":
                    result = "怒族";
                    break;
                case "43":
                    result = "乌孜别克族";
                    break;
                case "44":
                    result = "俄罗斯族";
                    break;
                case "45":
                    result = "鄂温克族";
                    break;
                case "46":
                    result = "崩龙族";
                    break;
                case "47":
                    result = "保安族";
                    break;
                case "48":
                    result = "裕固族";
                    break;
                case "49":
                    result = "京族";
                    break;
                case "50":
                    result = "塔塔尔族";
                    break;
                case "51":
                    result = "独龙族";
                    break;
                case "52":
                    result = "鄂伦春族";
                    break;
                case "53":
                    result = "赫哲族";
                    break;
                case "54":
                    result = "门巴族";
                    break;
                case "55":
                    result = "珞巴族";
                    break;
                case "56":
                    result = "基诺族";
                    break;
                default:
                    result = "其他";
                    break;
            }
            return result;
        }
        #endregion
        #endregion

    }
}
