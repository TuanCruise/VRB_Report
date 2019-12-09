using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using System.Net.Mail;
using FIS.FReader.Entities;
using FIS.Base;

namespace FIS.FReader.Process
{
    public class warningPROCESS : AbstractProcess
    {
        public override void Start()
        {
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


        public void Run()
        {

            try
            {
                while (Thread.CurrentThread.ThreadState == ThreadState.Running)
                {
                    try
                    {
                        using (SAController ctrlSA = new SAController(FReadEntities.MainSession))
                        {
                            DataContainer dc = null;
                            List<string> values = new List<string>();
                            ctrlSA.ExecuteProcedureFillDataset(out dc, "IMP_WARNINGEMAIL", values);
                        }
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
                SetProcessState(ProcessState.Stopped);
            }
        }

        public override void Read()
        {
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


        private void SendMail()
        {
            //--Param


            //string strListAddressTo = this["S01"].ToString();
            string strListAddressTo = "huyvq3@fpt.com.vn";
            string[] strArrayAddressTo = strListAddressTo.Split(',');

            //--Mail config
            DataContainer container;
            DataSet dsResult2;
            using (SAController ctrlSA = new SAController(FReadEntities.MainSession))
            {
                List<string> values = new List<string>();
                ctrlSA.ExecuteProcedureFillDataset(out container, "SP_MAILCONFIG_SEL", values);
                dsResult2 = container.DataSet;

            }

            SmtpClient SmtpServer = new SmtpClient(dsResult2.Tables[0].Rows[0]["MAIL_ID"].ToString());
            SmtpServer.Port = Int32.Parse(dsResult2.Tables[0].Rows[0]["PORT"].ToString());
            string[] strMailName = dsResult2.Tables[0].Rows[0]["MAIL_NAME"].ToString().Split('@');
            SmtpServer.Credentials = new System.Net.NetworkCredential(strMailName[0].ToString(), dsResult2.Tables[0].Rows[0]["PASSWORD"].ToString());
            SmtpServer.EnableSsl = false;
            string strAddress = dsResult2.Tables[0].Rows[0]["MAIL_NAME"].ToString();
            //--Send mail
            foreach (string item in strArrayAddressTo)
            {
                MailMessage mail = new MailMessage(new MailAddress(strAddress), new MailAddress(item));

                //--Subject and Body
                mail.BodyEncoding = Encoding.Default;
                mail.Subject = "[FREAD]WARNING EMAIL";
                mail.Body = "WARNING MAIL TEST";
                mail.Priority = MailPriority.High;
                mail.IsBodyHtml = true;
                //--attachment
                //if (path != "")
                //{
                //    foreach (string fileAttachment in AttachmentFiles)
                //    {
                //        mail.Attachments.Add(new Attachment(fileAttachment));
                //    }
                //}
                try
                {
                    SmtpServer.Send(mail);
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
