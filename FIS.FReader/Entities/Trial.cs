using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIS.FReader.Entities
{
    class Trial : FileToRead
    {
        private String reportFromDate;

        public String ReportFromDate
        {
            get { return reportFromDate; }
            set { reportFromDate = value; }
        }
        private String reportToDate;

        public String ReportToDate
        {
            get { return reportToDate; }
            set { reportToDate = value; }
        }
        private List<Branch> listBranch = new List<Branch>();

        public List<Branch> ListBranch
        {
            get { return listBranch; }
            set { listBranch = value; }
        }
    }
    public class Branch
    {
        private String branchName;

        public String BranchName
        {
            get { return branchName; }
            set { branchName = value; }
        }
        private String currency;

        public String Currency
        {
            get { return currency; }
            set { currency = value; }
        }
        private List<GroupTrial> listGroupTrial = new List<GroupTrial>();

        public List<GroupTrial> ListGroupTrial
        {
            get { return listGroupTrial; }
            set { listGroupTrial = value; }
        }

    }
    public class GroupTrial
    {
        private String groupName;

        public String GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }
        private List<TrialRow> listTrialRow = new List<TrialRow>();

        public List<TrialRow> ListTrialRow
        {
            get { return listTrialRow; }
            set { listTrialRow = value; }
        }

    }
    public class TrialRow
    {
        private String accountNo;

        public String AccountNo
        {
            get { return accountNo; }
            set { accountNo = value; }
        }
        private String openingBalance;

        public String OpeningBalance
        {
            get { return openingBalance; }
            set { openingBalance = value; }
        }
        private String debit;

        public String Debit
        {
            get { return debit; }
            set { debit = value; }
        }
        private String credit;

        public String Credit
        {
            get { return credit; }
            set { credit = value; }
        }
        private String closingBalance;

        public String ClosingBalance
        {
            get { return closingBalance; }
            set { closingBalance = value; }
        }
    }
}