using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ChangeMoveType : GameServerNetworkPacket
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

        protected internal override void Write()
        {
            WriteC(0x2e);
            WriteD(_charObjId);
            WriteD(_running ? Run : Walk);
            WriteD(0); // c2
        }
    }
}
