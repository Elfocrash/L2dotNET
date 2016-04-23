using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Models
{
    public class ServerModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Wan { get; set; }

        public int Port { get; set; }

        public string Code { get; set; }

        public ServerModel()
        {

        }

    }
}
