using System.Collections.Generic;

namespace L2dotNET.GameService.Model.Npcs.Ai
{
    public class AiManager
    {
        private static volatile AiManager _instance;
        private static readonly object SyncRoot = new object();

        public static AiManager Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new AiManager();
                    }
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            Register(new BroadcastingTower());
        }

        public SortedList<int, AiTemplate> RegisteredAis = new SortedList<int, AiTemplate>();

        private void Register(AiTemplate t)
        {
            RegisteredAis.Add(t.Id, t);
        }

        public AiTemplate CheckChatWindow(int id)
        {
            if (!RegisteredAis.ContainsKey(id))
            {
                return null;
            }

            AiTemplate t = RegisteredAis[id];
            return t.ChatOvr ? t : null;
        }

        public AiTemplate CheckDialogResult(int id)
        {
            if (!RegisteredAis.ContainsKey(id))
            {
                return null;
            }

            AiTemplate t = RegisteredAis[id];
            return t.DialogOvr ? t : null;
        }
    }
}