using L2dotNET.GameService.Model.Npcs.Decor;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class StaticObject : GameServerNetworkPacket
    {
        private readonly L2StaticObject _obj;

        public StaticObject(L2StaticObject obj)
        {
            _obj = obj;
        }

        protected internal override void Write()
        {
            WriteC(0x99);
            WriteD(_obj.StaticId);
            WriteD(_obj.ObjId);
        }
    }
}