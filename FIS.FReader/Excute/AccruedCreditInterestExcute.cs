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
    class AccruedCreditInterestExcute
    {
        AccruedCreditInterest accruedReport;

        List<AccruedCreditInterest> listAccruedReport;


        ContractAccruedType accruedType = null;
        ContractAccrued accrued = null;


        internal List<AccruedCreditInterest> ListAccruedReport
        {
            get { return listAccruedReport; }
            set { listAccruedReport = value; }
        }


        public AccruedCreditInterest AccruedReport
        {
            get { return accruedReport; }
            //set { feeReport = value; }
        }

        /// <summary>
        /// get list report from list file input
        /// </summary>
        /// <param name="inputFile"> list input file by list string</param>
        /// <returns>List AccruedCreditInterest</returns>
        public List<AccruedCreditInterest> GetReport(List<string> inputFile)
        {
            listAccruedReport = new List<AccruedCreditInterest>();
            for (int i = 0; i < inputFile.Count; i++)
            {
                listAccruedReport.Add(GetReport(inputFile[i]));
            }
            return listAccruedReport;
        }

        /// <summary>
        /// get report from a file by input file name
        /// </summary>
        /// <param name="inputFile"> file name </param>
        /// <returns>report feeReport</returns>
        public AccruedCreditInterest GetReport(string inputFile)
        {
            accruedReport = new AccruedCreditInterest();
            var watch = Stopwatch.StartNew();
            accruedReport.LoadFile(inputFile);
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
                    accruedReport.FileRow = rowNumber;
                }
                ReadLine(line, rowNumber);
            }

            // the code that you want to measure comes here
            fileStream.Close();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            accruedReport.TimeProcess = elapsedMs.ToString();
            return accruedReport;
        }
        private String ConvertDate(string S)
        {
            DateTime dtReturn;
            dtReturn = DateTime.ParseExact(S, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            String ouput = dtReturn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            return (ouput);
        }
        private void ReadLine(String inputString, int inputRowNumber)
        {
            if (inputString.StartsWith(Constant.AccruedCreditInterest.ReportConstant.IGNORE_ROW_1))
            { return; }

            if (inputString.StartsWith(Constant.AccruedCreditInterest.ReportConstant.IGNORE_ROW_2))
            { return; }

            if (inputString.Contains(Constant.AccruedCreditInterest.ReportConstant.REPORTCREATIONTIME))
            {
                this.accruedReport.ReportCreationTime = inputString.Replace(Constant.AccruedCreditInterest.ReportConstant.REPORTCREATIONTIME, " ").Trim().ToString();
                this.accruedReport.ReportDate = ConvertDate(inputString.Replace(Constant.AccruedCreditInterest.ReportConstant.REPORTCREATIONTIME, " ").Trim().ToString()).ToString();
                return;
            }

            if (inputString.Contains(Constant.AccruedCreditInterest.ReportConstant.CURRENCY))
            {
                accruedType.Currency = inputString.Split(Constant.AccruedCreditInterest.ReportConstant.COLON)[1].Replace(Constant.AccruedCreditInterest.ReportConstant.COLUMNSEPARATE, Constant.AccruedCreditInterest.ReportConstant.SPACE).Trim();
                return;
            }

            //if (inputString.StartsWith(Constant.AccruedCreditInterest.ReportConstant.CONTRACTTYPE1) && !inputString.StartsWith(Constant.AccruedCreditInterest.ReportConstant.CONTRACTTYPETOTAL))
            //{
            //    accruedType = new ContractAccruedType();
            //    try
            //    {
            //    //    accruedType.PreFix = inputString.Split(new[] { Constant.AccruedCreditInterest.ReportConstant.CONTRACTTYPE }, StringSplitOptions.None)[0].Replace(Constant.AccruedCreditInterest.ReportConstant.COLUMNSEPARATE, Constant.AccruedCreditInterest.ReportConstant.SPACE).Trim();
            //    //    if (inputString.Split(new[] { Constant.AccruedCreditInterest.ReportConstant.CONTRACTTYPE }, StringSplitOptions.None)[1] == null)
            //    //    {
            //    //        accruedType.ConType = string.Empty;
            //    //    }
            //    //    else
            //    //    { accruedType.ConType = inputString.Split(new[] { Constant.AccruedCreditInterest.ReportConstant.CONTRACTTYPE }, StringSplitOptions.None)[1].Replace(Constant.AccruedCreditInterest.ReportConstant.SEPARATEDASH, Constant.AccruedCreditInterest.ReportConstant.SPACE).Replace(Constant.AccruedCreditInterest.ReportConstant.COLUMNSEPARATE, Constant.AccruedCreditInterest.ReportConstant.SPACE).Trim(); }
            //    }
            //    catch (Exception)
            //    {
            //        accruedType.PreFix = inputString;
            //    }
            //    return;
            //}

            if (inputString.Contains("Contract type:"))
            {
                accruedType = new ContractAccruedType();
                accruedType.ConType = inputString.Split(Constant.AccruedCreditInterest.ReportConstant.COLON)[1].Replace(Constant.AccruedCreditInterest.ReportConstant.COLUMNSEPARATE, Constant.AccruedCreditInterest.ReportConstant.SPACE).Trim();
                return;
            }


            if (inputString.StartsWith(Constant.AccruedCreditInterest.ReportConstant.CURRENCYTOTAL))
            {
                accruedReport.ListContractType.Add(accruedType);
                return;
            }

            if (inputString.Split(Constant.AccruedCreditInterest.ReportConstant.COLUMNSEPARATE).Length == 5)
            {
                if (!String.IsNullOrEmpty(inputString.Split(Constant.AccruedCreditInterest.ReportConstant.COLUMNSEPARATE)[1].Trim()) || !String.IsNullOrEmpty(inputString.Split(Constant.AccruedCreditInterest.ReportConstant.COLUMNSEPARATE)[2].Trim()))
                {
                    accrued = new ContractAccrued();
                    accrued.ContractNo = inputString.Split(Constant.AccruedCreditInterest.ReportConstant.COLUMNSEPARATE)[1].Trim();
                    accrued.ClientName = inputString.Split(Constant.AccruedCreditInterest.ReportConstant.COLUMNSEPARATE)[2].Trim();
                    accrued.Interests = inputString.Split(Constant.AccruedCreditInterest.ReportConstant.COLUMNSEPARATE)[3].Trim();
                    accrued.RowNumber = inputRowNumber;
                    accruedType.ListContract.Add(accrued);
                }
                return;
            }
        }

    }
}
