using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Managers.bbs
{
    public class BbsManager
    {
        private readonly Config.Config _config;

        public BbsManager(Config.Config config)
        {
            _config = config;
        }

        public async Task RequestShow(L2Player player, int type)
        {
            if (_config.GameplayConfig.Server.CommunityBoard.EnableCommunityBoard)
                player.ShowHtmBbs("<html><body><br><br><center>Welcome to the community board</center><br><br></body></html>");
            else
                await player.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.CbOffline));
        }
    }
}