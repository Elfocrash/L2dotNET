using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Utility;

namespace L2dotNET.LoginService.Managers
{
    sealed class NetworkBlock
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private static volatile NetworkBlock _instance;
        private static readonly object SyncRoot = new object();

        public static NetworkBlock Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new NetworkBlock();
                }

                return _instance;
            }
        }

        private readonly List<NBInterface> _blocks = new List<NBInterface>();

        public NetworkBlock()
        {
            using (StreamReader reader = new StreamReader(new FileInfo(@"sq\blocks.txt").FullName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine() ?? string.Empty;
                    if (line.Length == 0)
                        continue;

                    if (line.StartsWithIgnoreCase("//"))
                        continue;

                    if (line.StartsWithIgnoreCase("d"))
                    {
                        NBInterface i = new NBInterface
                        {
                            DirectIp = line.Split(' ')[1],
                            Forever = line.Split(' ')[2].EqualsIgnoreCase("0")
                        };
                        _blocks.Add(i);
                    }
                    else
                    {
                        if (!line.StartsWithIgnoreCase("m"))
                            continue;

                        NBInterface i = new NBInterface
                        {
                            Mask = line.Split(' ')[1],
                            Forever = line.Split(' ')[2].EqualsIgnoreCase("0")
                        };
                        _blocks.Add(i);
                    }
                }
            }

            Log.Info($"{_blocks.Count} network blocks.");
        }

        public bool Allowed(string ip)
        {
            if (_blocks.Count == 0)
                return true;

            foreach (NBInterface nbi in _blocks)
            {
                if (nbi.DirectIp?.Equals(ip) ?? false)
                {
                    if (nbi.Forever)
                        return false;

                    if (nbi.TimeEnd.CompareTo(DateTime.Now) == 1)
                        return false;
                }

                if (nbi.Mask == null)
                    continue;

                string[] a = ip.Split('.'),
                         b = nbi.Mask.Split('.');
                bool[] d = new bool[4];
                for (byte c = 0; c < 4; c++)
                {
                    d[c] = false;

                    if (b[c] == "*")
                        d[c] = true;
                    else
                    {
                        if (b[c] == a[c])
                            d[c] = true;
                        else
                        {
                            if (!b[c].Contains("/"))
                                continue;

                            byte n = byte.Parse(b[c].Split('/')[0]),
                                 x = byte.Parse(b[c].Split('/')[1]);
                            byte t = byte.Parse(a[c]);
                            d[c] = (t >= n) && (t <= x);
                        }
                    }
                }

                byte cnt = (byte)d.Count(u => u);

                if (cnt >= 4)
                    return false;
            }

            return true;
        }
    }
}