using L2dotNET.Models.Zones;

namespace L2dotNET.Models.Npcs
{
    public class SpawnLocation : Location
    {
        int _respawnDelay;
        public int Heading { get; set; }
        public int RespawnDelay {
            get => _respawnDelay;
            set => _respawnDelay = value * 1000;
        }

        public SpawnLocation(int x, int y, int z,int heading, int respawnDelay) : base(x, y, z)
        {
            Heading = heading;
            RespawnDelay = respawnDelay;
        }

        public SpawnLocation(SpawnLocation loc) : base(loc)
        {
            Heading = loc.Heading;
        }

        public override bool Equals(object obj)
        {
            SpawnLocation loc = obj as SpawnLocation;
            return (loc?.X == X && loc?.Y == Y && loc?.Z == Z && loc.Heading == Heading);
        }
    }
}