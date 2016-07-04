using System;
using System.Linq;
using System.Timers;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.World;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Model.Zones.Classes
{
    class poison : L2Zone
    {
        public poison()
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
                if (o is L2Player)
                {
                    if (Template.Target == ZoneTemplate.ZoneTarget.Npc)
                        continue;

                    affect((L2Character)o);
                }
                else if (o is L2Warrior)
                {
                    if ((Template.Target == ZoneTemplate.ZoneTarget.Pc) || (Template.Target == ZoneTemplate.ZoneTarget.OnlyPc))
                        continue;

                    affect((L2Character)o);
                }
        }

        private void affect(L2Character target)
        {
            Random rn = new Random();
            if (Template.Skills != null)
                foreach (Skill sk in Template.Skills.Where(sk => rn.Next(0, 100) <= Template.SkillProb))
                    target.AddAbnormal(sk, target, true, false);

            if (Template.Skill != null)
            {
                if (rn.Next(0, 100) > Template.SkillProb)
                    return;

                target.AddAbnormal(Template.Skill, target, true, false);
            }

            //надо бы как то найти и вынести эту фичу. она недокументирована
            if (Template.Name.StartsWithIgnoreCase("[spa_") && Template.Name.EndsWithIgnoreCase("1]"))
            {
                if (rn.Next(0, 100) > Template.SkillProb)
                    return;

                const int a = 4551,
                          b = 4552,
                          c = 4553,
                          d = 4554;

                int x1 = 0,
                    x2 = 0;
                int id = int.Parse(Template.Name.Substring(5).Replace("]", ""));

                switch (id)
                {
                    case 11:
                        x1 = a;
                        x2 = c;
                        break; //bd
                    case 21:
                        x1 = c;
                        x2 = b;
                        break; //ad
                    case 31:
                        x1 = a;
                        x2 = b;
                        break; //cd
                    case 41:
                        x1 = c;
                        x2 = b;
                        break; //ad
                    case 51:
                        x1 = a;
                        x2 = b;
                        break; //cd
                    case 61:
                        x1 = a;
                        x2 = c;
                        break; //bd
                    case 71:
                        x1 = b;
                        x2 = c;
                        break; //ad
                    case 81:
                        x1 = a;
                        x2 = b;
                        break; //cd
                    case 91:
                        x1 = a;
                        x2 = c;
                        break; //bd
                }

                if (rn.Next(0, 100) <= Template.SkillProb)
                    target.AddAbnormalSpa(x1, false);

                if (rn.Next(0, 100) <= Template.SkillProb)
                    target.AddAbnormalSpa(x2, false);

                if (rn.Next(0, 100) <= Template.SkillProb)
                    target.AddAbnormalSpa(d, false);
            }
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