using System.Collections.Generic;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    public class MagicEffectIcons : GameserverPacket
    {
        private readonly List<int[]> _timers = new List<int[]>();

        public void AddIcon(int id, int lvl, int duration)
        {
            _timers.Add(new[] { id, lvl, duration });
        }

        public override void Write()
        {
            WriteByte(0x85);
            WriteShort((short)_timers.Count);

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