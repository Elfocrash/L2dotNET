using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ResponsePackets
{
    static class LoginServPing
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0xA1;

        internal static Packet ToPacket(int key)
        {
            Packet p = new Packet(Opcode);
            p.WriteInt(key);
            return p;
        }
    }
}