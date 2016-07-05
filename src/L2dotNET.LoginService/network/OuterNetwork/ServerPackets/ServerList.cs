using System.Collections.Generic;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Model;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.OuterNetwork.ServerPackets
{
    /// <summary>
    /// Play accepted packet.
    /// </summary>
    internal static class ServerList
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0x04;

        /// <summary>
        /// Returns play accepted server > client packet.
        /// </summary>
        /// <param name="client"><see cref="LoginClient"/> object.</param>
        /// <returns>Play accepted <see cref="Packet"/>.</returns>
        internal static Packet ToPacket(LoginClient client)
        {
            List<L2Server> servers = ServerThreadPool.Instance.Servers;
            Packet p = new Packet(Opcode);
            p.WriteByte((byte)servers.Count, (byte)client.ActiveAccount.LastServer);
            foreach (L2Server server in servers)
            {
                p.WriteByte(server.Id);
                p.WriteBytesArray(server.GetIp(client));
                p.WriteInt(server.Port);
                p.WriteByte(0);
                p.WriteByte(1); // pvp?
                p.WriteShort(server.CurrentPlayers);
                p.WriteShort(server.MaxPlayers);

                p.WriteByte(server.Connected); // status

                int bits = 0x40;
                if (server.TestMode)
                {
                    bits |= 0x04;
                }

                p.WriteInt(bits);
                p.WriteByte(0); //brackets
            }

            return p;
        }
    }
}