using System.Linq;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Quests
{
    public class QuestOrigin
    {
        public int QuestId;
        public int StartNpc;
        public string QuestName;
        public int[] TalkNpcs,
                     ActItems;
        public bool Repeatable = false;

        public string NoActionRequired = "You are either not on a quest that involves this NPC, or you don't meet this NPC's minimum quest requirements.";

        public virtual void OnHit(L2Player player, L2Character target, int stage) { }

        public virtual void OnCast(L2Player player, Skill skill, int stage) { }

        public virtual void OnDie(L2Player player, L2Character killer, int stage) { }

        public virtual void OnTalkToNpc(L2Player player, L2Npc npc, int stage) { }

        public virtual void OnKill(L2Player player, L2Warrior mob, int stage) { }

        public virtual void OnAccept(L2Player player, L2Npc npc) { }

        public virtual void TryAccept(L2Player player, L2Npc npc) { }

        public virtual bool CanTalk(L2Player player, L2Npc npc)
        {
            return TalkNpcs.Any(id => npc.Template.NpcId == id);
        }

        public virtual void OnTalkToNpcQm(L2Player player, L2Npc npc, int reply) { }

        public virtual void OnEarnItem(L2Player player, int stage, int id) { }
    }
}