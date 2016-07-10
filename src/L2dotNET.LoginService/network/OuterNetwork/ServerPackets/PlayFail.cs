using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.OuterNetwork.ServerPackets
{
    static class PlayFail
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0x06;

        internal static Packet ToPacket(LoginClient client, PlayFailReason reason)
        {
            Packet p = new Packet(Opcode);
            p.WriteInt((byte)reason);
            return p;
        }
    }
}