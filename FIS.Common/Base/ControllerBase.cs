using System;
using FIS.Common;

namespace FIS.Base
{
    public abstract class ControllerBase : IDisposable
    {
        protected string ConnectionString { get; set; }

        protected ControllerBase()
        {
            ConnectionString = App.Configs.ConnectionString;
        }

        protected ControllerBase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void Dispose()
        {
        }
    }
}
