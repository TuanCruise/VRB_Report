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
    class VisaExecute
    {
        Visa visaReport;
        Report report = null;
        Report_110 report_110 = null;
        Report_120 report_120 = null;
        Report_130 report_130 = null;
        Report_140 report_140 = null;
        Report_210 report_210 = null;
        Report_900 report_900 = null;
        StreamReader streamReader;
        int flag = 0;
        int flagGroup120 = 0;
        int flagGroup130 = 0;
        int flagGroup140 = 0;
        int flagGroup210 = 0;
        List<Visa> listVisaReport;

        internal List<Visa> ListVisaReport
        {
            get { return listVisaReport; }
            //set { listLoanReport = value; }
        }

        /// <summary>
        /// get list report from list file input
        /// </summary>
        /// <param name="inputFile"> list input file by list string</param>
        /// <returns>List FeeOverduePayment</returns>
        public List<Visa> GetReport(List<string> inputFile)
        {
            listVisaReport = new List<Visa>();
            for (int i = 0; i < inputFile.Count; i++)
            {
                listVisaReport.Add(GetReport(inputFile[i]));
            }
            return listVisaReport;
        }


        /// <summary>
        /// get report from a file by input file name
        /// </summary>
        /// <param name="inputFile"> file name </param>
        /// <returns>report feeReport</returns>
        public Visa GetReport(string inputFile)
        {
            visaReport = new Visa();
            var watch = Stopwatch.StartNew();
            visaReport.LoadFile(inputFile);
            const Int32 BufferSize = 128;
            var fileStream = File.OpenRead(inputFile);
            streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize);
            String line;
            List<string> ltsString = new List<string>();
            int rowNumber = 0;
            while ((line = streamReader.ReadLine()) != null)
            {
                rowNumber++;
                if (streamReader.EndOfStream)
                {
                    visaReport.FileRow = rowNumber;
                }
                ReadLine(line, rowNumber);
            }

            // the code that you want to measure comes here
            fileStream.Close();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            visaReport.TimeProcess = elapsedMs.ToString();
            return visaReport;
        }

        public Visa VisaReport
        {
            get { return visaReport; }
            //set { feeReport = value; }
        }
        private String ConvertDate(string S)
        {
            DateTime dtReturn;
            dtReturn = DateTime.ParseExact(S, "ddMMMyy", CultureInfo.InvariantCulture);
            String ouput = dtReturn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            return (ouput);
        }
        private void ReadLine(String inputString, int inputRowNumber)
        {
            #region Ignore
            if (inputString.StartsWith(Constant.Visa.ReportConstant.IGNORE_ROW_1))
                return;
            if (inputString.StartsWith(Constant.Visa.ReportConstant.IGNORE_ROW_2))
                return;
            if (inputString.StartsWith(Constant.Visa.ReportConstant.IGNORE_ROW_3))
                return;
            if (inputString.StartsWith(Constant.Visa.ReportConstant.IGNORE_ROW_4))
                return;
            if (inputString.StartsWith(Constant.Visa.ReportConstant.IGNORE_ROW_5))
                return;
            if (inputString.StartsWith(Constant.Visa.ReportConstant.IGNORE_ROW_6))
                return;
            if (inputString.StartsWith(Constant.Visa.ReportConstant.IGNORE_ROW_7))
                return;
            if (inputString.StartsWith(Constant.Visa.ReportConstant.IGNORE_ROW_8))
                return;
            if (inputString.StartsWith(Constant.Visa.ReportConstant.IGNORE_ROW_9))
                return;
            if (inputString.StartsWith(Constant.Visa.ReportConstant.IGNORE_ROW_10))
                return;
            if (inputString.StartsWith(Constant.Visa.ReportConstant.IGNORE_ROW_11))
                return;
            if (inputString.StartsWith(Constant.Visa.ReportConstant.IGNORE_ROW_12))
                return;
            #endregion
            #region ReportID
            if (inputString.Contains(Constant.Visa.ReportConstant.REPORT_110_START))
            {
                report_110 = new Report_110();
                flag = 110;
                report_110.ReportID = (inputString.Split(' ')[3].Trim().ToString());
            }
            if (inputString.Contains(Constant.Visa.ReportConstant.REPORT_120_START))
            {
                report_120 = new Report_120();
                flag = 120;
                report_120.ReportID = (inputString.Split(' ')[3].Trim().ToString());
            }
            if (inputString.Contains(Constant.Visa.ReportConstant.REPORT_130_START))
            {
                report_130 = new Report_130();
                flag = 130;
                report_130.ReportID = (inputString.Split(' ')[3].Trim().ToString());
            }
            if (inputString.Contains(Constant.Visa.ReportConstant.REPORT_140_START))
            {
                report_140 = new Report_140();
                flag = 140;
                report_140.ReportID = (inputString.Split(' ')[3].Trim().ToString());
            }
            if (inputString.Contains(Constant.Visa.ReportConstant.REPORT_210_START))
            {
                report_210 = new Report_210();
                flag = 210;
                report_210.ReportID = (inputString.Split(' ')[3].Trim().ToString());
            }
            if (inputString.Contains(Constant.Visa.ReportConstant.REPORT_900_START))
            {
                report_900 = new Report_900();
                flag = 900;
                report_900.ReportID = (inputString.Split(' ')[3].Trim().ToString());
            }
            #endregion
            #region ReportingFor
            if (inputString.Contains("REPORTING FOR"))
            {
                if (flag == 110)
                {
                    report_110.ReportingFor = inputString.Substring(Constant.Visa.ReportConstant.REPORTING_FOR_START, Constant.Visa.ReportConstant.REPORTING_FOR_LENGTH).Trim();
                    report_110.ProcDate = ConvertDate(inputString.Substring(Constant.Visa.ReportConstant.PROC_DATE_START, Constant.Visa.ReportConstant.PROC_DATE_LENGTH).Trim()).ToString();
                } if (flag == 120)
                {
                    report_120.ReportingFor = inputString.Substring(Constant.Visa.ReportConstant.REPORTING_FOR_START, Constant.Visa.ReportConstant.REPORTING_FOR_LENGTH).Trim();
                    report_120.ProcDate = ConvertDate(inputString.Substring(Constant.Visa.ReportConstant.PROC_DATE_START, Constant.Visa.ReportConstant.PROC_DATE_LENGTH).Trim()).ToString();
                }
                if (flag == 130)
                {
                    report_130.ReportingFor = inputString.Substring(Constant.Visa.ReportConstant.REPORTING_FOR_START, Constant.Visa.ReportConstant.REPORTING_FOR_LENGTH).Trim();
                    report_130.ProcDate = ConvertDate(inputString.Substring(Constant.Visa.ReportConstant.PROC_DATE_START, Constant.Visa.ReportConstant.PROC_DATE_LENGTH).Trim()).ToString();
                }
                if (flag == 140)
                {
                    report_140.ReportingFor = inputString.Substring(Constant.Visa.ReportConstant.REPORTING_FOR_START, Constant.Visa.ReportConstant.REPORTING_FOR_LENGTH).Trim();
                    report_140.ProcDate = ConvertDate(inputString.Substring(Constant.Visa.ReportConstant.PROC_DATE_START, Constant.Visa.ReportConstant.PROC_DATE_LENGTH).Trim()).ToString();
                }
                if (flag == 210)
                {
                    report_210.ReportingFor = inputString.Substring(Constant.Visa.ReportConstant.REPORTING_FOR_START, Constant.Visa.ReportConstant.REPORTING_FOR_LENGTH).Trim();
                    report_210.ProcDate =ConvertDate(inputString.Substring(Constant.Visa.ReportConstant.PROC_DATE_START, Constant.Visa.ReportConstant.PROC_DATE_LENGTH).Trim()).ToString();
                }
                if (flag == 900)
                {
                    report_900.ReportingFor = inputString.Substring(Constant.Visa.ReportConstant.REPORTING_FOR_START, Constant.Visa.ReportConstant.REPORTING_FOR_LENGTH).Trim();
                    report_900.ProcDate =ConvertDate(inputString.Substring(Constant.Visa.ReportConstant.PROC_DATE_START, Constant.Visa.ReportConstant.PROC_DATE_LENGTH).Trim()).ToString();
                }

            }
            #endregion
            #region RollUp
            if (inputString.Contains("ROLLUP TO") || inputString.Contains("REPORT DATE"))
            {

                if (flag == 110)
                {
                    report_110.RollUp = inputString.Substring(Constant.Visa.ReportConstant.ROLLUP_TO_START, Constant.Visa.ReportConstant.ROLLUP_TO_LENGTH).Trim();
                    report_110.ReportDate =ConvertDate(inputString.Substring(Constant.Visa.ReportConstant.REPORT_DATE_START, Constant.Visa.ReportConstant.REPORT_DATE_LENGTH).Trim()).ToString();
                }
                if (flag == 120)
                {
                    report_120.RollUp = inputString.Substring(Constant.Visa.ReportConstant.ROLLUP_TO_START, Constant.Visa.ReportConstant.ROLLUP_TO_LENGTH).Trim();
                    report_120.ReportDate = ConvertDate(inputString.Substring(Constant.Visa.ReportConstant.REPORT_DATE_START, Constant.Visa.ReportConstant.REPORT_DATE_LENGTH).Trim()).ToString();
                }
                if (flag == 130)
                {
                    report_130.RollUp = inputString.Substring(Constant.Visa.ReportConstant.ROLLUP_TO_START, Constant.Visa.ReportConstant.ROLLUP_TO_LENGTH).Trim();
                    report_130.ReportDate = ConvertDate(inputString.Substring(Constant.Visa.ReportConstant.REPORT_DATE_START, Constant.Visa.ReportConstant.REPORT_DATE_LENGTH).Trim()).ToString();
                }
                if (flag == 140)
                {
                    report_140.RollUp = inputString.Substring(Constant.Visa.ReportConstant.ROLLUP_TO_START, Constant.Visa.ReportConstant.ROLLUP_TO_LENGTH).Trim();
                    report_140.ReportDate = ConvertDate(inputString.Substring(Constant.Visa.ReportConstant.REPORT_DATE_START, Constant.Visa.ReportConstant.REPORT_DATE_LENGTH).Trim()).ToString();
                }
                if (flag == 210)
                {
                    report_210.RollUp = inputString.Substring(Constant.Visa.ReportConstant.ROLLUP_TO_START, Constant.Visa.ReportConstant.ROLLUP_TO_LENGTH).Trim();
                    report_210.ReportDate = ConvertDate(inputString.Substring(Constant.Visa.ReportConstant.REPORT_DATE_START, Constant.Visa.ReportConstant.REPORT_DATE_LENGTH).Trim()).ToString();
                }
                if (flag == 900)
                {
                    report_900.RollUp = inputString.Substring(Constant.Visa.ReportConstant.ROLLUP_TO_START, Constant.Visa.ReportConstant.ROLLUP_TO_LENGTH).Trim();
                    report_900.ReportDate =ConvertDate(inputString.Substring(Constant.Visa.ReportConstant.REPORT_DATE_START, Constant.Visa.ReportConstant.REPORT_DATE_LENGTH).Trim()).ToString();
                }
            }
            #endregion
            #region Funds
            if (inputString.Contains("FUNDS XFER ENTITY:"))
            {
                if (flag == 110)
                {
                    report_110.Funds = inputString.Substring(Constant.Visa.ReportConstant.FUNDS_START, Constant.Visa.ReportConstant.FUNDS_LENGTH).Trim();
                }
                if (flag == 120)
                {
                    report_120.Funds = inputString.Substring(Constant.Visa.ReportConstant.FUNDS_START, Constant.Visa.ReportConstant.FUNDS_LENGTH).Trim();
                }
                if (flag == 130)
                {
                    report_130.Funds = inputString.Substring(Constant.Visa.ReportConstant.FUNDS_START, Constant.Visa.ReportConstant.FUNDS_LENGTH).Trim();
                }
                if (flag == 140)
                {
                    report_140.Funds = inputString.Substring(Constant.Visa.ReportConstant.FUNDS_START, Constant.Visa.ReportConstant.FUNDS_LENGTH).Trim();
                }
                if (flag == 210)
                {
                    report_210.Funds = inputString.Substring(Constant.Visa.ReportConstant.FUNDS_START, Constant.Visa.ReportConstant.FUNDS_LENGTH).Trim();
                }
                if (flag == 900)
                {
                    report_900.Funds = inputString.Substring(Constant.Visa.ReportConstant.FUNDS_START, Constant.Visa.ReportConstant.FUNDS_LENGTH).Trim();
                }
            }
            #endregion
            #region SETTLEMENT
            if (inputString.Contains("SETTLEMENT CURRENCY:"))
            {
                if (flag == 110)
                {
                    report_110.SettlementCurrency = inputString.Substring(Constant.Visa.ReportConstant.SETTLEMENT_CURRENCTY_START, Constant.Visa.ReportConstant.SETTLEMENT_CURRENCTY_LENGTH).Trim();
                }
                if (flag == 120)
                {
                    report_120.SettlementCurrency = inputString.Substring(Constant.Visa.ReportConstant.SETTLEMENT_CURRENCTY_START, Constant.Visa.ReportConstant.SETTLEMENT_CURRENCTY_LENGTH).Trim();
                }
                if (flag == 130)
                {
                    report_130.SettlementCurrency = inputString.Substring(Constant.Visa.ReportConstant.SETTLEMENT_CURRENCTY_START, Constant.Visa.ReportConstant.SETTLEMENT_CURRENCTY_LENGTH).Trim();
                }
                if (flag == 140)
                {
                    report_140.SettlementCurrency = inputString.Substring(Constant.Visa.ReportConstant.SETTLEMENT_CURRENCTY_START, Constant.Visa.ReportConstant.SETTLEMENT_CURRENCTY_LENGTH).Trim();
                }
                if (flag == 210)
                {
                    report_210.SettlementCurrency = inputString.Substring(Constant.Visa.ReportConstant.SETTLEMENT_CURRENCTY_START, Constant.Visa.ReportConstant.SETTLEMENT_CURRENCTY_LENGTH).Trim();
                }
            }
            #endregion
            #region CLEARING CURRENCY
            if (inputString.Contains("CLEARING CURRENCY:"))
            {
                if (flag == 120)
                {
                    report_120.ClearingCurrency = inputString.Substring(Constant.Visa.ReportConstant.CLEARING_CURRENCTY_START, Constant.Visa.ReportConstant.CLEARING_CURRENCTY_LENGTH).Trim();
                }
                if (flag == 210)
                {
                    report_210.ClearingCurrency = inputString.Substring(Constant.Visa.ReportConstant.CLEARING_CURRENCTY_START, Constant.Visa.ReportConstant.CLEARING_CURRENCTY_LENGTH).Trim();
                }
                if (flag == 900)
                {
                    report_900.ClearingCurrency = inputString.Substring(Constant.Visa.ReportConstant.CLEARING_CURRENCTY_START, Constant.Visa.ReportConstant.CLEARING_CURRENCTY_LENGTH).Trim();
                }
            }
            #endregion

            ReadRp110(report_110, inputString);
            ReadRp120(report_120, inputString);
            ReadRp130(report_130, inputString);
            ReadRp140(report_140, inputString);
            ReadRp210(report_210, inputString);
            #region End
            if (inputString.Contains(Constant.Visa.ReportConstant.REPORT_110_END))
            {
                visaReport.ListReport_110.Add(report_110);

            }
            if (inputString.Contains(Constant.Visa.ReportConstant.REPORT_120_END))
            {
                visaReport.ListReport_120.Add(report_120);

            }
            if (inputString.Contains(Constant.Visa.ReportConstant.REPORT_130_END))
            {

                visaReport.ListReport_130.Add(report_130);

            }
            if (inputString.Contains(Constant.Visa.ReportConstant.REPORT_140_END))
            {

                visaReport.ListReport_140.Add(report_140);

            }
            if (inputString.Contains(Constant.Visa.ReportConstant.REPORT_210_END))
            {

                visaReport.ListReport_210.Add(report_210);

            }
            if (inputString.Contains(Constant.Visa.ReportConstant.REPORT_900_END))
            {

                visaReport.ListReport.Add(report_900);

            }
            #endregion
        }
        private void ReadRp110(Report_110 report_110, String inputString)
        {
            if (inputString.StartsWith("    NET SETTLEMENT AMOUNT                               "))
            {
                if (flag == 110)
                {
                    Detail detail = new Detail();
                    detail.Colum1 = inputString.Substring(1, 37).Trim();
                    detail.Colum2 = inputString.Substring(45, 20).Trim();
                    detail.Colum3 = inputString.Substring(65, 20).Trim();
                    detail.Colum4 = inputString.Substring(89, 20).Trim();
                    detail.Colum5 = inputString.Substring(117, 15).Trim();
                    report_110.ListDetail.Add(detail);
                }
            }
        }
        private void ReadRp120(Report_120 report_120, String inputString)
        {
            Group group = null;

            if (inputString.StartsWith(" ACQUIRER TRANSACTIONS                          "))
            {
                if (flag == 120)
                {
                    //acquirer = new Acquirer();
                    //group = new Group();
                    //group.GroupName = "ACQUIRER TRANSACTIONS";
                    flagGroup120 = 1;
                }

            }
            if (inputString.StartsWith(" ISSUER TRANSACTIONS                    "))
            {
                if (flag == 120)
                {
                    //issuser = new Issuser();
                    //group = new Group();
                    //group.GroupName = "ISSUER TRANSACTIONS";
                    flagGroup120 = 2;
                }

            }

            if (inputString.Contains("NET   "))
            {
                if (flag == 120)
                {
                    if (flagGroup120 == 0)
                    {
                        return;
                    }
                    if (flagGroup120 == 1)
                    {
                        group = new Group();
                        group.GroupName = "ACQUIRER TRANSACTIONS";
                        Detail detail = new Detail();
                        detail.Colum1 = inputString.Substring(1, 37).Trim();
                        detail.Colum2 = inputString.Substring(38, 15).Trim();
                        detail.Colum3 = inputString.Substring(57, 15).Trim();
                        detail.Colum4 = inputString.Substring(73, 24).Trim();
                        detail.Colum5 = inputString.Substring(97, 20).Trim();
                        detail.Colum6 = inputString.Substring(120, 12).Trim();
                        group.ListDetail.Add(detail);
                        report_120.ListGroup.Add(group);
                    } if (flagGroup120 == 2)
                    {
                        group = new Group();
                        group.GroupName = "ISSUER TRANSACTIONS";
                        Detail detail = new Detail();
                        detail.Colum1 = inputString.Substring(1, 37).Trim();
                        detail.Colum2 = inputString.Substring(38, 15).Trim();
                        detail.Colum3 = inputString.Substring(57, 15).Trim();
                        detail.Colum4 = inputString.Substring(73, 24).Trim();
                        detail.Colum5 = inputString.Substring(97, 20).Trim();
                        detail.Colum6 = inputString.Substring(120, 12).Trim();
                        group.ListDetail.Add(detail);
                        report_120.ListGroup.Add(group);
                    }

                }

            }
        }
        private void ReadRp130(Report_130 report_130, String inputString)
        {
            Group group = null;
            if (inputString.StartsWith(" ACQUIRER TRANSACTIONS                          "))
            {
                if (flag == 130)
                {
                    flagGroup130 = 1;
                }

            }
            if (inputString.StartsWith(" ISSUER TRANSACTIONS                    "))
            {
                if (flag == 130)
                {
                    flagGroup130 = 2;
                }

            }
            if (inputString.Contains("NET   "))
            {
                if (flag == 130)
                {
                    if (flagGroup130 == 0)
                    {
                        return;
                    }
                    if (flagGroup130 == 1)
                    {
                        group = new Group();
                        group.GroupName = "ACQUIRER TRANSACTIONS";
                        Detail detail = new Detail();
                        detail.Colum1 = inputString.Substring(1, 37).Trim();
                        detail.Colum2 = inputString.Substring(57, 15).Trim();
                        detail.Colum3 = inputString.Substring(73, 20).Trim();
                        detail.Colum4 = inputString.Substring(97, 20).Trim();
                        detail.Colum5 = inputString.Substring(117, 15).Trim();
                        group.ListDetail.Add(detail);
                        report_130.ListGroup.Add(group);
                    } if (flagGroup130 == 2)
                    {
                        group = new Group();
                        group.GroupName = "ISSUER TRANSACTIONS";
                        Detail detail = new Detail();
                        detail.Colum1 = inputString.Substring(1, 37).Trim();
                        detail.Colum2 = inputString.Substring(57, 15).Trim();
                        detail.Colum3 = inputString.Substring(73, 20).Trim();
                        detail.Colum4 = inputString.Substring(97, 20).Trim();
                        detail.Colum5 = inputString.Substring(117, 15).Trim();
                        group.ListDetail.Add(detail);
                        report_130.ListGroup.Add(group);
                    }

                }

            }
        }
        private void ReadRp140(Report_140 report_140, String inputString)
        {
            Group group = null;
            if (inputString.StartsWith(" ACQUIRER TRANSACTIONS                          "))
            {
                if (flag == 140)
                {
                    flagGroup140 = 1;
                }

            }
            if (inputString.StartsWith(" ISSUER TRANSACTIONS                    "))
            {
                if (flag == 140)
                {
                    flagGroup140 = 2;
                }

            }
            if (inputString.StartsWith(" NET   ISSUER CHARGES                       "))
            {
                if (flag == 140)
                {
                    if (flagGroup140 == 0)
                    {
                        return;
                    }
                    if (flagGroup140 == 1)
                    {
                        group = new Group();
                        group.GroupName = "ACQUIRER TRANSACTIONS";
                        Detail detail = new Detail();
                        detail.Colum1 = inputString.Substring(1, 37).Trim();
                        detail.Colum2 = inputString.Substring(57, 15).Trim();
                        detail.Colum3 = inputString.Substring(73, 20).Trim();
                        detail.Colum4 = inputString.Substring(97, 20).Trim();
                        detail.Colum5 = inputString.Substring(117, 15).Trim();
                        group.ListDetail.Add(detail);
                        report_140.ListGroup.Add(group);
                    } if (flagGroup140 == 2)
                    {
                        group = new Group();
                        group.GroupName = "ISSUER TRANSACTIONS";
                        Detail detail = new Detail();
                        detail.Colum1 = inputString.Substring(1, 37).Trim();
                        detail.Colum2 = inputString.Substring(57, 15).Trim();
                        detail.Colum3 = inputString.Substring(73, 20).Trim();
                        detail.Colum4 = inputString.Substring(97, 20).Trim();
                        detail.Colum5 = inputString.Substring(117, 15).Trim();
                        group.ListDetail.Add(detail);
                        report_140.ListGroup.Add(group);
                    }

                }

            }
        }
        private void ReadRp210(Report_210 report_210, String inputString)
        {
            Group group = null;
            if (inputString.StartsWith(" ACQUIRER TRANSACTIONS                          "))
            {
                if (flag == 210)
                {
                    flagGroup210 = 1;
                }

            }
            if (inputString.StartsWith(" ISSUER TRANSACTIONS                    "))
            {
                if (flag == 210)
                {
                    flagGroup210 = 2;
                }

            }
            if (inputString.StartsWith(" TOTAL "))
            {
                if (flag == 210)
                {
                    if (flagGroup210 == 0)
                    {
                        return;
                    }
                    string nextLine = streamReader.ReadLine();
                    if (flagGroup210 == 1)
                    {

                        group = new Group();
                        group.GroupName = "ACQUIRER TRANSACTIONS";
                        Detail detail = new Detail();
                        detail.Colum1 = inputString.Substring(1, 37).Trim();
                        detail.Colum2 = nextLine.Substring(57, 15).Trim();
                        detail.Colum3 = nextLine.Substring(73, 20).Trim();
                        detail.Colum4 = nextLine.Substring(97, 20).Trim();
                        detail.Colum5 = nextLine.Substring(117, 15).Trim();
                        group.ListDetail.Add(detail);
                        report_210.ListGroup.Add(group);
                    } if (flagGroup210 == 2)
                    {
                        group = new Group();
                        group.GroupName = "ISSUER TRANSACTIONS";
                        Detail detail = new Detail();
                        detail.Colum1 = inputString.Substring(1, 37).Trim();
                        detail.Colum2 = nextLine.Substring(57, 15).Trim();
                        detail.Colum3 = nextLine.Substring(73, 20).Trim();
                        detail.Colum4 = nextLine.Substring(97, 20).Trim();
                        detail.Colum5 = nextLine.Substring(117, 15).Trim();
                        group.ListDetail.Add(detail);
                        report_210.ListGroup.Add(group);
                    }

                }

            }
        }
    }
}