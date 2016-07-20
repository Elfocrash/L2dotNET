using System;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Npcs.Cubic
{
    public class Cubic
    {
        public CubicTemplate Template;
        public int CurrentCount;
        public L2Player Owner;

        public Cubic(L2Player player, CubicTemplate t)
        {
            Owner = player;
            Template = t;
        }

        public DateTime SummonedTime,
                        SummonEndTime;
        public System.Timers.Timer AiAction,
                                   SummonEnd;

        public virtual void OnSummon()
        {
            SummonedTime = DateTime.Now;
            SummonEndTime = DateTime.Now.AddSeconds(Template.Duration);
            AiAction = new System.Timers.Timer
                       {
                           Interval = Template.Delay * 1000
                       };
            AiAction.Elapsed += new System.Timers.ElapsedEventHandler(AiActionTask);

            SummonEnd = new System.Timers.Timer
                        {
                            Interval = Template.Duration * 1000
                        };
            SummonEnd.Elapsed += new System.Timers.ElapsedEventHandler(SummonEndTask);

            AiAction.Enabled = true;
            SummonEnd.Enabled = true;

            Owner.SendMessage($"Summoned cubic #{Template.Id} for {Template.Duration / 60} min.");
        }

        public void AiActionTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            CurrentCount += Template.AiActionTask(Owner);
            if (CurrentCount > Template.MaxCount)
                OnEnd(true);
        }

        public void OnEnd(bool inheritOwner)
        {
            if (AiAction.Enabled)
                AiAction.Enabled = false;
            if (SummonEnd.Enabled)
                SummonEnd.Enabled = false;

            if (inheritOwner)
                Owner.StopCubic(this);
        }

        public void SummonEndTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            OnEnd(true);
        }
    }
}