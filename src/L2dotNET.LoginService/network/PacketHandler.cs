using System.Threading;
using log4net;
using L2dotNET.LoginService.Network.InnerNetwork.ClientPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network
{
    public class PacketHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PacketHandler));

        public static void Handle(Packet packet, LoginClient client)
        {
            PacketBase incPacket = null;
            switch (packet.FirstOpcode)
            {
                case 0x00:
                    incPacket = new RequestAuthLogin(packet, client);
                    break;
                case 0x02:
                    incPacket = new RequestServerLogin(packet, client);
                    break;
                case 0x05:
                    incPacket = new RequestServerList(packet, client);
                    break;
                case 0x07:
                    incPacket = new AuthGameGuard(packet, client);
                    break;

                default:
                    Log.Warn($"LoginClient: received unk request {packet.FirstOpcode}");
                    break;
            }

            if (incPacket != null)
                new Thread(incPacket.RunImpl).Start();
        }
    }
}