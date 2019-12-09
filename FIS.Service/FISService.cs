using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Threading;
using FIS.Common;
using FIS.Controllers;
using FIS.Utils;

namespace FIS.Service
{
    partial class FISService : ServiceBase
    {
        private List<ServiceHost> m_ServiceHosts;
        
        public FISService()
        {
            InitializeComponent();
        }

        public void InitializeServiceHost<T>()
        {
            var uri = new Uri(App.Configs.ServiceUri);
            var urihttp = new Uri(App.Configs.ServiceUriHttp);
            var serviceHost = new ServiceHost(typeof(T), uri);
            var serviceHostHttp = new ServiceHost(typeof(T), urihttp);

            if (!string.IsNullOrEmpty(urihttp.ToString()))
            {
                serviceHostHttp.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;                                                
                var binding = CommonUtils.CreateHttpBinding();
                serviceHostHttp.AddServiceEndpoint(typeof(T), binding, typeof(T).Name, urihttp);
                ServiceMetadataBehavior smb = serviceHostHttp.Description.Behaviors.Find<ServiceMetadataBehavior>();
                if (smb == null)
                {
                    smb = new ServiceMetadataBehavior();
                    smb.HttpGetEnabled = true;
                    serviceHostHttp.Description.Behaviors.Add(smb);
                }                                    
            }

            if (App.Configs.ServiceUri.StartsWith("net.tcp"))
            if (!string.IsNullOrEmpty(uri.ToString()))
            {
                serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;
                var binding = CommonUtils.CreateTcpBinding();
                serviceHost.AddServiceEndpoint(typeof(T), binding, typeof(T).Name, uri);

                ServiceThrottlingBehavior throttle = serviceHost.Description.Behaviors.Find<ServiceThrottlingBehavior>(); 
                if (throttle == null) 
                    { 
                        throttle = new ServiceThrottlingBehavior();
                        throttle.MaxConcurrentCalls = int.MaxValue; throttle.MaxConcurrentInstances = int.MaxValue; throttle.MaxConcurrentSessions = int.MaxValue;
                        serviceHost.Description.Behaviors.Add(throttle); 
                    }
            }

            m_ServiceHosts.Add(serviceHost);
            m_ServiceHosts.Add(serviceHostHttp);
        }

        public void OpenService()
        {
            try
            {
                App.Environment = new ServerEnvironment();
                m_ServiceHosts = new List<ServiceHost>();
                InitializeServiceHost<SAController>();

                foreach (var serviceHost in m_ServiceHosts)
                {
                    serviceHost.Open();
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Fis.Service", ex.Message, EventLogEntryType.Error);
                Stop();
            }
        }

        public void CloseService()
        {
            foreach (var serviceHost in m_ServiceHosts)
            {
                serviceHost.Close();
            }
        }

        protected override void OnStart(string[] args)
        {
            var thread = new Thread(OpenService);
            thread.Start();
        }

        protected override void OnStop()
        {
            CloseService();
        }

        public void StartDebug()
        {
            m_ServiceHosts = new List<ServiceHost>();
            InitializeServiceHost<SAController>();

            foreach (var serviceHost in m_ServiceHosts)
            {
                serviceHost.Open();
            }

            while (Thread.CurrentThread.ThreadState == System.Threading.ThreadState.Running) Thread.Sleep(5000);
        }
    }
}