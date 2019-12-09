using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using FIS.Common;

namespace FIS.Utils
{
    public static class ThreadUtils
    {
        public static void SetCultureInfo(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public static void SetClientCultureInfo()
        {
            SetCultureInfo(App.Environment.ClientInfo.Culture);
        }
    }
    public class WorkerThread : IDisposable
    {
        public delegate void WorkerDelegate(WorkerThread worker);
        public bool AbortRequest { get; set; }
        public double RefreshTime { get; set; }
        public bool IsSuccessful { get; set; }
        public WorkerDelegate Worker { get; set; }
        public float PercentComplete { get; set; }
        public string StatusText { get; set; }
        public string JobName { get; set; }
        public Control Parent { get; set; }
        private DateTime m_LastUpdateGui;
        private Thread m_MainThread;

        public event EventHandler DoUpdateGUI;
        public event EventHandler ProcessComplete;

        public WorkerThread(WorkerDelegate worker, Control parent)
        {
            AbortRequest = false;
            Parent = parent;
            Worker = worker;
            RefreshTime = 500;
            IsSuccessful = false;
            m_LastUpdateGui = DateTime.MinValue;
            ThreadUtils.SetClientCultureInfo();
        }

        public void Start()
        {
            m_MainThread = new Thread(delegate()
                {
                    Worker(this);
                    if (ProcessComplete != null && Parent != null && !Parent.Disposing)
                    {
                        if (Parent.InvokeRequired)
                            Parent.Invoke(ProcessComplete, this, null);
                        else
                            ProcessComplete(this, null);
                    }
                });
            m_MainThread.Start();
        }

        public void Stop()
        {
            AbortRequest = true;
            ExecuteUpdateGUI(true);
        }

        public delegate void ExecuteUpdateGUIInvoker(bool force);

        public void ExecuteUpdateGUI(bool force)
        {
            if (DoUpdateGUI != null)
            {
                if (force)
                {
                    if (Parent.InvokeRequired)
                    {
                        Parent.BeginInvoke(new ExecuteUpdateGUIInvoker(ExecuteUpdateGUI), true);
                        return;
                    }

                    if (!Parent.InvokeRequired)
                    {
                        DoUpdateGUI(this, null);
                    }

                    return;
                }
                
                if ((DateTime.Now - m_LastUpdateGui).TotalMilliseconds > RefreshTime)
                {
                    m_LastUpdateGui = DateTime.Now;
                    ExecuteUpdateGUI(true);
                }
            }
        }

        public void ExecuteUpdateGUI()
        {
            ExecuteUpdateGUI(false);
        }

        public void SendAbort()
        {
            AbortRequest = true;
        }

        public void Dispose()
        {
            Stop();
        }
    }

    public class ThreadCollection : List<WorkerThread>, IDisposable
    {
        #region IDisposable Members
        public void Dispose()
        {
            foreach (var thread in this)
            {
                thread.Dispose();
            }
        }
        #endregion
    }
}
