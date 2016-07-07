using L2dotNET.GameService.Templates;

namespace L2dotNET.GameService.Model.Items
{
    class EtcItem : ItemTemplate
    {
        //private readonly EtcItemTypeId _type;

        public EtcItem(StatsSet set) : base(set) { }

        public override int GetItemMask()
        {
            // EtcItemType firstOrDefault = EtcItemType.Values.FirstOrDefault(x => x.Id == _type);
            // return firstOrDefault != null ? (int)firstOrDefault : 0;
            return 0;
        }
    }
}