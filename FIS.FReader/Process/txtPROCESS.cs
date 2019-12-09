using System;
using System.Threading;
using System.Data;
using System.Collections.Generic;
using FIS.Entities;
using FIS.FReader.Entities;
using FIS.Base;
//using FIS.FReader;
using System.IO;
using FIS.FReader.Excute;
using System.Configuration;
using System.Globalization;


namespace FIS.FReader.Process
{
    public class txtPROCESS: AbstractProcess
    {
        //LoanExcute loanExcute;
        static string HOMEDirectory = FReadEntities.HOMEFolderDirectory;
        string sFileName;
        string sLogID;
        string sReportDate;
        string sReportCreatedDate;
        List<string> sListFile;
        string s_FilePath;
        string sError;
        Boolean ErrorReading;
        
        //public Session session;
        SAController ctrlSA = new SAController(FReadEntities.MainSession);
        /// <summary>
        /// Khới động Process
        /// </summary>

        public override void Start()
        {
                GetListFile();
                try
                {
                    InitializeProcess();
                    ProcessThread = new Thread(Run);
                    ProcessThread.Start();
                }
                catch (Exception ex)
                {
                    WriteError(ex);
                    new Thread(delegate()
                    {
                        Thread.Sleep(2000);
                        SetProcessState(ProcessState.Stopped);
                    }).Start();
            }
        }


        /// <summary>
        /// Nhân Process
        /// </summary>

        public void Run()
        {

            try
            {
                WriteInfo("Bắt đầu đọc dữ liệu");
                while (Thread.CurrentThread.ThreadState == ThreadState.Running)
                {
                    try
                    {

                        string[] ListReportFile = Directory.GetFileSystemEntries(HOMEDirectory, "*.txt");
                        foreach (string F in ListReportFile)
                        {
                            sFileName = F.Replace(HOMEDirectory + "\\", "").ToUpper();
                            foreach (string Fname in sListFile)
                            {
                                if (sFileName.Contains(Fname))
                                {
                                    s_FilePath = F.ToUpper();
                                    BeginReading();
                                    if (!ErrorReading)
                                    {
                                        Read();
                                    }
                                }
                            }
                            //Console.WriteLine(F);
                            //FileName = FilePath.Replace(@"C:\Users\datle2508\Desktop\Caythumuc\", "");
                            //Console.WriteLine(FileName);
                        }
                        SetProcessState(ProcessState.Running);
                    }
                    catch (ThreadAbortException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        WriteError(ex);
                    }

                    SetProcessState(ProcessState.Sleep);
                    Thread.Sleep(20000);
                }
            }
            catch (ThreadAbortException)
            {
            }
            finally
            {
                WriteInfo("Đã kết thúc đọc dữ liệu");
                SetProcessState(ProcessState.Stopped);
            }
        }
        public override void Read()
        {
            //SetProcessState(ProcessState.Running);
                //Console.WriteLine(FilePath);
            WriteInfo("Bắt đầu đọc file: " + sFileName);

                if (sFileName.Contains("FEE"))
                    {
                        FeeOverduePaymentExcute fee = new FeeOverduePaymentExcute();
                        fee.GetReport(s_FilePath);
                        CommitData(fee);
                        FinishReading();
                    }

                if (sFileName.Contains("OVERDUE"))
                    {
                        ContractOverduePaymentExcute fee1 = new ContractOverduePaymentExcute();
                        fee1.GetReport(s_FilePath);
                        CommitData(fee1);
                        FinishReading();
                    }
                if (sFileName.Contains("TRICHNOTUDONG"))
                {
                        DebtAutomationExcute fee2 = new DebtAutomationExcute();
                        fee2.GetReport(s_FilePath);
                        CommitData(fee2);
                        FinishReading();
                }
                if (sFileName.Contains("INTEREST"))
                {
                    AccruedCreditInterestExcute fee3 = new AccruedCreditInterestExcute();
                    fee3.GetReport(s_FilePath);
                    CommitData(fee3);
                    FinishReading();
                }
                if (sFileName.Contains("BALANCE"))
                {
                    try
                    {
                        LoanExcute fee4 = new LoanExcute();
                        fee4.GetReport(s_FilePath);
                        CommitData(fee4);
                        FinishReading();
                    }
                    catch(Exception ex)
                    {
                        sError = ex.Message.ToString();
                        ErrorReading = true;
                    }
                }
                if (sFileName.Contains("EP747"))
                {
                    try
                    {
                        VisaExecute fee5 = new VisaExecute();
                        fee5.GetReport(s_FilePath);
                        CommitData(fee5);
                        FinishReading();
                    }
                    catch (Exception ex)
                    {
                        sError = ex.Message.ToString();
                        ErrorReading = true;
                    }
                }
                if (sFileName.Contains("TRIAL"))
                {
                    try
                    {
                        TrialExecute trial = new TrialExecute();
                        trial.GetReport(s_FilePath);
                        CommitData(trial);
                        FinishReading();
                    }
                    catch (Exception ex)
                    {
                        sError = ex.Message.ToString();
                        ErrorReading = true;
                    }
                }
        }



        private void GetListFile()
        {
            DataContainer dc = null;
            List<string> values = new List<string>();
            sListFile = new List<string>();
            values.Add("TXT");
            ctrlSA.ExecuteProcedureFillDataset(out dc, "IMP_LISTFILE", values);
            for (int i = 0; i < dc.DataTable.Rows.Count; i++)
            {
                sListFile.Add(dc.DataTable.Rows[i]["FILENAME"].ToString());
            }
        }


        private void BeginReading()
        {
            DataContainer dc = null;
            List<string> values = new List<string>();
            
            string[] s = sFileName.Split('_');
            if (s.Length == 2)
            {
                ErrorReading = false;
                values.Add(s[0].ToUpper());
                values.Add(sFileName.ToUpper());
                values.Add(s[1].ToUpper().Replace(".TXT", ""));
                values.Add(null);
            }
            else if(s.Length == 3)
            {
                ErrorReading = false;
                values.Add(s[0].ToUpper());
                values.Add(sFileName.ToUpper());
                values.Add(s[2].ToUpper().Replace(".TXT", ""));
                values.Add(s[1].ToUpper().Replace("LAN",""));
            }
            else
            {
                ErrorReading = true;
                values.Add(null);
                values.Add(sFileName.ToUpper());
                values.Add(null);
                values.Add(null);
                WriteErrorLog("Tên file không đúng định dạng");
            }
            ctrlSA.ExecuteProcedureFillDataset(out dc, "IMP_BEGINREADING", values);
            sLogID = dc.DataTable.Rows[0]["LOGID"].ToString();
        }

        private void FinishReading()
        {
            List<string> values = new List<string>();
            if (ErrorReading)
            {
                values.Add(sLogID); //pv_LOGID
                values.Add(""); //PV_CREATEDTAE
                values.Add(sReportDate); //PV_REPORTDATE
                values.Add(""); //PV_TERMNO
                values.Add(sError);//pv_ERRDESCRIPTION
                ctrlSA.ExecuteStoreProcedure("IMP_ERRORREADING", values);
                WriteInfo("Kết thúc đọc file: " + sFileName);
            }
            else
            {              
                values.Add(sLogID); //pv_LOGID
                values.Add(""); //PV_CREATEDTAE
                values.Add(sReportDate); //PV_REPORTDATE
                values.Add(""); //PV_TERMNO
                ctrlSA.ExecuteStoreProcedure("IMP_FINISHREADING", values);
                MoveFile("BACKUP");
                WriteInfo("Kết thúc đọc file: " + sFileName);
            }    
        }

        private void WriteErrorLog(string errString)
        {
            WriteError(String.Format("File {0}: {1}",sFileName, errString));
            MoveFile("ERROR");
        }

        private void  MoveFile(string FolderName)
        {
            string sTagetDirectory = string.Format("{0}\\{1}", HOMEDirectory, FolderName);
            string sFileMove = string.Format("{0}\\{1}_{2}", sTagetDirectory, sLogID, sFileName);
            if (!Directory.Exists(sTagetDirectory)) Directory.CreateDirectory(sTagetDirectory);
            if (File.Exists(sFileMove)) sFileMove = string.Format("{0}\\{1}_{2}", sTagetDirectory, sLogID, sFileName);
            Directory.Move(s_FilePath, sFileMove);
        }

        private DateTime ConvertDate(string S)
        {
            DateTime dtReturn;
            dtReturn = DateTime.ParseExact(S, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return (dtReturn);
        }
        private DateTime ConvertDate2(string S)
        {
            DateTime dtReturn;
                dtReturn = DateTime.ParseExact(S, "ddMMMyy", CultureInfo.InvariantCulture);
                Console.WriteLine("{0} converts to {1}.", S, dtReturn.ToString());
            return (dtReturn);
        }
        private void CommitData(FeeOverduePaymentExcute fee)
        {
            SetProcessState(ProcessState.Saving);
            try
            {
                sReportDate = fee.FeeReport.ReportDate;
                sReportCreatedDate = fee.FeeReport.ReportCreationTime;
                for (int i = 0; i < fee.FeeReport.ListContractType.Count; i++)
                {
                    for (int j = 0; j < fee.FeeReport.ListContractType[i].ListContract.Count; j++)
                    {
                        List<string> values = new List<string>();
                        values.Add(sLogID);
                        values.Add(fee.FeeReport.ListContractType[i].ConType);// pvCONTRACTYPE
                        values.Add(fee.FeeReport.ListContractType[i].Currency); //pvCURRENCY
                        values.Add(fee.FeeReport.ListContractType[i].ListContract[j].ContractNo); //pvCONTRACT
                        values.Add(fee.FeeReport.ListContractType[i].ListContract[j].Account); //pvACCOUNT
                        values.Add(fee.FeeReport.ListContractType[i].ListContract[j].Due); //pvFEEDUE
                        values.Add(fee.FeeReport.ListContractType[i].ListContract[j].Paid);//pvFEEPAID
                        ctrlSA.ExecuteStoreProcedure("TXTREADER.IMP_FEEOVERDUEPAYMENT", values);
                        //values.ForEach(Console.WriteLine);
                    }
                    //Console.WriteLine("abc---- " + fee.FeeReport.ListContractType[i].ListContract.Count);
                }
            }
            catch (Exception ex)
            {
                sError = ex.Message.ToString();
                ErrorReading = true;
            }
        }

        private void CommitData(DebtAutomationExcute fee2)
        {
            SetProcessState(ProcessState.Saving);
            try
            {
                for (int i = 0; i < fee2.DebtReport.ListDebt.Count; i++)
                {
                    List<string> values = new List<string>();
                    values.Add(sLogID);
                    values.Add(fee2.DebtReport.ListDebt[i].CardAccountNumber);// pv_CARDNUMBER
                    values.Add(fee2.DebtReport.ListDebt[i].EmbossingName); //pv_NAME
                    values.Add(fee2.DebtReport.ListDebt[i].BranchPart); //pv_BRANCHCODE
                    values.Add(fee2.DebtReport.ListDebt[i].BankAccountNumber); //pv_BANKACCOUNT
                    values.Add(fee2.DebtReport.ListDebt[i].DebtCreditFlag); //pv_FLAG
                    values.Add(fee2.DebtReport.ListDebt[i].CardAccountCurrency); //pv_CURRENTCY
                    values.Add(fee2.DebtReport.ListDebt[i].Amount); //pv_AMOUNT
                    values.Add(fee2.DebtReport.ListDebt[i].NumOfDecPlace); //pv_DECIMALNUMBER
                    values.Add(fee2.DebtReport.ListDebt[i].PostingDate); //pv_POSTINGDATE
                    values.Add(fee2.DebtReport.ListDebt[i].AccountNumber); //pv_ACCOUNTNUMBER
                    ctrlSA.ExecuteStoreProcedure("TXTREADER.IMP_DEBTEXECUTE", values);
                    //values.ForEach(Console.WriteLine);
                    //Console.WriteLine("abc---- " + fee2.DebtReport.ListDebt[i].RowNumber);
                }
            }
            catch (Exception ex)
            {
                sError = ex.Message.ToString();
                ErrorReading = true;
            }
            //Console.WriteLine("---- " + fee2.DebtReport.ListDebt.Count);
            //Console.WriteLine("-------TimeProcess------- " + fee2.DebtReport.TimeProcess);
            //Console.WriteLine("-------TotalRow---------- " + fee2.DebtReport.FileRow);
            //Console.WriteLine("-------FileName---------- " + fee2.DebtReport.FileName);
            //Console.WriteLine("-------FileSize---------- " + fee2.DebtReport.FileSize);
            //Console.WriteLine("-------FileLastMod------- " + fee2.DebtReport.FileLastMod);
            //Console.WriteLine("-------FileCreate-------- " + fee2.DebtReport.FileCreate);
            //Console.WriteLine("-------FileLocation------ " + fee2.DebtReport.FileLocation);
            //Console.WriteLine("-------FileHash---------- " + fee2.DebtReport.FileHash);
        }

        private void CommitData(ContractOverduePaymentExcute fee1)
        {
            SetProcessState(ProcessState.Saving);
            try
            {
                sReportDate = fee1.ContractReport.ReportDate;
                sReportCreatedDate = fee1.ContractReport.ReportCreationTime;
                for (int i = 0; i < fee1.ContractReport.ListContractType.Count; i++)
                {
                    //Console.WriteLine("-- " + fee1.ContractReport.ListContractType[i].PreFix);
                    //Console.WriteLine("-- " + fee1.ContractReport.ListContractType[i].ConType);
                    for (int j = 0; j < fee1.ContractReport.ListContractType[i].ListContract.Count; j++)
                    {
                        List<string> values = new List<string>();
                        values.Add(sLogID);
                        values.Add(fee1.ContractReport.ListContractType[i].ConType);// pvCONTRACTYPE
                        values.Add(fee1.ContractReport.ListContractType[i].ListContract[j].Currency); //pvCURRENCY
                        values.Add(fee1.ContractReport.ListContractType[i].ListContract[j].ContractNo); //pvCONTRACT
                        values.Add(fee1.ContractReport.ListContractType[i].ListContract[j].Account); //pvACCOUNT
                        values.Add(fee1.ContractReport.ListContractType[i].ListContract[j].MinPaymentAcc); //pvPAYMENTAMOUNT
                        values.Add(fee1.ContractReport.ListContractType[i].ListContract[j].DuePeriod);//pvDUEPERIOD
                        ctrlSA.ExecuteStoreProcedure("TXTREADER.IMP_OVERDUEPAYMENT", values);
                        //values.ForEach(Console.WriteLine);
                        //Console.WriteLine("RowNum---- " + fee1.ContractReport.ListContractType[i].ListContract[j].RowNumber);

                    }
                    //Console.WriteLine("abc---- " + fee1.ContractReport.ListContractType[i].ListContract.Count);
                }
            }
            catch (Exception ex)
            {
                sError = ex.Message.ToString();
                ErrorReading = true;
            }
            //Console.WriteLine("-------TimeProcess------- " + fee1.ContractReport.TimeProcess);
            //Console.WriteLine("-------TotalRow---------- " + fee1.ContractReport.FileRow);
            //Console.WriteLine("-------FileName---------- " + fee1.ContractReport.FileName);
            //Console.WriteLine("-------FileSize---------- " + fee1.ContractReport.FileSize);
            //Console.WriteLine("-------FileLastMod------- " + fee1.ContractReport.FileLastMod);
            //Console.WriteLine("-------FileCreate-------- " + fee1.ContractReport.FileCreate);
            //Console.WriteLine("-------FileLocation------ " + fee1.ContractReport.FileLocation);
            //Console.WriteLine("-------FileHash---------- " + fee1.ContractReport.FileHash);
        }

        
        private void CommitData(AccruedCreditInterestExcute fee3)
        {
            SetProcessState(ProcessState.Saving);
            try
            {
                sReportDate = fee3.AccruedReport.ReportDate;
                sReportCreatedDate = fee3.AccruedReport.ReportCreationTime;
                for (int i = 0; i < fee3.AccruedReport.ListContractType.Count; i++)
                {         
                    for (int j = 0; j < fee3.AccruedReport.ListContractType[i].ListContract.Count; j++)
                    {
                        List<string> values = new List<string>();
                        values.Add(sLogID);
                        values.Add(fee3.AccruedReport.ListContractType[i].ConType);// pvCONTRACTYPE
                        values.Add(fee3.AccruedReport.ListContractType[i].Currency); //pvCURRENCY
                        values.Add(fee3.AccruedReport.ListContractType[i].ListContract[j].ContractNo); //pvCONTRACTNO
                        values.Add(fee3.AccruedReport.ListContractType[i].ListContract[j].ClientName); //pvCLIENTNAME
                        values.Add(fee3.AccruedReport.ListContractType[i].ListContract[j].Interests); //pvINTEREST
                        Console.WriteLine("rownum---- " + fee3.AccruedReport.ListContractType[i].ListContract[j].RowNumber);
                        ctrlSA.ExecuteStoreProcedure("TXTREADER.IMP_ACCRUEDINTEREST", values);
                        //values.ForEach(Console.WriteLine);
                    }
                    //Console.WriteLine("abc---- " + fee3.AccruedReport.ListContractType[i].ListContract.Count);
                }
            }
            catch (Exception ex)
            {
                sError = ex.Message.ToString();
                ErrorReading = true;
            }
        }

        private void CommitData(LoanExcute fee4)
        {
            SetProcessState(ProcessState.Saving);
            try
            {
                //sReportDate = fee4.LoanReport.ReportDate;
                //for (int i = 0; i < fee4.LoanReport.ListLoanType.Count; i++)
                //{
                //    List<string> values = new List<string>();
                //    values.Add(sLogID);
                //    values.Add(fee4.LoanReport.ReportDateTableName);
                //    values.Add(fee4.LoanReport.ListLoanType[i].AcctNo);
                //    values.Add(fee4.LoanReport.ListLoanType[i].Branch);
                //    values.Add(fee4.LoanReport.ListLoanType[i].Name);
                //    values.Add(fee4.LoanReport.ListLoanType[i].PassportNo);
                //    values.Add(fee4.LoanReport.ListLoanType[i].OpeningBalance);
                //    //ctrlSA.ExecuteStoreProcedure("TXTREADER.IMP_DEBTBALANCE", values);
                //    values.ForEach(Console.WriteLine);
                //}

                sReportDate = fee4.LoanReport.ReportDate;
                sReportCreatedDate = fee4.LoanReport.ReportCreationDate;
                for (int i = 0; i < fee4.LoanReport.ListGroupLoan.Count; i++)
                {
                    for (int j = 0; j < fee4.LoanReport.ListGroupLoan[i].ListLoanType.Count; j++)
                    {
                        List<string> values = new List<string>();
                        values.Add(sLogID);

                        values.Add(fee4.LoanReport.ListGroupLoan[i].ReportDateTableName);
                        values.Add(fee4.LoanReport.ListGroupLoan[i].ListLoanType[j].AcctNo);
                        values.Add(fee4.LoanReport.ListGroupLoan[i].ListLoanType[j].Branch);
                        values.Add(fee4.LoanReport.ListGroupLoan[i].ListLoanType[j].Name);
                        values.Add(fee4.LoanReport.ListGroupLoan[i].ListLoanType[j].PassportNo);
                        values.Add(fee4.LoanReport.ListGroupLoan[i].ListLoanType[j].OpeningBalance);
                        ctrlSA.ExecuteStoreProcedure("TXTREADER.IMP_DEBTBALANCE", values);
                        //values.ForEach(Console.WriteLine);
                    }

                }

            }
            catch (Exception ex)
            {
                sError = ex.Message.ToString();
                ErrorReading = true;
            }
        }
        private void CommitData(VisaExecute visa)
        {
            SetProcessState(ProcessState.Saving);
            //VSS-110
            try
            {
                sReportDate = visa.VisaReport.ListReport_110[1].ReportDate;
                for (int i = 0; i < visa.VisaReport.ListReport_110.Count; i++)
                {
                    for (int j = 0; j < visa.VisaReport.ListReport_110[i].ListDetail.Count; j++)
                    {
                        List<string> values = new List<string>();
                        values.Add(sLogID);
                        values.Add((i + 1).ToString());//REPORT_NUMBER
                        values.Add(visa.VisaReport.ListReport_110[i].ReportID);//REPORT_ID
                        values.Add(visa.VisaReport.ListReport_110[i].ReportingFor);//REPORT_FOR
                        values.Add(visa.VisaReport.ListReport_110[i].RollUp);//ROLL_UP
                        values.Add(visa.VisaReport.ListReport_110[i].ProcDate);//PROC_DATE
                        values.Add(visa.VisaReport.ListReport_110[i].ReportDate);//REPORT_DATE
                        values.Add(visa.VisaReport.ListReport_110[i].Funds);//FUNDS
                        values.Add(visa.VisaReport.ListReport_110[i].SettlementCurrency);//SETTLMENT_CURRENCY
                        values.Add(visa.VisaReport.ListReport_110[i].ClearingCurrency);//CLEARING_CURRENCY
                        values.Add(visa.VisaReport.ListReport_110[i].ListDetail[j].Colum5);//TOTAL_AMOUNT
                        ctrlSA.ExecuteStoreProcedure("TXTREADER.IMP_VSS110_EXECUTE", values);
                        //values.ForEach(Console.WriteLine);
                    }

                }
                //VSS-120
                for (int i = 0; i < visa.VisaReport.ListReport_120.Count; i++)
                {
                    for (int j = 0; j < visa.VisaReport.ListReport_120[i].ListGroup.Count; j++)
                    {
                        for (int k = 0; k < visa.VisaReport.ListReport_120[i].ListGroup[j].ListDetail.Count; k++)
                        {
                            List<string> values = new List<string>();
                            values.Add(sLogID);
                            values.Add((i + 1).ToString());//REPORT_NUMBER
                            values.Add(visa.VisaReport.ListReport_120[i].ReportID);//REPORT_ID
                            values.Add(visa.VisaReport.ListReport_120[i].ReportingFor);//REPORT_FOR
                            values.Add(visa.VisaReport.ListReport_120[i].RollUp);//ROLL_UP
                            values.Add(visa.VisaReport.ListReport_120[i].ProcDate);//PROC_DATE
                            values.Add(visa.VisaReport.ListReport_120[i].ReportDate);//REPORT_DATE
                            values.Add(visa.VisaReport.ListReport_120[i].Funds);//FUNDS
                            values.Add(visa.VisaReport.ListReport_120[i].SettlementCurrency);//SETTLMENT_CURRENCY
                            values.Add(visa.VisaReport.ListReport_120[i].ClearingCurrency);//CLEARING_CURRENCY
                            values.Add(visa.VisaReport.ListReport_120[i].ListGroup[j].GroupName);//GROUP_NAME
                            values.Add(visa.VisaReport.ListReport_120[i].ListGroup[j].ListDetail[k].Colum1);//NAME
                            values.Add(visa.VisaReport.ListReport_120[i].ListGroup[j].ListDetail[k].Colum5);//VALUE_CREDIT
                            values.Add(visa.VisaReport.ListReport_120[i].ListGroup[j].ListDetail[k].Colum6);//VALUE_DEBIT
                            ctrlSA.ExecuteStoreProcedure("TXTREADER.IMP_VSS120_EXECUTE", values);
                            //values.ForEach(Console.WriteLine);
                        }
                    }
                }
                //VSS-130
                for (int i = 0; i < visa.VisaReport.ListReport_130.Count; i++)
                {
                    for (int j = 0; j < visa.VisaReport.ListReport_130[i].ListGroup.Count; j++)
                    {
                        for (int k = 0; k < visa.VisaReport.ListReport_130[i].ListGroup[j].ListDetail.Count; k++)
                        {
                            List<string> values = new List<string>();
                            values.Add(sLogID);
                            values.Add((i + 1).ToString());//REPORT_NUMBER
                            values.Add(visa.VisaReport.ListReport_130[i].ReportID);//REPORT_ID
                            values.Add(visa.VisaReport.ListReport_130[i].ReportingFor);//REPORT_FOR
                            values.Add(visa.VisaReport.ListReport_130[i].RollUp);//ROLL_UP
                            values.Add(visa.VisaReport.ListReport_130[i].ProcDate);//PROC_DATE
                            values.Add(visa.VisaReport.ListReport_130[i].ReportDate);//REPORT_DATE
                            values.Add(visa.VisaReport.ListReport_130[i].Funds);//FUNDS
                            values.Add(visa.VisaReport.ListReport_130[i].SettlementCurrency);//SETTLMENT_CURRENCY
                            values.Add(visa.VisaReport.ListReport_130[i].ClearingCurrency);//CLEARING_CURRENCY
                            values.Add(visa.VisaReport.ListReport_130[i].ListGroup[j].GroupName);//GROUP_NAME
                            values.Add(visa.VisaReport.ListReport_130[i].ListGroup[j].ListDetail[k].Colum1);//NAME
                            values.Add(visa.VisaReport.ListReport_130[i].ListGroup[j].ListDetail[k].Colum4);//FEE_CREDIT
                            values.Add(visa.VisaReport.ListReport_130[i].ListGroup[j].ListDetail[k].Colum5);//FEEE_DEBIT
                            ctrlSA.ExecuteStoreProcedure("TXTREADER.IMP_VSS130_EXECUTE", values);
                            //values.ForEach(Console.WriteLine);
                        }
                    }
                }
                //VSS-140
                for (int i = 0; i < visa.VisaReport.ListReport_140.Count; i++)
                {
                    for (int j = 0; j < visa.VisaReport.ListReport_140[i].ListGroup.Count; j++)
                    {
                        for (int k = 0; k < visa.VisaReport.ListReport_140[i].ListGroup[j].ListDetail.Count; k++)
                        {
                            List<string> values = new List<string>();
                            values.Add(sLogID);
                            values.Add((i + 1).ToString());//REPORT_NUMBER
                            values.Add(visa.VisaReport.ListReport_140[i].ReportID);//REPORT_ID
                            values.Add(visa.VisaReport.ListReport_140[i].ReportingFor);//REPORT_FOR
                            values.Add(visa.VisaReport.ListReport_140[i].RollUp);//ROLL_UP
                            values.Add(visa.VisaReport.ListReport_140[i].ProcDate);//PROC_DATE
                            values.Add(visa.VisaReport.ListReport_140[i].ReportDate);//REPORT_DATE
                            values.Add(visa.VisaReport.ListReport_140[i].Funds);//FUNDS
                            values.Add(visa.VisaReport.ListReport_140[i].SettlementCurrency);//SETTLMENT_CURRENCY
                            values.Add(visa.VisaReport.ListReport_140[i].ClearingCurrency);//CLEARING_CURRENCY
                            values.Add(visa.VisaReport.ListReport_140[i].ListGroup[j].GroupName);//GROUP_NAME
                            values.Add(visa.VisaReport.ListReport_140[i].ListGroup[j].ListDetail[k].Colum1);//NAME
                            values.Add(visa.VisaReport.ListReport_140[i].ListGroup[j].ListDetail[k].Colum4);//VISA_CHARGES_CREDITS
                            values.Add(visa.VisaReport.ListReport_140[i].ListGroup[j].ListDetail[k].Colum5);//VISA_CHARGES_DEBIT
                            ctrlSA.ExecuteStoreProcedure("TXTREADER.IMP_VSS140_EXECUTE", values);
                            //values.ForEach(Console.WriteLine);
                        }
                    }
                }
                //VSS-210
                for (int i = 0; i < visa.VisaReport.ListReport_210.Count; i++)
                {
                    for (int j = 0; j < visa.VisaReport.ListReport_210[i].ListGroup.Count; j++)
                    {
                        for (int k = 0; k < visa.VisaReport.ListReport_210[i].ListGroup[j].ListDetail.Count; k++)
                        {
                            List<string> values = new List<string>();
                            values.Add(sLogID);
                            values.Add((i + 1).ToString());//REPORT_NUMBER
                            values.Add(visa.VisaReport.ListReport_210[i].ReportID);//REPORT_ID
                            values.Add(visa.VisaReport.ListReport_210[i].ReportingFor);//REPORT_FOR
                            values.Add(visa.VisaReport.ListReport_210[i].RollUp);//ROLL_UP
                            values.Add(visa.VisaReport.ListReport_210[i].ProcDate);//PROC_DATE
                            values.Add(visa.VisaReport.ListReport_210[i].ReportDate);//REPORT_DATE
                            values.Add(visa.VisaReport.ListReport_210[i].Funds);//FUNDS
                            values.Add(visa.VisaReport.ListReport_210[i].SettlementCurrency);//SETTLMENT_CURRENCY
                            values.Add(visa.VisaReport.ListReport_210[i].ClearingCurrency);//CLEARING_CURRENCY
                            values.Add(visa.VisaReport.ListReport_210[i].ListGroup[j].GroupName);//GROUP_NAME
                            values.Add(visa.VisaReport.ListReport_210[i].ListGroup[j].ListDetail[k].Colum1);//NAME
                            values.Add(visa.VisaReport.ListReport_210[i].ListGroup[j].ListDetail[k].Colum5);//OPT_ISSUER_FEE
                            ctrlSA.ExecuteStoreProcedure("TXTREADER.IMP_VSS210_EXECUTE", values);
                            //values.ForEach(Console.WriteLine);
                        }
                    }
                }
            }
                
            catch (Exception ex)
            {
                sError = ex.Message.ToString();
                ErrorReading = true;
            }
        }
        private void CommitData(TrialExecute trial)
        {
            SetProcessState(ProcessState.Saving);
            try
            {
                for (int i = 0; i < trial.TrialReport.ListBranch.Count; i++)
                {
                    for (int j = 0; j < trial.TrialReport.ListBranch[i].ListGroupTrial.Count; j++)
                    {
                        for (int k = 0; k < trial.TrialReport.ListBranch[i].ListGroupTrial[j].ListTrialRow.Count; k++)
                        {
                            List<string> values = new List<string>();
                            values.Add(sLogID);//logid
                            values.Add(trial.TrialReport.ReportFromDate);//REPORT_FROM
                            values.Add(trial.TrialReport.ReportToDate);//REPORT_TO
                            values.Add(trial.TrialReport.ListBranch[i].BranchName);//BRANCH
                            values.Add(trial.TrialReport.ListBranch[i].Currency);//CURRENCY
                            values.Add(trial.TrialReport.ListBranch[i].ListGroupTrial[j].GroupName);//ACCT
                            values.Add(trial.TrialReport.ListBranch[i].ListGroupTrial[j].ListTrialRow[k].AccountNo);//ACCOUNTNO
                            values.Add(trial.TrialReport.ListBranch[i].ListGroupTrial[j].ListTrialRow[k].OpeningBalance);//OPENINGBALANCE
                            values.Add(trial.TrialReport.ListBranch[i].ListGroupTrial[j].ListTrialRow[k].Debit);//DEBIT
                            values.Add(trial.TrialReport.ListBranch[i].ListGroupTrial[j].ListTrialRow[k].Credit);//CREDIT
                            values.Add(trial.TrialReport.ListBranch[i].ListGroupTrial[j].ListTrialRow[k].ClosingBalance);//COSLINGBALANCE
                            ctrlSA.ExecuteStoreProcedure("TXTREADER.IMP_TRIAL", values);
                            //values.ForEach(Console.WriteLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sError = ex.Message.ToString();
                ErrorReading = true;
            }
        }
        protected override void SetProcessState(ProcessState State)
        {
            switch (State)
            {
                case ProcessState.Error:
                case ProcessState.Running:
                case ProcessState.Stopped:
                    ProcessStatusText = string.Format("Đã ngừng đọc", TotalError);
                    break;
            }
            base.SetProcessState(State);
        }
    }
}
