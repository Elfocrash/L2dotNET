using System.Linq;
using L2dotNET.Enums;
using L2dotNET.tables;
using L2dotNET.templates;

namespace L2dotNET.Models.items
{
    public abstract class ItemTemplate
    {
        public static readonly int Type1WeaponRingEarringNecklace = 0;
        public static readonly int Type1ShieldArmor = 1;
        public static readonly int Type1ItemQuestitemAdena = 4;

        public static readonly int Type2Weapon = 0;
        public static readonly int Type2ShieldArmor = 1;
        public static readonly int Type2Accessory = 2;
        public static readonly int Type2Quest = 3;
        public static readonly int Type2Money = 4;
        public static readonly int Type2Other = 5;

        public static readonly int SlotNone = 0x0000;
        public static readonly int SlotUnderwear = 0x0001;
        public static readonly int SlotREar = 0x0002;
        public static readonly int SlotLEar = 0x0004;
        public static readonly int SlotLrEar = 0x00006;
        public static readonly int SlotNeck = 0x0008;
        public static readonly int SlotRFinger = 0x0010;
        public static readonly int SlotLFinger = 0x0020;
        public static readonly int SlotLrFinger = 0x0030;
        public static readonly int SlotHead = 0x0040;
        public static readonly int SlotRHand = 0x0080;
        public static readonly int SlotLHand = 0x0100;
        public static readonly int SlotGloves = 0x0200;
        public static readonly int SlotChest = 0x0400;
        public static readonly int SlotLegs = 0x0800;
        public static readonly int SlotFeet = 0x1000;
        public static readonly int SlotBack = 0x2000;
        public static readonly int SlotLrHand = 0x4000;
        public static readonly int SlotFullArmor = 0x8000;
        public static readonly int SlotFace = 0x010000;
        public static readonly int SlotAlldress = 0x020000;
        public static readonly int SlotHair = 0x040000;
        public static readonly int SlotHairall = 0x080000;

        public static readonly int SlotWolf = -100;
        public static readonly int SlotHatchling = -101;
        public static readonly int SlotStrider = -102;
        public static readonly int SlotBabypet = -103;

        public static readonly int SlotAllweapon = SlotLrHand | SlotRHand;

        public int ItemId { get; set; }
        public string Name { get; set; }
        public int Type1 { get; set; }
        public int Type2 { get; set; }
        public int Weight { get; set; }
        public bool Stackable { get; set; }
        public MaterialType MaterialType { get; set; }
        public CrystalType CrystalType { get; set; }
        public int Duration { get; set; }
        public int BodyPart { get; set; }
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
            BodyPart = ItemTable.Instance.Slots[set.GetString("bodypart", "none")];
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