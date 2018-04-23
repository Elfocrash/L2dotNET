using System.Collections.Generic;
using System.Linq;
using log4net;
using L2dotNET.DataContracts;
using L2dotNET.Models.Player;
using L2dotNET.Models.Player.Basic;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using L2dotNET.World;

namespace L2dotNET.Managers
{
    public class AnnouncementManager
    {
        private readonly IServerService _serverService;

        private static readonly ILog Log = LogManager.GetLogger(typeof(AnnouncementManager));

        public List<AnnouncementContract> Announcements { get; set; }

        public AnnouncementManager(IServerService serverService)
        {
            _serverService = serverService;
        }

        public void Initialize()
        {
            Announcements = _serverService.GetAnnouncementsList();
            Log.Info($"Loaded {Announcements.Count} annoucements.");
        }

        public void Announce(string text)
        {
            CreatureSay cs = new CreatureSay(SayIDList.CHAT_ANNOUNCE, text);
            L2World.Instance.GetPlayers().ForEach(p => p.SendPacket(cs));
        }

        public void CriticalAnnounce(string text)
        {
            CreatureSay cs = new CreatureSay(SayIDList.CHAT_CRITICAL_ANNOUNCE, text);
            L2World.Instance.GetPlayers().ForEach(p => p.SendPacket(cs));
        }

        public void ScreenAnnounce(string text)
        {
            CreatureSay cs = new CreatureSay(SayIDList.CHAT_SCREEN_ANNOUNCE, text);
            L2World.Instance.GetPlayers().ForEach(p => p.SendPacket(cs));
        }

        public void OnEnter(L2Player player)
        {
            if ((Announcements == null) || (Announcements.Count == 0))
                return;

            CreatureSay cs = new CreatureSay(SayIDList.CHAT_ANNOUNCE);

            foreach (AnnouncementContract announcement in Announcements.Where(announcement => announcement.Type == 0))
            {
                cs.Text = announcement.Text;
                player.SendPacket(cs);
            }
        }
    }
}