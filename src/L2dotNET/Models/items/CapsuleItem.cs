using System.Collections.Generic;

namespace L2dotNET.Models.items
{
    public class CapsuleItem
    {
        public int Id;
        public List<CapsuleItemReward> Rewards = new List<CapsuleItemReward>();
    }
}