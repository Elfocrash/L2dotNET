using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace L2dotNET.Auth.managers
{
    class NetworkRedirect
    {
        private static NetworkRedirect nb = new NetworkRedirect();
        public static NetworkRedirect getInstance()
        {
            return nb;
        }

        protected List<NetRedClass> redirects = new List<NetRedClass>();
        public NetRedClass GlobalRedirection = null;

        public NetworkRedirect()
        {
            StreamReader reader = new StreamReader(new FileInfo(@"sq\server_redirect.txt").FullName);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.Length == 0 || line.StartsWith("//"))
                    continue;

                NetRedClass i = new NetRedClass();
                string[] sp = line.Split(' ');
                i.serverId = short.Parse(sp[0]);
                i.mask = sp[1];
                i.setRedirect(sp[2]);

                if (i.serverId == -1)
                    GlobalRedirection = i;
                else
                    redirects.Add(i);
            }

            CLogger.info("NetworkRedirect: " + redirects.Count + " redirects. Global is " + (GlobalRedirection == null ? "disabled":"enabled"));
        }

        public byte[] GetRedirect(LoginClient client, short serverId)
        {
            if (GlobalRedirection != null)
            {
                string[] a = client._address.ToString().Split(':')[0].Split('.'), b = GlobalRedirection.mask.Split('.');
                byte[] d = new byte[4];
                for (byte c = 0; c < 4; c++)
                {
                    d[c] = 0;

                    if (b[c] == "*")
                        d[c] = 1;
                    else if (b[c] == a[c])
                        d[c] = 1;
                    else if (b[c].Contains("/"))
                    {
                        byte n = byte.Parse(b[c].Split('/')[0]), x = byte.Parse(b[c].Split('/')[1]);
                        byte t = byte.Parse(a[c]);
                        d[c] = (t >= n && t <= x) ? (byte)1 : (byte)0;
                    }
                }

                if (d.Min() == 1)
                {
                    CLogger.info("Redirecting client to global " + GlobalRedirection.redirect + " on #" + serverId);
                    return GlobalRedirection.redirectBits;
                }
            }
            else
            {
                if (redirects.Count == 0)
                    return null;

                foreach (NetRedClass nr in redirects)
                {
                    if (nr.serverId == serverId)
                    {
                        string[] a = client._address.ToString().Split(':')[0].Split('.'), b = nr.mask.Split('.');
                        byte[] d = new byte[4];
                        for (byte c = 0; c < 4; c++)
                        {
                            d[c] = 0;

                            if (b[c] == "*")
                                d[c] = 1;
                            else if (b[c] == a[c])
                                d[c] = 1;
                            else if (b[c].Contains("/"))
                            {
                                byte n = byte.Parse(b[c].Split('/')[0]), x = byte.Parse(b[c].Split('/')[1]);
                                byte t = byte.Parse(a[c]);
                                d[c] = (t >= n && t <= x) ? (byte)1 : (byte)0;
                            }
                        }

                        if (d.Min() == 1)
                        {
                            CLogger.info("Redirecting client to " + nr.redirect + " on #" + serverId);
                            return nr.redirectBits;
                        }
                    }
                }
            }

            return null;
        }
    }
}
