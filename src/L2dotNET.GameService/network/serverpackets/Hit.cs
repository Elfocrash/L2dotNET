namespace L2dotNET.GameService.Network.Serverpackets
{
    public class Hit
    {
        public int TargetId;
        public int Damage;
        public int Flags;

        public Hit(int targetId, int damage, bool miss, bool crit, bool shld, bool soulshot, int grade)
        {
            TargetId = targetId;
            Damage = damage;
            if (soulshot)
                Flags |= 0x10 | grade;

            if (crit)
                Flags |= 0x20;
            if (shld)
                Flags |= 0x40;
            if (miss)
                Flags |= 0x80;
        }
    }
}