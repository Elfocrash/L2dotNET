using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mapster;

namespace L2dotNET.DataContracts
{
    [Table("Servers")]
    public class ServerContract
    {
        [Key]
        public int ServerId { get; set; }

        public string Name { get; set; }

        public string Wan { get; set; }

        public int Port { get; set; }

        [AdaptMember("ServerKey")]
        public string Key { get; set; }
    }
}