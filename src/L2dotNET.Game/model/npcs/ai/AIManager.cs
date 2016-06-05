using System.Collections.Generic;

namespace L2dotNET.GameService.Model.npcs.ai
{
    public class AIManager
    {
        private static volatile AIManager instance;
        private static readonly object syncRoot = new object();

        public static AIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new AIManager();
                        }
                    }
                }

                return instance;
            }
        }

        public void Initialize()
        {
            register(new broadcasting_tower());
        }

        public SortedList<int, AITemplate> _registeredAis = new SortedList<int, AITemplate>();

        public AIManager() { }

        private void register(AITemplate t)
        {
            _registeredAis.Add(t.id, t);
        }

        public AITemplate CheckChatWindow(int id)
        {
            if (_registeredAis.ContainsKey(id))
            {
                AITemplate t = _registeredAis[id];
                if (t.chatOvr)
                    return t;
                else
                    return null;
            }

            return null;
        }

        public AITemplate CheckDialogResult(int id)
        {
            if (_registeredAis.ContainsKey(id))
            {
                AITemplate t = _registeredAis[id];
                if (t.dialogOvr)
                    return t;
                else
                    return null;
            }

            return null;
        }
    }
}