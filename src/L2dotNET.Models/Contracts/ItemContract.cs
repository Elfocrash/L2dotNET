using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using L2dotNET.DataContracts.Shared.Enums;

namespace L2dotNET.DataContracts
{
    [Table("Items")]
    public class ItemContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemId { get; set; }

        public int CharacterId { get; set; }

        public int ObjectId { get; set; }

        public int Count { get; set; }

        public int Enchant { get; set; }

        public ItemLocation Location { get; set; }

        public int LocationData { get; set; }

        public int? TimeOfUse { get; set; }

        public int CustomType1 { get; set; }

        public int CustomType2 { get; set; }

        public int ManaLeft { get; set; }

        public int Time { get; set; }
    }
}