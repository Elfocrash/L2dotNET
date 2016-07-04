using System.Collections.Generic;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2
{
    public class EffectResult
    {
        public byte TotalUi;
        public SortedList<int, double> Sus;
        public byte HpMpCp;

        public void AddSu(int stat, double value)
        {
            if (Sus == null)
                Sus = new SortedList<int, double>();

            if (Sus.ContainsKey(stat))
                lock (Sus)
                {
                    Sus.Remove(stat);
                }

            Sus.Add(stat, value);

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