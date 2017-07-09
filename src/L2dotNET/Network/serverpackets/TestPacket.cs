using System;

namespace L2dotNET.Network.serverpackets
{
    //Temp Test Packet Class, only for test purposes
    class TestPacket : GameserverPacket
    {
        private byte _opCode;
        private int _writeInt;

        public TestPacket(byte opCode, int writeInt)
        {
            _opCode = opCode;
            _writeInt = writeInt;
        }

        public override void Write()
        {
            WriteByte(_opCode);
            WriteInt(_writeInt);
            Console.WriteLine($"Opcode {_opCode} | parameter {_writeInt}");
        }
    }
}