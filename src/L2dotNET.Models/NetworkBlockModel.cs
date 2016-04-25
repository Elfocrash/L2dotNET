using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Models
{
    public class NetworkBlockModel
    {
        public string DirectIp { get; set; }

        public string Mask { get; set; }

        public bool Permanent { get; set; }

        public DateTime TimeEnd { get; set; }
    }
}
