using System;
using System.Timers;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills
{
    public class AbnormalEffect
    {
        public int Time;
        public int Lvl;
        public int Id;

        public int Active;

        public Timer MTimer;
        public L2Character Owner;

        public DateTime StopTime;
        public Skill Skill;

        public void ForcedStop(bool msg, bool icon)
        {
            Active = 0;
            if ((MTimer != null) && MTimer.Enabled)
            {
                MTimer.Stop();
                MTimer.Enabled = false;
            }

            Owner.OnAveEnd(this, msg, icon, null);
        }

        public int GetTime()
        {
            if (Time == -2) //unlimit buff time
                return -1;

            long elapsedTicks = StopTime.Ticks - DateTime.Now.Ticks;
            int res = (int)(elapsedTicks * 1E-7);
            return res;
        }

        public void Timer()
        {
            if (Time == -2)
                return;

            StopTime = DateTime.Now.AddSeconds(Time);
            MTimer = new Timer(Time * 1000);
            MTimer.Elapsed += Timer_Elapsed;
            MTimer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MTimer.Stop();
            Active = 0;
            Owner.OnAveEnd(this, true, true, null);
        }
    }
}