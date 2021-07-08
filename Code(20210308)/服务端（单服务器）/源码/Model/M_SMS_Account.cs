using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public class M_SMS_Account
    {
        public M_SMS_Account()
        { }

        private string accountname;
        /// <summary>
        /// 账号名
        /// </summary>
        public string Accountname
        {
            get { return accountname; }
            set { accountname = value; }
        }
        private string pwd;
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
        private string companyname;
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Companyname
        {
            get { return companyname; }
            set { companyname = value; }
        }
        private string sign;
        /// <summary>
        /// 账号签名
        /// </summary>
        public string Sign
        {
            get { return sign; }
            set { sign = value; }
        }
        private string serverurl;
        /// <summary>
        /// 服务网址
        /// </summary>
        public string Serverurl
        {
            get { return serverurl; }
            set { serverurl = value; }
        }

        private string noticecheckin;
        /// <summary>
        /// 访客登记完成,短信通知被访人其访客的到访
        /// </summary>
        public string NoticeCheckin
        {
            get { return noticecheckin; }
            set { noticecheckin = value; }
        }
        private string noticeleave;
        /// <summary>
        /// 访客签离,短信通知被访人其访客已离开
        /// </summary>
        public string NoticeLeave
        {
            get { return noticeleave; }
            set { noticeleave = value; }
        }

    }
}
