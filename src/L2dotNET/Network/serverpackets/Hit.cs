namespace L2dotNET.Network.serverpackets
{
    public class Hit
    {
        private const int HITFLAG_USESS = 0x10;
        private const int HITFLAG_CRITICAL = 0x20;
        private const int HITFLAG_SHIELD = 0x40;
        private const int HITFLAG_MISS = 0x80;

        public int TargetId { get; }
        public int Damage { get; }
        public int Flags { get; }

        public bool IsMiss { get; }
        public bool IsCritical { get; }
        public bool IsSoulshotUsed { get; }
        public int SoulshotGrade { get; }
        public int Shield { get; }

        public Hit(int targetId, int damage, bool isMiss, bool isCritical, int shield, bool isSoulshotUsed, int soulshotGrade)
        {
            TargetId = targetId;
            Damage = damage;
            IsMiss = isMiss;
            IsCritical = isCritical;
            Shield = shield;
            IsSoulshotUsed = isSoulshotUsed;
            SoulshotGrade = soulshotGrade;

            if (isMiss)
            {
                Flags = HITFLAG_MISS;
                return;
            }

            if (isSoulshotUsed)
            {
                Flags |= HITFLAG_USESS | soulshotGrade;
            }

            if (isCritical)
            {
                Flags |= HITFLAG_CRITICAL;
            }

            if (shield > 0)
            {
                Flags |= HITFLAG_SHIELD;
            }
        }
    }
}