using System.Collections.Generic;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.AI.Template
{
    public class Ai
    {
        public L2Npc Myself;

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

        public Dictionary<string, string> Dialog;

        public string GetDialog(string fn)
        {
            if (Dialog == null)
            {
                return "dialog.null";
            }

            return Dialog.ContainsKey(fn) ? Dialog[fn] : "dialog.not.found";
        }

        public const int Adena = 57;

        public const string SummonNpcGroup = "@summon_npc_group";

        public void AddAttackDesire(L2Character target, int rate) { }
    }
}