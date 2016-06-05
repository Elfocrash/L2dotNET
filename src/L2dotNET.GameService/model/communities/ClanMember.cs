using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Communities
{
    public class ClanMember
    {
        public string Name;
        public byte Level;
        public byte classId;
        public int ObjID;
        public int sponsorId;
        public short ClanType;
        public string NickName,
                      _pledgeTypeName;
        public int ClanPrivs;
        public string _ownerName = "";
        public int Gender;
        public int Race;
        public int online;
        public L2Player Target;

        internal int haveMaster()
        {
            return 0;
        }

        public int OnlineID()
        {
            if (online == 0)
                return 0;

            return ObjID;
        }
    }
}