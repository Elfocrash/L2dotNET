using System.Collections.Generic;
using L2dotNET.Models;
using L2dotNET.World;

namespace L2dotNET.Network.serverpackets
{
    class PartySpelled : GameserverPacket
    {
        public PartySpelled(L2Character character)
        {
            _id = character.ObjId;
            _summonType = character.ObjectSummonType;
            _character = character;
        }

        private readonly List<int[]> _timers = new List<int[]>();
        private readonly int _id;
        private readonly L2Character _character;
        private readonly int _summonType;

        public void AddIcon(int iconId, int lvl, int duration)
        {
            _timers.Add(new[] { iconId, lvl, duration });
        }

        public override void Write()
        {
            if (_character == null)
                return;

            WriteByte(0xee);
            WriteInt(_summonType);
            WriteInt(_id);
            WriteInt(_timers.Count);

            foreach (int[] f in _timers)
            {
                WriteInt(f[0]); //id
                WriteShort((short)f[1]); //lvl

                int duration = f[2];

                if (f[2] == -1)
                    duration = -1;

                if ((f[0] >= 5123) && (f[0] <= 5129))
                    duration = -1;

                WriteInt(duration);
            }
        }
    }
}