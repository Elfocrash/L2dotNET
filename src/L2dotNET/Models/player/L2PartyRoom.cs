using System.Collections.Generic;

namespace L2dotNET.model.player
{
    public class L2PartyRoom
    {
        public int RoomId;
        public int MaxMembers;
        public int MinLevel;
        public int MaxLevel;
        public int LootDist;
        public string Title;
        public uint Location = 1;
        public int LeaderId;

        public List<L2Player> Players = new List<L2Player>();
    }
}