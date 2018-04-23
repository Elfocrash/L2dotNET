﻿using System.Timers;
using L2dotNET.Models.Npcs;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Tables;

namespace L2dotNET.Models.Zones.Classes
{
    class DamageZone : L2Zone
    {
        public DamageZone(IdFactory idFactory) : base(idFactory)
        {
            ZoneId = idFactory.NextId();
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

                    ((L2Player)o).ReduceHpArea(Template.DamageOnHp, Template.MessageNo);
                }
                else
                {
                    if (!(o is L2Warrior))
                        continue;

                    if (Template.Target == ZoneTemplate.ZoneTarget.Pc)
                        continue;

                    ((L2Warrior)o).ReduceHpArea(Template.DamageOnHp, Template.MessageNo);
                }
            }
        }

        public override void OnEnter(L2Object obj)
        {
            if (!Enabled)
                return;

            base.OnEnter(obj);

            obj.OnEnterZone(this);

            if (!(obj is L2Player))
                return;

            L2Player p = (L2Player)obj;
            p.IsInDanger = true;
            p.SendPacket(new EtcStatusUpdate(p));
        }

        public override void OnExit(L2Object obj, bool cls)
        {
            if (!Enabled)
                return;

            base.OnExit(obj, cls);

            obj.OnExitZone(this, cls);

            if (!(obj is L2Player))
                return;

            L2Player p = (L2Player)obj;
            p.IsInDanger = false;
            p.SendPacket(new EtcStatusUpdate(p));
        }
    }
}