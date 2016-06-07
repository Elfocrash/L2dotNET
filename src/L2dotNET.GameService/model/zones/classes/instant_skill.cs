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
                foreach (TSkill sk in Template._skills.Where(sk => rn.Next(0, 100) <= Template._skill_prob))
                {
                    target.addAbnormal(sk, null, true, false);
                }
            }

            if (Template._skill != null)
            {
                if (rn.Next(0, 100) > Template._skill_prob)
                    return;

                target.addAbnormal(Template._skill, null, true, false);
            }
        }

        public override void onEnter(L2Object obj)
        {
            if (!_enabled)
                return;

            base.onEnter(obj);

            obj.onEnterZone(this);
        }

        public override void onExit(L2Object obj, bool cls)
        {
            if (!_enabled)
                return;

            base.onExit(obj, cls);

            obj.onExitZone(this, cls);
        }
    }
}