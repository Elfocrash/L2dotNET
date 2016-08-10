using L2dotNET.model.player;

namespace L2dotNET.Handlers
{
    public interface IChatHandler
    {
        void HandleChat(int type, L2Player player, string target, string text);
    }
}