using System.Runtime.Serialization;
using FIS.Base;

namespace FIS.Entities
{
    [DataContract]
    public class ImportModuleInfo : ModuleInfo
    {
        [DataMember, Column(Name = "IMPORTSTORE")]
        public string ImportStore { get; set; }
        [DataMember, Column(Name = "SELECTSTORE")]
        public string SelectStore { get; set; }
        [DataMember, Column(Name = "EXCELNAME")]
        public string ExcelName { get; set; }
    }
}
