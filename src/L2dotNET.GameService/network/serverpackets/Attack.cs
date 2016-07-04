using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class Attack : GameServerNetworkPacket
    {
        protected int AttackerObjId;
        public bool Soulshot;
        public int Grade;
        private readonly int _x;
        private int _tx;
        private readonly int _y;
        private int _ty;
        private readonly int _z;
        private int _tz;
        private Hit[] _hits;

        public Attack(L2Character player, L2Object target, bool ss, int grade)
        {
            AttackerObjId = player.ObjId;
            Soulshot = ss;
            Grade = grade;
            _x = player.X;
            _y = player.Y;
            _z = player.Z;
            _tx = target.X;
            _ty = target.Y;
            _tz = target.Z;
            _hits = new Hit[0];
        }

        public void AddHit(int targetId, int damage, bool miss, bool crit, bool shld)
        {
            int pos = _hits.Length;
            Hit[] tmp = new Hit[pos + 1];

            for (int i = 0; i < _hits.Length; i++)
                tmp[i] = _hits[i];

            tmp[pos] = new Hit(targetId, damage, miss, crit, shld, Soulshot, Grade);
            _hits = tmp;
        }

        public bool HasHits()
        {
            return _hits.Length > 0;
        }

        protected internal override void Write()
        {
            WriteC(0x05);

            WriteD(AttackerObjId);
            WriteD(_hits[0].TargetId);
            WriteD(_hits[0].Damage);
            WriteC(_hits[0].Flags);
            WriteD(_x);
            WriteD(_y);
            WriteD(_z);
            WriteH((short)(_hits.Length - 1));
            for (int i = 1; i < _hits.Length; i++)
            {
                WriteD(_hits[i].TargetId);
                WriteD(_hits[i].Damage);
                WriteC(_hits[i].Flags);
            }
            //writeD(tx);
            //writeD(ty);
            //writeD(tz);
        }
    }
}