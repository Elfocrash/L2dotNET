namespace L2dotNET.GameService.Network.Serverpackets
{
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