using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.BLL
{
    public partial class B_Groble_Info
    {
        private readonly DAL.D_Groble_Info dal = new DAL.D_Groble_Info();
        public B_Groble_Info()
        { }

        #region  Method

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string machinecode)
        {
            return dal.Exists(machinecode);
        }

        /// <summary>
        /// 是否存在该记录 服务端
        /// </summary>
        public bool ExistsService(string isService)
        {
            return dal.ExistsService(isService);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.M_Groble_Info model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.M_Groble_Info model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.M_Groble_Info GetModel()
        {
            return dal.GetModel();
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.M_Groble_Info GetServiceModel(string isService)
        {
            return dal.GetServiceModel(isService);
        }

        /// <summary>
        /// 更改一进一出的全局配置
        /// </summary>
        /// <param name="isLeaveAndCancel"></param>
        public void UpdateLeaveAndCancel(string isLeaveAndCancel)
        {
            dal.UpdateLeaveAndCancel(isLeaveAndCancel);
        }

        /// <summary>
        /// 更改常访卡有效天数的全局配置
        /// </summary>
        /// <param name="days"></param>
        public void UpdateGrantDays(decimal days)
        {
            dal.UpdateGrantDays(days);
        }

        #endregion  Method
    }
}
