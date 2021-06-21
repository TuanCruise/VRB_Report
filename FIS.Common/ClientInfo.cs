using FIS.Entities;
using System.Globalization;
using System.Runtime.Serialization;

namespace FIS
{
    [DataContract]
    public class ClientInfo
    {
        [DataMember]
        public string SessionKey { get; set; }
        public string UserName { get; set; }
        public string LanguageID { get; set; }
        public string IPAdress { get; set; }
        public string DNSName { get; set; }
        public CultureInfo Culture { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
