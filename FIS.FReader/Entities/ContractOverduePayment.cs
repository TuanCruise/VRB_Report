using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIS.FReader.Entities
{
    public class ContractOverduePayment:FileToRead
    {
        private String reportDate;
        public String ReportDate
        {
            get { return reportDate; }
            set { reportDate = value; }
        }

        private String reportCreationTime;
        public String ReportCreationTime
        {
            get { return reportCreationTime; }
            set { reportCreationTime = value; }
        }

        private List<ContractOverType> listContractType = new List<ContractOverType>();
        public List<ContractOverType> ListContractType
        {
            get { return listContractType; }
            set { listContractType = value; }
        }

    }

    public class ContractOverType
    {
        private String preFix;

        public String PreFix
        {
            get { return preFix; }
            set { preFix = value; }
        }

        private String conType;

        public String ConType
        {
            get { return conType; }
            set { conType = value; }
        }

        private List<ContractOver> listContract = new List<ContractOver>();

        public List<ContractOver> ListContract
        {
            get { return listContract; }
            set { listContract = value; }
        }
    }

    public class ContractOver
    {
        private String contractNo;

        public String ContractNo
        {
            get { return contractNo; }
            set { contractNo = value; }
        }

        private String account;

        public String Account
        {
            get { return account; }
            set { account = value; }
        }

        private string currency;

        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }

        private string minPaymentAcc;

        public string MinPaymentAcc
        {
            get { return minPaymentAcc; }
            set { minPaymentAcc = value; }
        }

        private string duePeriod;

        public string DuePeriod
        {
            get { return duePeriod; }
            set { duePeriod = value; }
        }

        private int rowNumber;

        public int RowNumber
        {
            get { return rowNumber; }
            set { rowNumber = value; }
        }
    }
}
