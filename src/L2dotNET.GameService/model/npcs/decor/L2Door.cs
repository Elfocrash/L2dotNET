using System;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Structures;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Model.Npcs.Decor
{
    public sealed class L2Door : L2StaticObject
    {
        public HideoutTemplate Structure;

        public L2Door()
        {
            ObjId = IdFactory.Instance.NextId();
            Type = 1;
            Closed = 1;
            MeshId = 1;
            Level = 1;
        }

        public override void OnSpawn()
        {
            CurHp = MaxHp;
            base.OnSpawn();
        }

        public override void NotifyAction(L2Player player)
        {
            if (Closed == 1)
            {
                Closed = 0;
            }
            else
            {
                Closed = 1;
            }

            BroadcastUserInfo();
        }

        public override int GetDamage()
        {
            int dmg = 6 - (int)Math.Ceiling((CurHp / MaxHp) * 6);
            if (dmg > 6)
            {
                return 6;
            }

            if (dmg < 0)
            {
                return 0;
            }

            return dmg;
        }

        private System.Timers.Timer _selfClose;

        public void OpenForTime()
        {
            Closed = 0;
            BroadcastUserInfo();

            if (_selfClose == null)
            {
                _selfClose = new System.Timers.Timer
                             {
                                 Interval = 60000
                             };
                _selfClose.Elapsed += new System.Timers.ElapsedEventHandler(SelfClose);
            }

            _selfClose.Enabled = true;
        }

        private void SelfClose(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Closed == 0)
            {
                Closed = 1;
                BroadcastUserInfo();
            }

            _selfClose.Enabled = false;
        }

        public override string AsString()
        {
            return "L2Door:" + ObjId + " " + StaticId + " " + ClanID;
        }
    }
}