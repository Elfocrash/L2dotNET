using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace L2dotNET.DataContracts
{
    [Table("CharacterSkills")]
    public class CharacterSkillContract
    {
        [Key]
        public int CharacterSkillId { get; set; }
        public int CharacterId { get; set; }
        public int SkillId { get; set; }
        public byte SkillLvl { get; set; }
        public int ClassId { get; set; }
    }
}
