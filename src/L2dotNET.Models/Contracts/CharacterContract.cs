using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace L2dotNET.DataContracts
{
    [Table("Characters")]
    public class CharacterContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CharacterId { get; set; }

        public int AccountId { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public int MaxHp { get; set; }

        public int CurHp { get; set; }

        public int MaxCp { get; set; }

        public int CurCp { get; set; }

        public int MaxMp { get; set; }

        public int CurMp { get; set; }

        public byte Face { get; set; }

        public byte HairStyle { get; set; }

        public byte HairColor { get; set; }

        public byte Sex { get; set; }

        public int Heading { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public long Exp { get; set; }

        public long ExpBeforeDeath { get; set; }

        public int Sp { get; set; }

        public int Karma { get; set; }

        public int PvpKills { get; set; }

        public int PkKills { get; set; }

        public int Race { get; set; }

        public int ClassId { get; set; }

        public int BaseClass { get; set; }

        public DateTime? DeleteTime { get; set; }

        public int CanCraft { get; set; }

        public string Title { get; set; }

        public int RecHave { get; set; }

        public int RecLeft { get; set; }

        public int AccessLevel { get; set; }

        public int Online { get; set; }

        public int OnlineTime { get; set; }

        public int CharSlot { get; set; }

        public DateTime? LastAccess { get; set; }

        public int PunishLevel { get; set; }

        public int PunishTimer { get; set; }

        public int PowerGrade { get; set; }

        public bool Nobless { get; set; }

        public bool Hero { get; set; }

        public DateTime? LastRecomDate { get; set; }
    }
}