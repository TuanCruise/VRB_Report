using FIS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FIS.Entities
{
    [DataContract]
    [Serializable]
    public class ImportInfoDetails
    {
        [DataMember, Column(Name = "MODID")]
        public string ModuleID { get; set; }

        [DataMember, Column(Name = "IMPORTSTORE")]
        public string SubModule { get; set; }        
    }   
}
