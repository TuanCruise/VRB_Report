using System.Runtime.Serialization;
using FIS.Base;

namespace FIS.Entities
{
    [DataContract]
    public class MenuItemInfo : EntityBase
    {
        [DataMember, Column(Name="MENUID")]
        public string MenuItemID { get; set; }
        [DataMember, Column(Name = "OWNERMENUID")]
        public string OwnerMenuItemID { get; set; }
        [DataMember, Column(Name = "MENUNAME")]
        public string MenuItemName { get; set; }
        [DataMember, Column(Name = "MODID")]
        public string ModuleID { get; set; }
        [DataMember, Column(Name = "SUBMOD")]
        public string SubModule { get; set; }
        [DataMember, Column(Name = "BEGINGROUP")]
        public string BeginGroup { get; set; }
        [DataMember, Column(Name = "MENUTYPE")]
        public string MenuType { get; set; }
    }
}
