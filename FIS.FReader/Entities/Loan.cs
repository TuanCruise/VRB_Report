using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIS.FReader.Entities
{
    class Loan : FileToRead
    {
        private String reportDate;
        public String ReportDate
        {
            get { return reportDate; }
            set { reportDate = value; }
        }
        private String reportCreationDate;

        public String ReportCreationDate
        {
            get { return reportCreationDate; }
            set { reportCreationDate = value; }
        }
        private List<GroupLoan> listGroupLoan = new List<GroupLoan>();

        public List<GroupLoan> ListGroupLoan
        {
            get { return listGroupLoan; }
            set { listGroupLoan = value; }
        }

    }
    public class GroupLoan
    {
        private String reportDateTableName;

        public String ReportDateTableName
        {
            get { return reportDateTableName; }
            set { reportDateTableName = value; }
        }

        private List<ContractLoanType> listLoanType = new List<ContractLoanType>();
        public List<ContractLoanType> ListLoanType
        {
            get { return listLoanType; }
            set { listLoanType = value; }
        }
    }
    public class ContractLoanType
    {
        private string acctNo;

        public string AcctNo
        {
            get { return acctNo; }
            set { acctNo = value; }
        }

        private string name;
        private string branch;

        public string Branch
        {
            get { return branch; }
            set { branch = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string passportNo;

        public string PassportNo
        {
            get { return passportNo; }
            set { passportNo = value; }
        }

        private string openingBalance;

        public string OpeningBalance
        {
            get { return openingBalance; }
            set { openingBalance = value; }
        }

        private int rowNumber;

        public int RowNumber
        {
            get { return rowNumber; }
            set { rowNumber = value; }
        }
    }
}
