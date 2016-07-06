using System.Linq;
using L2dotNET.GameService.Enums;
using L2dotNET.GameService.Templates;

namespace L2dotNET.GameService.Model.Items
{
    public class Armor : ItemTemplate
    {
        private readonly ArmorTypeId _type;

        public Armor(StatsSet set) : base(set)
        {
            ArmorType firstOrDefault = ArmorType.Values.FirstOrDefault(x => x.Name == set.GetString("armor_type", "none").ToUpper());
            _type = firstOrDefault?.Id ?? ArmorType.None.Id;

            int bodyPart = BodyPart;
            if ((bodyPart == SlotNeck) || (bodyPart == SlotFace) || (bodyPart == SlotHair) || (bodyPart == SlotHairall) || ((bodyPart & SlotREar) != 0) || ((bodyPart & SlotLFinger) != 0) || ((bodyPart & SlotBack) != 0))
            {
                Type1 = Type1WeaponRingEarringNecklace;
                Type2 = Type2Accessory;
            }
            else
            {
                if ((_type == ArmorType.None.Id) && (BodyPart == SlotLHand)) // retail define shield as NONE
                {
                    _type = ArmorType.Shield.Id;
                }

                Type1 = Type1ShieldArmor;
                Type2 = Type2ShieldArmor;
            }
        }

        public override int GetItemMask()
        {
            ArmorType firstOrDefault = ArmorType.Values.FirstOrDefault(x => x.Id == _type);
            return firstOrDefault != null ? (int)firstOrDefault : 0;
        }
    }
}