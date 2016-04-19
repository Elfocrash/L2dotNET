using System;
using System.Threading;
using L2dotNET.Game.network.loginauth;
using L2dotNET.Game.network.loginauth.recv;

namespace L2dotNET.Game.network
{
    public class PacketHandlerAuth
    {
        public static void handlePacket(AuthThread login, byte[] buff)
        {
            byte id = buff[0];
            Console.WriteLine("login>gs: "+id);
            string cninfo = "handlepacket: request " + id.ToString("x2") + " size " + buff.Length;

            ReceiveAuthPacket msg = null;
            switch (id)
            {
                case 0xA1:
                    msg = new LoginServPingResponse(login, buff);
                    break;
                case 0xA5:
                    msg = new LoginServLoginFail(login, buff);
                    break;
                case 0xA6:
                    msg = new LoginServLoginOk(login, buff);
                    break;
                case 0xA7:
                    msg = new LoginServAcceptPlayer(login, buff);
                    break;
                case 0xA8:
                    msg = new LoginServKickAccount(login, buff);
                    break;
            }

            if (msg == null)
            {
                Console.WriteLine(cninfo);
                return;
            }

            //if (!login.IsConnected)
            //    return;

            new Thread(new ThreadStart(msg.run)).Start();
        }
    }
}
