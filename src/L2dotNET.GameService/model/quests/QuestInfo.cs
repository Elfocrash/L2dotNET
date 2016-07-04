namespace L2dotNET.GameService.Model.Quests
{
    public class QuestInfo
    {
        public bool Completed;
        public QuestOrigin Template;
        public int Id;
        public int Stage;

        public QuestInfo(QuestOrigin quest)
        {
            Id = quest.QuestId;
            Stage = 1;
            Template = quest;
            Completed = false;
        }

        public QuestInfo(int id, int stage, int fin)
        {
            Id = id;
            Stage = stage;
            Completed = fin == 1;

            if (fin == 0)
                Template = QuestManager.Instance.Quests[Id];
        }
    }
}