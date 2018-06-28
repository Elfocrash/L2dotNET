using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace L2dotNET.DataContracts
{
    [Table("Accounts")]
    public class AccountContract
    {
        [Key]
        public int AccountId { get; set; }

        public string Login { get; set; }

        public byte[] Password { get; set; }

        public DateTime LastActive { get; set; }

        public int AccessLevel { get; set; }

        public byte LastServer { get; set; }

        public AccountContract()
        {
            LastActive = DateTime.Now;
            AccessLevel = 0;
            LastServer = 1;
        }
    }
}