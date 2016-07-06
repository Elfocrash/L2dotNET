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
        private static readonly ILog Log = LogManager.GetLogger(typeof(QuestManager));
        private static volatile QuestManager _instance;
        private static readonly object SyncRoot = new object();

        public static QuestManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new QuestManager();
                        }
                    }
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            object[] items = ScriptCompiler.Instance.CompileFolder(@"cmpl\quests");

            if (items == null)
            {
                return;
            }

            foreach (object obj in items)
            {
                Register((QuestOrigin)obj);
            }

            //register(new _0003_will_the_seal_be_broken());

            Log.Info($"QuestManager: loaded {Quests.Count} quests.");
        }

        public readonly SortedList<int, QuestOrigin> Quests = new SortedList<int, QuestOrigin>();

        private void Register(QuestOrigin qo)
        {
            Quests.Add(qo.QuestId, qo);
        }

        public void QuestAccept(L2Player player, L2Npc npc, int questId)
        {
            QuestOrigin qo = Quests[questId];
            qo.OnAccept(player, npc);
        }

        public void TalkSelection(L2Player player, L2Npc npc)
        {
            List<object[]> qlist = new List<object[]>();
            List<int> ilist = new List<int>();
            List<int> clist = new List<int>();
            bool nullex = false;
            foreach (QuestInfo qi in player.Quests)
            {
                if (qi.Completed)
                {
                    clist.Add(qi.Id);
                    continue;
                }

                if (qi.Template.CanTalk(player, npc))
                {
                    qlist.Add(new object[] { qi.Template, "<a action=\"bypass -h quest_continue?quest_id=" + qi.Id + "\">[" + qi.Template.QuestName + " (In Progress)]</a><br1>", qi.Template.QuestId });
                    ilist.Add(qi.Id);
                }
            }

            foreach (QuestOrigin qo in Quests.Values.Where(qo => !ilist.Contains(qo.QuestId)).Where(qo => qo.StartNpc == npc.Template.NpcId))
            {
                if (clist.Contains(qo.QuestId))
                {
                    qlist.Add(new object[] { null, "[" + qo.QuestName + " (Completed)]<br1>", 0 });
                    nullex = true;
                    continue;
                }

                qlist.Add(new object[] { qo, "<a action=\"bypass -h quest_tryaccept?quest_id=" + qo.QuestId + "\">[" + qo.QuestName + "]</a><br1>", qo.QuestId });
            }

            if (!nullex && (qlist.Count == 1))
            {
                foreach (object[] o in qlist)
                    if (((string)o[1]).Contains("(In Progress)"))
                    {
                        player.quest_Talk(npc, ((QuestOrigin)o[0]).QuestId);
                    }
                    else
                    {
                        ((QuestOrigin)o[0]).TryAccept(player, npc);
                    }

                return;
            }

            if (qlist.Count == 0)
            {
                player.ShowHtmPlain("No quests for you now. Talk to others", npc);
                return;
            }

            StringBuilder sb = new StringBuilder();

            foreach (object[] ur in qlist)
            {
                sb.Append((string)ur[1]);
            }

            player.ShowHtmPlain(sb.ToString(), npc);

            qlist.Clear();
            ilist.Clear();
            clist.Clear();
        }

        public void OnQuestTalk(L2Player player, L2Npc npc, int ask, int reply)
        {
            foreach (QuestInfo qo in player.Quests.Where(qo => qo.Id == ask))
            {
                qo.Template.OnTalkToNpcQm(player, npc, reply);
                break;
            }
        }

        public void Quest_continue(L2Player player, L2Npc npc, int qid)
        {
            player.quest_Talk(npc, qid);
        }

        public void Quest_tryaccept(L2Player player, L2Npc npc, int qid)
        {
            QuestOrigin qo = Quests[qid];
            qo.TryAccept(player, npc);
        }
    }
}