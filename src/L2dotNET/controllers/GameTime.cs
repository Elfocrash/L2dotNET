using System;
using System.Runtime.Remoting.Contexts;
using log4net;
using L2dotNET.Models.player;
using L2dotNET.Network;
using L2dotNET.Network.serverpackets;
using L2dotNET.world;

namespace L2dotNET.Controllers
{
    [Synchronization]
    public class GameTime
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GameTime));

        private static volatile GameTime _instance;
        private static readonly object SyncRoot = new object();

        public static GameTime Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new GameTime();
                }

                return _instance;
            }
        }

        public int Time;
        private readonly GameserverPacket _dayPk = new SunRise();
        private readonly GameserverPacket _nightPk = new SunSet();
        private System.Timers.Timer _timeController;
        public DateTime ServerStartUp;
        public static bool Night;

        private const int SecDay = 10800,
                          SecNight = 3600,
                          SecHour = 600,
                          SecDn = 14400;
        private const int SecScale = 1800;

        public void Initialize()
        {
            ServerStartUp = DateTime.Now;
            Time = 5800 + 0; // 10800 18:00 вечер
            _timeController = new System.Timers.Timer
            {
                Interval = 1000,
                Enabled = true
            };
            _timeController.Elapsed += ActionTime;

            Log.Info("GameTime Controller: started 18:00 PM.");
        }

        private void ActionTime(object sender, System.Timers.ElapsedEventArgs e)
        {
            Time++;

            switch (Time)
            {
                case SecDay + SecScale: // 21:00
                    NotifyStartNight();
                    break;
                case SecScale: // 03:00
                    NotifyStartDay();
                    break;
            }

            if (Time == SecDn)
                Time = 0;
        }

        private void NotifyStartDay()
        {
            Night = false;
            L2World.Instance.GetPlayers().ForEach(p => p.NotifyDayChange(_dayPk));
        }

        private void NotifyStartNight()
        {
            Night = true;
            L2World.Instance.GetPlayers().ForEach(p => p.NotifyDayChange(_nightPk));
        }

        public void EnterWorld(L2Player p)
        {
            p.NotifyDayChange(Night ? _nightPk : _dayPk);
        }

        public void ShowInfo(L2Player player)
        {
            DateTime dt = new DateTime(2000, 1, 1, 0, 0, 0).AddSeconds(Time * 6);

            SystemMessage sm = new SystemMessage(Night ? SystemMessage.SystemMessageId.TimeS1S2InTheNight : SystemMessage.SystemMessageId.TimeS1S2InTheDay);
            sm.AddString(dt.ToString("hh"));
            sm.AddString(dt.ToString("mm:ss"));
            player.SendPacket(sm);
        }
    }
}