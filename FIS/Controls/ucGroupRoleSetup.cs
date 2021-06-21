using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTreeList.Nodes;
using FIS.AppClient.Interface;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;

namespace FIS.AppClient.Controls
{
    public partial class ucGroupRoleSetup : ucModule,
        IParameterFieldSupportedModule
    {
        private List<Role> m_Roles;
        public Dictionary<int, FaultException> UserRoleExceptions { get; set; }
        public Dictionary<int, int> UserRoleImageIndexies { get; set; }

        public int GroupID
        {
            get
            {
                var field = FieldUtils.GetModuleFieldsByName(ModuleInfo.ModuleID, "GROUPID")[0];
                if (this[field.FieldID] == null)
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_MODULE_PARAMETER_REQUIRE, "GROUPID");

                return int.Parse(this[field.FieldID].ToString());
            }
        }

        public string GroupName
        {
            get
            {
                var field = FieldUtils.GetModuleFieldsByName(ModuleInfo.ModuleID, "GROUPNAME")[0];
                return (string)this[field.FieldID];
            }
        }

        public ucGroupRoleSetup()
        {
            InitializeComponent();
            roleTree.StateImageList = ThemeUtils.Image16;
        }

        protected override void BuildButtons()
        {
            base.BuildButtons();
            var form = Parent as Form;
            if (form != null)
            {
                form.CancelButton = btnClose;
                form.AcceptButton = btnSetup;
            }
        }

        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();
            if (GroupID != 0)
            {
                lbTitle.Text = string.Format(Language.Info, GroupName);

                try
                {
                    using (var ctrlSA = new SAController())
                    {
                        List<User> users;
                        ctrlSA.ListUsersInGroup(out users, GroupID);
                        lstUserList.ImageList = ThemeUtils.Image16;
                        foreach (var user in users)
                        {
                            lstUserList.Items.Add(new ImageListBoxItem
                            {
                                Value = user,
                                ImageIndex = ThemeUtils.GetImage16x16Index("USER_EDIT")
                            });
                        }

                        ctrlSA.ListGroupRoles(out m_Roles, GroupID);
                        foreach (var role in m_Roles)
                        {
                            role.TranslatedRoleName = Language.GetRoleName(role.RoleName);
                        }

                        roleTree.DataSource = m_Roles;
                        UpdateCategory(roleTree.Nodes);
                    }
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                    CloseModule();
                }
#if !DEBUG
                colRoleID.Visible = false;
#endif
            }
        }

        private void roleTree_GetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            var role = roleTree.GetDataRecordByNode(e.Node) as Role;

            if (role != null)
            {
                if (role.RoleType == CODES.DEFROLE.ROLETYPE.CATEGORY)
                {
                    e.NodeImageIndex = Language.FolderOpenImageIndex;
                }
                else if (role.RoleType == CODES.DEFROLE.ROLETYPE.HIGH_ROLE)
                {
                    if (role.RoleValue == "Y")
                        e.NodeImageIndex = Language.HightRoleYesImageIndex;
                    else
                        e.NodeImageIndex = Language.HighRoleNoImageIndex;
                }
                else if (role.RoleType == CODES.DEFROLE.ROLETYPE.NORMAL_ROLE)
                {
                    if (role.RoleValue == "Y")
                        e.NodeImageIndex = Language.RoleYesImageIndex;
                    else
                        e.NodeImageIndex = Language.RoleNoImageIndex;
                }
            }
        }

        private void UpdateRoleValue(TreeListNode node)
        {
            UpdateRoleValue(node, false);
        }

        private void UpdateRoleValue(TreeListNode node, bool refreshCategory)
        {
            roleTree.SuspendLayout();
            var role = roleTree.GetDataRecordByNode(node) as Role;

            if (role != null)
            {
                if (role.RoleType == CODES.DEFROLE.ROLETYPE.CATEGORY && role.RoleValue != null)
                {
                    foreach (TreeListNode childNode in node.Nodes)
                    {
                        var childRole = roleTree.GetDataRecordByNode(childNode) as Role;
                        if (childRole != null)
                        {
                            childRole.RoleValue = role.RoleValue;
                            UpdateRoleValue(childNode);
                        }
                    }
                }
                else
                {
                    if (role.RoleValue == "Y" && !string.IsNullOrEmpty(role.RequireRoleID))
                    {
                        var requireNode = roleTree.FindNodeByFieldValue("RoleID", role.RequireRoleID);
                        if (requireNode != null)
                        {
                            var requireRole = roleTree.GetDataRecordByNode(requireNode) as Role;
                            if (requireRole != null && requireRole.RoleValue == "N")
                            {
                                requireRole.RoleValue = "Y";
                                UpdateRoleValue(requireNode);
                            }
                        }
                    }

                    if (role.RoleValue == "N")
                    {
                        var dependRoleIDs = (from dependRole in m_Roles
                                             where dependRole.RequireRoleID == role.RoleID
                                             select dependRole.RoleID).ToArray();
                        foreach (var dependRoleID in dependRoleIDs)
                        {
                            var dependNode = roleTree.FindNodeByFieldValue("RoleID", dependRoleID);
                            if (dependNode != null)
                            {
                                var dependRole = roleTree.GetDataRecordByNode(dependNode) as Role;
                                if (dependRole != null && dependRole.RoleValue == "Y")
                                {
                                    dependRole.RoleValue = "N";
                                    UpdateRoleValue(dependNode);
                                }
                            }
                        }
                    }
                }
            }

            if (refreshCategory)
                UpdateCategory(roleTree.Nodes);

            roleTree.Refresh();
            roleTree.ResumeLayout();
        }

        private string UpdateCategory(TreeListNodes nodes)
        {
            var countYes = 0;
            var countNo = 0;
            foreach (TreeListNode childNode in nodes)
            {
                var role = roleTree.GetDataRecordByNode(childNode) as Role;
                if (role != null && role.RoleType == CODES.DEFROLE.ROLETYPE.CATEGORY)
                {
                    role.RoleValue = UpdateCategory(childNode.Nodes);
                }
                else
                {
                    UpdateCategory(childNode.Nodes);
                }

                if (role != null)
                {
                    if (role.RoleValue == "Y") countYes++;
                    if (role.RoleValue == "N") countNo++;
                    if (role.RoleValue == null) { countYes++; countNo++; }
                }
            }

            if (countYes == 0 && countNo != 0) return "N";
            if (countNo == 0 && countYes != 0) return "Y";
            return null;
        }

        private void repoRoleValue_EditValueChanged(object sender, EventArgs e)
        {
            roleTree.CloseEditor();
        }

        private void roleTree_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            if (e.Column == colRoleValue)
                UpdateRoleValue(e.Node, true);
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            Execute();
        }

        public override void Execute()
        {
            base.Execute();

            try
            {
                btnSetup.Enabled = false;
                btnClose.Enabled = false;
                ctrlMainTab.SelectedTabPage = tabUserList;
                ctrlMainTab.Refresh();

                UserRoleExceptions = new Dictionary<int, FaultException>();
                UserRoleImageIndexies = new Dictionary<int, int>();

                foreach (ImageListBoxItem item in lstUserList.Items)
                {
                    item.ImageIndex = ThemeUtils.GetImage16x16Index("USER_EDIT");
                }

                using (var ctrlSA = new SAController())
                {
                    ctrlSA.SaveGroupRoles(m_Roles.Where(i => i.RoleValue == "Y").ToList(), GroupID);
                }
                btnClose.Enabled = true;
                //----------------------------------------------
                /*CurrentThread = new WorkerThread(delegate
                     {
                         CurrentThread.ExecuteUpdateGUI(true);
                         foreach (ImageListBoxItem item in lstUserList.Items)
                         {
                             var user = item.Value as User;
                             if (user != null)
                             {
                                 try
                                 {
                                     using (var ctrlSA = new SAController())
                                     {
                                         UserRoleImageIndexies[lstUserList.Items.IndexOf(item)] = ThemeUtils.GetImage16x16Index("EXECUTE_STORE");
                                         CurrentThread.ExecuteUpdateGUI(true);
                                         ctrlSA.SaveUserRoles(m_Roles.Where(i => i.RoleValue == "Y").ToList(), user.UserID);
                                         UserRoleImageIndexies[lstUserList.Items.IndexOf(item)] = ThemeUtils.GetImage16x16Index("USER_DONE");
                                     }
                                 }
                                 catch (FaultException ex)
                                 {
                                     UserRoleImageIndexies[lstUserList.Items.IndexOf(item)] = ThemeUtils.GetImage16x16Index("USER_FAIL");
                                     UserRoleExceptions[lstUserList.Items.IndexOf(item)] = ex;
                                 }
                                 catch (Exception ex)
                                 {
                                     UserRoleImageIndexies[lstUserList.Items.IndexOf(item)] = ThemeUtils.GetImage16x16Index("USER_FAIL");
                                     UserRoleExceptions[lstUserList.Items.IndexOf(item)] = ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.Message);
                                 }

                                 CurrentThread.ExecuteUpdateGUI(true);
                             }
                         }
                     }, this);
                CurrentThread.DoUpdateGUI += CurrentThread_DoUpdateGUI;
                CurrentThread.ProcessComplete +=
                    delegate
                    {
                        btnSetup.Enabled = true;
                        btnClose.Enabled = true;
                        if (UserRoleExceptions.Count == 0)
                        {
                            CloseModule();
                        }
                    };
                CurrentThread.Start();*/
                //--------------------------------------------
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        void CurrentThread_DoUpdateGUI(object sender, EventArgs e)
        {
            foreach (var pair in UserRoleImageIndexies)
            {
                lstUserList.Items[pair.Key].ImageIndex = pair.Value;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseModule();
        }

        private void lstUserList_DoubleClick(object sender, EventArgs e)
        {
            if (lstUserList.SelectedItem != null && UserRoleExceptions.ContainsKey(lstUserList.SelectedIndex))
            {
                frmInfo.ShowError(Language.Title, UserRoleExceptions[lstUserList.SelectedIndex]);
            }
        }
    }
}
