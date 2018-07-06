using System.Linq;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Enums;
using L2dotNET.Templates;

namespace L2dotNET.Models.Items
{
    public abstract class ItemTemplate
    {
        public int ItemId { get; set; }
        public string Name { get; set; }

        public int Type1 { get; set; }
        public int Type2 { get; set; }

        public int Weight { get; set; }
        public int Duration { get; set; }
        public int Price { get; set; }
        public CrystalTypeId CrystalType { get; set; }
        public int CrystalCount { get; set; }
        public bool Crystallizable { get; set; }
        public bool Stackable { get; set; }
        public bool Sellable { get; set; }
        public bool Dropable { get; set; }
        public bool Destroyable { get; set; }
        public bool Tradeable { get; set; }
        public bool Depositable { get; set; }
        public BodyPartType BodyPart { get; set; }
        public bool HeroItem => ItemId >= 6611 && ItemId <= 6621 || ItemId == 6842;
        public bool IsOlyRestricted { get; set; }
        public ActionType DefaultAction { get; set; }

        public abstract int GetItemMask();

        public CrystalType GetCrystalType()
        {
            return Enums.CrystalType.Values.First(x => x.Id == CrystalType);
        }

        public int GetCrystalCount(int enchantLevel)
        {
            CrystalType crystalType = GetCrystalType();

            if (enchantLevel > 3)
            {
                switch (Type2)
                {
                    case 1:
                    case 2:
                        return CrystalCount + crystalType.CrystalEnchantBonusArmor * (3 * enchantLevel - 6);

                    case 0:
                        return CrystalCount + crystalType.CrystalEnchantBonusWeapon * (2 * enchantLevel - 3);

                    default:
                        return CrystalCount;
                }
            }

            if (enchantLevel <= 0)
                return CrystalCount;

            switch (Type2)
            {
                case 1:
                case 2:
                    return CrystalCount + crystalType.CrystalEnchantBonusArmor * enchantLevel;
                case 0:
                    return CrystalCount + crystalType.CrystalEnchantBonusWeapon * enchantLevel;
                default:
                    return CrystalCount;
            }
        }

    }
}