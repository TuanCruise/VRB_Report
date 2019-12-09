using System;
using System.Threading;
using System.Data;
using System.Collections.Generic;
using FIS.Entities;
using FIS.Entities.FRead;
using FIS.Base;
using FIS.FReader.Controllers;


namespace FIS.FReader.Controllers
{
    public class txtPROCESS: AbstractProcess
    {
        public Session session;
        /// <summary>
        /// Khới động Process
        /// </summary>
        public override void Start()
        {
            try
            {

                using (var ctrlSA = new SAController(session))
                {
                    ctrlSA.CreateUserSession(out session, "huyvq", "1");
                }
            }
            catch (Exception ex)
            {
            }
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


        /// <summary>
        /// Nhân Process
        /// </summary>
        public void Run()
        {
            DataContainer container = null;
            List<string> value = new List<string>();
            try
            {
                using (var ctrlSA = new SAController(session))
                {
                    ctrlSA.ExecuteProcedureFillDataset(out container, "sp_list_defribbon", value);
                }
            }
            catch (ThreadAbortException)
            {
            }
            finally
            {
                WriteInfo("Đã kết thúc đọc dữ liệu");
                SetProcessState(ProcessState.Stopped);
            }
        }

        public override void Read()
        {

        }

        public override void CommitData()
        {
            base.CommitData();
        }

        protected override void SetProcessState(ProcessState State)
        {
            switch (State)
            {
                case ProcessState.Error:
                case ProcessState.Running:
                case ProcessState.Stopped:
                    ProcessStatusText = string.Format("HNX: Đã ngừng đọc", TotalError);
                    break;
            }
            base.SetProcessState(State);
        }
    }
}
