using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTab;
using FIS.AppClient.Interface;
using FIS.Base;
using FIS.CODES.DEFMODFLD;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Extensions;
using FIS.Utils;
using System.IO;
using System.Text;
using DevExpress.XtraGrid.Views.BandedGrid;
using System.Xml.Serialization;
using System.Data;
using DevExpress.Utils.Menu;
using DevExpress.Utils.Drawing;
using DevExpress.Data;
using DevExpress.XtraEditors.Mask;
using System.ServiceModel;
using Padding = System.Windows.Forms.Padding;
using FIS.AppClient.Utils;
using System.Linq;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.XtraRichEdit;
using FIS.CODES.DEFGROUPSUMMARY;

namespace FIS.AppClient.Controls
{
    public partial class ucModule :  XtraUserControl, IModule, IFormattable
    {
        #region Parameter Fields
        /// <summary>
        /// Danh sách Parameter Field
        /// </summary>
        protected List<ModuleFieldInfo> ParameterFields { get; set; }
        public GridView view,view1,view2;
        int i = 0;
        protected string SecID;
        double TotalRowCount; double TotalValue;
        /// <summary>
        /// Giá trị Parameter theo FieldInfo
        /// </summary>
        protected Dictionary<string, object> ParameterByFieldID { get; set; }
        #endregion

        #region Maintain Fields
        /// <summary>
        /// Danh sách Fields maintain
        /// </summary>
        protected List<ModuleFieldInfo> CommonFields { get; set; }

        /// <summary>
        /// Control theo mã Field
        /// </summary>
        protected Dictionary<string, BaseEdit> CommonControlByID { get; set; }
        protected Dictionary<string, Control> _CommonControlByID { get; set; }

        [Browsable(false)]
        public Dictionary<string, BaseLayoutItem> CommonLayoutItemByID {get;set;}
        #endregion
     
        #region Grid Column Fields
        [Browsable(false)]
        public List<ModuleFieldInfo> ColumnFields { get; set; }

        [Browsable(false)]
        public List<GroupSummaryInfo> GroupSummaries { get; set; }

        [Browsable(false)]
        public List<ModuleFieldInfo> GroupFields { get; set; }
        public GridCheckMarksUtils CheckMarksUtils { get; set; }
        public DataRow selectedRows;
        [Browsable(false)]
        public Dictionary<string, GridColumn> GridColumnByID { get; set; }

        [Browsable(false)]
        public Dictionary<string, RepositoryItemImageComboBox> StoreRepositories { get; set; }

        protected List<GridBand> Bands { get; set; }
        #endregion

        #region Condition & Static Condition Fields
        [Browsable(false)]
        public List<ModuleFieldInfo> ConditionFields { get; protected set; }
        #endregion

        #region Property & Members
        public static List<ucModule> AliveModuleIntances { get; set; }
//#if DEBUG
        protected ContextMenuStrip Context { get; private set; }
//#endif
        [Category("Buttons")]
        public SimpleButton CancelButton { get; set; }
        [Category("Buttons")]
        public SimpleButton AcceptButton { get; set; }
        protected object m_ModuleLanguage;

        /// <summary>
        /// Thông tin Module
        /// </summary>
        [Browsable(false)]
        public ModuleInfo ModuleInfo { get; set; }

        /// <summary>
        /// Tên đầy đủ của module
        /// Format: Constants.MODULE_FULLNAME_FORMAT
        /// </summary>
        [Browsable(false)]
        public string ModuleFullName { get; set; }

        private bool m_PauseCallback;
        [Browsable(false)]
        public bool PauseCallback
        {
            get { return m_PauseCallback; }
            set { m_PauseCallback = value; }
        }
       
        [Browsable(false)]
        public bool StopCallback { get; set; }
        public bool IsFile { get; set; }

        /// <summary>
        /// Lấy giá trị của field theo FieldID
        /// </summary>
        /// <param name="fieldID">Mã field</param>
        /// <returns>Giá trị của Field</returns>
        public virtual object this[string fieldID]
        {
            get
            {
                if (CommonControlByID != null && CommonControlByID.ContainsKey(fieldID))
                {
                    return CommonControlByID[fieldID].EditValue;
                }

                if (ParameterByFieldID.ContainsKey(fieldID))
                    return ParameterByFieldID[fieldID];

                throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_MODULE_FIELD_NOT_FOUND_OR_DUPLICATE,
                    "ModuleFieldByID", ModuleFullName, fieldID);
            }

            set
            {
                if (CommonControlByID != null && CommonControlByID.ContainsKey(fieldID))
                {
                    var commonControl = CommonControlByID[fieldID];
                    var field = (ModuleFieldInfo)commonControl.Tag;

                    if (commonControl is ComboBoxEdit)
                    {
                        LoadComboxListSource(((ComboBoxEdit)commonControl).Properties);
                    }

                    commonControl.EditValue = value.DecodeAny(field);

                    return;
                }
                
                if (ParameterByFieldID.ContainsKey(fieldID))
                {
                    ParameterByFieldID[fieldID] = value;
                    return;
                }

                throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_MODULE_FIELD_NOT_FOUND_OR_DUPLICATE,
                    "ModuleFieldByID", ModuleInfo.ModuleID, fieldID);
            }
        }

        /// <summary>
        /// Sự kiện Module bị đóng
        /// </summary>
        public event EventHandler ModuleClosed;
        /// <summary>
        /// Sự kiện Module đang đóng
        /// </summary>
        public event CancelEventHandler ModuleClosing;
        /// <summary>
        /// Yêu cầu module cha refresh lại dữ liệu
        /// </summary>
        [Browsable(false)]
        public bool RequireRefresh { get; set; }
        [Browsable(false)]
        public WorkerThread CurrentThread { get; set; }
        [Browsable(false)]
        public ModuleLanguage Language { get; set; }
        [Browsable(false)]
        public ucModulePreview ucPreview { get; set; }
        #endregion

        #region Initialize Methods
        /// <summary>
        /// Initialize Module
        /// Task:
        ///  - Initialize GUI
        ///  - Initialize UIType
        ///  - Initialize Buttons
        ///  - Initialize Module Fields
        /// </summary>
        /// <param name="moduleInfo">Thông tin Module</param>
        public virtual void InitializeModuleInfo(ModuleInfo moduleInfo)
        {
//#if DEBUG
            //if (frmMain.m_BarFieldMaker != null)
            //{
            //    frmMain.RemoveResourceBarItem(frmMain.m_BarFieldMaker);
            //    frmMain.m_BarFieldMaker = null;
            //}
            
            //Context = new ContextMenuStrip();
            //frmMain.ForceLoad(moduleInfo.ModuleID);
            Context = new ContextMenuStrip();
            MainProcess.ForceLoad(moduleInfo.ModuleID);
//#endif
            switch(moduleInfo.ExecuteMode)
            {
                case CODES.DEFMOD.EXECMODE.SINGLE_INSTANCE:
                    foreach (var instance in AliveModuleIntances)
                    {
                        var info = instance.ModuleInfo;
                        if (info.ModuleID == moduleInfo.ModuleID &&
                            info.SubModule == moduleInfo.SubModule)
                        {
                            if (info.UIType == CODES.DEFMOD.UITYPE.TABPAGE)
                            {
                                instance.Parent.Focus();
                            }
                            //throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_MODULE_SINGLE_INSTANCE);
                        }
                    }                
                    break;
                case CODES.DEFMOD.EXECMODE.BLOCKED:
                    throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_BLOCKED_MODULE);
            }

            ModuleInfo = moduleInfo;
            RequireRefresh = false;
            ModuleFullName =
                string.Format(
                    CONSTANTS.MODULE_FULLNAME_FORMAT,
                    moduleInfo.ModuleTypeName,
                    moduleInfo.ModuleName
                );
            if (!string.IsNullOrEmpty(moduleInfo.RoleID))
            {
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.CheckRole(moduleInfo);
                }
            }
            //add by TrungTT - 27.12.2012 - Userlog
            try
            {
                using (var client = new SAController())
                {
                    if (moduleInfo.ModuleID != "LOGIN")
                        client.ExecuteUsersLog(moduleInfo.ModuleID, moduleInfo.ModuleName);
                }
            }
            catch
            {
            }
            //End TrungTT
            //
            InitializeLanguage();
            // Initialize GUI
            var skin = CommonSkins.GetSkin(UserLookAndFeel.Default);
            InitializeGUI(skin);
            // Initialize UIType
            SettingUIType();
            //
            SuspendLayout();
            // Initialize Module Fields
            BuildFields();
            // Initialize Layout
            InitializeLayout();
            // Initialize Buttons
            BuildButtons();
            ResumeLayout();
            // Add to alive instances
            AliveModuleIntances.Add(this);
        }

        /// <summary>
        /// Thiết đặt lại Layout đã save
        /// </summary>
        public virtual void InitializeLayout()
        {
            // 1. If module implement ICommonFieldSupportedModule
            var commonFieldSupportedModule = this as ICommonFieldSupportedModule;
            if(commonFieldSupportedModule != null)
            {
                InitializeCommonFieldLayout(commonFieldSupportedModule);
            }

            // 2. If module implement IGroupColumnFieldSupportedModule
            var groupColumnFieldSupportedModule = this as IGroupColumnFieldSupportedModule;
            if(groupColumnFieldSupportedModule != null)
            {
                InitializeGroupColumnFieldLayout(groupColumnFieldSupportedModule);
            }
            //add by TrungTT - 08.12.2011 - Customize Layout - Source by LongND5
            // 3. InitializeColumnFieldLayout
            var columnFieldSupportedModule = this as IColumnFieldSupportedModule;
            if (columnFieldSupportedModule != null)
            {
                InitializeColumnFieldLayout(columnFieldSupportedModule);
            }
            //End TrungTT
        }
        //add by TrungTT - 08.12.2011 - Customize Layout - Source by LongND5
        public virtual void InitializeColumnFieldLayout(IColumnFieldSupportedModule columnFieldSupportedModule)
        {
            DataContainer containerColumn;
            string extraValue;
            List<string> lstParam = new List<string>();
            var extraProperty = ModuleUtils.GetProfileExtraProperty(ModuleInfo, CONSTANTS.COLUMNS_VISIBLE_INDEXES);
            lstParam.Add(extraProperty);
            DataSet ds = new DataSet();

            if (!ModuleUtils.IsProfileExtraPropertyExisted(ModuleInfo, CONSTANTS.COLUMNS_VISIBLE_INDEXES))
            {
                try
                {
                    using (var client = new SAController())
                    {
                        client.GetExtraCurrentUserProfile(extraProperty, out extraValue);
                        ModuleUtils.SetExtraProfile(ModuleInfo, CONSTANTS.COLUMNS_VISIBLE_INDEXES, extraValue);
                    }
                }
                catch
                {
                    ModuleUtils.SetExtraProfile(ModuleInfo, CONSTANTS.COLUMNS_VISIBLE_INDEXES, null);
                }
            }

            try
            {
                using (var client = new SAController())
                {
                    client.ExecuteProcedureFillDataset(out containerColumn, "sp_get_profile_columns", lstParam);
                    ds = containerColumn.DataSet;
                }
            }
            catch
            { }
            if (ds.Tables[0].Rows.Count != 0)
            {
                extraValue = ds.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                extraValue = null;
            }
            //extraValue = ModuleUtils.GetProfileExtraValue(ModuleInfo, CONSTANTS.COLUMNS_VISIBLE_INDEXES);
            if (!string.IsNullOrEmpty(extraValue))
            {
                var visibleIndexes = extraValue.Split(new[] { "," }, StringSplitOptions.None).Select(item => int.Parse(item)).ToArray();
                for (var i = 0; i < visibleIndexes.Length; i++)
                {
                    columnFieldSupportedModule.GridView.Columns[i].VisibleIndex = visibleIndexes[i];
                }
            }

            columnFieldSupportedModule.GridView.ColumnPositionChanged +=
                delegate
                {
                    try
                    {
                        var visibleIndexes = (from GridColumn column in columnFieldSupportedModule.GridView.Columns
                                              where column.Name != "CheckMarkSelection"
                                              select column.VisibleIndex.ToString()).ToList();
                        extraValue = string.Join(",", visibleIndexes.ToArray());

                        using (var client = new SAController())
                        {
                            client.SetExtraCurrentUserProfile(extraProperty, extraValue);
                        }

                        ModuleUtils.SetExtraProfile(ModuleInfo, CONSTANTS.COLUMNS_VISIBLE_INDEXES, extraValue);
                    }
                    catch
                    {
                    }
                };
        }
        //End TrungTT
        public virtual void InitializeGroupColumnFieldLayout(IGroupColumnFieldSupportedModule groupColumnFieldSupportedModule)
        {
#if DEBUG
            groupColumnFieldSupportedModule.GridView.OptionsView.ShowColumnHeaders = true;
#endif
            if(!string.IsNullOrEmpty(groupColumnFieldSupportedModule.GroupLayoutStoredData))
            {
                var sr = new StringReader(groupColumnFieldSupportedModule.GroupLayoutStoredData);
                var serializer = new XmlSerializer(typeof(List<string[]>));
                var listLayout = (List<string[]>)serializer.Deserialize(sr);
                foreach (var item in listLayout)
                {
                    foreach (var parent in Bands)
                    {
                        foreach (var child in Bands)
                        {
                            if (parent.Name == item[1] && child.Name == item[0])
                            {
                                parent.Children.Add(child);
                            }
                        }
                    }
                }                
            }
        }

        protected void RestoreCommonFieldLayout(ICommonFieldSupportedModule commonFieldSupportedModule)
        {
            var layout = commonFieldSupportedModule.CommonLayout;
            if (!string.IsNullOrEmpty(commonFieldSupportedModule.CommonLayoutStoredData))
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(commonFieldSupportedModule.CommonLayoutStoredData)))
                {
                    layout.RestoreLayoutFromStream(ms);
                    ms.Close();
                }
            }
            else
            {
                foreach (var control in CommonControlByID.Values)
                {
                    var item = layout.AddItem(control.Name, control);
                    item.CustomizationFormText = item.Name = string.Format("item{0}", control.Name);
                    item.Text = control.Name;
                }
                //add by TrungTT - 23.11.2012 - Add Grid into Layout
                foreach (var control in _CommonControlByID.Values)
                {
                    var item = layout.AddItem(control.Name, control);
                    item.CustomizationFormText = item.Name = string.Format("item{0}", control.Name);
                    item.Text = control.Name;
                }
                //End TrungTT
            }
        }

        /// <summary>
        /// Thiết đặt Layout Common Field đã save
        /// </summary>
        /// <param name="commonFieldSupportedModule">Module cần thiết đặt</param>
        protected virtual void InitializeCommonFieldLayout(ICommonFieldSupportedModule commonFieldSupportedModule)
        {
            RestoreCommonFieldLayout(commonFieldSupportedModule);

            var layout = commonFieldSupportedModule.CommonLayout;
            foreach(var field in CommonFields)
            {
                switch(field.ControlType)
                {
                    case CODES.DEFMODFLD.CTRLTYPE.DEFINEDGROUP:
                        var item = layout.Items.FindByName(field.FieldName) as LayoutControlGroup ?? layout.AddGroup();
                        item.Name = field.FieldName;
                        item.CaptionImage = Language.GetButtonIcon16(field.FieldName);
                        CommonLayoutItemByID.Add(field.FieldID, item);
                        break;
                    default:
                        var control = (BaseEdit)GetControlByFieldID(field.FieldID);                                                
                        if (field.Nullable == CONSTANTS.No)
                        {
                            try
                            {
                                layout.Items.FindByName("item" + field.FieldName).AppearanceItemCaption.ForeColor = GetColor(SysvarUtils.GetVarValue("SYS", "ISNULLCOLOR"));
                                layout.Items.FindByName("item" + field.FieldName).Text = 
                                    layout.Items.FindByName("item" + field.FieldName).Text.IndexOf(SysvarUtils.GetVarValue("SYS", "ISNULLTEXT")) > 0 ?  layout.Items.FindByName("item" + field.FieldName).Text :
                                    layout.Items.FindByName("item" + field.FieldName).Text + " " + SysvarUtils.GetVarValue("SYS", "ISNULLTEXT");
                            }
                            catch
                            { }                            
                            CommonLayoutItemByID.Add(field.FieldID, layout.GetItemByControl(control));
                        }
                        else
                        {
                            try
                            {
                                layout.Items.FindByName("item" + field.FieldName).AppearanceItemCaption.ForeColor = Color.Black;
                                layout.Items.FindByName("item" + field.FieldName).Text = layout.Items.FindByName("item" + field.FieldName).Text.Replace(SysvarUtils.GetVarValue("SYS", "ISNULLTEXT"),"");
                            }
                            catch
                            { }     
                            CommonLayoutItemByID.Add(field.FieldID, layout.GetItemByControl(control));
                        }                                                
                        break;
                }
            }

#if DEBUG
            foreach (var control in CommonControlByID.Values)
            {
                var item = layout.GetItemByControl(control);
                if(item.Parent == null)
                {
                    item.Name = item.CustomizationFormText = string.Format("item{0}", control.Name);
                    item.Text = control.Name;
                }
            }
#endif

            layout.LookAndFeel.SkinName = App.Environment.ClientInfo.UserProfile.ApplicationSkinName;
        }
        private Color GetColor(string color)
        {
            Color vColor = new Color();
            switch(color.ToUpper())
            {
                case "RED" : 
                    vColor =  Color.Red;
                    break;
                case "BLUE":
                    vColor = Color.Blue;
                    break;
                default:
                    vColor = Color.Black;
                    break;                  
            }
            return vColor;
        }

        /// <summary>
        /// Nội dung các Language sẽ được sử dụng
        /// </summary>
        public virtual void InitializeLanguage()
        {
            if (Language == null)
            {
                Language = new ModuleLanguage(ModuleInfo);
            }
            Language.Title = LangUtils.TranslateModuleItem(LangType.MODULE_TITLE, ModuleInfo);
            Language.ExecutingStatus = Language.GetSpecialStatus("Executing");
            Language.Size = Language.GetSize();
        }

        /// <summary>
        /// Thiết đặt chế độ hiển thị của Module
        /// </summary>
        public void SettingUIType()
        {
            Dock = DockStyle.Fill;
            //EDIT BY TRUNGTT - 23- 2011 - DOCKING MODULE
            //if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP)
            //{
            //    var frmOwner = new frmModuleBox();
            //    frmOwner.Controls.Add(this);
            //    frmOwner.ucModule = this;
            //    frmOwner.CancelButton = CancelButton;
            //    frmOwner.AcceptButton = AcceptButton;
            //    Parent = frmOwner;

            //    frmOwner.MaximizeBox = (ModuleInfo.UserMax == CODES.DEFMOD.USERMAX.YES);
            //    frmOwner.MinimizeBox = (ModuleInfo.UserHide == CODES.DEFMOD.USERHIDE.YES);

            //    frmOwner.Name = ModuleFullName;
            //    frmOwner.Text = LangUtils.TranslateModuleItem(LangType.MODULE_TITLE, ModuleInfo);

            //    var imageName = LangUtils.TranslateModuleItem(LangType.MODULE_ICON, ModuleInfo);
            //    if (ThemeUtils.Image16.Images.ContainsKey(imageName))
            //        frmOwner.Icon = Icon.FromHandle(((Bitmap)ThemeUtils.Image16.Images[imageName]).GetHicon());

            //    frmOwner.Text += string.Format(" :: {0}", ModuleInfo.ModuleID);
            //    frmOwner.ClientSize = Language.Size;
            //}

            //if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.TABPAGE)
            //{
            //    var tabOwner = new XtraTabPage();
            //    tabOwner.Controls.Add(this);
            //    // Set child & parent
            //    tabOwner.Tag = this;
            //    Parent = tabOwner;
            //    //
            //    tabOwner.Name = ModuleFullName;
            //    tabOwner.Text = LangUtils.TranslateModuleItem(LangType.MODULE_TITLE, ModuleInfo);
            //    var imageName = LangUtils.TranslateModuleItem(LangType.MODULE_ICON, ModuleInfo);
            //    tabOwner.Image = ThemeUtils.Image16.Images[imageName];
            //    //
            //    tabOwner.Text += string.Format(" :: {0}", ModuleInfo.ModuleID);
            //}
            //if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP || ModuleInfo.UIType == CODES.DEFMOD.UITYPE.TABPAGE)
            //{
            //    var frmOwner = new frmModuleBox();
            //    frmOwner.Controls.Add(this);
            //    frmOwner.ucModule = this;
            //    frmOwner.CancelButton = CancelButton;
            //    frmOwner.AcceptButton = AcceptButton;
            //    Parent = frmOwner;

            //    frmOwner.MaximizeBox = (ModuleInfo.UserMax == CODES.DEFMOD.USERMAX.YES);
            //    frmOwner.MinimizeBox = (ModuleInfo.UserHide == CODES.DEFMOD.USERHIDE.YES);

            //    frmOwner.Name = ModuleFullName;
            //    frmOwner.Text = LangUtils.TranslateModuleItem(LangType.MODULE_TITLE, ModuleInfo);

            //    var imageName = LangUtils.TranslateModuleItem(LangType.MODULE_ICON, ModuleInfo);
            //    if (ThemeUtils.Image16.Images.ContainsKey(imageName))
            //        frmOwner.Icon = Icon.FromHandle(((Bitmap)ThemeUtils.Image16.Images[imageName]).GetHicon());

            //    frmOwner.Text += string.Format(" :: {0}", ModuleInfo.ModuleID);
            //    frmOwner.ClientSize = Language.Size;
            //}
            if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP || ModuleInfo.UIType == CODES.DEFMOD.UITYPE.TABPAGE)
            {
                var frmOwner = new frmModuleBox();
                frmOwner.Controls.Add(this);
                frmOwner.ucModule = this;
                frmOwner.CancelButton = CancelButton;
                frmOwner.AcceptButton = AcceptButton;
                Parent = frmOwner;

                frmOwner.MaximizeBox = (ModuleInfo.UserMax == CODES.DEFMOD.USERMAX.YES);
                frmOwner.MinimizeBox = (ModuleInfo.UserHide == CODES.DEFMOD.USERHIDE.YES);

                frmOwner.Name = ModuleFullName;
                frmOwner.Text = string.Format("{0} :: {1}", Language.Title, ModuleInfo.ModuleID);
                var imageName = LangUtils.TranslateModuleItem(LangType.MODULE_ICON, ModuleInfo);
                if (ThemeUtils.Image16.Images.ContainsKey(imageName))
                    frmOwner.Icon = Icon.FromHandle(((Bitmap)ThemeUtils.Image16.Images[imageName]).GetHicon());
                frmOwner.ClientSize = Language.Size;
            }
            //END TRUNGTT
        }
        #endregion

        #region Should Overridable Methods
        /// <summary>
        /// Khởi tạo Giao diện
        /// </summary>
        /// <param name="skin">Theme đang sử dụng</param>
        /// 
       
        protected virtual void InitializeGUI(Skin skin)
        {
        }

        protected virtual void BuildFields()
        {
            // 1. If module implement IParameterFieldSupportedModule
            var parameterFieldSupportedModule = this as IParameterFieldSupportedModule;
            if(parameterFieldSupportedModule != null)
            {
                BuildParameterFields();
            }

            // 2. If module implement ICommonFieldSupportedModule
            var commonFieldSupportedModule = this as ICommonFieldSupportedModule;
            if(commonFieldSupportedModule != null)
            {
                BuildCommonFields(commonFieldSupportedModule.CommonLayout);
            }

            // 3. If module implement IColumnFieldSupportedModule
            var columnFieldSupportedModule = this as IColumnFieldSupportedModule;
            if (columnFieldSupportedModule != null)
            {
                // Create columns for GridView
                BuildColumnFields(columnFieldSupportedModule.GridView);
                DeployGroupSummaries(columnFieldSupportedModule);

                var groupColumnFieldSupportedModule = this as IGroupColumnFieldSupportedModule;
                if (groupColumnFieldSupportedModule != null)
                {
                    BuildGroupColumnFields((BandedGridView)groupColumnFieldSupportedModule.GridView);
                }
            }

            // 4. If module implement IConditionFieldSupportedModule
            var conditionSupportedModule = this as IConditionFieldSupportedModule;
            if (conditionSupportedModule != null)
            {
                // Create root UISearchGroup in ConditionLayoutGroup
                conditionSupportedModule.RootGroup = BuildConditionFields(conditionSupportedModule.ConditionLayoutGroup);
                conditionSupportedModule.UpdateConditionQuery();
            }
        }

        private void BuildGroupColumnFields(BandedGridView gridView)
        {
            var gr = new GraphicsInfo().AddGraphics(null);
            Bands = new List<GridBand>();

            foreach (var columnField in ColumnFields)
            {
                var column = GridColumnByID[columnField.FieldID];
                column.MinWidth = gr.MeasureString(column.Caption, column.AppearanceHeader.Font).ToSize().Width + 8;
#if DEBUG
                column.Caption = columnField.FieldName;
#endif

                var band = new GridBand
                {
                    Name = string.Format("band{0}", columnField.FieldName),
                    Tag = columnField.FieldID,
                    Caption =
                        LangUtils.TranslateModuleItem(
                             LangType.LABEL_FIELD,
                             ModuleInfo,
                             columnField.FieldName),
                };

                band.AppearanceHeader.Options.UseTextOptions = true;
                band.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                Bands.Add(band);
                band.Columns.Add((BandedGridColumn)column);
            }

            GroupFields =
                FieldUtils.GetModuleFields(
                    ModuleInfo.ModuleID,
                    CODES.DEFMODFLD.FLDGROUP.SEARCH_GROUP
                );

            foreach (var groupField in GroupFields)
            {
                var band = new GridBand
                               {
                                   Name = string.Format("band{0}", groupField.FieldName),
                                   Tag = groupField.FieldID,
                                   Caption = LangUtils.TranslateModuleItem(
                                       LangType.LABEL_FIELD,
                                       ModuleInfo,
                                       groupField.FieldName)
                               };
                band.MinWidth = gr.MeasureString(band.Caption, band.AppearanceHeader.Font).ToSize().Width + 4;

                band.AppearanceHeader.Options.UseTextOptions = true;
                band.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                Bands.Add(band);
            }

            var orderedBands =
                (from GridBand band in Bands
                 orderby (string)band.Tag
                 select band);

            var visibleIndex = 1;
            gridView.Bands.AddRange(orderedBands.ToArray());
        }

        protected virtual void DeployGroupSummaries(IColumnFieldSupportedModule modColumn)
        {
            GroupSummaries =
                FieldUtils.GetModuleGroupSummary(ModuleInfo.ModuleID, ModuleInfo.ModuleType);

            foreach (var summary in GroupSummaries)
            {
                var summaryType = SummaryItemType.None;
                switch (summary.SummaryType)
                {
                    case SUMMARYTYPE.COUNT:
                        summaryType = SummaryItemType.Count;
                        break;
                    case SUMMARYTYPE.SUM:
                        summaryType = SummaryItemType.Sum;
                        break;
                    case SUMMARYTYPE.AVERAGE:
                        summaryType = SummaryItemType.Average;
                        break;
                    case SUMMARYTYPE.MIN:
                        summaryType = SummaryItemType.Min;
                        break;
                    case SUMMARYTYPE.MAX:
                        summaryType = SummaryItemType.Max;
                        break;
                }

                var summaryFieldName = summary.FieldName;
                if (!ColumnFields.Exists(i => i.FieldGroup == FLDGROUP.SEARCH_COLUMN && i.FieldName == summary.FieldName))
                {
                    var column = new GridColumn();
                    //summaryFieldName = column.FieldName = string.Format("groupExp" + summary.GroupID); 
                    column.FieldName = "TEST";
                    column.UnboundExpression = summary.FieldName;
                    column.UnboundType = UnboundColumnType.Decimal;
                    column.OptionsColumn.ShowInCustomizationForm = false;
                    column.Visible = false;                     
                    modColumn.GridView.Columns.Add(column);
                    
                }

                if (!string.IsNullOrEmpty(summary.FooterColumn))
                {
                    var column = ColumnFields.First(i => i.FieldName == summary.FooterColumn && i.FieldGroup == FLDGROUP.SEARCH_COLUMN);

                    if (column != null)
                    {
                        var nameLabel = "Footer[" + summary.GroupName + "]";

                        var label = Language.GetLabelText("Footer[" + summary.GroupName + "]");
                        if (label == nameLabel)
                        {
                            switch (summary.SummaryType)
                            {
                                case SUMMARYTYPE.COUNT:
                                    label = LangUtils.TranslateBasic("Count = {0:#,0}", "GLOBAL$GroupSummary.Footer[Count]");
                                    break;
                                case SUMMARYTYPE.SUM:
                                    label = LangUtils.TranslateBasic("Sum = {0:#,0}", "GLOBAL$GroupSummary.Footer[Sum]");
                                    break;
                                case SUMMARYTYPE.AVERAGE:
                                    label = LangUtils.TranslateBasic("Avg = {0:#,0}", "GLOBAL$GroupSummary.Footer[Avg]");
                                    break;
                                case SUMMARYTYPE.MIN:
                                    label = LangUtils.TranslateBasic("Min = {0:#,0}", "GLOBAL$GroupSummary.Footer[Min]");
                                    break;
                                case SUMMARYTYPE.MAX:
                                    label = LangUtils.TranslateBasic("Max = {0:#,0}", "GLOBAL$GroupSummary.Footer[Max]");
                                    break;
                            }
                        }

                        var gridColumn = GridColumnByID[column.FieldID];
                        modColumn.GridView.GroupSummary.Add(new GridGroupSummaryItem()
                        {
                            DisplayFormat = label,
                            FieldName = summaryFieldName,
                            SummaryType = summaryType,
                            ShowInGroupColumnFooter = gridColumn
                        });

                        gridColumn.SummaryItem.DisplayFormat = label;
                        gridColumn.SummaryItem.FieldName = summaryFieldName;
                        gridColumn.SummaryItem.SummaryType = summaryType;

                        modColumn.GridView.OptionsView.ShowFooter = true;
                    }
                }

                if (summary.ShowHeader == "Y")
                {
                    var nameLabel = "Header[" + summary.GroupName + "]";
                    var label = Language.GetLabelText("Header[" + summary.GroupName + "]");

                    if (label == nameLabel)
                    {
                        switch (summary.SummaryType)
                        {
                            case SUMMARYTYPE.COUNT:
                                label = LangUtils.TranslateBasic("Count = {0:#,0}", "GLOBAL$GroupSummary.Header[Count]");
                                break;
                            case SUMMARYTYPE.SUM:
                                label = LangUtils.TranslateBasic("Sum = {0:#,0}", "GLOBAL$GroupSummary.Header[Sum]");
                                break;
                            case SUMMARYTYPE.AVERAGE:
                                label = LangUtils.TranslateBasic("Avg = {0:#,0}", "GLOBAL$GroupSummary.Header[Avg]");
                                break;
                            case SUMMARYTYPE.MIN:
                                label = LangUtils.TranslateBasic("Min = {0:#,0}", "GLOBAL$GroupSummary.Header[Min]");
                                break;
                            case SUMMARYTYPE.MAX:
                                label = LangUtils.TranslateBasic("Max = {0:#,0}", "GLOBAL$GroupSummary.Header[Max]");
                                break;
                        }
                    }

                    modColumn.GridView.GroupSummary.Add(new GridGroupSummaryItem()
                    {
                        DisplayFormat = label,
                        FieldName = summaryFieldName,
                        SummaryType = summaryType
                    });
                    
                }
            }
        }
        
        protected virtual void BuildColumnFields(GridView gridView)
        {
            ColumnFields =
                FieldUtils.GetModuleFields(
                    ModuleInfo.ModuleID,
                    CODES.DEFMODFLD.FLDGROUP.SEARCH_COLUMN
                );

            GridColumnByID = new Dictionary<string, GridColumn>();
            StoreRepositories = new Dictionary<string, RepositoryItemImageComboBox>();            
            foreach (var columnField in ColumnFields)
            {
                var gridColumn = CreateColumn(gridView, columnField);
                GridColumnByID.Add(columnField.FieldID, gridColumn);
            }

            //TUDQ them
            foreach (var columnField in ColumnFields)
            {
                if (columnField.Group_Summary == CONSTANTS.Yes)
                {
                    gridView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
                    gridView.OptionsView.ShowFooter = true;
                    GridGroupSummaryItem item = new GridGroupSummaryItem();                    
                    item.FieldName = columnField.FieldName;
                    item.ShowInGroupColumnFooter = gridView.Columns.ColumnByFieldName(columnField.FieldName);
                    if (columnField.FieldName == "ROA" || columnField.FieldName == "ROE")
                    {
                       
                        gridView.Columns.ColumnByFieldName(columnField.FieldName).SummaryItem.SummaryType = SummaryItemType.Custom;
                    }                                            
                    else
                    {
                        gridView.Columns.ColumnByFieldName(columnField.FieldName).SummaryItem.SummaryType = SummaryItemType.Sum;
                    }
                    
                    gridView.Columns.ColumnByFieldName(columnField.FieldName).SummaryItem.FieldName = columnField.FieldName;
                    if (columnField.FieldFormat != "")
                        gridView.Columns.ColumnByFieldName(columnField.FieldName).SummaryItem.DisplayFormat = "{0:" + columnField.FieldFormat + "}";
                    gridView.GroupSummary.Add(item);
                }
            }

           
            gridView.CustomSummaryCalculate += delegate(object sender, CustomSummaryEventArgs e)
            {

                if (((GridSummaryItem)e.Item).FieldName == "ROA" || ((GridSummaryItem)e.Item).FieldName == "ROE")
                {
                    GridView gv = (GridView)sender;
                    if (!e.IsGroupSummary)
                    {                        
                        if (e.SummaryProcess == CustomSummaryProcess.Start)
                        {
                            TotalRowCount = 0; TotalValue = 0;
                        }
                        if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                        {
                            TotalRowCount++; TotalValue = TotalValue + Convert.ToDouble(e.FieldValue);                                                            
                            e.TotalValue = Math.Round(TotalValue / TotalRowCount, 4);                            
                        }
                    }                
                }
            };

            //End
//#if DEBUG
            gridView.ShowGridMenu += delegate(object sender, GridMenuEventArgs e)
            {
                if (e.HitInfo.InColumn && !e.HitInfo.InRow && e.MenuType == GridMenuType.Column)
                {
                    var item = new DXMenuItem
                    {
                        BeginGroup = true,
                        Caption = "Save Column Position",
                        Image = ThemeUtils.Image16.Images["SAVE"]
                    };

                    DataContainer container;
                    item.Click += delegate
                        {
                            using(var ctrlSA = new SAController())
                            {
                                var index = 0;
                                var columns = (from GridColumn column in gridView.Columns
                                               orderby column.GroupIndex, column.Fixed, column.VisibleIndex
                                               select column).ToArray();
                                foreach (var column in columns)
                                {
                                    var fieldInfo = column.Tag as ModuleFieldInfo;
                                    if (fieldInfo != null)
                                    {
                                        var fldID = string.Format("L{0:00}", index++ + 1);
                                        var strSQLQuery = string.Format(
                                            @"UPDATE DEFMODFLD SET FLDID = '{0}' WHERE MODID = '{1}' AND FLDGROUP = '{2}' AND FLDNAME = '{3}' AND FLDNAME = '{4}'",
                                            fldID, fieldInfo.ModuleID, fieldInfo.FieldGroup, fieldInfo.FieldName, fieldInfo.FieldName);
                                        ctrlSA.ExecuteSQL(out container, strSQLQuery);
                                    }
                                }
                            }
                        };
                    
                    e.Menu.Items.Add(item);
                    
                    var gridColumn = e.HitInfo.Column;
                    if(gridColumn != null)
                    {
                        var fieldInfo = gridColumn.Tag as ModuleFieldInfo;
                        if(fieldInfo != null)
                        {
                            item = new DXMenuItem
                                           {
                                               Caption = "Edit Column",
                                               Image = ThemeUtils.Image16.Images["EDIT"]
                                           };
                            
                            item.Click += delegate
                                              {
                                                  try
                                                  {
                                                      var ucModule = MainProcess.CreateModuleInstance("02903", "MED");
                                                      ucModule["P01"] = fieldInfo.ModuleID;
                                                      ucModule["C01"] = fieldInfo.FieldID;
                                                      ucModule.ShowDialogModule(this);
                                                  }
                                                  catch (Exception ex)
                                                  {
                                                      ShowError(ex);
                                                  }
                                              };                        
                            
                            e.Menu.Items.Add(item);

                            if (fieldInfo.UnboundExpression == "Y") { gridColumn.ShowUnboundExpressionMenu = true; }
                        }                                                
                    }
                }
                else
                {
                    e.Allow = false;
                }
            };
//#endif
        }

        protected virtual UISearchGroup BuildConditionFields(LayoutControlGroup conditionLayoutGroup)
        {
            ConditionFields =
                FieldUtils.GetModuleFields(
                    ModuleInfo.ModuleID,
                    CODES.DEFMODFLD.FLDGROUP.SEARCH_CONDITION
                );

            var rootGroup = new UISearchGroup(this, conditionLayoutGroup);

            if (ConditionFields.Count > 0)
            {
                foreach (var field in ConditionFields)
                {
                    if (field.FixedOnSearch == CODES.DEFMODFLD.FOS.YES)
                    {
                        rootGroup.CreateFixedCondition(field);
                    }
                }
            }
            else
            {
                conditionLayoutGroup.Visibility = LayoutVisibility.Never;
            }

            return rootGroup;
        }

        public void Callback(BaseEdit callbackControl)
        {
            try
            {
                PauseCallback = true;
                if (callbackControl != null)
                {
                    var fieldInfo = callbackControl.Tag as ModuleFieldInfo;
                    if (fieldInfo != null)
                    {
                        using (var ctrlSA = new SAController())
                        {
                            DataContainer con;
                            List<string> values;
                            GetOracleParameterValues(out values, fieldInfo.Callback);
                            ctrlSA.CallbackQuery(out con, fieldInfo.ModuleID, fieldInfo.FieldID, values);
                            AssignFieldValuesFromResult(con);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {
                PauseCallback = false;
            }
        }

        void MaintainControl_Callback(object sender, EventArgs e)
        {
            if(!PauseCallback && !StopCallback)
            {
                var callbackControl = sender as BaseEdit;
                Callback(callbackControl);
            }
        }

        protected virtual void BuildButtons()
        {
        }

        /// <summary>
        /// Thiết đặt giá trị mặc định cho các fields
        /// </summary>
        public virtual void SetDefaultValues()
        {
            SetDefaultValues(GetModuleFields());
        }

        protected virtual void SetDefaultValues(List<ModuleFieldInfo> fields)
        {
            foreach (var field in fields)
            {
                if (field.DefaultValue != null)
                {
                    switch (field.FieldGroup)
                    {
                        case CODES.DEFMODFLD.FLDGROUP.PARAMETER:
                        case CODES.DEFMODFLD.FLDGROUP.COMMON:
                        case CODES.DEFMODFLD.FLDGROUP.SEARCH_EXPORT:
                            this[field.FieldID] = field.DefaultValue.Decode(field);
                            break;
                        //add by TrungTT - 12.3.2012 - Form Send Mail
                        case CODES.DEFMODFLD.FLDGROUP.SEND_MAIL:
                            this[field.FieldID] = field.DefaultValue.Decode(field);
                            break;
                        //End TrungTT
                    }
                }
                if( field.Nullable == CODES.DEFMODFLD.NULLABLE.NO &&
                    field.ControlType == CODES.DEFMODFLD.CTRLTYPE.COMBOBOX &&
                    field.FieldGroup == CODES.DEFMODFLD.FLDGROUP.COMMON &&
                    this[field.FieldID] == null)
                {
                    var cbo = GetControlByFieldID(field.FieldID) as ComboBoxEdit;
                    if(cbo != null && cbo.Properties.Items.Count > 0)
                    {
                        switch (field.FieldGroup)
                        {
                            case CODES.DEFMODFLD.FLDGROUP.PARAMETER:
                            case CODES.DEFMODFLD.FLDGROUP.COMMON:
                                cbo.SelectedIndex = 0;
                                break;
                        }                                            
                    }
                }
            }
        }

        protected virtual void InitializeModuleData()
        {
#if DEBUG
            SetupSaveSize();
#endif
            SetDefaultValues();
        }

        private delegate void LockUserActionInvoker();
        public virtual void LockUserAction()
        {
            if(InvokeRequired)
            {
                Invoke(new LockUserActionInvoker(LockUserAction));
                return;
            }

            if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP && Parent != null)
            {
                ((frmModuleBox) Parent).CanUserClose = false;
            }
        }

        private delegate void UnLockUserActionInvoke();
        public virtual void UnLockUserAction()
        {
            if (InvokeRequired)
            {
                Invoke(new UnLockUserActionInvoke(UnLockUserAction));
                return;
            }

            if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP)
            {
                var parentForm = Parent as frmModuleBox;
                if(parentForm != null)
                {
                    parentForm.CanUserClose = true;                    
                }
            }
        }
//#if DEBUG
        public void SetupFieldMaker()
        {
            var mnuFieldMaker = Context.Items.Add("Quick Add Fields", ThemeUtils.Image16.Images["QUICK_ADD"]);
            mnuFieldMaker.Click +=
                delegate
                {
                    try
                    {
                        var ucModule = MainProcess.CreateModuleInstance("FLDMK", "MMN");
                        ucModule["C01"] = ModuleInfo.ModuleID;
                        ucModule.ShowModule(MainProcess.GetMainForm());
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                };
            Context.Items.Add(mnuFieldMaker);            
        }

        public void SetupParameterFields()
        {
            var mnuParameterFields = Context.Items.Add("List Parameters", ThemeUtils.Image16.Images["PARAMETER"]);
            mnuParameterFields.Click +=
                delegate
                {
                    try
                    {
                        var ucModule = MainProcess.CreateModuleInstance("03906", "MMN");
                        ucModule["C01"] = ModuleInfo.ModuleID;
                        ucModule.ShowModule(MainProcess.GetMainForm());
                        ucModule.Execute();
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                };
            Context.Items.Add(mnuParameterFields);
        }

        public void SetupCommonFields()
        {
            var mnuCommonFields = Context.Items.Add("List Common Fields", ThemeUtils.Image16.Images["COMMON"]);
            mnuCommonFields.Click +=
                delegate
                {
                    try
                    {
                        var ucModule = MainProcess.CreateModuleInstance("03905", "MMN");
                        ucModule["C01"] = ModuleInfo.ModuleID;
                        ucModule.ShowModule(MainProcess.GetMainForm());
                        ucModule.Execute();
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                };
            Context.Items.Add(mnuCommonFields);
        }

        public void SetupConditionFields()
        {
            var mnuConditionFields = Context.Items.Add("List Condition Fields", ThemeUtils.Image16.Images["CONDITION"]);
            mnuConditionFields.Click +=
                delegate
                {
                    try
                    {
                        var ucModule = MainProcess.CreateModuleInstance("03902", "MMN");
                        ucModule["C01"] = ModuleInfo.ModuleID;
                        ucModule.ShowModule(MainProcess.GetMainForm());
                        ucModule.Execute();
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                };
            Context.Items.Add(mnuConditionFields);
        }

        public void SetupColumnFields()
        {
            var mnuColumnFields = Context.Items.Add("List Column Fields", ThemeUtils.Image16.Images["COLUMN"]);
            mnuColumnFields.Click +=
                delegate
                {
                    try
                    {
                        var ucModule = MainProcess.CreateModuleInstance("03904", "MMN");
                        ucModule["C01"] = ModuleInfo.ModuleID;
                        ucModule.ShowModule(MainProcess.GetMainForm());
                        ucModule.Execute();
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }

                };
            Context.Items.Add(mnuColumnFields);
        }

        public void SetupSeparator()
        {
            Context.Items.Add(new ToolStripSeparator());
        }

        public void SetupLanguageTool()
        {
            var mnuLanguageTool = Context.Items.Add("Translate module", ThemeUtils.Image16.Images["COMMON"]);
            mnuLanguageTool.Click +=
                delegate
                {
                    try
                    {
                        var ucModule = (ucEditLanguage)MainProcess.CreateModuleInstance(STATICMODULE.EDITLANG, "MMN");
                        ucModule.mainView.ActiveFilterString = string.Format("LANGNAME LIKE '{0}$%' OR LANGNAME LIKE '{0}.%'", ModuleInfo.ModuleName);
                        ucModule.Execute();
                        ucModule.ShowDialogModule(this);
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                };
            Context.Items.Add(mnuLanguageTool);
        }

        public void SetupSaveSize()
        {
            if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP)
            {
                var sqlCommands = new string[0];
                
                var mnuSaveSize = Context.Items.Add("Save window's Size", ThemeUtils.Image16.Images["COMMON"]);
                mnuSaveSize.Enabled = false;
                mnuSaveSize.Click +=
                    delegate
                    {
                        try
                        {
                            using (var ctrlSA = new SAController())
                            {
                                foreach (var sql in sqlCommands)
                                {
                                    DataContainer container;
                                    ctrlSA.ExecuteSQL(out container, sql);
                                }
                                LangUtils.RefreshLanguage();
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowError(ex);
                        }
                    };

                Context.Items.Add(mnuSaveSize);

                Resize += delegate
                {
                    sqlCommands = new[]
                    {
                        "DELETE FROM DEFLANG WHERE LANGID = 'VN' AND LANGNAME = '{0}.Size'",
                        "INSERT INTO DEFLANG(LANGID, LANGNAME, LANGVALUE) VALUES ('VN', '{0}.Size', '{1},{2}')"
                    };

                    for (var i = 0; i < sqlCommands.Length; i++)
                    {
                        sqlCommands[i] = string.Format(sqlCommands[i],
                            ModuleInfo.ModuleName,
                            ClientSize.Width,
                            ClientSize.Height);
                    }

                    mnuSaveSize.Text = string.Format("Save ({0}, {1})", ClientSize.Width, ClientSize.Height);
                    mnuSaveSize.Enabled = true;
                };
            }
        }

        public void SetupSaveLayout(LayoutControl mainLayout)
        {
            var mnuLayout = Context.Items.Add("Save Layout", ThemeUtils.Image16.Images["SAVE"]);

            mnuLayout.Click +=
                delegate
                    {
                        try
                        {
                            mainLayout.HiddenItems.Clear();

                            using (var ctrlSA = new SAController())
                            {
                                using (var ms = new MemoryStream())
                                {
                                    mainLayout.SaveLayoutToStream(ms);
                                    var savedLayout = Encoding.UTF8.GetString(ms.ToArray());
                                    ctrlSA.SaveLayout(
                                        ModuleInfo.ModuleID, ModuleInfo.SubModule,
                                        App.Environment.ClientInfo.LanguageID, savedLayout);
                                    ms.Close();
                                }
                                LangUtils.RefreshLanguage();
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowError(ex);
                        }
                    };
        }

        public void SetupSaveGridLayout(BandedGridView view, List<GridBand> bands)
        {
            var mnuGridLayout = Context.Items.Add("Save Grid Layout", ThemeUtils.Image16.Images["SAVE"]);
            mnuGridLayout.Click +=
                delegate
                {
                    try
                    {
                        using (var ctrlSA = new SAController())
                        {
                            var listLayout = (from band in bands
                                              orderby band.VisibleIndex
                                              where band.ParentBand != null
                                              select new[] {band.Name, band.ParentBand.Name}).ToList();

                            var serializer = new XmlSerializer(listLayout.GetType());
                            var sw = new StringWriter();
                            serializer.Serialize(sw, listLayout);

                            ctrlSA.SaveLayout(
                                ModuleInfo.ModuleID, "GRID",
                                App.Environment.ClientInfo.LanguageID, sw.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                };
        }

        public void SetupSaveAllLayout(LayoutControl mainLayout)
        {
            var mnuAllLayout = Context.Items.Add("Save All Layout", ThemeUtils.Image16.Images["SAVE"]);
            mnuAllLayout.Click +=
                delegate
                {
                    try
                    {
                        mainLayout.HiddenItems.Clear();

                        using (var ctrlSA = new SAController())
                        {
                            using (var ms = new MemoryStream())
                            {
                                
                                mainLayout.SaveLayoutToStream(ms);
                                var savedLayout = Encoding.UTF8.GetString(ms.ToArray());
                                ctrlSA.SaveLayout(
                                    ModuleInfo.ModuleID, "ALL",
                                    App.Environment.ClientInfo.LanguageID, savedLayout);
                                ms.Close();
                            }
                        }
                        LangUtils.RefreshLanguage();
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                };
        }

        public void SetupContextMenu(LayoutControl mainLayout)
        {
            Context.Items.Add("Edit Layout", ThemeUtils.Image16.Images["GUI"]).Click +=
                delegate
                    {
                        mainLayout.ShowCustomizationForm();
                    };
            Context.Items.Add(new ToolStripSeparator());
            mainLayout.ContextMenuStrip = Context;
        }

        public void SetupModuleEdit()
        {
            Context.Items.Add("Edit Module", ThemeUtils.Image16.Images["MODULE"]).Click +=
                delegate
                {
                    try
                    {
                        var ucModule = MainProcess.CreateModuleInstance(STATICMODULE.MODULE_CONFIG, "MMN");
                        ucModule["C01"] = ModuleInfo.ModuleID;
                        ucModule.ShowModule(MainProcess.GetMainForm());
                        ucModule.Execute();
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                };
        }

        public void SetupGenenerateScript()
        {
            Context.Items.Add("Generate Script", ThemeUtils.Image16.Images["SCRIPT"]).Click +=
                delegate
                {
                    try
                    {
                        var ucModule = MainProcess.CreateModuleInstance("03907", "MMN");
                        ucModule["C02"] = ModuleInfo.ModuleID;
                        ucModule.ShowModule(MainProcess.GetMainForm());
                        ucModule.Execute();
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                };
        }

        public void SetupFieldsSuggestion()
        {
            Context.Items.Add("Quick ListSource", ThemeUtils.Image16.Images["QUICK_LISTSOURCE"]).Click +=
                delegate
                {
                    try
                    {
                        var ucModule = MainProcess.CreateModuleInstance("03903", "MMN");
                        ucModule["C01"] = ModuleInfo.ModuleID;
                        ucModule.ShowModule(MainProcess.GetMainForm());
                        ucModule.Execute();
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                };
        }

        public void SetupAddButton()
        {
            Context.Items.Add("Add Button", ThemeUtils.Image16.Images["ADD"]).Click +=
                delegate
                {
                    try
                    {
                        var ucModule = MainProcess.CreateModuleInstance("02909", "MAD");
                        ucModule["C01"] = ModuleInfo.ModuleID;
                        ucModule.ShowModule(MainProcess.GetMainForm());
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }
                };
        }

        public void SetupRefreshModule()
        {
        }
//#endif

        #endregion

        #region Oracle Utils
        private void GetOracleParameterValues(List<OracleParam> @params)
        {
            var fields = GetModuleFields();
            foreach (var param in @params)
            {
                foreach (var field in fields)
                {
                    if (!string.IsNullOrEmpty(field.ParameterName) && param.Name == field.ParameterName)
                    {
                        param.Value = this[field.FieldID].Encode(field);
                        if (param.Name.ToUpper() == CONSTANTS.Parameter_SecID)
                        {
                            SecID = this[field.FieldID].Encode(field);
                        }
                    }
                }
            }
        }

        private void GetOracleParameterValues(List<OracleParam> @params, DataRow modifiedRow)
        {
            foreach (var param in @params)
            {
                foreach (var field in CommonFields)
                {
                    if (!string.IsNullOrEmpty(field.ParameterName) && param.Name == field.ParameterName)
                    {
                        param.Value = this[field.FieldID].Encode(field);
                    }
                }
                foreach (var field in ColumnFields)
                {
                    if (!string.IsNullOrEmpty(field.ParameterName) && param.Name == field.ParameterName)
                    {
                        param.Value = modifiedRow[field.FieldName].Encode(field);
                    }
                }
            }
        }

        public virtual void GetOracleParameterValues(out List<string> oracleValues, string storeName)
        {
            var oracleParams = ModuleUtils.GetOracleParams(storeName);
            GetOracleParameterValues(oracleParams);
            oracleValues = oracleParams.ToListString();
        }

        public virtual void GetOracleParameterValues(out List<string> oracleValues, string storeName, DataRow modifiedRow)
        {
            var oracleParams = ModuleUtils.GetOracleParams(storeName);
            GetOracleParameterValues(oracleParams, modifiedRow);
            oracleValues = oracleParams.ToListString();            
        }
        #endregion

        #region Module Utils
        public List<ModuleFieldInfo> GetModuleFields()
        {
            var moduleFields = new List<ModuleFieldInfo>();
            // 1. If module implement IParameterFieldSupportedModule
            var parameterFieldSupportedModule = this as IParameterFieldSupportedModule;
            if (parameterFieldSupportedModule != null)
            {
                moduleFields.AddRange(ParameterFields);
            }

            // 2. If module implement ICommonFieldSupportedModule
            var commonFieldSupportedModule = this as ICommonFieldSupportedModule;
            if (commonFieldSupportedModule != null)
            {
                moduleFields.AddRange(CommonFields);
            }

            // 3. If module implement IColumnFieldSupportedModule
            var columnFieldSupportedModule = this as IColumnFieldSupportedModule;
            if (columnFieldSupportedModule != null)
            {
                moduleFields.AddRange(ColumnFields);
            }

            return moduleFields;
        }
        protected virtual object GetControlByFieldID(string fieldID)
        {
            // 1. If module implement ICommonFieldSupportedModule
            var commonFieldSupportedModule = this as ICommonFieldSupportedModule;
            if (commonFieldSupportedModule != null && CommonControlByID.ContainsKey(fieldID))
            {
                return CommonControlByID[fieldID];
            }

            // 2. If module implement IColumnFieldSupportedModule
            var columnFieldSupportedModule = this as IColumnFieldSupportedModule;
            if (columnFieldSupportedModule != null && GridColumnByID.ContainsKey(fieldID))
            {
                return GridColumnByID[fieldID];
            }

            return null;
        }

        protected BaseLayoutItem GetLayoutItemByFieldID(string fieldID)
        {
            return CommonLayoutItemByID[fieldID];
        }

        public ModuleFieldInfo GetModuleFieldByName(string fieldGroup, string fieldName)
        {
            return FieldUtils.GetModuleFieldByName(ModuleInfo.ModuleID, fieldGroup, fieldName);
        }

        protected void BuildParameterFields()
        {
            ParameterFields =
                FieldUtils.GetModuleFields(
                    ModuleInfo.ModuleID,
                    CODES.DEFMODFLD.FLDGROUP.PARAMETER
                );

            ParameterByFieldID = new Dictionary<string, object>();
            foreach (var field in ParameterFields)
            {
                ParameterByFieldID.Add(field.FieldID, null);                
            }
        }

        protected void LoadValuesFromResult(DataContainer con)
        {
            if (con != null && con.DataTable != null)
            {
                var resultTable = con.DataTable;

                if (resultTable.Rows.Count > 0)
                {
                    var fields = GetModuleFields();
                    foreach (var field in fields)
                    {
                        foreach (DataColumn column in resultTable.Columns)
                        {
                            var value = resultTable.Rows[0][column];
                            try
                            {
                                if (field.FieldName == column.ColumnName)
                                {
                                    SetFieldValue(field.FieldName, value.DecodeAny(field));
                                }

                                if (ModuleInfo.SubModule == CODES.DEFMOD.SUBMOD.MAINTAIN_ADD &&
                                    field.ReadOnlyOnAdd == CODES.DEFMODFLD.READONLYMODE.READONLY)
                                {
                                }
                                else if (ModuleInfo.SubModule == CODES.DEFMOD.SUBMOD.MAINTAIN_EDIT &&
                                    field.ReadOnlyOnEdit == CODES.DEFMODFLD.READONLYMODE.READONLY)
                                {
                                }
                                else if ((ModuleInfo.SubModule == CODES.DEFMOD.SUBMOD.MAINTAIN_VIEW ||
                                    ModuleInfo.SubModule == CODES.DEFMOD.SUBMOD.MODULE_MAIN) &&
                                    field.ReadOnlyOnView == CODES.DEFMODFLD.READONLYMODE.READONLY)
                                {
                                }
                                else if (field.FieldName + ".READONLY" == column.ColumnName.ToUpper())
                                {
                                    var ctrl = (BaseEdit)GetControlByFieldID(field.FieldID);
                                    SetReadOnly(ctrl, value.ToString().ToUpper() == "Y");
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.Message, "LoadValueFromResult", field.FieldName, value);
                            }
                        }
                    }
                }
            }
        }

        protected void AssignFieldValuesFromResult(DataContainer con)
        {
            if (con != null && con.DataTable != null)
            {
                var resultTable = con.DataTable;

                if (resultTable.Rows.Count > 0)
                {
                    var commonModule = this as ICommonFieldSupportedModule;

                    var fields = GetModuleFields();

                    if (commonModule != null)
                    {
                        var commonLayout = commonModule.CommonLayout;

                        commonLayout.SuspendLayout();
                        commonLayout.BeginUpdate();
                        
                        foreach (DataColumn column in resultTable.Columns)
                        {
                            foreach (var field in fields)
                            {
                                if (field.FieldName + ".VISIBLE" == column.ColumnName.ToUpper())
                                {
                                    BaseLayoutItem layoutItem;
                                    string strVisible = resultTable.Rows[0][column].ToString().ToUpper();

                                    if (field.ControlType == CODES.DEFMODFLD.CTRLTYPE.DEFINEDGROUP)
                                    {
                                        layoutItem = GetLayoutItemByFieldID(field.FieldID);
                                    }
                                    else
                                    {
                                        layoutItem = GetLayoutItemByFieldID(field.FieldID);
                                    }

                                    switch (strVisible)
                                    {
                                        case CODES.DEFMODFLD.VISIBLE.YES:
                                            layoutItem.Visibility = LayoutVisibility.Always;
                                            break;
                                        case CODES.DEFMODFLD.VISIBLE.NO:
                                            layoutItem.Visibility = LayoutVisibility.Never;
#if DEBUG
                                            layoutItem.Visibility = LayoutVisibility.OnlyInCustomization;
#endif
                                            break;
                                        default:
                                            var expression = ExpressionUtils.ParseScript(strVisible);
                                            if (expression.Operands.Count == 1 && expression.Operands[0].Type == OperandType.VALUE)
                                            {
                                                var anchorField = FieldUtils.GetModuleFieldsByName(
                                                    ModuleInfo.ModuleID,
                                                    expression.Operands[0].NameOrValue)[0];
                                                var anchorItem = GetLayoutItemByFieldID(anchorField.FieldID);
                                                commonLayout.Root.Remove(layoutItem);
                                                switch (expression.StoreProcName)
                                                {
                                                    case "BOTTOM_OF":
                                                        commonLayout.AddItem(layoutItem, anchorItem, InsertType.Bottom);
                                                        break;
                                                    case "TOP_OF":
                                                        commonLayout.AddItem(layoutItem, anchorItem, InsertType.Top);
                                                        break;
                                                    case "LEFT_OF":
                                                        commonLayout.AddItem(layoutItem, anchorItem, InsertType.Left);
                                                        break;
                                                    case "RIGHT_OF":
                                                        commonLayout.AddItem(layoutItem, anchorItem, InsertType.Right);
                                                        break;
                                                }
                                                layoutItem.Visibility = LayoutVisibility.Always;
                                            }
                                            break;
                                    }
                                    break;
                                }
                                if (field.FieldName + ".EXPANDED" == column.ColumnName.ToUpper())
                                {
                                    if (field.ControlType == CODES.DEFMODFLD.CTRLTYPE.DEFINEDGROUP)
                                    {
                                        var layoutItem = (LayoutGroup)GetLayoutItemByFieldID(field.FieldID);
                                        switch (resultTable.Rows[0][column].ToString().ToUpper())
                                        {
                                            case "Y":
                                                layoutItem.Expanded = true;
                                                break;
                                            default:
                                                layoutItem.Expanded = false;
                                                break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }

                        if (resultTable.Columns.Contains("POPUP.ERROR"))
                        {
                            var errCode = Convert.ToInt32(resultTable.Rows[0]["POPUP.ERROR"]);
                            var handledEx = ErrorUtils.CreateError(errCode);
                            ShowError(handledEx);
                        }

                        if (resultTable.Columns.Contains("MODULE.REFRESH"))
                        {
                            var strRefresh = resultTable.Rows[0]["MODULE.REFRESH"] as string;
                            if (strRefresh == "Y") Refresh();
                        }

                        if (resultTable.Columns.Contains("MODULE.CALLBACK"))
                        {
                            var fldName = resultTable.Rows[0]["MODULE.CALLBACK"] as string;
                            var field = GetModuleFieldByName(FLDGROUP.COMMON, fldName);
                            var ctrl = _CommonControlByID[field.FieldID];
                            Callback((BaseEdit)ctrl);
                        }

                        if (resultTable.Columns.Contains("INPUT.FOCUS"))
                        {
                            var field = GetModuleFieldByName(CODES.DEFMODFLD.FLDGROUP.COMMON, resultTable.Rows[0]["INPUT.FOCUS"].ToString());
                            if (field != null)
                            {
                                var ctrl = GetLayoutItemByFieldID(field.FieldID);
                                commonLayout.FocusHelper.FocusElement(ctrl, true);
                            }
                        }

                        commonLayout.EndUpdate();
                        commonLayout.ResumeLayout(true);
                    }

                    foreach (DataColumn column in resultTable.Columns)
                    {
                        foreach (var field in fields)
                        {
                            if (field.FieldName == column.ColumnName)
                            {
                                SetFieldValue(field.FieldName, resultTable.Rows[0][field.FieldName].DecodeAny(field));
                                break;
                            }

                            if (field.FieldName + ".LISTSOURCE" == column.ColumnName.ToUpper())
                            {
                                if (field.ControlType == CTRLTYPE.FILESAVE)
                                {
                                    var ctrl = GetControlByFieldID(field.FieldID) as ButtonEdit;
                                    if (ctrl != null)
                                    {
                                        ctrl.Properties.Buttons[0].Tag = resultTable.Rows[0][column].ToString();
                                    }
                                }
                                break;
                            }

                            if (field.FieldName + ".REFRESH" == column.ColumnName.ToUpper())
                            {
                                var edit = GetControlByFieldID(field.FieldID) as ButtonEdit;

                                if(edit is ComboBoxEdit)
                                    LoadComboxListSource(edit.Properties as RepositoryItemComboBox);
                                break;
                            }

                            if (field.FieldName + ".READONLY" == column.ColumnName.ToUpper())
                            {
                                var strReadOnly = resultTable.Rows[0][column] as string;
                                var ctrl = (BaseEdit)GetControlByFieldID(field.FieldID);

                                if(!string.IsNullOrEmpty(strReadOnly))
                                    switch(strReadOnly.ToUpper())
                                    {
                                        case "Y":
                                            SetReadOnly(ctrl, true);
                                            break;
                                        case "N":
                                            SetReadOnly(ctrl, false);
                                            break;
                                        case "D":
                                            if (ModuleInfo.SubModule == CODES.DEFMOD.SUBMOD.MAINTAIN_ADD &&
                                                field.ReadOnlyOnAdd == CODES.DEFMODFLD.READONLYMODE.READONLY)
                                            {
                                                SetReadOnly(ctrl, true);
                                            }

                                            if (ModuleInfo.SubModule == CODES.DEFMOD.SUBMOD.MAINTAIN_EDIT &&
                                                field.ReadOnlyOnEdit == CODES.DEFMODFLD.READONLYMODE.READONLY)
                                            {
                                                SetReadOnly(ctrl, true);
                                            }

                                            if ((ModuleInfo.SubModule == CODES.DEFMOD.SUBMOD.MAINTAIN_VIEW ||
                                                ModuleInfo.SubModule == CODES.DEFMOD.SUBMOD.MODULE_MAIN) &&
                                                field.ReadOnlyOnView == CODES.DEFMODFLD.READONLYMODE.READONLY)
                                            {
                                                SetReadOnly(ctrl, true);
                                            }
                                            break;
                                    }
                                break;
                            }
                        }
                    }
                }
            }
        }
        public  void ShowDialogModule(IWin32Window owner)
        {
            ShowDialogModule(owner, false);
        }

        /// <summary>
        /// Hiển thị Module
        /// </summary>
        public virtual void ShowDialogModule(IWin32Window owner, bool execute)
        {
            if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP)
            {
                var frmOwner = (XtraForm)Parent;
                frmOwner.Activate();
                InitializeModuleData();
                if(execute) Execute();
                frmOwner.ShowDialog(owner);
            }

            if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.TABPAGE)
            {
                throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_ONLY_MODULE_POPUP_CAN_SHOW_AS_DIALOG);
            }

            if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.NOWINDOW)
            {
                throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_ONLY_MODULE_POPUP_CAN_SHOW_AS_DIALOG);
            }
        }

        /// <summary>
        /// Hiển thị Module
        /// </summary>
        public virtual void ShowModule(IWin32Window owner)
        {
            //EDIT BY TRUNGTT - 23.08.2011 - DOCKING MODULE
            try
            {
                if (!string.IsNullOrEmpty(ModuleInfo.RoleID))
                {
                    using (var ctrlSA = new SAController())
                    {
                        ctrlSA.CheckRole(ModuleInfo);
                    }
                }
                //
                if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP)
                {
                    //EDIT BY TRUNGTT - 23.08.2011 - DOCKING MODULE
                    //var frmOwner = (XtraForm)Parent;
                    //frmOwner.Activate();
                    //InitializeModuleData();
                    //frmOwner.Show(owner);
                    var frmOwner = (XtraForm)Parent;
                    frmOwner.Activate();

                    StopCallback = true;
                    IsFile = false;
                    //OnModuleLoad();
                    //OnModuleLoadComplete();
                    InitializeModuleData();
                    //DataRegistry.StartMonitor();
                    if (!IsFile)
                    {
                        frmOwner.Show(owner);
                    }                    
                    //END TRUNGTT
                 }

                if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.TABPAGE)
                {
                    //EDIT BY TRUNGTT - 23.08.2011 - DOCKING MODULE
                    //var tabOwner = (XtraTabPage)Parent;
                    //InitializeModuleData();
                    //MainProcess.AddTabModule(tabOwner);
                    //
                    var frmOwner = (XtraForm)Parent;
                    frmOwner.Activate();

                    InitializeModuleData();

                    frmOwner.MdiParent = MainProcess.GetMainForm();
                    frmOwner.Show();
                    //END TRUNGTT
                }

                if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.NOWINDOW)
                {
                    InitializeModuleData();
                    Execute();                    
                }
            }
            //EDIT BY TRUNGTT - 23.08.2011 - DOCKING MODULE
            catch (Exception ex)
            {
                ShowError(ex);
                CloseModule();
            }
            //
        }

        public void CloseModule(CancelEventArgs e)
        {
            if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP)
            {
                if (ModuleClosing != null) ModuleClosing(this, e);
                if (!e.Cancel)
                {
                    var frmOwner = (XtraForm)Parent;
                    if (ModuleClosed != null)
                    {
                        frmOwner.FormClosed += delegate
                            {
                                ModuleClosed(this, new EventArgs());
                                Dispose();
                            };
                    }
                }
            }            
        }

        private delegate void CloseModuleInvoker();
        /// <summary>
        /// Yêu cầu đóng Module
        /// </summary>
        public void CloseModule()
        {
            //EDIT BY TRUNGTT - 28.03.2011 - DOCKING MODULE
            //if(InvokeRequired)
            //{
            //    BeginInvoke(new CloseModuleInvoker(CloseModule));
            //    return;
            //}

            //try
            //{
            //    if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP)
            //    {
            //        var frmOwner = (XtraForm)Parent;
                    
            //        if (frmOwner.Owner != null)
            //        {
            //            frmOwner.Owner.Activate();
            //        }

            //        if (ModuleClosed != null) ModuleClosed(this, new EventArgs());
            //        frmOwner.Dispose();
            //        Dispose();
            //    }

            //    if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.TABPAGE)
            //    {
            //        var e = new CancelEventArgs(false);
            //        if (ModuleClosing != null) ModuleClosing(this, e);
            //        if (!e.Cancel)
            //        {
            //            var tabOwner = (XtraTabPage)Parent;
            //            MainProcess.RemoveTabModule(tabOwner);
            //            if (ModuleClosed != null) ModuleClosed(this, new EventArgs());
            //            Dispose();
            //        }
            //    }

            //    if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.NOWINDOW)
            //    {
            //        var e = new CancelEventArgs(false);
            //        if (ModuleClosing != null) ModuleClosing(this, e);
            //        if (!e.Cancel)
            //        {
            //            if (ModuleClosed != null) ModuleClosed(this, new EventArgs());
            //            Dispose();
            //        }
            //    }
            //}
            //catch(Exception ex)
            //{
            //    ShowError(ex);
            //}
            if (InvokeRequired)
            {
                Invoke(new CloseModuleInvoker(CloseModule));
                return;
            }

            try
            {
                if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP || ModuleInfo.UIType == CODES.DEFMOD.UITYPE.TABPAGE)
                {
                    var frmOwner = (XtraForm)Parent;
                    if (frmOwner != null)
                    {
                        if (frmOwner.Owner != null)
                        {
                            frmOwner.Owner.Activate();
                        }
                        if (ModuleClosed != null) ModuleClosed(this, new EventArgs());
                        frmOwner.Dispose();
                    }
                }

                if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.NOWINDOW)
                {
                    var e = new CancelEventArgs(false);
                    if (ModuleClosing != null) ModuleClosing(this, e);
                    if (!e.Cancel)
                    {
                        if (ModuleClosed != null) ModuleClosed(this, new EventArgs());
                        Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            //END TRUNGTT
        }

        public bool ValidateModule()
        {
            var result = true;
            errorProvider.ClearErrors();

            var commonFieldSupportedModule = this as ICommonFieldSupportedModule;
            if(commonFieldSupportedModule != null)
            {
                if(commonFieldSupportedModule.ValidateRequire && !ValidateCommonFieldSupportedModule(commonFieldSupportedModule))
                {
                    result = false;
                }
            }

            var columnFieldSupportedModule = this as IColumnFieldSupportedModule;
            if(columnFieldSupportedModule != null)
            {
                UpdateStoreRepositories();
            }

            return result;
        }

        public bool ValidateCommonFieldSupportedModule(ICommonFieldSupportedModule module)
        {
            var isSuccess = true;
            foreach (var field in CommonFields)
            {
                if (field.ControlType == CODES.DEFMODFLD.CTRLTYPE.DEFINEDGROUP) continue;

                var validateRule = FieldUtils.GetValidateName(ModuleInfo, field);
                if (!ValidateCommonField(module, field, validateRule, isSuccess))
                    isSuccess = false;
            }

            return isSuccess;
        }

        private bool ValidateValue(ValidateInfo validateInfo, object value, string fieldLabel, out string errorDescription)
        {
            if(value != null && value.ToString() != "" && validateInfo != null)
            {
                // Regex
                if (!string.IsNullOrEmpty(validateInfo.RegularMatch) && !Regex.IsMatch(value.ToString(), validateInfo.RegularMatch))
                {
                    errorDescription = Language.GetRegExValidateText(validateInfo.ValidateName, fieldLabel);
                    return false;
                }
                // Kiem tra doi voi truong hop Validation Numeric
                if (validateInfo.ValueType == "N")
                {
                    var numValue = Convert.ToDecimal(value);
                    // Number Not In [Values]
                    if (validateInfo.NumberNotEqual != null && validateInfo.NumberNotEqual.Contains(numValue))
                    {
                        errorDescription = Language.GetValidateNumber(validateInfo.ValidateName, fieldLabel);
                        return false;
                    }
                    // Number >= Min Value
                    if (!string.IsNullOrEmpty(validateInfo.NumberMinValue) && numValue < Convert.ToDecimal(validateInfo.NumberMinValue))
                    {
                        errorDescription = Language.GetValidateNumber(validateInfo.ValidateName, fieldLabel);
                        return false;
                    }
                    // Number <= Max Value
                    if (!string.IsNullOrEmpty(validateInfo.NumberMaxValue) && numValue > Convert.ToDecimal(validateInfo.NumberMaxValue))
                    {
                        errorDescription = Language.GetValidateNumber(validateInfo.ValidateName, fieldLabel);
                        return false;
                    }
                    // Number Is Integer
                    if (validateInfo.NumberIsInteger == "Y" && numValue != decimal.Truncate(numValue))
                    {
                        errorDescription = Language.GetValidateNumber(validateInfo.ValidateName, fieldLabel);
                        return false;
                    }
                }                                                
            }

            errorDescription = null;
            return true;
        }

        private bool ValidateSyntaxValue(ModuleFieldInfo fieldInfo, List<string> values, string fieldLabel, out string errorDescription)
        {
            using(var ctrSA = new SAController())
            {
                try
                {
                    ctrSA.ValidateFieldInfoSyntax(ModuleInfo.ModuleID, ModuleInfo.SubModule, fieldInfo.FieldID, values);
                }
                catch(FaultException ex)
                {
                    errorDescription = ex.ToMessage(new object[] { fieldLabel });
                    return false;
                }
            }
            errorDescription = null;
            return true;
        }

        public bool ValidateCommonField(ICommonFieldSupportedModule module, ModuleFieldInfo field, string validateName, bool focusWhenError)
        {
            try
            {

                if (field.ControlType == "RT" || field.ControlType == "GV")
                {
                    var control = _CommonControlByID[field.FieldID];
                    if (control == null || !control.Visible || !control.Enabled) return true;
                    
                    //var value = this[field.FieldID];
                    var value = control.Text;
                    ProcExpression validateSyntax = null;

                    var fieldLabel = module.CommonLayout.GetItemByControl(control).Text;                    
                    var checkNull = field.Nullable == CODES.DEFMODFLD.NULLABLE.NO;

                    string errorDescription = null;
                    bool isSuccess = true;

                    if (checkNull)
                    {
                        if (value == null || value.ToString() == "")
                        {
                            errorDescription = Language.GetNullValidateText(fieldLabel);
                            isSuccess = false;
                        }
                    }

                    if (isSuccess && validateName != null)
                    {
                        if (ExpressionUtils.IsExpression(validateName))
                        {
                            validateSyntax = ExpressionUtils.ParseScript(validateName);
                            validateName = validateSyntax.StoreProcName;
                        }

                        if (!string.IsNullOrEmpty(validateName))
                        {
                            var validateInfo = FieldUtils.GetValidateInfo(validateName);

                            isSuccess = ValidateValue(validateInfo, value, fieldLabel, out errorDescription);
                            if (isSuccess && validateSyntax != null)
                            {
                                var listValues = new List<string> { value.Encode(field) };

                                foreach (var operand in validateSyntax.Operands)
                                {
                                    if (operand.Type == OperandType.NAME)
                                    {
                                        var otherField = GetModuleFieldByName(CODES.DEFMODFLD.FLDGROUP.COMMON, operand.NameOrValue);
                                        listValues.Add(this[otherField.FieldID].Encode(otherField));
                                    }
                                }

                                isSuccess = ValidateSyntaxValue(field, listValues, fieldLabel, out errorDescription);
                            }
                        }
                    }

                    if (!isSuccess)
                    {
                        errorProvider.SetError(control, errorDescription);
                        errorProvider.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
                        if (focusWhenError)
                        {
                            control.Focus();
                            ActiveControl = control;
                        }
                    }

                    return isSuccess;
                }
                else
                {
                    var control = CommonControlByID[field.FieldID];
                    if (control == null || !control.Visible || !control.Enabled || control.Properties.ReadOnly) return true;

                    var value = this[field.FieldID];
                    ProcExpression validateSyntax = null;

                    var fieldLabel = module.CommonLayout.GetItemByControl(control).Text;
                    var checkNull = field.Nullable == CODES.DEFMODFLD.NULLABLE.NO;                    

                    string errorDescription = null;
                    bool isSuccess = true;

                    if (checkNull)
                    {
                        if (value == null || value.ToString() == "")
                        {
                            errorDescription = Language.GetNullValidateText(fieldLabel);
                            isSuccess = false;
                        }
                    }

                    if (isSuccess && field.TxCheck !=null)
                    {                        
                        DateTime dtServerDate = App.Environment.ServerInfo.ServerNow;                        
                        //var dtvalue = this[field.FieldID].ToString().DecodeAny(field);
                        var dtvalue = this[field.FieldID].ToString();
                        switch (field.FieldType)
                        {                            
                            case CODES.DEFMODFLD.FLDTYPE.DATE:
                            #region Apply rule check TxDate vs Now
                                int intPosition = field.TxCheck.IndexOf("[");
                                string formular = field.TxCheck.Substring(0, intPosition);
                                //int result = DateTime.Compare(Convert.ToDateTime(dtvalue, App.Environment.ServerInfo.Culture), dtServerDate);
                                int result = DateTime.Compare(Convert.ToDateTime(dtvalue), dtServerDate);
                                switch (formular)
                                {
                                    case ">=":
                                        if (result < 0)
                                        {
                                            errorDescription = Language.GetDateValidateText(fieldLabel);
                                            isSuccess = false;
                                        }
                                        break;
                                    case "<=":
                                        if (result > 0)
                                        {
                                            errorDescription = Language.GetDateValidateText(fieldLabel);
                                            isSuccess = false;
                                        }
                                        break;
                                    case ">":
                                        if (result <= 0)
                                        {
                                            errorDescription = Language.GetDateValidateText(fieldLabel);
                                            isSuccess = false;
                                        }
                                        break;
                                    case "<":
                                        if (result >= 0)
                                        {
                                            errorDescription = Language.GetDateValidateText(fieldLabel);
                                            isSuccess = false;
                                        }
                                        break;
                                    default:
                                        isSuccess = true;
                                        break;
                                }
                                break;
                            # endregion
                            default:
                                break;                          
                        }           
                
#region Apply Rule Static   
                        switch (field.TxCheck)
                        {
                            case CODES.DEFMODFLD.TXCHECK.CHECKRYEAR:
                                int result = Int32.Parse(dtvalue.ToString()) - dtServerDate.Year;
                                if (result > 0)
                                {
                                    errorDescription = Language.GetDateValidateText(fieldLabel);
                                    isSuccess = false;
                                } 
                                break;
                            default:
                                break;
                        }
#endregion
                    }

                    if (isSuccess && validateName != null)
                    {
                        if (ExpressionUtils.IsExpression(validateName))
                        {
                            validateSyntax = ExpressionUtils.ParseScript(validateName);
                            validateName = validateSyntax.StoreProcName;
                        }

                        if (!string.IsNullOrEmpty(validateName))
                        {
                            var validateInfo = FieldUtils.GetValidateInfo(validateName);

                            isSuccess = ValidateValue(validateInfo, value, fieldLabel, out errorDescription);
                            if (isSuccess && validateSyntax != null)
                            {
                                var listValues = new List<string> { value.Encode(field) };

                                foreach (var operand in validateSyntax.Operands)
                                {
                                    if (operand.Type == OperandType.NAME)
                                    {
                                        var otherField = GetModuleFieldByName(CODES.DEFMODFLD.FLDGROUP.COMMON, operand.NameOrValue);
                                        listValues.Add(this[otherField.FieldID].Encode(otherField));
                                    }
                                }

                                isSuccess = ValidateSyntaxValue(field, listValues, fieldLabel, out errorDescription);
                            }
                        }
                    }

                    if (!isSuccess)
                    {
                        errorProvider.SetError(control, errorDescription);
                        errorProvider.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
                        if (focusWhenError)
                        {
                            control.Focus();
                            ActiveControl = control;
                        }
                    }

                    return isSuccess;
                }                
            }
            catch(Exception ex)
            {
                ShowError(ex);
                return false;
            }
        }
        
        public void ShowError(Exception ex)
        {
            if(ex is FaultException)
                frmInfo.ShowError(Language.Title, (FaultException)ex, this);
            else
                frmInfo.ShowError(Language.Title, ErrorUtils.CreateError(ex), this);
        }
        #endregion

        #region Fields Utils
        public void SetReadOnly(BaseEdit ctrl, bool isReadOnly)
        {
            ctrl.Properties.ReadOnly = isReadOnly;
            ctrl.Properties.AllowFocused = !isReadOnly;

            var repoButtonEdit = ctrl.Properties as RepositoryItemButtonEdit;            
            if (repoButtonEdit != null)
            {
                foreach (EditorButton button in repoButtonEdit.Buttons)
                {
                    button.Enabled = !isReadOnly;
#if DEBUG
                    if ((string) button.Tag == "DEBUG_EDIT")
                    {
                        button.Enabled = true;
                    }
#endif
                }
            }
        }

        public virtual void SetFieldValue(string name, object value)
        {
            var fields = FieldUtils.GetModuleFieldsByName(ModuleInfo.ModuleID, null, name);
            foreach (var field in fields)
            {
                if (field.FieldGroup != FLDGROUP.SEARCH_COLUMN)
                {
                    SetFieldValue(field, value);
                }
            }
        }

        public virtual void SetFieldValue(ModuleFieldInfo field, object value)
        {
            if (value != null && value != DBNull.Value)
                this[field.FieldID] = value;

            if (value == null || value == DBNull.Value)
            {
                if (!string.IsNullOrEmpty(field.DefaultValue))
                {
                    this[field.FieldID] = field.DefaultValue.Decode(field);
                }
                else
                {
                    this[field.FieldID] = null;
                }
            }
        }

        public virtual void SetControlListSource(Control edit)
        {
            if (edit is BaseEdit)
                SetBaseEditListSource((BaseEdit)edit);
            if (edit is GridControl)
                SetGridViewListSource((GridControl)edit);
        }

        public virtual void SetBaseEditListSource(BaseEdit edit)
        {
            if (edit is ComboBoxEdit)
            {
                var fieldInfo = edit.Tag as ModuleFieldInfo;

                if (fieldInfo != null)
                {
                    if (!string.IsNullOrEmpty(fieldInfo.ListSource))
                    {
                        var match = Regex.Match(fieldInfo.ListSource, "^:([^.]+).([^.]+)$");

                        if (match.Success)
                        {
                            SetControlDefinedCodeListSource(edit as ComboBoxEdit, fieldInfo, match.Groups[1].Value, match.Groups[2].Value);
                            //if (fieldInfo.Nullable == Codes.DEFMODFLD.NULLABLE.NO) (edit as ComboBoxEdit).SelectedIndex = 0;
                        }
                        else
                        {
                            var source = fieldInfo.ListSource;

                            var procExpression = ExpressionUtils.ParseScript(source);

                            if (procExpression != null && procExpression.StoreProcName != null)
                            {
                                var count = (from op in procExpression.Operands
                                             where op.Type == OperandType.NAME
                                             select 1).Count();

                                edit.Properties.Tag = procExpression;
                                if (count == 0)
                                {
                                    try
                                    {
                                        LoadComboxListSource(edit.Properties as RepositoryItemComboBox);
                                        //if (fieldInfo.Nullable == Codes.DEFMODFLD.NULLABLE.NO) (edit as ComboBoxEdit).SelectedIndex = 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        ShowError(ex);
                                    }
                                }
                                else
                                {
                                    foreach (var operand in procExpression.Operands)
                                    {
                                        if(operand.Type == OperandType.NAME)
                                        {
                                            var dependEdit = (BaseEdit)GetControlByFieldID(operand.NameOrValue);
                                            dependEdit.EditValueChanged +=
                                                delegate
                                                    {
                                                        try
                                                        {
                                                            LoadComboxListSource(edit.Properties as RepositoryItemComboBox);
                                                            edit.EditValue = null;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            ShowError(ex);
                                                        }
                                                    };
                                        }
                                    }

                                    LoadComboxListSource(edit.Properties as RepositoryItemComboBox);
                                }
                            }
                        }
                    }
                }
            }
            //TUDQ them
            else if (edit is RadioGroup)
            {
                var fieldInfo = edit.Tag as ModuleFieldInfo;

                if (fieldInfo != null)
                {
                    if (!string.IsNullOrEmpty(fieldInfo.ListSource))
                    {
                        var match = Regex.Match(fieldInfo.ListSource, "^:([^.]+).([^.]+)$");

                        if (match.Success)
                        {
                            SetRGControlDefinedCodeListSource(edit as RadioGroup, fieldInfo, match.Groups[1].Value, match.Groups[2].Value);
                            //if (fieldInfo.Nullable == Codes.DEFMODFLD.NULLABLE.NO) (edit as ComboBoxEdit).SelectedIndex = 0;
                        }
                        else
                        {
                            var source = fieldInfo.ListSource;

                            var procExpression = ExpressionUtils.ParseScript(source);

                            if (procExpression != null && procExpression.StoreProcName != null)
                            {
                                var count = (from op in procExpression.Operands
                                             where op.Type == OperandType.NAME
                                             select 1).Count();

                                edit.Properties.Tag = procExpression;
                                if (count == 0)
                                {
                                    try
                                    {
                                        LoadRadioGroupSource(edit.Properties as RepositoryItemRadioGroup);
                                        //if (fieldInfo.Nullable == Codes.DEFMODFLD.NULLABLE.NO) (edit as ComboBoxEdit).SelectedIndex = 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        ShowError(ex);
                                    }
                                }
                                else
                                {
                                    foreach (var operand in procExpression.Operands)
                                    {
                                        if (operand.Type == OperandType.NAME)
                                        {
                                            var dependEdit = (BaseEdit)GetControlByFieldID(operand.NameOrValue);
                                            dependEdit.EditValueChanged +=
                                                delegate
                                                {
                                                    try
                                                    {
                                                        LoadRadioGroupSource(edit.Properties as RepositoryItemRadioGroup);
                                                        edit.EditValue = null;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                         ShowError(ex);
                                                    }
                                                };
                                        }
                                    }
                                    LoadRadioGroupSource(edit.Properties as RepositoryItemRadioGroup);
                                }
                            }
                        }
                    }
                }
            }
            //End
        }

        //add by TrungTT - 28.11.2011 - Set GridViewCotnrol's Source
        public void SetGridViewListSource(GridControl edit)
        {
            var fieldInfo = edit.Tag as ModuleFieldInfo;

            if (fieldInfo != null)
            {
                if (!string.IsNullOrEmpty(fieldInfo.ListSource))
                {
                    var source = fieldInfo.ListSource;

                    var procExpression = ExpressionUtils.ParseScript(source);

                    if (procExpression != null && procExpression.StoreProcName != null)
                    {
                        var count = (from op in procExpression.Operands
                                     where op.Type == OperandType.NAME
                                     select 1).Count();

                        edit.Tag = procExpression;
                        if (count == 0)
                        {
                            try
                            {
                                LoadGridViewListSource(edit as GridControl);
                            }
                            catch (Exception ex)
                            {
                                ShowError(ex);
                            }
                        }
                        else
                        {
                            foreach (var operand in procExpression.Operands)
                            {
                                if (operand.Type == OperandType.NAME)
                                {
                                    var dependEdit = (BaseEdit)GetControlByFieldID(operand.NameOrValue);
                                    dependEdit.EditValueChanged +=
                                        delegate
                                        {
                                            try
                                            {
                                                LoadGridViewListSource(edit);                                                
                                            }
                                            catch (Exception ex)
                                            {
                                                ShowError(ex);
                                            }
                                        };
                                }
                            }

                            LoadGridViewListSource(edit);
                        }
                    }
                }
            }
        }
        //end TrungTT

        public virtual void SetControlListSource(PopupContainerEdit popupEdit, FlowLayoutPanel checkList)
        {
            var fieldInfo = popupEdit.Tag as ModuleFieldInfo;

            if (fieldInfo != null)
            {
                if (!string.IsNullOrEmpty(fieldInfo.ListSource))
                {
                    var match = Regex.Match(fieldInfo.ListSource, "^:([^.]+).([^.]+)$");

                    if (match.Success)
                    {
                        SetControlDefinedCodeListSource(checkList, match.Groups[1].Value, match.Groups[2].Value);
                    }
                    else
                    {
                        var source = fieldInfo.ListSource;

                        var procExpression = ExpressionUtils.ParseScript(source);

                        if (procExpression != null && procExpression.StoreProcName != null)
                        {
                            var count = (from op in procExpression.Operands
                                         where op.Type == OperandType.NAME
                                         select 1).Count();

                            checkList.Tag = procExpression;
                            if (count == 0)
                            {
                                LoadComboxListSource(popupEdit, checkList);
                            }
                            else
                            {
                                foreach (var operand in procExpression.Operands)
                                {
                                    if (operand.Type == OperandType.NAME)
                                    {
                                        var dependEdit = (BaseEdit)GetControlByFieldID(operand.NameOrValue);
                                        dependEdit.EditValueChanged +=
                                            delegate
                                            {
                                                try
                                                {
                                                    LoadComboxListSource(popupEdit, checkList);
                                                    popupEdit.EditValue = null;
                                                }
                                                catch (Exception ex)
                                                {
                                                    ShowError(ex);
                                                }
                                            };
                                    }
                                }

                                LoadComboxListSource(popupEdit, checkList);
                            }
                        }
                    }
                }
            }
        }

        protected void LoadComboxListSource(RepositoryItemComboBox properties)
        {
            var procExpression = properties.Tag as ProcExpression;
            if (procExpression != null)
            {
                var fieldInfo = properties.OwnerEdit.Tag as ModuleFieldInfo;
                LoadComboBoxListSource(fieldInfo, properties);
            }
        }

        //add by TrungTT - 28.11.2011 - Load GridView ListSource
        protected void LoadGridViewListSource(GridControl gridView)
        {
            var procExpression = gridView.Tag as ProcExpression;
            if (procExpression != null)
            {
                var fieldInfo = gridView.Tag as ModuleFieldInfo;
                LoadGridViewListSource(fieldInfo, gridView);
            }
        }
        //End TrungTT
        
        protected void LoadComboxListSource(PopupContainerEdit popupEdit, FlowLayoutPanel popupPanel)
        {
            var procExpression = popupPanel.Tag as ProcExpression;
            if (procExpression != null)
            {
                var fieldInfo = popupEdit.Tag as ModuleFieldInfo;
                LoadComboBoxListSource(fieldInfo, popupPanel);
            }
        }

        protected void LoadComboBoxListSource(ModuleFieldInfo fieldInfo, RepositoryItemComboBox properties)
        {
            var procExpression = properties.Tag as ProcExpression;
            if (procExpression != null)
            {
                var sourceList = App.Environment.GetSourceList(ModuleInfo, fieldInfo,
                    (from operand in procExpression.Operands
                     select 
                        operand.Type == OperandType.VALUE ?
                            operand.NameOrValue :
                            (
                                this[operand.NameOrValue] == null ? "" : this[operand.NameOrValue].ToString()
                             )).ToList());
                properties.Items.Clear();
                properties.Items.AddRange(
                    (from item in sourceList
                     select new ImageComboBoxItem
                     {
                         ImageIndex = ThemeUtils.GetImage16x16Index(item.ImageName),
                         Value = item.Value.Decode(fieldInfo),
                         Description = item.Text
                     }).ToArray());
            }
        }
        
        //add by TrungTT - 28.11.2011 - Load GridView ListSource 2 param
        protected void LoadGridViewListSource(ModuleFieldInfo fieldInfo, GridControl gridView)
        {
           
            var procExpression = gridView.Tag as ProcExpression;
            if (procExpression != null)
            {
                DataContainer container;
                try 
                {
                    using (SAController ctrlSA = new SAController())
                    {
                        ctrlSA.ExecuteProcedureFillDataset(out container, ((FIS.Utils.ProcExpression)(gridView.Tag)).StoreProcName,
                                (from operand in procExpression.Operands
                                 select
                                    operand.Type == OperandType.VALUE ?
                                        operand.NameOrValue :
                                        (
                                            this[operand.NameOrValue] == null ? "" : this[operand.NameOrValue].ToString()
                                         )).ToList());

                        DataSet dsResult = container.DataSet;
                        
                        gridView.DataSource = container.DataTable;
                        gridView.ViewRegistered += new ViewOperationEventHandler(gridView_ViewRegistered);
                        //TuDq them
                        if (container.DataTable.Rows.Count > 0)
                        {
                            
                            GridView gv = new GridView(gridView);
                            RepositoryItemMemoEdit ritem = new RepositoryItemMemoEdit();
                            gridView.RepositoryItems.Add(ritem);
                            gridView.MainView = gv;
                            gv.OptionsView.RowAutoHeight = true;                            
                            DataColumnCollection  columns = container.DataTable.Columns;
                            if (columns.Contains("DESCRIPTION"))
                            {
                                gv.Columns["DESCRIPTION"].ColumnEdit = ritem;
                                gv.Columns["DESCRIPTION"].OptionsColumn.FixedWidth = true;
                                gv.Columns["DESCRIPTION"].Width = 300;      
                            }
                                                      
                            List<string> values = new List<string>();
                            values.Add(ModuleInfo.ModuleID);
                            DataContainer con;
                            ctrlSA.ExecuteProcedureFillDataset(out con, "sp_gridColLang", values);
                            DataTable dt = con.DataTable;
                            for(int i=0;i<gv.Columns.Count;i++)
                            {                                                              
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    string langname = Convert.ToString(dt.Rows[j]["LANGNAME"]);
                                    if (langname.IndexOf("$") != -1 && langname.IndexOf(".") != -1)
                                    {
                                        string col = langname.Split('$')[1].Split('.')[0];
                                        if (col == gv.Columns[i].FieldName)
                                        {
                                            gv.Columns[i].Caption = Convert.ToString(dt.Rows[j]["LANGVALUE"]);
                                            break;
                                        }
                                    }
                                }
                            }
                            gv.BestFitColumns();
                            
                        }
                        //
                      }
                }
                catch (FaultException ex)
                {
                    ShowError(ex);
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            }
        }
       

        public void gridView_ViewRegistered(object sender, ViewOperationEventArgs e)
        {
            
            Console.WriteLine("ViewRegistered: Name={0}, LevelName={1}, IsDetailView={2}", e.View.Name,e.View.LevelName, e.View.IsDetailView);
            if (e.View is GridView & i == 0)
            {
                view = ((GridView)e.View);
                view.DataSourceChanged += delegate
                {
                    view.PopulateColumns();

                    view.BestFitColumns();
                    
                    view.OptionsBehavior.Editable = false;
                    view.OptionsCustomization.AllowFilter = false;
                    view.OptionsMenu.EnableColumnMenu = false;
                    view.OptionsSelection.EnableAppearanceFocusedCell = false;
                    view.OptionsView.ColumnAutoWidth = false;
                    view.OptionsView.EnableAppearanceEvenRow = true;
                    view.OptionsView.EnableAppearanceOddRow = true;
                    view.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
                    view.OptionsView.ShowGroupPanel = false;

                    foreach (GridColumn gridColumn in this.view.Columns)
                    {
                        if (gridColumn.Name == "colCONTENT")
                            view.Columns["CONTENT"].Visible = false;
                    }
                    foreach (GridColumn gridColumn in view.Columns)
                    {
                        string fieldName = gridColumn.Name.ToString();
                        gridColumn.Caption = LangUtils.Translate(LangType.LABEL_FIELD, fieldName, fieldName);
                    }
                    i=1;
                    view.DoubleClick += new EventHandler(view_DoubleClick);
                };
            }
            else if (e.View is GridView & i == 1)
            {
                view1 = ((GridView)e.View);
                view1.DataSourceChanged += delegate
                {
                    view1.PopulateColumns();

                    view1.BestFitColumns();

                    view1.OptionsBehavior.Editable = false;
                    view1.OptionsCustomization.AllowFilter = false;
                    view1.OptionsMenu.EnableColumnMenu = false;
                    view1.OptionsSelection.EnableAppearanceFocusedCell = false;
                    view1.OptionsView.ColumnAutoWidth = false;
                    view1.OptionsView.EnableAppearanceEvenRow = true;
                    view1.OptionsView.EnableAppearanceOddRow = true;
                    view1.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
                    view1.OptionsView.ShowGroupPanel = false;
                    foreach (GridColumn gridColumn in view1.Columns)
                    {
                        if (gridColumn.Name == "colCONTENT")
                            view1.Columns["CONTENT"].Visible = false;
                    }
                    foreach (GridColumn gridColumn in view1.Columns)
                    {
                        string fieldName = gridColumn.Name.ToString();
                        gridColumn.Caption = LangUtils.Translate(LangType.LABEL_FIELD, fieldName, fieldName);
                    }
                    i=2;
                    view1.DoubleClick += new EventHandler(view1_DoubleClick);
                };
            }
            else if (e.View is GridView & i == 2)
            {
                view2 = ((GridView)e.View);
                view2.DataSourceChanged += delegate
                {
                    view2.PopulateColumns();

                    view2.BestFitColumns();

                    view2.OptionsBehavior.Editable = false;
                    view2.OptionsCustomization.AllowFilter = false;
                    view2.OptionsMenu.EnableColumnMenu = false;
                    view2.OptionsSelection.EnableAppearanceFocusedCell = false;
                    view2.OptionsView.ColumnAutoWidth = false;
                    view2.OptionsView.EnableAppearanceEvenRow = true;
                    view2.OptionsView.EnableAppearanceOddRow = true;
                    view2.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
                    view2.OptionsView.ShowGroupPanel = false;
                    foreach (GridColumn gridColumn in view2.Columns)
                    {
                        if (gridColumn.Name == "colCONTENT")
                            view2.Columns["CONTENT"].Visible = false;
                    }
                    
                    foreach (GridColumn gridColumn in view2.Columns)
                    {
                        string fieldName = gridColumn.Name.ToString();
                        gridColumn.Caption = LangUtils.Translate(LangType.LABEL_FIELD, fieldName, fieldName);
                    }
                    i = 2;
                    view2.DoubleClick += new EventHandler(view2_DoubleClick);
                };
            }
            
        }
        //End TrungTT

        //NghiaLX 22/12/2011 Listen to GridView's double click event
        protected void view_DoubleClick(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = view.CalcHitInfo((e as MouseEventArgs).Location);
            if (hi.InRowCell == true)
            {
                foreach (GridColumn gridColumn in this.view.Columns)
                {
                    if (gridColumn.Name == "colCONTENT")
                    {
                       
                            var selectedRows = view.GetFocusedDataRow();
                            try
                            {
                                RichEditControl rtb = new RichEditControl();
                                
                               rtb.Document.HtmlText = selectedRows["CONTENT"].ToString();
                                rtb.Dock = DockStyle.Fill;
                                rtb.ReadOnly = true;
                                PopupContainerControl popupControl = new PopupContainerControl();
                                popupControl.Controls.Add(rtb);
                                //popupControl.AutoSizeMode = AutoSizeMode.GrowOnly;
                                popupControl.AutoSize = true;
                                popupControl.Width = 800;
                                popupControl.Height = 240;

                                PopupContainerEdit editor = new PopupContainerEdit();
                                editor.Properties.PopupControl = popupControl;
                                Controls.Add(editor);

                                editor.Top = 200;

                                editor.ShowPopup();
                                
                               
                            }
                            catch (FaultException ex)
                            {
                                ShowError(ex);
                            }
                            catch (Exception ex)
                            {
                                ShowError(ex);
                            }

                        }
                    }
                }   
        }
        protected void view1_DoubleClick(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = view1.CalcHitInfo((e as MouseEventArgs).Location);
            if (hi.InRowCell == true)
            {
                foreach (GridColumn gridColumn in this.view1.Columns)
                {
                    if (gridColumn.Name == "colCONTENT")
                    {

                        var selectedRows = view1.GetFocusedDataRow();
                        try
                        {
                            RichEditControl rtb = new RichEditControl();

                            rtb.Document.HtmlText = selectedRows["CONTENT"].ToString();
                            rtb.Dock = DockStyle.Fill;
                            rtb.ReadOnly = true;
                            PopupContainerControl popupControl = new PopupContainerControl();
                            popupControl.Controls.Add(rtb);
                            //popupControl.AutoSizeMode = AutoSizeMode.GrowOnly;                            
                            popupControl.AutoSize = true;
                            popupControl.Width = 800;
                            popupControl.Height = 240;

                            PopupContainerEdit editor = new PopupContainerEdit();
                            editor.Properties.PopupControl = popupControl;
                            Controls.Add(editor);

                            editor.Top = 200;

                            editor.ShowPopup();
                            

                        }
                        catch (FaultException ex)
                        {
                            ShowError(ex);
                        }
                        catch (Exception ex)
                        {
                            ShowError(ex);
                        }

                    }
                }
            }
        }
        protected void view2_DoubleClick(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = view2.CalcHitInfo((e as MouseEventArgs).Location);
            if (hi.InRowCell == true)
            {
                foreach (GridColumn gridColumn in this.view2.Columns)
                {
                    if (gridColumn.Name == "colCONTENT")
                    {

                        var selectedRows = view2.GetFocusedDataRow();
                        try
                        {
                            RichEditControl rtb = new RichEditControl();

                            rtb.Document.HtmlText = selectedRows["CONTENT"].ToString();
                            rtb.Dock = DockStyle.Fill;
                            rtb.ReadOnly = true;
                            PopupContainerControl popupControl = new PopupContainerControl();
                            popupControl.Controls.Add(rtb);
                            //popupControl.AutoSizeMode = AutoSizeMode.GrowOnly;
                            popupControl.AutoSize = true;
                            popupControl.Width = 800;
                            popupControl.Height = 240;

                            PopupContainerEdit editor = new PopupContainerEdit();
                            editor.Properties.PopupControl = popupControl;
                            Controls.Add(editor);

                            editor.Top = 200;

                            editor.ShowPopup();
                            

                        }
                        catch (FaultException ex)
                        {
                            ShowError(ex);
                        }
                        catch (Exception ex)
                        {
                            ShowError(ex);
                        }

                    }
                }
            }
        }  
        //end NghiaLX

        protected void LoadComboBoxListSource(ModuleFieldInfo fieldInfo, FlowLayoutPanel popupPanel)
        {
            var procExpression = popupPanel.Tag as ProcExpression;
            if (procExpression != null)
            {
                var sourceList = App.Environment.GetSourceList(ModuleInfo, fieldInfo,
                    (from operand in procExpression.Operands
                     select
                        operand.Type == OperandType.VALUE ?
                            operand.NameOrValue :
                            (
                                this[operand.NameOrValue] == null ? "" : this[operand.NameOrValue].ToString()
                             )).ToList());

                popupPanel.Parent.SuspendLayout();
                popupPanel.Controls.Clear();
                var items = (from item in sourceList
                             select new CheckEdit
                             {
                                 Tag = item.Value.Decode(fieldInfo),
                                 AutoSizeInLayoutControl = true,
                                 Margin = new Padding(2, 0, 15, 0),
                                 Text = item.Text
                             }).ToArray();
                foreach (var item in items)
                {
                    item.Font = new Font(FontFamily.GenericMonospace, 10f);
                    item.Properties.AutoWidth = true;
                }
                popupPanel.Controls.AddRange(items);
                popupPanel.Parent.ResumeLayout(true);
            }
        }

        private static void SetControlDefinedCodeListSource(FlowLayoutPanel popupPanel, string codeType, string codeName)
        {
            var codes = CodeUtils.GetCodes(codeType, codeName);
            popupPanel.SuspendLayout();
            popupPanel.Controls.Clear();
            var items = (from code in codes
                         where code.CodePosition >= 0
                         select new CheckEdit
                         {
                             Tag = code.CodeValue,
                             Text =
                                LangUtils.Translate(
                                    LangType.DEFINE_CODE,
                                    code.CodeType,
                                    code.CodeName,
                                    code.CodeValueName),
                             Margin = new Padding(0)
                         }).ToArray();
            foreach (var item in items)
            {
                item.Font = new Font(FontFamily.GenericMonospace, 10f);
                item.Properties.AutoWidth = true;
            }
            popupPanel.Controls.AddRange(items);
            popupPanel.ResumeLayout(true);
        }

        private static void SetControlDefinedCodeListSource(ComboBoxEdit edit, ModuleFieldInfo fieldInfo, string codeType, string codeName)
        {
            var codes = CodeUtils.GetCodes(codeType, codeName);

            edit.Properties.Items.Clear();
            edit.Properties.Items.AddRange(
                (from code in codes
                 where code.CodePosition >= 0
                 select new ImageComboBoxItem
                 {
                     Value = code.CodeValue.Decode(fieldInfo),
                     Description = LangUtils.Translate(LangType.DEFINE_CODE, code.CodeType, code.CodeName, code.CodeValueName),
                     ImageIndex = ThemeUtils.GetImage16x16Index(code.CodeValueName)
                 }).ToArray());
        }

        public void SetControlDefaultValue(BaseEdit edit)
        {
            var fieldInfo = edit.Tag as ModuleFieldInfo;
            if (fieldInfo != null && !string.IsNullOrEmpty(fieldInfo.DefaultValue))
            {
                edit.EditValue = fieldInfo.DefaultValue.Decode(fieldInfo);
            }
        }
        #endregion

        #region DataGrid Utils
        public GridColumn CreateColumn(GridView view, ModuleFieldInfo fieldInfo)
        {
            try
            {
                GridColumn gridColumn;
                if (!string.IsNullOrEmpty(fieldInfo.UnboundExpression))
                {
                    gridColumn = new GridColumn
                    {
                        Name = "col" + fieldInfo.FieldName,
                        FieldName = fieldInfo.FieldName,
                        Tag = fieldInfo,
                        Visible = true,
                        SortMode = ColumnSortMode.Value,
                        FilterMode = ColumnFilterMode.Value,
                        OptionsColumn = { AllowMerge = DefaultBoolean.True }
                    };

                    switch (fieldInfo.FieldType)
                    {
                        case FLDTYPE.DATETIME:
                        case FLDTYPE.DATE:
                            gridColumn.UnboundType = UnboundColumnType.DateTime;
                            break;
                        case FLDTYPE.STRING:
                            gridColumn.UnboundType = UnboundColumnType.String;
                            break;
                        default:
                            gridColumn.UnboundType = UnboundColumnType.Decimal;
                            break;
                    }

                    gridColumn.UnboundExpression = fieldInfo.UnboundExpression;
                }
                else
                {
                    gridColumn = view.Columns.Add();
                    gridColumn.Name = "col" + fieldInfo.FieldName;
                    gridColumn.FieldName = fieldInfo.FieldName;
                    gridColumn.Tag = fieldInfo;
                    gridColumn.Visible = true;
                    gridColumn.SortMode = ColumnSortMode.Value;
                    gridColumn.OptionsColumn.AllowMerge = DefaultBoolean.True;
                }

                if (fieldInfo.GroupOnSearch == CODES.DEFMODFLD.GOS.YES)
                {
                    //gridColumn.GroupIndex = gridColumn.VisibleIndex;                         
                    gridColumn.GroupIndex = view.Columns.Count;
                }

                gridColumn.ToolTip =
                gridColumn.Caption =
                    LangUtils.TranslateModuleItem(
                        LangType.LABEL_FIELD,
                        ModuleInfo,
                        fieldInfo.FieldName
                    );
                //
                gridColumn.ImageIndex = ThemeUtils.GetImage16x16Index(
                    LangUtils.TranslateModuleItem(
                        LangType.ICON_FIELD,
                        ModuleInfo,
                        fieldInfo.FieldName
                    ));
                //
                switch (fieldInfo.TextAlign)
                {
                    case CODES.DEFMODFLD.TEXTALIGN.CENTER:
                        gridColumn.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                        gridColumn.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                        gridColumn.AppearanceHeader.Options.UseTextOptions = true;
                        break;
                    case CODES.DEFMODFLD.TEXTALIGN.LEFT:
                        gridColumn.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
                        gridColumn.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                        break;
                    case CODES.DEFMODFLD.TEXTALIGN.RIGHT:
                        gridColumn.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
                        gridColumn.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                        break;
                }
                //
                if (fieldInfo.FixedWidth > 0)
                {
                    gridColumn.OptionsColumn.FixedWidth = true;
                    gridColumn.Width = fieldInfo.FixedWidth;
                }
                //
                if (fieldInfo.IconOnly == CODES.DEFMODFLD.ICONONLY.YES)
                {
                    gridColumn.OptionsColumn.ShowCaption = false;
                }
                //
                switch (fieldInfo.HoldColumn)
                {
                    case CODES.DEFMODFLD.HOLDCOLUMN.LEFT:
                        gridColumn.Fixed = FixedStyle.Left;
                        break;
                    case CODES.DEFMODFLD.HOLDCOLUMN.RIGHT:
                        gridColumn.Fixed = FixedStyle.Right;
                        break;
                }
                //
                if (fieldInfo.MegerCell == CODES.DEFMODFLD.MERGECELL.YES)
                {
                    view.OptionsView.AllowCellMerge = true;
                    gridColumn.OptionsColumn.AllowMerge = DefaultBoolean.True;
                }
                else
                {
                    gridColumn.OptionsColumn.AllowMerge = DefaultBoolean.False;
                }
                var repo = MakeRepository(fieldInfo, view);
                if (repo != null) gridColumn.ColumnEdit = repo;
                ApplyFormatInfo(fieldInfo, gridColumn);
                //
                switch (fieldInfo.ColumnSort)
                {
                    case CODES.DEFMODFLD.COLUMNSORT.ASCENDING:
                        gridColumn.SortOrder = ColumnSortOrder.Ascending;
                        break;
                    case CODES.DEFMODFLD.COLUMNSORT.DESCENDING:
                        gridColumn.SortOrder = ColumnSortOrder.Descending;
                        break;
                }
                //
                if (!string.IsNullOrEmpty(fieldInfo.ParameterName) && fieldInfo.ReadOnlyOnView == CODES.DEFMODFLD.READONLYMODE.READWRITE)
                {
                    gridColumn.OptionsColumn.AllowEdit = true;
                    if (fieldInfo.ControlType == CODES.DEFMODFLD.CTRLTYPE.COMBOBOX)
                    {
                        gridColumn.RealColumnEdit.EditValueChanged +=
                            delegate
                            {
                                view.CloseEditor();
                            };
                    }
                }
                else
                {
                    gridColumn.OptionsColumn.AllowEdit = false;
                }
                //
                switch (fieldInfo.Nullable)
                {
                    case CODES.DEFMODFLD.NULLABLE.NULL_HORIZONTAL:
                        gridColumn.RealColumnEdit.NullText = "―";
                        break;
                }

                return gridColumn;
            }
            catch(FaultException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        private RepositoryItem MakeRepository(ModuleFieldInfo fieldInfo, GridView gridView)
        {
            if (fieldInfo.ControlType == CODES.DEFMODFLD.CTRLTYPE.TEXTAREA)
            {
                var repository = new RepositoryItemMemoEdit {Tag = fieldInfo};
                return repository;
            }
            //TUDQ them
            if (fieldInfo.ControlType == CODES.DEFMODFLD.CTRLTYPE.CHECKBOX)
            {
                var repository = new RepositoryItemCheckEdit { Tag = fieldInfo, ValueChecked = (string)"1", ValueUnchecked = (string)"0" };
                return repository;
            }
            //
            if (fieldInfo.ControlType == CODES.DEFMODFLD.CTRLTYPE.COMBOBOX)
            {
                var repository = new RepositoryItemImageComboBox
                                     {
                                         SmallImages = ThemeUtils.Image16,
                                         Tag = fieldInfo
                                     };

                if (fieldInfo.IconOnly == CODES.DEFMODFLD.ICONONLY.YES)
                {
                    repository.GlyphAlignment = HorzAlignment.Center;
                }

                if(string.IsNullOrEmpty(fieldInfo.ListSource))
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_FIELD_NOT_CONFIG_LISTSOURCE, string.Format("{0}.{1}", fieldInfo.ModuleID, fieldInfo.FieldID));
                }

                var match = Regex.Match(fieldInfo.ListSource, "^:([^.]+).([^.]+)$");

                if (match.Success)
                {
                    SetColumnDefinedCodeListSource(repository, match.Groups[1].Value, match.Groups[2].Value);
                }
                else
                {
                    StoreRepositories[fieldInfo.FieldID] = repository;
                }

                if (fieldInfo.Nullable == CODES.DEFMODFLD.NULLABLE.YES)
                {
                    var editButton = new EditorButton(ButtonPredefines.Ellipsis);
                    repository.Buttons.Add(editButton);

                    repository.ButtonClick += delegate(object sender, ButtonPressedEventArgs e)
                    {
                        if (e.Button.Kind == ButtonPredefines.Ellipsis)
                        {
                            gridView.EditingValue = null;
                        }
                    };
                }

                return repository;
            }

            return null;
        }

        protected void UpdateStoreRepositories()
        {
            foreach(var fieldInfo in ColumnFields)
            {
                var fieldID = fieldInfo.FieldID;
                if(StoreRepositories.ContainsKey(fieldID))
                {
                    UpdateStoreRepositories(fieldInfo, StoreRepositories[fieldID]);
                }
            }
        }

        private void UpdateStoreRepositories(ModuleFieldInfo fieldInfo, RepositoryItemImageComboBox repository)
        {
            var procExpression = ExpressionUtils.ParseScript(fieldInfo.ListSource);
            repository.Tag = procExpression;

            foreach (var operand in procExpression.Operands)
            {
                if (operand.Type == OperandType.NAME)
                {
                    var dependEdit = (BaseEdit) GetControlByFieldID(operand.NameOrValue);
                    dependEdit.EditValueChanged +=
                        delegate
                            {
                                try
                                {
                                    LoadComboBoxListSource(fieldInfo, repository);
                                }
                                catch (Exception ex)
                                {
                                    ShowError(ex);
                                }
                            };
                }
            }

            LoadComboBoxListSource(fieldInfo, repository);
        }

        private static void SetColumnDefinedCodeListSource(RepositoryItemComboBox properties, string codeType, string codeName)
        {
            var codes = CodeUtils.GetCodes(codeType, codeName);
            var fieldInfo = properties.Tag as ModuleFieldInfo;

            if (fieldInfo != null)
            {
                properties.Items.Clear();
                properties.Items.AddRange(
                    (from code in codes
                     where code.CodePosition >= 0
                     select new ImageComboBoxItem
                     {
                         Value = code.CodeValue.Decode(fieldInfo),
                         Description =
                            LangUtils.Translate(
                                LangType.DEFINE_CODE,
                                code.CodeType,
                                code.CodeName,
                                code.CodeValueName),
                         ImageIndex = ThemeUtils.GetImage16x16Index(code.CodeValueName)
                     }).ToArray());
            }
        }

        public static void ApplyFormatInfo(ModuleFieldInfo fieldInfo, GridColumn column)
        {
            if (!string.IsNullOrEmpty(fieldInfo.FieldFormat))
            {
                switch (fieldInfo.FieldType)
                {
                    case CODES.DEFMODFLD.FLDTYPE.DATE:
                    case CODES.DEFMODFLD.FLDTYPE.DATETIME:
                        column.DisplayFormat.FormatType = FormatType.DateTime;
                        break;
                    case CODES.DEFMODFLD.FLDTYPE.DECIMAL:
                    case CODES.DEFMODFLD.FLDTYPE.DOUBLE:
                    case CODES.DEFMODFLD.FLDTYPE.FLOAT:
                    case CODES.DEFMODFLD.FLDTYPE.INT32:
                    case CODES.DEFMODFLD.FLDTYPE.INT64:
                        column.DisplayFormat.FormatType = FormatType.Numeric;
                        break;
                }
                column.DisplayFormat.FormatString = fieldInfo.FieldFormat;
            }
        }
        
        public static void ApplyFormatInfo(ModuleFieldInfo fieldInfo, BaseEdit baseEdit)
        {
            if (!string.IsNullOrEmpty(fieldInfo.FieldFormat))
            {
                var formatType = FormatType.None;
                var maskType = MaskType.None;
                switch (fieldInfo.FieldType)
                {
                    case CODES.DEFMODFLD.FLDTYPE.DATE:
                        formatType = FormatType.DateTime;
                        maskType = MaskType.DateTime;
                        break;
                    case CODES.DEFMODFLD.FLDTYPE.DECIMAL:
                    case CODES.DEFMODFLD.FLDTYPE.DOUBLE:
                    case CODES.DEFMODFLD.FLDTYPE.FLOAT:
                    case CODES.DEFMODFLD.FLDTYPE.INT32:
                    case CODES.DEFMODFLD.FLDTYPE.INT64:
                        formatType = FormatType.Numeric;
                        maskType = MaskType.Numeric;
                        break;
                }

                if(formatType != FormatType.None)
                {
                    baseEdit.Properties.DisplayFormat.FormatType = formatType;
                    baseEdit.Properties.EditFormat.FormatType = formatType;
                }
                
                if (baseEdit.Properties is RepositoryItemTextEdit)
                {
                    if(!baseEdit.Properties.ReadOnly && maskType != MaskType.None)
                    {
                        var repoMask = baseEdit.Properties as RepositoryItemTextEdit;
                        repoMask.Mask.MaskType = maskType;
                        repoMask.Mask.EditMask = fieldInfo.FieldFormat;
                    }
                }

                baseEdit.Properties.DisplayFormat.FormatString = fieldInfo.FieldFormat;
                baseEdit.Properties.EditFormat.FormatString = fieldInfo.FieldFormat;
            }

            switch (fieldInfo.TextAlign)
            {
                case CODES.DEFMODFLD.TEXTALIGN.CENTER:
                    baseEdit.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                    break;
                case CODES.DEFMODFLD.TEXTALIGN.LEFT:
                    baseEdit.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
                    break;
                case CODES.DEFMODFLD.TEXTALIGN.RIGHT:
                    baseEdit.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Far;
                    break;
            }
        }
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            var field =
                FieldUtils.GetModuleFieldsByName(
                    ModuleInfo.ModuleID,
                    format
                );
            if (field.Count > 0 && this[field[0].FieldID] != null)
                return this[field[0].FieldID].ToString();
            return string.Format("{{{0}}}", format);
        }
        #endregion

        static ucModule()
        {
            AliveModuleIntances = new List<ucModule>();
        }

        public ucModule()
        {
            InitializeComponent();
            PauseCallback = false;
            StopCallback = false;
        }

        public virtual void Execute()
        {
        }

        protected void ShowWaitingBox()
        {
            ShowWaitingBox(Language.ExecutingStatus);
        }

        protected void ShowWaitingBox(string statusName)
        {
            if (ModuleInfo.UIType != CODES.DEFMOD.UITYPE.NOWINDOW && Parent != null)
            {
                var ucWaitingBox = new ucWaitingBox();
                Parent.Controls.Add(ucWaitingBox);
                ucWaitingBox.BringToFront();

                ucWaitingBox.Left = Parent.ClientSize.Width / 2 - ucWaitingBox.Width / 2;
                ucWaitingBox.Top = Parent.ClientSize.Height / 2 - ucWaitingBox.Height / 2;
                ucWaitingBox.StatusText = statusName;
            }
        }

        protected void HideWaitingBox()
        {
            if (ModuleInfo.UIType != CODES.DEFMOD.UITYPE.NOWINDOW)
            {
                if(Parent != null)
                {
                    for (var i = 0; i < Parent.Controls.Count; )
                    {
                        var control = Parent.Controls[i];
                        if (control is ucWaitingBox)
                        {
                            Parent.Controls.RemoveAt(i);
                            control.Dispose();
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
            }
        }

        protected override void DestroyHandle()
        {
            base.DestroyHandle();
            GC.Collect();
        }

        //TuDQ them

        private static void SetRGControlDefinedCodeListSource(RadioGroup edit, ModuleFieldInfo fieldInfo, string codeType, string codeName)
        {
            var codes = CodeUtils.GetCodes(codeType, codeName);

            edit.Properties.Items.Clear();
            edit.Properties.Items.AddRange(
                (from code in codes
                 where code.CodePosition >= 0
                 select new RadioGroupItem
                 {
                     Value = code.CodeValue.Decode(fieldInfo),
                     Description = LangUtils.Translate(LangType.DEFINE_CODE, code.CodeType, code.CodeName, code.CodeValueName)
                 }).ToArray());
        }

        protected void LoadRadioGroupSource(RepositoryItemRadioGroup properties)
        {
            var procExpression = properties.Tag as ProcExpression;
            if (procExpression != null)
            {
                var fieldInfo = properties.OwnerEdit.Tag as ModuleFieldInfo;
                LoadRadioGroupSource(fieldInfo, properties);
            }
        }

        protected void LoadRadioGroupSource(ModuleFieldInfo fieldInfo, RepositoryItemRadioGroup properties)
        {
            var procExpression = properties.Tag as ProcExpression;
            if (procExpression != null)
            {
                var sourceList = App.Environment.GetSourceList(ModuleInfo, fieldInfo,
                    (from operand in procExpression.Operands
                     select
                        operand.Type == OperandType.VALUE ?
                            operand.NameOrValue :
                            (
                                this[operand.NameOrValue] == null ? "" : this[operand.NameOrValue].ToString()
                             )).ToList());
                properties.Items.Clear();
                properties.Items.AddRange(
                    (from item in sourceList
                     select new RadioGroupItem
                     {
                         Value = item.Value.Decode(fieldInfo),
                         Description = item.Text
                     }).ToArray());
            }
        }
        //End
    }
}

