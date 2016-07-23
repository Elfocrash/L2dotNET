using System;
using System.Linq;
using System.Timers;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Zones.Classes
{
    class instant_skill : L2Zone
    {
        public instant_skill()
        {
            ZoneId = IdFactory.Instance.NextId();
        }

        public override void OnInit()
        {
            Enabled = Template.DefaultStatus;

            if (Enabled && (Template.UnitTick > 0))
                StartTimer();
        }

        public override void OnTimerAction(object sender, ElapsedEventArgs e)
        {
            if (ObjectsInside.Count == 0)
                return;

            foreach (L2Object o in ObjectsInside.Values)
            {
                if (o is L2Player)
                {
                    if (Template.Target == ZoneTemplate.ZoneTarget.Npc)
                        continue;

                    affect((L2Character)o);
                }
                else
                {
                    if (!(o is L2Warrior))
                        continue;

                    if ((Template.Target == ZoneTemplate.ZoneTarget.Pc) || (Template.Target == ZoneTemplate.ZoneTarget.OnlyPc))
                        continue;

                    affect((L2Character)o);
                }
            }
        }

        private void affect(L2Character target)
        {
            Random rn = new Random();
            if (Template.Skills != null)
            {
                foreach (Skill sk in Template.Skills.Where(sk => rn.Next(0, 100) <= Template.SkillProb))
                    target.AddAbnormal(sk, null, true, false);
            }

            if (Template.Skill == null)
                return;

            if (rn.Next(0, 100) > Template.SkillProb)
                return;

            target.AddAbnormal(Template.Skill, null, true, false);
        }

        public override void OnEnter(L2Object obj)
        {
            if (!Enabled)
                return;

            base.OnEnter(obj);

            obj.OnEnterZone(this);
        }

        public override void OnExit(L2Object obj, bool cls)
        {
            if (!Enabled)
                return;

            base.OnExit(obj, cls);

            obj.OnExitZone(this, cls);
        }
    }
}