using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using L2dotNET.LoginService.Network;
using L2dotNET.Utility;

namespace L2dotNET.LoginService.Managers
{
    class NetworkRedirect
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NetworkRedirect));
        private static volatile NetworkRedirect _instance;
        private static readonly object SyncRoot = new object();

        protected List<NetRedClass> Redirects = new List<NetRedClass>();
        public NetRedClass GlobalRedirection { get; set; }

        public static NetworkRedirect Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new NetworkRedirect();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            using (StreamReader reader = new StreamReader(new FileInfo(@"sq\server_redirect.txt").FullName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine() ?? string.Empty;
                    if ((line.Length == 0) || line.StartsWithIgnoreCase("//"))
                        continue;

                    NetRedClass i = new NetRedClass();
                    string[] sp = line.Split(' ');
                    i.ServerId = short.Parse(sp[0]);
                    i.Mask = sp[1];
                    i.SetRedirect(sp[2]);

                    if (i.ServerId == -1)
                        GlobalRedirection = i;
                    else
                        Redirects.Add(i);
                }
            }

            Log.Info($"NetworkRedirect: {Redirects.Count} redirects. Global is {(GlobalRedirection == null ? "disabled" : "enabled")}");
        }

        public byte[] GetRedirect(LoginClient client, short serverId)
        {
            if (GlobalRedirection != null)
            {
                string[] a = client.Address.ToString().Split(':')[0].Split('.'),
                         b = GlobalRedirection.Mask.Split('.');
                byte[] d = new byte[4];
                for (byte c = 0; c < 4; c++)
                {
                    d[c] = 0;

                    if (b[c] == "*")
                        d[c] = 1;
                    else
                    {
                        if (b[c] == a[c])
                            d[c] = 1;
                        else
                        {
                            if (!b[c].Contains("/"))
                                continue;

                            byte n = byte.Parse(b[c].Split('/')[0]),
                                 x = byte.Parse(b[c].Split('/')[1]);
                            byte t = byte.Parse(a[c]);
                            d[c] = (t >= n) && (t <= x) ? (byte)1 : (byte)0;
                        }
                    }
                }

                if (d.Min() != 1)
                    return null;

                Log.Info($"Redirecting client to global {GlobalRedirection.Redirect} on #{serverId}");
                return GlobalRedirection.RedirectBits;
            }

            if (Redirects.Count == 0)
                return null;

            foreach (NetRedClass nr in Redirects.Where(nr => nr.ServerId == serverId))
            {
                string[] a = client.Address.ToString().Split(':')[0].Split('.'),
                         b = nr.Mask.Split('.');
                byte[] d = new byte[4];
                for (byte c = 0; c < 4; c++)
                {
                    d[c] = 0;

                    if (b[c] == "*")
                        d[c] = 1;
                    else
                    {
                        if (b[c] == a[c])
                            d[c] = 1;
                        else
                        {
                            if (!b[c].Contains("/"))
                                continue;

                            byte n = byte.Parse(b[c].Split('/')[0]),
                                 x = byte.Parse(b[c].Split('/')[1]);
                            byte t = byte.Parse(a[c]);
                            d[c] = (t >= n) && (t <= x) ? (byte)1 : (byte)0;
                        }
                    }
                }

                if (d.Min() != 1)
                    continue;

                Log.Info($"Redirecting client to {nr.Redirect} on #{serverId}");
                return nr.RedirectBits;
            }

            return null;
        }
    }
}