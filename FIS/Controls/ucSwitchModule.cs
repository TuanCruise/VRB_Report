using System.Collections.Generic;
using FIS.AppClient.Interface;
using FIS.Controllers;
using FIS.Entities;
using FIS.AppClient.Utils;

namespace FIS.AppClient.Controls
{
    public class ucSwitchModule : ucModule,
        IParameterFieldSupportedModule
    {
        #region Properties & Members
        public SwitchModuleInfo SwitchInfo
        {
            get
            {
                return (SwitchModuleInfo)ModuleInfo;
            }
        }
        #endregion

        #region Module Utils
        public override void ShowModule(System.Windows.Forms.IWin32Window owner)
        {
            using (var ctrlSA = new SAController())
            {
                List<string> values;
                string targetModule;

                GetOracleParameterValues(out values, SwitchInfo.SwitchStore);
                if (values.Count == 1 && values[0] == null)
                {
                    CloseModule();
                }
                else
                {
                    ctrlSA.ExecuteSwitchModule(out targetModule, ModuleInfo.ModuleID, ModuleInfo.SubModule, values);                    
                    //tudq them
                    if (SwitchInfo.SwitchStore.ToUpper() == "SP_SWITCH_MAINTAIN_LOG" || SwitchInfo.SwitchStore.ToUpper() == "SP_SWITCH_VIEW_TLLOGMEMBER")
                        Program.txnum = values[1];
                    //
                    if (SwitchInfo.SwitchStore.ToUpper() == "SP_SWITCH_APPROVE" )
                        Program.txnum = values[1]; //HUYVQ: Fix 2 -> 1: bỏ BUROWID

                    if (SwitchInfo.SwitchStore.ToUpper() == "SP_SWITCH_IMP")
                    {
                        Program.rptid = values[1];
                        if(values.Count > 2)
                            Program.rptlogID = values[2];
                    }
                    var module = MainProcess.CreateModuleInstance(targetModule);

                    foreach (var param in ParameterFields)
                    {
                        module.SetFieldValue(param.FieldName, ParameterByFieldID[param.FieldID]);
                    }

                    module.ModuleClosed += delegate
                    {
                        RequireRefresh = module.RequireRefresh;
                        CloseModule();
                    };

                    module.ShowModule(owner);
                }                
            }
        }
        #endregion
    }
}
