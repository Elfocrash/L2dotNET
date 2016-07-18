using System;
using L2dotNET.GameService.World;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ChangeMoveType : GameserverPacket
    {
        public static readonly int Walk = 0;
        public static readonly int Run = 1;

        private readonly int _charObjId;
        private readonly bool _running;

        public ChangeMoveType(L2Character character)
        {
            _charObjId = character.ObjId;
            _running = Convert.ToBoolean(character.IsRunning);
        }

        public override void Write()
        {
            WriteByte(0x2e);
            WriteInt(_charObjId);
            WriteInt(_running ? Run : Walk);
            WriteInt(0); // c2
        }
    }
}