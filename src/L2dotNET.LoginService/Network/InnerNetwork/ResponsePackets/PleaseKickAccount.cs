using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ResponsePackets
{
    static class PleaseKickAccount
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0xA8;

        internal static Packet ToPacket(int accountId)
        {
            Packet p = new Packet(Opcode);
            p.WriteInt(accountId);
            return p;
        }
    }
}