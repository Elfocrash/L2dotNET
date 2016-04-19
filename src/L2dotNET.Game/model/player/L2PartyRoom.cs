using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L2dotNET.Game.model.player
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
