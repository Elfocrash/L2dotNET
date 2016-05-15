using L2dotNET.LoginService.gscommunication;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.OuterNetwork
{
    internal static class LoginServPing
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0xA1;
        internal static Packet ToPacket()
        {
            Packet p = new Packet(Opcode);
            p.WriteString("LoginServPing: PING!");
            return p;
        }
    }
}
