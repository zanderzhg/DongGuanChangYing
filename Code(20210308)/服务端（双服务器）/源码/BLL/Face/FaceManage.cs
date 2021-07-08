using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ADServer.DAL;
using System.Windows.Forms;

namespace ADServer.BLL.Face
{
    public class FaceManage
    {
        /// <summary>
        /// 将人脸从库中删除
        /// </summary>
        /// <param name="jobNumber">工号</param>
        /// <param name="photoDB">人脸照片库</param>
        /// <returns>0 成功</returns>
        public static int DelFace(string jobNumber, string photoDB)
        {
            uint pid = 0; //人员编号
            uint tid = 0; //照片编号

            // 设置查询条件
            IntPtr hQueryParams = FaceHelper.FgiCreateParamSet();

            #region 【步骤1】通过身份号码去人员表查询人员id
            //【步骤1】通过身份号码去人员表查询人员id
            char[] nums = jobNumber.ToCharArray();
            FaceHelper.FgiAddParameter(hQueryParams, "身份号码", (int)OperatorType.OpLess, nums, nums.Length);

            // 生成记录集
            IntPtr hRecords = FaceHelper.FgiCreateRecordSet();
            //设置查询范围  查前10条
            int ret = FaceHelper.FgiSetQueryPaging(hRecords, 10, 1);
            // 获取记录
            ret = FaceHelper.FgiQueryPersons(SysFunc.UsToken, photoDB, hQueryParams, hRecords);
            //MessageBox.Show("FgiQueryPersons:" + jobNumber + "_" + SysFunc.UsToken + "_" + photoDB + "_" + +ret);
            if (ret != 0)
            {
                //FileHelper.WriteSystemMsg(Server.MapPath("/SysLog"), "" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",【FgiQueryPersons】:Code=" + ret + ",获取记录失败！");
                return ret;
            }



            //检查输入的身份号码跟查出来的身份号码是否匹配
            bool isMatch = false;
            int queryCount = FaceHelper.FgiGetFieldCount(hRecords);
            if (queryCount > 0)
            {
                for (int i = 0; i < queryCount; ++i)
                {
                    //依次取出元素
                    int size = 0;
                    IntPtr field = FaceHelper.FgiGetField(hRecords, i, ref size);
                    string pidStr = Marshal.PtrToStringAnsi(FaceHelper.FgiGetField_S(field, "系统编号", ref size));
                    string certNum = Marshal.PtrToStringAnsi(FaceHelper.FgiGetField_S(field, "身份号码", ref size));

                    if (jobNumber.Equals(certNum))
                    {
                        //匹配成功
                        pid = uint.Parse(pidStr);
                        isMatch = true;
                        break;
                    }
                }
            }

            FaceHelper.FgiCloseHandle(hQueryParams);
            FaceHelper.FgiCloseHandle(hRecords);

            if (isMatch == false)
            {
                //FileHelper.WriteSystemMsg(Server.MapPath("/SysLog"), "【FgiAddPhoto】:Code=" + ret + ",该工号所对应的人员不存在！");
            }
            #endregion

            #region 【步骤2】通过人员id去照片表查询照片id
            //【步骤2】通过人员id去照片表查询照片id
            hQueryParams = FaceHelper.FgiCreateParamSet();
            ret = FaceHelper.FgiAddParameter(hQueryParams, "人员编号", (int)OperatorType.OpEqual, ref pid, sizeof(uint));

            // 获取记录总数 有可能一个人入库了多张照片
            int total = 0;
            ret = FaceHelper.FgiCountPhotos(SysFunc.UsToken, photoDB, hQueryParams, ref total);
            int pageSize = total;
            int pageIndex = 1;

            // 生成记录集
            hRecords = FaceHelper.FgiCreateRecordSet();
            //设置查询范围
            ret = FaceHelper.FgiSetQueryPaging(hRecords, (UInt32)pageSize, (UInt32)pageIndex);
            // 获取记录
            ret = FaceHelper.FgiQueryPhotos(SysFunc.UsToken, photoDB, hQueryParams, hRecords);
       
            if (ret != 0)
            {
                //FileHelper.WriteSystemMsg(Server.MapPath("/SysLog"), "" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",【FgiQueryPhotos】:Code=" + ret + ",获取记录失败！");
                return ret;
            }
            //解析数据
            queryCount = FaceHelper.FgiGetFieldCount(hRecords);
            if (queryCount > 0)
            {
                for (int i = 0; i < queryCount; ++i)
                {
                    //依次取出元素
                    int size = 0;
                    IntPtr field = FaceHelper.FgiGetField(hRecords, i, ref size);

                    //从IntPtr读出数据到内存byte[]里面，再从byte[]转换到相应的结构体
                    PHOTOFACE photoFace = new PHOTOFACE();
                    byte[] pByte = new byte[size];
                    Marshal.Copy(field, pByte, 0, size);  // Marshal.SizeOf(identifyTask)
                    photoFace = (PHOTOFACE)SysFunc.BytesToStruct(pByte, photoFace.GetType());

                    //获取照片编号
                    tid = photoFace.fid;
                }
            }
            #endregion

            #region 【步骤3】通过人员id、照片id去删除对应的数据
            //【步骤3】通过人员id、照片id去删除对应的数据
            // 设置查询条件
            hQueryParams = FaceHelper.FgiCreateParamSet();
            FaceHelper.FgiAddParameter(hQueryParams, "照片编号", (int)OperatorType.OpEqual, ref tid, sizeof(uint));

            //先删除人脸照片
            ret = FaceHelper.FgiDeletePhotos(SysFunc.UsToken, photoDB, hQueryParams);
            FaceHelper.FgiCloseHandle(hQueryParams);

            //人脸照片删除成功  再删除人员信息
            if (ret == 0)
            {
                uint numDel = 0;

                // 设置查询条件
                hQueryParams = FaceHelper.FgiCreateParamSet();
                FaceHelper.FgiAddParameter(hQueryParams, "系统编号", (int)OperatorType.OpEqual, ref pid, sizeof(uint));

                // 删除记录
                ret = FaceHelper.FgiDeletePersons(SysFunc.UsToken, photoDB, hQueryParams, ref numDel);
                if (ret != 0)
                {
                    //FileHelper.WriteSystemMsg(Server.MapPath("/SysLog"), "" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",【FgiDeletePersons】:Code=" + ret + ",人员信息删除失败！");
                }

                FaceHelper.FgiCloseHandle(hQueryParams);
            }
            else
            {
                //FileHelper.WriteSystemMsg(Server.MapPath("/SysLog"), "" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",【FgiDeletePhotos】:Code=" + ret + ",人脸照片删除失败！");
            }
            #endregion

            return ret;
        }
    }
}
