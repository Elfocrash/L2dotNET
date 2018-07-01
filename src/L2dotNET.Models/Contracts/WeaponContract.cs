using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using L2dotNET.DataContracts.Shared.Enums;

namespace L2dotNET.DataContracts
{
    [Table("Weapons")]
    public class WeaponContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WeaponId { get; set; }

        public string Name { get; set; }

        public BodyPartType BodyPart { get; set; }

        public bool Crystallizable { get; set; }

        public int Weight { get; set; }

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

        public int Duration { get; set; }

        public int Price { get; set; }

        public CrystalTypeId CrystalType { get; set; }

        public int CrystalCount { get; set; }

        public bool Sellable { get; set; }

        public bool Dropable { get; set; }

        public bool Destroyable { get; set; }

        public bool Tradeable { get; set; }

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
    }
}