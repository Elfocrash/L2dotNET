using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Managers.BBS
{
    public class BbsManager
    {
        private static volatile BbsManager _instance;
        private static readonly object SyncRoot = new object();

        public static BbsManager Instance
        {
            get
            {
                if (_instance == null)
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new BbsManager();
                    }

                return _instance;
            }
        }

        public void RequestShow(L2Player player, int type)
        {
            if (Config.Config.Instance.GameplayConfig.Server.CommunityBoard.EnableCommunityBoard)
                player.ShowHtmBbs("<html><body><br><br><center>Welcome to the community board</center><br><br></body></html>");
            else
                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.CbOffline));
        }
    }
}