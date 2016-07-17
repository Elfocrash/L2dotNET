using System.Collections.Generic;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PartySpelled : GameServerNetworkPacket
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

        protected internal override void Write()
        {
            if (_character == null)
                return;

            WriteC(0xee);
            WriteD(_summonType);
            WriteD(_id);
            WriteD(_timers.Count);

            foreach (int[] f in _timers)
            {
                WriteD(f[0]); //id
                WriteH((short)f[1]); //lvl

                int duration = f[2];

                if (f[2] == -1)
                    duration = -1;

                if ((f[0] >= 5123) && (f[0] <= 5129))
                    duration = -1;

                WriteD(duration);
            }
        }
    }
}