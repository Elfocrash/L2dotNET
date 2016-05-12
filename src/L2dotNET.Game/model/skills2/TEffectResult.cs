using System.Collections.Generic;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.model.skills2
{
    public class TEffectResult
    {
        public byte TotalUI;
        public SortedList<int, double> sus;
        public byte HpMpCp = 0;

        public void addSU(int stat, double value)
        {
            if (sus == null)
                sus = new SortedList<int, double>();

            if (sus.ContainsKey(stat))
                lock (sus)
                    sus.Remove(stat);

            sus.Add(stat, value);

            if (HpMpCp == 0 && stat == StatusUpdate.MAX_HP || stat == StatusUpdate.MAX_MP || stat == StatusUpdate.MAX_CP || stat == StatusUpdate.CUR_HP || stat == StatusUpdate.CUR_MP || stat == StatusUpdate.CUR_CP)
                HpMpCp = 1;
        }

        public void addAll(SortedList<int, double> newlist)
        {
            foreach (int stat in newlist.Keys)
            {
                addSU(stat, newlist[stat]);
            }
        }

        public TEffectResult AsTotalUI()
        {
            this.TotalUI = 1;
            return this;
        }
    }
}
