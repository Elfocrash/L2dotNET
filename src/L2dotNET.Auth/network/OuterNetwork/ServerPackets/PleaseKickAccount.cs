using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.OuterNetwork
{
    internal static class PleaseKickAccount
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0xA8;

        internal static Packet ToPacket(string account)
        {
            Packet p = new Packet(Opcode);
            p.WriteString(account.ToLower());
            return p;
        }
    }
}