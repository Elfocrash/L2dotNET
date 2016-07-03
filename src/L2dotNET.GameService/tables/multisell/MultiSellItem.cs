using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Tables.Multisell
{
    public class MultiSellItem
    {
        public int id;
        public int count;
        public ItemTemplate template;

        public short enchant
        {
            get
            {
                if (l2item != null)
                    return (short)l2item.Enchant;

                if ((template != null) && (template.enchanted > 0))
                    return template.enchanted;

                return 0;
            }
        }

        public int augment = 0;
        public L2Item l2item;

        public short AttrAttackType
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrAttackType;

                if (template == null)
                    return -2;

                return template.AttrAttackType;
            }
        }

        public short AttrAttackValue
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrAttackValue;

                return template == null ? (short)0 : template.AttrAttackValue;
            }
        }

        public short AttrDefenseValueFire
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrDefenseValueFire;

                if (template == null)
                    return 0;

                return template.AttrDefenseValueFire;
            }
        }

        public short AttrDefenseValueWater
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrDefenseValueWater;

                if (template == null)
                    return 0;

                return template.AttrDefenseValueWater;
            }
        }

        public short AttrDefenseValueWind
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrDefenseValueWind;

                if (template == null)
                    return 0;

                return template.AttrDefenseValueWind;
            }
        }

        public short AttrDefenseValueEarth
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrDefenseValueEarth;

                if (template == null)
                    return 0;

                return template.AttrDefenseValueEarth;
            }
        }

        public short AttrDefenseValueHoly
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrDefenseValueHoly;

                if (template == null)
                    return 0;

                return template.AttrDefenseValueHoly;
            }
        }

        public short AttrDefenseValueUnholy
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrDefenseValueUnholy;

                if (template == null)
                    return 0;

                return template.AttrDefenseValueUnholy;
            }
        }

        public int Durability
        {
            get
            {
                if (l2item != null)
                    return l2item.Template.Durability;

                if (template == null)
                    return 0;

                return template.Durability;
            }
        }

        public short Type2
        {
            get
            {
                if (template == null)
                    return 0;

                if (l2item != null)
                    return l2item.Template.Type2();

                return template.Type2();
            }
        }

        public int BodyPartId
        {
            get
            {
                if (template == null)
                    return 0;

                if (l2item != null)
                    return l2item.Template.BodyPartId();

                return template.BodyPartId();
            }
        }

        public int ItemID
        {
            get { return id; }
        }
    }
}