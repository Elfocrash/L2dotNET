using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.OuterNetwork
{
    public enum LoginFailReason
    {
        REASON_SYSTEM_ERROR = 0x01,
        REASON_PASS_WRONG = 0x02,
        REASON_USER_OR_PASS_WRONG = 0x03,
        REASON_ACCESS_FAILED = 0x04,
        REASON_ACCOUNT_IN_USE = 0x07,
        REASON_SERVER_OVERLOADED = 0x0f,
        REASON_SERVER_MAINTENANCE = 0x10,
        REASON_TEMP_PASS_EXPIRED = 0x11,
        REASON_DUAL_BOX = 0x23
    }

    /// <summary>
    /// Login failed packet.
    /// </summary>
    internal static class LoginFail
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0x01;

        /// <summary>
        /// Login failed server > client packet.
        /// </summary>
        /// <param name="response">Login failed reason.</param>
        /// <returns>Login failed <see cref="Packet"/>.</returns>
        internal static Packet ToPacket(LoginFailReason response)
        {
            Packet p = new Packet(Opcode);
            p.WriteByte((byte)response);
            return p;
        }
    }
}
