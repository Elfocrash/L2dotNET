using System.Collections.Generic;
using L2dotNET.GameService.model.npcs;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.ai.template
{
    public class AI
    {
        public L2Citizen myself;

        public virtual void Created() { }

        public virtual void Talked(L2Player talker) { }

        public virtual void Attacked(L2Character attacker, double damage) { }

        public virtual void SeeCreature(L2Character cha) { }

        public virtual void TimerStartEx(int timerId) { }

        public virtual void NothingToDo() { }

        public virtual void MoveToFinished(int x, int y, int z) { }

        public virtual void MoveToWaypointFinished() { }

        public virtual void TalkedReply(L2Player talker, int ask, int reply) { }

        public virtual void TalkedBypass(L2Player talker, string bypass) { }

        public virtual void AttackFinished(L2Character target) { }

        public virtual bool TeleportRequested(L2Player talker)
        {
            return true;
        }

        public Dictionary<string, string> dialog;

        public string GetDialog(string fn)
        {
            if (dialog == null)
                return "dialog.null";
            else
            {
                if (dialog.ContainsKey(fn))
                    return dialog[fn];
                else
                    return "dialog.not.found";
            }
        }

        public const int @adena = 57;

        public const string @summon_npc_group = "@summon_npc_group";

        public void AddAttackDesire(L2Character target, int rate) { }
    }
}