using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Templates;
using L2dotNET.Utility;

namespace L2dotNET.Models.Items
{
    public class EtcItem : ItemTemplate
    {
        public EtcItemTypeId ItemType { get; set; }

        public override int GetItemMask()
        {
            return 1 << ((int)ItemType + 21);
        }
    }
}