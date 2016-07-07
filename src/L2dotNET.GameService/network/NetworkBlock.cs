using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using L2dotNET.Models;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Network
{
    class NetworkBlock
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

        protected List<NetworkBlockModel> Blocks = new List<NetworkBlockModel>();

        public void Initialize()
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
                        NetworkBlockModel nbModel = new NetworkBlockModel
                                                    {
                                                        DirectIp = line.Split(' ')[1],
                                                        Permanent = line.Split(' ')[2].EqualsIgnoreCase("0")
                                                    };
                        Blocks.Add(nbModel);
                    }
                    else if (line.StartsWithIgnoreCase("m"))
                    {
                        NetworkBlockModel nbModel = new NetworkBlockModel
                                                    {
                                                        Mask = line.Split(' ')[1],
                                                        Permanent = line.Split(' ')[2].EqualsIgnoreCase("0")
                                                    };
                        Blocks.Add(nbModel);
                    }
                }
            }

            Log.Info($"NetworkBlock: {Blocks.Count} blocks.");
        }

        public bool Allowed(string ip)
        {
            if (Blocks.Count == 0)
            {
                return true;
            }

            foreach (NetworkBlockModel nbi in Blocks)
            {
                if (nbi.DirectIp != null)
                {
                    if (nbi.DirectIp.Equals(ip))
                    {
                        if (nbi.Permanent)
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
                    for (int c = 0; c < 4; c++)
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
                            short n = short.Parse(b[c].Split('/')[0]),
                                  x = short.Parse(b[c].Split('/')[1]);
                            short t = short.Parse(a[c]);
                            d[c] = (t >= n) && (t <= x);
                        }
                    }

                    if (d.Any(u => u))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}