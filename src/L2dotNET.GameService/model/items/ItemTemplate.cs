using System.Collections.Generic;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Model.Skills2.Effects;

namespace L2dotNET.GameService.Model.Items
{
    public class ItemTemplate
    {
        public int ItemId;

        public L2ItemBodypart Bodypart;

        public int Durability = -1;

        public L2ItemType Type = L2ItemType.Etcitem;
        public L2ItemArmorType TypeArmor = L2ItemArmorType.None;
        public L2ItemConsume StackType = L2ItemConsume.Normal;
        public L2ItemGrade CrystallGrade = L2ItemGrade.None;
        public L2ItemWeaponType WeaponType = L2ItemWeaponType.Sword;

        public int RecipeId = 0;
        public int Weight = 0;
        public int CryCount = 0;

        public int MpConsume = 0;
        public int LimitedMinutes = 0;

        public bool IsAutoSs
        {
            get { return (DefaultAction == "action_soulshot") || (DefaultAction == "action_spiritshot") || (DefaultAction == "action_summon_soulshot") || (DefaultAction == "action_summon_spiritshot"); }
        }

        public int SoulshotCount = 0,
                   SpiritshotCount = 0;
        public int Price = 0;
        public int AbnormalMaskEvent = 0;

        //  public TSkill critical_attack_skill, attack_skill, magic_skill;

        public int IsTrade = 1,
                   IsDrop = 1,
                   IsDestruct = 1,
                   EnchantEnable = 0;
        public int IsPrivateStore = 1,
                   ElementalEnable = 0,
                   IsOlympiadCanUse = 1,
                   IsPremium = 0;
        public int PhysicalDamage,
                   RandomDamage,
                   AttackRange,
                   AttackSpeed,
                   MagicalDamage;
        public int PhysicalDefense,
                   MagicalDefense,
                   MpBonus,
                   MagicWeapon;

        public int AvoidModify,
                   ShieldDefense,
                   ShieldDefenseRate;
        public double HitModify;

        public int BaseAttrAttackType = -2;
        public int BaseAttrAttackValue;
        public int BaseAttrDefenseValueFire;
        public int BaseAttrDefenseValueWater;
        public int BaseAttrDefenseValueWind;
        public int BaseAttrDefenseValueEarth;
        public int BaseAttrDefenseValueHoly;
        public int BaseAttrDefenseValueUnholy;

        public int GetCrystallId()
        {
            switch (CrystallGrade)
            {
                case L2ItemGrade.D:
                    return 1458;
                case L2ItemGrade.C:
                    return 1459;
                case L2ItemGrade.B:
                    return 1460;
                case L2ItemGrade.A:
                    return 1461;
                case L2ItemGrade.S:
                case L2ItemGrade.S80:
                case L2ItemGrade.S84:
                case L2ItemGrade.S86:
                    return 1462;
            }

            return 0;
        }

        public int[] GetSoulshots()
        {
            switch (CrystallGrade)
            {
                case L2ItemGrade.None:
                    return new[] { 1835, 5789 };
                case L2ItemGrade.D:
                    return new[] { 1463, 22082 };
                case L2ItemGrade.C:
                    return new[] { 1464, 22083 };
                case L2ItemGrade.B:
                    return new[] { 1465, 22084 };
                case L2ItemGrade.A:
                    return new[] { 1466, 22085 };
                case L2ItemGrade.S:
                case L2ItemGrade.S80:
                case L2ItemGrade.S84:
                case L2ItemGrade.S86:
                    return new[] { 1467, 22086 };
            }

            return null;
        }

        public enum L2ItemType
        {
            Weapon,
            Armor,
            Etcitem,
            Questitem,
            Accessary,
            Asset
        }

        public enum L2ItemArmorType
        {
            None,
            Magic,
            Light,
            Heavy
        }

        public enum L2ItemConsume
        {
            Normal,
            Stackable
        }

        public enum L2ItemGrade
        {
            None,
            D,
            C,
            B,
            A,
            S,
            S80,
            S84,
            S86
        }

        public enum L2ItemWeaponType
        {
            None,
            Shield,
            Sword,
            Blunt,
            Dagger,
            Pole,
            Fist,
            Dualfist,
            Bow,
            Etc, //book
            Dual,
            Fishingrod,
            Rapier,
            Crossbow,
            Ancientsword,
            Dualdagger,
            Flag,
            Ownthing
        }

        public enum L2ItemBodypart
        {
            None,

            Lhand,

            Back, //cloak

            Hair,
            Alldress, //suit

            Rbracelet, //belt? the same as lbracelet
            Lbracelet, //bracelet
            Deco1, //talisman
            Sigil,
            Waist, //belt

            //pts
            Rhand,
            Lrhand,
            Chest,
            Legs,
            Feet,
            Head,
            Gloves,
            Onepiece,
            Ears,
            Fingers,
            Neck,
            Underwear, //shirt
            Hair2,
            Hairall
        }

        private short _canEquipSex = -1;

        public bool CanEquipSex(int sex)
        {
            if (_canEquipSex == -1)
                return true;

            return sex == _canEquipSex;
        }

        private short _canEquipCastle = -1;

        public bool CanEquipCastle(int castleId)
        {
            if (_canEquipCastle == -1)
                return true;

            return castleId == _canEquipCastle;
        }

        public short CanEquipHero = -1;

        public bool CanEquipHeroic(int heroic)
        {
            if (CanEquipHero == -1)
                return true;

            return heroic == CanEquipHero;
        }

        private short _canEquipNobless = -1;

        public bool CanEquipNobless(int noble)
        {
            if (_canEquipNobless == -1)
                return true;

            return noble == _canEquipNobless;
        }

        private short _canEquipChaotic = -1;
        public string HtmFile;

        public bool CanEquipChaotic(int pkkills)
        {
            if (_canEquipChaotic == -1)
                return true;

            return pkkills == 0;
        }

        public bool IsStackable()
        {
            return StackType == L2ItemConsume.Stackable;
        }

        public short Type1()
        {
            short val = 0;
            switch (Type)
            {
                case L2ItemType.Weapon:
                    switch (WeaponType)
                    {
                        case L2ItemWeaponType.Shield:
                            val = 2;
                            break;

                        default:
                            val = 0;
                            break;
                    }

                    break;
                case L2ItemType.Armor:
                    val = 1;
                    break;
                case L2ItemType.Accessary:
                    val = 3;
                    break;
                case L2ItemType.Questitem:
                    val = 4;
                    break;
            }

            return val;
        }

        public short Type2()
        {
            short val = 0;
            switch (Type)
            {
                case L2ItemType.Weapon:
                    val = 0;
                    break;
                case L2ItemType.Armor:
                    val = 1;
                    break;
                case L2ItemType.Accessary:
                    val = 2;
                    break;
                case L2ItemType.Questitem:
                    val = 3;
                    break;
                case L2ItemType.Asset:
                    val = 4;
                    break;
                case L2ItemType.Etcitem:
                    val = 5;
                    break;
            }

            return val;
        }

        public int BodyPartId()
        {
            int id = 0;
            switch (Bodypart)
            {
                case L2ItemBodypart.Underwear:
                    id = Inventory.Inventory.PaperdollUnder;
                    break;
                case L2ItemBodypart.Ears:
                    id = Inventory.Inventory.PaperdollHairall;
                    break;
                case L2ItemBodypart.Neck:
                    id = 8;
                    break;
                case L2ItemBodypart.Fingers:
                    id = 16 | 32;
                    break;
                case L2ItemBodypart.Head:
                    id = 64;
                    break;
                case L2ItemBodypart.Rhand:
                    id = 128;
                    break;
                case L2ItemBodypart.Lhand:
                    id = 256;
                    break;
                case L2ItemBodypart.Gloves:
                    id = 512;
                    break;
                case L2ItemBodypart.Chest:
                    id = 1024;
                    break;
                case L2ItemBodypart.Legs:
                    id = 2048;
                    break;
                case L2ItemBodypart.Feet:
                    id = 4096;
                    break;
                case L2ItemBodypart.Back:
                    id = Inventory.Inventory.PaperdollBack;
                    break;
                case L2ItemBodypart.Lrhand:
                    id = 16384;
                    break;
                case L2ItemBodypart.Hair:
                    id = Inventory.Inventory.PaperdollHair;
                    break;
                case L2ItemBodypart.Hairall:
                    id = Inventory.Inventory.PaperdollHairall;
                    break;
            }

            return id;
        }

        public bool SetItem = false;
        public List<Effect> Stats = new List<Effect>();
        public string DefaultAction;
        public int ImmediateEffect = 1;
        public string ArmorType;
        public string EtcitemType;
        public int DelayShareGroup = -1;
        public int Blessed = 0;
        public int ExImmediateEffect;
        public int DropPeriod = 10;
        public int ExDropPeriod;
        public int UseSkillDistime;
        public int EquipReuseDelay;
        public int KeepType;
        public int CanMove;
        public int Critical;
        public int ReuseDelay;
        public short Enchanted = 0;
        public int ForNpc;

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

        public void BuildEffect()
        {
            if (PhysicalDamage > 0)
            {
                PPhysicalAttack b = new PPhysicalAttack();
                b.Type = EffectType.PPhysicalDefense;
                b.HashId = ItemId;
                b.Build("p_physical_attack all +" + PhysicalDamage);
                Stats.Add(b);
            }

            if (PhysicalDefense > 0)
            {
                PPhysicalDefence b = new PPhysicalDefence();
                b.Type = EffectType.PPhysicalDefense;
                b.HashId = ItemId;
                b.Build("p_physical_defense all +" + PhysicalDefense);
                Stats.Add(b);
            }

            if (MagicalDamage > 0)
            {
                PMagicalAttack b = new PMagicalAttack();
                b.Type = EffectType.PMagicalAttack;
                b.HashId = ItemId;
                b.Build("p_magical_attack all +" + MagicalDamage);
                Stats.Add(b);
            }

            if (MagicalDefense > 0)
            {
                PMagicalDefence b = new PMagicalDefence();
                b.Type = EffectType.PMagicalDefense;
                b.HashId = ItemId;
                b.Build("p_magical_defense all +" + MagicalDefense);
                Stats.Add(b);
            }

            if (ShieldDefense > 0)
            {
                PPhysicalShieldDefence b = new PPhysicalShieldDefence();
                b.Type = EffectType.PPhysicalShieldDefence;
                b.HashId = ItemId;
                b.Build("p_physical_shield_defence +" + ShieldDefense);
                Stats.Add(b);
            }
        }

        public List<Skill> MultiSkills;

        public void AddMultiSkills(string value)
        {
            if (MultiSkills == null)
                MultiSkills = new List<Skill>();

            foreach (string v in value.Split(';'))
            {
                Skill sk = SkillTable.Instance.Get(int.Parse(v.Split('-')[0]), int.Parse(v.Split('-')[1]));
                if (sk != null)
                    MultiSkills.Add(sk);
            }

            if (MultiSkills.Count == 0)
                MultiSkills = null;
        }

        internal void SetReducingSoulShots(string value) { }

        internal void SetReducingMpConsume(string value) { }

        internal void SetEquipCondition(string value) { }

        internal void SetDamageRange(string value) { }

        internal void SetAttributeAttack(string value) { }

        internal void SetAttributeDefend(string value) { }

        public Skill UnequipSkill;

        public void SetUnequipSkill(string value)
        {
            UnequipSkill = SkillTable.Instance.Get(int.Parse(value.Split('-')[0]), int.Parse(value.Split('-')[1]));
        }

        internal void SetEquipOption(string value) { }

        internal void SetUseCondition(string value) { }

        public Skill ItemSkill;

        public void AddItemSkill(string value)
        {
            ItemSkill = SkillTable.Instance.Get(int.Parse(value.Split('-')[0]), int.Parse(value.Split('-')[1]));
        }

        public Skill ItemSkillEnch4;
        public short AttrDefenseValueUnholy;
        public short AttrDefenseValueHoly;
        public short AttrDefenseValueEarth;
        public short AttrDefenseValueWind;
        public short AttrDefenseValueWater;
        public short AttrDefenseValueFire;
        public short AttrAttackValue;
        public short AttrAttackType = -2;

        public void AddItemEnch4(string value)
        {
            ItemSkillEnch4 = SkillTable.Instance.Get(int.Parse(value.Split('-')[0]), int.Parse(value.Split('-')[1]));
        }
    }
}