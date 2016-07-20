namespace L2dotNET.Models
{
    public class EtcItemModel
    {
        public int ItemId { get; set; }

        public string Name { get; set; }

        public bool Crystallizable { get; set; }

        public string ItemType { get; set; }

        public int Weight { get; set; }

        public string ConsumeType { get; set; }

        public string CrystalType { get; set; }

        public int Duration { get; set; }

        public int Price { get; set; }

        public int CrystalCount { get; set; }

        public bool Sellable { get; set; }

        public bool Dropable { get; set; }

        public bool Destroyable { get; set; }

        public bool Tradeable { get; set; }

        public string OldName { get; set; }

        public string OldType { get; set; }
    }
}