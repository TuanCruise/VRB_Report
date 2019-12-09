using FIS.Base;
using System.Runtime.Serialization;

namespace FIS.Entities
{
    [DataContract]
    public class OracleStore : EntityBase
    {
        [DataMember, Column(Name = "STORENAME")]
        public string StoreName { get; set; }
    }
}
