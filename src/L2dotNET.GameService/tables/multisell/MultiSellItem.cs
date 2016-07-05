using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Tables.Multisell
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

                if ((Template != null) && (Template.Enchanted > 0))
                    return Template.Enchanted;

                return 0;
            }
        }

        public int Augment = 0;
        public L2Item L2Item;

        public short AttrAttackType
        {
            get
            {
                if (L2Item != null)
                    return L2Item.AttrAttackType;

                if (Template == null)
                    return -2;

                return Template.AttrAttackType;
            }
        }

        public short AttrAttackValue
        {
            get
            {
                if (L2Item != null)
                    return L2Item.AttrAttackValue;

                return Template == null ? (short)0 : Template.AttrAttackValue;
            }
        }

        public short AttrDefenseValueFire
        {
            get
            {
                if (L2Item != null)
                    return L2Item.AttrDefenseValueFire;

                if (Template == null)
                    return 0;

                return Template.AttrDefenseValueFire;
            }
        }

        public short AttrDefenseValueWater
        {
            get
            {
                if (L2Item != null)
                    return L2Item.AttrDefenseValueWater;

                if (Template == null)
                    return 0;

                return Template.AttrDefenseValueWater;
            }
        }

        public short AttrDefenseValueWind
        {
            get
            {
                if (L2Item != null)
                    return L2Item.AttrDefenseValueWind;

                if (Template == null)
                    return 0;

                return Template.AttrDefenseValueWind;
            }
        }

        public short AttrDefenseValueEarth
        {
            get
            {
                if (L2Item != null)
                    return L2Item.AttrDefenseValueEarth;

                if (Template == null)
                    return 0;

                return Template.AttrDefenseValueEarth;
            }
        }

        public short AttrDefenseValueHoly
        {
            get
            {
                if (L2Item != null)
                    return L2Item.AttrDefenseValueHoly;

                if (Template == null)
                    return 0;

                return Template.AttrDefenseValueHoly;
            }
        }

        public short AttrDefenseValueUnholy
        {
            get
            {
                if (L2Item != null)
                    return L2Item.AttrDefenseValueUnholy;

                if (Template == null)
                    return 0;

                return Template.AttrDefenseValueUnholy;
            }
        }

        public int Durability
        {
            get
            {
                if (L2Item != null)
                    return L2Item.Template.Durability;

                if (Template == null)
                    return 0;

                return Template.Durability;
            }
        }

        public short Type2
        {
            get
            {
                if (Template == null)
                    return 0;

                if (L2Item != null)
                    return L2Item.Template.Type2();

                return Template.Type2();
            }
        }

        public int BodyPartId
        {
            get
            {
                if (Template == null)
                    return 0;

                if (L2Item != null)
                    return L2Item.Template.BodyPartId();

                return Template.BodyPartId();
            }
        }

        public int ItemId => Id;
    }
}