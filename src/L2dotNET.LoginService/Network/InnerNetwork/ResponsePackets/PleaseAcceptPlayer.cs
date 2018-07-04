using System;
using L2dotNET.DataContracts;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ResponsePackets
{
    static class PleaseAcceptPlayer
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0xA7;

        internal static Packet ToPacket(AccountContract account, SessionKey key)
        {
            Packet p = new Packet(Opcode);
            p.WriteInt(account.AccountId);
            p.WriteInt(key.LoginOkId1);
            p.WriteInt(key.LoginOkId2);
            p.WriteInt(key.PlayOkId1);
            p.WriteInt(key.PlayOkId2);
            return p;
        }
    }
}