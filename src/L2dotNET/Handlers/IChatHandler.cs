using L2dotNET.Models.Player;

namespace L2dotNET.Handlers
{
    public interface IChatHandler
    {
        void HandleChat(int type, L2Player player, string target, string text);
    }
}