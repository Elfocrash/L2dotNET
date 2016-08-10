using L2dotNET.model.items;

namespace L2dotNET.tables.multisell
{
    public class MultiSellItem
    {
        public int Id;
        public int Count;
        public ItemTemplate Template;

        public short Enchant
        {
            get
            {
                if (L2Item != null)
                    return (short)L2Item.Enchant;

                return 0;
            }
        }

        public int Augment = 0;
        public L2Item L2Item;

        public int Durability { get; set; }

        public short Type2
        {
            get
            {
                if (Template == null)
                    return 0;

                if (L2Item != null)
                    return (short)L2Item.Template.Type2;

                return (short)Template.Type2;
            }
        }

        public int BodyPartId
        {
            get
            {
                if (Template == null)
                    return 0;

                if (L2Item != null)
                    return L2Item.Template.BodyPart;

                return Template.BodyPart;
            }
        }

        public int ItemId => Id;
    }
}