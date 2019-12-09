using System.Runtime.Serialization;
using FIS.Base;

namespace FIS.Entities
{
    [DataContract]
    public class ExecProcModuleInfo : ModuleInfo
    {
        [DataMember, Column(Name = "EXECUTESTORE")]
        public string ExecuteStore { get; set; }
    }
}
