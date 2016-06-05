using System.Collections.Generic;

namespace L2dotNET.GameService.Model.Items
{
    public class CapsuleItem
    {
        public int id;
        public List<CapsuleItemReward> rewards = new List<CapsuleItemReward>();
    }
}