using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FIS.AppClient.Interface;
using FIS.Common;
using FIS.Entities;
using FIS.Utils;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using System.Drawing;
using System.ComponentModel;
using FIS.AppClient.Utils;
using FIS.Base;
using FIS.Controllers;
using System.Data;

namespace FIS.AppClient.Controls
{
    public class UISearchGroup : IDisposable
    {
        public List<UISearchCondition> Conditions { get; private set; }
        public List<UISearchGroup> Groups { get; private set; }
        public UISearchGroup ParentGroup { get; private set; }
        public ucModule ParentModule { get; private set; }
        public IConditionFieldSupportedModule ConditionModule
        {
            get
            {
                return (IConditionFieldSupportedModule) ParentModule;
            }
        }
        public LayoutControlGroup ParentLayoutGroup { get; private set; }
        public LayoutControlGroup LayoutGroup { get; private set; }
        public ModuleInfo ModuleInfo { get; private set; }
        public LayoutControl SearchLayout { get; private set; }

        private readonly CheckEdit m_EditUse;
        private readonly ImageComboBoxEdit m_SQLLogic;
        private readonly SimpleButton m_AddGroup;
        private readonly SimpleButton m_AddCondition;
        private readonly SimpleButton m_DeleteGroup;

        private readonly LayoutControlItem m_EditUseLayoutItem;
        private readonly LayoutControlItem m_SQLLogicLayoutItem;
        private readonly EmptySpaceItem m_SQLLogicEmptySpace;
        private readonly LayoutControlItem m_AddGroupLayoutItem;
        private readonly LayoutControlItem m_AddConditionLayoutItem;
        private readonly LayoutControlItem m_DeleteGroupLayoutItem;
        private readonly SimpleSeparator m_MainSeparator;

        public bool InUse
        {
            get
            {
                return m_EditUse.Checked;
            }
            set
            {
                m_EditUse.Checked = value;
                
                foreach (var group in Groups)
                {
                    group.InUse = value;
                }

                foreach (var condition in Conditions)
                {
                    condition.InUse = value;
                }
            }
        }

        public UISearchGroup(ucModule parentModule, LayoutControlGroup parentLayoutGroup)
        {
            ParentModule = parentModule;
            ModuleInfo = parentModule.ModuleInfo;
            ParentLayoutGroup = parentLayoutGroup;
            SearchLayout = (LayoutControl)parentLayoutGroup.Owner;
            
            Groups = new List<UISearchGroup>();
            Conditions = new List<UISearchCondition>();
            
            LayoutGroup = new LayoutControlGroup();

            SearchLayout.BeginUpdate();
            parentLayoutGroup.BeginUpdate();

            m_EditUse = new CheckEdit();
            // GetFields: ModID = ModuleTypeID, Group = SQL_EXPRESSION
            var fieldSqlLogic =
                FieldUtils.GetModuleFields(
                    ModuleInfo.ModuleType,
                    CODES.DEFMODFLD.FLDGROUP.SQL_EXPRESSION
                )[0];
            //
            m_SQLLogic = (ImageComboBoxEdit)parentModule.CreateControl(fieldSqlLogic);
            parentModule.SetControlListSource(m_SQLLogic);
            parentModule.SetControlDefaultValue(m_SQLLogic);
            
            m_AddGroup = new SimpleButton();
            m_AddCondition = new SimpleButton();
            m_DeleteGroup = new SimpleButton();

            SearchLayout.Controls.AddRange(new Control[] { m_EditUse, m_SQLLogic, m_AddGroup, m_AddCondition, m_DeleteGroup });

            (m_EditUseLayoutItem = LayoutGroup.AddItem("Use Condition", m_EditUse)).BeginInit();
            (m_MainSeparator = (SimpleSeparator)LayoutGroup.AddItem(new SimpleSeparator(), m_EditUseLayoutItem, InsertType.Bottom)).BeginInit();
            (m_SQLLogicLayoutItem = LayoutGroup.AddItem("Group Type", m_SQLLogic, m_EditUseLayoutItem, InsertType.Right)).BeginInit();
            (m_SQLLogicEmptySpace = (EmptySpaceItem)LayoutGroup.AddItem(new EmptySpaceItem(LayoutGroup), m_SQLLogicLayoutItem, InsertType.Right)).BeginInit();
            (m_DeleteGroupLayoutItem = LayoutGroup.AddItem("Delete Group", m_DeleteGroup,m_SQLLogicEmptySpace, InsertType.Right)).BeginInit();
            (m_AddGroupLayoutItem = LayoutGroup.AddItem("Add Group", m_AddGroup, m_DeleteGroupLayoutItem, InsertType.Right)).BeginInit();
            (m_AddConditionLayoutItem = LayoutGroup.AddItem("Add Condition", m_AddCondition, m_AddGroupLayoutItem, InsertType.Right)).BeginInit();

            // m_chkEditUse
            m_EditUse.Text = "";
            m_EditUse.Checked = true;
            m_EditUse.EditValueChanged += CheckEditUse_CheckedChanged;

            // m_btnDeleteGroup
            parentModule.Language.FormatButton(m_DeleteGroup, "btnDeleteGroupCondition");
            m_DeleteGroup.Click += DeleteGroupCondition_Click;

            // m_btnAddGroup
            parentModule.Language.FormatButton(m_AddGroup, "btnAddGroupCondition");
            m_AddGroup.Click += AddGroupCondition_Click;

            // m_btnAddCondition
            parentModule.Language.FormatButton(m_AddCondition, "btnAddCondition");
            m_AddCondition.Click += AddConditionButton_Click;

            // LayoutGroup
            LayoutGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(9, 2, 2, 2);
            LayoutGroup.TextLocation = DevExpress.Utils.Locations.Left;
            LayoutGroup.Text = " ";

            // m_layout_chkEditUse
            m_EditUseLayoutItem.TextVisible = false;
            m_EditUseLayoutItem.SizeConstraintsType = SizeConstraintsType.Custom;
            m_EditUseLayoutItem.ControlAlignment = ContentAlignment.MiddleCenter;
            m_EditUseLayoutItem.FillControlToClientArea = false;
            m_EditUseLayoutItem.MinSize =
                m_EditUseLayoutItem.MaxSize = new Size(20, 24);

            // m_layout_cboSQLLogic
            m_SQLLogicLayoutItem.TextVisible = false;
            m_SQLLogicLayoutItem.SizeConstraintsType = SizeConstraintsType.Custom;
            m_SQLLogicLayoutItem.ControlAlignment = ContentAlignment.MiddleCenter;
            m_EditUseLayoutItem.FillControlToClientArea = false;
            m_SQLLogicLayoutItem.MinSize =
                m_SQLLogicLayoutItem.MaxSize = new Size(240, 24);

            // m_layout_btnDeleteGroup
            m_DeleteGroupLayoutItem.TextVisible = false;
            m_DeleteGroupLayoutItem.SizeConstraintsType = SizeConstraintsType.Custom;
            m_DeleteGroupLayoutItem.ControlAlignment = ContentAlignment.MiddleCenter;
            m_EditUseLayoutItem.FillControlToClientArea = false;
            m_DeleteGroupLayoutItem.MinSize =
                m_DeleteGroupLayoutItem.MaxSize = new Size(140, 24);

            // m_layout_cboSQLLogic
            m_AddGroupLayoutItem.TextVisible = false;
            m_AddGroupLayoutItem.SizeConstraintsType = SizeConstraintsType.Custom;
            m_AddGroupLayoutItem.ControlAlignment = ContentAlignment.MiddleCenter;
            m_EditUseLayoutItem.FillControlToClientArea = false;
            m_AddGroupLayoutItem.MinSize =
                m_AddGroupLayoutItem.MaxSize = new Size(140, 24);
            
            // m_layout_cboSQLLogic
            m_AddConditionLayoutItem.TextVisible = false;
            m_AddConditionLayoutItem.SizeConstraintsType = SizeConstraintsType.Custom;
            m_AddConditionLayoutItem.ControlAlignment = ContentAlignment.MiddleCenter;
            m_EditUseLayoutItem.FillControlToClientArea = false;
            m_AddConditionLayoutItem.MinSize =
                m_AddConditionLayoutItem.MaxSize = new Size(110, 24);

            m_MainSeparator.FillControlToClientArea = false;
            m_MainSeparator.SizeConstraintsType = SizeConstraintsType.Default;

            m_EditUseLayoutItem.EndInit();
            m_SQLLogicLayoutItem.EndInit();
            m_SQLLogicEmptySpace.EndInit();
            m_DeleteGroupLayoutItem.EndInit();
            m_AddGroupLayoutItem.EndInit();
            m_AddConditionLayoutItem.EndInit();
            m_MainSeparator.EndInit();

            parentLayoutGroup.Add(LayoutGroup);
            
            if (ConditionModule.ConditionLayoutGroup == parentLayoutGroup)
            {
                parentLayoutGroup.AddItem(new EmptySpaceItem(), LayoutGroup, InsertType.Bottom);
                m_DeleteGroupLayoutItem.Visibility = LayoutVisibility.Never;
            }

            parentLayoutGroup.EndUpdate();
            SearchLayout.EndUpdate();
        }

        void DeleteGroupCondition_Click(object sender, EventArgs e)
        {
            ParentGroup.Groups.Remove(this);
            Dispose();
            ConditionModule.UpdateConditionQuery();
        }

        void CheckEditUse_CheckedChanged(object sender, EventArgs e)
        {
            ParentLayoutGroup.Update();
            InUse = m_EditUse.Checked;
        }

        void AddConditionButton_Click(object sender, EventArgs e)
        {
            try
            {
                SearchLayout.SuspendLayout();
                new UISearchCondition(ModuleInfo, this);
                SearchLayout.ResumeLayout();
            }
            catch (Exception ex)
            {
                ParentModule.ShowError(ex);
            }
        }

        public void CreateFixedCondition(ModuleFieldInfo fieldInfo)
        {
            try
            {
                SearchLayout.SuspendLayout();
                var newCondition = new UISearchCondition(ModuleInfo, this);
                newCondition.SetCondition(fieldInfo);
                SearchLayout.ResumeLayout();
                ConditionModule.UpdateConditionQuery();
            }
            catch (Exception ex)
            {
                ParentModule.ShowError(ex);
            }
        }

        void AddGroupCondition_Click(object sender, EventArgs e)
        {
            CreateSubUISearchGroup();
        }

        public UISearchGroup CreateSubUISearchGroup()
        {
            var subGroup = new UISearchGroup(ParentModule, LayoutGroup);
            Groups.Add(subGroup);
            subGroup.ParentGroup = this;
            return subGroup;
        }

        public SearchConditionInstance GetConditionInstance()
        {
            var instance = new SearchConditionInstance {SQLLogic = m_SQLLogic.EditValue.ToString()};

            var subInstance = new List<SearchConditionInstance>();
            foreach (var group in Groups)
            {
                if(group.InUse)
                    subInstance.Add(group.GetConditionInstance());
            }

            foreach (var condition in Conditions)
            {
                if(condition.InUse)
                    subInstance.Add(condition.GetConditionInstance());
            }

            instance.SubCondition = subInstance.ToArray();

            return instance;
        }

        public void Dispose()
        {
            foreach (var group in Groups)
            {
                group.Dispose();
            }

            foreach (var condition in Conditions)
            {
                condition.Dispose();
            }

            if (ParentGroup != null)
            {
                ParentLayoutGroup.Remove(LayoutGroup);
            }

            LayoutGroup.Dispose();
            m_EditUse.Dispose();
            m_SQLLogic.Dispose();
            m_AddGroup.Dispose();
            m_AddCondition.Dispose();
            m_DeleteGroup.Dispose();
            m_SQLLogicLayoutItem.Dispose();
            m_MainSeparator.Dispose();
            m_SQLLogicEmptySpace.Dispose();
            m_EditUseLayoutItem.Dispose();
            m_AddGroupLayoutItem.Dispose();
            m_AddConditionLayoutItem.Dispose();
            m_DeleteGroupLayoutItem.Dispose();
        }
    }

    public class UISearchCondition : IDisposable
    {
        public UISearchGroup Group { get; set; }
        public ucModule ParentModule { get; set; }
        public IConditionFieldSupportedModule ConditionModule
        {
            get
            {
                return (IConditionFieldSupportedModule)ParentModule;
            }
        }
        public ModuleInfo m_ModuleInfo;
        public ModuleFieldInfo FieldInfo
        {
            get
            {
                return (ModuleFieldInfo)m_Condition.EditValue;
            }
        }
        public object Value
        {
            get
            {
                return m_EditValue.EditValue;
            }
            set
            {
                m_EditValue.EditValue = value;
            }
        }

        private readonly LayoutControlGroup m_ConditionGroupLayout;
        private readonly CheckEdit m_EditUse;
        private readonly ImageComboBoxEdit m_Condition;
        private ImageComboBoxEdit m_Operator;
        private BaseEdit m_EditValue;
        private readonly SimpleButton m_RemoveButton;
        private readonly LayoutControlItem m_EditUseLayoutItem;
        private readonly LayoutControlItem m_ConditionLayoutItem;
        private readonly LayoutControlItem m_OperatorLayoutItem;
        private readonly LayoutControlItem m_EditValueLayoutItem;
        private readonly LayoutControlItem m_RemoveButtonLayoutItem;

        public bool InUse
        {
            get
            {
                return m_EditUse.Checked;
            }
            set
            {
                m_EditUse.Checked = value;
            }
        }

        public UISearchCondition(ModuleInfo moduleInfo, UISearchGroup group)
        {
            Group = group;
            m_ModuleInfo = moduleInfo;
            Group.Conditions.Add(this);
            ParentModule = group.ParentModule;
            
            // NewGroup
            m_ConditionGroupLayout = group.LayoutGroup.AddGroup();
            m_ConditionGroupLayout.GroupBordersVisible = false;

            // CheckEdit Use
            m_EditUse = new CheckEdit {TabStop = false, Text = ""};
            // -> Layout
            m_EditUseLayoutItem = m_ConditionGroupLayout.AddItem("", m_EditUse);
            m_EditUseLayoutItem.TextVisible = false;
            m_EditUseLayoutItem.SizeConstraintsType = SizeConstraintsType.Custom;
            m_EditUseLayoutItem.MaxSize =
                m_EditUseLayoutItem.MinSize = new Size(24, 22);
            m_EditUse.CheckedChanged += CheckEditUse_CheckedChanged;

            // Condition ComboBox
            m_Condition = new ImageComboBoxEdit {TabStop = false};
#if DEBUG
            var button = new EditorButton(ButtonPredefines.Up);
            button.Tag = "DEBUG_EDIT";
            m_Condition.Properties.Buttons.Add(button);
            m_Condition.ButtonClick += delegate(object sender, ButtonPressedEventArgs e)
            {
                var fieldInfo = m_Condition.EditValue as ModuleFieldInfo;
                if (e.Button == button)
                {
                    var ucModule = MainProcess.CreateModuleInstance("02905", "MED");
                    ucModule["P01"] = fieldInfo.ModuleID;
                    ucModule["C01"] = fieldInfo.FieldID;
                    ucModule.ShowDialogModule(ParentModule);
                }
            };
#endif
            m_Condition.EditValueChanged += cboCondition_EditValueChanged;
            // -> Layout
            m_ConditionLayoutItem = m_ConditionGroupLayout.AddItem("", m_Condition, m_EditUseLayoutItem, InsertType.Right);
            m_ConditionLayoutItem.TextVisible = false;
            m_ConditionLayoutItem.SizeConstraintsType = SizeConstraintsType.Custom;
            m_ConditionLayoutItem.MinSize =
                m_ConditionLayoutItem.MaxSize = new Size(320, 22);

            // Operator ComboBox
            // -> Layout
            m_OperatorLayoutItem = m_ConditionGroupLayout.AddItem(m_ConditionLayoutItem, InsertType.Right);

            // Editor Value
            // -> Layout
            m_EditValueLayoutItem = m_ConditionGroupLayout.AddItem(m_OperatorLayoutItem, InsertType.Right);

            // Button Remove
            m_RemoveButton = new SimpleButton
                                {
                                    TabStop = false
                                };
            ParentModule.Language.FormatButton(m_RemoveButton, "btnRemoveCondition");

            m_RemoveButton.Click += m_btnRemove_Click;
            // -> Layout
            m_RemoveButtonLayoutItem = m_ConditionGroupLayout.AddItem("", m_RemoveButton, m_EditValueLayoutItem, InsertType.Right);
            m_RemoveButtonLayoutItem.TextVisible = false;
            m_RemoveButtonLayoutItem.SizeConstraintsType = SizeConstraintsType.Custom;
            m_RemoveButtonLayoutItem.MinSize =
                m_RemoveButtonLayoutItem.MaxSize = new Size(100, 22);

            group.LayoutGroup.Add(m_ConditionGroupLayout);

            InitializeConditionComboBox();
            ConditionModule.UpdateConditionQuery();
            m_EditUse.Checked = true;
        }

        void m_btnRemove_Click(object sender, EventArgs e)
        {
            Group.LayoutGroup.BeginUpdate();
            Group.Conditions.Remove(this);
            Group.LayoutGroup.Remove(m_ConditionGroupLayout);
            m_ConditionGroupLayout.Clear();
            Dispose();
            Group.LayoutGroup.EndUpdate();
            ConditionModule.UpdateConditionQuery();
        }

        void CheckEditUse_CheckedChanged(object sender, EventArgs e)
        {
            m_Condition.Enabled = m_EditUse.Checked;
            m_Operator.Enabled = m_EditUse.Checked;
            m_EditValue.Enabled = m_EditUse.Checked;
            ConditionModule.UpdateConditionQuery();
        }

        void cboCondition_EditValueChanged(object sender, EventArgs e)
        {
            if (m_Operator != null)
                m_Operator.EditValueChanged -= EditValue_EditValueChanged;
            if (m_EditValue != null)
                m_EditValue.EditValueChanged -= EditValue_EditValueChanged;

            UpdateOperatorComboBox();
            UpdateConditionValueControl();

            if (m_Operator != null)
                m_Operator.EditValueChanged += EditValue_EditValueChanged;
            if (m_EditValue != null)
                m_EditValue.EditValueChanged += EditValue_EditValueChanged;
        }

        private void UpdateOperatorComboBox()
        {
            // Lấy thông tin Field được chọn
            var field = m_Condition.EditValue as ModuleFieldInfo;
            // Lấy các phép toán tương ứng với FieldType
            if (field != null)
            {
                var opField = FieldUtils.GetModuleFieldByID(
                    ParentModule.ModuleInfo.ModuleType,
                    field.ConditionType);

                m_ConditionGroupLayout.BeginUpdate();
                m_EditValueLayoutItem.BeginInit();

                if (m_Operator != null)
                {
                    m_OperatorLayoutItem.Control = null;
                    m_Operator.Parent = null;
                }

                m_Operator = (ImageComboBoxEdit)ParentModule.CreateControl(opField);
                ParentModule.SetControlListSource(m_Operator);
            }

            m_OperatorLayoutItem.Control = m_Operator;
            m_OperatorLayoutItem.TextVisible = false;
            m_OperatorLayoutItem.SizeConstraintsType = SizeConstraintsType.Custom;
            m_OperatorLayoutItem.MinSize =
                m_OperatorLayoutItem.MaxSize = new Size(180, 1);

            m_EditValueLayoutItem.EndInit();
            m_ConditionGroupLayout.EndUpdate();

            ParentModule.SetControlDefaultValue(m_Operator);
        }

        private void UpdateConditionValueControl()
        {
            m_ConditionGroupLayout.BeginUpdate();
            m_EditValueLayoutItem.BeginInit();
            if (m_EditValue != null)
            {
                m_EditValueLayoutItem.Control = null;
                m_EditValue.Parent = null;
            }

            // Lấy thông tin Field được chọn
            var fieldInfo = m_Condition.EditValue as ModuleFieldInfo;
            // Tạo Control theo đúng field
            m_EditValue = (ParentModule.CreateControl(fieldInfo)) as BaseEdit;
            ParentModule.SetupControlListSource(fieldInfo, m_EditValue);
            m_EditValueLayoutItem.Control = m_EditValue;
            m_EditValueLayoutItem.TextVisible = false;

            m_EditValueLayoutItem.EndInit();
            m_ConditionGroupLayout.EndUpdate();
            ParentModule.SetControlDefaultValue(m_EditValue);
        }

        void EditValue_EditValueChanged(object sender, EventArgs e)
        {
            ConditionModule.UpdateConditionQuery();
        }

        public void InitializeConditionComboBox()
        {
            var haveWhereExtension = false;
            m_Condition.SuspendLayout();
            m_Condition.Properties.Items.Clear();
            foreach (var item in ParentModule.ConditionFields)
            {
                var haveToCreateItem = true;
                if (item.WhereExtension == "Y")
                {
                    haveWhereExtension = true;
                    if (Group == ConditionModule.RootGroup)
                    {
                        var count = (from condition in Group.Conditions
                                     where condition != this && (condition.m_Condition.EditValue as ModuleFieldInfo).FieldID == item.FieldID
                                     select 1).Count();

                        if (count > 0) haveToCreateItem = false;
                    }
                }

                if(haveToCreateItem)
                    m_Condition.Properties.Items.Add(
                        new ImageComboBoxItem
                        {
                            Description = ParentModule.Language.GetLabelText(item.FieldName),
                            Value = item
                        }
                    );
            }

            m_Condition.SelectedIndex = 0;
            if (haveWhereExtension)
            {
                m_Condition.QueryPopUp -= m_Condition_QueryPopup;
                m_Condition.QueryPopUp += m_Condition_QueryPopup;
            }
            m_Condition.ResumeLayout();
        }

        void m_Condition_QueryPopup(object sender, CancelEventArgs e)
        {
            InitializeConditionComboBox();
        }

        public SearchConditionInstance GetConditionInstance()
        {
            var instance = new SearchConditionInstance();
            var objConditionInfo = (ModuleFieldInfo)m_Condition.EditValue;
            instance.ConditionID = objConditionInfo.FieldID;
            instance.Operator = m_Operator.EditValue.ToString();
            instance.Value = m_EditValue.EditValue.Encode(objConditionInfo);
            if (string.IsNullOrEmpty(objConditionInfo.FunctionSCDValue) != true)
            {
                using (var ctrlSA = new SAController())
                {
                    List<string> values;
                    DataContainer container = null;

                    DataSet dsResult;
                    values = new List<string>();
                    values.Add(objConditionInfo.FunctionSCDValue + "('" + instance.Value + "')");
                    ctrlSA.ExecuteProcedureFillDataset(out container, "sp_get_condition_value", values);
                    dsResult = container.DataSet;
                    if (dsResult.Tables.Count == 1)
                    {
                        instance.Value = dsResult.Tables[0].Rows[0][0].ToString();
                    }

                }
            }                       
            return instance;
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_ConditionGroupLayout.Dispose();
            m_EditUse.Dispose();
            m_Condition.Dispose();
            m_Operator.Dispose();
            m_EditValue.Dispose();
            m_RemoveButton.Dispose();
            m_EditUseLayoutItem.Dispose();
            m_ConditionLayoutItem.Dispose();
            m_OperatorLayoutItem.Dispose();
            m_EditValueLayoutItem.Dispose();
            m_RemoveButtonLayoutItem.Dispose();
        }

        #endregion

        internal void SetCondition(ModuleFieldInfo fieldInfo)
        {
            m_Condition.EditValue = fieldInfo;
        }
    }
}
