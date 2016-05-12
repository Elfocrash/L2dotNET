using System.Collections.Generic;

namespace L2dotNET.GameService.network.l2send
{
    public class MagicEffectIcons : GameServerNetworkPacket
    {
        List<int[]> _timers = new List<int[]>();
        public void addIcon(int id, int lvl, int duration)
        {
            _timers.Add(new int[] {id,lvl,duration});
        }

        protected internal override void write()
        {
            writeC(0x85);
            writeH((short)_timers.Count);

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
