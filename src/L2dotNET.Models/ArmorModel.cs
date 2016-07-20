namespace L2dotNET.Models
{
    public class ArmorModel
    {
        public int ItemId { get; set; }

        public string Name { get; set; }

        public string BodyPart { get; set; }

        public bool Crystallizable { get; set; }

        public string ArmorType { get; set; }

        public int Weight { get; set; }

        public int AvoidModify { get; set; }

        public int Duration { get; set; }

        public int Pdef { get; set; }

        public int Mdef { get; set; }

        public int MpBonus { get; set; }

        public int Price { get; set; }

        public int CrystalCount { get; set; }

        public bool Sellable { get; set; }

        public bool Dropable { get; set; }

        public bool Destroyable { get; set; }

        public bool Tradeable { get; set; }

        public int ItemSkillId { get; set; }

        public int ItemSkillLvl { get; set; }
    }
}