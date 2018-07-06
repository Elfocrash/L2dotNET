using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using L2dotNET.DataContracts.Shared.Enums;
using Mapster;

namespace L2dotNET.DataContracts
{
    [Table("EtcItems")]
    public class EtcItemContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [AdaptMember("ItemId")]
        public int EtcItemId { get; set; }

        public string Name { get; set; }

        public EtcItemTypeId ItemType { get; set; }

        public int Weight { get; set; }

        public CrystalTypeId CrystalType { get; set; }

        public int Duration { get; set; }

        public int Price { get; set; }

        public int CrystalCount { get; set; }

        public bool Crystallizable { get; set; }

        public bool Sellable { get; set; }

        public bool Dropable { get; set; }

        public bool Destroyable { get; set; }

        public bool Tradeable { get; set; }

        public bool Stackable { get; set; }
    }
}