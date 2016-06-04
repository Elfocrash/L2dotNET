using System;
using L2dotNET.GameService.model.structures;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.model.npcs.decor
{
    public class L2Door : L2StaticObject
    {
        public HideoutTemplate structure;
        public L2Door()
        {
            ObjID = IdFactory.Instance.nextId();
            Type = 1;
            Closed = 1;
            MeshID = 1;
            Level = 1;
        }

        public override void onSpawn()
        {
            CurHP = MaxHP;
            base.onSpawn();
        }

        public override void NotifyAction(L2Player player)
        {
            if (Closed == 1)
                Closed = 0;
            else
                Closed = 1;

            broadcastUserInfo();
        }

        public override int GetDamage()
        {
            int dmg = 6 - (int)Math.Ceiling(CurHP / MaxHP * 6);
            if (dmg > 6)
                return 6;

            if (dmg < 0)
                return 0;

            return dmg;
        }

        private System.Timers.Timer selfClose;
        public void OpenForTime()
        {
            Closed = 0;
            broadcastUserInfo();

            if (selfClose == null)
            {
                selfClose = new System.Timers.Timer();
                selfClose.Interval = 60000;
                selfClose.Elapsed += new System.Timers.ElapsedEventHandler(SelfClose);
            }

            selfClose.Enabled = true;
        }

        private void SelfClose(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Closed == 0)
            {
                Closed = 1;
                broadcastUserInfo();
            }

            selfClose.Enabled = false;
        }

        public override string asString()
        {
            return "L2Door:" + ObjID + " " + StaticID + " " + ClanID;
        }
    }
}
