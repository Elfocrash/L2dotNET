using System.Runtime.Remoting.Contexts;
using System.Threading;
using log4net;
using L2dotNET.GameService.Network.LoginAuth;
using L2dotNET.GameService.Network.LoginAuth.Recv;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network
{
    [Synchronization]
    public class PacketHandlerAuth
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PacketHandlerAuth));

        public static void HandlePacket(Packet packet, AuthThread login)
        {
            PacketBase packetBase = null;
            switch (packet.FirstOpcode)
            {
                case 0xA1:
                    packetBase = new LoginServPingResponse(packet, login);
                    break;
                case 0xA5:
                    packetBase = new LoginServLoginFail(packet, login);
                    break;
                case 0xA6:
                    packetBase = new LoginServLoginOk(packet, login);
                    break;
                case 0xA7:
                    packetBase = new LoginServAcceptPlayer(packet, login);
                    break;
                case 0xA8:
                    packetBase = new LoginServKickAccount(packet, login);
                    break;
            }

            if (packetBase != null)
                new Thread(packetBase.RunImpl).Start();
        }
    }
}