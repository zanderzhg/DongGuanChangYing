using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public class M_White_Info
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _visitCompany;

        public string VisitCompany
        {
            get { return _visitCompany; }
            set { _visitCompany = value; }
        }
        private string _certType;

        public string CertType
        {
            get { return _certType; }
            set { _certType = value; }
        }
        private string _certNo;

        public string CertNo
        {
            get { return _certNo; }
            set { _certNo = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _sex;

        public string Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }
        private string _phone;

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        private string _operateName;

        public string OperateName
        {
            get { return _operateName; }
            set { _operateName = value; }
        }
    }
}
