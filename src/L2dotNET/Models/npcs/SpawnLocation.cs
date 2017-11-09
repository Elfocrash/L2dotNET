using L2dotNET.DataContracts;
using L2dotNET.Models;
using L2dotNET.Models.zones;

namespace L2dotNET.model.npcs
{
    public class SpawnLocation : Location
    {
        public int Heading { get; set; }

        public SpawnLocation(int x, int y, int z,int heading) : base(x, y, z)
        {
            Heading = heading;
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