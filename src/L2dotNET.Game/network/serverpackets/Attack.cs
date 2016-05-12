using L2dotNET.GameService.world;

namespace L2dotNET.GameService.network.l2send
{
    class Attack : GameServerNetworkPacket
    {
        protected int _attackerObjId;
        public bool soulshot;
        public int _grade;
        private int _x, tx;
        private int _y, ty;
        private int _z, tz;
        private Hit[] _hits;

        public Attack(L2Character player, L2Object target, bool ss, int grade)
        {
            _attackerObjId = player.ObjID;
            soulshot = ss;
            _grade = grade;
            _x = player.X;
            _y = player.Y;
            _z = player.Z;
            tx = target.X;
            ty = target.Y;
            tz = target.Z;
            _hits = new Hit[0];
        }

        public void addHit(int targetId, int damage, bool miss, bool crit, bool shld)
        {
            int pos = _hits.Length;
            Hit[] tmp = new Hit[pos + 1];

            for (int i = 0; i < _hits.Length; i++)
                tmp[i] = _hits[i];

            tmp[pos] = new Hit(targetId, damage, miss, crit, shld, soulshot, _grade);
            _hits = tmp;
        }

        public bool hasHits()
        {
            return _hits.Length > 0;
        }

        protected internal override void write()
        {
            writeC(0x05);

            writeD(_attackerObjId);
            writeD(_hits[0]._targetId);
            writeD(_hits[0]._damage);
            writeC(_hits[0]._flags);
            writeD(_x);
            writeD(_y);
            writeD(_z);
            writeH((short)(_hits.Length - 1));
            for (int i = 1; i < _hits.Length; i++)
            {
                writeD(_hits[i]._targetId);
                writeD(_hits[i]._damage);
                writeC(_hits[i]._flags);
            }
            //writeD(tx);
            //writeD(ty);
            //writeD(tz);
        }
    }

    public class Hit
    {
        public int _targetId;
        public int _damage;
        public int _flags;

        public Hit(int targetId, int damage, bool miss, bool crit, bool shld, bool soulshot, int _grade)
        {
            _targetId = targetId;
            _damage = damage;
            if (soulshot)
                _flags |= 0x10 | _grade;

            if (crit)
                _flags |= 0x20;
            if (shld)
                _flags |= 0x40;
            if (miss)
                _flags |= 0x80;
        }
    }
}
