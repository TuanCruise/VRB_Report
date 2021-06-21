using FIS.Common;
using FIS.Utils;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace FIS.Base
{
    public abstract class RemoteControllerBase
    {
        protected Binding m_Binding;
        protected EndpointAddress m_EndpointAddress;

        protected RemoteControllerBase()
        {
            try
            {
                if (App.Configs.ServiceUri == null)
                {
                    if (App.Configs.ServiceUri.StartsWith("net.tcp"))
                    {
                        m_Binding = CommonUtils.CreateTcpBinding();
                    }
                    if (App.Configs.ServiceUri.StartsWith("http"))
                    {
                        m_Binding = CommonUtils.CreateHttpBinding();
                    }
                    m_EndpointAddress = new EndpointAddress(App.Configs.ServiceUri + "/" + GetType().Name);
                }
                else
                {
                    if (App.Configs.ServiceUri.StartsWith("net.tcp"))
                    {
                        m_Binding = CommonUtils.CreateTcpBinding();
                    }
                    if (App.Configs.ServiceUri.StartsWith("http"))
                    {
                        m_Binding = CommonUtils.CreateHttpBinding();
                    }
                    m_EndpointAddress = new EndpointAddress(App.Configs.ServiceUri + "/" + GetType().Name);
                }
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_CONNECT_TO_SERVER_FAIL, ex.Message);
            }

        }
    }
}
