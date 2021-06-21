﻿using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace FIS
{
    [DataContract]
    public class ServerInfo
    {
        private string m_CultureName;
        [DataMember]
        public string CultureName
        {
            get { return m_CultureName; }
            set
            {
                m_CultureName = value;
                Culture = CultureInfo.GetCultureInfo(m_CultureName);
            }
        }
        public DateTime ServerNow
        {
            get { return DateTime.Now; }
        }
        public CultureInfo Culture { get; set; }
        public string ApplicationName { get; set; }
    }
}
