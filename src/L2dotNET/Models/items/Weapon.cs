using System.Linq;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Enums;

namespace L2dotNET.Models.Items
{
    public class Weapon : ItemTemplate
    {
        public int Soulshots { get; set; }
        public int Spiritshots { get; set; }
        public int Pdam { get; set; }
        public int RndDam { get; set; }
        public WeaponTypeId WeaponType { get; set; }
        public int Critical { get; set; }
        public int HitModify { get; set; }
        public int AvoidModify { get; set; }
        public int ShieldDef { get; set; }
        public int ShieldDefRate { get; set; }
        public int AtkSpeed { get; set; }
        public int MpConsume { get; set; }
        public int Mdam { get; set; }
        public int ItemSkillId { get; set; }
        public int ItemSkillLvl { get; set; }
        public int Enchant4SkillId { get; set; }
        public int Enchant4SkillLvl { get; set; }
        public int OnCastSkillId { get; set; }
        public int OnCastSkillLvl { get; set; }
        public int OnCastSkillChance { get; set; }
        public int OnCritSkillId { get; set; }
        public int OnCritSkillLvl { get; set; }
        public int OnCritSkillChance { get; set; }

        public override int GetItemMask()
        {
            WeaponType orDefault = Enums.WeaponType.Values.FirstOrDefault(x => x.Id == WeaponType);

            return orDefault?.GetMask() ?? 0;
        }

    }
}