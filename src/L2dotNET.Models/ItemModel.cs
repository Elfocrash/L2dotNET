namespace L2dotNET.Models
{
    public class ItemModel
    {
        public int OwnerId { get; set; }

        public int ObjectId { get; set; }

        public int ItemId { get; set; }

        public int Count { get; set; }

        public int Enchant { get; set; }

        public string Location { get; set; }

        public int LocationData { get; set; }

        public int? TimeOfUse { get; set; }

        public int CustomType1 { get; set; }

        public int CustomType2 { get; set; }

        public int ManaLeft { get; set; }

        public int Time { get; set; }

        public bool ExistsInDb { get; set; }

        public bool StoredInDb { get; set; }
    }
}