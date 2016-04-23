using L2dotNET.Game.model.npcs.decor;

namespace L2dotNET.Game.network.l2send
{
    class StaticObject : GameServerNetworkPacket
    {
        private L2StaticObject obj;
        public StaticObject(L2StaticObject obj)
        {
            this.obj = obj;
        }

        protected internal override void write()
        {
            writeC(0x99);
            writeD(obj.StaticID);
            writeD(obj.ObjID);
        }
    }
}
