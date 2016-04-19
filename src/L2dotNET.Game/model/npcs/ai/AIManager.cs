using System.Collections.Generic;

namespace L2dotNET.Game.model.npcs.ai
{
    public class AIManager
    {
        private static AIManager ai = new AIManager();

        public static AIManager getInstance()
        {
            return ai;
        }

        public SortedList<int, AITemplate> _registeredAis = new SortedList<int, AITemplate>();

        public AIManager()
        {
            register(new broadcasting_tower());
        }

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
