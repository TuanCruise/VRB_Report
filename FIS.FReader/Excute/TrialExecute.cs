using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIS.FReader.Entities;
using FIS.FReader.Utilities;
using System.Diagnostics;
using System.IO;

namespace FIS.FReader.Excute
{
    class TrialExecute
    {
        Trial trialReport;
        List<Trial> listTrialReport;
        Branch branch = null;
        GroupTrial groupTrial = null;
        TrialRow trialRow = null;
        internal List<Trial> ListTrialReport
        {
            get { return listTrialReport; }
            //set { listLoanReport = value; }
        }
        public Trial TrialReport
        {
            get { return trialReport; }
            //set { feeReport = value; }
        }
        /// <summary>
        /// get list report from list file input
        /// </summary>
        /// <param name="inputFile"> list input file by list string</param>
        /// <returns>List Load</returns>
        public List<Trial> GetReport(List<string> inputFile)
        {
            listTrialReport = new List<Trial>();
            for (int i = 0; i < inputFile.Count; i++)
            {
                listTrialReport.Add(GetReport(inputFile[i]));
            }
            return listTrialReport;
        }
        /// <summary>
        /// get report from a file by input file name
        /// </summary>
        /// <param name="inputFile"> file name </param>
        /// <returns>report feeReport</returns>
        public Trial GetReport(string inputFile)
        {
            trialReport = new Trial();
            var watch = Stopwatch.StartNew();
            trialReport.LoadFile(inputFile);
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
                    trialReport.FileRow = rowNumber;
                }
                ReadLine(line, rowNumber);
            }

            // the code that you want to measure comes here
            fileStream.Close();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            trialReport.TimeProcess = elapsedMs.ToString();
            return trialReport;
        }

        private void ReadLine(String inputString, int inputRowNumber)
        {
            if (inputString.Contains("+----------------------+----------------+----------------+----------------+----------------+"))
                return;
            if (inputString.Contains("|      Account No      |    Opening     |     Debit      |     Credit     |    Closing     |"))
                return;
            if (inputString.Contains("|                      |    balance     |                |                |    balance     |"))
                return;
            if (inputString.Contains("+----------------------+----------------+----------------+----------------+----------------+"))
                return;
            if (inputString.Contains("for period from"))
            {
                trialReport.ReportFromDate = inputString.Substring(42, 11).Trim();
                trialReport.ReportToDate = inputString.Substring(56, 12).Trim();
            }
            if (inputString.Contains("Branch"))
            {
                branch = new Branch();
                branch.BranchName = inputString.Replace("Branch ", "").Trim().ToString();
                //Console.WriteLine(branch.BranchName);
            }
            if (inputString.Contains("                    Curr "))
            {
                branch.Currency = inputString.Replace("Curr ", "").Trim().ToString();
                //Console.WriteLine(branch.Currency);
            }
            if (inputString.Contains("                    Acct "))
            {
                groupTrial = new GroupTrial();
                groupTrial.GroupName = inputString.Replace("Acct ", "").Trim().ToString();
                //Console.WriteLine(groupTrial.GroupName);
            }
            //=============================
            if (inputString.Split(Constant.Loan.ReportConstant.COLUMNSEPARATE).Length == 7)
            {
                if (!inputString.StartsWith("| "))
                {
                    trialRow = new TrialRow();
                    trialRow.AccountNo = inputString.Split(Constant.Loan.ReportConstant.COLUMNSEPARATE)[1].Trim();
                    trialRow.OpeningBalance = inputString.Split(Constant.Loan.ReportConstant.COLUMNSEPARATE)[2].Trim();
                    trialRow.Debit = inputString.Split(Constant.Loan.ReportConstant.COLUMNSEPARATE)[3].Trim();
                    trialRow.Credit = inputString.Split(Constant.Loan.ReportConstant.COLUMNSEPARATE)[4].Trim();
                    trialRow.ClosingBalance = inputString.Split(Constant.Loan.ReportConstant.COLUMNSEPARATE)[5].Trim();
                    groupTrial.ListTrialRow.Add(trialRow);
                }
                if (inputString.StartsWith("| Total for            |"))
                {
                    branch.ListGroupTrial.Add(groupTrial);
                }
                if (inputString.StartsWith("| Currency total "))
                {
                    trialReport.ListBranch.Add(branch);
                    //Console.WriteLine("asasasa");
                }

            }
            //===========================

        }
    }
}
