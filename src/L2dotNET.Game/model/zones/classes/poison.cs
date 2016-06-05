using System;
using System.Timers;
using L2dotNET.GameService.model.npcs;
using L2dotNET.GameService.model.skills2;
using L2dotNET.GameService.tables;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.model.zones.classes
{
    class poison : L2Zone
    {
        public poison()
        {
            ZoneID = IdFactory.Instance.nextId();
        }

        public override void onInit()
        {
            _enabled = Template.DefaultStatus;

            if (_enabled && Template._unit_tick > 0)
                startTimer();
        }

        public override void onTimerAction(object sender, ElapsedEventArgs e)
        {
            if (ObjectsInside.Count == 0)
                return;

            foreach (L2Object o in ObjectsInside.Values)
            {
                if (o is L2Player)
                {
                    if (Template._target == ZoneTemplate.ZoneTarget.npc)
                        continue;

                    affect((L2Character)o);
                }
                else if (o is L2Warrior)
                {
                    if (Template._target == ZoneTemplate.ZoneTarget.pc || Template._target == ZoneTemplate.ZoneTarget.only_pc)
                        continue;

                    affect((L2Character)o);
                }
            }
        }

        private void affect(L2Character target)
        {
            Random rn = new Random();
            if (Template._skills != null)
            {
                foreach (TSkill sk in Template._skills)
                {
                    if (rn.Next(0, 100) > Template._skill_prob)
                        continue;

                    target.addAbnormal(sk, target, true, false);
                }
            }

            if (Template._skill != null)
            {
                if (rn.Next(0, 100) > Template._skill_prob)
                    return;

                target.addAbnormal(Template._skill, target, true, false);
            }

            //надо бы как то найти и вынести эту фичу. она недокументирована
            if (Template.Name.StartsWith("[spa_") && Template.Name.EndsWith("1]"))
            {
                if (rn.Next(0, 100) > Template._skill_prob)
                    return;

                int a = 4551, b = 4552, c = 4553, d = 4554, x1 = 0, x2 = 0;
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

                if (rn.Next(0, 100) <= Template._skill_prob)
                    target.addAbnormalSPA(x1, false);

                if (rn.Next(0, 100) <= Template._skill_prob)
                    target.addAbnormalSPA(x2, false);

                if (rn.Next(0, 100) <= Template._skill_prob)
                    target.addAbnormalSPA(d, false);
            }
        }

        public override void onEnter(world.L2Object obj)
        {
            if (!_enabled)
                return;

            base.onEnter(obj);

            obj.onEnterZone(this);
        }

        public override void onExit(world.L2Object obj, bool cls)
        {
            if (!_enabled)
                return;

            base.onExit(obj, cls);

            obj.onExitZone(this, cls);
        }
    }
}