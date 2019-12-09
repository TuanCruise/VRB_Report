using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIS.FReader.Entities;
using System.Diagnostics;
using System.IO;
using FIS.FReader.Utilities;

namespace FIS.FReader.Excute
{
    class DebtAutomationExcute
    {
        DebtAutomation debtReport;

        List<DebtAutomation> listDebtReport;

        public DebtAutomation DebtReport
        {
            get { return debtReport; }
            //set { debtReport = value; }
        }

        /// <summary>
        /// get list report from list file input
        /// </summary>
        /// <param name="inputFile"> list input file by list string</param>
        /// <returns>List DebtAutomation</returns>
        public List<DebtAutomation> GetReport(List<string> inputFile)
        {
            listDebtReport = new List<DebtAutomation>();
            for (int i = 0; i < inputFile.Count; i++)
            {
                listDebtReport.Add(GetReport(inputFile[i]));
            }
            return listDebtReport;
        }

        /// <summary>
        /// get report from a file by input file name
        /// </summary>
        /// <param name="inputFile"> file name </param>
        /// <returns>report debtReport</returns>
        public DebtAutomation GetReport(string inputFile)
        {
            debtReport = new DebtAutomation();
            var watch = Stopwatch.StartNew();
            debtReport.LoadFile(inputFile);
            const Int32 BufferSize = 128;
            var fileStream = File.OpenRead(inputFile);
            var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize);
            String line;
            List<string> ltsString = new List<string>();
            int rowNumber = 0;
            while ((line = streamReader.ReadLine()) != null)
            {
                if(streamReader.EndOfStream)
                {
                    debtReport.FileRow = rowNumber;
                }
                else
                {
                    rowNumber++;
                    ReadLine(line, rowNumber);
                }
                
            }

            // the code that you want to measure comes here
            fileStream.Close();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            debtReport.TimeProcess = elapsedMs.ToString();
            return debtReport;
        }

        private void ReadLine(String inputString, int inputRowNumber)
        {
            Debt debt = new Debt();
            debt.CardAccountNumber = inputString.Substring(Constant.DebtAutomation.ReportConstant.COLUMN_1_START, Constant.DebtAutomation.ReportConstant.COLUMN_1_LENGTH).Trim();
            debt.EmbossingName = inputString.Substring(Constant.DebtAutomation.ReportConstant.COLUMN_2_START, Constant.DebtAutomation.ReportConstant.COLUMN_2_LENGTH).Trim();
            debt.BranchPart = inputString.Substring(Constant.DebtAutomation.ReportConstant.COLUMN_3_START, Constant.DebtAutomation.ReportConstant.COLUMN_3_LENGTH).Trim();
            debt.BankAccountNumber = inputString.Substring(Constant.DebtAutomation.ReportConstant.COLUMN_4_START, Constant.DebtAutomation.ReportConstant.COLUMN_4_LENGTH).Trim();
            debt.DebtCreditFlag = inputString.Substring(Constant.DebtAutomation.ReportConstant.COLUMN_5_START, Constant.DebtAutomation.ReportConstant.COLUMN_5_LENGTH).Trim();
            debt.CardAccountCurrency = inputString.Substring(Constant.DebtAutomation.ReportConstant.COLUMN_6_START, Constant.DebtAutomation.ReportConstant.COLUMN_6_LENGTH).Trim();
            debt.Amount = inputString.Substring(Constant.DebtAutomation.ReportConstant.COLUMN_7_START, Constant.DebtAutomation.ReportConstant.COLUMN_7_LENGTH).Trim();
            debt.NumOfDecPlace = inputString.Substring(Constant.DebtAutomation.ReportConstant.COLUMN_8_START, Constant.DebtAutomation.ReportConstant.COLUMN_8_LENGTH).Trim();
            debt.PostingDate = inputString.Substring(Constant.DebtAutomation.ReportConstant.COLUMN_9_START, Constant.DebtAutomation.ReportConstant.COLUMN_9_LENGTH).Trim();
            debt.AccountNumber = inputString.Substring(Constant.DebtAutomation.ReportConstant.COLUMN_10_START, inputString.Length - Constant.DebtAutomation.ReportConstant.COLUMN_10_START).Trim();
            debt.RowNumber = inputRowNumber;
            debtReport.ListDebt.Add(debt);
        }
    
    }
}
