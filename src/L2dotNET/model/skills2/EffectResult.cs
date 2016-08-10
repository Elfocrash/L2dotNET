using System.Collections.Generic;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.model.skills2
{
    public class EffectResult
    {
        public byte TotalUi;
        public SortedList<int, double> Sus = new SortedList<int, double>();
        public byte HpMpCp;

        public void AddSu(int stat, double value)
        {
            lock (Sus)
            {
                if (Sus.ContainsKey(stat))
                    Sus.Remove(stat);

                Sus.Add(stat, value);
            }

            if (((HpMpCp == 0) && (stat == StatusUpdate.MaxHp)) || (stat == StatusUpdate.MaxMp) || (stat == StatusUpdate.MaxCp) || (stat == StatusUpdate.CurHp) || (stat == StatusUpdate.CurMp) || (stat == StatusUpdate.CurCp))
                HpMpCp = 1;
        }

        public void AddAll(SortedList<int, double> newlist)
        {
            foreach (int stat in newlist.Keys)
                AddSu(stat, newlist[stat]);
        }

        public EffectResult AsTotalUi()
        {
            TotalUi = 1;
            return this;
        }
    }
}