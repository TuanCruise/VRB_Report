using System;
using System.Runtime.Serialization;
using FIS.Base;

namespace FIS.Entities
{
    [DataContract]
    public class User
    {
        [DataMember, Column(Name = "USERID")]
        public int UserID { get; set; }
        [DataMember, Column(Name = "GROUPID")]
        public string GroupID { get; set; }
        [DataMember, Column(Name = "USERNAME")]
        public string Username { get; set; }
        [DataMember, Column(Name = "DISPLAYNAME")]
        public string DisplayName { get; set; }
        [DataMember, Column(Name = "USERSTATUS")]
        public string UserStatus { get; set; }
        [DataMember, Column(Name = "CREATEDATE")]
        public DateTime CreateDate { get; set; }
        [DataMember, Column(Name = "LASTMODIFY")]
        public DateTime LastModify { get; set; }
        [DataMember, Column(Name = "LASTLOGIN")]
        public DateTime LastLogin { get; set; }

        public override string ToString()
        {
            return Username + " - " + DisplayName;
        }
    }
}
