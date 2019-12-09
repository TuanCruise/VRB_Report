using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Data;
using FIS.AppClient.Controls;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Extensions;
using FIS.Utils;
using System.Windows.Forms;

namespace FIS.AppClient.Threads
{
    public class ImportExecuteThread : WorkerThread
    {
        public ImportModuleInfo ImportInfo { get; set; }
        public List<DataRow> ImportRows { get; set; }
        public Dictionary<DataRow, string> ErrorInfos { get; set; }
        public new ucIMWizard Parent { get; private set; }

        public ImportExecuteThread(Control parent)
            : base(null, parent)
        {
            Parent = (ucIMWizard)parent;
            Worker = ExecuteImport;
        }

        public void ExecuteImport(WorkerThread worker)
        {
            try
            {
                Parent.LockUserAction();

                ErrorInfos = new Dictionary<DataRow, string>();
                int count = 0, total = ImportRows.Count;

                try
                {
                    if (total != 0)
                    {
                        foreach (var importRow in ImportRows)
                        {
                            try
                            {
                                using (var ctrlSA = new SAController())
                                {
                                    List<string> values;
                                    Parent.GetOracleParameterValues(out values, ImportInfo.ImportStore, importRow);
                                    ctrlSA.ExecuteImport(ImportInfo.ModuleID, ImportInfo.SubModule, values);

                                    //add by trungtt - 20130508 - verify data before import
                                    //List<string> valuesVerify;
                                    //if(Program.blVerifyImport == true)
                                    //{
                                    //    Parent.GetOracleParameterValues(out valuesVerify,ImportInfo.VerifyStore);
                                    //    ctrlSA.ExecuteStoreProcedure(ImportInfo.VerifyStore, valuesVerify);
                                    //}
                                    //end trungtt

                                    StatusText = values[1].ToString();                                    
                                    ExecuteUpdateGUI();
                                }   
                            }
                            catch (FaultException ex)
                            {
                                ErrorInfos[importRow] = string.Format("{0}\r\n{1}", ex.ToMessage(), ex.Reason);                                
                                Program.blEnableImport = false;
                            }
                            catch (Exception cex)
                            {
                                var ex = ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, cex.Message);
                                ErrorInfos[importRow] = string.Format("{0}\r\n{1}", ex.ToMessage(), ex.Reason);
                                Program.blEnableImport = false;
                            }
                            finally
                            {
                                PercentComplete = count++ * 100.0f / total;
                                ExecuteUpdateGUI();
                            }
                        }
                    }
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.Message);
                }
            }
            catch (Exception ex)
            {
                Parent.ShowError(ex);
            }
            finally
            {
                Parent.UnLockUserAction();
                PercentComplete = 100.0f;
                ExecuteUpdateGUI(true);
            }
        }

        public void ExecuteImportEx()
        {
            try
            {
                Parent.LockUserAction();

                ErrorInfos = new Dictionary<DataRow, string>();
                int count = 0, total = ImportRows.Count;

                try
                {
                    if (total != 0)
                    {
                        foreach (var importRow in ImportRows)
                        {
                            try
                            {
                                using (var ctrlSA = new SAController())
                                {
                                    List<string> values;
                                    Parent.GetOracleParameterValues(out values, ImportInfo.ImportStore, importRow);
                                    ctrlSA.ExecuteImport(ImportInfo.ModuleID, ImportInfo.SubModule, values);

                                    //add by trungtt - 20130508 - verify data before import
                                    //List<string> valuesVerify;
                                    //if(Program.blVerifyImport == true)
                                    //{
                                    //    Parent.GetOracleParameterValues(out valuesVerify,ImportInfo.VerifyStore);
                                    //    ctrlSA.ExecuteStoreProcedure(ImportInfo.VerifyStore, valuesVerify);
                                    //}
                                    //end trungtt

                                    StatusText = values[1].ToString();
                                    ExecuteUpdateGUI();
                                }
                            }
                            catch (FaultException ex)
                            {
                                ErrorInfos[importRow] = string.Format("{0}\r\n{1}", ex.ToMessage(), ex.Reason);
                                Program.blEnableImport = false;
                            }
                            catch (Exception cex)
                            {
                                var ex = ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, cex.Message);
                                ErrorInfos[importRow] = string.Format("{0}\r\n{1}", ex.ToMessage(), ex.Reason);
                                Program.blEnableImport = false;
                            }
                            finally
                            {
                                PercentComplete = count++ * 100.0f / total;
                                ExecuteUpdateGUI();
                            }
                        }
                    }
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.Message);
                }
            }
            catch (Exception ex)
            {
                Parent.ShowError(ex);
            }
            finally
            {
                Parent.UnLockUserAction();
                PercentComplete = 100.0f;
                ExecuteUpdateGUI(true);
            }
        }
    }

    public class ImportRange
    {
        public int Bottom { get; set; }
        public int Top { get; set; }

        public ImportRange(int bottom, int top)
        {
            Bottom = bottom;
            Top = top;
        }

        public bool HaveToImport(int row)
        {
            return (row >= Bottom) && (row <= Top);
        }
    }
}
