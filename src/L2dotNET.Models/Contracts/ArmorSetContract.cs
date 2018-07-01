using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace L2dotNET.DataContracts
{
    [Table("ArmorSets")]
    public class ArmorSetContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ArmorSetId { get; set; }

        public int? ChestId { get; set; }

        public int? LegsId { get; set; }

        public int? HeadId { get; set; }

        public int? GlovesId { get; set; }

        public int? FeetId { get; set; }

        public int? ShieldId { get; set; }

        public int? SkillId { get; set; }

        public int? ShieldSkillId { get; set; }

        public int? Enchant6SkillId { get; set; }
    }
}
