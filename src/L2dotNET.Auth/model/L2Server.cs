using L2dotNET.Auth.managers;

namespace L2dotNET.Auth.gscommunication
{
    public class L2Server
    {
        public string code;
        public byte[] DefaultAddress;
        public ServerThread thread;
        public string info;
        public byte id;

        public byte[] GetIP(LoginClient client)
        {
            if (DefaultAddress == null)
            {
                string ip = "0.0.0.0";
                if (thread != null)
                    ip = thread.wan;

                DefaultAddress = new byte[4];
                string[] w = ip.Split('.');
                DefaultAddress[0] = byte.Parse(w[0]);
                DefaultAddress[1] = byte.Parse(w[1]);
                DefaultAddress[2] = byte.Parse(w[2]);
                DefaultAddress[3] = byte.Parse(w[3]);
            }

            if (thread != null)
            {
                byte[] redirect = NetworkRedirect.getInstance().GetRedirect(client, id);
                if (redirect != null)
                    return redirect;
            }

            return DefaultAddress;
        }

        public byte connected
        {
            get { return thread != null ? (thread.connected ? (byte)1 : (byte)0) : (byte)0; } 
        }

        public short CurPlayers
        {
            get { return thread != null ? thread.curp : (short)0; } 
        }

        public short MaxPlayers
        {
            get { return thread != null ? thread.maxp : (short)0; } 
        }

        public int Port
        {
            get { return thread != null ? thread.port : 0; } 
        }

        public bool testMode
        {
            get { return thread != null ? thread.testMode : false; } 
        }

        public bool gmonly 
        {
            get { return thread != null ? thread.gmonly :false; } 
        }
    }
}
