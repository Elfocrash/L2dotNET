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
        public bool Stackable { get; set; }
        public MaterialType MaterialType { get; set; }
        public CrystalType CrystalType { get; set; }
        public int Duration { get; set; }
        public BodyPartType BodyPart { get; set; }
        public int ReferencePrice { get; set; }
        private readonly int _crystalCount;

        public bool Sellable { get; set; }
        public bool Dropable { get; set; }
        public bool Destroyable { get; set; }
        public bool Tradable { get; set; }
        public bool Depositable { get; set; }

        public bool HeroItem { get; set; }
        public bool IsOlyRestricted { get; set; }
        public ActionType DefaultAction { get; set; }

        public abstract int GetItemMask();

        protected ItemTemplate(StatsSet set)
        {
            ItemId = set.GetInt("item_id");
            Name = set.GetString("name");
            Type1 = set.GetInt("type1");
            Type2 = set.GetInt("type2");
            Weight = set.GetInt("weight");

            MaterialType = (MaterialType)set.GetInt("material");
            Duration = set.GetInt("duration", -1);
            BodyPart = (BodyPartType)set.GetInt("bodypart");
            ReferencePrice = set.GetInt("price");
            CrystalType = CrystalType.Values.FirstOrDefault(x => x.Id == (CrystalTypeId)set.GetInt("crystal_type"));
            _crystalCount = set.GetInt("crystal_count");

            Stackable = set.GetBool("stackable");
            Sellable = set.GetBool("sellable", true);
            Dropable = set.GetBool("dropable", true);
            Destroyable = set.GetBool("destroyable", true);
            Tradable = set.GetBool("tradeable", true);
            //Depositable = set.GetBool("is_depositable", true);

            HeroItem = ((ItemId >= 6611) && (ItemId <= 6621)) || (ItemId == 6842);
            //IsOlyRestricted = set.GetBool("is_oly_restricted");

            //DefaultAction = (ActionType)set.GetInt("default_action");
        }

        public int GetCrystalCount(int enchantLevel)
        {
            if (enchantLevel > 3)
            {
                switch (Type2)
                {
                    case 1:
                    case 2:
                        return _crystalCount + (CrystalType.CrystalEnchantBonusArmor * ((3 * enchantLevel) - 6));

                    case 0:
                        return _crystalCount + (CrystalType.CrystalEnchantBonusWeapon * ((2 * enchantLevel) - 3));

                    default:
                        return _crystalCount;
                }
            }

            if (enchantLevel <= 0)
                return _crystalCount;

            switch (Type2)
            {
                case 1:
                case 2:
                    return _crystalCount + (CrystalType.CrystalEnchantBonusArmor * enchantLevel);
                case 0:
                    return _crystalCount + (CrystalType.CrystalEnchantBonusWeapon * enchantLevel);
                default:
                    return _crystalCount;
            }
        }

    }
}