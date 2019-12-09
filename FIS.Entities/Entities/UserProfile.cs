using System.Runtime.Serialization;
using FIS.Base;

namespace FIS.Entities
{
    // NOTE: ALL PROFILE VARIABLES WILL BE DEFINE IN HERE
    [DataContract]
    public class UserProfile
    {
        [DataMember, Column(Name = "SKINNAME"), SyncDB("Black")]
        public string ApplicationSkinName { get; set; }
        [DataMember, Column(Name = "PAGESIZE"), SyncDB("100")]
        public int MaxPageSize { get; set; }
        public UserProfile()
        {
            ApplicationSkinName = "Black";
            MaxPageSize = 100;
        }
    }
}
