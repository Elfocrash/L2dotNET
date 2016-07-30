using System.Linq;
using L2dotNET.Enums;
using L2dotNET.GameService.Templates;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Model.Items
{
    public class Weapon : ItemTemplate
    {
        public WeaponTypeId Type { get; set; }
        private int AvoidModifier { get; set; }
        private int Pdef { get; set; }
        private int Mdef { get; set; }
        private int MpBonus { get; set; }
        private int HpBonus { get; set; }

        public Weapon(StatsSet set) : base(set)
        {
            Type = Utilz.GetEnumFromString(set.GetString("armor_type", "none"), WeaponTypeId.None);
            AvoidModifier = set.GetInt("avoid_modify");
            Pdef = set.GetInt("p_def");
            Mdef = set.GetInt("m_def");
            MpBonus = set.GetInt("mp_bonus");
            HpBonus = set.GetInt("hp_bonus");

            int bodyPart = BodyPart;
            if ((bodyPart == SlotNeck) || (bodyPart == SlotFace) || (bodyPart == SlotHair) || (bodyPart == SlotHairall) || ((bodyPart & SlotREar) != 0) || ((bodyPart & SlotLFinger) != 0) || ((bodyPart & SlotBack) != 0))
            {
                Type1 = Type1WeaponRingEarringNecklace;
                Type2 = Type2Accessory;
            }
            else
            {
                Type1 = Type1ShieldArmor;
                Type2 = Type2ShieldArmor;
            }
        }

        public override int GetItemMask()
        {
            var orDefault = WeaponType.Values.FirstOrDefault(x => x.Id == Type);
            if (orDefault != null)
            {
                int firstOrDefault = orDefault.GetMask();
                return firstOrDefault;
            }
            return 0;
        }

    }
}