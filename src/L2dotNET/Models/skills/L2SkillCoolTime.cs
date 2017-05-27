using System;
using System.Timers;
using L2dotNET.world;

namespace L2dotNET.model.skills
{
    public class L2SkillCoolTime
    {
        public int Id,
                   Lvl,
                   Total,
                   Delay;

        private Timer _timer;
        public L2Character Owner;
        public DateTime StopTime;

        public void ForcedStop()
        {
            _timer.Stop();
        }

        public int GetDelay()
        {
            TimeSpan ts = StopTime - DateTime.Now;
            return (int)ts.TotalSeconds;
        }

        public void Timer()
        {
            if (Delay == 0)
                return;

            StopTime = DateTime.Now.AddSeconds(Delay);
            _timer = new Timer(Delay * 1000);
            _timer.Elapsed += ActionTime;
            _timer.Enabled = true;
        }

        private void ActionTime(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;
            lock (Owner.Reuse)
                Owner.Reuse.Remove(Id);
        }
    }
}