using FIS.FReader.Entities;
using FIS.FReader.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FIS.FReader.Excute
{
    class FeeOverduePaymentExcute
    {
        FeeOverduePayment feeReport;
        List<FeeOverduePayment> listFeeReport;

        FeeContractType contractType = new FeeContractType();
        FeeContract contract = null;
        Boolean inContractT = false;

        public FeeOverduePayment FeeReport
        {
            get { return feeReport; }
            //set { feeReport = value; }
        }

        /// <summary>
        /// get list report from list file input
        /// </summary>
        /// <param name="inputFile"> list input file by list string</param>
        /// <returns>List FeeOverduePayment</returns>
        public List<FeeOverduePayment> GetReport(List<string> inputFile)
        {
            listFeeReport = new List<FeeOverduePayment>();
            for (int i = 0; i < inputFile.Count; i++)
            {
                listFeeReport.Add(GetReport(inputFile[i]));
            }
            return listFeeReport;
        }

        /// <summary>
        /// get report from a file by input file name
        /// </summary>
        /// <param name="inputFile"> file name </param>
        /// <returns>report feeReport</returns>
        public FeeOverduePayment GetReport(string inputFile)
        {
            feeReport = new FeeOverduePayment();
            var watch = Stopwatch.StartNew();
            feeReport.LoadFile(inputFile);
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
                    feeReport.FileRow = rowNumber;
                }
                ReadLine(line, rowNumber);
            }

            // the code that you want to measure comes here
            fileStream.Close();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            feeReport.TimeProcess = elapsedMs.ToString();
            return feeReport;
        }

        private void ReadLine(String inputString, int inputRowNumber)
        {
            try
            {
                if (inputString.Contains(Constant.FeeOverduePayment.ReportConstant.REPORTDATE))
                {
                    this.feeReport.ReportDate = inputString.Split(Constant.FeeOverduePayment.ReportConstant.COLON)[1].Trim();
                }

                if (inputString.Contains(Constant.FeeOverduePayment.ReportConstant.REPORTCREATIONTIME))
                {
                    this.feeReport.ReportCreationTime = inputString.Replace(Constant.FeeOverduePayment.ReportConstant.REPORTCREATIONTIME, " ").Trim();
                }

                if (inputString.Split(Constant.FeeOverduePayment.ReportConstant.COLUMNSEPARATE).Length == 3)
                {
                    if (inputString.Contains(Constant.FeeOverduePayment.ReportConstant.CURRENCY))
                    {
                        contractType.Currency = inputString.Split(Constant.FeeOverduePayment.ReportConstant.COLON)[1].Replace(Constant.FeeOverduePayment.ReportConstant.COLUMNSEPARATE, Constant.FeeOverduePayment.ReportConstant.SPACE).Trim();
                    }
                    //else if (!inContractT && !inputString.Contains(Constant.FeeOverduePayment.ReportConstant.ROWSEPARATE))
                    //{
                    //    contractType = new FeeContractType();
                    //    try
                    //    {
                    //        contractType.PreFix = inputString.Split(new[] { Constant.FeeOverduePayment.ReportConstant.CONTRACTTYPE }, StringSplitOptions.None)[0].Replace(Constant.FeeOverduePayment.ReportConstant.COLUMNSEPARATE, Constant.FeeOverduePayment.ReportConstant.SPACE).Trim();
                    //        if (inputString.Split(new[] { Constant.FeeOverduePayment.ReportConstant.CONTRACTTYPE }, StringSplitOptions.None)[1] == null)
                    //        {
                    //            contractType.ConType = string.Empty;
                    //        }
                    //        else
                    //        { contractType.ConType = inputString.Split(new[] { Constant.FeeOverduePayment.ReportConstant.CONTRACTTYPE }, StringSplitOptions.None)[1].Replace(Constant.FeeOverduePayment.ReportConstant.SEPARATEDASH, Constant.FeeOverduePayment.ReportConstant.SPACE).Replace(Constant.FeeOverduePayment.ReportConstant.COLUMNSEPARATE, Constant.FeeOverduePayment.ReportConstant.SPACE).Trim(); }
                    //    }
                    //    catch (Exception)
                    //    {
                    //        contractType.PreFix = inputString;
                    //    }
                    //}
                    else if (!inputString.Contains("Total                                    ") && inputString.Contains("Contract Type"))
                    {
                        contractType = new FeeContractType();
                        contractType.ConType = inputString.Split(Constant.FeeOverduePayment.ReportConstant.COLUMNSEPARATE)[1].Trim();
                    }

                }

                if (inputString.StartsWith(Constant.FeeOverduePayment.ReportConstant.TOTAL))
                {
                    feeReport.ListContractType.Add(contractType);
                }

                if (inputString.Split(Constant.FeeOverduePayment.ReportConstant.COLUMNSEPARATE).Length == 6)
                {
                    if (!String.IsNullOrEmpty(inputString.Split(Constant.FeeOverduePayment.ReportConstant.COLUMNSEPARATE)[1].Trim()) || !String.IsNullOrEmpty(inputString.Split(Constant.FeeOverduePayment.ReportConstant.COLUMNSEPARATE)[2].Trim()))
                    {
                        contract = new FeeContract();
                        contract.ContractNo = inputString.Split(Constant.FeeOverduePayment.ReportConstant.COLUMNSEPARATE)[1].Trim();
                        contract.Account = inputString.Split(Constant.FeeOverduePayment.ReportConstant.COLUMNSEPARATE)[2].Trim();
                        contract.Due = inputString.Split(Constant.FeeOverduePayment.ReportConstant.COLUMNSEPARATE)[3].Trim();
                        contract.Paid = inputString.Split(Constant.FeeOverduePayment.ReportConstant.COLUMNSEPARATE)[4].Trim();
                        contract.RowNumber = inputRowNumber;
                        try
                        {
                            var bug = Double.Parse(contract.Paid);
                            contractType.ListContract.Add(contract);
                        }
                        catch
                        {

                        }
                    }
                }
            }
            catch
            {
                //throw ex;
            }
        }

    }
}
