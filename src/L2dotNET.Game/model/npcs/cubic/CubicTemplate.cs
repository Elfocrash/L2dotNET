using System;
using L2dotNET.GameService.model.skills2;
using L2dotNET.GameService.world;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.model.npcs.cubic
{
    public class CubicTemplate
    {
        public int id, level = 1, duration = 900, delay, max_count, use_up;
        public byte slot;
        public double power;
        public TSkill skill1;
        public byte skill1rate;
        public string skill1target = "target";
        public TSkill skill2;
        public byte skill2rate;
        public string skill2target = "target";
        public TSkill skill3;
        public byte skill3rate;
        public string skill3target = "target";

        public virtual int AiActionTask(L2Player owner)
        {
            return 0;
        }

        private System.Timers.Timer SkillCast;
        private L2Character target;
        private L2Player caster;
        private TSkill cast;
        public void CallSkill(L2Player caster, TSkill skill, L2Character target)
        {
            if (SkillCast == null)
                SkillCast = new System.Timers.Timer();

            this.target = target;
            if (skill.skill_hit_time > 0)
            {
                SkillCast.Interval = skill.skill_hit_time;
                SkillCast.Elapsed += new System.Timers.ElapsedEventHandler(SkillCastTask);
                SkillCast.Enabled = true;
            }
            else
                SkillCastTask(null, null);

            caster.broadcastPacket(new MagicSkillUse(caster, target, skill, skill.skill_hit_time));
        }

        private void SkillCastTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            SkillCast.Enabled = false;
            if (target == null || caster == null)
                return;

            if (!cast.ConditionOk(caster))
                return;

            if (cast.reuse_delay > 0)
            {
                if (caster._reuse.ContainsKey(cast.skill_id))
                {
                    TimeSpan ts = caster._reuse[cast.skill_id].stopTime - DateTime.Now;

                    if (ts.TotalMilliseconds > 0)
                    {
                        if (ts.TotalHours > 0)
                        {
                            //There are $s2 hour(s), $s3 minute(s), and $s4 second(s) remaining in $s1's re-use time.
                            SystemMessage sm = new SystemMessage(2305);
                            sm.AddSkillName(cast.skill_id, cast.level);
                            sm.AddNumber((int)ts.Hours);
                            sm.AddNumber((int)ts.Minutes);
                            sm.AddNumber((int)ts.Seconds);
                            caster.sendPacket(sm);
                        }
                        else if (ts.TotalMinutes > 0)
                        {
                            //There are $s2 minute(s), $s3 second(s) remaining in $s1's re-use time.
                            SystemMessage sm = new SystemMessage(2304);
                            sm.AddSkillName(cast.skill_id, cast.level);
                            sm.AddNumber((int)ts.Minutes);
                            sm.AddNumber((int)ts.Seconds);
                            caster.sendPacket(sm);
                        }
                        else
                        {
                            //There are $s2 second(s) remaining in $s1's re-use time.
                            SystemMessage sm = new SystemMessage(2303);
                            sm.AddSkillName(cast.skill_id, cast.level);
                            sm.AddNumber((int)ts.Seconds);
                            caster.sendPacket(sm);
                        }

                        return;
                    }
                }
            }




            //do

            if (cast.reuse_delay > 0)
            {
                L2SkillCoolTime reuse = new L2SkillCoolTime();
                reuse.id = cast.skill_id;
                reuse.lvl = cast.level;
                reuse.total = (int)cast.reuse_delay;
                reuse.delay = reuse.total;
                reuse._owner = caster;
                reuse.timer();
                caster._reuse.Add(reuse.id, reuse);
            }

            target.addAbnormal(cast, caster, false, false);
        }
    }
}
