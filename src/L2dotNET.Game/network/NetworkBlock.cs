using System;
using System.Collections.Generic;
using System.IO;
using L2dotNET.Models;
using log4net;

namespace L2dotNET.GameService.network
{
    class NetworkBlock
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NetworkBlock));
        private static volatile NetworkBlock instance;
        private static object syncRoot = new object();

        public static NetworkBlock Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new NetworkBlock();
                        }
                    }
                }

                return instance;
            }
        }

        protected List<NetworkBlockModel> blocks = new List<NetworkBlockModel>();

        public NetworkBlock()
        {

        }

        public void Initialize()
        {
            using (StreamReader reader = new StreamReader(new FileInfo(@"sq\blocks.txt").FullName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.Length == 0)
                        continue;

                    if (line.StartsWith("//"))
                        continue;

                    if (line.StartsWith("d"))
                    {
                        NetworkBlockModel nbModel = new NetworkBlockModel();
                        nbModel.DirectIp = line.Split(' ')[1];
                        nbModel.Permanent = line.Split(' ')[2].Equals("0");
                        blocks.Add(nbModel);
                    }
                    else if (line.StartsWith("m"))
                    {
                        NetworkBlockModel nbModel = new NetworkBlockModel();
                        nbModel.Mask = line.Split(' ')[1];
                        nbModel.Permanent = line.Split(' ')[2].Equals("0");
                        blocks.Add(nbModel);
                    }
                }
            }
            log.Info($"NetworkBlock: { blocks.Count } blocks.");
        }

        public bool Allowed(string ip)
        {
            if (blocks.Count == 0)
                return true;

            foreach (NetworkBlockModel nbi in blocks)
            {
                if (nbi.DirectIp != null)
                    if (nbi.DirectIp.Equals(ip))
                    {
                        if (nbi.Permanent)
                            return false;
                        else
                        {
                            if (nbi.TimeEnd.CompareTo(DateTime.Now) == 1)
                            {
                                return false;
                            }
                        }
                    }

                if (nbi.Mask != null)
                {
                    string[] a = ip.Split('.'), b = nbi.Mask.Split('.');
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
}
