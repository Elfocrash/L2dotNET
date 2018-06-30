using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace L2dotNET.DataContracts
{
    [Table("Spawnlist")]
    public class SpawnlistContract
    {
        [Key]
        public int SpawnId { get; set; }

        public int TemplateId { get; set; }

        public int LocX { get; set; }

        public int LocY { get; set; }

        public int LocZ { get; set; }

        public int Heading { get; set; }

        public int RespawnDelay { get; set; }

        public int RespawnRand { get; set; }

        public byte PeriodOfDay { get; set; }
    }
}