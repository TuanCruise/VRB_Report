using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIS.FReader.Entities;
using System.Globalization;
using FIS.FReader.Utilities;
using System.IO;
using System.Diagnostics;

namespace FIS.FReader.Excute
{
    class LoanExcute
    {
        Loan loanReport;
        List<Loan> listLoanReport;

        internal List<Loan> ListLoanReport
        {
            get { return listLoanReport; }
            //set { listLoanReport = value; }
        }

        ContractLoanType contractLoanType = null;
        GroupLoan groupLoan = null;
        public Loan LoanReport
        {
            get { return loanReport; }
            //set { feeReport = value; }
        }
        /// <summary>
        /// get list report from list file input
        /// </summary>
        /// <param name="inputFile"> list input file by list string</param>
        /// <returns>List Load</returns>
        public List<Loan> GetReport(List<string> inputFile)
        {
            listLoanReport = new List<Loan>();
            for (int i = 0; i < inputFile.Count; i++)
            {
                listLoanReport.Add(GetReport(inputFile[i]));
            }
            return listLoanReport;
        }

        /// <summary>
        /// get report from a file by input file name
        /// </summary>
        /// <param name="inputFile"> file name </param>
        /// <returns>report feeReport</returns>
        public Loan GetReport(string inputFile)
        {
            loanReport = new Loan();
            var watch = Stopwatch.StartNew();
            loanReport.LoadFile(inputFile);
            const Int32 BufferSize = 128;
            var fileStream = File.OpenRead(inputFile);
            var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize);
            String line;
            List<string> ltsString = new List<string>();
            int rowNumber = 0;
            while ((line = streamReader.ReadLine()) != null)
            {
                rowNumber++;
                if (streamReader.EndOfStream)
                {
                    loanReport.FileRow = rowNumber;
                }
                ReadLine(line, rowNumber);
            }

            // the code that you want to measure comes here
            fileStream.Close();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            loanReport.TimeProcess = elapsedMs.ToString();
            return loanReport;
        }

        private void ReadLine(String inputString, int inputRowNumber)
        {
            if (inputString.StartsWith(Constant.Loan.ReportConstant.IGNORE_ROW_1))
                return;

            if (inputString.StartsWith(Constant.Loan.ReportConstant.IGNORE_ROW_2))
                return;

            if (inputString.StartsWith(Constant.Loan.ReportConstant.IGNORE_ROW_3))
                return;

            if (inputString.StartsWith(Constant.Loan.ReportConstant.IGNORE_ROW_4))
                return;
            if (inputString.Contains("--"))
                return;

            if (inputRowNumber == 1)
            {
                this.loanReport.ReportCreationDate = inputString.Trim();
                return;
            }

            if (inputString.Trim().StartsWith("for"))
            {
                this.loanReport.ReportDate = inputString.Replace("for", " ").Trim();
                return;
            }

            if (inputString.Trim().StartsWith(Constant.Loan.ReportConstant.ACCT))
            {
                groupLoan = new GroupLoan();
                this.groupLoan.ReportDateTableName = inputString.Trim();
                return;
            }

            if (inputString.Split(Constant.Loan.ReportConstant.COLUMNSEPARATE).Length == 7)
            {
                //if (!String.IsNullOrEmpty(inputString.Split(Constant.Loan.ReportConstant.COLUMNSEPARATE)[1].Trim()) || !String.IsNullOrEmpty(inputString.Split(Constant.Loan.ReportConstant.COLUMNSEPARATE)[2].Trim()))
                //{
                    contractLoanType = new ContractLoanType();
                    contractLoanType.AcctNo = inputString.Split(Constant.Loan.ReportConstant.COLUMNSEPARATE)[1].Trim();
                    contractLoanType.Branch = inputString.Split(Constant.Loan.ReportConstant.COLUMNSEPARATE)[2].Trim();
                    contractLoanType.Name = inputString.Split(Constant.Loan.ReportConstant.COLUMNSEPARATE)[3].Trim();
                    contractLoanType.PassportNo = inputString.Split(Constant.Loan.ReportConstant.COLUMNSEPARATE)[4].Trim();
                    contractLoanType.OpeningBalance = inputString.Split(Constant.Loan.ReportConstant.COLUMNSEPARATE)[5].Trim();
                    contractLoanType.RowNumber = inputRowNumber;
                    this.groupLoan.ListLoanType.Add(contractLoanType);
                //}
                return;
            }
            if (inputString.Contains("|Total   "))
            {
                this.loanReport.ListGroupLoan.Add(groupLoan);
            }
        }

    }
}
