using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.OuterNetwork.ServerPackets
{
    static class LoginOk
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0x03;

        internal static Packet ToPacket(LoginClient client)
        {
            Packet p = new Packet(Opcode);
            p.WriteInt(client.Login1, client.Login2);
            p.WriteInt(0x00, 0x00, 0x000003ea, 0x00, 0x00, 0x00);
            p.WriteBytesArray(new byte[16]);
            return p;
        }
    }
}