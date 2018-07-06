using System.Collections.Generic;

namespace L2dotNET.Models.Items
{
    public class CapsuleItem
    {
        public int Id { get; }
        public ICollection<CapsuleItemReward> Rewards { get; }

        public CapsuleItem(int id)
        {
            Id = id;
            Rewards = new List<CapsuleItemReward>();
        }
    }
}