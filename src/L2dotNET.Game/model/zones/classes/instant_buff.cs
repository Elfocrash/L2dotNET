using L2dotNET.GameService.tables;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.model.zones.classes
{
    class instant_buff : L2Zone
    {
        public instant_buff()
        {
            ZoneID = IdFactory.Instance.nextId();
        }

        public override void onInit()
        {
            _enabled = true;
        }

        private void affect(L2Character target)
        {
            target.sendMessage("u can feel defence.");
            //Random rn = new Random();
            //if (Zone._skills != null)
            //{
            //    foreach (L2Skill sk in Zone._skills)
            //    {
            //        if (rn.Next(0, 100) > Zone._skill_prob)
            //            continue;

            //        target.addAbnormal(sk, null, true, false);
            //    }
            //}

            //if (Zone._skill != null)
            //{
            //    if (rn.Next(0, 100) > Zone._skill_prob)
            //        return;

            //    target.addAbnormal(Zone._skill, null, true, false);
            //}
        }

        public override void onEnter(world.L2Object obj)
        {
            if (!_enabled)
                return;

            base.onEnter(obj);

            obj.onEnterZone(this);

            if (obj is L2Character)
            {
                ((L2Character)obj).sendMessage("u can feel defence.");
            }
        }

        public override void onExit(world.L2Object obj, bool cls)
        {
            if (!_enabled)
                return;

            base.onExit(obj, cls);

            obj.onExitZone(this, cls);


            if (obj is L2Character)
            {
                ((L2Character)obj).sendMessage("u lost effect defence.");
            }
        }
    }
}
