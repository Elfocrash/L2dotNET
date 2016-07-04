using System.Collections.Generic;

namespace L2dotNET.GameService.Network.Serverpackets
{
    public class MagicEffectIcons : GameServerNetworkPacket
    {
        private readonly List<int[]> _timers = new List<int[]>();

        public void AddIcon(int id, int lvl, int duration)
        {
            _timers.Add(new[] { id, lvl, duration });
        }

        protected internal override void Write()
        {
            WriteC(0x85);
            WriteH((short)_timers.Count);

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