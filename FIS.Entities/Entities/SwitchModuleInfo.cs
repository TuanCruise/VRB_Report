using System.Runtime.Serialization;
using FIS.Base;

namespace FIS.Entities
{
    [DataContract]
    public class SwitchModuleInfo : ModuleInfo
    {
        [DataMember, Column(Name="SWITCHSTORE")]
        public string SwitchStore { get; set; }
    }
}
