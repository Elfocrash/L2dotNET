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
            ReasonSystemError = 0x01,
            ReasonUserOrPassWrong = 0x02,
            Reason3 = 0x03,
            Reason4 = 0x04,
            ReasonTooManyPlayers = 0x0f
        }

        internal static Packet ToPacket(LoginClient client, PlayFailReason reason)
        {
            Packet p = new Packet(Opcode);
            p.WriteInt((byte)reason);
            return p;
        }
    }
}