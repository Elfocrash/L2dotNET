using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class Attack : GameServerNetworkPacket
    {
        protected int _attackerObjId;
        public bool soulshot;
        public int _grade;
        private readonly int _x;
        private int tx;
        private readonly int _y;
        private int ty;
        private readonly int _z;
        private int tz;
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
}