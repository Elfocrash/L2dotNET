using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.OuterNetwork
{
    internal static class ServerLoginOk
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0xA6;

        internal static Packet ToPacket()
        {
            Packet p = new Packet(Opcode);
            p.WriteString("Gameserver Authenticated");
            return p;
        }
    }
}