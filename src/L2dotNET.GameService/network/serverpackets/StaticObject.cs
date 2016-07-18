using L2dotNET.GameService.Model.Npcs.Decor;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class StaticObject : GameserverPacket
    {
        private readonly L2StaticObject _obj;

        public StaticObject(L2StaticObject obj)
        {
            _obj = obj;
        }

        public override void Write()
        {
            WriteByte(0x99);
            WriteInt(_obj.StaticId);
            WriteInt(_obj.ObjId);
        }
    }
}