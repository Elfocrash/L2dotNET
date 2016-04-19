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
            writeC(0x9f);
            writeD(obj.StaticID);
            writeD(obj.ObjID);
            writeD(obj.Type);
            writeD(obj.CanBeSelected());
            writeD(obj.MeshID);
            writeD(obj.Closed);
            writeD(obj.Enemy());
            writeD(obj.CurHP);
            writeD(obj.MaxHP);
            writeD(obj.ShowHP());
            writeD(obj.GetDamage());
        }
    }
}
