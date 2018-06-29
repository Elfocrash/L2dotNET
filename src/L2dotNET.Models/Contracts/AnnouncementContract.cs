using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace L2dotNET.DataContracts
{
    [Table("Announcements")]
    public class AnnouncementContract
    {
        [Key]
        public int AnnouncementId { get; set; }

        public string Text { get; set; }

        public int Type { get; set; }
    }
}