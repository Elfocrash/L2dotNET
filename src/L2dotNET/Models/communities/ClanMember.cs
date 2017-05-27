using L2dotNET.model.player;

namespace L2dotNET.model.communities
{
    public class ClanMember
    {
        public string Name;
        public byte Level;
        public byte ClassId;
        public int ObjId;
        public int SponsorId;
        public short ClanType;
        public string NickName,
                      PledgeTypeName;
        public int ClanPrivs;
        public string OwnerName = string.Empty;
        public int Gender;
        public int Race;
        public int Online;
        public L2Player Target;

        internal int HaveMaster()
        {
            return 0;
        }

        public int OnlineId()
        {
            return Online == 0 ? 0 : ObjId;
        }
    }
}