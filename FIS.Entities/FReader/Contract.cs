using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIS.FRead
{
    public class Contract : FReadEntityBase
    {
        List<ContractInfo> listData = new List<ContractInfo>();
        private string m_string_ContractType;
        private int m_int_Currency;
        public List<ContractInfo> ListData
        {
            get { return listData; }
            set { listData = value; }
        }
        public string ContractType
        {
            get { return m_string_ContractType; }
            set { m_string_ContractType = value; }
        }
        public int Currency
        {
            get { return m_int_Currency; }
            set { m_int_Currency = value; }
        }

    }
}
