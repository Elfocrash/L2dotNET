using log4net;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Playable.PetAI
{
    public class StandartAiTemplate
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(StandartAiTemplate));

        public L2Character Character;
        public System.Timers.Timer Ai1Sec,
                                   Follow;
        private bool _followStatus;

        public void Enable()
        {
            Ai1Sec = new System.Timers.Timer
                     {
                         Interval = 1000
                     };
            Ai1Sec.Elapsed += new System.Timers.ElapsedEventHandler(DoThink);
            Ai1Sec.Enabled = true;
        }

        public void Disable()
        {
            if (Ai1Sec != null)
                Ai1Sec.Enabled = false;

            if (Follow != null)
                Follow.Enabled = false;
        }

        public void SetFollowStatus(bool value)
        {
            if (Follow == null)
            {
                Follow = new System.Timers.Timer
                         {
                             Interval = 200
                         };
                Follow.Elapsed += new System.Timers.ElapsedEventHandler(DoFollow);
            }

            Follow.Enabled = value;
            _followStatus = value;
        }

        public void ChangeFollowStatus()
        {
            SetFollowStatus(!_followStatus);
        }

        public virtual void DoThink(object sender = null, System.Timers.ElapsedEventArgs e = null) { }

        public virtual void DoFollow(object sender = null, System.Timers.ElapsedEventArgs e = null) { }

        public virtual void NotifyOnHit(L2Character attacker, double damage) { }

        public virtual void NotifyStartMoving() { }

        public virtual void NotifyStopMoving() { }

        public virtual void NotifyOnKill(L2Character target) { }

        public virtual void NotifyOnDie(L2Character killer)
        {
            SetFollowStatus(false);
        }

        public virtual void NotifyOnStartNight() { }

        public virtual void NotifyOnStartDay() { }

        public virtual void NotifyTargetNull()
        {
            Log.Info("NotifyTargetNull");
        }

        public virtual void NotifyTargetDead()
        {
            Log.Info("NotifyTargetDead");
        }

        public virtual void NotifyMpEnd(L2Character target)
        {
            Log.Info("NotifyMpEnd");
        }

        public virtual void NotifyEvaded(L2Character target)
        {
            Log.Info("NotifyEvaded");
        }

        public virtual void Attack(L2Character target) { }

        public virtual void StopAutoAttack() { }
    }
}