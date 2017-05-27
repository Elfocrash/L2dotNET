using System;
using L2dotNET.model.player;
using L2dotNET.model.skills;
using L2dotNET.model.skills2;
using L2dotNET.Network.serverpackets;
using L2dotNET.world;

namespace L2dotNET.model.npcs.cubic
{
    public class CubicTemplate
    {
        public int Id,
                   Level = 1,
                   Duration = 900,
                   Delay,
                   MaxCount,
                   UseUp;
        public byte Slot;
        public double Power;
        public Skill Skill1;
        public byte Skill1Rate;
        public string Skill1Target = "target";
        public Skill Skill2;
        public byte Skill2Rate;
        public string Skill2Target = "target";
        public Skill Skill3;
        public byte Skill3Rate;
        public string Skill3Target = "target";

        public virtual int AiActionTask(L2Player owner)
        {
            return 0;
        }

        private System.Timers.Timer _skillCast;
        private L2Character _target;
        private L2Player _caster;
        private Skill _cast;

        public void CallSkill(L2Player casterPlayer, Skill skill, L2Character targetPlayer)
        {
            _caster = casterPlayer;
            _cast = skill;

            if (_skillCast == null)
                _skillCast = new System.Timers.Timer();

            _target = targetPlayer;
            if (skill.SkillHitTime > 0)
            {
                _skillCast.Interval = skill.SkillHitTime;
                _skillCast.Elapsed += new System.Timers.ElapsedEventHandler(SkillCastTask);
                _skillCast.Enabled = true;
            }
            else
                SkillCastTask(null, null);

            casterPlayer.BroadcastPacket(new MagicSkillUse(casterPlayer, targetPlayer, skill, skill.SkillHitTime));
        }

        private void SkillCastTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            _skillCast.Enabled = false;
            if ((_target == null) || (_caster == null))
                return;

            if (!_cast.ConditionOk(_caster))
                return;

            if (_cast.ReuseDelay > 0)
            {
                if (_caster.Reuse.ContainsKey(_cast.SkillId))
                {
                    TimeSpan ts = _caster.Reuse[_cast.SkillId].StopTime - DateTime.Now;

                    if (ts.TotalMilliseconds > 0)
                    {
                        if (ts.TotalHours > 0)
                        {
                            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S2HoursS3MinutesS4SecondsRemainingInS1ReuseTime);
                            sm.AddSkillName(_cast.SkillId, _cast.Level);
                            sm.AddNumber(ts.Hours);
                            sm.AddNumber(ts.Minutes);
                            sm.AddNumber(ts.Seconds);
                            _caster.SendPacket(sm);
                        }
                        else
                        {
                            if (ts.TotalMinutes > 0)
                            {
                                SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S2MinutesS3SecondsRemainingInS1ReuseTime);
                                sm.AddSkillName(_cast.SkillId, _cast.Level);
                                sm.AddNumber(ts.Minutes);
                                sm.AddNumber(ts.Seconds);
                                _caster.SendPacket(sm);
                            }
                            else
                            {
                                SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S2SecondsRemainingInS1ReuseTime);
                                sm.AddSkillName(_cast.SkillId, _cast.Level);
                                sm.AddNumber(ts.Seconds);
                                _caster.SendPacket(sm);
                            }
                        }

                        return;
                    }
                }
            }

            //do

            if (_cast.ReuseDelay > 0)
            {
                L2SkillCoolTime reuse = new L2SkillCoolTime
                {
                    Id = _cast.SkillId,
                    Lvl = _cast.Level,
                    Total = (int)_cast.ReuseDelay,
                    Owner = _caster
                };
                reuse.Delay = reuse.Total;
                reuse.Timer();
                _caster.Reuse.Add(reuse.Id, reuse);
            }

            _target.AddAbnormal(_cast, _caster, false, false);
        }
    }
}