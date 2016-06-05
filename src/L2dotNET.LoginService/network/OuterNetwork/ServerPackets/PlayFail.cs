using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.OuterNetwork.ServerPackets
{
    internal static class PlayFail
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0x06;

        public enum PlayFailReason
        {
            REASON_SYSTEM_ERROR = 0x01,
            REASON_USER_OR_PASS_WRONG = 0x02,
            REASON3 = 0x03,
            REASON4 = 0x04,
            REASON_TOO_MANY_PLAYERS = 0x0f
        }

        internal static Packet ToPacket(LoginClient client, PlayFailReason reason)
        {
            Packet p = new Packet(Opcode);
            p.WriteInt((byte)reason);
            return p;
        }
    }
}