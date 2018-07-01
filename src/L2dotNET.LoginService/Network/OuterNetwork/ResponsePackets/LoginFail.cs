using L2dotNET.LoginService.Network.Enums;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.OuterNetwork.ResponsePackets
{
    /// <summary>
    /// Login failed packet.
    /// </summary>
    static class LoginFail
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