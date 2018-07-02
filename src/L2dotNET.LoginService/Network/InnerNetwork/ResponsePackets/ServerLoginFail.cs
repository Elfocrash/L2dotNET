using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ResponsePackets
{
    static class ServerLoginFail
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0xA5;

        internal static Packet ToPacket(string code)
        {
            Packet p = new Packet(Opcode);
            p.WriteString(code);
            return p;
        }
    }
}