using System.Collections.Generic;

namespace L2dotNET.GameService.Model.Items
{
    public class CapsuleItem
    {
        public int Id;
        public List<CapsuleItemReward> Rewards = new List<CapsuleItemReward>();
    }
}