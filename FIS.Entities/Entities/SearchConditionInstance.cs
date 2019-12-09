using System.Runtime.Serialization;
using FIS.Base;

namespace FIS.Entities
{
    [DataContract]
    public class SearchConditionInstance
    {
        [DataMember, Column(Name = "CONDID")]
        public string ConditionID { get; set; }
        [DataMember, Column(Name = "SQLLOGIC")]
        public string SQLLogic { get; set; }
        [DataMember, Column(Name = "OPERATOR")]
        public string Operator { get; set; }
        [DataMember, Column(Name = "CONDID")]
        public string Value { get; set; }
        [DataMember]
        public SearchConditionInstance[] SubCondition { get; set; }
    }
}
