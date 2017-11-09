namespace L2dotNET.DataContracts
{
    public class SpawnlistContract
    {
        public int TemplateId { get; set; }

        public int LocX { get; set; }

        public int LocY { get; set; }

        public int LocZ { get; set; }

        public int Heading { get; set; }

        public int RespawnDelay { get; set; }

        public int RespawnRand { get; set; }

        public int PerdiodOfDay { get; set; }
    }
}