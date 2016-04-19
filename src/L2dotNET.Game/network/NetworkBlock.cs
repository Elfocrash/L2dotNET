using System;
using System.Collections.Generic;
using System.IO;
using L2dotNET.Game.logger;

namespace L2dotNET.Game.network
{
    class NetworkBlock
    {
        private static NetworkBlock nb = new NetworkBlock();
        public static NetworkBlock getInstance()
        {
            return nb;
        }

        protected List<NB_interface> blocks = new List<NB_interface>();

        public NetworkBlock()
        {
            StreamReader reader = new StreamReader(new FileInfo(@"sq\blocks.txt").FullName);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.Length == 0)
                    continue;

                if (line.StartsWith("//"))
                    continue;

                if (line.StartsWith("d"))
                {
                    NB_interface i = new NB_interface();
                    i.directIp = line.Split(' ')[1];
                    i.forever = line.Split(' ')[2].Equals("0");
                    blocks.Add(i);
                }
                else if (line.StartsWith("m"))
                {
                    NB_interface i = new NB_interface();
                    i.mask = line.Split(' ')[1];
                    i.forever = line.Split(' ')[2].Equals("0");
                    blocks.Add(i);
                }
            }

            CLogger.info("NetworkBlock: " + blocks.Count+" blocks.");
        }

        public bool allowed(string ip)
        {
            if (blocks.Count == 0)
                return true;

            foreach (NB_interface nbi in blocks)
            {
                if(nbi.directIp != null)
                    if (nbi.directIp.Equals(ip))
                    {
                        if (nbi.forever)
                            return false;
                        else
                        {
                            if (nbi.timeEnd.CompareTo(DateTime.Now) == 1)
                            {
                                return false;
                            }
                        }
                    }

                if (nbi.mask != null)
                {
                    string[] a = ip.Split('.'), b = nbi.mask.Split('.');
                    bool[] d = new bool[4];
                    for (int c = 0; c < 4; c++)
                    {
                        d[c] = false;

                        if (b[c] == "*")
                            d[c] = true;
                        else if (b[c] == a[c])
                            d[c] = true;
                        else if (b[c].Contains("/"))
                        {
                            short n = short.Parse(b[c].Split('/')[0]), x = short.Parse(b[c].Split('/')[1]);
                            short t = short.Parse(a[c]);
                            d[c] = t >= n && t <= x;
                        }
                    }

                    foreach (bool u in d)
                        if (u)
                            return false;
                }
            }

            return true;
        }
    }

    public class NB_interface
    {
        public string directIp = null;
        public string mask = null;
        public bool forever = false;
        public DateTime timeEnd;
    }
}
