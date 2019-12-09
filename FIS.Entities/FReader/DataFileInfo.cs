using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIS.FRead
{
    public class DataFileInfo : FReadEntityBase
    {
        private string m_string_FileName;
        List<Contract> listContract = new List<Contract>();
        List<LoanAcct> ListLoanAcct = new List<LoanAcct>();

        public List<LoanAcct> ListLoanAct
        {
            get { return ListLoanAcct; }
            set { ListLoanAcct = value; }
        }

        public List<Contract> ListContract
        {
            get { return listContract; }
            set { listContract = value; }
        }
        public string FileName
        {
            get { return m_string_FileName; }
            set { m_string_FileName = value; }
        }
        private DateTime m_date_DateCreate;

        public DateTime DateCreate
        {
            get { return m_date_DateCreate; }
            set { m_date_DateCreate = value; }
        }
    }
}
