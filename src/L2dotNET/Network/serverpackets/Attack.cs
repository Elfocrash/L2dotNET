using System.Collections.Generic;
using L2dotNET.Models;

namespace L2dotNET.Network.serverpackets
{
    
    class Attack : GameserverPacket
    {
        public List<Hit> Hits { get; set; }

        private readonly int _attackerId;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;
        private readonly bool Soulshot;
        private readonly int Grade;

        public Attack(L2Character player, params Hit[] hits)
        {
            _attackerId = player.ObjectId;
            _x = player.X;
            _y = player.Y;
            _z = player.Z;
            Hits = new List<Hit>(hits);
        }


        public Attack(L2Character player, L2Object target, bool ss, int grade)
        {
            _attackerId = player.ObjectId;
            Soulshot = ss;
            Grade = grade;
            _x = player.X;
            _y = player.Y;
            _z = player.Z;
            Hits = new List<Hit>(0);
        }


        public void AddHit(int targetId, int damage, bool miss, bool crit, bool shld)
        {
            int pos = Hits.Count;
            Hit[] tmp = new Hit[pos + 1];

            for (int i = 0; i < Hits.Count; i++)
                tmp[i] = Hits[i];

            tmp[pos] = new Hit(targetId, damage, miss, crit, shld, Soulshot, Grade);
            //  Hits = tmp;
        }

        public bool HasHits()
        {
            return Hits.Count > 0;
        }

        public override void Write()
        {
            WriteByte(0x05);
            WriteInt(_attackerId);
            WriteInt(Hits[0].TargetId);
            WriteInt(Hits[0].Damage);
            WriteByte(Hits[0].Flags);
            WriteInt(_x);
            WriteInt(_y);
            WriteInt(_z);
            WriteShort((short)(Hits.Count - 1));
            for (int i = 1; i < Hits.Count; i++)
            {
                WriteInt(Hits[i].TargetId);
                WriteInt(Hits[i].Damage);
                WriteByte(Hits[i].Flags);
            }
        }
    }
}