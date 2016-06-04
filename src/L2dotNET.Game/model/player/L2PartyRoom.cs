using System.Collections.Generic;

namespace L2dotNET.GameService.model.player
{
    public class L2PartyRoom
    {
        public int _roomId;
        public int _maxMembers;
        public int _minLevel;
        public int _maxLevel;
        public int _lootDist;
        public string _title;
        public uint _location = 1;
        public int _leaderId;

        public List<L2Player> _players = new List<L2Player>();
    }
}
