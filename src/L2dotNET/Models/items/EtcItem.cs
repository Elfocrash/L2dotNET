using L2dotNET.Enums;
using L2dotNET.templates;
using L2dotNET.Utility;

namespace L2dotNET.Models.items
{
    public class EtcItem : ItemTemplate
    {
        public EtcItemTypeId Type { get; set; }

        public EtcItem(StatsSet set) : base(set)
        {
            Type = Utilz.GetEnumFromString(set.GetString("item_type", "None"), EtcItemTypeId.None);
        }

        public override int GetItemMask()
        {
            return 1 << ((int)Type + 21);
        }
    }
}