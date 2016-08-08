using L2dotNET.Enums;
using L2dotNET.GameService.Templates;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Model.Items
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
            return 1 << (int)Type + 21;
        }
    }
}