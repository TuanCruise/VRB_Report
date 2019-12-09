using System.Collections.Generic;
using System.Diagnostics;
using DevExpress.XtraEditors.Repository;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Extensions;
using FIS.Utils;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace FIS.FReader
{
    public class ReaderEnvironment
    {
        public ClientInfo ClientInfo { get; set; }
        private CachedHashInfo m_ServerCachedHashInfo;
        protected ServerInfo m_ServerInfo;
        public ServerInfo ServerInfo
        {
            get
            {
                return m_ServerInfo;
            }
        }
        public CachedHashInfo ServerCachedHashInfo {
            get
            {
                return m_ServerCachedHashInfo;
            }
        }
        
        public EnvironmentType EnvironmentType
        {
            get { return EnvironmentType.CLIENT_APPLICATION; }
        }
  

        public ReaderEnvironment()
        {
            ClientInfo = new ClientInfo
                             {
                                 //LanguageID =MaximusRegistry.GetValueOrCreate(CONSTANTS.REGNAME_LANGID, CONSTANTS.DEFAULT_LANGID)
                                 LanguageID = CONSTANTS.DEFAULT_LANGID
                             };

            GetServerInfo();
        }

        public void GetServerInfo()
        {
            //using (var ctrlSA = new SAController())
            //{
            //    ctrlSA.GetServerInfo(out m_ServerInfo, out m_ServerCachedHashInfo, ClientInfo.LanguageID);
          //  }
        }
    }
}
