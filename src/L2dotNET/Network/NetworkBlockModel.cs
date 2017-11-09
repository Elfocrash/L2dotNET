using System;

namespace L2dotNET.Network
{
    public class NetworkBlockModel
    {
        public string DirectIp { get; set; }

        public string Mask { get; set; }

        public bool Permanent { get; set; }

        public DateTime TimeEnd { get; set; }
    }
}