using L2dotNET.Models;

namespace L2dotNET.Network.serverpackets
{
    class Attack : GameserverPacket
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
            AttackerObjId = player.ObjectId;
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

        public override void Write()
        {
            WriteByte(0x05);
            WriteInt(AttackerObjId);
            WriteInt(_hits[0].TargetId);
            WriteInt(_hits[0].Damage);
            WriteByte(_hits[0].Flags);
            WriteInt(_x);
            WriteInt(_y);
            WriteInt(_z);
            WriteShort((short)(_hits.Length - 1));
            for (int i = 1; i < _hits.Length; i++)
            {
                WriteInt(_hits[i].TargetId);
                WriteInt(_hits[i].Damage);
                WriteByte(_hits[i].Flags);
            }
        }
    }
}