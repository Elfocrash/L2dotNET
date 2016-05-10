using System.Collections.Generic;

namespace L2dotNET.Game.model.npcs.ai
{
    public class AIManager
    {
        private static volatile AIManager instance;
        private static object syncRoot = new object();

        public static AIManager Instance
        {
            get
            {
                if(instance == null)
                {
                    lock(syncRoot)
                    {
                        if(instance == null)
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

        private void register(AITemplate t)
        {
            _registeredAis.Add(t.id, t);
        }

        public AITemplate checkChatWindow(int id)
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

        public AITemplate checkDialogResult(int id)
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
