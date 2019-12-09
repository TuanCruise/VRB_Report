using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using FIS.FReader.Entities;
//using FIS.FRead;
//using FReader.Collections;

namespace FIS.FReader
{
    /// <summary>
    /// Created by LongND5, 2010-June-23
    /// Lớp mẫu cho một Process
    /// </summary>
    public abstract class AbstractProcess
    {
        public List<string> LogBuffer { get; private set; }
        public string LastError { get; private set; }
        public ProcessState ProcessState { get; protected set; }
        public int TotalError { get; private set; }
        public string ProcessStatusText { get; protected set; }
        public event EventHandler ProcessStateChange;
        protected Thread ProcessThread;

        //public abstract void Read(string filePatch);
        public abstract void Read();
        public abstract void Start();

        /// <summary>
        /// Khởi tạo Process
        /// </summary>
        public virtual void InitializeProcess()
        {
        }
        public AbstractProcess()
        {
            //DataFileInfo = new DataFileInfo();
            //Contract = new Contract();
            ProcessState = ProcessState.Stopped;
            LogBuffer = new List<string>();
            TotalError = 0;
        }

        /// <summary>
        /// Dừng Process
        /// </summary>
        public void Stop()
        {
            if (ProcessThread != null) ProcessThread.Abort();
        }

        /// <summary>
        /// Thiết đặt trạng thái Process
        /// </summary>
        /// <param name="State"></param>
        protected virtual void SetProcessState(ProcessState State)
        {
            ProcessState = State;
            if (ProcessStateChange != null) ProcessStateChange(this, new EventArgs());
        }

        /// <summary>
        /// Log thông tin
        /// </summary>
        /// <param name="Info">Thông tin cần log</param>
        protected void WriteInfo(string Info)
        {
            LogBuffer.Add(string.Format("[Thông báo] {0:HH:mm:ss} - {1}", DateTime.Now, Info));
        }

        /// <summary>
        /// Log lỗi
        /// </summary>
        /// <param name="Error">Chi tiết lỗi cần log</param>
        protected void WriteError(string Error)
        {
            var match = Regex.Match(Error, "<MES>([^<]+)</MES>");
            if(match.Success)
            {
                Error = match.Groups[1].Value;
                LastError = string.Format("\r\n*** [Lỗi] {0:HH:mm:ss} - {1} ***\r\n", DateTime.Now, Error);
            }
            else
            {
                LastError = string.Format("[Lỗi] {0:HH:mm:ss} - {1}", DateTime.Now, Error);                
            }
            TotalError++;
            LogBuffer.Add(LastError);
            SetProcessState(ProcessState.Error);
        }

        /// <summary>
        /// Log lỗi
        /// </summary>
        /// <param name="Exception">Ngoại lệ cần log</param>
        protected void WriteError(Exception Exception)
        {
            WriteError(Exception.Message);
        }

        /// <summary>
        /// Kiểm tra xem Giá chứng khoán thay đổi thì đưa vô Database
        /// </summary>
        public virtual void CommitData()
        {
            SetProcessState(ProcessState.Saving); 
        }
        /// <summary>
        /// Thông tin file
        /// </summary>
        //public DataFileInfo DataFileInfo { get; set; }
        /// <summary>
        /// Thông tin hợp đồng
        /// </summary>
        //public Contract Contract { get; set; }

    }
}
