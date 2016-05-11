using L2dotNET.Game.network.loginauth;
using L2dotNET.Game.network.loginauth.recv;
using log4net;
using System;
using System.Threading;

namespace L2dotNET.Game.network
{
    public class PacketHandlerAuth
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PacketHandlerAuth));

        public static void handlePacket(AuthThread login, byte[] buff)
        {
            byte id = 0;
            try
            {
               id = buff[0];
            }
            catch(IndexOutOfRangeException e)
            {
                log.Error($"Something went wrong on PacketHandlerAuth: { e }");
                goto SkipLabel;
            }

            log.Info($"login>gs: { id }");
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
                log.Info(cninfo);
                return;
            }

            new Thread(new ThreadStart(msg.run)).Start();
        SkipLabel:;
        }
    }
}
