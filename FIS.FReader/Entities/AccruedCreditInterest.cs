using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIS.FReader.Entities
{
    class AccruedCreditInterest : FileToRead
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

        private List<ContractAccruedType> listContractType = new List<ContractAccruedType>();
        public List<ContractAccruedType> ListContractType
        {
            get { return listContractType; }
            set { listContractType = value; }
        }
    }

    public class ContractAccruedType
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

        private List<ContractAccrued> listContract = new List<ContractAccrued>();

        public List<ContractAccrued> ListContract
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

    public class ContractAccrued
    {
        private String contractNo;

        public String ContractNo
        {
            get { return contractNo; }
            set { contractNo = value; }
        }

        private String clientName;

        public String ClientName
        {
            get { return clientName; }
            set { clientName = value; }
        }

        private string interests;

        public string Interests
        {
            get { return interests; }
            set { interests = value; }
        }

        private int rowNumber;

        public int RowNumber
        {
            get { return rowNumber; }
            set { rowNumber = value; }
        }
    }
}
