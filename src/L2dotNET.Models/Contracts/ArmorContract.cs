using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using L2dotNET.DataContracts.Shared.Enums;

namespace L2dotNET.DataContracts
{
    [Table("Armors")]
    public class ArmorContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ArmorId { get; set; }

        public string Name { get; set; }

        public BodyPartType BodyPart { get; set; }

        public bool Crystallizable { get; set; }

        public ArmorTypeId ArmorType { get; set; }

        public int Weight { get; set; }

        public bool AvoidModify { get; set; }

        public int Duration { get; set; }

        public int Pdef { get; set; }

        public int Mdef { get; set; }

        public int MpBonus { get; set; }

        public int Price { get; set; }

        public int CrystalCount { get; set; }

        public bool Sellable { get; set; }

        public bool Dropable { get; set; }

        public bool Destroyable { get; set; }

        public bool Tradeable { get; set; }

        public int ItemSkillId { get; set; }

        public byte ItemSkillLvl { get; set; }
    }
}