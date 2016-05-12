using L2dotNET.GameService.world;
using log4net;
using System;

namespace L2dotNET.GameService.model.playable.petai
{
    public class StandartAiTemplate
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StandartAiTemplate));

        public L2Character character;
        public System.Timers.Timer ai1sec, follow;
        private bool FollowStatus = false;

        public void Enable()
        {
            ai1sec = new System.Timers.Timer();
            ai1sec.Interval = 1000;
            ai1sec.Elapsed += new System.Timers.ElapsedEventHandler(DoThink);
            ai1sec.Enabled = true;
        }

        public void Disable()
        {
            if(ai1sec != null)
                ai1sec.Enabled = false;

            if (follow != null)
                follow.Enabled = false;
        }

        public void SetFollowStatus(bool value)
        {
            if (follow == null)
            {
                follow = new System.Timers.Timer();
                follow.Interval = 200;
                follow.Elapsed += new System.Timers.ElapsedEventHandler(DoFollow);
            }

            follow.Enabled = value;
            FollowStatus = value;
        }

        public void ChangeFollowStatus()
        {
            SetFollowStatus(!FollowStatus);
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
            log.Info("NotifyTargetNull");
        }

        public virtual void NotifyTargetDead()
        {
            log.Info("NotifyTargetDead");
        }

        public virtual void NotifyMpEnd(L2Character target)
        {
            log.Info("NotifyMpEnd");
        }

        public virtual void NotifyEvaded(L2Character target)
        {
            log.Info("NotifyEvaded");
        }

        public virtual void Attack(L2Character target) { }
        public virtual void StopAutoAttack() { }
    }
}
