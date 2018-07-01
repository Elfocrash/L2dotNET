using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Managers;
using L2dotNET.LoginService.Network;

namespace L2dotNET.LoginService.Model
{
    public class L2Server
    {
        public byte ServerId { get; set; }
        public string Name { get; set; }
        public ServerThread Thread { get; set; }

        public bool Connected => Thread?.Connected ?? false;

        public short CurrentPlayers => Thread?.CurrentPlayers ?? 0;

        public short MaxPlayers => Thread?.MaxPlayers ?? 0;

        public int Port => Thread?.Port ?? 0;

        public bool TestMode => Thread?.TestMode ?? false;

        public bool GmOnly => Thread?.GmOnly ?? false;

        private byte[] DefaultAddress { get; set; }

        public byte[] GetIp(LoginClient client)
        {
            if (DefaultAddress == null)
            {
                string ip = Thread?.Wan ?? "0.0.0.0";

                DefaultAddress = new byte[4];
                string[] w = ip.Split('.');
                DefaultAddress[0] = byte.Parse(w[0]);
                DefaultAddress[1] = byte.Parse(w[1]);
                DefaultAddress[2] = byte.Parse(w[2]);
                DefaultAddress[3] = byte.Parse(w[3]);
            }

            if (Thread == null)
                return DefaultAddress;

            byte[] redirect = NetworkRedirect.Instance.GetRedirect(client, ServerId);
            return redirect ?? DefaultAddress;
        }
    }
}