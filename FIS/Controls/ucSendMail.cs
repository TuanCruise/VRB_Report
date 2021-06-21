using DevExpress.XtraLayout;
using DevExpress.XtraRichEdit;
using FIS.AppClient.Interface;
using FIS.Base;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Text;

namespace FIS.AppClient.Controls
{
    public partial class ucSendMail : ucModule,
        ICommonFieldSupportedModule
    {
        public ucSendMail()
        {
            InitializeComponent();
            mainLayout.AllowCustomizationMenu = true;
        }

        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();
            lbTitle.Text = Language.Title;
        }

        RichEditControl richEdit;

        protected override void LoadCommandFields()
        {
            base.LoadCommandFields();
            CommonFields.Clear();
            CommonFields.AddRange(
                FieldUtils.GetModuleFields(
                    ModuleInfo.ModuleType,
                    CODES.DEFMODFLD.FLDGROUP.SEND_MAIL
                ));
            //richEdit = new RichEditControl
            //{
            //    Name = "rtxLoadReport"
            //};
            //mainLayout.Controls.Add(richEdit);
        }

        protected override void BuildButtons()
        {
#if DEBUG
            SetupContextMenu(mainLayout);
            SetupLanguageTool();
            SetupSaveLayout(mainLayout);
#endif
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            lbTitle.BackColor = ThemeUtils.BackTitleColor;
            lbTitle.ForeColor = ThemeUtils.TitleColor;
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

        public override void UnLockUserAction()
        {
            base.UnLockUserAction();

            if (!InvokeRequired)
            {
                HideWaitingBox();
                Enabled = true;
            }
        }

        public bool ValidateRequire
        {
            get { return true; }
        }

        public LayoutControl CommonLayout
        {
            get { return mainLayout; }
        }

        public string CommonLayoutStoredData
        {
            get { return Language.SendMailLayout; }
        }

        private void btnSendMail_Click(object sender, EventArgs e)
        {
            if (ValidateModule())
            {
                SendMail();
            }
        }
        private void SendMail()
        {
            //--Param


            string strListAddressTo = this["S01"].ToString();
            //string strListAddressTo = "phuongthao181086@yahoo.com,tranthanhtrung1987@yahoo.com,phuongthao.myosung@gmail.com";
            string[] strArrayAddressTo = strListAddressTo.Split(',');

            //--Mail config
            DataContainer container;
            DataSet dsResult2;
            using (SAController ctrlSA = new SAController())
            {
                List<string> values = new List<string>();
                ctrlSA.ExecuteProcedureFillDataset(out container, "SP_MAILCONFIG_SEL", values);
                dsResult2 = container.DataSet;

            }

            //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpClient SmtpServer = new SmtpClient(dsResult2.Tables[0].Rows[0]["MAIL_ID"].ToString());
            //SmtpServer.Port = 587;
            SmtpServer.Port = Int32.Parse(dsResult2.Tables[0].Rows[0]["PORT"].ToString());
            //SmtpServer.Credentials = new System.Net.NetworkCredential("tranthanhtrung.fithou", "vuthiphuongthao");
            string[] strMailName = dsResult2.Tables[0].Rows[0]["MAIL_NAME"].ToString().Split('@');
            SmtpServer.Credentials = new System.Net.NetworkCredential(strMailName[0].ToString(), dsResult2.Tables[0].Rows[0]["PASSWORD"].ToString());
            SmtpServer.EnableSsl = false;
            //string strAddress = "tranthanhtrung.fithou@gmail.com";
            string strAddress = dsResult2.Tables[0].Rows[0]["MAIL_NAME"].ToString();
            //--Send mail
            foreach (string item in strArrayAddressTo)
            {
                MailMessage mail = new MailMessage(new MailAddress(strAddress), new MailAddress(item));

                //--Subject and Body
                mail.BodyEncoding = Encoding.Default;
                mail.Subject = lbTitle.Text;
                mail.Body = this["S03"].ToString();
                mail.Priority = MailPriority.High;
                mail.IsBodyHtml = true;
                //--attachment
                if (path != "")
                {
                    foreach (string fileAttachment in AttachmentFiles)
                    {
                        mail.Attachments.Add(new Attachment(fileAttachment));
                    }
                }
                try
                {
                    SmtpServer.Send(mail);
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }

            }
            CloseModule();
        }

        String[] AttachmentFiles = null;
        string path = "";
        private void lnkFile_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            //openFile.Filter = "Mp3 Files|*.Mp3|Wma Files|*.wma|All Files|*.*";
            openFile.Filter = "All Files|*.*";
            openFile.Multiselect = false;
            openFile.ShowDialog();
            String MusicFile = "";
            MusicFile = openFile.FileName;
            //String[] MusicFiles = openFile.FileNames;
            AttachmentFiles = openFile.FileNames;
            path = "attachment";
        }
        public bool ValidateModule()
        {
            var result = true;
            errorProvider.ClearErrors();

            var commonFieldSupportedModule = this as ICommonFieldSupportedModule;
            if (commonFieldSupportedModule != null)
            {
                if (commonFieldSupportedModule.ValidateRequire && !ValidateCommonFieldSupportedModule(commonFieldSupportedModule))
                {
                    result = false;
                }
            }

            var columnFieldSupportedModule = this as IColumnFieldSupportedModule;
            if (columnFieldSupportedModule != null)
            {
                UpdateStoreRepositories();
            }

            return result;
        }
    }
}
