using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Scripting;

namespace L2dotNET.GameService.Model.Quests
{
    public class QuestManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(QuestManager));
        private static volatile QuestManager instance;
        private static readonly object syncRoot = new object();

        public static QuestManager Instance
        {
            get
            {
                if (instance == null)
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new QuestManager();
                    }

                return instance;
            }
        }

        public void Initialize()
        {
            object[] items = ScriptCompiler.Instance.CompileFolder(@"cmpl\quests");

            if (items == null)
                return;

            foreach (object obj in items)
                register((QuestOrigin)obj);

            //register(new _0003_will_the_seal_be_broken());

            log.Info($"QuestManager: loaded {_quests.Count} quests.");
        }

        public readonly SortedList<int, QuestOrigin> _quests = new SortedList<int, QuestOrigin>();

        public QuestManager() { }

        private void register(QuestOrigin qo)
        {
            _quests.Add(qo.questId, qo);
        }

        public void QuestAccept(L2Player player, L2Npc npc, int questId)
        {
            QuestOrigin qo = _quests[questId];
            qo.onAccept(player, npc);
        }

        public void TalkSelection(L2Player player, L2Npc npc)
        {
            List<object[]> qlist = new List<object[]>();
            List<int> ilist = new List<int>();
            List<int> clist = new List<int>();
            bool nullex = false;
            foreach (QuestInfo qi in player._quests)
            {
                if (qi.completed)
                {
                    clist.Add(qi.id);
                    continue;
                }

                if (qi._template.canTalk(player, npc))
                {
                    qlist.Add(new object[] { qi._template, "<a action=\"bypass -h quest_continue?quest_id=" + qi.id + "\">[" + qi._template.questName + " (In Progress)]</a><br1>", qi._template.questId });
                    ilist.Add(qi.id);
                }
            }

            foreach (QuestOrigin qo in _quests.Values)
            {
                if (ilist.Contains(qo.questId))
                    continue;

                if (qo.startNpc == npc.Template.NpcId)
                {
                    if (clist.Contains(qo.questId))
                    {
                        qlist.Add(new object[] { null, "[" + qo.questName + " (Completed)]<br1>", 0 });
                        nullex = true;
                        continue;
                    }

                    qlist.Add(new object[] { qo, "<a action=\"bypass -h quest_tryaccept?quest_id=" + qo.questId + "\">[" + qo.questName + "]</a><br1>", qo.questId });
                }
            }

            if (!nullex && (qlist.Count == 1))
            {
                foreach (object[] o in qlist)
                    if (((string)o[1]).Contains("(In Progress)"))
                        player.quest_Talk(npc, ((QuestOrigin)o[0]).questId);
                    else
                        ((QuestOrigin)o[0]).tryAccept(player, npc);

                return;
            }

            if (qlist.Count == 0)
            {
                player.ShowHtmPlain("No quests for you now. Talk to others", npc);
                return;
            }

            StringBuilder sb = new StringBuilder();

            foreach (object[] ur in qlist)
                sb.Append((string)ur[1]);

            player.ShowHtmPlain(sb.ToString(), npc);

            qlist.Clear();
            ilist.Clear();
            clist.Clear();
        }

        public void OnQuestTalk(L2Player player, L2Npc npc, int ask, int reply)
        {
            foreach (QuestInfo qo in player._quests.Where(qo => qo.id == ask))
            {
                qo._template.onTalkToNpcQM(player, npc, reply);
                break;
            }
        }

        public void Quest_continue(L2Player player, L2Npc npc, int qid)
        {
            player.quest_Talk(npc, qid);
        }

        public void Quest_tryaccept(L2Player player, L2Npc npc, int qid)
        {
            QuestOrigin qo = _quests[qid];
            qo.tryAccept(player, npc);
        }
    }
}