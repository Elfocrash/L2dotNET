using System.Timers;

namespace L2dotNET.model.player.transformation
{
    public class L2Transform
    {
        public TransformTemplate Template;
        public Timer MTimer;
        public L2Player Owner;

        public L2Transform(TransformTemplate tempalte)
        {
            Template = tempalte;
        }

        public void Timer(int seconds)
        {
            MTimer = new Timer(seconds * 1000);
            MTimer.Elapsed += ActionTimeEnd;
            MTimer.Enabled = true;
        }

        private void ActionTimeEnd(object sender, ElapsedEventArgs e)
        {
            MTimer.Enabled = false;
            Owner.Untransform();
        }
    }
}