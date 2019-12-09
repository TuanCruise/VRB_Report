using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIS.FReader.Entities
{
    public class FeeOverduePayment : FileToRead
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

        private List<FeeContractType> listContractType = new List<FeeContractType>();
        public List<FeeContractType> ListContractType
        {
            get { return listContractType; }
            set { listContractType = value; }
        }

        public FeeOverduePayment()
        { }

        public FeeOverduePayment(String inputFileName)
        {
        }

        public String ReadFromFile(String inputFileName)
        {


            return null;
        }

        public FeeOverduePayment GetReport(String inputLine)
        {
                if (inputLine.Contains("Report Date:"))
                {
                    this.ReportDate = inputLine.Split(':')[1].Trim();

                }
                else if (inputLine.Contains("Report Creation Time"))
                {
                    this.ReportCreationTime = inputLine.Replace("Report Creation Time: ", " ").Trim();
                }

            return this;
        }
    }

    public class FeeContractType
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

        private List<FeeContract> listContract = new List<FeeContract>();

        public List<FeeContract> ListContract
        {
            get { return listContract; }
            set { listContract = value; }
        }

        private string currency;

        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }
    }

    public class FeeContract
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

        private string due;

        public string Due
        {
            get { return due; }
            set { due = value; }
        }

        private string paid;

        public string Paid
        {
            get { return paid; }
            set { paid = value; }
        }

        private int rowNumber;

        public int RowNumber
        {
            get { return rowNumber; }
            set { rowNumber = value; }
        }
    }
}
