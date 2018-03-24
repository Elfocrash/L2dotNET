using L2dotNET.DataContracts;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.OuterNetwork.ServerPackets
{
    static class PleaseAcceptPlayer
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0xA7;

        internal static Packet ToPacket(AccountContract account, string time)
        {
            Packet p = new Packet(Opcode);
            //writeC(0xA7);
            //writeS(account..ToLower());
            //writeS(account.type.ToString());
            //writeS(account.timeend);
            //writeC(account.premium ? 1 : 0);
            //writeQ(account.points);
            //writeS(time);
            return p;
        }
    }
}