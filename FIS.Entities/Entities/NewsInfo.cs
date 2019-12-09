using System.Runtime.Serialization;
using FIS.Base;

namespace FIS.Entities
{
    [DataContract]
    public class NewsInfo : EntityBase
    {
        [DataMember, Column(Name = "NEWSID")]
        public string NewsID { get; set; }
        [DataMember, Column(Name = "SYMBOL")]
        public string Symbol { get; set; }
        [DataMember, Column(Name = "TITLE")]
        public string Title { get; set; }
    }
}
