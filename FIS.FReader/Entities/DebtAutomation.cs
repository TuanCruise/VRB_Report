using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIS.FReader.Entities
{
    public class DebtAutomation:FileToRead
    {
        private List<Debt> listDebt = new List<Debt>();

        public List<Debt> ListDebt
        {
            get { return listDebt; }
            set { listDebt = value; }
        }
    }

    //public class Debt
    //{
    //    private string column1;

    //    public string Column1
    //    {
    //        get { return column1; }
    //        set { column1 = value; }
    //    }
    //    private string column2;

    //    public string Column2
    //    {
    //        get { return column2; }
    //        set { column2 = value; }
    //    }
    //    private string column3;

    //    public string Column3
    //    {
    //        get { return column3; }
    //        set { column3 = value; }
    //    }
    //    private string column4;

    //    public string Column4
    //    {
    //        get { return column4; }
    //        set { column4 = value; }
    //    }
    //    private string column5;

    //    public string Column5
    //    {
    //        get { return column5; }
    //        set { column5 = value; }
    //    }

    //    private int rowNumber;

    //    public int RowNumber
    //    {
    //        get { return rowNumber; }
    //        set { rowNumber = value; }
    //    }
    //}

    public class Debt
    {
        private string cardNumber;

        public string CardAccountNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }
        private string nameOnCard;

        public string EmbossingName
        {
            get { return nameOnCard; }
            set { nameOnCard = value; }
        }
        private string branchPart;

        public string BranchPart
        {
            get { return branchPart; }
            set { branchPart = value; }
        }
        private string bankAccountNumber;

        public string BankAccountNumber
        {
            get { return bankAccountNumber; }
            set { bankAccountNumber = value; }
        }
        private string debtCreditFlag;

        public string DebtCreditFlag
        {
            get { return debtCreditFlag; }
            set { debtCreditFlag = value; }
        }
        private string cardAccountCurrency;

        public string CardAccountCurrency
        {
            get { return cardAccountCurrency; }
            set { cardAccountCurrency = value; }
        }
        private string amount;

        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        private string numOfDecPlace;

        public string NumOfDecPlace
        {
            get { return numOfDecPlace; }
            set { numOfDecPlace = value; }
        }
        private string postingDate;

        public string PostingDate
        {
            get { return postingDate; }
            set { postingDate = value; }
        }
        private string accountNumber;

        public string AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; }
        }
        private string invoiceNumber;

        public string InvoiceNumber
        {
            get { return invoiceNumber; }
            set { invoiceNumber = value; }
        }
        private string filler;

        public string Filler
        {
            get { return filler; }
            set { filler = value; }
        }
        private int rowNumber;

        public int RowNumber
        {
            get { return rowNumber; }
            set { rowNumber = value; }
        }
    }
}
