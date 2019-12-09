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
    class ContractOverduePaymentExcute
    {
        ContractOverduePayment contractReport;

        List<ContractOverduePayment> lstContractReport;

        public List<ContractOverduePayment> LstContractReport
        {
            get { return lstContractReport; }
            //set { lstContractReport = value; }
        }

        public ContractOverduePayment ContractReport
        {
            get { return contractReport; }
            // set { contractReport = value; }
        }

        ContractOverType contractType = null;
        ContractOver contract = null;
        Boolean inContractT = false;
        int endBlockCheck = 0;

        /// <summary>
        /// get list report from list file input
        /// </summary>
        /// <param name="inputFile"> list input file by list string</param>
        /// <returns>List ContractOverduePayment</returns>
        public List<ContractOverduePayment> GetReport(List<string> inputFile)
        {
            lstContractReport = new List<ContractOverduePayment>();
            for (int i = 0; i < inputFile.Count; i++)
            {
                lstContractReport.Add(GetReport(inputFile[i]));
            }
            return lstContractReport;
        }

        /// <summary>
        /// get report from a file by input file name
        /// </summary>
        /// <param name="inputFile"> file name </param>
        /// <returns>report ContractOverduePayment</returns>
        public ContractOverduePayment GetReport(string inputFile)
        {
            contractReport = new ContractOverduePayment();
            var watch = Stopwatch.StartNew();
            contractReport.LoadFile(inputFile);
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
                    contractReport.FileRow = rowNumber;
                }
                ReadLine(line, rowNumber);
            }

            // the code that you want to measure comes here
            fileStream.Close();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            contractReport.TimeProcess = elapsedMs.ToString();
            return contractReport;
        }

        private void ReadLine(String inputString, int inputRowNumber)
        {
            if (inputString.Contains(Constant.ContractOverduePayment.ReportConstant.REPORTDATE))
            {
                this.contractReport.ReportDate = inputString.Split(':')[1].Trim();
            }

            if (inputString.Contains(Constant.ContractOverduePayment.ReportConstant.REPORTCREATIONTIME))
            {
                this.contractReport.ReportCreationTime = inputString.Replace(Constant.ContractOverduePayment.ReportConstant.REPORTCREATIONTIME, " ").Trim();
            }

            if (inputString.Split(Constant.ContractOverduePayment.ReportConstant.COLUMNSEPARATE).Length == 3)
            {
                 if (!inContractT && !inputString.Contains(Constant.ContractOverduePayment.ReportConstant.ROWSEPARATE))
                {
                    contractType = new ContractOverType();
                    //try
                    //{
                        //contractType.PreFix = inputString.Split(new[] { Constant.ContractOverduePayment.ReportConstant.CONTRACTTYPE }, StringSplitOptions.None)[0].Replace(Constant.ContractOverduePayment.ReportConstant.COLUMNSEPARATE, Constant.ContractOverduePayment.ReportConstant.SPACE).Trim();
                        //if (inputString.Split(new[] { Constant.ContractOverduePayment.ReportConstant.CONTRACTTYPE }, StringSplitOptions.None)[1] == null)
                        //{
                        contractType.ConType = inputString.Split('|')[1].Trim();
                        //contractReport.ListContractType.Add(contractType);
                        //}
                        //else
                        //{ contractType.ConType = inputString.Split(new[] { Constant.ContractOverduePayment.ReportConstant.CONTRACTTYPE }, StringSplitOptions.None)[1].Replace(Constant.ContractOverduePayment.ReportConstant.SEPARATEDASH, Constant.ContractOverduePayment.ReportConstant.SPACE).Replace(Constant.ContractOverduePayment.ReportConstant.COLUMNSEPARATE, Constant.ContractOverduePayment.ReportConstant.SPACE).Trim(); }
                   
                    
                    //}
                    //catch (Exception)
                    //{
                    //    contractType.PreFix = inputString;
                    //}
                }
            }

            if (inputString.StartsWith(Constant.ContractOverduePayment.ReportConstant.ROWSEPARATE))
            {
                endBlockCheck++;
            }

            if (inputString.StartsWith(Constant.ContractOverduePayment.ReportConstant.COLUMNSEPARATE.ToString()) && endBlockCheck == 2)
            {
                endBlockCheck = 0;
                ContractReport.ListContractType.Add(contractType);
            }

            if (inputString.Split(Constant.ContractOverduePayment.ReportConstant.COLUMNSEPARATE).Length == 7)
            {
                if ((!String.IsNullOrEmpty(inputString.Split(Constant.ContractOverduePayment.ReportConstant.COLUMNSEPARATE)[1].Trim()) || !String.IsNullOrEmpty(inputString.Split(Constant.ContractOverduePayment.ReportConstant.COLUMNSEPARATE)[2].Trim())) && !(inputString.Contains(Constant.ContractOverduePayment.ReportConstant.TABLEHEADER)))
                {
                    contract = new ContractOver();
                    contract.ContractNo = inputString.Split(Constant.ContractOverduePayment.ReportConstant.COLUMNSEPARATE)[1].Trim();
                    contract.Account = inputString.Split(Constant.ContractOverduePayment.ReportConstant.COLUMNSEPARATE)[2].Trim();
                    contract.Currency = inputString.Split(Constant.ContractOverduePayment.ReportConstant.COLUMNSEPARATE)[3].Trim();
                    contract.MinPaymentAcc = inputString.Split(Constant.ContractOverduePayment.ReportConstant.COLUMNSEPARATE)[4].Trim();
                    contract.DuePeriod = inputString.Split(Constant.ContractOverduePayment.ReportConstant.COLUMNSEPARATE)[5].Trim();
                    contract.RowNumber = inputRowNumber;
                    contractType.ListContract.Add(contract);
                }
            }
        }

    }
}
