using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using L2dotNET.Utility;

namespace L2dotNET.LoginService.Managers
{
    sealed class NetworkBlock
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NetworkBlock));

        private static volatile NetworkBlock _instance;
        private static readonly object SyncRoot = new object();

        public static NetworkBlock Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new NetworkBlock();
                        }
                    }
                }

                return _instance;
            }
        }

        private readonly List<NbInterface> _blocks = new List<NbInterface>();

        public NetworkBlock()
        {
            using (StreamReader reader = new StreamReader(new FileInfo(@"sq\blocks.txt").FullName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine() ?? string.Empty;
                    if (line.Length == 0)
                    {
                        continue;
                    }

                    if (line.StartsWithIgnoreCase("//"))
                    {
                        continue;
                    }

                    if (line.StartsWithIgnoreCase("d"))
                    {
                        NbInterface i = new NbInterface();
                        i.DirectIp = line.Split(' ')[1];
                        i.Forever = line.Split(' ')[2].EqualsIgnoreCase("0");
                        _blocks.Add(i);
                    }
                    else if (line.StartsWithIgnoreCase("m"))
                    {
                        NbInterface i = new NbInterface();
                        i.Mask = line.Split(' ')[1];
                        i.Forever = line.Split(' ')[2].EqualsIgnoreCase("0");
                        _blocks.Add(i);
                    }
                }
            }

            Log.Info($"NetworkBlock: {_blocks.Count} blocks.");
        }

        public bool Allowed(string ip)
        {
            if (_blocks.Count == 0)
            {
                return true;
            }

            foreach (NbInterface nbi in _blocks)
            {
                if (nbi.DirectIp != null)
                {
                    if (nbi.DirectIp.Equals(ip))
                    {
                        if (nbi.Forever)
                        {
                            return false;
                        }

                        if (nbi.TimeEnd.CompareTo(DateTime.Now) == 1)
                        {
                            return false;
                        }
                    }
                }

                if (nbi.Mask != null)
                {
                    string[] a = ip.Split('.'),
                             b = nbi.Mask.Split('.');
                    bool[] d = new bool[4];
                    for (byte c = 0; c < 4; c++)
                    {
                        d[c] = false;

                        if (b[c] == "*")
                        {
                            d[c] = true;
                        }
                        else if (b[c] == a[c])
                        {
                            d[c] = true;
                        }
                        else if (b[c].Contains("/"))
                        {
                            byte n = byte.Parse(b[c].Split('/')[0]),
                                 x = byte.Parse(b[c].Split('/')[1]);
                            byte t = byte.Parse(a[c]);
                            d[c] = (t >= n) && (t <= x);
                        }
                    }

                    byte cnt = (byte)d.Count(u => u);

                    if (cnt >= 4)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}