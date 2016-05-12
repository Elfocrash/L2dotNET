using System.Collections.Generic;
using System.Text;
using L2dotNET.GameService.model.npcs;
using L2dotNET.GameService.model.quests.data;
using L2dotNET.GameService.scripting;
using log4net;

namespace L2dotNET.GameService.model.quests
{
    public class QuestManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(QuestManager));
        private static QuestManager qm = new QuestManager();

        public static QuestManager getInstance()
        {
            return qm;
        }

        public readonly SortedList<int, QuestOrigin> _quests = new SortedList<int, QuestOrigin>();

        public QuestManager()
        {
            object[] items = ScriptCompiler.Instance.CompileFolder(@"cmpl\quests");

            if (items == null)
                return;

            foreach (object obj in items)
            {
                register((QuestOrigin)obj);
            }

            register(new _0003_will_the_seal_be_broken());
            register(new _0005_miners_favor());
            register(new _0006_step_into_the_future());
            register(new _0007_a_trip_begins());
            register(new _0008_an_adventure_begins());
            register(new _0009_into_the_city_of_humans());
            register(new _0010_into_the_world());
            register(new _0011_secret_meeting_with_ketra_orcs());
            register(new _0246_PossessorOfAPreciousSoul());
            register(new _0605_alliance_with_ketra_orcs());
            register(new _0606_war_with_varka_silenos());

            log.Info($"QuestManager: loaded { _quests.Count } quests.");
        }

        private void register(QuestOrigin qo)
        {
            _quests.Add(qo.questId, qo);
        }

        public void questAccept(L2Player player, L2Citizen npc, int questId)
        {
            QuestOrigin qo = _quests[questId];
            qo.onAccept(player, npc);
        }

        public void talkSelection(L2Player player, L2Citizen npc)
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

            if (!nullex && qlist.Count == 1)
            {
                foreach (object[] o in qlist)
                {
                    if (((string)o[1]).Contains("(In Progress)"))
                        player.quest_Talk(npc, ((QuestOrigin)o[0]).questId);
                    else
                        ((QuestOrigin)o[0]).tryAccept(player, npc);
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

        public void onQuestTalk(L2Player player, L2Citizen npc, int ask, int reply)
        {
            foreach (QuestInfo qo in player._quests)
            {
                if (qo.id == ask)
                {
                    qo._template.onTalkToNpcQM(player, npc, reply);
                    break;
                }
            }
        }

        public void quest_continue(L2Player player, L2Citizen npc, int qid)
        {
            player.quest_Talk(npc, qid);
        }

        public void quest_tryaccept(L2Player player, L2Citizen npc, int qid)
        {
            QuestOrigin qo = _quests[qid];
            qo.tryAccept(player, npc);
        }
    }
}
