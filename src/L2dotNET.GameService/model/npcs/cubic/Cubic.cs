using System;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Npcs.Cubic
{
    public class Cubic
    {
        public CubicTemplate template;
        public int current_count;
        public L2Player owner;

        public Cubic(L2Player player, CubicTemplate t)
        {
            owner = player;
            template = t;
        }

        public DateTime SummonedTime,
                        SummonEndTime;
        public System.Timers.Timer AiAction,
                                   SummonEnd;

        public virtual void OnSummon()
        {
            SummonedTime = DateTime.Now;
            SummonEndTime = DateTime.Now.AddSeconds(template.duration);
            AiAction = new System.Timers.Timer();
            AiAction.Interval = template.delay * 1000;
            AiAction.Elapsed += new System.Timers.ElapsedEventHandler(AiActionTask);

            SummonEnd = new System.Timers.Timer();
            SummonEnd.Interval = template.duration * 1000;
            SummonEnd.Elapsed += new System.Timers.ElapsedEventHandler(SummonEndTask);

            AiAction.Enabled = true;
            SummonEnd.Enabled = true;

            owner.SendMessage("Summoned cubic #" + template.id + " for " + (template.duration / 60) + " min.");
        }

        public void AiActionTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            current_count += template.AiActionTask(owner);
            if (current_count > template.max_count)
                OnEnd(true);
        }

        public void OnEnd(bool inheritOwner)
        {
            if (AiAction.Enabled)
                AiAction.Enabled = false;
            if (SummonEnd.Enabled)
                SummonEnd.Enabled = false;

            if (inheritOwner)
                owner.StopCubic(this);
        }

        public void SummonEndTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            OnEnd(true);
        }
    }
}