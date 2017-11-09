using System.Collections.Generic;
using System.Linq;
using log4net;
using L2dotNET.DataContracts;
using L2dotNET.model.player;
using L2dotNET.model.player.basic;
using L2dotNET.Models;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using L2dotNET.world;
using Ninject;

namespace L2dotNET.managers
{
    class AnnouncementManager
    {
        [Inject]
        public IServerService ServerService => GameServer.Kernel.Get<IServerService>();

        private static readonly ILog Log = LogManager.GetLogger(typeof(AnnouncementManager));

        private static volatile AnnouncementManager _instance;
        private static readonly object SyncRoot = new object();

        public List<AnnouncementContract> Announcements { get; set; }

        public static AnnouncementManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new AnnouncementManager();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            Announcements = ServerService.GetAnnouncementsList();
            Log.Info($"Announcement manager: Loaded {Announcements.Count} annoucements.");
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