
using L2dotNET.Logging.Abstraction;
using L2dotNET.Models.Player;

namespace L2dotNET.Models.Items
{
    public class ItemEffect
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();
        public int[] Ids;

        public void Use(L2Character character, L2Item item)
        {
            if (character is L2Player)
                UsePlayer((L2Player)character, item);
            else
            {
               
                    Log.Warn($"Unk object {character.Name} tried to use {item.Template.ItemId}");
            }
        }

        public virtual void UsePlayer(L2Player player, L2Item item)
        {
            player.SendMessageAsync("You cannot use this item.");
        }
        
    }
}