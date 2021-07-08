using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ADServer.BLL
{
    public partial class B_Employ_Info
    {
        private readonly DAL.D_Employ_Info dal = new DAL.D_Employ_Info();
        public B_Employ_Info()
        { }

        #region 自定义方法

        /// <summary>
        /// 根据员工编号，删除该员工确认账号、来访信息
        /// </summary>
        /// <param name="empno"></param>
        public void deleteAllByEmpno(int empno)
        {
            dal.deleteAllByEmpno(empno);
        }

        /// <summary>
        /// 得到员工信息模板
        /// </summary>
        public DataSet GetEmployCommon()
        {
            return dal.GetEmployCommon();
        }

        /// <summary>
        /// 存储过程处理导入员工数据
        /// </summary>
        /// <returns></returns>
        public string ProcDealImport()
        {
            return dal.ProcDealImport();
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetListNew(int topNum, string strWhere)
        {
            return dal.GetListNew(topNum, strWhere);
        }

        /// <summary>
        /// 开启来访确认功能后查询被访人信息--增加被访人在线状态
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetListByService(string strWhere)
        {
            return dal.GetListByService(strWhere);
        }

        /// <summary>
        /// 新增修改时，判断名字是否重复
        /// （同一公司，同一部门，姓名不能重复）
        /// </summary>
        /// <param name="belongName"></param>
        /// <returns></returns>
        public Boolean Exists(int companyid, int deptno, int empno, string empname)
        {
            return dal.Exists(companyid, deptno, empno, empname);
        }

        /// <summary>
        /// 判断手机号码是否已存在
        /// </summary>
        /// <param name="empTel"></param>
        /// <returns></returns>
        public Boolean ExistTel(string empTel)
        {
            return dal.ExistTel(empTel);
        }

        /// <summary>
        /// 判断是否存在同一手机号码
        /// </summary>
        /// <param name="empTel"></param>
        /// <returns></returns>
        public Boolean ExistTel(string empTel, string name)
        {
            return dal.ExistTel(empTel, name);
        }

        /// <summary>
        /// 判断是否存在同一手机号码
        /// </summary>
        /// <param name="empTel"></param>
        /// <returns></returns>
        public Boolean ExistTel(string empTel, int empNo)
        {
            return dal.ExistTel(empTel, empNo);
        }

        /// <summary>
        /// 判断IC卡号是否已存在
        /// </summary>
        /// <param name="empTel"></param>
        /// <returns></returns>
        public Boolean ExistICCardno(string cardno)
        {
            return dal.ExistICCardno(cardno);
        }

        /// <summary>
        /// 判断是否存在同一IC卡号
        /// </summary>
        /// <param name="empTel"></param>
        /// <returns></returns>
        public Boolean ExistICCardno(string cardno, string name)
        {
            return dal.ExistICCardno(cardno, name);
        }

        /// <summary>
        /// 判断是否存在同一IC卡号
        /// </summary>
        /// <param name="empTel"></param>
        /// <returns></returns>
        public Boolean ExistICCardno(string cardno, int empNo)
        {
            return dal.ExistICCardno(cardno, empNo);
        }

        /// <summary>
        /// 根据员工姓名获取员工编号
        /// </summary>
        /// <returns></returns>
        public int GetEmpNo(string name)
        {
            return dal.GetEmpNo(name);
        }

        /// <summary>
        /// 根据员工卡卡号获取员工编号
        /// </summary>
        /// <returns></returns>
        public int GetEmpNoByCardno(string cardno)
        {
            return dal.GetEmpNoByCardno(cardno);
        }


        #endregion


        #region  Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.M_Employ_Info model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.M_Employ_Info model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 设置员工卡的门禁期限
        /// </summary>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public bool SetCardEndDate(int empNo, DateTime? endDate)
        {
            return dal.SetCardEndDate(empNo, endDate);
        }


        /// <summary>
        /// 重置预约网密码
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool ResetPwd(int empId, string pwd)
        {
            return dal.ResetPwd(empId, pwd);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int EmpNo)
        {

            return dal.Delete(EmpNo);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string EmpNolist)
        {
            return dal.DeleteList(EmpNolist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.M_Employ_Info GetModel(int EmpNo)
        {
            return dal.GetModel(EmpNo);
        }


        /// <summary>
        /// 通过姓名和手机号码查询被访人
        /// </summary>
        public Model.M_Employ_Info GetModel(string empName, string empTel)
        {
            return dal.GetModel(empName, empTel);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        public DataSet GetEmployBook(string empno)
        {
            return dal.GetEmployBook(empno);
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.M_Employ_Info> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.M_Employ_Info> DataTableToList(DataTable dt)
        {
            List<Model.M_Employ_Info> modelList = new List<Model.M_Employ_Info>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Model.M_Employ_Info model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Model.M_Employ_Info();
                    if (dt.Rows[n]["EmpNo"] != null && dt.Rows[n]["EmpNo"].ToString() != "")
                    {
                        model.EmpNo = int.Parse(dt.Rows[n]["EmpNo"].ToString());
                    }
                    if (dt.Rows[n]["EmpName"] != null && dt.Rows[n]["EmpName"].ToString() != "")
                    {
                        model.EmpName = dt.Rows[n]["EmpName"].ToString();
                    }
                    if (dt.Rows[n]["EmpSex"] != null && dt.Rows[n]["EmpSex"].ToString() != "")
                    {
                        model.EmpSex = dt.Rows[n]["EmpSex"].ToString();
                    }
                    if (dt.Rows[n]["EmpFloor"] != null && dt.Rows[n]["EmpFloor"].ToString() != "")
                    {
                        model.EmpFloor = dt.Rows[n]["EmpFloor"].ToString();
                    }
                    if (dt.Rows[n]["EmpRoomCode"] != null && dt.Rows[n]["EmpRoomCode"].ToString() != "")
                    {
                        model.EmpRoomCode = dt.Rows[n]["EmpRoomCode"].ToString();
                    }
                    if (dt.Rows[n]["EmpTel"] != null && dt.Rows[n]["EmpTel"].ToString() != "")
                    {
                        model.EmpTel = dt.Rows[n]["EmpTel"].ToString();
                    }
                    if (dt.Rows[n]["EmpMobile"] != null && dt.Rows[n]["EmpMobile"].ToString() != "")
                    {
                        model.EmpMobile = dt.Rows[n]["EmpMobile"].ToString();
                    }
                    if (dt.Rows[n]["EmpExtTel"] != null && dt.Rows[n]["EmpExtTel"].ToString() != "")
                    {
                        model.EmpExtTel = dt.Rows[n]["EmpExtTel"].ToString();
                    }
                    if (dt.Rows[n]["EmpPosition"] != null && dt.Rows[n]["EmpPosition"].ToString() != "")
                    {
                        model.EmpPosition = dt.Rows[n]["EmpPosition"].ToString();
                    }
                    if (dt.Rows[n]["EmpPhoto"] != null && dt.Rows[n]["EmpPhoto"].ToString() != "")
                    {
                        model.EmpPhoto = (byte[])dt.Rows[n]["EmpPhoto"];
                    }
                    if (dt.Rows[n]["DeptNo"] != null && dt.Rows[n]["DeptNo"].ToString() != "")
                    {
                        model.DeptNo = int.Parse(dt.Rows[n]["DeptNo"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #region 德生智能信息管理系统

        /// <summary>
        /// 是否存在此被访人id的记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean ExistEmpId_pf(string EmployNo)
        {
            return dal.ExistEmpId_pf(EmployNo);
        }
        /// <summary>
        /// 添加员工信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add_pf(Model.M_Employ_Info model)
        {
            return dal.Add_pf(model);
        }
        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update_pf(Model.M_Employ_Info model)
        {
            return dal.Update_pf(model);
        }

        public bool Delete_pf(string EmployNo)
        {
            return dal.Delete_pf(EmployNo);
        }

        public Model.M_Employ_Info GetModel_pf(string EmployNo)
        {
            return dal.GetModel_pf(EmployNo);
        }


        #endregion

        #region 微信预约被访人档案

        /// <summary>
        /// 是否存在此被访人id的记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean ExistEmpId_wx(int weixinId)
        {
            return dal.ExistEmpId_wx(weixinId);
        }

        /// <summary>
        /// 添加员工信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add_wx(Model.M_Employ_Info model)
        {
            return dal.Add_wx(model);
        }

        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update_wx(Model.M_Employ_Info model)
        {
            return dal.Update_wx(model);
        }

        public bool Delete_wx(int weixinId)
        {
            return dal.Delete_wx(weixinId);
        }

        public Model.M_Employ_Info GetModel_wx(int weixinId)
        {
            return dal.GetModel_wx(weixinId);
        }

        #endregion

        #region 盛炬一卡通平台

        /// <summary>
        /// 查询盛炬一卡通平台的员工列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetList_SJP(string strWhere)
        {
            return dal.GetList_SJP(strWhere);
        }

        /// <summary>
        /// 是否存在此被访人id的记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean ExistEmpId_SJP(string sjId)
        {
            return dal.ExistEmpId_SJP(sjId);
        }

        public Model.M_Employ_Info GetModel_SJP(string sjId)
        {
            return dal.GetModel_SJP(sjId);
        }

        public bool isDeleted_SJP(string sjId)
        {
            return dal.isDeleted_SJP(sjId);
        }

        public bool Delete_SJP(string sjId)
        {
            return dal.Delete_SJP(sjId);
        }

        /// <summary>
        /// 修改员工信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update_SJP(Model.M_Employ_Info model)
        {
            return dal.Update_SJP(model);
        }

        #endregion

        #endregion  Method

        public DataSet GetListWhereTel(string strWhere)
        {
            return dal.GetListWhereTel(strWhere);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ADServer.Model.M_Employ_Info GetModelByCarNum(string carNum)
        {
            return dal.GetModelByCarNum(carNum);
        }


        /// <summary>
        /// 判断工号是否已存在
        /// </summary>
        /// <param name="empnum"></param>
        /// <returns></returns>
        public Boolean ExistNum(string empnum)
        {
            return dal.ExistNum(empnum);
        }
    }
}
