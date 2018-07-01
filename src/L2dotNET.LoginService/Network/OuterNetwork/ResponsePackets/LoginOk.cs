using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.OuterNetwork.ResponsePackets
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
            p.WriteInt(client.Key.LoginOkId1, client.Key.LoginOkId2);
            p.WriteIntArray(new int[2]);
            p.WriteInt(0x000003ea);
            p.WriteIntArray(new int[3]);
            p.WriteByteArray(new byte[16]);
            return p;
        }
    }
}