using L2dotNET.Game.model.quests;

namespace L2dotNET.Game
{
    public class QuestInfo
    {
        public bool completed = false;
        public QuestOrigin _template;
        public int id;
        public int stage;

        public QuestInfo(QuestOrigin quest)
        {
            id = quest.questId;
            stage = 1;
            _template = quest;
            completed = false;
        }

        public QuestInfo(int _id, int _stage, int fin)
        {
            id = _id;
            stage = _stage;
            completed = fin == 1;

            if (fin == 0)
            {
                _template = QuestManager.getInstance()._quests[id];
            }
        }
    }
}
