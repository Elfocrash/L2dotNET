using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Templates;
using L2dotNET.Utility;

namespace L2dotNET.Models.Items
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