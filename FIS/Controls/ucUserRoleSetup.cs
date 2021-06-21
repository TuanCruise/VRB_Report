using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using FIS.AppClient.Interface;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FIS.AppClient.Controls
{
    public partial class ucUserRoleSetup : ucModule,
        IParameterFieldSupportedModule
    {
        private List<Role> m_Roles;

        public int UserID
        {
            get
            {
                var field = FieldUtils.GetModuleFieldsByName(ModuleInfo.ModuleID, "USERID")[0];
                if (this[field.FieldID] == null)
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_MODULE_PARAMETER_REQUIRE, "USERID");

                return int.Parse(this[field.FieldID].ToString());
            }
        }

        public string UserName
        {
            get
            {
                var field = FieldUtils.GetModuleFieldsByName(ModuleInfo.ModuleID, "USERNAME")[0];
                return (string)this[field.FieldID];
            }
        }

        public ucUserRoleSetup()
        {
            InitializeComponent();
#if DEBUG
            roleTree.DragOver += roleTree_DragOver;
#endif
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
            if (UserID != 0)
            {
                lbTitle.Text = string.Format(Language.Info, UserName);

                try
                {
                    using (var ctrlSA = new SAController())
                    {
                        ctrlSA.ListUserRoles(out m_Roles, UserID);
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

        private void roleTree_GetStateImage(object sender, GetStateImageEventArgs e)
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

        private void roleTree_CellValueChanged(object sender, CellValueChangedEventArgs e)
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
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.SaveUserRoles(m_Roles, UserID);
                    CloseModule();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
                CloseModule();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseModule();
        }

#if DEBUG
        private void roleTree_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
#endif
    }
}
