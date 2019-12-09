using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIS.FRead
{
    public class LoanAcct : FReadEntityBase
    {
        string m_string_AcctNo;
        string m_string_AcctName;
        string m_string_PassportNo;
        string m_string_OpenBalance;
        public string AcctNo
        {
            get { return m_string_AcctNo; }
            set { m_string_AcctNo = value; }
        }


        public string AcctName
        {
            get { return m_string_AcctName; }
            set { m_string_AcctName = value; }
        }
        

        public string PassportNo
        {
            get { return m_string_PassportNo; }
            set { m_string_PassportNo = value; }
        }
        

        public string OpenBalance
        {
            get { return m_string_OpenBalance; }
            set { m_string_OpenBalance = value; }
        }
    }
}
