using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIS.FRead
{
    public class ContractInfo : FReadEntityBase
    {

        private string m_string_ContractNo;
        private string m_string_ClientName;
        private string m_string_Interests;

        public string ContractNo
        {
            get { return m_string_ContractNo; }
            set { m_string_ContractNo = value; }
        }
        public string ClientName
        {
            get { return m_string_ClientName; }
            set { m_string_ClientName = value; }
        }
        public string Interests
        {
            get { return m_string_Interests; }
            set { m_string_Interests = value; }
        }

    }
}
