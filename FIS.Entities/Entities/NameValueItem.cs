using System.Runtime.Serialization;
using FIS.Base;

namespace FIS.Entities
{
    [DataContract]
    public class NameValueItem
    {
        [DataMember, Column(Name = "IMAGE")]
        public string ImageName { get; set; }
        [DataMember, Column(Name = "TEXT")]
        public string Text { get; set; }
        [DataMember, Column(Name = "VALUE")]
        public string Value { get; set; }
    }
}
