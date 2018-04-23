﻿using L2dotNET.Tables;

namespace L2dotNET.Models.Zones.Classes
{
    class InstantBuffZone : L2Zone
    {
        public InstantBuffZone(IdFactory idFactory) : base(idFactory)
        {
            ZoneId = idFactory.NextId();
        }

        public override void OnInit()
        {
            Enabled = true;
        }

        private void affect(L2Character target)
        {
            target.SendMessage("u can feel defence.");
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

        public override void OnEnter(L2Object obj)
        {
            if (!Enabled)
                return;

            base.OnEnter(obj);

            obj.OnEnterZone(this);

            if (obj is L2Character)
                ((L2Character)obj).SendMessage("u can feel defence.");
        }

        public override void OnExit(L2Object obj, bool cls)
        {
            if (!Enabled)
                return;

            base.OnExit(obj, cls);

            obj.OnExitZone(this, cls);

            if (obj is L2Character)
                ((L2Character)obj).SendMessage("u lost effect defence.");
        }
    }
}