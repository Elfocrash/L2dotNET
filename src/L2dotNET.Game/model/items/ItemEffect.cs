using log4net;
using L2dotNET.GameService.model.playable;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.model.items
{
    public class ItemEffect
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ItemEffect));
        public int[] ids;
        public void Use(L2Character character, L2Item item)
        {
            if (character is L2Player)
                UsePlayer((L2Player)character, item);
            else if (character is L2Pet)
                UsePet((L2Pet)character, item);
            else
                log.Warn($"Unk object { character.Name } tried to use { item.Template.ItemID }");
        }

        public virtual void UsePlayer(L2Player player, L2Item item) 
        {
            player.sendMessage("You cannot use this item.");
        }

        public virtual void UsePet(L2Pet pet, L2Item item) 
        {
            pet.Owner.sendSystemMessage(SystemMessage.SystemMessageId.PET_CANNOT_USE_ITEM);
        }
    }
}
