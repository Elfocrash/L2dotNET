using System.Timers;

namespace L2dotNET.GameService.Model.Player.Transformation
{
    public class L2Transform
    {
        public TransformTemplate Template;
        public Timer _timer;
        public L2Player owner;

        public L2Transform(TransformTemplate tempalte)
        {
            Template = tempalte;
        }

        public void timer(int seconds)
        {
            _timer = new Timer(seconds * 1000);
            _timer.Elapsed += new ElapsedEventHandler(actionTimeEnd);
            _timer.Enabled = true;
        }

        private void actionTimeEnd(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;
            owner.untransform();
        }
    }
}