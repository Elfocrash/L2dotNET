using System.Collections.Generic;
using L2dotNET.Game.world;

namespace L2dotNET.Game.network.l2send
{
    class PartySpelled : GameServerNetworkPacket
    {
        public PartySpelled(L2Character character)
        {
            this.id = character.ObjID;
            this.summonType = character.ObjectSummonType;
            this.character = character;
        }

        List<int[]> _timers = new List<int[]>();
        private int id;
        private L2Character character;
        private int summonType;
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
