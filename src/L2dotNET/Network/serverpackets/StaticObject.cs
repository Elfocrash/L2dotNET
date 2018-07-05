using L2dotNET.Models.Npcs.Decor;

namespace L2dotNET.Network.serverpackets
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
            WriteInt(_obj.CharacterId);
        }
    }
}