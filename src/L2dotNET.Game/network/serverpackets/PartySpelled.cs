using System.Collections.Generic;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.network.serverpackets
{
    class PartySpelled : GameServerNetworkPacket
    {
        public PartySpelled(L2Character character)
        {
            this.id = character.ObjID;
            this.summonType = character.ObjectSummonType;
            this.character = character;
        }

        private readonly List<int[]> _timers = new List<int[]>();
        private readonly int id;
        private readonly L2Character character;
        private readonly int summonType;

        public void addIcon(int id, int lvl, int duration)
        {
            _timers.Add(new int[] { id, lvl, duration });
        }

        protected internal override void write()
        {
            if (character == null)
                return;

            writeC(0xee);
            writeD(summonType);
            writeD(id);
            writeD(_timers.Count);

            foreach (int[] f in _timers)
            {
                writeD(f[0]); //id
                writeH((short)f[1]); //lvl

                int duration = f[2];

                if (f[2] == -1)
                    duration = -1;

                if (f[0] >= 5123 && f[0] <= 5129)
                    duration = -1;

                writeD(duration);
            }
        }
    }
}