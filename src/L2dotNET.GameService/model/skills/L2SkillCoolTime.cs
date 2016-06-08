using System;
using System.Timers;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills
{
    public class L2SkillCoolTime
    {
        public int id,
                   lvl,
                   total,
                   delay;

        private Timer _timer;
        public L2Character _owner;
        public DateTime stopTime;

        public void forcedStop()
        {
            _timer.Stop();
        }

        public int getDelay()
        {
            TimeSpan ts = stopTime - DateTime.Now;
            return (int)ts.TotalSeconds;
        }

        public void timer()
        {
            if (delay == 0)
                return;

            stopTime = DateTime.Now.AddSeconds(delay);
            _timer = new Timer(delay * 1000);
            _timer.Elapsed += new ElapsedEventHandler(actionTime);
            _timer.Enabled = true;
        }

        private void actionTime(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;
            lock (_owner._reuse)
            {
                _owner._reuse.Remove(id);
            }
        }
    }
}