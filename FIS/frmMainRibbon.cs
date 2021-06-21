using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using FIS.AppClient.Controls;
using FIS.AppClient.Interface;
using FIS.AppClient.Utils;
using FIS.Base;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace FIS.AppClient
{
    public partial class frmMainRibbon : RibbonForm, IMain
    {
        internal MainProcess Process { get; set; }
        public delegate void workerFunctionDelegate(string strText);
        public delegate void RunNewsDelegate(string strText);
        public frmMainRibbon()
        {
            InitializeComponent();

            barButtonItem6.Enabled = Program.blLogin;
            if (Program.blLogin == true)
            {
                barButtonItem3.Enabled = false;
            }
            else
            {
                barButtonItem3.Enabled = true;
            }

            if (!DesignMode)
            {
                InitializeLanguage();
                //InitializeMenu();                
            }

            Process = new MainProcess(this);
        }

        public void InitializeMenu()
        {
            ((IMain)this).InitializeMenu();
        }

        #region IMain Members
        MainProcess IMain.Process { get { return Process; } }
        IMainLanguage IMain.Language { get { return Language; } }

        void IMain.OnLogout()
        {
            foreach (RibbonPage page in ribbon.Pages)
            {
                page.Visible = false;
            }
        }

        void IMain.InitializeMenu()
        {
            var dicButtonItems = new Dictionary<string, object>();
            List<RibbonItemInfo> colRibbonItemsInfo;

            using (var ctrlSA = new SAController())
            {
                ctrlSA.ListRibbonItems(out colRibbonItemsInfo);
            }

            ribbon.Pages.Clear();

            foreach (var item in colRibbonItemsInfo)
            {
                if (item.RibbonType == CODES.DEFRIBBON.RIBTYPE.RIBBON_PAGE)
                {
                    var ribbonPage = new RibbonPage
                    {
                        Name = item.RibbonName,
                        Text = LangUtils.Translate(LangType.MENU_CAPTION, item.RibbonName),

                        Tag = item,
                        Visible = false
                    };
                    ribbon.Pages.Add(ribbonPage);
                    dicButtonItems.Add(item.RibbonID, ribbonPage);
                }
                else if (item.RibbonType == CODES.DEFRIBBON.RIBTYPE.RIBBON_GROUP)
                {
                    var ribbonPageGroup = new RibbonPageGroup
                    {
                        Name = item.RibbonName,
                        Text = LangUtils.Translate(LangType.MENU_CAPTION, item.RibbonName),
                        AllowTextClipping = false,
                        ShowCaptionButton = false,
                        Tag = item
                    };
                    dicButtonItems.Add(item.RibbonID, ribbonPageGroup);
                }
                else if (item.RibbonType == CODES.DEFRIBBON.RIBTYPE.BUTTON48 ||
                    item.RibbonType == CODES.DEFRIBBON.RIBTYPE.BUTTON16 ||
                    item.RibbonType == CODES.DEFRIBBON.RIBTYPE.SUB_BUTTON16
                    )
                {
                    var barItem = new BarButtonItem
                    {
                        Name = item.RibbonName,
                        Caption = LangUtils.Translate(LangType.MENU_CAPTION, item.RibbonName),
                        ItemShortcut = LangUtils.GetShortcut(LangType.MENU_HOTKEY, item.RibbonName),
                        Tag = item
                    };
                    //barItem.Appearance.Font = new Font("Tahoma", 10, FontStyle.Regular);

                    if (item.RibbonType == CODES.DEFRIBBON.RIBTYPE.BUTTON48)
                        barItem.LargeGlyph = LangUtils.Get48x48Image(LangType.MENU_ICON, item.RibbonName);
                    if (item.RibbonType == CODES.DEFRIBBON.RIBTYPE.BUTTON16)
                        barItem.Glyph = LangUtils.Get16x16Image(LangType.MENU_ICON, item.RibbonName);
                    if (item.RibbonType == CODES.DEFRIBBON.RIBTYPE.SUB_BUTTON16)
                        barItem.Glyph = LangUtils.Get16x16Image(LangType.MENU_ICON, item.RibbonName);

                    ribbon.Items.Add(barItem);
                    dicButtonItems.Add(item.RibbonID, barItem);
                }
                else if (item.RibbonType == CODES.DEFRIBBON.RIBTYPE.BUTTON32)
                {
                    var subMenu = new BarSubItem
                    {
                        Name = item.RibbonName,
                        Caption = LangUtils.Translate(LangType.MENU_CAPTION, item.RibbonName),
                        ItemShortcut = LangUtils.GetShortcut(LangType.MENU_HOTKEY, item.RibbonName),
                        Tag = item
                    };
                    //subMenu.Glyph = LangUtils.Get32x32Image(LangType.MENU_ICON, item.RibbonName);
                    subMenu.LargeGlyph = LangUtils.Get48x48Image(LangType.MENU_ICON, item.RibbonName);
                    ribbon.Items.Add(subMenu);
                    dicButtonItems.Add(item.RibbonID, subMenu);
                }
            }

            foreach (var item in colRibbonItemsInfo)
            {
                if (item.RibbonType == CODES.DEFRIBBON.RIBTYPE.RIBBON_PAGE)
                {
                    var child = dicButtonItems[item.RibbonID] as RibbonPage;
                    if (child != null)
                    {
                        ribbon.Pages.Add(child);
                    }
                }

                if (item.RibbonType == CODES.DEFRIBBON.RIBTYPE.RIBBON_GROUP)
                {
                    var child = dicButtonItems[item.RibbonID] as RibbonPageGroup;
                    var parent = dicButtonItems[item.RibbonOwnerID] as RibbonPage;
                    if (parent != null && child != null)
                    {
                        parent.Groups.Add(child);
                    }
                }

                if (item.RibbonType == CODES.DEFRIBBON.RIBTYPE.BUTTON16 ||
                    item.RibbonType == CODES.DEFRIBBON.RIBTYPE.BUTTON48)
                {
                    var child = dicButtonItems[item.RibbonID] as BarButtonItem;
                    var parent = dicButtonItems[item.RibbonOwnerID] as RibbonPageGroup;
                    //child.Appearance.Font = new Font("Tahoma", 10, FontStyle.Regular);
                    if (parent != null && child != null)
                    {
                        parent.ItemLinks.Add(child, item.BeginGroup == CODES.DEFRIBBON.BEGINGROUP.YES);
                    }
                }
                if (item.RibbonType == CODES.DEFRIBBON.RIBTYPE.BUTTON32)
                {
                    var child = dicButtonItems[item.RibbonID] as BarSubItem;
                    //child.Appearance.Font = new Font("Tahoma", 10, FontStyle.Regular);
                    var parent = dicButtonItems[item.RibbonOwnerID] as RibbonPageGroup;
                    if (parent != null && child != null)
                    {
                        parent.ItemLinks.Add(child, item.BeginGroup == CODES.DEFRIBBON.BEGINGROUP.YES);
                    }
                }
                if (item.RibbonType == CODES.DEFRIBBON.RIBTYPE.SUB_BUTTON16)
                {
                    var child = dicButtonItems[item.RibbonID] as BarButtonItem;
                    //child.Appearance.Font = new Font("Tahoma", 10, FontStyle.Regular);
                    var parent = dicButtonItems[item.RibbonOwnerID] as BarSubItem;
                    if (parent != null && child != null)
                    {
                        parent.ItemLinks.Add(child, item.BeginGroup == CODES.DEFRIBBON.BEGINGROUP.YES);
                    }
                }
            }

        }

        void IMain.ApplyMenu()
        {
            InitializeMenu();
            foreach (RibbonPage page in ribbon.Pages)
            {
                page.Visible = true;
            }
            //LoadSubMenu();
            LoadSkins();
            User userInfo = new User();
            DataContainer container;
            DataContainer container2;
            string strSysDate = null;
            string strPerson = null;
            using (SAController ctrlSA = new SAController())
            {
                ctrlSA.GetSessionUserInfo(out userInfo);
                ctrlSA.GetSysDate(out container);
                var dsResult = container.DataSet;
                strSysDate = "Thời gian: " + dsResult.Tables[0].Rows[0][0].ToString() + ",";
                strPerson = dsResult.Tables[1].Rows[0][0].ToString();
                List<string> values = new List<string>();
                values.Add(dsResult.Tables[0].Rows[0][0].ToString());
                //ctrlSA.ExecuteProcedureFillDataset(out container2, "SP_PERSON_SEL_MAIN", values);
                //var dsResult2 = container2.DataSet;
                //strPerson = "Liên hệ: "+ dsResult2.Tables[0].Rows[0][0].ToString();
            }

            barButtonItem1.ImageIndex = 2;
            barButtonItem1.Caption = "Đăng xuất ( " + userInfo.Username + " )";
            barStaticItem1.ImageIndex = 3;
            barStaticItem1.Caption = strSysDate;
            barStaticItem2.Caption = strPerson;
            timer1.Enabled = true;
            timer2.Enabled = true;

            //RunNews();
            //if (Program.AppName == CONSTANTS.AppNameSMS)
            //{
            //    RunAlert(userInfo.Username);
            //}

        }

        void IMain.StartupModules()
        {
            Process.StartupModules();
        }

        XtraTabPage IMain.AddTabModule(XtraTabPage tabPage)
        {
            //tabMain.TabPages.Add(tabPage);
            //tabMain.SelectedTabPage = tabPage;
            //return tabPage;
            return null;
        }

        void IMain.SelectTabPage(XtraTabPage tabPage)
        {
            //tabMain.SelectedTabPage = tabPage;
        }

        void IMain.RemoveTabModule(XtraTabPage tabPage)
        {
            //tabMain.TabPages.Remove(tabPage);
        }

        void IMain.AddModulePreview(ucModulePreview preview)
        {
        }

        void IMain.RemoveModulePreview(ucModulePreview preview)
        {
        }

        void IMain.RegisterButton(BarButtonItem button)
        {
        }

        void IMain.CancelRegisterButton(BarButtonItem button)
        {
        }
        #endregion

        #region Overloads
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = Language.ApplicationTitle;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            frmSplash.CloseForm();
            Activate();
            MainProcess.ShowLogin();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (App.Environment.ClientInfo.SessionKey == null || frmConfirm.ShowConfirm(Language.ExitTitle, Language.ExitConfirm, this))
            {
                MainProcess.LogoutFromSystem(false);
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == Win32.WM_COPYDATA && msg.WParam == Handle)
            {
                var fileInfo = (Win32.COPYDATASTRUCT)msg.GetLParam(typeof(Win32.COPYDATASTRUCT));
                Process.InstallModule(fileInfo.m_LpData);
            }
            base.WndProc(ref msg);
        }

        protected override void DestroyHandle()
        {
            base.DestroyHandle();
            MainProcess.LogoutFromSystem(false);
        }
        #endregion

        private void txtModuleID_EditValueChanged(object sender, EventArgs e)
        {
            //#if DEBUG                        
            if (txtModuleID.EditValue != null)
            {
                var moduleID = txtModuleID.EditValue.ToString().ToUpper();
                txtModuleID.EditValue = null;
                MainProcess.ExecuteModule(moduleID);
            }
            //#endif
        }

        private void ribbon_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (e.Item.Name == CONSTANTS.MENU_NAME_LOGOUT)
            {
                if (frmConfirm.ShowConfirm(
                            ((IMain)this).Language.LogoutTitle,
                            ((IMain)this).Language.LogoutConfirm,
                            this))
                {
                    barButtonItem1.Caption = "";
                    MainProcess.LogoutFromSystem(true);
                }
            }
            else
            {
                MainProcess.ExecuteMenu(e.Item.Name, e.Item.Tag as RibbonItemInfo);
            }
        }

        private void tabMain_CloseButtonClick(object sender, EventArgs e)
        {
            var evt = e as ClosePageButtonEventArgs;
            if (evt != null)
            {
                var page = evt.Page as XtraTabPage;
                if (page != null)
                    if (page.Controls.Count > 0)
                    {
                        ((ucModule)page.Tag).CloseModule();
                    }
            }
        }

        private void tabMain_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            if (e.PrevPage != null) e.PrevPage.Controls[0].Enabled = false;
            if (e.Page != null) e.Page.Controls[0].Enabled = true;
        }

        //HuyVQ3: Bỏ phần đa thị trường
        //private void LoadSubMenu()
        //{
        //    DataContainer container;
        //    using (var ctrlSA = new SAController())
        //    {
        //        ctrlSA.ExcuteMarket(out container);
        //        var dsResult = container.DataSet;

        //        BarManager barManager = new BarManager();
        //        barManager.BeginUpdate();
        //        barSubItem1.ClearLinks();
        //        foreach (DataRow row in dsResult.Tables[0].Rows)
        //        {
        //        BarButtonItem item = new BarButtonItem(barManager, row[0].ToString());
        //        barSubItem1.AddItem(item);                    
        //        }
        //        barManager.ItemClick += new ItemClickEventHandler(barManager_ItemClick);
        //        barManager.EndUpdate();
        //    }            
        //}
        void barManager_ItemClick(object sender, ItemClickEventArgs e)
        {
            //foreach (LinkPersistInfo item in barSubItem1.LinksPersistInfo)
            //{
            //    item.Item.Enabled = true;
            //}
            BarSubItem subMenu = e.Item as BarSubItem;
            if (subMenu != null) return;
            {
                using (SAController ctrlSA = new SAController())
                {
                    List<string> values = new List<string>();
                    values.Add(e.Item.Caption.ToString());
                    ctrlSA.UpdateMarket(values);
                }
                e.Item.Enabled = false;
            }
        }
        private void LoadSkins()
        {
            DataContainer container;
            DataContainer containerSkins;
            using (var ctrlSA = new SAController())
            {
                ctrlSA.ExecuteLoadSkins(out container);
                var dsResult = container.DataSet;

                ctrlSA.ExecuteLoadCurrentSkins(out containerSkins);
                var dsResultSkins = containerSkins.DataSet;

                BarManager barManagerSkins = new BarManager();
                barManagerSkins.BeginUpdate();
                barManagerSkins.Images = this.imageCollection2;
                barSubItem2.ClearLinks();
                foreach (DataRow row in dsResult.Tables[0].Rows)
                {
                    BarButtonItem item = new BarButtonItem(barManagerSkins, row[1].ToString(), Int32.Parse(row[0].ToString()) - 1);
                    barSubItem2.AddItem(item);
                    if (row[1].ToString() == dsResultSkins.Tables[0].Rows[0][0].ToString())
                    {
                        item.Enabled = false;
                    }
                }

                barManagerSkins.ItemClick += new ItemClickEventHandler(barManagerSkins_ItemClick);
                barManagerSkins.EndUpdate();
            }
        }
        void barManagerSkins_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (LinkPersistInfo item in barSubItem2.LinksPersistInfo)
            {
                item.Item.Enabled = true;
            }
            BarSubItem subMenu = e.Item as BarSubItem;
            if (subMenu != null) return;
            {
                using (SAController ctrlSA = new SAController())
                {
                    List<string> values = new List<string>();
                    values.Add(e.Item.Caption.ToString());
                    ctrlSA.ExecuteChangeSkins(values);
                }
                e.Item.Enabled = false;
            }
            App.Environment.GetCurrentUserProfile();
            ThemeUtils.ChangeSkin(App.Environment.ClientInfo.UserProfile.ApplicationSkinName);
        }
        private void frmMainRibbon_Load(object sender, EventArgs e)
        {
            ribbon.ApplicationButtonDropDownControl = applicationMenu1;
        }

        private void ribbon_ShowCustomizationMenu(object sender, RibbonCustomizationMenuEventArgs e)
        {
            e.ShowCustomizationMenu = false;
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (frmConfirm.ShowConfirm(
                            ((IMain)this).Language.LogoutTitle,
                            ((IMain)this).Language.LogoutConfirm,
                            this))
            {
                barButtonItem1.Caption = "";
                MainProcess.LogoutFromSystem(true);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            barStaticItem3.Caption = DateTime.Now.ToLongTimeString();
        }
        private delegate void appButtonInvoke();
        private void appButton()
        {
            if (InvokeRequired)
            {
                Invoke(new appButtonInvoke(appButton));
            }
            try
            {
                ribbon.ApplicationButtonDropDownControl = applicationMenu1;
                applicationMenu1.ShowRightPane = true;
                //applicationMenu1.RightPaneControlContainer = popupControlContainer1;

                AppMenuFileLabel label1 = new AppMenuFileLabel();
                //Image labelUncheckedImage = imageCollection3.Images[1];
                //Image labelCheckedImage = imageCollection3.Images[0];
                label1.Dock = DockStyle.Top;
                label1.AutoHeight = true;
                label1.Caption = "Document1.rtf";
                label1.ShowCheckButton = true;
                //label1.Image = labelUncheckedImage;
                //label1.SelectedImage = labelCheckedImage;
                // Fires when the label is clicked
                label1.LabelClick += new EventHandler(label1_LabelClick);
                //label1.Tag = "c:\\My Documents\\Document1.rtf";

                // Add the label to the container.
                //popupControlContainer1.Controls.Add(label1);
            }
            catch (Exception ex)
            {

            }
        }
        private void ribbon_ApplicationButtonClick(object sender, EventArgs e)
        {
            /*barButtonItem6.Enabled = Program.blLogin;
            if (Program.blLogin == true)
            {
                barButtonItem3.Enabled = false;
            }
            else
            {
                barButtonItem3.Enabled = true;
            }*/
            ribbon.ApplicationButtonDropDownControl = applicationMenu1;
            //applicationMenu1.ShowRightPane = true;
        }
        private void label1_LabelClick(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            RunNews();
            RunAlert();
        }
        private void RunAlert()
        {
            DataContainer container;
            using (SAController ctrlSA = new SAController())
            {
                List<string> values = new List<string>();
                //values.Add(username);                                           
                ctrlSA.ExecuteProcedureFillDataset(out container, "sp_alertlog_sel_all", values);
                var dsResult2 = container.DataSet;
                if (dsResult2.Tables[0].Rows.Count > 0)
                {
                    Displaynotify();
                }

            }
        }
        protected void Displaynotify()
        {
            try
            {
                //notifyIcon1.Icon = new System.Drawing.Icon(Path.GetFullPath(@"image\graph.ico"));
                notifyIcon1.Text = "COMS";
                notifyIcon1.Visible = true;
                notifyIcon1.BalloonTipTitle = "VRB - COMS";
                notifyIcon1.BalloonTipText = "Bạn có 1 thông báo mới";
                notifyIcon1.ShowBalloonTip(100);
            }
            catch (Exception ex)
            {
            }
        }
        private void RunNews()
        {
            workerFunctionDelegate w = workerFunction;

            DataContainer container;
            string strNews = "";
            using (SAController ctrlSA = new SAController())
            {
                List<string> values = new List<string>();
                // Kiem tra Session neu null thi nghi
                FIS.Entities.Session session;
                ctrlSA.GetCurrentSessionInfo(out session);
                int type = 0;
                if (session != null)
                {
                    ctrlSA.ExecuteProcedureFillDataset(out container, "sp_alertlog_sel_all", values);
                    var dsResult2 = container.DataSet;
                    if (dsResult2.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsResult2.Tables[0].Rows.Count; i++)
                        {
                            strNews = " Tin mới : " + dsResult2.Tables[0].Rows[i][0].ToString();
                            barStaticItem4.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                            barStaticItem4.Appearance.ForeColor = System.Drawing.Color.Red;
                            barStaticItem4.Appearance.Options.UseFont = true;
                            barStaticItem4.Appearance.Options.UseForeColor = true;
                            barStaticItem4.ImageIndex = 8;
                            type = Convert.ToInt32(dsResult2.Tables[0].Rows[i][1].ToString());
                            w.BeginInvoke(strNews, null, null);
                        }
                    }
                    else
                    {
                        barStaticItem4.Caption = null;
                        barStaticItem4.ImageIndex = 99;
                    }
                    if (type == 1)  //Call form alert
                    {
                        frmAlert frm = new frmAlert();
                        frm.Show();
                    }
                }
            }
        }
        void workerFunction(string strText)
        {
            this.Invoke(new RunNewsDelegate(NewsDelegate), new object[] { strText });
            Thread.Sleep(5000);
        }
        void NewsDelegate(string strText)
        {
            barStaticItem4.Caption = strText;
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainProcess.ExecuteModule("02340", "MED");
        }
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainProcess.ExecuteMenu("MNU_EXIT");
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainProcess.ExecuteModule("LOGIN", "MMN");
        }

        private void barStaticItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainProcess.ExecuteModule("03271", "MMN");
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }

    }
}