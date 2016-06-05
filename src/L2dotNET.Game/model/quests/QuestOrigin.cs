using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Quests
{
    public class QuestOrigin
    {
        public int questId;
        public int startNpc;
        public string questName;
        public int[] talkNpcs,
                     actItems;
        public bool repeatable = false;

        public string no_action_required = "You are either not on a quest that involves this NPC, or you don't meet this NPC's minimum quest requirements.";

        public virtual void onHit(L2Player player, L2Character target, int stage) { }

        public virtual void onCast(L2Player player, TSkill skill, int stage) { }

        public virtual void onDie(L2Player player, L2Character killer, int stage) { }

        public virtual void onTalkToNpc(L2Player player, L2Npc npc, int stage) { }

        public virtual void onKill(L2Player player, L2Warrior mob, int stage) { }

        public virtual void onAccept(L2Player player, L2Npc npc) { }

        public virtual void tryAccept(L2Player player, L2Npc npc) { }

        public virtual bool canTalk(L2Player player, L2Npc npc)
        {
            foreach (int id in talkNpcs)
            {
                if (npc.Template.NpcId == id)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual void onTalkToNpcQM(L2Player player, L2Npc npc, int reply) { }

        public virtual void onEarnItem(L2Player player, int stage, int id) { }
    }
}