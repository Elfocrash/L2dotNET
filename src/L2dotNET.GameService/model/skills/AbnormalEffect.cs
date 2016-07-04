using System;
using System.Timers;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills
{
    public class AbnormalEffect
    {
        public int time;
        public int lvl;
        public int id;

        public int active;

        public Timer _timer;
        public L2Character _owner;

        public DateTime stopTime;
        public TSkill skill;

        public void forcedStop(bool msg, bool icon)
        {
            active = 0;
            if ((_timer != null) && _timer.Enabled)
            {
                _timer.Stop();
                _timer.Enabled = false;
            }

            _owner.OnAveEnd(this, msg, icon, null);
        }

        public int getTime()
        {
            if (time == -2) //unlimit buff time
                return -1;

            long elapsedTicks = stopTime.Ticks - DateTime.Now.Ticks;
            int res = (int)(elapsedTicks * 1E-7);
            return res;
        }

        public void timer()
        {
            if (time == -2)
                return;

            stopTime = DateTime.Now.AddSeconds(time);
            _timer = new Timer(time * 1000);
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            active = 0;
            _owner.OnAveEnd(this, true, true, null);
        }
    }
}