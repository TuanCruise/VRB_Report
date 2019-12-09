using System;
using System.ServiceModel;
using FIS.AppClient.Interface;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using System.Collections.Generic;

namespace FIS.AppClient.Controls
{
    public partial class ucStoreExecute : ucModule,
        IParameterFieldSupportedModule
    {
        #region Properties & Members
        public ExecProcModuleInfo ExecProcInfo {
            get
            {
                return (ExecProcModuleInfo)ModuleInfo;
            }
        }
        #endregion

        public ucStoreExecute()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Execute();
        }

        protected override void BuildButtons()
        {
            base.BuildButtons();
#if DEBUG
            SetupModuleEdit();
            SetupGenenerateScript();
            SetupSeparator();
            SetupParameterFields();
            SetupSeparator();
            SetupFieldMaker();
            SetupSeparator();
            SetupLanguageTool();

            if(Parent != null)
                Parent.ContextMenuStrip = Context;
#endif
        }

        public override void Execute()
        {
            new WorkerThread(
                delegate
                    {
                        try
                        {
                            LockUserAction();

                            using (var ctrlSA = new SAController())
                            {
                                List<string> values;
                                GetOracleParameterValues(out values, ExecProcInfo.ExecuteStore);
                                ctrlSA.ExecuteProcedure(ModuleInfo.ModuleID, ModuleInfo.SubModule, values);

                                // Hard-codes for special modules
                                switch (ModuleInfo.ModuleID)
                                {
                                    case "0128":
                                        App.Environment.GetCurrentUserProfile();
                                        ThemeUtils.ChangeSkin(App.Environment.ClientInfo.UserProfile.ApplicationSkinName);
                                        break;
                                }

                                RequireRefresh = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowError(ex);
                        }
                        finally
                        {
                            CloseModule();
                        }
                    },
            this).Start();
        }

        public override void LockUserAction()
        {
            base.LockUserAction();

            if (!InvokeRequired)
            {
                ShowWaitingBox();
                Enabled = false;
            }
        }
    }
}
