
namespace L2dotNET.Game.world
{
    public class InstanceTemplate
    {
        public int ClientId;
        public string ClientName;
        public virtual bool startFailed(L2Player player) { return false; }
        public virtual bool joinFailed(L2Player player) { return false; }
        public virtual bool closeFailed(L2Player player) { return false; }

        public int reuseH;
        public int reuseM;
        public int ActionTime;

        public int maxInside = -1;
        public int minParty = -1;

        public int x0, y0, z0; //вне инстанса
        public int x1, y1, z1; //внутри. первый спавн
        public bool ReuseActive = false;
        public bool WholeZoneIsPvp = false;
        
    }
}
